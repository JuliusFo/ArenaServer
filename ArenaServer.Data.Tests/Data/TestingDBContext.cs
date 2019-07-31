using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;

namespace ArenaServer.Data.Tests
{
    public class TestingDBContext
    {
        #region Fields

        private readonly Lazy<AppDbContext> defaultDb;

        private static int uniqueDbNumber;
        private int currentDbNumber;
        private readonly InMemoryDatabaseRoot databaseRoot;

        private readonly CancellationTokenSource cancellation;

        #endregion

        #region Constructor

        public TestingDBContext()
        {
            this.currentDbNumber = Interlocked.Increment(ref uniqueDbNumber);
            this.databaseRoot = new InMemoryDatabaseRoot();
            this.defaultDb = new Lazy<AppDbContext>(CreateDb);
            DbFactory = new TestAppDbContextFactory(CreateDb);
            this.cancellation = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (this.defaultDb.IsValueCreated)
                this.defaultDb.Value.Dispose();

            cancellation.Cancel();
        }

        #endregion

        #region Properties

        public AppDbContext Db => defaultDb.Value;

        public IAppDbContextFactory DbFactory { get; }

        public CancellationToken CancellationToken => cancellation.Token;

        #endregion

        #region Database

        private AppDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase($"AppDb_{currentDbNumber}", databaseRoot)
                .Options;

            return new AppDbContext(options);
        }

        sealed class TestAppDbContextFactory : IAppDbContextFactory
        {
            private readonly Func<AppDbContext> creator;

            public TestAppDbContextFactory(Func<AppDbContext> creator) => this.creator = creator;

            public AppDbContext Create() => creator();
        }

        #endregion
    }
}
