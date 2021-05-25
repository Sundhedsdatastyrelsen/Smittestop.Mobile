using System;
using NDB.Covid19.Models.SQLite;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class LogDTO
    {
        public LogDTO(LogSQLiteModel log)
        {
#if UNIT_TEST
            DeviceType = "123";
            DeviceDescription = "123";
#else
            DeviceType = DeviceUtils.DeviceType;
            DeviceDescription = DeviceUtils.DeviceModel;
#endif
            ReportedTime = log.ReportedTime;
            Severity = log.Severity;
            Description = log.Description;
            ApiVersion = log.ApiVersion;
            BuildVersion = log.BuildVersion;
            BuildNumber = log.BuildNumber;
            DeviceOSVersion = log.DeviceOSVersion;
            DeviceCorrelationId = "";
            ExceptionType = log.ExceptionType;
            ExceptionMessage = log.ExceptionMessage;
            ExceptionStackTrace = log.ExceptionStackTrace;
            InnerExceptionType = log.InnerExceptionType;
            InnerExceptionMessage = log.InnerExceptionMessage;
            InnerExceptionStackTrace = log.InnerExceptionStackTrace;
            Api = log.Api;
            ApiErrorCode = log.ApiErrorCode;
            ApiErrorMessage = log.ApiErrorMessage;
            AdditionalInfo = log.AdditionalInfo;
            CorrelationId = log.CorrelationId;
        }

        //Parameters that are always filled out
        public DateTime ReportedTime { get; }
        public string Severity { get; }
        public string Description { get; }
        public int ApiVersion { get; }
        public string BuildVersion { get; }
        public string BuildNumber { get; }
        public string DeviceOSVersion { get; }
        public string DeviceCorrelationId { get; }
        public string DeviceType { get; }
        public string DeviceDescription { get; }

        //Used for exceptions
        public string ExceptionType { get; }
        public string ExceptionMessage { get; }
        public string ExceptionStackTrace { get; }
        public string InnerExceptionType { get; }
        public string InnerExceptionMessage { get; }
        public string InnerExceptionStackTrace { get; }

        //Used for API errors
        public string Api { get; set; }
        public int? ApiErrorCode { get; }
        public string ApiErrorMessage { get; }

        public string AdditionalInfo { get; }

        //Used for the authentication/submission flow
        public string CorrelationId { get; }

        public override string ToString()
        {
            return $"{Severity} Log: " + Description;
        }
    }
}