using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using EsmaeelAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BC = BCrypt.Net.BCrypt;

namespace EsmaeelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly EsmaeelContext _context;
        public TokenController(EsmaeelContext context, IConfiguration config)
        {
            this._context = context;
            this._configuration = config;
        }
        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userdata)
        {
            if (_userdata != null && _userdata.EmailName != null && _userdata.Password != null)
            {
                //تحقق من اميل و باسورد
                //var user = await GetUser(_userdata.EmailName, _userdata.Password);
                //var user = _context.UserInfo.SingleOrDefault(x => x.EmailName == _userdata.EmailName);
                var user = await CheakEmail(_userdata.EmailName);
                //الان تحقق داخل داتا بيس و جلب القييم
                if (user != null ||BC.Verify(_userdata.Password,user.Password))
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id",user.UserId.ToString()),
                        new Claim("FirstName",user.FirstName),
                        new Claim("LastName",user.LastName),
                        new Claim("UserName",user.UserName),
                        new Claim("EmailName",user.EmailName)
                    };
                    //تشفير التوكين
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issure"],_configuration["Jwt:Audience"],claims,expires:DateTime.UtcNow.AddDays(1),signingCredentials:signIn);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                   return BadRequest("Invalid Email and Pasworrd");
                }


            }
            else
            {
                return BadRequest();
            }
            
        }
        private async Task<UserInfo>GetUser(string email,string password)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.EmailName == email && u.Password == password);
        }

        private async Task<UserInfo> CheakEmail(string email)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.EmailName == email);
        }

    }

}
