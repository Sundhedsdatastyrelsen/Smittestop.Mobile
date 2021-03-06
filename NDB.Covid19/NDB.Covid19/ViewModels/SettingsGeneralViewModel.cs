using System;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

namespace NDB.Covid19.ViewModels
{
    public class SettingsGeneralViewModel
    {
        public static string SETTINGS_GENERAL_TITLE = "SETTINGS_GENERAL_TITLE".Translate();
        public static string SETTINGS_GENERAL_EXPLANATION_ONE = "SETTINGS_GENERAL_EXPLANATION_ONE".Translate();
        public static string SETTINGS_GENERAL_EXPLANATION_TWO = "SETTINGS_GENERAL_EXPLANATION_TWO".Translate();
        public static string SETTINGS_GENERAL_MOBILE_DATA_HEADER = "SETTINGS_GENERAL_MOBILE_DATA_HEADER".Translate();
        public static string SETTINGS_GENERAL_MOBILE_DATA_DESC = "SETTINGS_GENERAL_MOBILE_DATA_DESC".Translate();

        public static string SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER =
            "SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER".Translate();

        public static string SETTINGS_GENERAL_RESTART_REQUIRED_TEXT =
            "SETTINGS_GENERAL_RESTART_REQUIRED_TEXT".Translate();

        public static string SETTINGS_GENERAL_MORE_INFO_LINK = "SETTINGS_GENERAL_MORE_INFO_LINK".Translate();

        public static string SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT =
            "SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT".Translate();

        public static string SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT =
            "SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT".Translate();

        public static string SETTINGS_GENERAL_DA = "SETTINGS_GENERAL_DA".Translate();
        public static string SETTINGS_GENERAL_EN = "SETTINGS_GENERAL_EN".Translate();

        public static DialogViewModel AreYouSureDialogViewModel = new DialogViewModel
        {
            Body = "SETTINGS_GENERAL_DIALOG_BODY".Translate(),
            CancelbtnTxt = "SETTINGS_GENERAL_DIALOG_CANCEL".Translate(),
            OkBtnTxt = "SETTINGS_GENERAL_DIALOG_OK".Translate(),
            Title = "SETTINGS_GENERAL_DIALOG_TITLE".Translate()
        };

        public static SettingsLanguageSelection Selection { get; private set; }

        public static DialogViewModel GetChangeLanguageViewModel => new DialogViewModel
        {
            Title = "SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER".Translate(),
            Body = "SETTINGS_GENERAL_RESTART_REQUIRED_TEXT".Translate(),
            OkBtnTxt = "SETTINGS_GENERAL_DIALOG_OK".Translate()
        };

        public bool GetStoredCheckedState()
        {
            return LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();
        }

        public void OnCheckedChange(bool isChecked)
        {
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(isChecked);
        }

        /// <summary>
        ///     Opens the smittestop.dk link in an in-app browser.
        /// </summary>
        public static void OpenSmitteStopLink()
        {
            try
            {
                ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(SETTINGS_GENERAL_MORE_INFO_LINK);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e,
                    "Failed to open smittestop.dk link on general settings page");
            }
        }

        public void SetSelection(SettingsLanguageSelection selection)
        {
            Selection = selection;
        }
    }
}