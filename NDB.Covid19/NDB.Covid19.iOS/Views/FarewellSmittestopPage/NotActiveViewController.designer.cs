// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Farewell
{
	[Register ("NotActiveViewController")]
	partial class NotActiveViewController
	{
		[Outlet]
		UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		UIKit.UIView descriptionView { get; set; }

		[Outlet]
		UIKit.UIButton languageButton { get; set; }

		[Outlet]
		UIKit.UIImageView ministryLogo { get; set; }

		[Outlet]
		UIKit.UIButton moreInformationButton { get; set; }

		[Outlet]
		UIKit.UILabel moreInformationDescriptionLabel { get; set; }

		[Action ("LanguageButtonTapped:")]
		partial void LanguageButtonTapped (Foundation.NSObject sender);

		[Action ("MoreInformationButtonTapped:")]
		partial void MoreInformationButtonTapped (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (descriptionView != null) {
				descriptionView.Dispose ();
				descriptionView = null;
			}

			if (languageButton != null) {
				languageButton.Dispose ();
				languageButton = null;
			}

			if (ministryLogo != null) {
				ministryLogo.Dispose ();
				ministryLogo = null;
			}

			if (moreInformationDescriptionLabel != null) {
				moreInformationDescriptionLabel.Dispose ();
				moreInformationDescriptionLabel = null;
			}

			if (moreInformationButton != null) {
				moreInformationButton.Dispose ();
				moreInformationButton = null;
			}
		}
	}
}
