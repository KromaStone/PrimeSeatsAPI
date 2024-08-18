using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PrimeSeats_Data.Data;
using PrimeSeats_Data.Repository.IRepository;
using PrimeSeats_Model;
using PrimeSeats_Model.DTO;
using PrimeSeats_Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrimeSeats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IAuthenticationRepository authenticationRepository;
        private readonly IConfiguration configuration;
        public AuthController(ApplicationDbContext dbContext, IAuthenticationRepository authenticationRepository, IConfiguration configuration)
        {
            this.authenticationRepository = authenticationRepository;
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
        var varification = authenticationRepository.login(loginDTO);
            return Ok(varification);
        }

        [HttpPost]
        [Route("AddRole")]
        public IActionResult addRole(AddRole addRole)
        {
           var admin = new AddRole()
           {
               Role = addRole.Role,
           };
            dbContext.SaveChanges();
            dbContext.SaveChanges();
            return Ok();
        }





        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            var user = dbContext.admins.FirstOrDefault(x => x.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return Unauthorized(new { Message = "Invalid refresh token" });
            }

            var newAccessToken = GenerateAccessToken(user);
            //var newRefreshToken = GenerateRefreshToken(user);

            //user.RefreshToken = newRefreshToken;
            //user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Reset expiry for refresh token
            dbContext.SaveChanges();

            //return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            return Ok(new { AccessToken = newAccessToken, RefreshToken = request });
        }



        private string GenerateAccessToken(Admin user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("Email", user.Email),
        new Claim("Role", user.Role),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1), // Set expiry for access token
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

//        private string GenerateRefreshToken(Admin user)
//        {
//            var claims = new[]
//{
//        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
//        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//        new Claim("Email", user.Email),
//        new Claim("Role", user.Role),
//    };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
//            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var token = new JwtSecurityToken(
//                configuration["Jwt:Issuer"],
//                configuration["Jwt:Audience"],
//                claims,
//                expires: DateTime.UtcNow.AddHours(1), // Set expiry for access token
//                signingCredentials: signIn);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }





    }

}
