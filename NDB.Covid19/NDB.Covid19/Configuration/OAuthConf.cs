namespace NDB.Covid19.Configuration
{
    public static class OAuthConf
    {
        private static readonly SecretsObj Secrets = SecretsInjection.GetSecrets();
        public static readonly string OAUTH2_CLIENT_ID = Secrets.Oauth2ClientId;
        public static readonly string OAUTH2_SCOPE = Secrets.Oauth2Scope;
        public static readonly string OAUTH2_REDIRECT_URL = Secrets.Oauth2RedirectUrl;
        public static readonly string OAUTH2_AUTHORISE_URL = Secrets.Oauth2AuthoriseUrl;
        public static readonly string OAUTH2_ACCESS_TOKEN_URL = Secrets.Oauth2AccessTokenUrl;
        public static readonly string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = Secrets.Oauth2VerifyTokenPublicKey;
    }
}