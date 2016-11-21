namespace TribbleServer.Models
{
    using Microsoft.EntityFrameworkCore;

    public class TribbleContext : DbContext { 
        public DbSet<Tribble> Tribbles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=./TribbleApi.db");
        }
    }
}
