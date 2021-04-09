// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    [Register ("LoadingPageViewController")]
    partial class LoadingPageViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LoadingText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView LoadingTextStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Spinner { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LoadingText != null) {
                LoadingText.Dispose ();
                LoadingText = null;
            }

            if (LoadingTextStackView != null) {
                LoadingTextStackView.Dispose ();
                LoadingTextStackView = null;
            }

            if (Spinner != null) {
                Spinner.Dispose ();
                Spinner = null;
            }
        }
    }
}