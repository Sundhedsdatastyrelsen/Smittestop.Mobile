using System.Collections.Generic;
using AndroidX.Fragment.App;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class WelcomePagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Fragment> pages;

        public WelcomePagerAdapter(FragmentManager fm, List<Fragment> pages)
            : base(fm)
        {
            this.pages = pages;
        }

        public override int Count => pages.Count;

        public override Fragment GetItem(int position)
        {
            return pages[position];
        }
    }
}