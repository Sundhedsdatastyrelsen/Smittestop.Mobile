using System;
using CoreGraphics;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage3
{
    public partial class SettingsPage3ViewController : BaseViewController
    {
        private UIButton _privacyPolicyButton;

        private ConsentViewModel _vm;

        public SettingsPage3ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _vm = new ConsentViewModel();
            InitPrivacyPolicyButton();
            InitLabels();
            SetStyling();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings Withdraw Consents", null);
        }

        private void InitLabels()
        {
            ConsentHelper.SetConsentLabels(LabelStackView, _vm.GetConsentSectionsTexts(), _privacyPolicyButton);
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
            TitleStackView.BottomAnchor.ConstraintEqualTo(ScrollView.TopAnchor, -33).Active = true;
        }

        private void SetStyling()
        {
            DeleteBtnWidthConstraint.Active = false;
            StyleUtil.InitButtonStyling(DeleteConsentBtn, ConsentViewModel.WITHDRAW_CONSENT_BUTTON_TEXT);
            PageTitle.SetAttributedText(ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE);
            StyleUtil.InitButtonStyling(_privacyPolicyButton, ConsentViewModel.CONSENT_SEVEN_BUTTON_TEXT);
        }

        private void InitPrivacyPolicyButton()
        {
            _privacyPolicyButton =
                new UIButton(new CGRect(0, 0, 0,
                    50)); // The frame should not be needed here, but it is since the cornerRadius in StyleUtil is set only once, not dynamically updated on redraw.
            _privacyPolicyButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _privacyPolicyButton.HeightAnchor.ConstraintEqualTo(50).Active = true;
            _privacyPolicyButton.TouchUpInside += OnPrivacyPolicyPressed;
        }

        private void OnPrivacyPolicyPressed(object sender, EventArgs eventArgs)
        {
            ConsentViewModel.OpenPrivacyPolicyLink();
        }


        partial void DeleteConsentBtn_TouchUpInside(DefaultBorderButton sender)
        {
            CreateDeleteWarning();
        }

        private void CreateDeleteWarning()
        {
            UIAlertController controller = UIAlertController.Create(
                ConsentViewModel.CONSENT_REMOVE_TITLE,
                ConsentViewModel.CONSENT_REMOVE_MESSAGE,
                UIAlertControllerStyle.Alert);
            controller.AddAction(UIAlertAction.Create(ConsentViewModel.CONSENT_OK_BUTTON_TEXT,
                UIAlertActionStyle.Destructive, DeleteWarningOKBtnTapped));
            controller.AddAction(UIAlertAction.Create(ConsentViewModel.CONSENT_NO_BUTTON_TEXT,
                UIAlertActionStyle.Cancel, null));
            PresentViewController(controller, true, null);
        }

        private void DeleteWarningOKBtnTapped(UIAlertAction obj)
        {
            DeleteBtnWidthConstraint.Constant = DeleteConsentBtn.Frame.Width;
            DeleteBtnWidthConstraint.Active = true;
            DeleteConsentBtn.SetTitle(string.Empty, UIControlState.Normal);

            UIActivityIndicatorView spinner = ShowSpinner();

            DeviceUtils.StopScanServices();
            DeviceUtils.CleanDataFromDevice();
            spinner.StopAnimating();
            spinner.RemoveFromSuperview();
            DeleteConsentBtn.SetTitle(ConsentViewModel.WITHDRAW_CONSENT_BUTTON_TEXT, UIControlState.Normal);
            DeleteBtnWidthConstraint.Active = false;

            // Show a dialog without any buttons, that way forcing the user to quit the app.
            UIAlertController controller = UIAlertController.Create(
                ConsentViewModel.WITHDRAW_CONSENT_SUCCESS_TITLE,
                ConsentViewModel.WITHDRAW_CONSENT_SUCCESS_TEXT,
                UIAlertControllerStyle.Alert);
            PresentViewController(controller, true, null);
        }

        private UIActivityIndicatorView ShowSpinner()
        {
            UIActivityIndicatorView spinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            spinner.HidesWhenStopped = true;
            spinner.TranslatesAutoresizingMaskIntoConstraints = false;

            View.Add(spinner);

            // Pin the spinner to the center of the DeleteConsentBtn
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                spinner.CenterXAnchor.ConstraintEqualTo(DeleteConsentBtn.CenterXAnchor),
                spinner.CenterYAnchor.ConstraintEqualTo(DeleteConsentBtn.CenterYAnchor)
            });

            spinner.StartAnimating();

            return spinner;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}