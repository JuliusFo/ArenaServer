using Microsoft.EntityFrameworkCore;

namespace ArenaServer.Data
{
    public sealed class AppDbContextFactory : IAppDbContextFactory
    {
        private readonly DbContextOptions options;


        public AppDbContextFactory()
        {
            options = new DbContextOptionsBuilder().UseSqlServer("Data Source=192.168.178.39;Initial Catalog=PokeArena;Persist Security Info=True;User ID=ArenaUser;Password=Kappa123").Options;
        }

        public AppDbContextFactory(DbContextOptions options)
        {
            this.options = options;
        }

        public AppDbContext Create()
        {
            return new AppDbContext(options);
        }
    }
}