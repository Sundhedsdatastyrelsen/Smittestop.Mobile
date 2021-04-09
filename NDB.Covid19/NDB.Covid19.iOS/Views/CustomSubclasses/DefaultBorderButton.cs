using System;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class DefaultBorderButton : UIButton, IDisposable
    {
        private UIActivityIndicatorView _spinner;

        public DefaultBorderButton(IntPtr handle) : base(handle)
        {
            Font = Font(FontType.FontSemiBold, 18f, 24f);
            SetTitleColor(UIColor.White, UIControlState.Normal);
            BackgroundColor = UIColor.Clear;
            TitleLabel.AdjustsFontSizeToFitWidth = true;
            SetTitleColor(UIColor.Clear, UIControlState.Selected);
            Layer.BorderWidth = 1;
            Layer.BorderColor = UIColor.White.CGColor;
            Layer.CornerRadius = Layer.Frame.Height / 2;
            TintColor = UIColor.Clear;
        }

        public new void Dispose()
        {
            HideSpinner();
            base.Dispose();
        }

        public override void SetTitle(string title, UIControlState forState)
        {
            base.SetTitle(title, forState);
            Superview.SetNeedsLayout();
            Layer.CornerRadius = Layer.Frame.Height / 2;
        }

        public void ShowSpinner(UIView parentView, UIActivityIndicatorViewStyle style)
        {
            _spinner = AddSpinnerToView(parentView, style);
            CenterView(_spinner, this);

            Selected = true;
            _spinner.StartAnimating();
        }

        public void HideSpinner()
        {
            _spinner?.RemoveFromSuperview();
            _spinner = null;
            Selected = false;
        }
    }
}