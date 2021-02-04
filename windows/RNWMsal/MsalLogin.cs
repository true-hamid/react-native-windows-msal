using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RNWMsal
{
    class MsalLogin
    {


        //Set the scope for API call to user.read
        private static string[] mScopes = new string[] { "https://management.core.windows.net//user_impersonation" };

        // Below are the clientId (Application Id) of your app registration and the tenant information. 
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - The content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use organizations
        //   - for any Work or School accounts, or Microsoft personal account, use common
        //   - for Microsoft Personal account, use consumers

        private static string mClientId = "";

        private static string mAuthority = "https://login.microsoftonline.com/";

        // The MSAL Public client app
        private static IPublicClientApplication PublicClientApp;

        private static AuthenticationResult authResult;


        public static async Task<string> CallLoginAPI(string Tenant, string Client){
            try
            {
                mClientId = Client;
                mAuthority = "https://login.microsoftonline.com/" + Tenant;

                // Sign-in user using MSAL and obtain an access token for MS Graph
                //GraphServiceClient graphClient = await SignInAndInitializeGraphServiceClient(scopes);
                return await SignInAndInitializeGraphServiceClient(mScopes);

                // Call the /me endpoint of Graph
                //User graphUser = await graphClient.Me.Request().GetAsync();

                //// Go back to the UI thread to make changes to the UI


                //    string response = "Display Name: " + graphUser.DisplayName + "\nBusiness Phone: " + graphUser.BusinessPhones.FirstOrDefault()
                //                      + "\nGiven Name: " + graphUser.GivenName + "\nid: " + graphUser.Id
                //                      + "\nUser Principal Name: " + graphUser.UserPrincipalName + "\n" + graphUser.OnlineMeetings;
                //return response;
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
        /// Sign in user using MSAL and obtain a token for MS Graph
        /// </summary>
        /// <returns>GraphServiceClient</returns>
        //private async static Task<GraphServiceClient> SignInAndInitializeGraphServiceClient(string[] scopes)
        private async static Task<string> SignInAndInitializeGraphServiceClient(string[] scopes)

        {
            //GraphServiceClient graphClient = new GraphServiceClient(MSGraphURL,
            //    new DelegateAuthenticationProvider(async (requestMessage) =>
            //    {
            //        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", await SignInUserAndGetTokenUsingMSAL(scopes));
            //    }));

            //return await Task.FromResult(graphClient);
            return await SignInUserAndGetTokenUsingMSAL(scopes);
        }

        /// <summary>
        /// Signs in the user and obtains an Access token for MS Graph
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns> Access Token</returns>
        private static async Task<string> SignInUserAndGetTokenUsingMSAL(string[] scopes)
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

            // It's good practice to not do work on the UI thread, so use ConfigureAwait(false) whenever possible.
            IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                                  .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                authResult = await PublicClientApp.AcquireTokenInteractive(scopes)
                                                  .ExecuteAsync()
                                                  .ConfigureAwait(false);

            }
            return authResult.AccessToken;
        }


    }
}
