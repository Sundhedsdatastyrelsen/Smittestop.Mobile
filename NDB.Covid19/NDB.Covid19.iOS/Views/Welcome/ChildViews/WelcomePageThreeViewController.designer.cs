// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    [Register ("WelcomePageThreeViewController")]
    partial class WelcomePageThreeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackArrow { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BodyText1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BodyText2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BoxText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        [Action ("BackArrowBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackArrowBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackArrow != null) {
                BackArrow.Dispose ();
                BackArrow = null;
            }

            if (BodyText1 != null) {
                BodyText1.Dispose ();
                BodyText1 = null;
            }

            if (BodyText2 != null) {
                BodyText2.Dispose ();
                BodyText2 = null;
            }

            if (BoxText != null) {
                BoxText.Dispose ();
                BoxText = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}