using Manager.DataLayer.Entities;
using Manager.SharedLibs;
using Manager.WebApp.Models.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class ApiAuthTokenController : Controller
    {
        public ApiAuthTokenController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Post(ApiAccountModel model)
        {
            if (model != null)
            {
                //var user = await GetUser(_userData.Email, _userData.Password);
                var user = new IdentityUser
                {
                    Id = "245983405",
                    UserName = "admin",
                    Email = "bangkhmt3@gmail.com",
                    FullName = "Vũ Lương Bằng"
                };

                if (user != null)
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

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
