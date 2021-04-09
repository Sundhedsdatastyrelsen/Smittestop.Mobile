using System;
using System.Collections.Generic;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.Welcome.ChildViews;
using UIKit;

namespace NDB.Covid19.iOS.Views.Welcome
{
    public partial class WelcomePageViewController : UIPageViewController
    {
        private PageViewController _currentPage;

        public List<string> PageTitles = new List<string>
            {"WelcomePageOne", "WelcomePageTwo", "WelcomePageFour", "WelcomePageThree"};

        public WelcomePageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _currentPage = ViewControllerAtIndex(0);
            PageViewController[] viewControllers = {_currentPage};

            SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
        }

        public int GoToNextPage()
        {
            _currentPage = NextViewController();
            PageViewController[] viewControllers = {_currentPage};
            SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
            return _currentPage.PageIndex;
        }

        public int GoToPreviousPage()
        {
            _currentPage = PreviousViewController();
            PageViewController[] viewControllers = {_currentPage};
            SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Reverse, true, null);
            return _currentPage.PageIndex;
        }

        public PageViewController ViewControllerAtIndex(int index)
        {
            PageViewController vc =
                NavigationHelper.ViewControllerByStoryboardName(PageTitles[index]) as PageViewController;
            vc.PageIndex = index;
            return vc;
        }

        public PageViewController NextViewController()
        {
            int index = _currentPage.PageIndex;
            index++;
            return index == PageTitles.Count ? null : ViewControllerAtIndex(index);
        }

        public PageViewController PreviousViewController()
        {
            int index = _currentPage.PageIndex;
            if (index == 0)
            {
                return null;
            }

            index--;
            return ViewControllerAtIndex(index);
        }
    }
}