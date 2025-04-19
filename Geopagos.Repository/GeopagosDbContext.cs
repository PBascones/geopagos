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

            modelBuilder.Entity<TournamentResult>()
               .HasMany(tr => tr.Players)
               .WithOne(ps => ps.TournamentResult)
               .HasForeignKey(ps => ps.TournamentResultId);

            modelBuilder.Entity<TournamentResult>()
                .HasOne(tr => tr.WinnerSnapshot)
                .WithMany()
                .HasForeignKey(tr => tr.WinnerSnapshotId);
        }
    }
}
