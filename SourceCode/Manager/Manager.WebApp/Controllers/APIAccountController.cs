using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIAccountController : Controller
    {
        private readonly IAPIStoreUser storeUser;
        private readonly ILogger<APIAccountController> _logger;

        public APIAccountController(ILogger<APIAccountController> logger)
        {
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            _logger = logger;

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(APILoginViewModel model)
        {
            if (storeUser.GetList().Any(user => user.UserName == model.UserName))
            {
                return Json(new { success = false, message = "Username " + model.UserName + " is already taken" });
            }

            var newUser = model.MappingObject<IdentityUser>();

            newUser.PasswordHash = Utility.Md5HashingData(model.Password);

            var res = storeUser.Register(newUser);

            return Json(new { success = true});

        }




        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(APILoginViewModel model)
        {
            try
            {
                model.UserName = model.UserName.ToStringNormally();

                var pwd = model.Password.ToStringNormally();
                pwd = Utility.Md5HashingData(pwd);


                var user = storeUser.Login(new IdentityUser { UserName = model.UserName, PasswordHash = pwd });

                if(user != null)
                {
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

                    return Json(new { success = true, id = user.Id });
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            return Json(new { success = false });
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Redirect to home page    
            return Json(new { success = true });
        }

    }
}
