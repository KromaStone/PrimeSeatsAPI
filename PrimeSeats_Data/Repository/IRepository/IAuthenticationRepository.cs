using PrimeSeats_Model.DTO;

namespace PrimeSeats_Data.Repository.IRepository
{
    public interface IAuthenticationRepository
    {
        Response login(LoginDTO loginDTO);
    }
}
