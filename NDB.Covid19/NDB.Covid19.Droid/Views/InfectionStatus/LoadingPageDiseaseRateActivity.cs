using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.DiseaseRate;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.InfectionStatus
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    internal class LoadingPageDiseaseRateActivity : AppCompatActivity
    {
        private bool _isRunning;
        private DiseaseRateViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE;
            SetContentView(Resource.Layout.loading_page);
            _viewModel = new DiseaseRateViewModel();
            FindViewById<ProgressBar>(Resource.Id.progress_bar).Visibility = ViewStates.Visible;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!_isRunning)
            {
                LoadDataAndStartDiseaseRateActivity();
                _isRunning = true;
            }
        }

        private async void LoadDataAndStartDiseaseRateActivity()
        {
            try
            {
                var isSuccess = await DiseaseRateViewModel.UpdateSSIDataAsync();
                if (!isSuccess && LocalPreferencesHelper.HasNeverSuccessfullyFetchedSSIData)
                {
                    OnError(new NullReferenceException("No SSI data"));
                    return;
                }

                LogUtils.LogMessage(LogSeverity.INFO, "Data for the disease rate of the day is loaded");
                OnActivityFinished();
            }
            catch (Exception e)
            {
                if (!IsFinishing)
                {
                    OnError(e);
                }
            }
        }

        private void OnActivityFinished()
        {
            RunOnUiThread(() => StartActivity(new Intent(this, typeof(DiseaseRateActivity))));
        }

        private void OnError(Exception e)
        {
            if (LocalPreferencesHelper.HasNeverSuccessfullyFetchedSSIData)
            {
                RunOnUiThread(() => AuthErrorUtils.GoToTechnicalErrorSSINumbers(this, LogSeverity.ERROR, e,
                    "Could not load data for disease rate of the day, showing technical error page"));
            }
            else
            {
                LogUtils.LogException(LogSeverity.ERROR, e,
                    "Could not load data for disease rate of the day, showing old data");
                RunOnUiThread(() => StartActivity(new Intent(this, typeof(DiseaseRateActivity))));
            }
        }
    }
}