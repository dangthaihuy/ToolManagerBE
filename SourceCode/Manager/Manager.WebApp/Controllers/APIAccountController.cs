using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class APIAccountController : Controller
    {
        private readonly IAPIStoreUser storeUser;
        private readonly ILogger<APIAccountController> _logger;

        public APIAccountController(ILogger<APIAccountController> logger)
        {
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            _logger = logger;

        }

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register(APILoginViewModel model)
        //{
        //    if (storeUser.GetList().Any(user => user.UserName == model.UserName))
        //    {
        //        return Json(new { success = false, message = "Username " + model.UserName + " is already taken" });
        //    }

        //    var newUser = model.MappingObject<IdentityUser>();

        //    newUser.PasswordHash = Utility.Md5HashingData(model.Password);

        //    var res = storeUser.Register(newUser);

        //    return Json(new { success = true });

        //}




        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ApiLoginModel model)
        {
            try
            {
                model.UserName = model.UserName.ToStringNormally();

                var pwd = model.Password.ToStringNormally();
                pwd = Manager.WebApp.Helpers.Utility.Md5HashingData(pwd);


                var user = storeUser.Login(new IdentityUser { UserName = model.UserName, PasswordHash = pwd });

                if (user != null)
                {
                    var tokenStr = AssignJWTToken(user);


                    return Ok(new { success = true, id = user.Id, token = tokenStr });
                }

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            return BadRequest(new { error = new { message = "Login fail" } });
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> Detail(string id)
        {
            try
            {
                if(id == "245983405")
                {
                    var user = new IdentityUser
                    {
                        Id = "245983405",
                        UserName = "admin",
                        Email = "bangkhmt3@gmail.com",
                        FullName = "Vũ Lương Bằng"
                    };

                    return Ok(new { success = true, data = user });
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            return BadRequest(new { error = new { message = "Not found" } });
        }

        private string AssignJWTToken(IdentityUser user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, AppConfiguration.GetAppsetting("Jwt:Subject")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("FullName", user.FullName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.GetAppsetting("Jwt:Key")));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                AppConfiguration.GetAppsetting("Jwt:Issuer"),
                AppConfiguration.GetAppsetting("Jwt:Audience"),
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            var tokenInstring = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenInstring;
        }
    }
}
