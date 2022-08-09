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
using System.Linq;
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

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApiRegisterModel model)
        {
            if (storeUser.GetList().Any(user => user.Email == model.Email))
            {
                return Json(new { success = false, message = "Username " + model.Email + " is already taken" });
            }

            var newUser = model.MappingObject<IdentityUser>();

            newUser.PasswordHash = Helpers.Utility.Md5HashingData(model.Password);

            var res = storeUser.Register(newUser);

            return Json(new { success = true });

        }




        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ApiLoginModel model)
        {
            try
            {
                model.Email = model.Email.ToStringNormally();

                var pwd = model.Password.ToStringNormally();
                pwd = Helpers.Utility.Md5HashingData(pwd);


                var user = storeUser.Login(new IdentityUser { Email = model.Email, PasswordHash = pwd });

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
        [Route("getlist")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var data = storeUser.GetList();
                if(data != null)
                {
                    return Ok(new { success = true, data = data });
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }

            return BadRequest(new { error = new { message = "Not found" } });
        }

        [HttpGet]
        [Route("getuser")]
        public async Task<IActionResult> GetById(string id)
        {
            if(id == null)
            {
                return BadRequest(new { error = new { message = "Not found" } });
            }
            try
            {
                var data = storeUser.GetById(id);
                if(data != null)
                {
                    return Ok(new { success = true, data = data });
                }
            }
            catch(Exception ex)
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
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.GetAppsetting("Jwt:Key")));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                AppConfiguration.GetAppsetting("Jwt:Issuer"),
                AppConfiguration.GetAppsetting("Jwt:Audience"),
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn);

            var tokenInstring = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenInstring;
        }
    }
}
