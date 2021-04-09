using System.Linq;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Models.Logging
{
    public class LogApiDetails
    {
        public LogApiDetails(ApiResponse apiResponse)
        {
            Api = "/" + apiResponse.Endpoint;
            ApiErrorCode = apiResponse.StatusCode > 0
                ? (int?) apiResponse.StatusCode
                : null;
            ApiErrorMessage = new int?[] {200, 201}.Contains(ApiErrorCode)
                ? null
                : Anonymizer.RedactText(apiResponse.ResponseText);
        }

        public string Api { get; }
        public int? ApiErrorCode { get; }
        public string ApiErrorMessage { get; }
    }
}