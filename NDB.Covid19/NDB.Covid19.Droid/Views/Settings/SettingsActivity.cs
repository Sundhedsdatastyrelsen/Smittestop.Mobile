using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    internal class SettingsActivity : AppCompatActivity
    {
        private static readonly SettingsViewModel _settingsViewModel = new SettingsViewModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings_page);
            Init();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings", null);
        }

        private void Init()
        {
            ConstraintLayout settingsIntroLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_intro_frame);
            ConstraintLayout howItWorksLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_saddan_frame);
            ConstraintLayout gdprLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_behandling_frame);
            ConstraintLayout helpLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_hjaelp_frame);
            ConstraintLayout aboutLayout = FindViewById<ConstraintLayout>(Resource.Id.om_frame);
            ConstraintLayout deploymentLayout = FindViewById<ConstraintLayout>(Resource.Id.test_frame);
            ConstraintLayout generalLayout = FindViewById<ConstraintLayout>(Resource.Id.general_settings);

            Button settingsIntroButton = settingsIntroLayout.FindViewById<Button>(Resource.Id.settings_link_text);
            Button howItWorksButton = howItWorksLayout.FindViewById<Button>(Resource.Id.settings_link_text);
            Button gdprButton = gdprLayout.FindViewById<Button>(Resource.Id.settings_link_text);
            Button helpButton = helpLayout.FindViewById<Button>(Resource.Id.settings_link_text);
            Button aboutButton = aboutLayout.FindViewById<Button>(Resource.Id.settings_link_text);
            Button generalButton = generalLayout.FindViewById<Button>(Resource.Id.settings_link_text);
            Button deploymentButton = deploymentLayout.FindViewById<Button>(Resource.Id.settings_link_text);

            settingsIntroButton.Text = _settingsViewModel.SettingItemList[0].Text;
            howItWorksButton.Text = _settingsViewModel.SettingItemList[1].Text;
            gdprButton.Text = _settingsViewModel.SettingItemList[2].Text;
            helpButton.Text = _settingsViewModel.SettingItemList[3].Text;
            aboutButton.Text = _settingsViewModel.SettingItemList[4].Text;
            generalButton.Text = _settingsViewModel.SettingItemList[5].Text;

            if (_settingsViewModel.ShowDebugItem)
            {
                deploymentButton.Text = _settingsViewModel.SettingItemList[6].Text;
                deploymentLayout.Visibility = ViewStates.Visible;
            }

            ViewGroup closeButton = FindViewById<ViewGroup>(Resource.Id.ic_close_white);
            closeButton.ContentDescription = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            closeButton.Click += new SingleClick((sender, e) => Finish()).Run;
            settingsIntroButton.Click +=
                new SingleClick((sender, args) => NavigationHelper.GoToOnBoarding(this, false)).Run;
            howItWorksButton.Click +=
                new SingleClick((sender, args) => NavigationHelper.GoToSettingsHowItWorksPage(this)).Run;
            helpButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToSettingsHelpPage(this)).Run;
            aboutButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToSettingsAboutPage(this)).Run;
            gdprButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToConsentsWithdrawPage(this)).Run;
            deploymentButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToDebugPage(this)).Run;
            generalButton.Click +=
                new SingleClick((sender, args) => NavigationHelper.GoToGenetalSettingsPage(this)).Run;
        }
    }
}