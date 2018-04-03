using CodeGenHero.BingoBuzz.Xam.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;
using System.Threading;

namespace CodeGenHero.BingoBuzz.Xam.Services
{
    public partial class DataRetrievalService : IDataRetrievalService
    {
        private static object _bingoInstanceLocker = new object();

        public ModelObj.BB.BingoInstance GetBingoInstance(Guid pk)
        {
            lock (_bingoInstanceLocker)
            {
                return _connection.Table<ModelData.BB.BingoInstance>().FirstOrDefault(x => x.BingoInstanceId == pk).ToModelObj();
            }
        }

        public List<ModelObj.BB.BingoInstance> GetBingoInstances()
        {
            lock (_bingoInstanceLocker)
            {
                return (from i in _connection.Table<ModelData.BB.BingoInstance>() select i.ToModelObj()).ToList();
            }
        }

        public int HardDeleteBingoInstance(Guid pk)
        {
            lock (_bingoInstanceLocker)
            {
                return _connection.Delete<ModelData.BB.BingoInstance>(pk);
            }
        }

        public async Task<bool> TruncateBingoInstanceTableAsync()
        {
            await _asyncConnection.QueryAsync<ModelData.BB.BingoInstance>("DELETE FROM BingoInstance");

            if (await _asyncConnection.Table<ModelData.BB.BingoInstance>().CountAsync() > 0)
            {
                //drop and create the table - just in case it gets corrupt...
                await _asyncConnection.DropTableAsync<ModelData.BB.BingoInstance>();
                _connection.CreateTable<ModelData.BB.BingoInstance>();
            }

            return true;
        }

        public Guid UpdateOrInsertBingoInstance(ModelObj.BB.BingoInstance item)
        {
            lock (_bingoInstanceLocker)
            {
                if (item.BingoInstanceId != null && item.BingoInstanceId != Guid.Empty)
                {
                    _connection.Update(item.ToModelData());
                }
                else
                {
                    item.BingoInstanceId = Guid.NewGuid();
                    _connection.Insert(item);
                }
                return item.BingoInstanceId;
            }
        }
    }
}