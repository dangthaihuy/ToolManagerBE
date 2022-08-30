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
    [Route("api/account")]
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
        public ActionResult Register(ApiRegisterModel model)
        {
            try
            {
                if (storeUser.GetList().Any(user => user.Email == model.Email))
                {
                    return Json(new { success = false, message = "Username " + model.Email + " is already taken" });
                }

                var newUser = model.MappingObject<IdentityInformationUser>();

                newUser.PasswordHash = Helpers.Utility.Md5HashingData(model.Password);

                var res = storeUser.Register(newUser);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not register: " + ex.ToString());
            }

            return Ok();

        }




        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult Login(ApiLoginModel model)
        {
            try
            {
                model.Email = model.Email.ToStringNormally();

                var pwd = model.Password.ToStringNormally();
                pwd = Helpers.Utility.Md5HashingData(pwd);


                var user = storeUser.Login(new IdentityInformationUser { Email = model.Email, PasswordHash = pwd });

                if (user != null)
                {
                    var tokenStr = AssignJWTToken(user);


                    return Ok(new { id = user.Id, token = tokenStr });
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
        public ActionResult GetList()
        {
            try
            {
                var data = storeUser.GetList();
                if (data != null)
                {
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getlist user: " + ex.ToString());
            }

            return BadRequest(new { error = new { message = "Not found" } });
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public ActionResult GetById(string id)
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
                    return Ok(data);
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not get currentuser: " + ex.ToString());
            }
            return BadRequest(new { error = new { message = "Not found" } });
        }

        private string AssignJWTToken(IdentityInformationUser user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, AppConfiguration.GetAppsetting("Jwt:Subject")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Fullname", user.Fullname),
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
