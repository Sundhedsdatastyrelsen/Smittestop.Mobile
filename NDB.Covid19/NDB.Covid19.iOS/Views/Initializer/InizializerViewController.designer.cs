// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using System.CodeDom.Compiler;
using Foundation;

namespace NDB.Covid19.iOS.Views.Initializer
{
	[Register ("InizializerViewController")]
	partial class InizializerViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ContinueInEnLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView ContinueInEnStackView { get; set; }

		[Outlet]
		UIKit.UIImageView HealthMinistryLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton StartButton { get; set; }

		[Action ("StartButton_TouchUpInside:")]
		partial void StartButton_TouchUpInside (UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ContinueInEnLbl != null) {
				ContinueInEnLbl.Dispose ();
				ContinueInEnLbl = null;
			}

			if (ContinueInEnStackView != null) {
				ContinueInEnStackView.Dispose ();
				ContinueInEnStackView = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}

			if (HealthMinistryLogo != null) {
				HealthMinistryLogo.Dispose ();
				HealthMinistryLogo = null;
			}

		}
	}
}
