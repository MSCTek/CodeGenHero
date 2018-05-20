using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public partial interface IDataRetrievalService
    {
        Task<BingoInstance> CreateNewBingoInstance(Guid meetingId);

        Task<bool> CreateNewMeeting(Meeting meeting, List<User> attendees);

        Task<bool> CreateSendNewBingoInstanceEvent(Guid bingoInstanceContentId, Guid bingoInstanceId, BingoBuzz.Constants.Enums.BingoInstanceEventType eventType);

        Task<List<ModelObj.BB.BingoInstanceContent>> GetBingoInstanceContentAsync(Guid bingoInstanceId);

        Task<int> GetTotalNumberOfBingos();

        Task<int> GetTotalNumberOfSquareClicks();

        Task<int> GetTotalNumberOfGames();

        Task<ModelObj.BB.BingoInstance> GetCurrentBingoInstanceOrNullAsync(Guid meetingId);

        Task<ModelObj.BB.User> GetUserByEmailOrNullAsync(string email);

        Task<ModelObj.BB.Company> GetCompanyByIdOrNull(Guid companyId);

         Task<List<ModelObj.BB.MeetingAttendee>> GetMeetingAttendeesAsync(Guid meetingId);

        Task<ModelObj.BB.Meeting> GetMeetingOrNullAsync(Guid meetingId);

        Task<List<ModelObj.BB.Meeting>> GetMeetingsAsync();

        Task<List<ModelObj.BB.User>> GetUsersAsync();
    }
}