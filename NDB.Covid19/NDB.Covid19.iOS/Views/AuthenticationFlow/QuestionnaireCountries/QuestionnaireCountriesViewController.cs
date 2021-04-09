using System;
using System.Collections.Generic;
using System.Linq;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.Enums.QuestionnaireCountriesVisitedEnum;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.ViewModels.QuestionnaireCountriesViewModel;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries
{
    public partial class QuestionnaireCountriesViewController : BaseViewController
    {
        private List<CountryDetailsViewModel> _countryList;

        private UIActivityIndicatorView _spinner;
        private QuestionnaireCountriesViewModel _viewModel;

        public QuestionnaireCountriesViewController(IntPtr handle) : base(handle)
        {
        }

        public static QuestionnaireCountriesViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("QuestionnaireCountries", null);
            QuestionnaireCountriesViewController vc =
                storyboard.InstantiateInitialViewController() as QuestionnaireCountriesViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new QuestionnaireCountriesViewModel();
            _countryList = new List<CountryDetailsViewModel>();
            SetStyling();
            SetAccessibility();
            AddObservers();
            SetupRadioButtons();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Countries Selection", null,
                GetCorrelationId());
            SetupTableView();
            SetupRadioButtons();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        private void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire Countries Selection", null,
                        GetCorrelationId());
                });
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Countries Selection", null,
                        GetCorrelationId());
                });
        }

        private void SetStyling()
        {
            SetupRadioButtons();
            StyleUtil.InitLabelWithSpacing(TitleLbl, StyleUtil.FontType.FontBold, COUNTRY_QUESTIONAIRE_HEADER_TEXT,
                1.14, 24, 38);
            StyleUtil.InitLabelWithSpacing(SubtitleLbl, StyleUtil.FontType.FontRegular,
                COUNTRY_QUESTIONAIRE_INFORMATION_TEXT, 1.28, 16, 20);
            StyleUtil.InitLabelWithSpacing(ListExplainLbl, StyleUtil.FontType.FontRegular, COUNTRY_QUESTIONAIRE_FOOTER,
                1.28, 16, 20);
            StyleUtil.InitLabelWithSpacing(NoLabel, StyleUtil.FontType.FontRegular,
                COUNTRY_QUESTIONAIRE_NO_RADIO_BUTTON, 1.28, 16, 20);
            StyleUtil.InitLabelWithSpacing(YesLabel, StyleUtil.FontType.FontRegular,
                COUNTRY_QUESTIONAIRE_YES_RADIO_BUTTON, 1.28, 16, 20);
            NextBtn.SetTitle(COUNTRY_QUESTIONAIRE_BUTTON_TEXT, UIControlState.Normal);
            ButtonView.Alpha = 0.9f;
            ButtonView.BackgroundColor = "#001F34".ToUIColor();
        }

        private async void SetupTableView()
        {
            _spinner = StyleUtil.ShowSpinner(View, UIActivityIndicatorViewStyle.WhiteLarge);
            _countryList = await _viewModel.GetListOfCountriesAsync();

            if (!_countryList.Any())
            {
                _spinner?.RemoveFromSuperview();
                OnServerError();
                return;
            }

            InvokeOnMainThread(() =>
            {
                TableViewHeightConstraint.Constant = _countryList.Count * CountryTableCell.ROW_HEIGHT;
                _spinner?.RemoveFromSuperview();
                CountryTableView.RegisterNibForCellReuse(CountryTableCell.Nib, CountryTableCell.Key);
                CountryTableView.Source = new CountryTableViewSource(_countryList);
                CountryTableView.ReloadData();
            });
        }

        private void SetAccessibility()
        {
            ListExplainLbl.AccessibilityLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbolsString(COUNTRY_QUESTIONAIRE_FOOTER);
            CloseButton.AccessibilityLabel =
                QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            DialogHelper.ShowDialog(this, _viewModel.CloseDialogViewModel, CloseConfirmed,
                UIAlertActionStyle.Destructive);
        }

        private void CloseConfirmed(UIAlertAction obj)
        {
            LogUtils.LogMessage(LogSeverity.INFO, "The user is returning to Infection Status", null,
                GetCorrelationId());
            NavigationController?.DismissViewController(true, null);
        }

        partial void NextBtnTapped(DefaultBorderButton sender)
        {
            NextBtn.ShowSpinner(View, UIActivityIndicatorViewStyle.White);

            List<CountryDetailsViewModel> countryList =
                YesRadioButton.Selected ? _countryList : new List<CountryDetailsViewModel>();
            _viewModel.InvokeNextButtonClick(OnSuccess, OnFail, countryList);
        }

        private void OnSuccess()
        {
            NextBtn.HideSpinner();
            NavigationController?.PushViewController(QuestionnairePreShareViewController.Create(), true);
        }

        //If the server fails, then we just skip this page.
        private void OnServerError()
        {
            LogUtils.LogMessage(LogSeverity.ERROR,
                $"{nameof(QuestionnaireCountriesViewController)}.{nameof(OnServerError)}: " +
                "Skipping language selection because countries failed to be fetched. (IOS)");
            _countryList = new List<CountryDetailsViewModel>();
            NextBtnTapped(null);
        }

        //Is only invoked if data was garbage collected.
        private void OnFail()
        {
            NextBtn.HideSpinner();
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireCountriesViewController)}.{nameof(OnFail)}: " +
                "AuthenticationState.personaldata was garbage collected (IOS)");
        }

        private void SetupRadioButtons()
        {
            NoRadioButton.Selected = Selection == No;
            YesRadioButton.Selected = Selection == Yes;
            ManageCountriesListVisibility();
        }

        private void HandleRadioBtnChange(QuestionnaireCountriesVisitedEnum selection, UIButton _)
        {
            Selection = selection;
            SetupRadioButtons();
        }

        private void ManageCountriesListVisibility()
        {
            bool hidden = Selection == No;
            CountryTableView.Hidden = hidden;
            SubtitleLbl.Hidden = hidden;
            ListExplainLbl.Hidden = hidden;
            Divider.Hidden = hidden;
        }

        partial void OnNoRadioButton_TouchUpInside(RadioButton sender)
        {
            HandleRadioBtnChange(No, sender);
        }

        partial void OnYesRadioButton_TouchUpInside(RadioButton sender)
        {
            HandleRadioBtnChange(Yes, sender);
        }
    }
}