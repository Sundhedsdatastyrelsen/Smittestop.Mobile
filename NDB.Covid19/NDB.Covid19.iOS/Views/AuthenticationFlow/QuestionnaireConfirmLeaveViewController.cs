using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.ViewModels.QuestionnaireConfirmLeaveViewModel;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class QuestionnaireConfirmLeaveViewController : BaseViewController
    {
        public QuestionnaireConfirmLeaveViewController(IntPtr handle) : base(handle)
        {
        }

        public static QuestionnaireConfirmLeaveViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("QuestionnaireConfirmLeave", null);
            QuestionnaireConfirmLeaveViewController vc =
                storyboard.InstantiateInitialViewController() as QuestionnaireConfirmLeaveViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTexts();
            AddObservers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Confirm Leave", null,
                GetCorrelationId());
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        private void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Confirm Leave", null,
                        GetCorrelationId());
                });
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Confirm Leave", null,
                        GetCorrelationId());
                });
        }

        private void SetTexts()
        {
            Title.SetAttributedText(QUESTIONNAIRE_CONFIRM_LEAVE_TITLE);
            StyleUtil.InitLabelWithHTMLFormat(Description, QUESTIONNAIRE_CONFIRM_LEAVE_DESCRIPTION);
            OkButton.SetTitle(QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_OK, UIControlState.Normal);
            CancelButton.SetTitle(QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_CANCEL, UIControlState.Normal);
            CloseButton.AccessibilityLabel =
                QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
        }

        private void GoToInfectionStatusPage()
        {
            NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
        }

        private void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireConfirmLeaveViewController)}.{nameof(OnFail)}: " +
                "No miba data or token expired");
        }

        private void GoToLoadingPage()
        {
            NavigationController?.PushViewController(LoadingPageViewController.Create(), true);
        }

        partial void OnCancelButton_TouchUpInside(DefaultBorderButton sender)
        {
            LogUtils.LogMessage(
                LogSeverity.INFO,
                "The user confirmed not sharing keys",
                null,
                GetCorrelationId());
            GoToInfectionStatusPage();
        }

        partial void OnOkButton_TouchUpInside(DefaultBorderButton sender)
        {
            ValidateData(GoToLoadingPage, OnFail);
        }

        partial void OnCloseButton_TouchUpInside(UIButton sender)
        {
            DialogHelper.ShowDialog(
                this,
                CloseDialogViewModel,
                CloseConfirmed,
                UIAlertActionStyle.Destructive);
        }

        private void CloseConfirmed(UIAlertAction obj)
        {
            LogUtils.LogMessage(
                LogSeverity.INFO,
                "The user is returning to Infection Status",
                null,
                GetCorrelationId());
            GoToInfectionStatusPage();
        }
    }
}