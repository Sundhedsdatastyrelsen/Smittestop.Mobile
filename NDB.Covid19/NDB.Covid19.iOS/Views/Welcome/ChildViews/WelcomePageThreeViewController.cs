using System;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.AccessibilityUtils;
using static NDB.Covid19.ViewModels.WelcomeViewModel;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public partial class WelcomePageThreeViewController : PageViewController
    {
        public WelcomePageThreeViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTexts();
            SetAccessibility();

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, TitleLabel);
            BackArrow.Hidden = !LocalPreferencesHelper.IsOnboardingCompleted;
            BackArrow.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
        }

        partial void BackArrowBtn_TouchUpInside(UIButton sender)
        {
            NavigationController.PopViewController(true);
        }

        private void SetTexts()
        {
            InitTitle(TitleLabel, WELCOME_PAGE_THREE_TITLE);
            InitBodyText(BodyText1, WELCOME_PAGE_THREE_BODY_ONE);
            InitBodyText(BodyText2, WELCOME_PAGE_THREE_BODY_TWO);
            InitBodyText(BoxText, WELCOME_PAGE_THREE_INFOBOX_BODY);
        }

        private void SetAccessibility()
        {
            TitleLabel.AccessibilityAttributedLabel = RemovePoorlySpokenSymbols(WELCOME_PAGE_THREE_TITLE);
            BodyText1.AccessibilityAttributedLabel =
                RemovePoorlySpokenSymbols(WELCOME_PAGE_THREE_BODY_ONE_ACCESSIBILITY);
            BodyText2.AccessibilityAttributedLabel = RemovePoorlySpokenSymbols(WELCOME_PAGE_THREE_BODY_TWO);
            BoxText.AccessibilityAttributedLabel = RemovePoorlySpokenSymbols(WELCOME_PAGE_THREE_INFOBOX_BODY);
        }
    }
}