using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PrimeSeats_Data.Data;
using PrimeSeats_Data.Repository.IRepository;
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
    }

}
