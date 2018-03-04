using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.Services
{
    public class StateService : IStateService
    {
        private User _currentUser;

        public StateService()
        {
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public Guid GetCurrentUserId()
        {
            return _currentUser.UserId;
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
    }
}