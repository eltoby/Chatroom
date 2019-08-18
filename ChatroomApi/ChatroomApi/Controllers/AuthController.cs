namespace ChatroomApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using ChatroomApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel login)
        {
            if (string.IsNullOrEmpty(login.User))
                return this.BadRequest("Invalid client request");

            if (login.Password != "pass")
                return this.Unauthorized();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:44324",
                audience: "http://localhost:44324",
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, login.User)
                },
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return this.Ok(new { Token = tokenString, User = login.User });
        }
    }
}
