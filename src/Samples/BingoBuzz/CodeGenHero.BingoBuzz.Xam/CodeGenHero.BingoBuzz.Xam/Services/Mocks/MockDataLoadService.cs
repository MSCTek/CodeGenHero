using CodeGenHero.BingoBuzz.Xam.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Services.Mocks
{
    public class MockDataLoadService : IDataDownloadService
    {
        public async Task InsertAllDataCleanLocalDB(Guid userId)
        {
        }

        public async Task InsertOrReplaceAuthenticatedUser(Guid userId)
        {
           
        }

        public Task InsertOrReplaceAuthenticatedUser(string email, Guid userId, string givenName, string surName)
        {
            throw new NotImplementedException();
        }
    }
}