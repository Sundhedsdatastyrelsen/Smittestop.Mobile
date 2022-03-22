// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.FarewellSmittestopPage
{
	[Register ("FarewellSmittestopPageViewController")]
	partial class FarewellSmittestopPageViewController
	{
		[Outlet]
		UIKit.UILabel FarwellSmitteStopUiLbl { get; set; }

		[Outlet]
		UIKit.UIButton MoreInfoButton { get; set; }

		[Outlet]
		UIKit.UILabel MoreInfoLabel { get; set; }

		[Outlet]
		UIKit.UIButton OkButtonFarwellUiButton { get; set; }

		[Outlet]
		UIKit.UILabel TexoneFarwellUiLbl { get; set; }

		[Outlet]
		UIKit.UILabel TextThreeFarwellLbl { get; set; }

		[Outlet]
		UIKit.UILabel TextTwoFarwllUiLbl { get; set; }

		[Outlet]
		UIKit.UIImageView UiImageOne { get; set; }

		[Outlet]
		UIKit.UIImageView UiImageThree { get; set; }

		[Outlet]
		UIKit.UIImageView UiImageTwo { get; set; }

		[Action ("MoreInfoButtonClicked:")]
		partial void MoreInfoButtonClicked (Foundation.NSObject sender);

		[Action ("OkButtonFarwell:")]
		partial void OkButtonFarwell (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (FarwellSmitteStopUiLbl != null) {
				FarwellSmitteStopUiLbl.Dispose ();
				FarwellSmitteStopUiLbl = null;
			}

			if (OkButtonFarwellUiButton != null) {
				OkButtonFarwellUiButton.Dispose ();
				OkButtonFarwellUiButton = null;
			}

			if (TexoneFarwellUiLbl != null) {
				TexoneFarwellUiLbl.Dispose ();
				TexoneFarwellUiLbl = null;
			}

			if (TextThreeFarwellLbl != null) {
				TextThreeFarwellLbl.Dispose ();
				TextThreeFarwellLbl = null;
			}

			if (TextTwoFarwllUiLbl != null) {
				TextTwoFarwllUiLbl.Dispose ();
				TextTwoFarwllUiLbl = null;
			}

			if (UiImageOne != null) {
				UiImageOne.Dispose ();
				UiImageOne = null;
			}

			if (UiImageThree != null) {
				UiImageThree.Dispose ();
				UiImageThree = null;
			}

			if (UiImageTwo != null) {
				UiImageTwo.Dispose ();
				UiImageTwo = null;
			}

			if (MoreInfoLabel != null) {
				MoreInfoLabel.Dispose ();
				MoreInfoLabel = null;
			}

			if (MoreInfoButton != null) {
				MoreInfoButton.Dispose ();
				MoreInfoButton = null;
			}
		}
	}
}
