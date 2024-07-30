using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PrimeSeats_Data.Data;
using PrimeSeats_Data.Repository.IRepository;
using PrimeSeats_Model;
using PrimeSeats_Model.DTO;
using PrimeSeats_Model.Models;
using System.Linq.Expressions;

namespace PrimeSeats_Data.Repository
{

    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public AdminRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Response AddAdmin(AdminDTO admin)
        {
            try {
                var ifEmailExist = _dbcontext.admins.FirstOrDefault(a => a.Email == admin.Email);
                if (ifEmailExist is not null)
                {
                    var response = new Response()
                    {
                        Message = "Email already exist",
                        Success = false,
                    };
                    return response;
                }
                else
                {
                    var role = Roles.Admin;
                    var adminDTO = new Admin()
                    {
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        Email = admin.Email,
                        Role = role,
                        Password = HashPassword(admin.Password),
                        PhoneNumber = admin.PhoneNumber,
                        Address = admin.Address,
                    };
                    _dbcontext.admins.Add(adminDTO);
                    _dbcontext.SaveChanges();
                    var response = new Response()
                    {
                        Message = "data saved",
                        Success = true,
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                // Create and return a Response object indicating failure
                var response = new Response()
                {
                    Message = $"An error occurred: {ex.Message}"
                };
                return response;

            }
        }

        public Admin getAdminByID(Guid id)
        {
            var Admin = _dbcontext.admins.Find(id);
            var adminDetails = new Admin()
            {
                Id = Admin.Id,
                FirstName = Admin.FirstName,
                LastName = Admin.LastName,
                Email = Admin.Email,
                Role = Admin.Role,
                Password = null,
                PhoneNumber = Admin.PhoneNumber,
                Address = Admin.Address,
            };
            return adminDetails;
        }

        public List<AdminDTO> getAllAdmins()
        {
            var allAdmins = _dbcontext.admins.ToList();
            if (allAdmins.Count == 0) { return null; }
            var admins = allAdmins.Select(field => new AdminDTO
            {
                Id  = field.Id,
                FirstName = field.FirstName,
                LastName = field.LastName,
                Role = field.Role,
                Password = null,
                Email = field.Email,
               PhoneNumber = field.PhoneNumber,
               Address = field.Address, 

            }).ToList();

            return admins;
        }

        public Response UpdateAdmin(Guid id, AdminDTO admin)
        {
            var ifAdminExist = _dbcontext.admins.Find(id);
            if (admin == null)
            {
                var response = new Response()
                {
                    Message = "User Doesn't Exist",
                    Success = false,

                };
                return response;
            }
            else
            {

                var admindto = new Admin
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    Role = Roles.Admin,
                    Password = admin.Password,
                    PhoneNumber = admin.PhoneNumber,
                    Address = admin.Address,
                };
                _dbcontext.Update(admindto);
                _dbcontext.SaveChanges();

                var response = new Response()
                {
                    Message = "Data Updated",
                    Success = true,

                };
                return response;
            }
        }

        public Response DeleteAdmin(Guid id)
        {
            var ifADminExist = _dbcontext.admins.Find(id);
            if(ifADminExist == null)
            {
                var response = new Response()
                {
                    Message = "Admin Doesn't Exist",
                    Success = false,

                };
                return response;
            }
            else {
                //_dbcontext.Remove(id);
                _dbcontext.admins.Remove(ifADminExist);
                _dbcontext.SaveChanges();

                var response = new Response()
                {
                    Message = "Admin Deleted",
                    Success = false,

                };
                return response;
            }
        }


        //encrypt
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            //string hashedPassword = Byc
            return hashedPassword;
        }
    }
}
