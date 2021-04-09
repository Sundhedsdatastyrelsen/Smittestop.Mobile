﻿using System;
using I18NPortable;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public static class DialogHelper
    {
        [Obsolete(
            "Use ShowDialog() that takes a DialogViewModel as parameter. That method can also handle if a Cancel-button is needed.")]
        public static void ShowDialog(UIViewController parent, string title, string message, string okBtnText,
            Action<UIAlertAction> onOKClicked)
        {
            parent.InvokeOnMainThread(() =>
            {
                UIAlertController controller = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
                controller.AddAction(UIAlertAction.Create(okBtnText, UIAlertActionStyle.Default, onOKClicked));
                parent.PresentViewController(controller, true, null);
            });
        }

        public static void ShowDialog(UIViewController parent, DialogViewModel viewModel,
            Action<UIAlertAction> onOKTapped, UIAlertActionStyle OkBtnStyle = UIAlertActionStyle.Default,
            Action<UIAlertAction> OnCancelTapped = null)
        {
            parent.InvokeOnMainThread(() =>
            {
                UIAlertController controller =
                    UIAlertController.Create(viewModel.Title, viewModel.Body, UIAlertControllerStyle.Alert);

                if (!string.IsNullOrWhiteSpace(viewModel.CancelbtnTxt))
                {
                    controller.AddAction(UIAlertAction.Create(viewModel.CancelbtnTxt, UIAlertActionStyle.Cancel,
                        OnCancelTapped));
                }

                if (!string.IsNullOrWhiteSpace(viewModel.OkBtnTxt))
                {
                    controller.AddAction(UIAlertAction.Create(viewModel.OkBtnTxt, OkBtnStyle, onOKTapped));
                }

                parent.PresentViewController(controller, true, null);
            });
        }

        public static void ShowMissingPermissionToAccessBluetoothDialog(UIViewController parent)
        {
            DialogViewModel vm = new DialogViewModel
            {
                Title = "NO_BLUETOOTH_TITLE".Translate(),
                Body = "PERMISSION_ENABLE_BLUETOOTH".Translate(),
                OkBtnTxt = "PERMISSIONS_NEEDED_BUTTON".Translate()
            };

            ShowDialog(
                parent,
                vm,
                action => { NavigationHelper.GoToAppSettings(); });
        }

        public static void ShowBluetoothTurnedOffDialog(UIViewController parent)
        {
            DialogViewModel vm = new DialogViewModel
            {
                Title = "NO_BLUETOOTH_TITLE".Translate(),
                Body = "PERMISSION_ENABLE_BT".Translate(),
                OkBtnTxt = "PERMISSIONS_NEEDED_BUTTON".Translate(),
                CancelbtnTxt = "PERMISSION_BLUETOOTH_BUTTON_CANCEL".Translate()
            };
            ShowDialog(
                parent,
                vm,
                action => { NavigationHelper.GoToSettings(); });
        }
    }
}