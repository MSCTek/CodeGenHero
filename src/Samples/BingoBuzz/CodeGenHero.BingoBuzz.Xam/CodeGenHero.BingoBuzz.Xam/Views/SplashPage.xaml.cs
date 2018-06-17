using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ViewModels;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeGenHero.BingoBuzz.Xam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage, IContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        private SplashViewModel vm
        {
            get { return BindingContext as SplashViewModel; }
        }

        public void PrepareForDispose()
        {
            if (vm != null)
            {
                vm.Cleanup();
                //clean up view here
                vm.Dispose();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (vm != null)
            {
            }
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // let's see if we have a user in our belly already
            try
            {
                AuthenticationResult ar =
                    await App.PCA.AcquireTokenSilentAsync(App.Scopes, App.PCA.Users.FirstOrDefault());
                RefreshUserData(ar.AccessToken);
                btnSignInSignOut.Text = "Sign out";
                btnOpenApp.IsVisible = true;
            }
            catch
            {
                // doesn't matter, we go in interactive more
                btnSignInSignOut.Text = "Sign in";
                btnOpenApp.IsVisible = false;
            }

           

        }
        async void OnSignInSignOut(object sender, EventArgs e)
        {
            try
            {
                if (btnSignInSignOut.Text == "Sign in")
                {
                    AuthenticationResult ar = await App.PCA.AcquireTokenAsync(App.Scopes, App.UiParent);
                    RefreshUserData(ar.AccessToken);
                    btnSignInSignOut.Text = "Sign out";
                    btnOpenApp.IsVisible = true;
                }
                else
                {
                    foreach (var user in App.PCA.Users)
                    {
                        App.PCA.Remove(user);
                    }
                    slUser.IsVisible = false;
                    btnSignInSignOut.Text = "Sign in";
                    btnOpenApp.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK, Thanks.");
            }
        }


        private async void OnOpenApp(object sender, EventArgs e)
        {
            //navigate to the welcome page
            if (vm != null)
            {
                await vm.Init();
            }
        }

        private async void OnViewRawUserInfo(object sender, EventArgs e)
        {
            await DisplayAlert("Raw User Token", userResponse, "OK, Thanks.");
        }

        private string userResponse = string.Empty;


        public async void RefreshUserData(string token)
        {
            //get data from API
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.SendAsync(message);
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject user = JObject.Parse(responseString);

                slUser.IsVisible = true;
                lblDisplayName.Text = user["displayName"].ToString();
                lblGivenName.Text = user["givenName"].ToString();
                lblId.Text = user["id"].ToString();
                lblSurname.Text = user["surname"].ToString();
                lblUserPrincipalName.Text = user["userPrincipalName"].ToString();

                userResponse = user.ToString();

                // just in case
                btnSignInSignOut.Text = "Sign out";
                btnOpenApp.IsVisible = true;

            }
            else
            {
                btnOpenApp.IsVisible = false;
                
            }
        }

        
    }
}