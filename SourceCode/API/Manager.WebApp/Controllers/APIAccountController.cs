using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private readonly IStoreToken storeToken;
        private readonly ILogger<APIAccountController> _logger;

        public APIAccountController(ILogger<APIAccountController> logger)
        {
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            storeToken = Startup.IocContainer.Resolve<IStoreToken>();
            _logger = logger;            
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult Register([FromForm] ApiRegisterModel model)
        {
            try
            {
                
                if (storeUser.GetList().Any(user => user.Email == model.Email))
                {
                    return Ok(new { apiMessage = new { type = "error", code = "auth002" } });
                }
                

                var newUser = model.MappingObject<IdentityInformationUser>();

                newUser.PasswordHash = Helpers.Utility.Md5HashingData(model.Password);

                var res = storeUser.Register(newUser);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not register: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { success = true, message = "Register success" });

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
                    
                    return Ok(new{ Id= user.Id , Token = token.Token, RefreshToken = token.RefreshToken});
                } 
                else
                {
                    return Ok(new { apiMessage = new { type = "error", code = "auth001" } });
                }

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }

        }

        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        public ActionResult RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = ((JsonResult)VerifyAndGenerateToken(tokenRequest)).Value;
                    if (result == null)
                    {
                        return Ok(new { apiMessage = new { type = "error", code = "auth003" } });
                    }

                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not refresh token: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
            return Ok(new { apiMessage = new { type = "error", code = "auth003" } });
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

            return Ok(new { apiMessage = new { type = "error", code = "getdata001" } });
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public ActionResult GetById(string id)
        {
            if(id == null)
            {
                return BadRequest(new { apiMessage = new { type = "error", message = "getdata001" } });
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

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
            return Ok(new { apiMessage = new { type = "error", code = "getdata001" } });
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update([FromForm] UserModel model)
        {
            

            try
            {
                var identity = new IdentityInformationUser();
                identity.Id = model.Id;

                /*var identity = model.MappingObject<IdentityInformationUser>();*/

                if (model.Avatar.Length > 0)
                {
                    var attachmentFolder = string.Format("Avatars/{0}", identity.Id);

                    var filePath = FileUploadHelper.UploadFile(model.Avatar, attachmentFolder);

                    await Task.FromResult(filePath);

                    if (!string.IsNullOrEmpty(filePath))
                    {

                        identity.Avatar = filePath;
                    }

                    var update = storeUser.Update(identity);

                    return Ok(new { apiMessage = new { type = "success", avatar = filePath } });
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not get currentuser: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "error", code = "user001" } });
        }


        private ActionResult AssignJWTToken(IdentityInformationUser user)
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

            var refreshToken = new IdentityRefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            var result = storeToken.Insert(refreshToken);


            return Json(new {Token = tokenInstring, RefreshToken = refreshToken.Token });
        }

        private ActionResult VerifyAndGenerateToken(TokenRequest tokenRequest)
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


                // Validation 2 - Validate encryption alg

                /*if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }
                // Validation 3 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has not yet expired"
                        }
                    });
                    
                }*/

                // validation 4 - validate existence of the token
                var storedToken = storeToken.GetList().FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

                /*if (storedToken == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token does not exist"
                        }
                    });
                }*/

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been used"
                        }
                    });
                }

                // Validation 6 - validate if revoked
                /*if (storedToken.IsRevorked)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been revoked"
                        }
                    });
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token doesn't match"
                        }
                    });
                }

                // Validation 8 - validate stored token expiry date
                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Refresh token has expired"
                        }
                    });
                }
*/

                // update current token 
                storedToken.IsUsed = true;
                storeToken.UpdateRefToken(storedToken);

                // Generate a new token
                var dbUser = storeUser.GetById(Convert.ToString(storedToken.UserId));

                return AssignJWTToken(dbUser);

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not refresh token: " + ex.ToString());

                return StatusCode(500, new { message = "Server error: Refresh token" });
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
