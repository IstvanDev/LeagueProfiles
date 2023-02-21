using LeagueProfiles.Models;
using Microsoft.EntityFrameworkCore;

namespace LeagueProfiles.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Champion> Champions { get; set; }

        //public DbSet<ChampionPlayer> ChampionPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ChampionPlayer>()
            //    .HasKey(cp => new { cp.PlayerId, cp.ChampionId });

            //modelBuilder.Entity<ChampionPlayer>()
            //    .HasOne(p => p.Player)
            //    .WithMany(cp => cp.ChampionPlayers)
            //    .HasForeignKey(p => p.PlayerId);

            //modelBuilder.Entity<ChampionPlayer>()
            //    .HasOne(c => c.Champion)
            //    .WithMany(cp => cp.ChampionPlayers)
            //    .HasForeignKey(c => c.ChampionId);
        }
    }
}
