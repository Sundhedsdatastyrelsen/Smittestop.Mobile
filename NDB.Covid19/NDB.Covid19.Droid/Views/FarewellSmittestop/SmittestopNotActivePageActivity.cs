using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
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

            Button button = FindViewById<Button>(Resource.Id.language_button);
            button.Text = SMITTESTOP_NOT_ACTIVE_BUTTONT_TEXT;
        }
    }
}
