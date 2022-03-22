using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;
using Action = Android.Views.Accessibility.Action;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace NDB.Covid19.Droid.Views.Welcome
{
    [Activity(Label = "", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTop)]
    public class WelcomeActivity : BaseAppCompatActivity, ViewPager.IOnPageChangeListener
    {
        private readonly WelcomePageFourFragment _welcomePageFour = new WelcomePageFourFragment();
        private readonly WelcomePageOneFragment _welcomePageOne = new WelcomePageOneFragment();
        private readonly WelcomePageThreeFragment _welcomePageThree = new WelcomePageThreeFragment();
        private readonly WelcomePageTwoFragment _welcomePageTwo = new WelcomePageTwoFragment();
        private Button _button;
        private TabLayout _dotLayout;
        private int _numPages;
        private NonSwipeableViewPager _pager;
        private List<Fragment> _pages;
        private Button _previousButton;
        public bool IsOnBoarding;

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageSelected(int position)
        {
            ScrollToTop();
            _previousButton.Visibility = position == 0 ? ViewStates.Invisible : ViewStates.Visible;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (State(savedInstanceState) == AppState.IsDestroyed)
            {
                return;
            }

            IsOnBoarding = Intent.GetBooleanExtra(DroidRequestCodes.isOnBoardinIntentExtra, false);

            if (IsOnBoarding && OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted)
            {
                GoToConsents();
                return;
            }

            _pages = new List<Fragment>(new Fragment[]
                {_welcomePageOne, _welcomePageTwo, _welcomePageFour, _welcomePageThree});
            _numPages = _pages.Count;

            SetContentView(Resource.Layout.welcome);

            _button = FindViewById<Button>(Resource.Id.buttonGetStarted);
            _previousButton = FindViewById<Button>(Resource.Id.buttonPrev);

            _previousButton.Text = WelcomeViewModel.PREVIOUS_PAGE_BUTTON_TEXT;
            _button.Text = WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT;
            _button.Click += new SingleClick(GetNextButton_Click, 500).Run;
            _previousButton.Click += new SingleClick(GetPreviousButton_Click, 500).Run;
            _previousButton.Visibility = ViewStates.Invisible;

            WelcomePagerAdapter adapter = new WelcomePagerAdapter(SupportFragmentManager, _pages);
            _pager = FindViewById<NonSwipeableViewPager>(Resource.Id.fragment);
            _pager.Adapter = adapter;
            _pager.SetPagingEnabled(false);
            _pager.AddOnPageChangeListener(this);
            _pager.AnnounceForAccessibility(IsOnBoarding
                ? WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_ONE
                : WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_ONE);

            _dotLayout = FindViewById<TabLayout>(Resource.Id.tabDots);
            _dotLayout.SetupWithViewPager(_pager, true);
        }

        protected override Intent GetStartingNewIntent()
        {
            return NavigationHelper.GetStartPageIntent(this);
        }

        private void GetNextButton_Click(object sender, EventArgs e)
        {
            ScrollToTop();

            if (_numPages == _pager.CurrentItem + 1)
            {
                if (IsOnBoarding)
                {
                    GoToConsents();
                    return;
                }

                RunOnUiThread(Finish);
                return;
            }

            _pager.SetCurrentItem(_pager.CurrentItem + 1, true);

            AnnouncePageChangesForScreenReaders();
        }

        private void GoToConsents()
        {
            Intent intent = new Intent(this, typeof(WelcomePageConsentsActivity));
            StartActivity(intent);
        }

        private void AnnouncePageChangesForScreenReaders()
        {
            // Change focus to fragment
            _pager.PerformAccessibilityAction(Action.AccessibilityFocus, null);

            Fragment activeFragment = _pages[_pager.CurrentItem];

            if (activeFragment == _welcomePageOne)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_ONE);
            }
            else if (activeFragment == _welcomePageTwo)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_TWO);
            }
            else if (activeFragment == _welcomePageThree)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_THREE);
            }
            else if (activeFragment == _welcomePageFour)
            {
                _pager.AnnounceForAccessibility(WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_FOUR);
            }
        }

        private void GetPreviousButton_Click(object sender, EventArgs e)
        {
            ScrollToTop();
            _pager.SetCurrentItem(_pager.CurrentItem - 1, true);
            _button.Visibility = ViewStates.Visible;
            AnnouncePageChangesForScreenReaders();
        }

        private void ScrollToTop()
        {
            (_pager.Adapter as WelcomePagerAdapter)?.GetItem(_pager.CurrentItem)?.View.ScrollTo(0, 0);
        }
    }
}