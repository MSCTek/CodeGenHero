using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.Services
{
    public class StateService : IStateService
    {
        private User _currentUser;
        private JObject _authenticationObject;
        private string _authEmail;
        //wondering if some of these are not going to be GUIDs - from different providers
        private string _authId;
        private string _authGivenName;
        private string _authSurName;

        public StateService()
        {
        }

        public string GetAuthEmail()
        {
            return _authEmail;
        }

        public string GetAuthSurName()
        {
            return _authSurName;
        }

        public string GetAuthGivenName()
        {
            return _authGivenName;
        }

        public string GetAuthId()
        {
            return _authId;
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

        public JObject GetAuthenticationObject()
        {
            return _authenticationObject;
        }

        public bool IsUserLoggedIn()
        {
           return _authenticationObject != null ? true : false; 
        }

        public void SetAuthenticationObject(JObject authenticationObject)
        {
            _authenticationObject = authenticationObject;
                
            _authGivenName = authenticationObject["givenName"].ToString();
            _authId = authenticationObject["id"].ToString();
            _authSurName = authenticationObject["surname"].ToString();
            _authEmail = authenticationObject["userPrincipalName"].ToString();
        }

    }
}