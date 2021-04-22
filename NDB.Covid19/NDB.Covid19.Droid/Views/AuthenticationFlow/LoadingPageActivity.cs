using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Nio.FileNio;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using Xamarin.ExposureNotifications;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.ViewModels.LoadingPageViewModel;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    internal class LoadingPageActivity : AppCompatActivity
    {
        private static int? refusedCount = 0;
        private bool _isRunning;
        private TextView _loadingText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE;
            SetContentView(Resource.Layout.loading_page);
            FindViewById<ProgressBar>(Resource.Id.progress_bar).Visibility = ViewStates.Visible;
            _loadingText = FindViewById<TextView>(Resource.Id.loading_text);
            _loadingText.Text = LOADING_PAGE_TEXT_NORMAL;
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!_isRunning)
            {
                StartTimer(OnFinished);
                ValidateData(StartPushActivity, OnFail);
                _isRunning = true;
            }
        }

        private void OnFinished()
        {
            RunOnUiThread(() => { _loadingText.Text = LOADING_PAGE_TEXT_TIME_EXTENDED; });
        }

        private async void StartPushActivity()
        {
            try
            {
                await ExposureNotification.SubmitSelfDiagnosisAsync();
                LogUtils.LogMessage(LogSeverity.INFO, "The user agreed to share keys", null, GetCorrelationId());
                OnActivityFinished();
            }
            catch (Exception e)
            {
                _ = e.HandleExposureNotificationException(nameof(LoadingPageActivity), nameof(StartPushActivity));
                OnError(e);
            }
        }

        private void OnFail()
        {
            OnError(new Exception("Validation Failed"), true);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                ExposureNotification.OnActivityResult(requestCode, resultCode, data);
            }
            catch (Exception e)
            {
                _ = e.HandleExposureNotificationException(nameof(LoadingPageActivity), nameof(OnActivityResult));
            }
        }

        private void OnActivityFinished()
        {
            refusedCount = 0;
            RunOnUiThread(() => StartActivity(new Intent(this, typeof(RegisteredActivity))));
        }

        private void OnError(Exception e, bool isOnFail = false)
        {
            if (e is AccessDeniedException)
            {
                if (refusedCount == null)
                {
                    refusedCount = 0;
                }

                LogUtils.LogMessage(LogSeverity.INFO, "The user refused to share keys", (refusedCount++).ToString(),
                    GetCorrelationId());
                GoToConfirmLeavePage();
            }
            else
            {
                if (!isOnFail)
                {
                    LogUtils.LogMessage(
                        LogSeverity.INFO,
                        "Something went wrong during key sharing (INFO with correlation id)",
                        e.Message,
                        GetCorrelationId());
                }

                refusedCount = 0;
                RunOnUiThread(
                    () => AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, e, "Pushing keys failed"));
            }
        }

        private void GoToConfirmLeavePage()
        {
            StartActivity(new Intent(this, typeof(QuestionnaireConfirmLeaveActivity)));
        }

        public override void OnBackPressed()
        {
            // Disabled back button
        }
    }
}