using MSC.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.Interfaces
{
    public interface IStateService
    {
        User GetCurrentUser();

        Guid GetCurrentUserId();

        void SetCurrentUser(User user);
    }
}