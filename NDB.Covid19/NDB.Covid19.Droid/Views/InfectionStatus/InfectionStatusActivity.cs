using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using CommonServiceLocator;
using NDB.Covid19.Droid.Services;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.AuthenticationFlow;
using NDB.Covid19.Droid.Views.Messages;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using Xamarin.ExposureNotifications;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.InfectionStatusViewModel;
using AlertDialog = Android.App.AlertDialog;
using Object = Java.Lang.Object;

namespace NDB.Covid19.Droid.Views.InfectionStatus
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class InfectionStatusActivity : AppCompatActivity
    {
        private readonly PermissionUtils _permissionUtils = ServiceLocator.Current.GetInstance<PermissionUtils>();
        private TextView _activityStatusDescription;
        private TextView _activityStatusText;
        private ImageView _buttonBackgroundAnimated;

        private bool _dialogDisplayed;
        private Button _diseaseRateCoverButton;
        private TextView _diseaseRateHeader;
        private TextView _diseaseRateLastUpdated;
        private ImageButton _menuIcon;
        private Button _messageCoverButton;
        private RelativeLayout _messageRelativeLayout;
        private TextView _messageSubHeader;
        private TextView _messeageHeader;
        private ImageView _notificationDot;
        private ImageButton _onOffButton;
        private NumberPicker _picker;
        private Button _registrationCoverButton;
        private TextView _registrationHeader;
        private RelativeLayout _registrationRelativeLayout;
        private TextView _registrationSubheader;
        private ConstraintLayout _scrollDown;
        private TextView _scrollDownHeader;
        private ScrollView _scrollView;
        private InfectionStatusViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = INFECTION_STATUS_PAGE_TITLE;
            SetContentView(Resource.Layout.infection_status);
            _viewModel = new InfectionStatusViewModel();
            InitLayout();
            UpdateMessagesStatus();
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED,
                OnMessageStatusChanged);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_UPDATE_DISEASE_RATE,
                OnAppDiseaseRateChanged);
        }

        private void OnAppDiseaseRateChanged(object _ = null)
        {
            RunOnUiThread(() =>
            {
                _diseaseRateLastUpdated.Text = LastUpdateString;
                _diseaseRateCoverButton.ContentDescription =
                    $"{INFECTION_STATUS_DISEASE_RATE_HEADER_TEXT} {LastUpdateAccessibilityString}";
            });
        }

        protected override void OnDestroy()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_UPDATE_DISEASE_RATE);
            base.OnDestroy();
        }

        private void OnMessageStatusChanged(object _ = null)
        {
            RunOnUiThread(() => _viewModel.UpdateNotificationDot());
        }

        protected override void OnResume()
        {
            base.OnResume();
            LocalPreferencesHelper.UpdateCorrelationId(null);
            LogUtils.LogMessage(LogSeverity.INFO, "User opened InfectionStatus", null);
            RequestSSIUpdate();
            _scrollView?.ScrollTo(0, 0);
            _permissionUtils.SubscribePermissionsMessagingCenter(this,
                o => PreventMultiplePermissionsDialogsForAction(_permissionUtils.HasPermissions));

            ShowPermissionsDialogIfTheyHavChangedWhileInIdle();

            UpdateUI();
            _viewModel.NewMessagesIconVisibilityChanged += OnNewMessagesIconVisibilityChanged;

            OnMessageStatusChanged();
        }

        private void ShowPermissionsDialogIfTheyHavChangedWhileInIdle()
        {
            RunOnUiThread(() =>
                PreventMultiplePermissionsDialogsForAction(_permissionUtils.CheckPermissionsIfChangedWhileIdle));
        }

        private async void PreventMultiplePermissionsDialogsForAction(Func<Task<bool>> action)
        {
            bool isRunning = await _viewModel.IsRunning();
            if ((!isRunning || !await _permissionUtils.HasPermissionsWithoutDialogs())
                && _dialogDisplayed == false)
            {
                _dialogDisplayed = true;
                if (action != null) await action.Invoke();
                _dialogDisplayed = false;
                // wait until BT state change will be completed
                await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI);
            }

            UpdateUI();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _permissionUtils.UnsubscribePErmissionsMessagingCenter(this);
            _viewModel.NewMessagesIconVisibilityChanged -= OnNewMessagesIconVisibilityChanged;
        }

        private async void InitLayout()
        {
            //TextViews
            _activityStatusText = FindViewById<TextView>(Resource.Id.infection_status_activity_status_textView);
            _activityStatusDescription =
                FindViewById<TextView>(Resource.Id.infection_status_activivity_status_description_textView);
            _diseaseRateHeader = FindViewById<TextView>(Resource.Id.infection_status_disease_rate_text_textView);
            _diseaseRateLastUpdated =
                FindViewById<TextView>(Resource.Id.infection_status_new_disease_rate_text_textView);
            _messeageHeader = FindViewById<TextView>(Resource.Id.infection_status_message_text_textView);
            _messageSubHeader = FindViewById<TextView>(Resource.Id.infection_status_new_message_text_textView);
            _registrationHeader = FindViewById<TextView>(Resource.Id.infection_status_registration_text_textView);
            _registrationSubheader =
                FindViewById<TextView>(Resource.Id.infection_status_registration_login_text_textView);
            _scrollDownHeader =
                FindViewById<TextView>(Resource.Id.infection_status_down_text);

            //Buttons
            _onOffButton = FindViewById<ImageButton>(Resource.Id.infection_status_on_off_button);
            _messageRelativeLayout =
                FindViewById<RelativeLayout>(Resource.Id.infection_status_messages_button_relativeLayout);
            _registrationRelativeLayout =
                FindViewById<RelativeLayout>(Resource.Id.infection_status_registration_button_relativeLayout);
            _menuIcon = FindViewById<ImageButton>(Resource.Id.infection_status_menu_icon_relativeLayout);
            _diseaseRateCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_disease_rate_button_relativeLayout_button);
            _messageCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_messages_button_relativeLayout_button);
            _registrationCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_registration_button_relativeLayout_button);

            //ImageViews
            _notificationDot = FindViewById<ImageView>(Resource.Id.infection_status_message_new_message_imageView);

            //Text initialization
            _activityStatusText.Text = INFECTION_STATUS_ACTIVE_TEXT;
            _activityStatusDescription.Text = INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            _diseaseRateHeader.Text = INFECTION_STATUS_DISEASE_RATE_HEADER_TEXT;
            _diseaseRateLastUpdated.Text = LastUpdateString;
            _messeageHeader.Text = INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            _messageSubHeader.Text = INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT;
            _registrationHeader.Text = INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            _registrationSubheader.Text = INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;
            _scrollDownHeader.Text = SCROLL_DOWN_HEADER_TEXT;

            //Accessibility
            _menuIcon.ContentDescription = INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            _notificationDot.ContentDescription = INFECTION_STATUS_NEW_MESSAGE_NOTIFICATION_DOT_ACCESSIBILITY_TEXT;
            _diseaseRateCoverButton.ContentDescription =
                $"{INFECTION_STATUS_DISEASE_RATE_HEADER_TEXT} {LastUpdateAccessibilityString}";
            _messageCoverButton.ContentDescription =
                $"{INFECTION_STATUS_MESSAGE_HEADER_TEXT} {INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT}";
            _registrationCoverButton.ContentDescription =
                $"{INFECTION_STATUS_REGISTRATION_HEADER_TEXT} {INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT}";

            //Button click events
            _onOffButton.Click += new SingleClick(StartStopButton_Click, 500).Run;
            _messageRelativeLayout.Click += new SingleClick(MessageLayoutButton_Click, 500).Run;
            _diseaseRateCoverButton.Click += new SingleClick(GoToLoadingPage, 500).Run;
            _messageCoverButton.Click += new SingleClick(MessageLayoutButton_Click, 500).Run;
            _registrationRelativeLayout.Click += new SingleClick(RegistrationLayoutButton_Click, 500).Run;
            _registrationCoverButton.Click += new SingleClick(RegistrationLayoutButton_Click, 500).Run;
            _menuIcon.Click += new SingleClick((sender, e) => NavigationHelper.GoToSettingsPage(this), 500).Run;

            _buttonBackgroundAnimated = FindViewById<ImageView>(Resource.Id.infection_status_background);
            if (!await _viewModel.IsRunning())
            {
                _onOffButton.PerformClick();
            }

            _scrollView = FindViewById<ScrollView>(Resource.Id.infection_status_scrollView);
            _scrollDown = FindViewById<ConstraintLayout>(Resource.Id.infection_status_scroll_down_layout);
            _scrollDown.Click += (sender, args) => _scrollView.FullScroll(FocusSearchDirection.Down);
            _scrollView?.ViewTreeObserver?.AddOnScrollChangedListener(new OnScrollListener(this, _scrollDown));
            _scrollView?.ViewTreeObserver?.AddOnDrawListener(new OnDrawScrollView(this));
            UpdateUI();
            FlightModeHandlerBroadcastReceiver.OnFlightModeChange += UpdateUI;
        }

        private void CheckAndShowScrollDownText()
        {
            RunOnUiThread(() =>
            {
                bool canScrollVerticallyDown = _scrollView.CanScrollVertically(1);
                bool isScrollDownShown = IsScrollDownShown;
                if (canScrollVerticallyDown && !isScrollDownShown)
                {
                    _scrollDown.Visibility = ViewStates.Visible;
                }
            });
        }

        private void CreatePulseAnimation(ImageView buttonBackgroundAnimated, bool isRunning)
        {
            RunOnUiThread(() =>
            {
                if (isRunning)
                {
                    Animation animation = AnimationUtils.LoadAnimation(this, Resource.Animation.background_circle_anim);
                    buttonBackgroundAnimated.Visibility = ViewStates.Visible;
                    buttonBackgroundAnimated.StartAnimation(animation);
                }
                else
                {
                    buttonBackgroundAnimated.Visibility = ViewStates.Invisible;
                    buttonBackgroundAnimated.ClearAnimation();
                }
            });
        }

        private void UpdateUI()
        {
            RunOnUiThread(async () =>
            {
                bool isLocationEnabled = await _permissionUtils.IsLocationEnabled();
                bool isRunning = await _viewModel.IsRunning(isLocationEnabled);
                _diseaseRateLastUpdated.Text = LastUpdateString;
                _activityStatusText.Text = await _viewModel.StatusTxt(isLocationEnabled);
                _activityStatusDescription.Text = await _viewModel.StatusTxtDescription(isLocationEnabled);
                _onOffButton.SetBackgroundResource(isRunning
                    ? Resource.Drawable.infection_status_on_off_button_green
                    : Resource.Drawable.Infection_status_on_off_button_red);
                _onOffButton.SetImageResource(isRunning ? Resource.Drawable.ic_pause : Resource.Drawable.ic_play);
                _onOffButton.ContentDescription = isRunning
                    ? INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT
                    : INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT;
                CreatePulseAnimation(_buttonBackgroundAnimated, isRunning);
            });
        }

        private void UpdateMessagesStatus()
        {
            RunOnUiThread(() =>
            {
                _notificationDot.Visibility = _viewModel.ShowNewMessageIcon ? ViewStates.Visible : ViewStates.Gone;
                _messageSubHeader.Text = _viewModel.NewMessageSubheaderTxt;
                _messageCoverButton.ContentDescription =
                    $"{INFECTION_STATUS_MESSAGE_HEADER_TEXT} {_viewModel.NewMessageSubheaderTxt}";
            });
        }

        private void OnNewMessagesIconVisibilityChanged(object sender, EventArgs e)
        {
            UpdateMessagesStatus();
        }

        private async void StartStopButton_Click(object sender, EventArgs e)
        {
            bool isRunning = await _viewModel.IsRunning();
            bool hasPermissionsWithoutDialogs = await _permissionUtils.HasPermissionsWithoutDialogs();

            if (isRunning && !hasPermissionsWithoutDialogs)
            {
                _ = await _permissionUtils.HasPermissions();
                return;
            }

            if (isRunning)
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.OffDialogViewModel,
                    ShowSpinnerDialog);
            }
            else
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.OnDialogViewModel,
                    StartGoogleAPI);
            }

            UpdateUI();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                ShowPermissionsDialogIfTheyHavChangedWhileInIdle();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                if (resultCode == Result.Ok)
                {
                    ExposureNotification.OnActivityResult(requestCode, resultCode, data);
                }
            }
            catch (Exception e)
            {
                _ = e.HandleExposureNotificationException(nameof(InfectionStatusActivity), nameof(OnActivityResult));
            }
            finally
            {
                _permissionUtils.OnActivityResult(requestCode, resultCode, data);
                UpdateUI();
            }
        }

        private async void StartGoogleAPI()
        {
            try
            {
                CloseReminderNotifications();
                await _viewModel.StartENService();
                bool isRunning = await _viewModel.IsRunning();
                if (isRunning)
                {
                    BackgroundFetchScheduler.ScheduleBackgroundFetch();
                }

                if (await _viewModel.IsEnabled() &&
                    !await _viewModel.IsRunning() &&
                    await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI) == BluetoothState.OFF)
                {
                    await _permissionUtils.HasPermissions();
                    // wait until BT state change will be completed
                    await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI);
                }
            }
            finally
            {
                UpdateUI();
            }
        }

        private async void StopGoogleAPI()
        {
            try
            {
                await _viewModel.StopENService();
            }
            finally
            {
                UpdateUI();
            }
        }

        private void MessageLayoutButton_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(MessagesActivity)));
        }

        private void ShowSpinnerDialog()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.spinner_dialog, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetCancelable(false);
            builder.SetTitle(INFECTION_STATUS_SPINNER_DIALOG_TITLE);
            builder.SetMessage(INFECTION_STATUS_SPINNER_DIALOG_MESSAGE);

            _picker = view.FindViewById(Resource.Id.picker) as NumberPicker;
            _picker.MaxValue = 5;
            _picker.MinValue = 0;
            _picker.DescendantFocusability = DescendantFocusability.BlockDescendants;
            _picker.SetDisplayedValues(
                new[]
                {
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_NO_REMINDER,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_ONE_HOUR,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWO_HOURS,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_FOUR_HOURS,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_EIGHT_HOURS,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWELVE_HOURS
                });

            builder.SetPositiveButton(
                INFECTION_STATUS_SPINNER_DIALOG_OK_BUTTON,
                (sender, args) =>
                {
                    switch (_picker.Value)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            StartReminderService((int) Math.Pow(2, _picker.Value - 1));
                            break;
                        case 5:
                            StartReminderService(12);
                            break;
                    }

                    StopGoogleAPI();
                    (sender as AlertDialog)?.Dismiss();
                });

            builder.SetView(view);
            builder.Create()?.Show();
        }

        private void CloseReminderNotifications()
        {
            NotificationManager notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationManager?.Cancel((int) NotificationsEnum.TimedReminderFinished);
            StopReminderService();
        }

        private void StartReminderService(long hourMultiplier)
        {
            Bundle bundle = new Bundle();
            long ticks = 1000 * 60 * 60 * hourMultiplier;
#if DEBUG
            ticks /= 60;
#endif
            bundle.PutLong("ticks", ticks);
            ForegroundServiceHelper
                .StartForegroundServiceCompat<TimedReminderForegroundService>(this, bundle);
        }

        private void StopReminderService()
        {
            ForegroundServiceHelper
                .StopForegroundServiceCompat<TimedReminderForegroundService>(this);
        }

        private async void RegistrationLayoutButton_Click(object sender, EventArgs e)
        {
            if (!await _viewModel.IsRunning(await _permissionUtils.IsLocationEnabled()))
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.ReportingIllDialogViewModel);
                return;
            }

            Intent intent = new Intent(this, typeof(InformationAndConsentActivity));
            StartActivity(intent);
        }

        private void GoToLoadingPage(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(LoadingPageDiseaseRateActivity)));
        }

        private class OnDrawScrollView : Object, ViewTreeObserver.IOnDrawListener
        {
            private readonly InfectionStatusActivity _self;

            public OnDrawScrollView(InfectionStatusActivity self)
            {
                _self = self;
            }

            public void OnDraw()
            {
                _self.CheckAndShowScrollDownText();
            }
        }

        private class OnScrollListener : Object, ViewTreeObserver.IOnScrollChangedListener
        {
            private readonly ConstraintLayout _layout;
            private readonly ScrollView _scrollView;
            private readonly Activity _self;

            public OnScrollListener(Activity self, ConstraintLayout layout)
            {
                _self = self;
                _layout = layout;
                _scrollView = self.FindViewById<ScrollView>(Resource.Id.infection_status_scrollView);
            }

            public void OnScrollChanged()
            {
                _self.RunOnUiThread(() =>
                {
                    if (!_scrollView.CanScrollVertically(1))
                    {
                        _layout.Visibility = ViewStates.Gone;
                        IsScrollDownShown = false;
                    }
                });
            }
        }
    }
}