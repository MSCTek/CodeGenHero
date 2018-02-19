using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using MSC.BingoBuzz.Xam.ModelObj.BB;

namespace MSC.BingoBuzz.Xam.Interfaces
{
    public interface IDataService
    {
        Task<List<Meeting>> GetCurrentFutureMeetingsAsync();

        Task InsertAllDataCleanLocalDB();
    }
}