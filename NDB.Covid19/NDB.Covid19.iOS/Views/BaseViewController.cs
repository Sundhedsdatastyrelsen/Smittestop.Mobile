﻿using System;
using System.Linq;
using CoreAnimation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views
{
    public class BaseViewController : UIViewController
    {
        protected internal BaseViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetGradientBackground();
        }

        private void SetGradientBackground()
        {
            CAGradientLayer gradientLayer = new CAGradientLayer();
            gradientLayer.Frame = View.Bounds;
            gradientLayer.Colors = new[] {"#245C89".ToUIColor().CGColor, "#002034".ToUIColor().CGColor};

            View.Layer.InsertSublayer(gradientLayer, 0);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE, ShowForceUpdatePage);
        }

        public override void ViewWillDisappear(bool animated)
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
            base.ViewWillDisappear(animated);
        }

        private void ShowForceUpdatePage(object _)
        {
            InvokeOnMainThread(() =>
            {
                UIViewController forceUpdateVC = NavigationHelper.ViewControllerByStoryboardName("ForceUpdate");
                forceUpdateVC.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                PresentViewController(forceUpdateVC, true, null);
            });
        }

        /// <summary>
        ///     If the ViewController is embedded in a NavigationController it will be popped. Otherwise it will be dismissed.
        /// </summary>
        /// <param name="animation">If set to <c>true</c> animation.</param>
        /// <param name="completionHandler">Completion handler.</param>
        public virtual void LeaveController(bool animation = true, Action completionHandler = null)
        {
            if (NavigationController != null)
            {
                if (NavigationController.ViewControllers.Count() > 1)
                {
                    NavigationController.PopViewController(animation);
                    completionHandler?.Invoke();
                }
                else
                {
                    NavigationController.DismissViewController(animation, completionHandler);
                }
            }
            else
            {
                DismissViewController(animation, completionHandler);
            }
        }
    }
}