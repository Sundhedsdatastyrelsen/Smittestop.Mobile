using System;
using System.Linq;
using Foundation;
using NDB.Covid19.iOS.Utils;
using ObjCRuntime;
using UIKit;

namespace NDB.Covid19.iOS.Views.HelpCustomView
{
    public partial class HelpCustomView : UIView
    {
        private string _buttonText;

        private string _displayText;
        private bool _eventHandlersSet;
        private UIView _initiaterView;
        private bool _stylingAlreadySet;

        public HelpCustomView(IntPtr handle) : base(handle)
        {
        }

        /// <summary>
        ///     This method instantiates and add the HelpCustomView to the parentView provided.
        /// </summary>
        /// <param name="parentView"></param>
        /// <returns></returns>
        public static HelpCustomView Create(UIView parentView, string displayText, string buttonText,
            UIView initiaterView)
        {
            NSArray arr = NSBundle.MainBundle.LoadNib(nameof(HelpCustomView), null, null);
            HelpCustomView v = Runtime.GetNSObject<HelpCustomView>(arr.ValueAt(0));
            v._displayText = displayText;
            v._buttonText = buttonText;
            v._initiaterView = initiaterView;
            EmbedInParentView(v, parentView);

            return v;
        }

        private static void EmbedInParentView(UIView childView, UIView parentView)
        {
            childView.WillMoveToSuperview(parentView);
            parentView.AddSubview(childView);
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                childView.LeadingAnchor.ConstraintEqualTo(parentView.LeadingAnchor),
                childView.TrailingAnchor.ConstraintEqualTo(parentView.TrailingAnchor),
                childView.TopAnchor.ConstraintEqualTo(parentView.TopAnchor),
                childView.BottomAnchor.ConstraintEqualTo(parentView.BottomAnchor)
            });
            childView.MovedToSuperview();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TranslatesAutoresizingMaskIntoConstraints = false;

            BackgroundView.IsAccessibilityElement = true;
            BackgroundView.AccessibilityLabel = "Luk";
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!_stylingAlreadySet)
            {
                SetupTextAndStyling();
            }
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);

            if (newsuper != null && !_eventHandlersSet)
            {
                SetupTapRecognizerOnBackgroundView();

                _eventHandlersSet = true;
            }
        }

        public override void MovedToSuperview()
        {
            base.MovedToSuperview();

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, TextLabel);
        }

        public override void RemoveFromSuperview()
        {
            base.RemoveFromSuperview();

            RemoveGestureRecognizers();
            _eventHandlersSet = false;
        }

        private void Close()
        {
            Superview.WillRemoveSubview(this);
            RemoveFromSuperview();
            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, _initiaterView);
        }

        private void SetupTapRecognizerOnBackgroundView()
        {
            UITapGestureRecognizer tap = new UITapGestureRecognizer(Close);
            BackgroundView.AddGestureRecognizer(tap);
        }

        private void RemoveGestureRecognizers()
        {
            if (BackgroundView.GestureRecognizers != null && BackgroundView.GestureRecognizers.Any())
            {
                foreach (UIGestureRecognizer gesture in BackgroundView.GestureRecognizers)
                {
                    BackgroundView.RemoveGestureRecognizer(gesture);
                }
            }
        }

        private void SetupTextAndStyling()
        {
            TextContainerView.Layer.CornerRadius = 12;
            TextLabel.Text = _displayText;
            TextLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 17);

            CloseBtn.SetTitle(_buttonText, UIControlState.Normal);
            CloseBtn.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 17);

            _stylingAlreadySet = true;
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            Close();
        }
    }
}