using Microsoft.EntityFrameworkCore;
using moreno.myrep.api.Models;

namespace moreno.myrep.api.Data
{
    public class MyrepDbContext : DbContext
    {
        public MyrepDbContext()
        {
            
        }

        public MyrepDbContext(DbContextOptions<MyrepDbContext> options) : base(options)
        {

        }

        public DbSet<MesTrabalho> MesTrabalho { get; set; }
        public DbSet<DiaTrabalho> DiaTrabalho { get; set; }
        public DbSet<Apontamento> Apontamento { get; set; }
    }
}
