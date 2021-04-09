using System;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.InfectionStatus;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.ViewModels.DiseaseRateViewModel;

namespace NDB.Covid19.Droid.Views.DiseaseRate
{
    [Activity(
        Theme = "@style/AppTheme",
        ParentActivity = typeof(InfectionStatusActivity),
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class DiseaseRateActivity : AppCompatActivity
    {
        private ViewGroup _closeButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = DISEASE_RATE_HEADER;
            SetContentView(Resource.Layout.activity_disease_rate);
            Init();
        }

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened DiseaseRate", null);
            InfectionStatusViewModel.RequestSSIUpdate(() => RunOnUiThread(UpdateUI));
        }

        private void Init()
        {
            UpdateUI();
            _closeButton = FindViewById<ViewGroup>(Resource.Id.disease_rate_close_cross_btn);
            _closeButton.Click += new StressUtils.SingleClick(OnCloseBtnClicked).Run;
            _closeButton.ContentDescription = MessagesViewModel.MESSAGES_ACCESSIBILITY_CLOSE_BUTTON;
        }

        private void UpdateUI()
        {
            FindViewById<TextView>(Resource.Id.disease_rate_header_textView).Text = DISEASE_RATE_HEADER;
            FindViewById<TextView>(Resource.Id.disease_rate_sub_header_textView).Text = LastUpdateStringSubHeader;
            TextView diseaseRateSubSub = FindViewById<TextView>(Resource.Id.disease_rate_sub_text);

            ISpanned formattedDescription =
                HtmlCompat.FromHtml(LastUpdateStringSubSubHeader, HtmlCompat.FromHtmlModeLegacy);
            diseaseRateSubSub.TextFormatted = formattedDescription;
            diseaseRateSubSub.ContentDescriptionFormatted = formattedDescription;
            diseaseRateSubSub.MovementMethod = LinkMovementMethod.Instance;
            //same color as Resource.Color.selectedDot #FADC5D
            diseaseRateSubSub.SetLinkTextColor(new Color(250, 220, 93));

            FindViewById<TextView>(Resource.Id.disease_rate_infected_header_text).Text = KEY_FEATURE_ONE_LABEL;
            FindViewById<TextView>(Resource.Id.disease_rate_infected_number_text).Text = ConfirmedCasesToday;
            FindViewById<TextView>(Resource.Id.disease_rate_infected_total_text).Text = ConfirmedCasesTotal;

            FindViewById<TextView>(Resource.Id.disease_rate_death_header_text).Text = KEY_FEATURE_TWO_LABEL;
            FindViewById<TextView>(Resource.Id.disease_rate_death_number_text).Text = DeathsToday;
            FindViewById<TextView>(Resource.Id.disease_rate_death_total_text).Text = DeathsTotal;

            FindViewById<TextView>(Resource.Id.disease_rate_tested_header_text).Text = KEY_FEATURE_THREE_LABEL;
            FindViewById<TextView>(Resource.Id.disease_rate_tested_number_text).Text = TestsConductedToday;
            FindViewById<TextView>(Resource.Id.disease_rate_tested_total_text).Text = TestsConductedTotal;

            FindViewById<TextView>(Resource.Id.disease_rate_vaccinated_header_text).Text = KEY_FEATURE_FOUR_LABEL;
            FindViewById<TextView>(Resource.Id.disease_rate_vaccinated_number).Text = VaccinatedFirst;
            FindViewById<TextView>(Resource.Id.disease_rate_vaccinated_number_label).Text =
                KEY_FEATURE_FOUR_FIRST_VACCINATION_LABEL;
            FindViewById<TextView>(Resource.Id.disease_rate_vaccinated_second_number).Text = VaccinatedSecond;
            FindViewById<TextView>(Resource.Id.disease_rate_vaccinated_second_number_label).Text =
                KEY_FEATURE_FOUR_SECOND_VACCINATION_LABEL;

            //Added newline for the UI to align.
            FindViewById<TextView>(Resource.Id.disease_rate_downloads_header_text).Text = $"{KEY_FEATURE_SIX_LABEL} \n";
            FindViewById<TextView>(Resource.Id.disease_rate_downloads_number_text).Text = SmittestopDownloadsTotal;

            FindViewById<TextView>(Resource.Id.disease_rate_positive_header_text).Text = KEY_FEATURE_FIVE_LABEL;
            FindViewById<TextView>(Resource.Id.disease_rate_positive_number_text).Text =
                NumberOfPositiveTestsResultsLast7Days;
            FindViewById<TextView>(Resource.Id.disease_rate_positive_total_text).Text =
                NumberOfPositiveTestsResultsTotal;
        }

        private void OnCloseBtnClicked(object arg1, EventArgs arg2)
        {
            GoToInfectionStatusActivity();
        }

        public override void OnBackPressed()
        {
            GoToInfectionStatusActivity();
        }

        private void GoToInfectionStatusActivity()
        {
            NavigationHelper.GoToResultPageAndClearTop(this);
        }
    }
}