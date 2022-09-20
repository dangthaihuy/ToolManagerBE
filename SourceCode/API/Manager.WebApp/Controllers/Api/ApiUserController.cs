using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Api
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class ApiUserController : Controller
    {
        private readonly IApiStoreUser storeUser;
        private readonly ILogger<ApiUserController> _logger;

        public ApiUserController(ILogger<ApiUserController> logger)
        {
            storeUser = Startup.IocContainer.Resolve<IApiStoreUser>();
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult Register(ApiRegisterModel model)
        {
            try
            {

                if (storeUser.GetByEmail(model.Email).Email != null)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "account101" } });
                }


                var newUser = model.MappingObject<IdentityInformationUser>();

                newUser.PasswordHash = Helpers.Utility.Md5HashingData(model.Password);

                var res = storeUser.Register(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not register: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "success", code = "account001" } });

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
                    dynamic token = ((JsonResult)AssignJWTToken(user)).Value;

                    return Ok(new { user.Id, token.Token, token.RefreshToken });
                }
                else
                {
                    return Ok(new { apiMessage = new { type = "error", code = "account102" } });
                }

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }

        }

        [HttpPost]
        [Route("refresh_token")]
        [AllowAnonymous]
        public ActionResult RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = ((JsonResult)VerifyAndGenerateToken(tokenRequest)).Value;
                    

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not refresh token: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
            return Ok(new { apiMessage = new { type = "error", code = "account103" } });
        }

        [HttpPost]
        [Route("change_password")]
        [AllowAnonymous]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var identityChangePw = model.MappingObject<IdentityInformationUser>();
                identityChangePw.Password = Helpers.Utility.Md5HashingData(model.Password);

                var user = storeUser.GetByPassword(identityChangePw);
                if (user != null)
                {
                    var identity = new IdentityInformationUser();
                    identity.Id = model.Id;
                    identity.PasswordHash = Helpers.Utility.Md5HashingData(model.NewPassword);

                    var res = storeUser.Update(identity);

                    return Ok(new { apiMessage = new { type = "success", code = "account004" } });

                }

                return Ok(new { apiMessage = new { type = "error", code = "account104" } });

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not change password: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

        }

        [HttpPost]
        [Route("forget_password")]
        [AllowAnonymous]
        public ActionResult ForgetPassword(ChangePasswordModel model)
        {

            try
            {
                var user = storeUser.GetByEmail(model.Email);
                if (user == null)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "account105" } });
                }

                dynamic token = ((JsonResult)AssignJWTToken(user)).Value;



                var url = $"{AppConfiguration.GetAppsetting("SystemSetting:ClientHost")}/resetpassword?email={model.Email}&token={token.Token}";

                var emailModel = new EmailModel();
                emailModel.Sender = "dangthaihuy2002@gmail.com";
                emailModel.SenderPwd = "vfdlwiuxvxzqjnjw";
                emailModel.Subject = "huy đẹp trai";
                emailModel.SenderName = "2se";


                emailModel.Receiver = model.Email;
                emailModel.Body = $"Ha ha đồ ngốc bấm vào đây đi: {url}";

                EmailHelpers.SendEmail(emailModel);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not forget password: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "success", code = "account005" } });
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

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }

            return Ok(new { apiMessage = new { type = "error", code = "account106" } });
        }

        [HttpGet]
        [Route("get_currentuser")]
        public ActionResult GetCurrentById(string id)
        {
            if (id == null)
            {
                return BadRequest(new { apiMessage = new { type = "error", message = "account107_1" } });
            }

            try
            {
                var data = storeUser.GetById(id);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get currentuser: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
            return Ok(new { apiMessage = new { type = "error", code = "account107_2" } });
        }

        [HttpGet]
        [Route("get_inforuser")]
        public ActionResult GetInforUser(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { apiMessage = new { type = "error", message = "account108_1" } });
            }

            try
            {
                var data = storeUser.GetInforUser(id);
                if (data != null)
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get currentuser: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
            return Ok(new { apiMessage = new { type = "error", code = "getdata108_2" } });
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update([FromForm] UserModel model)
        {


            try
            {
                var identity = new IdentityInformationUser();
                identity.Id = model.Id;
                identity.Fullname = model.Fullname;
                identity.Phone = model.Phone;

                /*var identity = model.MappingObject<IdentityInformationUser>();*/

                if (model != null)
                {
                    if (model.Avatar != null)
                    {
                        var attachmentFolder = string.Format("Avatars/Users/{0}", identity.Id);

                        var filePath = FileUploadHelper.UploadFile(model.Avatar, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {

                            identity.Avatar = filePath;
                        }
                    }

                    var updateUser = storeUser.Update(identity);

                    return Ok(new { userProfile = updateUser, apiMessage = new { type = "success", code = "account009" } });
                }

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get currentuser: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "error", code = "account109" } });
        }

        [HttpPost]
        [Route("reset_password")]
        [AllowAnonymous]
        public ActionResult ResetPassword(ChangePasswordModel model)
        {
            try
            {
                var user = storeUser.GetByEmail(model.Email);

                if (user == null)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "account110" } });
                }

                var identity = model.MappingObject<IdentityInformationUser>();
                identity.Id = user.Id;
                identity.PasswordHash = Helpers.Utility.Md5HashingData(model.Password);

                var res = storeUser.Update(identity);


            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not reset password: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }




            return Ok(new { apiMessage = new { type = "success", code = "account010" } });
        }





        private JsonResult AssignJWTToken(IdentityInformationUser user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, AppConfiguration.GetAppsetting("Jwt:Subject")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
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
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signIn);

            var tokenInstring = new JwtSecurityTokenHandler().WriteToken(token);

            var identity = user.MappingObject<IdentityInformationUser>();

            identity.RefreshToken = RandomString(35) + Guid.NewGuid();

            var result = storeUser.Update(identity);


            return Json(new { Token = tokenInstring, RefreshToken = identity.RefreshToken });
        }

        private JsonResult VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var _tokenValidationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.GetAppsetting("Jwt:Key"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = AppConfiguration.GetAppsetting("Jwt:Audience"),
                    ValidIssuer = AppConfiguration.GetAppsetting("Jwt:Issuer"),
                };
                // Validation 1 - Validation JWT token format
                _tokenValidationParams.ValidateLifetime = false;
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                _tokenValidationParams.ValidateLifetime = true;



                var user = storeUser.GetByRefreshToken(tokenRequest.RefreshToken);

                if(user.Id == 0)
                {
                    return Json(new { apiMessage = new { type = "error", code = "account103" } });
                }
                


                return AssignJWTToken(user);

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not refresh token: " + ex.ToString());

                return Json(new { code = 500, message = "Server error: Refresh token" });
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }


        //Random string 
        private string RandomString(int length)
        {
            var random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
