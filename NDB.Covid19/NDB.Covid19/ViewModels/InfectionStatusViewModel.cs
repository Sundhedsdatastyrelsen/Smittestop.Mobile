using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using Xamarin.Essentials;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ViewModels
{
    public class InfectionStatusViewModel
    {
        public static bool IsScrollDownShown = LocalPreferencesHelper.IsScrollDownShown;

        private DateTime _latestMessageDateTime = DateTime.Today;

        public InfectionStatusViewModel()
        {
            //This subscribe is intentional placed here, as the InfectionStatus is not supposed to be garbed collected
            SubscribeMessages();
            Connectivity.ConnectivityChanged += (sender, args) =>
            {
                if (args.NetworkAccess == NetworkAccess.Internet)
                {
                    RequestSSIUpdate();
                }
            };
        }

        public static string INFECTION_STATUS_PAGE_TITLE => "SMITTESPORING_PAGE_TITLE".Translate();
        public static string INFECTION_STATUS_ACTIVE_TEXT => "SMITTESPORING_ACTIVE_HEADER".Translate();
        public static string INFECTION_STATUS_INACTIVE_TEXT => "SMITTESPORING_INACTIVE_HEADER".Translate();

        public static string INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT =>
            "SMITTESPORING_ACTIVE_DESCRIPTION".Translate();

        public static string SMITTESPORING_INACTIVE_DESCRIPTION => "SMITTESPORING_INACTIVE_DESCRIPTION".Translate();
        public static string INFECTION_STATUS_MESSAGE_HEADER_TEXT => "SMITTESPORING_MESSAGE_HEADER".Translate();

        public static string INFECTION_STATUS_MESSAGE_ACCESSIBILITY_TEXT =>
            "SMITTESPORING_MESSAGE_HEADER_ACCESSIBILITY".Translate();

        public static string INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT => "SMITTESPORING_MESSAGE_DESCRIPTION".Translate();

        public static string INFECTION_STATUS_NO_NEW_MESSAGE_SUBHEADER_TEXT =>
            "SMITTESPORING_NO_NEW_MESSAGE_DESCRIPTION".Translate();

        public static string INFECTION_STATUS_REGISTRATION_HEADER_TEXT => "SMITTESPORING_REGISTER_HEADER".Translate();

        public static string INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT =>
            "SMITTESPORING_REGISTER_DESCRIPTION".Translate();

        public static string SCROLL_DOWN_HEADER_TEXT => "SMITTESPORING_SCROLL".Translate();
        public static string INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT => "MENU_TEXT".Translate();

        public static string INFECTION_STATUS_NEW_MESSAGE_NOTIFICATION_DOT_ACCESSIBILITY_TEXT =>
            "SMITTESPORING_NEW_MESSAGE_NOTIFICATION_DOT_ACCESSIBILITY".Translate();

        public static string INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT =>
            "SMITTESPORING_START_BUTTON_ACCESSIBILITY".Translate();

        public static string INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT =>
            "SMITTESPORING_STOP_BUTTON_ACCESSIBILITY".Translate();

        public static string INFECTION_STATUS_DISEASE_RATE_HEADER_TEXT =>
            "SMITTESPORING_DISEASE_RATE_HEADER".Translate();

        public static string INFECTION_STATUS_DISEASE_RATE_LAST_UPDATED_TEXT =>
            "SMITTESPORING_DISEASE_RATE_UPDATE".Translate();

        public static string INFECTION_STATUS_DISEASE_RATE_LAST_UPDATED_ACCESSIBILITY_TEXT =>
            "SMITTESPORING_DISEASE_RATE_UPDATE_ACCESSIBILITY".Translate();

        // Spinner Dialog
        public static string INFECTION_STATUS_SPINNER_DIALOG_TITLE =>
            "INFECTION_STATUS_SPINNER_DIALOG_TITLE".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_MESSAGE =>
            "INFECTION_STATUS_SPINNER_DIALOG_MESSAGE".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OPTION_NO_REMINDER =>
            "INFECTION_STATUS_SPINNER_DIALOG_OPTION_NO_REMINDER".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OPTION_ONE_HOUR =>
            "INFECTION_STATUS_SPINNER_DIALOG_OPTION_ONE_HOUR".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWO_HOURS =>
            "INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWO_HOURS".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OPTION_FOUR_HOURS =>
            "INFECTION_STATUS_SPINNER_DIALOG_OPTION_FOUR_HOURS".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OPTION_EIGHT_HOURS =>
            "INFECTION_STATUS_SPINNER_DIALOG_OPTION_EIGHT_HOURS".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWELVE_HOURS =>
            "INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWELVE_HOURS".Translate();

        public static string INFECTION_STATUS_SPINNER_DIALOG_OK_BUTTON =>
            "INFECTION_STATUS_SPINNER_DIALOG_OK_BUTTON".Translate();

        public static DateTime DiseaseRateUpdatedDateTime =>
            LocalPreferencesHelper.SSILastUpdateDateTime.ToLocalTime();

        public static string LastUpdateString => DiseaseRateUpdatedDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(
                INFECTION_STATUS_DISEASE_RATE_LAST_UPDATED_TEXT,
                $"{DateUtils.GetDateFromDateTime(DiseaseRateUpdatedDateTime, "m")}",
                $"{DateUtils.GetDateFromDateTime(DiseaseRateUpdatedDateTime, "t")}")
            : "";

        public static string LastUpdateAccessibilityString =>
            DiseaseRateUpdatedDateTime != DateTime.MinValue.ToLocalTime()
                ? string.Format(
                    INFECTION_STATUS_DISEASE_RATE_LAST_UPDATED_ACCESSIBILITY_TEXT,
                    $"{DateUtils.GetDateFromDateTime(DiseaseRateUpdatedDateTime, "m")}",
                    $"{DateUtils.GetDateFromDateTime(DiseaseRateUpdatedDateTime, "t")}")
                : "";

        public bool ShowNewMessageIcon { get; private set; }
        public EventHandler NewMessagesIconVisibilityChanged { get; set; }
        public bool IsAppRestricted { get; set; }

        public string NewMessageSubheaderTxt =>
            ShowNewMessageIcon
                ? $"{INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT} {DateUtils.GetDateFromDateTime(_latestMessageDateTime, "d. MMMMM")}"
                : INFECTION_STATUS_NO_NEW_MESSAGE_SUBHEADER_TEXT;

        public string NewMessageAccessibilityText =>
            INFECTION_STATUS_MESSAGE_ACCESSIBILITY_TEXT + ". " + NewMessageSubheaderTxt;

        public string NewRegistrationAccessibilityText =>
            INFECTION_STATUS_REGISTRATION_HEADER_TEXT + ". " + INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;

        public string NewDiseaseRateAccessibilityText =>
            INFECTION_STATUS_DISEASE_RATE_HEADER_TEXT + ". " + LastUpdateAccessibilityString;

        /// <summary>
        ///     Show when trying to stop the scanner.
        /// </summary>
        public DialogViewModel OffDialogViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_TOGGLE_OFF_HEADER".Translate(),
            Body = "SMITTESPORING_TOGGLE_OFF_DESCRIPTION".Translate(),
            OkBtnTxt = "SMITTESPORING_TOGGLE_OFF_CONFIRM".Translate(),
            CancelbtnTxt = "SMITTESPORING_TOGGLE_OFF_CANCEL".Translate()
        };

        /// <summary>
        ///     Show when trying to start the scanner.
        /// </summary>
        public DialogViewModel OnDialogViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_TOGGLE_ON_HEADER".Translate(),
            Body = "SMITTESPORING_TOGGLE_ON_DESCRIPTION".Translate(),
            OkBtnTxt = "SMITTESPORING_TOGGLE_ON_CONFIRM".Translate(),
            CancelbtnTxt = "SMITTESPORING_TOGGLE_ON_CANCEL".Translate()
        };

        /// <summary>
        ///     Show this on iOS when user has denied access to EN-api but tries to start scanning any way.
        /// </summary>
        public DialogViewModel PermissionViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_EN_PERMISSION_DENIED_HEADER".Translate(),
            Body = "SMITTESPORING_EN_PERMISSION_DENIED_BODY".Translate(),
            OkBtnTxt = "SMITTESPORING_EN_PERMISSION_DENIED_OK_BTN".Translate()
        };

        /// <summary>
        ///     Show this when user tries to report ill while the scanner is not running.
        /// </summary>
        public DialogViewModel ReportingIllDialogViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_REPORTING_ILL_DIALOG_HEADER".Translate(),
            Body = "SMITTESPORING_REPORTING_ILL_DIALOG_BODY".Translate(),
            OkBtnTxt = "SMITTESPORING_REPORTING_ILL_DIALOG_OK_BTN".Translate()
        };

        public static async void RequestSSIUpdate(Action onFinish = null)
        {
            if (DiseaseRateUpdatedDateTime.Date != SystemTime.Now().ToLocalTime().Date)
            {
                await DiseaseRateViewModel.UpdateSSIDataAsync();
                onFinish?.Invoke();
            }
        }

        public async Task<string> StatusTxt()
        {
            return await IsRunning()
                ? INFECTION_STATUS_ACTIVE_TEXT
                : INFECTION_STATUS_INACTIVE_TEXT;
        }

        public async Task<string> StatusTxt(bool isLocationEnabled)
        {
            return await IsRunning() && isLocationEnabled
                ? INFECTION_STATUS_ACTIVE_TEXT
                : INFECTION_STATUS_INACTIVE_TEXT;
        }

        public async Task<string> StatusTxtDescription()
        {
            return await IsRunning()
                ? INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT
                : SMITTESPORING_INACTIVE_DESCRIPTION;
        }

        public async Task<string> StatusTxtDescription(bool isLocationEnabled)
        {
            return await IsRunning() && isLocationEnabled
                ? INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT
                : SMITTESPORING_INACTIVE_DESCRIPTION;
        }

        public async Task<bool> IsRunning()
        {
            if (IsAppRestricted)
            {
                return false;
            }

            try
            {
                return await ExposureNotification.GetStatusAsync() == Status.Active;
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(IsRunning)))
                {
                    throw e;
                }

                return false;
            }
        }

        public async Task<bool> IsRunning(bool isLocationEnabled)
        {
            return await IsRunning() && isLocationEnabled;
        }

        public async Task<bool> IsEnabled()
        {
            try
            {
                return await ExposureNotification.IsEnabledAsync();
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(IsEnabled)))
                {
                    throw e;
                }

                return false;
            }
        }

        public async Task<bool> StartENService()
        {
            if (IsAppRestricted)
            {
                return false;
            }

            try
            {
                await ExposureNotification.StartAsync();
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(StartENService)))
                {
                    throw e;
                }
            }

            return await IsRunning();
        }

        public async Task<bool> StopENService()
        {
            if (IsAppRestricted)
            {
                return false;
            }

            try
            {
                await ExposureNotification.StopAsync();
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(StopENService)))
                {
                    throw e;
                }
            }

            return await IsRunning();
        }

        public async void CheckIfAppIsRestricted(Action action = null)
        {
            try
            {
                if (await IsEnabled() && await IsRunning())
                {
                    await ExposureNotification.StartAsync();
                }

                IsAppRestricted = false;
            }
            catch (Exception e)
            {
                LogUtils.LogException(
                    LogSeverity.WARNING,
                    e,
                    $"{nameof(InfectionStatusViewModel)}:{nameof(CheckIfAppIsRestricted)} - Could not start EN Api. Assuming EN api is restricted");
                IsAppRestricted = true;
            }

            action?.Invoke();
        }

        private async Task NewMessagesFetched()
        {
            List<MessageItemViewModel> orderedMessages =
                MessageUtils.ToMessageItemViewModelList((await MessageUtils.GetMessages())
                    .OrderByDescending(message => message.TimeStamp)
                    .ToList());

            if (orderedMessages.Any())
            {
                _latestMessageDateTime = orderedMessages[0].TimeStamp;
            }

            List<MessageSQLiteModel> unreadMessagesResult = await MessageUtils.GetAllUnreadMessages();
            ShowNewMessageIcon = unreadMessagesResult.Any();

            NewMessagesIconVisibilityChanged?.Invoke(this, null);
        }

        public void SubscribeMessages()
        {
            MessagingCenter.Subscribe<object>(
                this,
                MessagingCenterKeys.KEY_MESSAGE_RECEIVED, async obj => { await NewMessagesFetched(); });
        }

        public async void UpdateNotificationDot()
        {
            await NewMessagesFetched();
        }
    }
}