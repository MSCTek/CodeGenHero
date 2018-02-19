using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.Services.Mocks
{
    public class MockDataService : IDataService
    {
        public async Task<List<Meeting>> GetCurrentFutureMeetingsAsync()
        {
            return new List<Meeting>();
        }

        public async Task InsertAllDataCleanLocalDB()
        {
        }
    }
}