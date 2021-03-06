using System;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public partial class WelcomePageFourViewController : PageViewController
    {
        public WelcomePageFourViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTexts();

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, PageTitle);
            BackArrow.Hidden = !LocalPreferencesHelper.IsOnboardingCompleted;
            BackArrow.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
        }

        partial void BackArrowBtn_TouchUpInside(UIButton sender)
        {
            NavigationController.PopViewController(true);
        }

        private void SetTexts()
        {
            InitTitle(PageTitle, WelcomeViewModel.WELCOME_PAGE_FOUR_TITLE);
            InitBodyText(Label1, WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_ONE);
            InitBodyText(Label2, WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_TWO);
            InitBodyText(Label3, WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_THREE);
        }
    }
}