using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace NDB.Covid19.Configuration
{
    public static class SecretsInjection
    {
        public static SecretsObj GetSecrets()
        {
            try
            {
                Assembly assembly = typeof(SecretsInjection).GetTypeInfo().Assembly;
                using (
                    StreamReader reader =
                        new StreamReader(
                            assembly.GetManifestResourceStream(
                                $"{typeof(CommonDependencyInjectionConfig).Namespace}.config.json")))
                {
                    return JsonConvert.DeserializeObject<SecretsObj>(reader.ReadToEnd());
                }
            }
            catch
            {
                return new SecretsObj
                {
                    AuthHeader = "",
                    BaseUrl = "http://localhost:9095/",
                    FetchMinMinutes = 120,
                    Oauth2AccessTokenUrl = "",
                    Oauth2AuthoriseUrl = "",
                    Oauth2ClientId = "",
                    Oauth2RedirectUrl = "",
                    Oauth2Scope = "",
                    Oauth2VerifyTokenPublicKey = "",
                    UseDevTools = true
                };
            }
        }
    }
}