using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePageThreeFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.welcome_page_three, container, false);
            TextView bodyOne = view.FindViewById<TextView>(Resource.Id.welcome_page_three_body_one);
            TextView bodyTwo = view.FindViewById<TextView>(Resource.Id.welcome_page_three_body_two);
            TextView header = view.FindViewById<TextView>(Resource.Id.welcome_page_three_title);
            TextView infoBoxBody = view.FindViewById<TextView>(Resource.Id.welcome_page_three_infobox_body);

            bodyOne.Text = WelcomeViewModel.WELCOME_PAGE_THREE_BODY_ONE;
            bodyOne.ContentDescription = WelcomeViewModel.WELCOME_PAGE_THREE_BODY_ONE_ACCESSIBILITY;
            bodyTwo.Text = WelcomeViewModel.WELCOME_PAGE_THREE_BODY_TWO;
            header.Text = WelcomeViewModel.WELCOME_PAGE_THREE_TITLE;
            infoBoxBody.Text = WelcomeViewModel.WELCOME_PAGE_THREE_INFOBOX_BODY;

            infoBoxBody.ContentDescription = WelcomeViewModel.WELCOME_PAGE_THREE_INFOBOX_BODY;

            WelcomePageTools.SetArrowVisibility(view);

            return view;
        }
    }
}