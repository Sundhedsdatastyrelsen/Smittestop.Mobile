using System;
using CoreGraphics;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class RadioButton : UIButton, IDisposable
    {
        private const int _borderWidth = 2;
        private UIView _innerView;

        private bool _selected;

        public RadioButton(IntPtr handle) : base(handle)
        {
            BackgroundColor = UIColor.Clear;
            Layer.BorderWidth = _borderWidth;
            Layer.BorderColor = UIColor.White.CGColor;
            TouchUpInside += OnTouchUpInside;

            AddInnerView();
            UpdateState();
        }

        private int _padding => _borderWidth * 3;

        public new bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                UpdateState();
            }
        }

        public new void Dispose()
        {
            TouchUpInside -= OnTouchUpInside;
            _innerView.RemoveFromSuperview();
            _innerView = null;
            base.Dispose();
        }


        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            SetTitle("", UIControlState.Normal);
            UpdateCornerRadius();
        }

        private void UpdateCornerRadius()
        {
            Layer.CornerRadius = Layer.Frame.Height / 2;
            _innerView.Layer.CornerRadius = _innerView.Layer.Frame.Height / 2;
        }

        private void OnTouchUpInside(object sender, EventArgs e)
        {
            Selected = !Selected;
        }

        private void UpdateState()
        {
            _innerView.Hidden = !Selected;

            if (Selected)
            {
                AccessibilityTraits |= UIAccessibilityTrait.Selected;
            }
            else
            {
                AccessibilityTraits &= ~UIAccessibilityTrait.Selected;
            }
        }

        private void AddInnerView()
        {
            _innerView = new UIView();

            _innerView.TranslatesAutoresizingMaskIntoConstraints = false;

            _innerView.BackgroundColor = UIColor.White;
            _innerView.UserInteractionEnabled = false;

            AddSubview(_innerView);
            SetInnerViewConstraints();
        }

        private void SetInnerViewConstraints()
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _innerView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, _padding),
                _innerView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -_padding),
                _innerView.TopAnchor.ConstraintEqualTo(TopAnchor, _padding),
                _innerView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -_padding)
            });

            SetNeedsLayout();
        }
    }
}