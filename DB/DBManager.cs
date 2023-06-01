using DB.Context;
using DB.Model;
using DB.Repository;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class DBManager
    {
        private static ApplicationContextSQLServer _sqlServerContext;
        private static ApplicationContextSQLite _sqliteContext;
        private static bool DbInitialized = false;
        public DBManager()
        {
            if (!DbInitialized)
            {
                TryInitializeDbContext();
                DbInitialized = true;
            }
        }

        public IRepository<Dog> DogRepository => new DogRepository(GetInitializedDbSet(), GetInitializedContext());

        private DbSet<Dog> GetInitializedDbSet()
        {
            if (_sqlServerContext != null)
            {
                return _sqlServerContext.Dogs;
            }
            return _sqliteContext.Dogs;
        }

        private DbContext GetInitializedContext()
        {

            if (_sqlServerContext != null)
            {
                return _sqlServerContext;
            }
            return _sqliteContext;
        }

        private void TryInitializeDbContext()
        {

            var isSQLiteOnly = false;
            if (isSQLiteOnly)
            {
                try
                {
                    _sqliteContext = new();
                }
                catch (Exception exceptionSQLite)
                {
                    Console.WriteLine(Environment.NewLine + exceptionSQLite.Message + Environment.NewLine);
                    throw new Exception("Can't create any database ");
                }
            }
            else

                try
                {
                    _sqlServerContext = new();
                }
                catch (Exception exceptionServer)
                {
                    Console.WriteLine(Environment.NewLine + exceptionServer.Message + Environment.NewLine);
                    try
                    {
                        _sqliteContext = new();
                    }
                    catch (Exception exceptionSQLite)
                    {
                        Console.WriteLine(Environment.NewLine + exceptionSQLite.Message + Environment.NewLine);
                        throw new Exception("Can't create any database ");
                    }
                }
        }
    }
}
