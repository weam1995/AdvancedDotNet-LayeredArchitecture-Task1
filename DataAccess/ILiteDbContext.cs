using LiteDB;

namespace CartServiceApp.DataAccess
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}
