using Microsoft.EntityFrameworkCore;

namespace ArenaServer.Data
{
    public sealed class AppDbContextFactory : IAppDbContextFactory
    {
        private readonly DbContextOptions options;


        public AppDbContextFactory()
        {
            options = new DbContextOptionsBuilder().Options;
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