using System;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class FocusableButton : UIButton
    {
        public Action OnFocus = () => { };
        public Action OnFocusLost = () => { };

        public FocusableButton(IntPtr handle) : base(handle)
        {
        }

        public override bool IsAccessibilityElement { get; set; } = true;

        public override void AccessibilityElementDidBecomeFocused()
        {
            if (AccessibilityElementIsFocused())
            {
                OnFocus?.Invoke();
            }

            base.AccessibilityElementDidBecomeFocused();
        }

        public override void AccessibilityElementDidLoseFocus()
        {
            base.AccessibilityElementDidLoseFocus();
            OnFocusLost?.Invoke();
        }
    }
}