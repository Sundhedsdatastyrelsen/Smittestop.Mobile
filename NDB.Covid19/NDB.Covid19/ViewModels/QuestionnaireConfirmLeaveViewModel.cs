using System;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public static class QuestionnaireConfirmLeaveViewModel
    {
        public static string QUESTIONNAIRE_CONFIRM_LEAVE_TITLE =>
            "QUESTIONNAIRE_CONFIRM_LEAVE_TITLE".Translate();

        public static string QUESTIONNAIRE_CONFIRM_LEAVE_DESCRIPTION =>
            "QUESTIONNAIRE_CONFIRM_LEAVE_DESCRIPTION".Translate();

        public static string QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_OK =>
            "QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_OK".Translate();

        public static string QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_CANCEL =>
            "QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_CANCEL".Translate();

        public static DialogViewModel CloseDialogViewModel => new DialogViewModel
        {
            Title = ErrorViewModel.REGISTER_LEAVE_HEADER,
            Body = ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
            OkBtnTxt = ErrorViewModel.REGISTER_LEAVE_CONFIRM,
            CancelbtnTxt = ErrorViewModel.REGISTER_LEAVE_CANCEL
        };

        public static void ValidateData(Action onSuccess, Action onFail)
        {
            if (AuthenticationState.PersonalData.Validate())
            {
                onSuccess?.Invoke();
            }
            else
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "Questionnaire Confirm Leave - no miba data or token expired",
                    null,
                    GetCorrelationId());
                onFail?.Invoke();
            }
        }
    }
}