using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using static NDB.Covid19.ViewModels.FarewellSmittestopViewModel;

namespace NDB.Covid19.Droid.Views.FarewellSmittestop
{
    [Activity(Label = "FarewellSmittestopPageActivity", Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class FarewellSmittestopPageActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.farewell_smittestop_page);

            TextView title = FindViewById<TextView>(Resource.Id.farewell_page_title);
            title.Text = FAREWELL_SMITTESTOP_TITLE;

            SetBulletText(Resource.Id.bullet_one, FAREWELL_SMITTESTOP_BODY_ONE);
            SetBulletText(Resource.Id.bullet_two, FAREWELL_SMITTESTOP_BODY_TWO);
            SetBulletText(Resource.Id.bullet_three, FAREWELL_SMITTESTOP_BODY_THREE);

            Button button = FindViewById<Button>(Resource.Id.ok_button);
            button.Text = FAREWELL_SMITTESTOP_BUTTON_TEXT;

            // TODO: add button click action

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
    }
}
