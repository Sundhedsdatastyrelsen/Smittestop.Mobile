using System;
using System.Net.Http.Headers;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using Xamarin.Essentials;

namespace NDB.Covid19.WebServices.Utils
{
    public class HttpClientManager
    {
        public static string CsrfpTokenCookieName = "Csrfp-Token";
        public static string CsrfpTokenHeader = "csrfp-token";
        private static HttpClientManager _instance;
        public IHttpClientAccessor HttpClientAccessor;

        private HttpClientManager()
        {
            HttpClientAccessor = new DefaultHttpClientAccessor();
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/zip"));
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("Authorization_Mobile", Conf.AUTHORIZATION_HEADER);

            // If running on a platform that is not supported by Xamarin.Essentials (for example if unit testing)
            IDeviceInfo deviceInfo = ServiceLocator.Current.GetInstance<IDeviceInfo>();
            if (deviceInfo.Platform == DevicePlatform.Unknown)
            {
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("Manufacturer", "Unknown");
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OSVersion", "Unknown");
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OS", "Unknown");
            }
            else
            {
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("Manufacturer", deviceInfo.Manufacturer);
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OSVersion", deviceInfo.VersionString);
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OS", DeviceUtils.DeviceType);
            }

            HttpClientAccessor.HttpClient.MaxResponseContentBufferSize = Conf.MAX_CONTENT_BUFFER_SIZE;
            HttpClientAccessor.HttpClient.Timeout = TimeSpan.FromSeconds(Conf.DEFAULT_TIMEOUT_SERVICECALLS_SECONDS);
        }

        public static HttpClientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    MakeNewInstance();
                }

                return _instance;
            }
        }

        public void AddSecretToHeaderIfMissing()
        {
            HeaderUtils.AddSecretToHeader(HttpClientAccessor);
        }

        public static void MakeNewInstance()
        {
            _instance?.HttpClientAccessor?.HttpClient?.CancelPendingRequests();
            _instance = new HttpClientManager();
        }

        public bool CheckInternetConnection()
        {
            return ServiceLocator.Current.GetInstance<IConnectivity>().NetworkAccess != NetworkAccess.None;
        }
    }
}