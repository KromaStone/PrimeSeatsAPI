using PrimeSeats_Model;
using PrimeSeats_Model.DTO;

namespace PrimeSeats_Data.Repository.IRepository
{
    public interface IAdminRepository
    {
        List<AdminDTO> getAllAdmins(); 
        Admin getAdminByID(Guid id);
        Response AddAdmin(AdminDTO admin);  
        Response UpdateAdmin(Guid id, AdminDTO admin);
        Response DeleteAdmin(Guid id);

    }
}
