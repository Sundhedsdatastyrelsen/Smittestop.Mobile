// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;
using NDB.Covid19.iOS.Views.CustomSubclasses;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage5
{
    [Register ("SettingsPage5ViewController")]
    partial class SettingsPage5ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BuildVersionLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView ContentText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SetttingsPageTitleLabel HeaderLabel { get; set; }

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (BuildVersionLbl != null) {
                BuildVersionLbl.Dispose ();
                BuildVersionLbl = null;
            }

            if (ContentText != null) {
                ContentText.Dispose ();
                ContentText = null;
            }

            if (HeaderLabel != null) {
                HeaderLabel.Dispose ();
                HeaderLabel = null;
            }
        }
    }
}