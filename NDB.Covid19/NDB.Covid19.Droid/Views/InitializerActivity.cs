using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using I18NPortable;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views
{
    [Activity(MainLauncher = true, Theme = "@style/AppTheme.Launcher", ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTop)]
    public class InitializerActivity : Activity
    {
        private RelativeLayout _continueInEnRelativeLayoutButton;
        private TextView _continueInEnTextView;
        private Button _launcherButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (!IsTaskRoot)
            {
                Finish();
            }

            base.OnCreate(savedInstanceState);


            if (OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted)
            {
                NavigationHelper.GoToWelcomeWhatsNewPage(this);
                Finish();
                return;
            }

            SetContentView(Resource.Layout.layout_with_launcher_button_ag_api);
            _launcherButton = FindViewById<Button>(Resource.Id.launcher_button);
            _continueInEnRelativeLayoutButton = FindViewById<RelativeLayout>(Resource.Id.continue_in_en_layout);
            _continueInEnTextView = FindViewById<TextView>(Resource.Id.continue_in_en_text);

            _launcherButton.Text = InitializerViewModel.LAUNCHER_PAGE_START_BTN;
            _continueInEnTextView.Text = InitializerViewModel.LAUNCHER_PAGE_CONTINUE_IN_ENG;

            _launcherButton.Click += new SingleClick(LauncherButton_Click).Run;
            _continueInEnRelativeLayoutButton.Click += new SingleClick(ContinueInEnButton_Click).Run;
            SetLogoBasedOnAppLanguage();
        }

        private void SetLogoBasedOnAppLanguage()
        {
            ImageView logo = FindViewById<ImageView>(Resource.Id.launcer_icon_imageview);
            string appLanguage = LocalesService.GetLanguage();
            logo?.SetImageResource(appLanguage != null && appLanguage.ToLower() == "en"
                ? Resource.Drawable.health_department_en
                : Resource.Drawable.health_department_da);

            if (logo?.LayoutParameters != null)
            {
                DisplayMetrics displayMetrics = new DisplayMetrics();
                WindowManager?.DefaultDisplay?.GetMetrics(displayMetrics);
                float logicalDensity = displayMetrics.Density;
                int px = (int) Math.Ceiling((appLanguage != null && appLanguage.ToLower() == "en" ? 120 : 180) *
                                            logicalDensity);
                logo.LayoutParameters.Width = px;
            }

            logo?.RequestLayout();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                NavigationHelper.GoToStartPageIfIsOnboarded(this);
            }
            else
            {
                ShowOutdatedGPSDialog();
            }
        }

        private void LauncherButton_Click(object sender, EventArgs e)
        {
            LocalPreferencesHelper.SetAppLanguage("da");
            LocalesService.Initialize();
            Continue();
        }

        private void ShowOutdatedGPSDialog()
        {
            DialogUtils.DisplayDialogAsync(
                this,
                "BASE_ERROR_TITLE".Translate(),
                "LAUNCHER_PAGE_GPS_VERSION_DIALOG_MESSAGE_ANDROID".Translate(),
                "ERROR_OK_BTN".Translate()
            );
        }

        private void Continue()
        {
            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                NavigationHelper.GoToOnBoarding(this, true);
            }
            else
            {
                ShowOutdatedGPSDialog();
            }
        }

        private void ContinueInEnButton_Click(object sender, EventArgs e)
        {
            LocalPreferencesHelper.SetAppLanguage("en");
            LocalesService.Initialize();
            Continue();
        }
    }
}