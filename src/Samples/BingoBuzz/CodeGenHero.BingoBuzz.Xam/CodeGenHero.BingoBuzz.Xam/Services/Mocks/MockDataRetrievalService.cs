using CodeGenHero.BingoBuzz.Constants;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Services.Mocks
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

        public Task<bool> CreateNewMeeting(Meeting meeting, List<User> attendees)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateSendNewBingoInstanceEvent(Guid bingoInstanceContentId, Guid bingoInstanceId, Enums.BingoInstanceEventType eventType)
        {
            throw new NotImplementedException();
        }

        public Task<List<BingoInstanceContent>> GetBingoInstanceContentAsync(Guid bingoInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetCompanyByIdOrNull(Guid companyId)
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

        public Task<User> GetUserByEmailOrNullAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNumberOfBingos()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNumberOfGames()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNumberOfSquareClicks()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateSendNewMeeting(Meeting meeting, List<User> attendees)
        {
            throw new NotImplementedException();
        }
    }
}