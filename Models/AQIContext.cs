using Microsoft.EntityFrameworkCore;
namespace ChinaAQIDataCore.Models
{
    public class AQIContext: DbContext
    {
        public AQIContext(DbContextOptions<AQIContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AQIDTO>()
                .HasKey( c => new {c.Area, c.StationCode, c.TimePoint });
        }

        public DbSet<AQIDTO> AQIItems { get; set; }

    }
}
