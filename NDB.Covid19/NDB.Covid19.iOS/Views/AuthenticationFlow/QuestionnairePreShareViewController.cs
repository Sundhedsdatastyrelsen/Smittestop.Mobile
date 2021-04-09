using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.ViewModels.QuestionnairePreShareViewModel;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class QuestionnairePreShareViewController : BaseViewController
    {
        public QuestionnairePreShareViewController(IntPtr handle) : base(handle)
        {
        }

        public static QuestionnairePreShareViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("QuestionnairePreShare", null);
            QuestionnairePreShareViewController vc =
                storyboard.InstantiateInitialViewController() as QuestionnairePreShareViewController;
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
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Pre Share", null,
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
                    LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Pre Share", null,
                        GetCorrelationId());
                });
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Pre Share", null,
                        GetCorrelationId());
                });
        }

        private void SetTexts()
        {
            Title.SetAttributedText(QUESTIONNAIRE_PRE_SHARE_TITLE);
            StyleUtil.InitLabelWithHTMLFormat(Description, QUESTIONNAIRE_PRE_SHARE_DESCRIPTION);
            NextButton.SetTitle(QUESTIONNAIRE_PRE_SHARE_NEXT_BUTTON, UIControlState.Normal);
            CloseButton.AccessibilityLabel =
                QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
        }

        partial void OnNextButton_TouchUpInside(DefaultBorderButton sender)
        {
            InvokeNextButtonClick(GoToLoadingPage, OnFail);
        }

        private void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnairePreShareViewController)}.{nameof(OnFail)}: " +
                "No miba data or token expired");
        }

        private void GoToLoadingPage()
        {
            NavigationController?.PushViewController(LoadingPageViewController.Create(), true);
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

        private void GoToInfectionStatusPage()
        {
            NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
        }
    }
}