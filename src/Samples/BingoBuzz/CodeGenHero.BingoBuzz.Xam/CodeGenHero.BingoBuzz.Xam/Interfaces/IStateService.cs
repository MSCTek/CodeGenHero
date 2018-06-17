using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public interface IStateService
    {
        User GetCurrentUser();

        Guid GetCurrentUserId();

        void SetCurrentUser(User user);

        string GetAuthId();

        string GetAuthGivenName();

        string GetAuthSurName();

        string GetAuthEmail();

        JObject GetAuthenticationObject();

        void SetAuthenticationObject(JObject authenticationObject);

        bool IsUserLoggedIn();
    }
}