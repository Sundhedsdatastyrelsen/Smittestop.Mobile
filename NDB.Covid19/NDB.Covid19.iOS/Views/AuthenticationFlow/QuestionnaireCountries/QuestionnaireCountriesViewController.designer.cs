// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;
using NDB.Covid19.iOS.Views.CustomSubclasses;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries
{
    [Register ("QuestionnaireCountriesViewController")]
    partial class QuestionnaireCountriesViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ButtonView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView CountryTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Divider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ListExplainLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        DefaultBorderButton NextBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        RadioButton NoRadioButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SubtitleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TableViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel YesLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        RadioButton YesRadioButton { get; set; }

        [Action ("NextBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NextBtnTapped (DefaultBorderButton sender);

        [Action ("OnCloseBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCloseBtnTapped (UIKit.UIButton sender);

        [Action ("OnNoRadioButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnNoRadioButton_TouchUpInside (RadioButton sender);

        [Action ("OnYesRadioButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnYesRadioButton_TouchUpInside (RadioButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ButtonView != null) {
                ButtonView.Dispose ();
                ButtonView = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (CountryTableView != null) {
                CountryTableView.Dispose ();
                CountryTableView = null;
            }

            if (Divider != null) {
                Divider.Dispose ();
                Divider = null;
            }

            if (ListExplainLbl != null) {
                ListExplainLbl.Dispose ();
                ListExplainLbl = null;
            }

            if (NextBtn != null) {
                NextBtn.Dispose ();
                NextBtn = null;
            }

            if (NoLabel != null) {
                NoLabel.Dispose ();
                NoLabel = null;
            }

            if (NoRadioButton != null) {
                NoRadioButton.Dispose ();
                NoRadioButton = null;
            }

            if (SubtitleLbl != null) {
                SubtitleLbl.Dispose ();
                SubtitleLbl = null;
            }

            if (TableViewHeightConstraint != null) {
                TableViewHeightConstraint.Dispose ();
                TableViewHeightConstraint = null;
            }

            if (TitleLbl != null) {
                TitleLbl.Dispose ();
                TitleLbl = null;
            }

            if (YesLabel != null) {
                YesLabel.Dispose ();
                YesLabel = null;
            }

            if (YesRadioButton != null) {
                YesRadioButton.Dispose ();
                YesRadioButton = null;
            }
        }
    }
}