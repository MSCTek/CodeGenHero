using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.Services.Mocks
{
    public class MockDataRetrievalService : IDataRetrievalService
    {
        public MockDataRetrievalService()
        {
        }

        public Task<BingoInstance> CreateNewBingoInstance(Guid meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BingoInstanceContent>> GetBingoInstanceContentAsync(Guid bingoInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<BingoInstance> GetCurrentBingoInstanceOrNullAsync(Guid meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<Meeting> GetMeetingAsync(Guid meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MeetingAttendee>> GetMeetingAttendeesAsync(Guid meetingId)
        {
            throw new NotImplementedException();
        }

        public Task<Meeting> GetMeetingOrNullAsync(Guid meetingId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Meeting>> GetMeetingsAsync()
        {
            return new List<Meeting>();
        }
    }
}