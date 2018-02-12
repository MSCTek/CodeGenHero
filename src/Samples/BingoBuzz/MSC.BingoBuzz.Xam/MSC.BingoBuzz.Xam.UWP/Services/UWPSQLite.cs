using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSC.BingoBuzz.Xam.Interfaces;
using SQLite;
using Windows.Storage;

namespace MSC.BingoBuzz.Xam.UWP.Services
{
    public class UWPSQLite : ISQLite
    {
        private string DBName = $"BingoBuzz.db3";

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
            var conn = new SQLite.SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return conn;
        }

        public SQLiteConnection GetConnection()
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
            var conn = new SQLite.SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return conn;
        }
    }
}