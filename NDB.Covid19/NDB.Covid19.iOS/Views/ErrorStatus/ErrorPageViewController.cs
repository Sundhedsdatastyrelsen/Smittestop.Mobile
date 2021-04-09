using System;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.ErrorStatus
{
    public partial class ErrorPageViewController : BaseViewController
    {
        public string ErrorMessage = "errorMessageText";
        public string ErrorSubtitle = "errorSubtitle";
        public string ErrorTitle = "errorTitle";

        public ErrorPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupStyling();
        }

        public static ErrorPageViewController Create(string errorTitle = "", string errorSubtitle = "",
            string errorMessageTxt = "")
        {
            LocalPreferencesHelper.UpdateCorrelationId(null);
            UIStoryboard storyboard = UIStoryboard.FromName("ErrorPage", null);
            ErrorPageViewController vc =
                (ErrorPageViewController) storyboard.InstantiateViewController(nameof(ErrorPageViewController));
            vc.ErrorTitle = errorTitle;
            vc.ErrorSubtitle = errorSubtitle;
            vc.ErrorMessage = errorMessageTxt;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            return vc;
        }


        private void SetupStyling()
        {
            ErrorTitleLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 24, 26);
            ErrorTitleLabel.Text = ErrorTitle;

            ErrorSubtitleLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 18);
            ErrorSubtitleLabel.Text = ErrorSubtitle;

            ErrorMessageLabel.SetAttributedText(ErrorMessage);
            ErrorMessageLabel.Editable = false;
            ErrorMessageLabel.Selectable = true;

            StyleUtil.InitButtonStyling(OkButton, ErrorViewModel.REGISTER_ERROR_DISMISS);

            BackButton.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_CLOSE_BUTTON_TEXT;

            ErrorTitleLabel.IsAccessibilityElement = ErrorTitle == "" ? false : true;
            ErrorSubtitleLabel.IsAccessibilityElement = ErrorSubtitle == "" ? false : true;
            ErrorMessageLabel.IsAccessibilityElement = ErrorMessage == "" ? false : true;

            if (ErrorTitleLabel.IsAccessibilityElement &&
                ErrorTitle == ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_HEADER)
            {
                ErrorTitleLabel.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_TOOMANYTRIES_HEADER;
            }

            if (ErrorMessageLabel.IsAccessibilityElement &&
                ErrorMessage == ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_DESCRIPTION)
            {
                ErrorMessageLabel.AccessibilityLabel =
                    ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_TOOMANYTRIES_DESCRIPTION;
            }
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            Close();
        }

        partial void DismissErrorBtn_TouchUpInside(UIButton sender)
        {
            Close();
        }

        private void Close()
        {
            if (NavigationController != null)
            {
                // BUGFIX: 99602
                // This is a special case where error on this screen has a NavigationController but should not
                // dismiss it as it is done by GoToResultPageFromAuthFlow() method.
                if (ErrorTitle.Equals(ErrorViewModel.REGISTER_ERROR_FETCH_SSI_DATA_HEADER))
                {
                    NavigationHelper.GoToResultPage(NavigationController, false);
                    return;
                }

                NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
            }
            else
            {
                LeaveController();
            }
        }
    }
}