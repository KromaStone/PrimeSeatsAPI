using PrimeSeats_Data.Data;
using PrimeSeats_Model.DTO;
using BCrypt.Net;
using PrimeSeats_Model;
using Microsoft.AspNetCore.Mvc;     
using PrimeSeats_Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace PrimeSeats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAdminRepository adminRepository;

        public AdminController(ApplicationDbContext dbContext , IAdminRepository adminRepository)
        {
            this.dbContext = dbContext;
            this.adminRepository = adminRepository;    
        }

        
        
        [HttpGet]
        public IActionResult getAllAdmins()
        {
            var allAdmins = adminRepository.getAllAdmins();
            if (allAdmins == null) {
                return NotFound("List is Empty");
            }
            return Ok(allAdmins);
        }

       
        
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult getAdminsById(Guid id)
        {
            var Admin = adminRepository.getAdminByID(id);
            if (Admin == null)
            {
                var response = new Response
                {
                    Message = "User Not Found"
                };
            }
            return Ok(Admin);
        }

        
        
        [HttpPost]
        public IActionResult AddAdmin(AdminDTO addAdminDto)
        {
            //if (addAdminDto.Email)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }     
            var AddAdmin = adminRepository.AddAdmin(addAdminDto);
            return Ok(AddAdmin);
        }



        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult updateAdmin(Guid id, AdminDTO updateAdminDTO)
        {
            var admin = adminRepository.UpdateAdmin(id,updateAdminDTO);
            return Ok(admin);
        }



        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult deleteAmin(Guid id)
        {
            var admin = adminRepository.DeleteAdmin(id);
            return Ok("data deleted");
        }


      
        
    }
}
