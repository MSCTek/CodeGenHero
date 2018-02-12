using MSC.BingoBuzz.Xam.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MSC.BingoBuzz.Xam.iOS.Services
{
    public class IOSSQLite : ISQLite
    {
        private string DBName = $"BingoBuzz.db3";

        public SQLite.SQLiteAsyncConnection GetAsyncConnection()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, DBName);
            var param = new SQLiteConnectionString(path, false);
            var connection = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return connection;
        }

        public SQLite.SQLiteConnection GetConnection()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, DBName);
            var conn = new SQLite.SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return conn;
        }
    }
}