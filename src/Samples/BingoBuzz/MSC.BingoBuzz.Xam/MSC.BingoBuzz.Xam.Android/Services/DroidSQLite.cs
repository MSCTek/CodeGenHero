using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MSC.BingoBuzz.Xam.Interfaces;
using SQLite;

namespace MSC.BingoBuzz.Xam.Droid.Services
{
    public class DroidSQLite : ISQLite
    {
        private string DBName = $"BingoBuzz.db3";

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, DBName);
            // Create the connection
            var conn = new SQLite.SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            // Return the database connection
            return conn;
        }

        public SQLiteConnection GetConnection()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, DBName);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            // Return the database connection
            return conn;
        }
    }
}