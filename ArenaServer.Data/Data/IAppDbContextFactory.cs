namespace ArenaServer.Data
{
    public interface IAppDbContextFactory
    {
        AppDbContext Create();
    }
}