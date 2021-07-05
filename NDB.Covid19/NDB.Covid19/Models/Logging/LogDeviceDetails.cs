using System;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

#if !UNIT_TEST
using Xamarin.Essentials;

#endif

namespace NDB.Covid19.Models.Logging
{
    public class LogDeviceDetails
    {
        public LogDeviceDetails(LogSeverity severity, string logMessage, string additionalInfo = "")
        {
            Severity = severity;
            Description = Anonymizer.RedactText(logMessage);

            DateTime reportedUtcDateTime = SystemTime.Now().ToUniversalTime();
            DateTime lastNTPDateTime = LocalPreferencesHelper.LastNTPUtcDateTime;
            ReportedTime = reportedUtcDateTime == null ||
                           (lastNTPDateTime - reportedUtcDateTime).Duration().Days >= 365 * 2
                ? LocalPreferencesHelper.LastNTPUtcDateTime
                : reportedUtcDateTime;

            ApiVersion = Conf.APIVersion;

            string addInfoPostfix = ServiceLocator.Current.GetInstance<IApiDataHelper>()
                .GetBackGroundServicVersionLogString();
            AdditionalInfo = Anonymizer.RedactText(additionalInfo) + addInfoPostfix;

#if UNIT_TEST
            BuildNumber = "23";
            BuildVersion = "1.1";
            DeviceOSVersion = "13.4";
#else
            BuildNumber = AppInfo.BuildString;
            BuildVersion = AppInfo.VersionString;
            DeviceOSVersion = DeviceInfo.VersionString;
#endif
        }

        public LogSeverity Severity { get; }
        public string Description { get; }
        public DateTime ReportedTime { get; }
        public int ApiVersion { get; }
        public string BuildVersion { get; }
        public string BuildNumber { get; }
        public string DeviceOSVersion { get; }
        public string AdditionalInfo { get; set; }
    }
}