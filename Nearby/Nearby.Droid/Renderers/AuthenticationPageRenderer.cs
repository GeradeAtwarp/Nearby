using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Nearby.Pages.Auth;
using Nearby.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using Nearby.Helpers;
using Newtonsoft.Json;
using Nearby.Utils;

[assembly: ExportRenderer(typeof(Authenticate), typeof(AuthenticationPageRenderer))]

namespace Nearby.Droid.Renderers
{
    public class AuthenticationPageRenderer : PageRenderer
    {
        bool isShown;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // Retrieve any stored account information
            var accounts = AccountStore.Create(Context).FindAccountsForService(App.AppName);
            var account = accounts.FirstOrDefault();

            if (account == null)
            {
                if (!isShown)
                {
                    isShown = true;

                    // Initialize the object that communicates with the OAuth service
                    var auth = new OAuth2Authenticator(
                                   Constants.ClientId,
                                   Constants.ClientSecret,
                                   Constants.Scope,
                                   new Uri(Constants.AuthorizeUrl),
                                   new Uri(Constants.RedirectUrl),
                                   new Uri(Constants.AccessTokenUrl));

                    // Register an event handler for when the authentication process completes
                    auth.Completed += OnAuthenticationCompleted;

                    // Display the UI
                    var activity = Context as Activity;
                    activity.StartActivity(auth.GetUI(activity));
                }
            }
            else
            {
                if (!isShown)
                {
                    App.User.Email = account.Username;
                    App.SuccessfulLoginAction.Invoke();
                }
            }

        }

        async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = response.GetResponseText();
                    App.User = JsonConvert.DeserializeObject<User>(userJson);
                    e.Account.Username = App.User.Email;
                    AccountStore.Create(Context).Save(e.Account, App.AppName);
                }
            }

            // If the user is logged in navigate to the TodoList page.
            // Otherwise allow another login attempt.
            App.SuccessfulLoginAction.Invoke();
        }
    }
}