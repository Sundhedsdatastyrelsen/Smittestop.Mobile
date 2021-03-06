using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.ViewModels.SettingsGeneralViewModel;
using static NDB.Covid19.ViewModels.SettingsViewModel;
using Object = Java.Lang.Object;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    internal class SettingsGeneralActivity : AppCompatActivity
    {
        private readonly SettingsGeneralViewModel _viewModel = new SettingsGeneralViewModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings_general);
            Init();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings General", null);
        }

        private void Init()
        {
            Button backButton = FindViewById<Button>(Resource.Id.arrow_back_general);
            backButton.ContentDescription = SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            TextView titleField = FindViewById<TextView>(Resource.Id.settings_general_title);
            TextView explanationOne = FindViewById<TextView>(Resource.Id.settings_general_explanation);
            TextView explanationTwo = FindViewById<TextView>(Resource.Id.settings_general_explanation_two);
            TextView mobileDataHeader = FindViewById<TextView>(Resource.Id.settings_general_mobile_data_header);
            TextView mobileDataDesc = FindViewById<TextView>(Resource.Id.settings_general_mobile_data_desc);
            TextView languageHeader = FindViewById<TextView>(Resource.Id.settings_general_select_lang_header);
            TextView languageDesc = FindViewById<TextView>(Resource.Id.settings_general_select_lang_desc_one);
            TextView languageLink = FindViewById<TextView>(Resource.Id.settings_general_link);

            TextView linkLayout = FindViewById<TextView>(Resource.Id.settings_general_link);

            titleField.Text = SETTINGS_GENERAL_TITLE;
            explanationOne.Text = SETTINGS_GENERAL_EXPLANATION_ONE;
            explanationTwo.Text = SETTINGS_GENERAL_EXPLANATION_TWO;
            mobileDataHeader.Text = SETTINGS_GENERAL_MOBILE_DATA_HEADER;
            mobileDataDesc.Text = SETTINGS_GENERAL_MOBILE_DATA_DESC;
            languageHeader.Text = SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER;
            languageDesc.Text = SETTINGS_GENERAL_RESTART_REQUIRED_TEXT;
            languageLink.Text = SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT;
            languageLink.ContentDescription = SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT;

            linkLayout.Click +=
                new StressUtils.SingleClick(
                    (o, args) => OpenSmitteStopLink()).Run;

            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.settings_general_select_lang_radio_group);
            RadioButton englishRadioButton = FindViewById<RadioButton>(Resource.Id.settings_general_english);
            RadioButton danishRadioButton = FindViewById<RadioButton>(Resource.Id.settings_general_danish);

            englishRadioButton.Text = SETTINGS_GENERAL_EN;
            danishRadioButton.Text = SETTINGS_GENERAL_DA;

            string appLanguage = LocalesService.GetLanguage();

            if (appLanguage == "en")
            {
                englishRadioButton.Checked = true;
            }
            else
            {
                danishRadioButton.Checked = true;
            }

            radioGroup.SetOnCheckedChangeListener(new OnCheckedChangeListener(this));

            SwitchCompat switchButton = FindViewById<SwitchCompat>(Resource.Id.settings_general_switch);
            switchButton.Checked = _viewModel.GetStoredCheckedState();
            switchButton.CheckedChange += OnCheckedChange;

            backButton.Click += new StressUtils.SingleClick((sender, args) => Finish()).Run;
        }

        private async void OnCheckedChange(object obj, EventArgs args)
        {
            SwitchCompat switchButton = (SwitchCompat) obj;
            if (!switchButton.Checked && !await DialogUtils.DisplayDialogAsync(this, AreYouSureDialogViewModel))
            {
                switchButton.Checked = true;
            }

            _viewModel.OnCheckedChange(switchButton.Checked);
        }

        private class OnCheckedChangeListener : Object, RadioGroup.IOnCheckedChangeListener
        {
            private readonly SettingsGeneralActivity _self;

            public OnCheckedChangeListener(SettingsGeneralActivity self)
            {
                _self = self;
            }

            public async void OnCheckedChanged(RadioGroup group, int checkedId)
            {
                switch (checkedId)
                {
                    case Resource.Id.settings_general_english:
                        await DialogUtils.DisplayDialogAsync(_self, GetChangeLanguageViewModel);
                        LocalPreferencesHelper.SetAppLanguage("en");
                        break;
                    case Resource.Id.settings_general_danish:
                        await DialogUtils.DisplayDialogAsync(_self, GetChangeLanguageViewModel);
                        LocalPreferencesHelper.SetAppLanguage("da");
                        break;
                }

                LocalesService.SetInternationalization();
            }
        }
    }
}