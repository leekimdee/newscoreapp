using Microsoft.EntityFrameworkCore;

namespace NewsCoreApp.Data.EF
{
    public class EFUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EFUnitOfWork()
        {
            string connectionString = Startup.ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}