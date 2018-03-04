using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public interface IDatabase
    {
        void CreateTables();

        Task DropTablesAsync();

        SQLiteAsyncConnection GetAsyncConnection();

        SQLiteConnection GetConnection();

        Task RestoreCurrentUserDatabaseAsync();

        void SetConnection(SQLiteConnection conn, SQLiteAsyncConnection asyncConn);
    }
}