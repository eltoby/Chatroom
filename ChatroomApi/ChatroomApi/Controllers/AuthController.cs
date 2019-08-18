namespace ChatroomApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using ChatroomApi.Domain;
    using ChatroomApi.Models;
    using ChatroomApi.Service;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IOptions<AppSettings> appSettings;

        public AuthController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.appSettings = appSettings;
        }

        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel login)
        {
            if (string.IsNullOrEmpty(login.User))
                return this.BadRequest("Invalid client request");

            if (!this.userService.IsValidUser(login.User, login.Password))
                return this.Unauthorized();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var baseUrl = this.appSettings.Value.BaseUrl;

            var tokenOptions = new JwtSecurityToken(
                issuer: baseUrl,
                audience: baseUrl,
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, login.User)
                },
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return this.Ok(new { Token = tokenString, login.User });
        }

        [Route("addUser")]
        public IActionResult AddUser([FromBody]LoginModel login)
        {
            var user = new User
            {
                Username = login.User,
                Password = login.Password
            };

            if (this.userService.AddUser(user))
                return this.Login(login);
            else
                return this.BadRequest("Username already exists");
        }
    }
}
