using System;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.ViewModels.SettingsPage4ViewModel;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage4
{
    public partial class SettingsPage4ViewController : BaseViewController
    {
        public SettingsPage4ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Concatenation the content
            string content = $"{CONTENT_TEXT_BEFORE_SUPPORT_LINK}<br><br>" +
                             $"{EMAIL_TEXT}:<br><a href=\"mailto:{EMAIL}\">{EMAIL}</a><br>{PHONE_NUM_Text}:<br><a href=\"tel:{PHONE_NUM}\">{PHONE_NUM}</a><br><br>" +
                             $"{SUPPORT_TEXT}";

            ContentText.SetAttributedText(content);

            //Ensuring text is resiezed correctly when font size is increased
            HeaderLabel.SetAttributedText(HEADER);
            ContentText.SizeToFit();
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings Help", null);
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}