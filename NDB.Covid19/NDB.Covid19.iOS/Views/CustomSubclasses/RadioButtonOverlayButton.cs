using System;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class RadioButtonOverlayButton : UIButton, IDisposable
    {
        private bool _selected;

        public RadioButtonOverlayButton(IntPtr handle) : base(handle)
        {
            TouchUpInside += OnTouchUpInside;

            UpdateState();
        }

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
            base.Dispose();
        }

        private void OnTouchUpInside(object sender, EventArgs e)
        {
            Selected = !Selected;
        }

        private void UpdateState()
        {
            if (Selected)
            {
                AccessibilityTraits |= UIAccessibilityTrait.Selected;
            }
            else
            {
                AccessibilityTraits &= ~UIAccessibilityTrait.Selected;
            }
        }
    }
}