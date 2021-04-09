using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.SettingsPage4ViewModel;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    internal class SettingsHelpActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = HEADER;
            SetContentView(Resource.Layout.settings_help);
            Init();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Settings Help", null);
        }

        private void Init()
        {
            ImageButton backButton = FindViewById<ImageButton>(Resource.Id.arrow_back_help);
            backButton.ContentDescription = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            TextView textField = FindViewById<TextView>(Resource.Id.settings_help_text);
            TextView titleField = FindViewById<TextView>(Resource.Id.settings_help_title);
            TextView hiddenLink = FindViewById<TextView>(Resource.Id.settings_help_link);

            titleField.Text = HEADER;
            textField.TextFormatted =
                HtmlCompat.FromHtml($"{CONTENT_TEXT_BEFORE_SUPPORT_LINK}<br><br>" +
                                    $"{EMAIL_TEXT}<br><a href=\"mailto:{EMAIL}\">{EMAIL}</a><br>{PHONE_NUM_Text}<br><a href=\"tel:{PHONE_NUM}\">{PHONE_NUM}</a><br><br>" +
                                    $"{SUPPORT_TEXT}", HtmlCompat.FromHtmlModeLegacy);
            textField.ContentDescriptionFormatted =
                HtmlCompat.FromHtml($"{CONTENT_TEXT_BEFORE_SUPPORT_LINK}<br><br>" +
                                    $"{EMAIL_TEXT}<br><a href=\"mailto:{EMAIL}\">{EMAIL}</a><br>{PHONE_NUM_Text}<br><a href=\"tel:{PHONE_NUM}\">{PHONE_NUM_ACCESSIBILITY}</a><br><br>" +
                                    $"{ACCESSIBILITY_SUPPORT_TEXT}", HtmlCompat.FromHtmlModeLegacy);
            textField.MovementMethod = LinkMovementMethod.Instance;
            backButton.Click += new SingleClick((sender, args) => Finish()).Run;

            hiddenLink.Text = SUPPORT_LINK;
            hiddenLink.ContentDescription = SUPPORT_LINK_SHOWN_TEXT;

            LinkUtil.LinkifyTextView(hiddenLink);
        }
    }
}