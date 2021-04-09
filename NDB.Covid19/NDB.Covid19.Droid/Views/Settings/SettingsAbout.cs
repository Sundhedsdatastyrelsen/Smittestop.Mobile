using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop,
        WindowSoftInputMode = SoftInput.AdjustResize)]
    internal class SettingsAbout : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = SettingsPage5ViewModel.SETTINGS_PAGE_5_HEADER;
            SetContentView(Resource.Layout.settings_about);
            Init();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings About", null);
        }

        private void Init()
        {
            Button backButton = FindViewById<Button>(Resource.Id.arrow_back_about);
            backButton.ContentDescription = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            TextView titleField = FindViewById<TextView>(Resource.Id.settings_about_title);
            TextView textField = FindViewById<TextView>(Resource.Id.settings_about_text);
            TextView hiddenLink = FindViewById<TextView>(Resource.Id.settings_about_link);
            FindViewById<TextView>(Resource.Id.settings_about_version_info_textview).Text =
                SettingsPage5ViewModel.GetVersionInfo();

            titleField.Text = SettingsPage5ViewModel.SETTINGS_PAGE_5_HEADER;
            textField.Text = SettingsPage5ViewModel.SETTINGS_PAGE_5_CONTENT +
                             $" {SettingsPage5ViewModel.SETTINGS_PAGE_5_LINK}";

            backButton.Click += new SingleClick((sender, args) => Finish()).Run;

            hiddenLink.Text = SettingsPage5ViewModel.SETTINGS_PAGE_5_LINK;

            LinkUtil.LinkifyTextView(hiddenLink);
        }
    }
}