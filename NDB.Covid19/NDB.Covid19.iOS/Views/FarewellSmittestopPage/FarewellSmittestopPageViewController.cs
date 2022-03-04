using System;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;

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
        }

        partial void OkButtonFarwell(UIKit.UIButton sender)
        {
            DeviceUtils.StopScanServices(); // Stop scan services if running.
            DeviceUtils.CleanDataFromDevice(); // Clean data from device.
            DismissViewController(true, null);
        }
    }
}