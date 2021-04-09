// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using UIKit;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    [Register ("QuestionnaireConfirmLeaveViewController")]
    partial class QuestionnaireConfirmLeaveViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DefaultBorderButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SettingsPageContentLabel Description { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DefaultBorderButton OkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SetttingsPageTitleLabel Title { get; set; }

        [Action ("OnCancelButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCancelButton_TouchUpInside (DefaultBorderButton sender);

        [Action ("OnCloseButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCloseButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("OnOkButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnOkButton_TouchUpInside (DefaultBorderButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (Description != null) {
                Description.Dispose ();
                Description = null;
            }

            if (OkButton != null) {
                OkButton.Dispose ();
                OkButton = null;
            }

            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }
        }
    }
}