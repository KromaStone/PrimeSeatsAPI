
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrimeSeats_Data.Data;
using PrimeSeats_Data.Repository.IRepository;
using PrimeSeats_Model;
using PrimeSeats_Model.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrimeSeats_Data.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;

        public AuthenticationRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }
        //public Response login(LoginDTO loginDTO)
        //{
        //    var user = dbContext.admins.FirstOrDefault(x => x.Email == loginDTO.Email);

        //    if (user == null)
        //    {
        //        var response = new Response()
        //        {
        //            Message = "Usre Not Found",
        //            Success = false,
        //            Token =""
        //        };
        //        return response;
        //    }

        //    bool isPasswordValid = VerifyPassword(loginDTO.Password, user.Password);

        //    if (isPasswordValid == true)
        //    {
        //        var claims = new[]
        //        {
        //    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    new Claim("Email", user.Email.ToString()),
        //    new Claim("Role", user.Role.ToString()),
        //};

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        //        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        var token = new JwtSecurityToken(
        //            configuration["Jwt:Issuer"],
        //            configuration["Jwt:Audience"],
        //            claims,
        //            //expires: DateTime.UtcNow.AddHours(24),
        //            expires: DateTime.UtcNow.AddMinutes(2),
        //            signingCredentials: signIn);

        //        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        //        var response = new Response()
        //        {
        //            Message =  user.FullName,
        //            Success = true,
        //            Token = tokenValue
        //        };
        //        return response;
        //    }
        //    else
        //    {

        //        //return Ok("Invalid password");
        //        var invalidResponse = new Response()
        //        {
        //            Message = "Invalid Password",
        //            Success = false,
        //            Token = ""
        //        };
        //        return invalidResponse;
        //    }

        //}







        public Response login(LoginDTO loginDTO)
        {
            var user = dbContext.admins.FirstOrDefault(x => x.Email == loginDTO.Email);

            if (user == null)
            {
                return new Response { Message = "User Not Found", Success = false, Token = "" };
            }

            bool isPasswordValid = VerifyPassword(loginDTO.Password, user.Password);

            if (isPasswordValid)
            {
                var accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                dbContext.SaveChanges();

                return new Response { Message = user.FullName, Success = true, Token = accessToken, RefreshToken = refreshToken };
            }

            return new Response { Message = "Invalid Password", Success = false, Token = "" };
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
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }




        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                return false;
            }
            bool passwordMatched = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return passwordMatched;
        }
   
    
    
    
    
    }
}
