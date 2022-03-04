using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using CommonServiceLocator;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.SmittestopNotActiveViewModel;

namespace NDB.Covid19.Droid.Views.FarewellSmittestop
{
    [Activity(MainLauncher = true,
        Label = "SmittestopNotActivePageActivity",
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTop)]
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

            SetLogoBasedOnAppLanguage();

            CheckOnboardingStatus();
        }

        private void CheckOnboardingStatus()
        {
            if (OnboardingStatusHelper.Status != OnboardingStatus.NoConsentsGiven)
            {
                NavigationHelper.GoToFarewellSmittestopPage(this);
            }
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
            Recreate();
        }

        private void SetLogoBasedOnAppLanguage()
        {
            ImageView logo = FindViewById<ImageView>(Resource.Id.health_department_imageview);
            string appLanguage = LocalesService.GetLanguage();
            logo?.SetImageResource(appLanguage != null && appLanguage.ToLower() == "en"
                ? Resource.Drawable.health_department_en
                : Resource.Drawable.health_department_da);

            if (logo?.LayoutParameters != null)
            {
                DisplayMetrics displayMetrics = new DisplayMetrics();
                WindowManager?.DefaultDisplay?.GetMetrics(displayMetrics);
                float logicalDensity = displayMetrics.Density;
                int px = (int)Math.Ceiling((appLanguage != null && appLanguage.ToLower() == "en" ? 120 : 180) *
                                            logicalDensity);
                logo.LayoutParameters.Width = px;
            }

            logo?.RequestLayout();
        }
    }
}