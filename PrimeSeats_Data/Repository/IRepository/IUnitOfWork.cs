namespace PrimeSeats_Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAdminRepository adminRepository { get; }


        void save();
    }
}
