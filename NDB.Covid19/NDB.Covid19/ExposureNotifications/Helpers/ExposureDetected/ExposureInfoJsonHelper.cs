using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureInfoJsonHelper
    {
        public static string ExposureInfosToJson(IEnumerable<ExposureInfo> exposureInfos)
        {
            IEnumerable<JsonCompatibleExposureInfo> jsonCompatibleExposureInfos
                = exposureInfos.Select(exposureInfo => new JsonCompatibleExposureInfo(exposureInfo));
            return JsonConvert.SerializeObject(jsonCompatibleExposureInfos);
        }

        public static IEnumerable<ExposureInfo> ExposureInfosFromJsonCompatibleString(
            string jsonCompatibleExposureInfosJson)
        {
            IEnumerable<JsonCompatibleExposureInfo> jsonCompatibleExposureInfos
                = JsonConvert.DeserializeObject<IEnumerable<JsonCompatibleExposureInfo>>(
                    jsonCompatibleExposureInfosJson);
            return jsonCompatibleExposureInfos.Select(jsonCompatibleExposureInfo =>
                new ExposureInfo(
                    jsonCompatibleExposureInfo.Timestamp,
                    jsonCompatibleExposureInfo.Duration,
                    jsonCompatibleExposureInfo.AttenuationValue,
                    jsonCompatibleExposureInfo.TotalRiskScore,
                    jsonCompatibleExposureInfo.TransmissionRiskLevel));
        }
    }
}