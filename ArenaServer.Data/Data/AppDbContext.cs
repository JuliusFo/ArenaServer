using Microsoft.EntityFrameworkCore;

namespace ArenaServer.Data
{
    public class AppDbContext : DbContext
    {
        #region Fields



        #endregion

        #region Constructor

        public AppDbContext() : base()
        {

        }

        #endregion

        #region Properties

        public DbSet<Twitchuser> Twitchuser { get; set; }

        public DbSet<SdPokemon> SdPokemon { get; set; }

        public DbSet<SdAchievement> SdAchievement { get; set; }

        public DbSet<CatchedPokemon> CatchedPokemon { get; set; }

        public DbSet<Achievements> Achievements { get; set; }

        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=192.168.178.39;Initial Catalog=PokeArena;Persist Security Info=True;User ID=ArenaUser;Password=Kappa123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TwitchuserConfiguration());
            modelBuilder.ApplyConfiguration(new SdPokemonConfiguration());
            modelBuilder.ApplyConfiguration(new SdAchievementConfiguration());
            modelBuilder.ApplyConfiguration(new CatchedPokemonConfiguration());
            modelBuilder.ApplyConfiguration(new AchievementsConfiguration());
        }

            #endregion
        }
}