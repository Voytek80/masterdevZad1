using MasterdevZad1.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterdevZad1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Klienci> Klienci { get; set; }
    }
}
