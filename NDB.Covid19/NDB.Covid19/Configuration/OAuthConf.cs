namespace NDB.Covid19.Configuration
{
    public static class OAuthConf
    {
        public static readonly string OAUTH2_CLIENT_ID = "INJECTED_DURING_BUILD";
        public static readonly string OAUTH2_SCOPE = "INJECTED_DURING_BUILD";
        public static readonly string OAUTH2_REDIRECT_URL = "INJECTED_DURING_BUILDt";

        public static readonly string OAUTH2_BASE_URL = "http://localhost:5001/";
        public static string OAUTH2_AUTHORISE_URL => OAUTH2_BASE_URL + "auth";
        public static string OAUTH2_ACCESS_TOKEN_URL => OAUTH2_BASE_URL + "token";
        public static readonly string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = "INJECTED_DURING_BUILD";
    }
}