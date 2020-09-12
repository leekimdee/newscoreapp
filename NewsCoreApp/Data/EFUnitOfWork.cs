using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NewsCoreApp.Data.EF
{
    public class EFUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EFUnitOfWork(DbFactory dbFactory)
        {
            _context = dbFactory.context;
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