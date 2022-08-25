using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Manager.DataLayer.Entities;
using Autofac;
using Manager.DataLayer.Stores;
using Manager.SharedLibs;
using Manager.WebApp.Resources;

namespace Manager.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;

        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            LoginViewModel model = new LoginViewModel();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ReturnUrl))
                    model.ReturnUrl = "/";

                if (ModelState.IsValid)
                {
                    //var users = new List<IdentityUser>();
                    //users.Add(new IdentityUser { Id = "10", UserName = "admin", FullName = "Vũ Lương Bằng", PasswordHash = "123456" });

                    //var user = users.Where(x => x.UserName == model.UserName && x.PasswordHash == model.Password).FirstOrDefault();

                    model.UserName = model.UserName.ToStringNormally();

                    var pwd = model.Password.ToStringNormally();
                    pwd = Utility.Md5HashingData(pwd);
                    var storeUser = Startup.IocContainer.Resolve<IStoreUser>();

                    var user = storeUser.Login(new IdentityUser { UserName = model.UserName, PasswordHash = pwd });
                    if (user != null)
                    {
                        if (user.LockoutEnabled)
                        {
                            this.AddNotification("Your account was locked !!!", NotificationType.ERROR);

                            return View(model);
                        }

                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));

                        var principal = new ClaimsPrincipal(identity);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);

                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        this.AddNotification("Invalid Credential", NotificationType.ERROR);

                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            return View(model);
        }

        public IActionResult FakeLogin()
        {
            //var claims = new[] { new Claim(ClaimTypes.Name, "MyUserNameOrID"), new Claim(ClaimTypes.Role, "SomeRoleName") };

            //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(identity));

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "bangkhmt3@gmail.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Vũ Lương Bằng"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);


            return Redirect("~/Home/Index");

            //ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            //return View("Login");
        }

        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Redirect to home page    
            return LocalRedirect("/");
        }
    }
}
