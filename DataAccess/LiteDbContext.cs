using LiteDB;
using Microsoft.Extensions.Options;

namespace CartServiceApp.DataAccess
{
    public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }
        public LiteDbContext(IOptions<LiteDbOptions> options) { Database = new LiteDatabase(options.Value.DatabaseLocation); }
    }
}
