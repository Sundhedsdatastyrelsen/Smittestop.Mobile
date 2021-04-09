using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonServiceLocator;
using CoreFoundation;
using CoreGraphics;
using MoreLinq;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Permissions;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using NDB.Covid19.iOS.Views.DiseaseRate;
using NDB.Covid19.iOS.Views.MessagePage;
using NDB.Covid19.iOS.Views.Settings;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using UserNotifications;
using static NDB.Covid19.ViewModels.InfectionStatusViewModel;


namespace NDB.Covid19.iOS.Views.InfectionStatus
{
    public partial class InfectionStatusViewController : BaseViewController
    {
        private UIButton _areYouInfectedBtn;

        private bool _comingFromOnboarding;

        private UIButton _diseaseRateViewBtn;
        private UIButton _messageViewBtn;

        private IOSPermissionManager _permissionManager;
        private PulseAnimationView _pulseAnimationView;

        private InfectionStatusViewModel _viewModel;

        public InfectionStatusViewController(IntPtr handle) : base(handle)
        {
        }

        private bool CanScrollVerticallyDown => ViewMain.Frame.Height > ScrollViewMain.Frame.Height;

        public static InfectionStatusViewController Create(bool comingFromOnboarding)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("InfectionStatus", null);
            InfectionStatusViewController vc =
                storyboard.InstantiateInitialViewController() as InfectionStatusViewController;
            vc._comingFromOnboarding = comingFromOnboarding;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public static UINavigationController GetInfectionSatusPageControllerInNavigationController()
        {
            UIViewController vc = Create(false);
            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return navigationController;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = new InfectionStatusViewModel();
            SetupStyling();
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED,
                OnMessageStatusChanged);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND,
                OnAppReturnsFromBackground);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_UPDATE_DISEASE_RATE,
                OnAppDiseaseRateChanged);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            LocalPreferencesHelper.UpdateCorrelationId(null);
            LogUtils.LogMessage(LogSeverity.INFO, "User opened InfectionStatus", null);
            UpdateUI();
            CheckAndShowScrollDownText();
        }

        public override void ViewDidUnload()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_UPDATE_DISEASE_RATE);
            base.ViewDidUnload();
        }

        private void OnAppDiseaseRateChanged(object _ = null)
        {
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                _diseaseRateViewBtn.AccessibilityLabel = _viewModel.NewDiseaseRateAccessibilityText;
                StyleUtil.InitLabelWithSpacing(NewDiseaseRateLbl, StyleUtil.FontType.FontRegular, LastUpdateString,
                    1.14, 12, 16);
            });
        }

        private void OnMessageStatusChanged(object _ = null)
        {
            InvokeOnMainThread(() => _viewModel.UpdateNotificationDot());
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetPermissionManager();
            ScrollViewMain.SetContentOffset(new CGPoint(0, 0), true);
            _viewModel.NewMessagesIconVisibilityChanged += OnNewMessagesIconVisibilityChanged;
            _diseaseRateViewBtn.TouchUpInside += OnDiseaseRateBtnTapped;
            _messageViewBtn.TouchUpInside += OnMessageBtnTapped;
            _areYouInfectedBtn.TouchUpInside += OnAreYouInfectedBtnTapped;

            OnAppReturnsFromBackground(null);

            if (_comingFromOnboarding)
            {
                StartIfStopped();
                _comingFromOnboarding = false;
            }

            _pulseAnimationView?.RestartAnimation();
        }


        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _viewModel.NewMessagesIconVisibilityChanged -= OnNewMessagesIconVisibilityChanged;
            _diseaseRateViewBtn.TouchUpInside -= OnDiseaseRateBtnTapped;
            _messageViewBtn.TouchUpInside -= OnMessageBtnTapped;
            _areYouInfectedBtn.TouchUpInside -= OnAreYouInfectedBtnTapped;
        }

        public void OnAppReturnsFromBackground(object obj)
        {
            _viewModel.CheckIfAppIsRestricted(UpdateUI);
            RequestSSIUpdate();
            UpdateUI();
            OnMessageStatusChanged();
        }

        public void SetPermissionManager()
        {
            if (_permissionManager == null)
            {
                _permissionManager = new IOSPermissionManager();
            }
        }

        public void StartIfStopped()
        {
            Task.Run(async () =>
            {
                if (await _viewModel.IsRunning() == false)
                {
                    await _viewModel.StartENService();

                    /// If EN is disabled, then the OS dialog will pop up.
                    /// Calling the StartENService method again seems to make the EN api properly update its state, before we update our UI.
                    /// This is not an optimal solution, but seems to work fine as a workaround for now.
                    /// A better way would be for us to be able to listen to state changes in the EN api.
                    if (await _viewModel.IsEnabled() == false)
                    {
                        await _viewModel.StartENService();
                    }

                    if (await _permissionManager.PoweredOff())
                    {
                        DialogHelper.ShowBluetoothTurnedOffDialog(this);
                    }

                    UpdateUI();
                }
            });
        }

        public void UpdateUI()
        {
            InvokeOnMainThread(async () =>
            {
                _diseaseRateViewBtn.AccessibilityLabel = _viewModel.NewDiseaseRateAccessibilityText;
                _areYouInfectedBtn.AccessibilityLabel = _viewModel.NewRegistrationAccessibilityText;
                StyleUtil.InitLabelWithSpacing(NewDiseaseRateLbl, StyleUtil.FontType.FontRegular, LastUpdateString,
                    1.14, 12, 16);

                _messageViewBtn.AccessibilityAttributedLabel =
                    AccessibilityUtils.RemovePoorlySpokenSymbols(_viewModel.NewMessageAccessibilityText);
                ActivityStatusLbl.Text = await _viewModel.StatusTxt();
                string statusTxtDescription = await _viewModel.StatusTxtDescription();
                ActivityExplainerLbl.Text = statusTxtDescription;
                ActivityExplainerLbl.AccessibilityAttributedLabel =
                    AccessibilityUtils.RemovePoorlySpokenSymbols(statusTxtDescription);
                SetOnOffBtnState(await _viewModel.IsRunning());
                UpdateNewIndicatorView();
            });
        }

        public void SetOnOffBtnState(bool isRunning)
        {
            if (_pulseAnimationView == null)
            {
                CreatePulseAnimation(OnOffBtn);
            }

            if (isRunning)
            {
                OnOffBtn.BackgroundColor = UIColor.FromRGB(86, 197, 104);
                OnOffBtn.SetImage(UIImage.FromBundle("pauseIcon"), UIControlState.Normal);
                string text = INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT;
                OnOffBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(text);
                OnOffBtn.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);
                _pulseAnimationView.Hidden = false;
            }
            else
            {
                OnOffBtn.BackgroundColor = UIColor.FromRGB(235, 87, 87);
                OnOffBtn.SetImage(UIImage.FromBundle("playIcon"), UIControlState.Normal);
                string text = INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT;
                OnOffBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(text);
                OnOffBtn.ImageEdgeInsets = new UIEdgeInsets(0, 5, 0, 0);
                _pulseAnimationView.Hidden = true;
            }
        }

        public void CreatePulseAnimation(UIView view)
        {
            _pulseAnimationView = new PulseAnimationView();
            OnOffBtnContainer.InsertSubview(_pulseAnimationView, 0);
            _pulseAnimationView.TranslatesAutoresizingMaskIntoConstraints = false;
            _pulseAnimationView.TopAnchor.ConstraintEqualTo(view.TopAnchor).Active = true;
            _pulseAnimationView.BottomAnchor.ConstraintEqualTo(view.BottomAnchor).Active = true;
            _pulseAnimationView.LeadingAnchor.ConstraintEqualTo(view.LeadingAnchor).Active = true;
            _pulseAnimationView.TrailingAnchor.ConstraintEqualTo(view.TrailingAnchor).Active = true;
            _pulseAnimationView.BackgroundColor = UIColor.FromRGB(86, 197, 104);
        }

        private void SetupStyling()
        {
            ScrollDownLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 12, 16);
            ScrollDownLbl.Text = SCROLL_DOWN_HEADER_TEXT;
            ScrollView.BackgroundColor = UIColor.FromRGB(255, 255, 255);
            ScrollView.Layer.CornerRadius = 12;
            NewIndicatorView.Layer.CornerRadius = NewIndicatorView.Layer.Frame.Height / 2;

            OnOffBtn.Layer.CornerRadius = OnOffBtn.Layer.Frame.Width / 2;

            ActivityStatusLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 17, 24);

            ActivityExplainerLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 20);
            ActivityExplainerLbl.Text = INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            MenuIcon.AccessibilityLabel = INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            SetupEncounterAndInfectedButtons();
        }

        private void CheckAndShowScrollDownText()
        {
            ScrollViewMain.Delegate = new CustomScrollViewDelegate(ScrollView);
            ScrollView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                CGPoint bottomOffset = new CGPoint(0,
                    ScrollViewMain.ContentSize.Height - ScrollViewMain.Bounds.Height +
                    ScrollViewMain.ContentInset.Bottom);
                ScrollViewMain.SetContentOffset(bottomOffset, true);
            }));
            if (CanScrollVerticallyDown && !LocalPreferencesHelper.IsScrollDownShown)
            {
                ScrollView.Hidden = false;
            }
        }

        public void SetupEncounterAndInfectedButtons()
        {
            DiseaseRateView.Subviews[0].Layer.CornerRadius = 12;
            MessageView.Subviews[0].Layer.CornerRadius = 12;
            AreYouInfectetView.Subviews[0].Layer.CornerRadius = 12;

            DiseaseRateLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 20);
            DiseaseRateLbl.Text = INFECTION_STATUS_DISEASE_RATE_HEADER_TEXT;
            StyleUtil.InitLabelWithSpacing(NewDiseaseRateLbl, StyleUtil.FontType.FontRegular, LastUpdateString, 1.14,
                12, 16);

            MessageLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 20);
            MessageLbl.Text = INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            NewRegistrationLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 12, 16);
            NewRegistrationLbl.Text = _viewModel.NewMessageSubheaderTxt;

            AreYouInfectetLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 20);
            AreYouInfectetLbl.Text = INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            LogInAndRegisterLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 12, 16);
            LogInAndRegisterLbl.Text = INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;

            // Default image for ImageViews is available only on iOS 13 and above so we set
            // image views on iOS 12.5 to the ChevronRight from the assets.
            if (AppDelegate.ShouldOperateIn12_5Mode)
            {
                DiseaseRate_ChevronImageView.Image = UIImage.FromBundle("ChevronRight");
                DiseaseRate_ChevronImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                Message_ChevronImageView.Image = UIImage.FromBundle("ChevronRight");
                Message_ChevronImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                AreYouInfected_ChevronImageView.Image = UIImage.FromBundle("ChevronRight");
                AreYouInfected_ChevronImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            }

            // We take the fairly complicated UIViews from the storyboard and embed them into UIButtons
            _diseaseRateViewBtn = new UIButton();
            _diseaseRateViewBtn.TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.EmbedViewInsideButton(DiseaseRateView, _diseaseRateViewBtn);

            _messageViewBtn = new UIButton();
            _messageViewBtn.TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.EmbedViewInsideButton(MessageView, _messageViewBtn);

            _areYouInfectedBtn = new UIButton();
            _areYouInfectedBtn.TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.EmbedViewInsideButton(AreYouInfectetView, _areYouInfectedBtn);
        }

        public void UpdateNewIndicatorView()
        {
            InvokeOnMainThread(() =>
            {
                NewIndicatorView.Hidden = !_viewModel.ShowNewMessageIcon;

                UIApplication.SharedApplication.ApplicationIconBadgeNumber = NewIndicatorView.Hidden ? 0 : 1;

                NewRegistrationLbl.Text = _viewModel.NewMessageSubheaderTxt;
                _messageViewBtn.AccessibilityLabel =
                    AccessibilityUtils.RemovePoorlySpokenSymbolsString(_viewModel.NewMessageAccessibilityText);
            });
        }

        public void OnNewMessagesIconVisibilityChanged(object sender, EventArgs e)
        {
            UpdateNewIndicatorView();
        }

        partial void OnMenubtnTapped(UIButton sender)
        {
            UIViewController vc = SettingsViewController.Create();
            NavigationController?.PushViewController(vc, true);
        }


        async partial void OnOffBtnTapped(UIButton sender)
        {
            if (_viewModel.IsAppRestricted)
            {
                DialogHelper.ShowDialog(
                    this,
                    _viewModel.PermissionViewModel,
                    action => { NavigationHelper.GoToAppSettings(); }
                );
                LogUtils.LogMessage(LogSeverity.WARNING, "EN api is restricted. Cannot start.");
                return;
            }

            if (await _viewModel.IsRunning() && await _viewModel.IsEnabled())
            {
                DialogHelper.ShowDialog(
                    this,
                    _viewModel.OffDialogViewModel,
                    action => ShowPickerController());
            }
            else
            {
                if (await _permissionManager.PoweredOn())
                {
                    DialogHelper.ShowDialog(
                        this,
                        _viewModel.OnDialogViewModel,
                        OnStartScannerChosen
                    );
                }
                else if (await _permissionManager.PermissionUnknown())
                {
                    // We do nothing. The OS will throw a dialog by itself
                    Debug.WriteLine("GetStatusAsync() == Status.Unknown");
                }
                else if (await _permissionManager.PoweredOff())
                {
                    DialogHelper.ShowBluetoothTurnedOffDialog(this);
                }
                else
                {
                    LogUtils.LogMessage(
                        LogSeverity.WARNING,
                        "Unknown error during app starting. Assuming permissions issue.");
                    // Then it must be because we don't have permission
                    DialogHelper.ShowDialog(
                        this,
                        _viewModel.PermissionViewModel,
                        action => { NavigationHelper.GoToAppSettings(); });
                }
            }
        }

        public void OnStartScannerChosen(UIAlertAction obj)
        {
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(
                new[] {NotificationsEnum.TimedReminderFinished.ToString()});
            // If dialog is confirmed start exposure notifications through this async method: _viewModel.StartEN(); 
            StartIfStopped();
            UpdateUI();
        }

        public async void OnStopScannerChosen()
        {
            // If dialog is dismissed stop exposure notifications through this async method: _viewModel.StopEN(); 
            await _viewModel.StopENService();
            UpdateUI();
        }

        public void OnMessageBtnTapped(object sender, EventArgs e)
        {
            OpenMessagesPage();
        }

        public void OnDiseaseRateBtnTapped(object sender, EventArgs e)
        {
            OpenDiseaseRatePage();
        }

        public async void OnAreYouInfectedBtnTapped(object sender, EventArgs e)
        {
            if (await _viewModel.IsRunning())
            {
                UINavigationController navigationController =
                    new UINavigationController(InformationAndConsentViewController
                        .GetInformationAndConsentViewController());
                navigationController.SetNavigationBarHidden(true, false);
                navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                PresentViewController(navigationController, true, null);
            }
            else
            {
                DialogHelper.ShowDialog(this, _viewModel.ReportingIllDialogViewModel, null);
            }
        }

        public void OpenMessagesPage()
        {
            UINavigationController vc = MessagePageViewController.GetMessagePageControllerInNavigationController();
            PresentViewController(vc, true, null);
        }

        public void OpenDiseaseRatePage()
        {
            NavigationController?.PushViewController(DiseaseRateLoadingPageViewController.Create(), true);
        }

        private void SetRecursiveInteraction(UIView parentView, bool isEnabled)
        {
            InvokeOnMainThread(() =>
            {
                parentView?.Subviews.ForEach(view =>
                {
                    if (!view.Equals(SpinnerMainView) ||
                        !view.Equals(Picker) ||
                        !view.Equals(SpinnerDialogTitle) ||
                        !view.Equals(SpinnerDialogMessage) ||
                        !view.Equals(SpinnerDialogButton))
                    {
                        view.AccessibilityElementsHidden = !isEnabled;
                        view.UserInteractionEnabled = isEnabled;
                    }
                });
            });
        }

        private void ShowPickerController()
        {
            SetRecursiveInteraction(View, false);
            // Make sure accessibility is enabled for the whole view
            SpinnerMainView.AccessibilityElementsHidden = false;
            SpinnerMainView.UserInteractionEnabled = true;

            // Disable accessibility for the subviews to prevent focus stealing
            SpinnerDialogButton.AccessibilityElementsHidden = true;
            SpinnerDialogButton.UserInteractionEnabled = false;
            Picker.AccessibilityElementsHidden = true;
            Picker.UserInteractionEnabled = false;
            SpinnerDialogTitle.AccessibilityElementsHidden = true;
            SpinnerDialogTitle.UserInteractionEnabled = false;
            SpinnerDialogMessage.AccessibilityElementsHidden = true;
            SpinnerDialogMessage.UserInteractionEnabled = false;

            StyleUtil.InitLabelWithSpacing(
                SpinnerDialogTitle,
                StyleUtil.FontType.FontBold,
                INFECTION_STATUS_SPINNER_DIALOG_TITLE,
                1.14,
                24,
                38,
                UITextAlignment.Center);
            StyleUtil.InitLabelWithSpacing(
                SpinnerDialogMessage,
                StyleUtil.FontType.FontRegular,
                INFECTION_STATUS_SPINNER_DIALOG_MESSAGE,
                1.28,
                16,
                20,
                UITextAlignment.Center);
            SpinnerDialogButton.SetTitle(
                INFECTION_STATUS_SPINNER_DIALOG_OK_BUTTON,
                UIControlState.Normal);

            SpinnerDialogTitle.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(INFECTION_STATUS_SPINNER_DIALOG_TITLE);
            SpinnerDialogMessage.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(INFECTION_STATUS_SPINNER_DIALOG_MESSAGE);
            SpinnerDialogButton.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(INFECTION_STATUS_SPINNER_DIALOG_OK_BUTTON);

            Picker.Model = new HoursPickerModel(
                new List<string>
                {
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_NO_REMINDER,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_ONE_HOUR,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWO_HOURS,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_FOUR_HOURS,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_EIGHT_HOURS,
                    INFECTION_STATUS_SPINNER_DIALOG_OPTION_TWELVE_HOURS
                });

            SpinnerMainView.Hidden = false;
            SetFocusTo(SpinnerDialogTitle, () =>
            {
                SpinnerDialogButton.AccessibilityElementsHidden = false;
                SpinnerDialogButton.UserInteractionEnabled = true;
                Picker.AccessibilityElementsHidden = false;
                Picker.UserInteractionEnabled = true;
                SpinnerDialogTitle.AccessibilityElementsHidden = false;
                SpinnerDialogTitle.UserInteractionEnabled = true;
                SpinnerDialogMessage.AccessibilityElementsHidden = false;
                SpinnerDialogMessage.UserInteractionEnabled = true;
            });
        }

        private void SetFocusTo(UIView view, Action onAccessibilityPostFinished = null)
        {
            DispatchQueue.MainQueue.DispatchAfter(
                new DispatchTime(DispatchTime.Now, 10000000L),
                () =>
                {
                    UIAccessibility.PostNotification(
                        UIAccessibilityPostNotification.LayoutChanged,
                        view);
                    onAccessibilityPostFinished?.Invoke();
                });
        }

        partial void OnSpinnerDialogButton_TouchUpInside(UIButton sender)
        {
            InvokeOnMainThread(() =>
            {
                SpinnerMainView.Hidden = true;
                switch (HoursPickerModel.SelectedOptionByUser)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        DisplayNotificationAfterTime((int) Math.Pow(2, HoursPickerModel.SelectedOptionByUser - 1));
                        break;
                    case 5:
                        DisplayNotificationAfterTime(12);
                        break;
                }

                OnStopScannerChosen();
                SetRecursiveInteraction(View, true);
                SetFocusTo(View);
                SpinnerMainView.AccessibilityElementsHidden = true;
            });
        }

        private void DisplayNotificationAfterTime(int hourMultiplier)
        {
            long ticks = 60 * 60 * hourMultiplier;
#if DEBUG
            ticks /= 60;
#endif
            ServiceLocator.Current
                .GetInstance<ILocalNotificationsManager>()
                .GenerateDelayedNotification(
                    NotificationsEnum.TimedReminderFinished.Data(),
                    ticks);
        }

        private class CustomScrollViewDelegate : UIScrollViewDelegate
        {
            private readonly UIView _button;

            public CustomScrollViewDelegate(UIView button)
            {
                _button = button;
            }

            public override void Scrolled(UIScrollView scrollView)
            {
                LocalPreferencesHelper.IsScrollDownShown = !(scrollView.ContentOffset.Y >=
                                                             scrollView.ContentSize.Height -
                                                             scrollView.Frame.Size.Height);
                _button.Hidden = !LocalPreferencesHelper.IsScrollDownShown;
            }
        }
    }
}