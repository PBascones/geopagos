using Geopagos.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace Geopagos.Repository
{
    public class GeopagosDbContext : DbContext
    {
        public GeopagosDbContext(DbContextOptions<GeopagosDbContext> options) : base(options) { }

        public DbSet<TournamentResult> TournamentResult { get; set; }
        public DbSet<PlayerSnapshot> PlayerSnapshot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentResult>()
                .HasMany(t => t.Players)
                .WithOne()
                .HasForeignKey(p => p.TournamentResultId);
        }
    }
}
