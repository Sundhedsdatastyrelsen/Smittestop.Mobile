using System;
using Foundation;
using I18NPortable;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.NotActive
{
    public partial class NotActiveViewController : BaseViewController
    {
        public static DialogViewModel LanguageChangedDialog => new DialogViewModel
        {
            Title = "SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER".Translate(),
            Body = "SETTINGS_GENERAL_RESTART_REQUIRED_TEXT".Translate(),
            OkBtnTxt = "SETTINGS_GENERAL_DIALOG_OK".Translate()
        };
        private bool _languageChanged = false;

        public NotActiveViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupLayout();

            // TODO: Check if any consent accepted, if yes -> Go to farawell
        }

        public void SetupLayout()
        {
            StyleUtil.InitLabel(descriptionLabel, StyleUtil.FontType.FontBold,
                SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_TEXT, 16, 22);
            StyleUtil.InitLabel(moreInformationDescriptionLabel, StyleUtil.FontType.FontRegular,
                SmittestopNotActiveViewModel.SMITTESTOP_NOT_ACTIVE_MORE_INFO, 18, 24);
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
            if (_languageChanged)
            {
                return;
            }

            _languageChanged = true;
            string appLanguage = LocalesService.GetLanguage();
            if (appLanguage != null && appLanguage.ToLower() == "en")
            {
                LocalPreferencesHelper.SetAppLanguage("da");
            } else
            {
                LocalPreferencesHelper.SetAppLanguage("en");
            }

            LocalesService.Initialize();
            DialogHelper.ShowDialog(this, LanguageChangedDialog, Action => { });
        }
    }
}
