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
using static NDB.Covid19.ViewModels.ErrorViewModel;
using static NDB.Covid19.ViewModels.QuestionnairePreShareViewModel;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class QuestionnairePreShareActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.questionnaire_pre_share);
            Init();
        }

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(
                LogSeverity.INFO,
                "The user is seeing Questionnaire Pre Share",
                null,
                GetCorrelationId());
        }

        private void Init()
        {
            TextView title = FindViewById(Resource.Id.title) as TextView;
            TextView description = FindViewById(Resource.Id.description) as TextView;
            Button nextButton = FindViewById(Resource.Id.next_button) as Button;
            title.TextFormatted =
                HtmlCompat.FromHtml(QUESTIONNAIRE_PRE_SHARE_TITLE, HtmlCompat.FromHtmlModeLegacy);
            description.TextFormatted =
                HtmlCompat.FromHtml(QUESTIONNAIRE_PRE_SHARE_DESCRIPTION, HtmlCompat.FromHtmlModeLegacy);

            nextButton.Text = QUESTIONNAIRE_PRE_SHARE_NEXT_BUTTON;

            nextButton.Click +=
                new StressUtils.SingleClick(
                    (o, arg) =>
                        InvokeNextButtonClick(GoToLoadingPage, OnFail)).Run;

            Button closeButton = FindViewById<Button>(Resource.Id.close_cross_btn);
            closeButton.ContentDescription = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            closeButton.Click +=
                new StressUtils.SingleClick((o, ev) => ShowAreYouSureToExitDialog()).Run;
        }

        private void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnairePreShareActivity)}.{nameof(OnFail)}: " +
                "No miba data or token expired");
        }

        private async void ShowAreYouSureToExitDialog()
        {
            bool isOkPressed = await DialogUtils.DisplayDialogAsync(
                this,
                REGISTER_LEAVE_HEADER,
                REGISTER_LEAVE_DESCRIPTION,
                REGISTER_LEAVE_CONFIRM,
                REGISTER_LEAVE_CANCEL);
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
    }
}