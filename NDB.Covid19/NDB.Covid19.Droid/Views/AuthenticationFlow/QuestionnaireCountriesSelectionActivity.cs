using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.AuthenticationFlow.QuestionnaireAdapters;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class QuestionnaireCountriesSelectionActivity : Activity
    {
        private readonly QuestionnaireCountriesViewModel _viewModel = new QuestionnaireCountriesViewModel();
        private Button _closeButton;
        private List<CountryDetailsViewModel> _countries = new List<CountryDetailsViewModel>();
        private TextView _footer;
        private Button _nextButton;
        private ProgressBar _progressBar;
        private RecyclerView _recyclerView;
        private TextView _subtitle;
        private TextView _title;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_HEADER_TEXT;

            InitView();
        }

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Countries Selection", null,
                GetCorrelationId());
        }

        private async void InitView()
        {
            SetContentView(Resource.Layout.questionnaire_countries);

            _title = FindViewById<TextView>(Resource.Id.countries_title);
            _subtitle = FindViewById<TextView>(Resource.Id.countries_subtitle);
            _footer = FindViewById<TextView>(Resource.Id.questionnaire_countries_footer);
            _nextButton = FindViewById<Button>(Resource.Id.countries_button);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.countries_list);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            _closeButton = FindViewById<Button>(Resource.Id.close_cross_btn);

            PrepareRadioGroup();

            //Accessibility
            _closeButton.ContentDescription = InformationAndConsentViewModel.CLOSE_BUTTON_ACCESSIBILITY_LABEL;

            _closeButton.Click += new StressUtils.SingleClick(OnExitClick).Run;
            _nextButton.Click += new StressUtils.SingleClick(OnNextButtonClick).Run;

            _title.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_HEADER_TEXT;
            _subtitle.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_INFORMATION_TEXT;
            _footer.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_FOOTER;
            _nextButton.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_BUTTON_TEXT;

            RunOnUiThread(() => ShowSpinner(true));
            _countries.AddRange(
                await _viewModel.GetListOfCountriesAsync() ??
                new List<CountryDetailsViewModel>());
            if (!_countries.Any())
            {
                RunOnUiThread(() => ShowSpinner(false));
                OnServerError();
                return;
            }

            RunOnUiThread(() =>
            {
                ShowSpinner(false);
                QuestionnaireCountriesSelectionAdapter adapter =
                    new QuestionnaireCountriesSelectionAdapter(_countries);
                LinearLayoutManager layoutManager = new LinearLayoutManager(this);
                _recyclerView.SetLayoutManager(layoutManager);
                _recyclerView.SetAdapter(adapter);
            });
        }

        private void PrepareRadioGroup()
        {
            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radio_group);
            radioGroup.CheckedChange += (sender, args) =>
            {
                ConstraintLayout visibilityLayout =
                    FindViewById<ConstraintLayout>(Resource.Id.countries_visibility_layout);

                visibilityLayout.Visibility = radioGroup.CheckedRadioButtonId switch
                {
                    Resource.Id.radio_yes => ViewStates.Visible,
                    Resource.Id.radio_no => ViewStates.Gone,
                    _ => visibilityLayout.Visibility
                };
            };
            RadioButton noRadioButton = FindViewById<RadioButton>(Resource.Id.radio_no);
            RadioButton yesRadioButton = FindViewById<RadioButton>(Resource.Id.radio_yes);

            noRadioButton!.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_NO_RADIO_BUTTON;
            yesRadioButton!.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_YES_RADIO_BUTTON;

            noRadioButton.Toggle();
        }

        private async void OnExitClick(object sender, EventArgs args)
        {
            bool isOkPressed = await DialogUtils.DisplayDialogAsync(this, _viewModel.CloseDialogViewModel);
            if (isOkPressed)
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user is returning to Infection Status", null,
                    GetCorrelationId());
                GoToInfectionStatusPage();
            }
        }

        private void GoToInfectionStatusPage()
        {
            NavigationHelper.GoToResultPageAndClearTop(this);
        }

        private void GoToPreSharePage()
        {
            StartActivity(new Intent(this, typeof(QuestionnairePreShareActivity)));
        }

        private void OnNextButtonClick(object sender, EventArgs args)
        {
            List<CountryDetailsViewModel> countries =
                FindViewById<RadioButton>(Resource.Id.radio_yes).Checked
                    ? _countries
                    : new List<CountryDetailsViewModel>();
            _viewModel.InvokeNextButtonClick(GoToPreSharePage, OnFail, countries);
        }

        //If the server fails, then we just skip this page.
        private void OnServerError()
        {
            LogUtils.LogMessage(LogSeverity.ERROR,
                $"{nameof(QuestionnaireCountriesSelectionActivity)}.{nameof(OnServerError)}: " +
                "Skipping language selection because countries failed to be fetched. (Android)");
            _countries = new List<CountryDetailsViewModel>();
            ShowSpinner(false);
            OnNextButtonClick(null, null);
        }

        //Is only invoked if data was garbage collected or miba data validation fails.
        private void OnFail()
        {
            ShowSpinner(false);
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireCountriesSelectionActivity)}.{nameof(OnFail)}: " +
                "AuthenticationState.personaldata was garbage collected (Android)");
        }

        private void ShowSpinner(bool show)
        {
            if (show)
            {
                _nextButton.Enabled = false;
                _nextButton.Visibility = ViewStates.Invisible;
                _progressBar.Visibility = ViewStates.Visible;
                _recyclerView.Visibility = ViewStates.Invisible;
            }
            else
            {
                _nextButton.Enabled = true;
                _nextButton.Visibility = ViewStates.Visible;
                _progressBar.Visibility = ViewStates.Gone;
                _recyclerView.Visibility = ViewStates.Visible;
            }
        }
    }
}