using Microsoft.EntityFrameworkCore;
using TVManagmentSystem.Models;

namespace TVManagmentSystem.DbContexti
{
    public class GlobalDbContext:DbContext
    {

        public GlobalDbContext(DbContextOptions<GlobalDbContext> OPS):base(OPS)
        {
                
        }
        public DbSet<Chanell> Chanelss { get; set; }

        public DbSet<Info> _info { get; set; }
    }
}
