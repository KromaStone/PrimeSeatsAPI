using PrimeSeats_Data.Data;
using PrimeSeats_Data.Repository.IRepository;

namespace PrimeSeats_Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly  ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            adminRepository = new AdminRepository(_context);

        }

        public IAdminRepository adminRepository { get; set; }

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
