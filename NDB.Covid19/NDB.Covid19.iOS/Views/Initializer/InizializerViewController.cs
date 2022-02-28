using System;
using CoreGraphics;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.FarwellSmittestopPage;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Initializer
{
    public partial class InizializerViewController : BaseViewController
    {
        private UITapGestureRecognizer _gestureRecognizer;
        private bool AvailableOnDevice;

        public InizializerViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            StyleUtil.InitButtonStyling(StartButton, InitializerViewModel.LAUNCHER_PAGE_START_BTN);
            StyleUtil.InitLabel(ContinueInEnLbl, StyleUtil.FontType.FontRegular,
                InitializerViewModel.LAUNCHER_PAGE_CONTINUE_IN_ENG, 12, 20);
        }

        private void SetLogoBasedOnAppLanguage()
        {
            string appLanguage = LocalesService.GetLanguage();
            HealthMinistryLogo.Image = appLanguage != null && appLanguage.ToLower() == "en"
                ? UIImage.FromBundle("MinistryOfHealthEn")
                : UIImage.FromBundle("MinistryOfHealth");

            HealthMinistryLogo.Image = MaxResizeImage(
                HealthMinistryLogo.Image,
                appLanguage != null && appLanguage.ToLower() == "en" ? 110 : 150,
                80);
            HealthMinistryLogo.ContentMode = UIViewContentMode.ScaleAspectFit;
        }

        private static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            CGSize sourceSize = sourceImage.Size;
            double maxResizeFactor = Math.Min(
                maxWidth / sourceSize.Width,
                maxHeight / sourceSize.Height);
            double width = maxResizeFactor * sourceSize.Width;
            double height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContextWithOptions(new CGSize(width, height), false, 0);
            sourceImage.Draw(new CGRect(0, 0, width, height));
            UIImage resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // The app is supported from iOS 12.5 incl. and until iOS 13.0 excl.
            // and from 13.6 incl. and higher.
            string currentiOSVersion = UIDevice.CurrentDevice.SystemVersion;
            AvailableOnDevice = currentiOSVersion.CompareTo("13.6") >= 0 ||
                                currentiOSVersion.CompareTo("12.5") >= 0 && currentiOSVersion.CompareTo("13.0") < 0;

            if (AvailableOnDevice)
            {
                if (OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted)
                {
                    NavigationHelper.GoToWelcomeWhatsNewPage(this);
                    return;
                }

                NavigationHelper.GoToResultPageIfOnboarded(this);
            }
            else
            {
                ShowOutdatedOSDialog();
            }

            SetLogoBasedOnAppLanguage();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SetupButton();

            if (OnboardingStatusHelper.Status == OnboardingStatus.CountriesOnboardingCompleted)
            {
                StartButton.Hidden = true;
                ContinueInEnStackView.Hidden = true;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ContinueInEnStackView.RemoveGestureRecognizer(_gestureRecognizer);
        }

        partial void StartButton_TouchUpInside(UIButton sender)
        {
            LocalPreferencesHelper.SetAppLanguage("da");
            LocalesService.Initialize();
            Continue();
        }

        private void ShowOutdatedOSDialog()
        {
            DialogViewModel dialogViewModel = new DialogViewModel
            {
                Title = "BASE_ERROR_TITLE".Translate(),
                Body = "LAUNCHER_PAGE_OS_VERSION_DIALOG_MESSAGE_IOS".Translate(),
                OkBtnTxt = "ERROR_OK_BTN".Translate()
            };
            DialogHelper.ShowDialog(this, dialogViewModel, action => { });
        }

        private void Continue()
        {
            if (AvailableOnDevice)
            {
                // NavigationHelper.GoToOnboardingPage(this);
                NavigationHelper.GoToFarwellSmittestopPage(this);


            }
            else
            {
                ShowOutdatedOSDialog();
            }
        }

        private void SetupButton()
        {
            _gestureRecognizer = new UITapGestureRecognizer();
            _gestureRecognizer.AddTarget(() => OnContinueInEnViewBtnTapped(_gestureRecognizer));
            ContinueInEnStackView.AddGestureRecognizer(_gestureRecognizer);
        }

        private void OnContinueInEnViewBtnTapped(UITapGestureRecognizer recognizer)
        {
            LocalPreferencesHelper.SetAppLanguage("en");
            LocalesService.Initialize();
            Continue();
        }
    }
}