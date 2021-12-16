using System.IO;
using System.Linq;
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
                string configJsonResource = assembly.GetManifestResourceNames().SingleOrDefault(x => x.Contains("config.json"));
                if (string.IsNullOrEmpty(configJsonResource)) throw new FileNotFoundException("Missing config.json");

                using (
                    StreamReader reader =
                        new StreamReader(
                            assembly.GetManifestResourceStream(configJsonResource)))
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