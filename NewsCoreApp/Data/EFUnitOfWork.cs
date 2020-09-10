using Microsoft.EntityFrameworkCore;
using NewsCoreApp.Data.Interfaces;

namespace NewsCoreApp.Data.EF
{
    public class EFUnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EFUnitOfWork(ApplicationDbContext context)
        {
            //string connectionString = Startup.ConnectionString;
            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer(connectionString);
            //_context = new ApplicationDbContext(optionsBuilder.Options);
            _context = context;
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