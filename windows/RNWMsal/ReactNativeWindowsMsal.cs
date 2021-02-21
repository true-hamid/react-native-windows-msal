using Microsoft.ReactNative.Managed;
using System;
using System.Threading.Tasks;

namespace RNWMsal
{
    [ReactModule]
    class ReactNativeWindowsMsal
    {
        [ReactMethod("getLoginToken")]
        public async void GetLoginTokenAsync(JSValue parameters, IReactPromise<string> promise)
        {
            string result = await MsalAuthentication.CallLoginAPI(parameters);
            promise.Resolve(result);
        }

        [ReactMethod("logoutUser")]
        public async void LogoutUser(IReactPromise<string> promise)
        {
            string result = await MsalAuthentication.LogoutUser();
            promise.Resolve(result);
        }
    }
}
