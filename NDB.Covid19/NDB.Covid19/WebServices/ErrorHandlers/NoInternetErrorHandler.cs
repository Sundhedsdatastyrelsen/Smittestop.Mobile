using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices.Utils;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public class NoInternetErrorHandler : BaseErrorHandler, IErrorHandler
    {
        public bool IsSilent;

        public NoInternetErrorHandler(bool IsSilent)
        {
            this.IsSilent = IsSilent;
        }

        public override string ErrorMessageTitle => "CONNECTION_ERROR_TITLE".Translate();
        public override string ErrorMessage => "NO_INTERNET_ERROR_MESSAGE".Translate();

        public bool IsResponsible(ApiResponse apiResponse)
        {
            return !HttpClientManager.Instance.CheckInternetConnection();
        }

        public void HandleError(ApiResponse apiResponse)
        {
            LogUtils.LogApiError(LogSeverity.WARNING, apiResponse, IsSilent, "",
                "Failed contact to server: No internet");
            if (!IsSilent)
            {
                ShowErrorToUser();
            }
        }
    }
}