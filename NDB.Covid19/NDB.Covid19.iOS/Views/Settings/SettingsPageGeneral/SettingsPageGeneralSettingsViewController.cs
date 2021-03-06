using System;
using System.Diagnostics;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;


namespace NDB.Covid19.iOS.Views.Settings.SettingsPageGeneral
{
    public partial class SettingsPageGeneralSettingsViewController : BaseViewController
    {
        private UITapGestureRecognizer _gestureRecognizer;
        private SettingsGeneralViewModel _viewModel;

        public SettingsPageGeneralSettingsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitLabel(Header, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_TITLE, 24, 28);
            InitLabel(HeaderLabel, FontType.FontRegular, SettingsGeneralViewModel.SETTINGS_GENERAL_EXPLANATION_ONE, 16,
                28);
            InitLabel(ContentLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_MOBILE_DATA_HEADER, 16,
                28);
            InitLabel(ContentLabelOne, FontType.FontRegular, SettingsGeneralViewModel.SETTINGS_GENERAL_EXPLANATION_TWO,
                16, 28);
            InitLabel(DescriptionLabel, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_MOBILE_DATA_DESC, 12, 28);
            InitLabel(ChooseLanguageHeaderLbl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER, 16, 28);
            InitLabel(RadioButton1Lbl, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_DA, 16, 28);
            InitLabel(RadioButton2Lbl, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_EN, 16, 28);
            InitLabel(RestartAppLabl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_RESTART_REQUIRED_TEXT, 12, 28);
            InitLabel(SmittestopLinkButtionLbl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT, 12, 28);

            //Implemented for correct voiceover due to Back button 
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            //Implemented for correct voiceover due to last paragraph and link
            SmittestopLinkButtionLbl.AccessibilityLabel =
                SettingsGeneralViewModel.SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT;

            //Implemented for correct voiceover due to smitte|stop, removing pronunciation of lodretstreg
            ContentLabel.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(SettingsGeneralViewModel
                    .SETTINGS_GENERAL_MOBILE_DATA_HEADER);

            _viewModel = new SettingsGeneralViewModel();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings General", null);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            switchButton.ValueChanged += SwitchValueChanged;
            SetupLinkButton();
            SetupRadioButtons();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            switchButton.ValueChanged -= SwitchValueChanged;
            SmittestopLinkButtonStackView.RemoveGestureRecognizer(_gestureRecognizer);
        }

        private void SetupLinkButton()
        {
            _gestureRecognizer = new UITapGestureRecognizer();
            _gestureRecognizer.AddTarget(() => OnSmittestopLinkButtionStackViewTapped(_gestureRecognizer));
            SmittestopLinkButtonStackView.AddGestureRecognizer(_gestureRecognizer);
        }

        private void SetupRadioButtons()
        {
            string appLanguage = LocalesService.GetLanguage();

            if (appLanguage == "en")
            {
                _viewModel.SetSelection(SettingsLanguageSelection.English);
            }
            else
            {
                _viewModel.SetSelection(SettingsLanguageSelection.Danish);
            }

            RadioButton1.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Danish;
            RadioButton2.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.English;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
            RadioButton1.Enabled = false;
            RadioButton2.Enabled = false;
        }

        public void SwitchValueChanged(object sender, EventArgs e)
        {
            Debug.Print("Switch clicked. Is on " + switchButton.On);

            if (!switchButton.On)
            {
                DialogHelper.ShowDialog(
                    this,
                    SettingsGeneralViewModel.AreYouSureDialogViewModel,
                    action => { _viewModel.OnCheckedChange(switchButton.On); },
                    UIAlertActionStyle.Default,
                    action =>
                    {
                        switchButton.On = true;
                        _viewModel.OnCheckedChange(switchButton.On);
                    });
            }
        }

        private void OnSmittestopLinkButtionStackViewTapped(UITapGestureRecognizer recognizer)
        {
            SettingsGeneralViewModel.OpenSmitteStopLink();
        }

        private void HandleRadioBtnChange(SettingsLanguageSelection selection, UIButton sender)
        {
            if (SettingsGeneralViewModel.Selection == selection)
            {
                RadioButton1.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Danish;
                RadioButton2.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.English;
                return;
            }

            switch (selection)
            {
                case SettingsLanguageSelection.Danish:
                    DialogHelper.ShowDialog(this, SettingsGeneralViewModel.GetChangeLanguageViewModel, Action => { });
                    LocalPreferencesHelper.SetAppLanguage("da");
                    break;
                case SettingsLanguageSelection.English:
                    DialogHelper.ShowDialog(this, SettingsGeneralViewModel.GetChangeLanguageViewModel, Action => { });
                    LocalPreferencesHelper.SetAppLanguage("en");
                    break;
            }

            LocalesService.SetInternationalization();
            SetupRadioButtons();
        }

        partial void RadioButton1_TouchUpInside(RadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Danish, sender);
        }

        partial void RadioButton2_TouchUpInside(RadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.English, sender);
        }
    }
}