using System;

using UIKit;
using System.Collections.Generic;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.StressUtils;

namespace NDB.Covid19.iOS.Views.FarwellSmittestopPage
{
    public partial class FarwellSmittestopPageViewController : BaseViewController


    {
        private FarwellSmittestopPageViewController _farwellPageController;
        public FarwellSmittestopPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            SetupStyling();

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
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
            TextThreeFarwellLbl.Text = FarewellSmittestopViewModel.FAREWELL_SMITTESTOP_BODY_TWO;


        }

        
    }
}