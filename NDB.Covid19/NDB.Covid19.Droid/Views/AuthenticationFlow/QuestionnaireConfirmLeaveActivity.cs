using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.ViewModels.QuestionnaireConfirmLeaveViewModel;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class QuestionnaireConfirmLeaveActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.questionnaire_confirm_leave);
            Init();
        }

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(
                LogSeverity.INFO,
                "The user is seeing Questionnaire Confirm Leave",
                null,
                GetCorrelationId());
        }

        private void Init()
        {
            TextView title = FindViewById(Resource.Id.title) as TextView;
            TextView description = FindViewById(Resource.Id.description) as TextView;
            Button okButton = FindViewById(Resource.Id.ok_button) as Button;
            Button cancelButton = FindViewById(Resource.Id.cancel_button) as Button;
            title.TextFormatted =
                HtmlCompat.FromHtml(QUESTIONNAIRE_CONFIRM_LEAVE_TITLE, HtmlCompat.FromHtmlModeLegacy);
            description.TextFormatted =
                HtmlCompat.FromHtml(QUESTIONNAIRE_CONFIRM_LEAVE_DESCRIPTION, HtmlCompat.FromHtmlModeLegacy);

            okButton.Text = QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_OK;
            cancelButton.Text = QUESTIONNAIRE_CONFIRM_LEAVE_BUTTON_CANCEL;

            cancelButton.Click +=
                new StressUtils.SingleClick((o, arg) =>
                {
                    LogUtils.LogMessage(
                        LogSeverity.INFO,
                        "The user confirmed not sharing keys",
                        null,
                        GetCorrelationId());
                    GoToInfectionStatusPage();
                }).Run;

            okButton.Click +=
                new StressUtils.SingleClick((o, arg) =>
                    ValidateData(GoToLoadingPage, OnFail)).Run;

            Button closeButton = FindViewById<Button>(Resource.Id.close_cross_btn);
            closeButton.ContentDescription = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            closeButton.Click +=
                new StressUtils.SingleClick((o, ev) => ShowAreYouSureToExitDialog()).Run;
        }

        private void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireConfirmLeaveActivity)}.{nameof(OnFail)}: " +
                "No miba data or token expired");
        }

        private async void ShowAreYouSureToExitDialog()
        {
            bool isOkPressed = await DialogUtils.DisplayDialogAsync(
                this,
                ErrorViewModel.REGISTER_LEAVE_HEADER,
                ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
                ErrorViewModel.REGISTER_LEAVE_CONFIRM,
                ErrorViewModel.REGISTER_LEAVE_CANCEL);
            if (isOkPressed)
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "The user is returning to Infection Status",
                    null,
                    GetCorrelationId());
                GoToInfectionStatusPage();
            }
        }

        private void GoToInfectionStatusPage()
        {
            NavigationHelper.GoToResultPageAndClearTop(this);
        }

        private void GoToLoadingPage()
        {
            StartActivity(new Intent(this, typeof(LoadingPageActivity)));
        }

        public override void OnBackPressed()
        {
            // Disabled back button
        }
    }
}