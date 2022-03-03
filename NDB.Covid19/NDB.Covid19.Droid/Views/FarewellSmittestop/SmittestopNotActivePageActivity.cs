using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.SmittestopNotActiveViewModel;

namespace NDB.Covid19.Droid.Views.FarewellSmittestop
{
    [Activity(Label = "SmittestopNotActivePageActivity", Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class SmittestopNotActivePageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.smittestop_not_active_page);

            TextView infoText = FindViewById<TextView>(Resource.Id.information_text);
            infoText.Text = SMITTESTOP_NOT_ACTIVE_TEXT;

            TextView moreInfoText = FindViewById<TextView>(Resource.Id.more_info_text);
            moreInfoText.Text = SMITTESTOP_NOT_ACTIVE_MORE_INFO;

            TextView moreInfoURL = FindViewById<TextView>(Resource.Id.more_info_link);
            moreInfoURL.Text = SMITTESTOP_NOT_ACTIVE_INFO_LINK_TEXT;

            Button languageButton = FindViewById<Button>(Resource.Id.language_button);
            languageButton.Text = SMITTESTOP_NOT_ACTIVE_BUTTONT_TEXT;
            languageButton.Click += new SingleClick(LanguageButton_Click).Run;

            RelativeLayout moreInfoRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.more_info_layout);
            moreInfoRelativeLayout.Click += new SingleClick((o, args) => MoreInfoButton_Click()).Run;
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
                    "Failed to open sum.dk link on smittestop not active page");
            }
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            string appLanguage = LocalesService.GetLanguage();
            if (appLanguage == "en")
            {
                LocalPreferencesHelper.SetAppLanguage("da");
            }
            else
            {
                LocalPreferencesHelper.SetAppLanguage("en");
            }
            LocalesService.Initialize();
            this.Recreate();
        }
    }
}