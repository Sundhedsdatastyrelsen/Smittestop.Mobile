using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.FarewellSmittestopPage
{
    public partial class FarewellSmittestopPageViewController : BaseViewController

    {
        public FarewellSmittestopPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupStyling();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public void SetupStyling()
        {
            StyleUtil.InitButtonStyling(OkButtonFarwellUiButton, FarewellSmittestopViewModel.FAREWELL_SMITTESTOP_BUTTON_TEXT);

            FarwellSmitteStopUiLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 24, 26);
            FarwellSmitteStopUiLbl.Text = FarewellSmittestopViewModel.FAREWELL_SMITTESTOP_TITLE;

            TexoneFarwellUiLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 16, 22);
            TexoneFarwellUiLbl.Text = FarewellSmittestopViewModel.FAREWELL_SMITTESTOP_BODY_ONE;
              
            TextTwoFarwllUiLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 16, 22);
            TextTwoFarwllUiLbl.Text = FarewellSmittestopViewModel.FAREWELL_SMITTESTOP_BODY_TWO;

            TextThreeFarwellLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 16, 22);
            TextThreeFarwellLbl.Text = FarewellSmittestopViewModel.FAREWELL_SMITTESTOP_BODY_THREE;

            SetupMoreInfo();
        }

        private void SetupMoreInfo()
        {
            StyleUtil.InitLabelWithSpacing(MoreInfoLabel, StyleUtil.FontType.FontRegular,
                FarewellSmittestopViewModel.SMITTESTOP_NOT_ACTIVE_MORE_INFO, 1.24, 18, 24, UITextAlignment.Center);
            StyleUtil.InitButtonWithArrowStyling(MoreInfoButton,
                FarewellSmittestopViewModel.SMITTESTOP_NOT_ACTIVE_INFO_LINK_TEXT);

            MoreInfoLabel.AccessibilityElementsHidden = true;
            MoreInfoButton.AccessibilityLabel = MoreInfoLabel.Text + ", " + MoreInfoButton.Title(UIControlState.Normal);

            // Only show for danish.
            string appLanguage = LocalesService.GetLanguage();
            if (appLanguage != null && appLanguage.ToLower() == "da")
            {
                MoreInfoLabel.Hidden = false;
                MoreInfoButton.Hidden = false;
            }
            else
            {
                MoreInfoLabel.Hidden = true;
                MoreInfoButton.Hidden = true;
            }
        }

        partial void OkButtonFarwell(UIKit.UIButton sender)
        {
            DeviceUtils.StopScanServices(); // Stop scan services if running.
            DeviceUtils.CleanDataFromDevice(); // Clean data from device.
            DismissViewController(true, null);
        }

        partial void MoreInfoButtonClicked(Foundation.NSObject sender)
        {
            NSUrl url = new NSUrl(FarewellSmittestopViewModel.SMITTESTOP_NOT_ACTIVE_INFO_URL);
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}