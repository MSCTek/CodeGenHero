﻿using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public interface IDataRetrievalService
    {
        Task<BingoInstance> CreateNewBingoInstance(Guid meetingId);

        Task<List<ModelObj.BB.BingoInstanceContent>> GetBingoInstanceContentAsync(Guid bingoInstanceId);

        Task<ModelObj.BB.BingoInstance> GetCurrentBingoInstanceOrNullAsync(Guid meetingId);

        Task<List<ModelObj.BB.MeetingAttendee>> GetMeetingAttendeesAsync(Guid meetingId);

        Task<ModelObj.BB.Meeting> GetMeetingOrNullAsync(Guid meetingId);

        Task<List<ModelObj.BB.Meeting>> GetMeetingsAsync();
    }
}