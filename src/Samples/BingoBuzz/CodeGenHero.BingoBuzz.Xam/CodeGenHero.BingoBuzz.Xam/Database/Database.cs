using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelData.BB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Database
{
    public sealed class Database : IDatabase
    {
        private SQLiteAsyncConnection _asyncConn;
        private SQLiteConnection _conn;

        public Database()
        {
        }

        public SQLiteAsyncConnection AsyncConnection
        {
            get { return _asyncConn; }
        }

        public SQLiteConnection Connection
        {
            get { return _conn; }
        }

        public void CreateTables()
        {
            try
            {
                if (_conn != null)
                {
                    _conn.CreateTable<BingoContent>();
                    _conn.CreateTable<BingoInstance>();
                    _conn.CreateTable<BingoInstanceContent>();
                    _conn.CreateTable<BingoInstanceEvent>();
                    _conn.CreateTable<BingoInstanceEventType>();
                    _conn.CreateTable<BingoInstanceStatusType>();
                    _conn.CreateTable<Company>();
                    _conn.CreateTable<FrequencyType>();
                    _conn.CreateTable<Meeting>();
                    _conn.CreateTable<MeetingAttendee>();
                    _conn.CreateTable<MeetingSchedule>();
                    _conn.CreateTable<NotificationMethodType>();
                    _conn.CreateTable<NotificationRule>();
                    _conn.CreateTable<RecurrenceRule>();
                    _conn.CreateTable<User>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error creating sqlite database tables. {ex.Message}");
            }
        }

        public async Task DropTablesAsync()
        {
            try
            {
                if (_conn != null)
                {
                    await _asyncConn.DropTableAsync<BingoContent>();
                    await _asyncConn.DropTableAsync<BingoInstance>();
                    await _asyncConn.DropTableAsync<BingoInstanceContent>();
                    await _asyncConn.DropTableAsync<BingoInstanceEvent>();
                    await _asyncConn.DropTableAsync<BingoInstanceEventType>();
                    await _asyncConn.DropTableAsync<BingoInstanceStatusType>();
                    await _asyncConn.DropTableAsync<Company>();
                    await _asyncConn.DropTableAsync<FrequencyType>();
                    await _asyncConn.DropTableAsync<Meeting>();
                    await _asyncConn.DropTableAsync<MeetingAttendee>();
                    await _asyncConn.DropTableAsync<MeetingSchedule>();
                    await _asyncConn.DropTableAsync<NotificationMethodType>();
                    await _asyncConn.DropTableAsync<NotificationRule>();
                    await _asyncConn.DropTableAsync<RecurrenceRule>();
                    await _asyncConn.DropTableAsync<User>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error creating sqlite database tables. {ex.Message}");
            }
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return _asyncConn;
        }

        public SQLiteConnection GetConnection()
        {
            return _conn;
        }

        public async Task RestoreCurrentUserDatabaseAsync()
        {
            await DropTablesAsync();
            CreateTables();
        }

        public void SetConnection(SQLiteConnection conn, SQLiteAsyncConnection asyncConn)
        {
            _conn = conn;
            _asyncConn = asyncConn;
        }
    }
}