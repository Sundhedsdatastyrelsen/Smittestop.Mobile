using System;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public static class QuestionnairePreShareViewModel
    {
        public static string QUESTIONNAIRE_PRE_SHARE_TITLE =>
            "QUESTIONNAIRE_PRE_SHARE_TITLE".Translate();

        public static string QUESTIONNAIRE_PRE_SHARE_DESCRIPTION =>
            "QUESTIONNAIRE_PRE_SHARE_DESCRIPTION".Translate();

        public static string QUESTIONNAIRE_PRE_SHARE_NEXT_BUTTON =>
            "QUESTIONNAIRE_PRE_SHARE_NEXT_BUTTON".Translate();

        public static DialogViewModel CloseDialogViewModel => new DialogViewModel
        {
            Title = ErrorViewModel.REGISTER_LEAVE_HEADER,
            Body = ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
            OkBtnTxt = ErrorViewModel.REGISTER_LEAVE_CONFIRM,
            CancelbtnTxt = ErrorViewModel.REGISTER_LEAVE_CANCEL
        };

        public static void InvokeNextButtonClick(Action onSuccess, Action onFail)
        {
            if (AuthenticationState.PersonalData.Validate())
            {
                onSuccess?.Invoke();
            }
            else
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "Questionnaire Pre Share - no miba data or token expired",
                    null,
                    GetCorrelationId());
                onFail?.Invoke();
            }
        }
    }
}