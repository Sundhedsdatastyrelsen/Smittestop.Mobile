namespace NDB.Covid19.Configuration
{
    public class SecretsObj
    {
        public string BaseUrl { get; set; }
        public string AuthHeader { get; set; }
        public string Oauth2ClientId { get; set; }
        public string Oauth2Scope { get; set; }
        public string Oauth2RedirectUrl { get; set; }
        public string Oauth2AuthoriseUrl { get; set; }
        public string Oauth2AccessTokenUrl { get; set; }
        public string Oauth2VerifyTokenPublicKey { get; set; }
        public int FetchMinHours { get; set; }
        public bool UseDevTools { get; set; }
    }
}