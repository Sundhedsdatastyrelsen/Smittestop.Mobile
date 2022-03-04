using System;
using Foundation;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Farewell
{
    public partial class NotActiveViewController : BaseViewController
    {
        public NotActiveViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UpdateLayout();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            CheckOnboardingStatus();
        }

        private void CheckOnboardingStatus()
        {
            if (OnboardingStatusHelper.Status != OnboardingStatus.NoConsentsGiven)
            {
                NavigationHelper.GoToFarewellSmittestopPage(this);
            }
        }

        private void UpdateLayout()
        {
            StyleUtil.InitLabel(descriptionLabel, StyleUtil.FontType.FontBold,
                SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_TEXT, 16, 22);
            StyleUtil.InitLabelWithSpacing(moreInformationDescriptionLabel, StyleUtil.FontType.FontRegular,
                SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_MORE_INFO, 1.24, 18, 24, UITextAlignment.Center);
            StyleUtil.InitButtonWithArrowStyling(moreInformationButton, SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_INFO_LINK_TEXT);
            StyleUtil.InitButtonStyling(languageButton, SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_BUTTONT_TEXT);
            descriptionView.Layer.CornerRadius = 8;

            SetLogoBasedOnAppLanguage();
        }

        private void SetLogoBasedOnAppLanguage()
        {
            string appLanguage = LocalesService.GetLanguage();
            ministryLogo.Image = appLanguage != null && appLanguage.ToLower() == "en"
                ? UIImage.FromBundle("MinistryOfHealthEn")
                : UIImage.FromBundle("MinistryOfHealth");
            ministryLogo.Image = StyleUtil.MaxResizeImage(
                ministryLogo.Image,
                appLanguage != null && appLanguage.ToLower() == "en" ? 110 : 150,
                50);
            ministryLogo.ContentMode = UIViewContentMode.ScaleAspectFit;
        }

        partial void MoreInformationButtonTapped(NSObject sender)
        {
            NSUrl url = new NSUrl(SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_INFO_URL);
            UIApplication.SharedApplication.OpenUrl(url);
        }

        partial void LanguageButtonTapped(NSObject sender)
        {
            string appLanguage = LocalesService.GetLanguage();
            if (appLanguage != null && appLanguage.ToLower() == "en")
            {
                LocalPreferencesHelper.SetAppLanguage("da");
            } else
            {
                LocalPreferencesHelper.SetAppLanguage("en");
            }

            LocalesService.Initialize();

            // Update layout with new language, will not update potential presenting views.
            UpdateLayout();
        }
    }
}
