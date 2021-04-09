// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.DiseaseRate
{
	[Register ("DiseaseRateViewController")]
	partial class DiseaseRateViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton BackButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BackgroundView1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BackgroundView2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BackgroundView3 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BackgroundView4 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BackgroundView5 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BackgroundView6 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView ConfirmedCases_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateNumber1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateNumber2Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateNumber3Lbl { get; set; }

		[Outlet]
		UIKit.UILabel DiseaseRateNumber4DescriptionLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateNumber4Lbl { get; set; }

		[Outlet]
		UIKit.UILabel DiseaseRateNumber4SecondDescriptionLbl { get; set; }

		[Outlet]
		UIKit.UILabel DiseaseRateNumber4SecondLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateNumber5Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateNumber6Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DiseaseRateOfTheDayTextLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIScrollView DiseaseRateScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextView DiseaseRateSubLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DiseaseRateView1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DiseaseRateView2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DiseaseRateView3 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DiseaseRateView4 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DiseaseRateView5 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DiseaseRateView6 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature2Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature3Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature4Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature5Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature6Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView NumberOfDeaths_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView NumberOfPositiveResults_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView NumberOfTests_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView PatientsAdmitted_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TotalDiseaseRateNumber1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TotalDiseaseRateNumber2Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TotalDiseaseRateNumber3Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TotalDiseaseRateNumber6Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView TotalDownloads_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView UIimage5 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView UIimage6 { get; set; }

		[Action ("BackButton_tapped:")]
		partial void BackButton_tapped (UIKit.UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}

			if (BackgroundView1 != null) {
				BackgroundView1.Dispose ();
				BackgroundView1 = null;
			}

			if (BackgroundView2 != null) {
				BackgroundView2.Dispose ();
				BackgroundView2 = null;
			}

			if (BackgroundView3 != null) {
				BackgroundView3.Dispose ();
				BackgroundView3 = null;
			}

			if (BackgroundView4 != null) {
				BackgroundView4.Dispose ();
				BackgroundView4 = null;
			}

			if (BackgroundView5 != null) {
				BackgroundView5.Dispose ();
				BackgroundView5 = null;
			}

			if (BackgroundView6 != null) {
				BackgroundView6.Dispose ();
				BackgroundView6 = null;
			}

			if (ConfirmedCases_StackView != null) {
				ConfirmedCases_StackView.Dispose ();
				ConfirmedCases_StackView = null;
			}

			if (DiseaseRateLbl != null) {
				DiseaseRateLbl.Dispose ();
				DiseaseRateLbl = null;
			}

			if (DiseaseRateNumber1Lbl != null) {
				DiseaseRateNumber1Lbl.Dispose ();
				DiseaseRateNumber1Lbl = null;
			}

			if (DiseaseRateNumber2Lbl != null) {
				DiseaseRateNumber2Lbl.Dispose ();
				DiseaseRateNumber2Lbl = null;
			}

			if (DiseaseRateNumber3Lbl != null) {
				DiseaseRateNumber3Lbl.Dispose ();
				DiseaseRateNumber3Lbl = null;
			}

			if (DiseaseRateNumber4Lbl != null) {
				DiseaseRateNumber4Lbl.Dispose ();
				DiseaseRateNumber4Lbl = null;
			}

			if (DiseaseRateNumber4DescriptionLbl != null) {
				DiseaseRateNumber4DescriptionLbl.Dispose ();
				DiseaseRateNumber4DescriptionLbl = null;
			}

			if (DiseaseRateNumber4SecondLbl != null) {
				DiseaseRateNumber4SecondLbl.Dispose ();
				DiseaseRateNumber4SecondLbl = null;
			}

			if (DiseaseRateNumber4SecondDescriptionLbl != null) {
				DiseaseRateNumber4SecondDescriptionLbl.Dispose ();
				DiseaseRateNumber4SecondDescriptionLbl = null;
			}

			if (DiseaseRateNumber5Lbl != null) {
				DiseaseRateNumber5Lbl.Dispose ();
				DiseaseRateNumber5Lbl = null;
			}

			if (DiseaseRateNumber6Lbl != null) {
				DiseaseRateNumber6Lbl.Dispose ();
				DiseaseRateNumber6Lbl = null;
			}

			if (DiseaseRateOfTheDayTextLbl != null) {
				DiseaseRateOfTheDayTextLbl.Dispose ();
				DiseaseRateOfTheDayTextLbl = null;
			}

			if (DiseaseRateScrollView != null) {
				DiseaseRateScrollView.Dispose ();
				DiseaseRateScrollView = null;
			}

			if (DiseaseRateSubLbl != null) {
				DiseaseRateSubLbl.Dispose ();
				DiseaseRateSubLbl = null;
			}

			if (DiseaseRateView1 != null) {
				DiseaseRateView1.Dispose ();
				DiseaseRateView1 = null;
			}

			if (DiseaseRateView2 != null) {
				DiseaseRateView2.Dispose ();
				DiseaseRateView2 = null;
			}

			if (DiseaseRateView3 != null) {
				DiseaseRateView3.Dispose ();
				DiseaseRateView3 = null;
			}

			if (DiseaseRateView4 != null) {
				DiseaseRateView4.Dispose ();
				DiseaseRateView4 = null;
			}

			if (DiseaseRateView5 != null) {
				DiseaseRateView5.Dispose ();
				DiseaseRateView5 = null;
			}

			if (DiseaseRateView6 != null) {
				DiseaseRateView6.Dispose ();
				DiseaseRateView6 = null;
			}

			if (KeyFeature1Lbl != null) {
				KeyFeature1Lbl.Dispose ();
				KeyFeature1Lbl = null;
			}

			if (KeyFeature2Lbl != null) {
				KeyFeature2Lbl.Dispose ();
				KeyFeature2Lbl = null;
			}

			if (KeyFeature3Lbl != null) {
				KeyFeature3Lbl.Dispose ();
				KeyFeature3Lbl = null;
			}

			if (KeyFeature4Lbl != null) {
				KeyFeature4Lbl.Dispose ();
				KeyFeature4Lbl = null;
			}

			if (KeyFeature5Lbl != null) {
				KeyFeature5Lbl.Dispose ();
				KeyFeature5Lbl = null;
			}

			if (KeyFeature6Lbl != null) {
				KeyFeature6Lbl.Dispose ();
				KeyFeature6Lbl = null;
			}

			if (NumberOfDeaths_StackView != null) {
				NumberOfDeaths_StackView.Dispose ();
				NumberOfDeaths_StackView = null;
			}

			if (NumberOfPositiveResults_StackView != null) {
				NumberOfPositiveResults_StackView.Dispose ();
				NumberOfPositiveResults_StackView = null;
			}

			if (NumberOfTests_StackView != null) {
				NumberOfTests_StackView.Dispose ();
				NumberOfTests_StackView = null;
			}

			if (PatientsAdmitted_StackView != null) {
				PatientsAdmitted_StackView.Dispose ();
				PatientsAdmitted_StackView = null;
			}

			if (TotalDiseaseRateNumber1Lbl != null) {
				TotalDiseaseRateNumber1Lbl.Dispose ();
				TotalDiseaseRateNumber1Lbl = null;
			}

			if (TotalDiseaseRateNumber2Lbl != null) {
				TotalDiseaseRateNumber2Lbl.Dispose ();
				TotalDiseaseRateNumber2Lbl = null;
			}

			if (TotalDiseaseRateNumber3Lbl != null) {
				TotalDiseaseRateNumber3Lbl.Dispose ();
				TotalDiseaseRateNumber3Lbl = null;
			}

			if (TotalDiseaseRateNumber6Lbl != null) {
				TotalDiseaseRateNumber6Lbl.Dispose ();
				TotalDiseaseRateNumber6Lbl = null;
			}

			if (TotalDownloads_StackView != null) {
				TotalDownloads_StackView.Dispose ();
				TotalDownloads_StackView = null;
			}

			if (UIimage5 != null) {
				UIimage5.Dispose ();
				UIimage5 = null;
			}

			if (UIimage6 != null) {
				UIimage6.Dispose ();
				UIimage6 = null;
			}

		}
	}
}
