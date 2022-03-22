using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using CommonServiceLocator;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.FarewellSmittestopViewModel;

namespace NDB.Covid19.Droid.Views.FarewellSmittestop
{
    [Activity(Label = "FarewellSmittestopPageActivity",
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTop)]
    public class FarewellSmittestopPageActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetupView();
        }

        private void SetupView()
        {
            SetContentView(Resource.Layout.farewell_smittestop_page);

            TextView title = FindViewById<TextView>(Resource.Id.farewell_page_title);
            title.Text = FAREWELL_SMITTESTOP_TITLE;

            SetBulletText(Resource.Id.bullet_one, FAREWELL_SMITTESTOP_BODY_ONE);
            SetBulletText(Resource.Id.bullet_two, FAREWELL_SMITTESTOP_BODY_TWO);
            SetBulletText(Resource.Id.bullet_three, FAREWELL_SMITTESTOP_BODY_THREE);

            SetupMoreInfo();

            Button button = FindViewById<Button>(Resource.Id.ok_button);
            button.Text = FAREWELL_SMITTESTOP_BUTTON_TEXT;
            button.Click += new SingleClick(OkButtonClick).Run;
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            DeviceUtils.StopScanServices(); // Stop scan services if running.
            DeviceUtils.CleanDataFromDevice(); // Clean data from device.
            Finish();
        }

        private void MoreInfoButton_Click()
        {
            try
            {
                ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(SMITTESTOP_NOT_ACTIVE_INFO_URL);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e,
                    "Failed to open smittestop.dk link on smittestop farewell page");
            }
        }

        private void SetBulletText(int resourceId, string textContent)
        {
            LinearLayout bullet = FindViewById<LinearLayout>(resourceId);
            CheckBox bulletCheckBox = bullet.FindViewById<CheckBox>(Resource.Id.bulletText);
            if (bulletCheckBox != null)
            {
                bulletCheckBox.TextFormatted = HtmlCompat.FromHtml(textContent, HtmlCompat.FromHtmlModeLegacy);
            }
        }

        private void SetupMoreInfo()
        {
            TextView moreInfoText = FindViewById<TextView>(Resource.Id.more_info_text);
            moreInfoText.Text = SMITTESTOP_NOT_ACTIVE_MORE_INFO;
            TextView moreInfoURL = FindViewById<TextView>(Resource.Id.more_info_link);
            moreInfoURL.Text = SMITTESTOP_NOT_ACTIVE_INFO_LINK_TEXT;
            RelativeLayout moreInfoRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.more_info_layout);
            moreInfoRelativeLayout.Click += new SingleClick((o, args) => MoreInfoButton_Click()).Run;

            // Only show for danish.
            string appLanguage = LocalesService.GetLanguage();
            if(appLanguage != null && appLanguage.ToLower() == "da") {
                moreInfoRelativeLayout.Visibility = Android.Views.ViewStates.Visible;
            } else
            {
                moreInfoRelativeLayout.Visibility = Android.Views.ViewStates.Invisible;
            }
        }
    }
}
