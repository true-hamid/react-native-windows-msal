using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ReactNative.Managed;

namespace RNWMsal
{
    class MsalAuthentication
    {


        //Set the scope for API call to user.read
        private static string[] mScopes = new string[] { "https://management.core.windows.net//user_impersonation" };

        private static string mClientId = "";

        private static string mAuthority = "https://login.microsoftonline.com/";

        // The MSAL Public client app
        private static IPublicClientApplication PublicClientApp;

        private static AuthenticationResult authResult;


        public static async Task<string> CallLoginAPI(JSValue parameters){
            try
            {
                string tenant = parameters["tenant"].AsString();
                mClientId = parameters["clientId"].AsString();
                mAuthority = "https://login.microsoftonline.com/" + tenant;

                // Sign-in user using MSAL and obtain an access token for MS Graph
                return await SignInUserAndGetTokenUsingMSAL();

            }
            catch (MsalException msalEx)
            {
                string title = "Error Acquiring Token: ";
                return title + msalEx;
            }
            catch (Exception ex)
            {
                string title = "Error Acquiring Token Silently: ";
                //await DisplayMessageAsync($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
                return title + ex;
            }
        }

        /// <summary>
        /// Signs in the user and obtains an Access token for MS Graph
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns> Access Token</returns>
        private static async Task<string> SignInUserAndGetTokenUsingMSAL()
        {
            // Initialize the MSAL library by building a public client application
            PublicClientApp = PublicClientApplicationBuilder.Create(mClientId)
                .WithAuthority(mAuthority)
                .WithUseCorporateNetwork(false)
                .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                 .WithLogging((level, message, containsPii) =>
                 {
                     Debug.WriteLine($"MSAL: {level} {message} ");
                 }, LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
                .Build();

            IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await PublicClientApp.AcquireTokenSilent(mScopes, firstAccount)
                                                  .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                authResult = await PublicClientApp.AcquireTokenInteractive(mScopes)
                                                  .ExecuteAsync()
                                                  .ConfigureAwait(false);
            }
            return authResult.AccessToken;
        }

        public static async Task<string> LogoutUser()
        {
            try
            {
                IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
                IAccount firstAccount = accounts.FirstOrDefault();
                await PublicClientApp.RemoveAsync(firstAccount).ConfigureAwait(false);
            }
            catch (Exception msalEx)
            {
                string title = "Error Acquiring Token: ";
                return title + msalEx;
            }
            return "success";
        }
    }
}
