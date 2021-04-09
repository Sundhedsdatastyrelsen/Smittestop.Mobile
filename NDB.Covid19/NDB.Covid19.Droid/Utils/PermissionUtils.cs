using System;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Provider;
using I18NPortable;
using NDB.Covid19.Droid.Utils.MessagingCenter;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static Android.Gms.Nearby.ExposureNotification.ExposureNotificationStatus;
using static NDB.Covid19.Droid.Utils.DroidExposureNotificationsStatusHelper;
using static NDB.Covid19.Droid.Utils.DroidRequestCodes;
using static Plugin.CurrentActivity.CrossCurrentActivity;

namespace NDB.Covid19.Droid.Utils
{
    public class PermissionUtils : IPermissionsHelper
    {
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public Task<bool> IsBluetoothEnabled()
        {
            return Task.Run(async () =>
                !await HasNearbyExposureNotificationStatus(BluetoothDisabled) &&
                !await HasNearbyExposureNotificationStatus(BluetoothSupportUnknown));
        }

        public Task<bool> IsLocationEnabled()
        {
            if ((int) Build.VERSION.SdkInt >= 30)
            {
                // Location is not required on Android 11 and above
                return Task.FromResult(true);
            }

            return Task.Run(
                async () => !await HasNearbyExposureNotificationStatus(LocationDisabled));
        }

        public async Task<bool> AreAllPermissionsGranted()
        {
            return !await IsNearbyExposureNotificationBluetoothAndLocationDisabled();
        }

        public async Task<bool> HasPermissions()
        {
            _tcs = new TaskCompletionSource<bool>();
            bool hasLocationPermissions = await HasLocationPermissionsAsync();
            bool hasBluetoothSupport = await HasBluetoothSupportAsync();
            if (hasLocationPermissions && hasBluetoothSupport)
            {
                _tcs.TrySetResult(true);
            }

            bool finalResult = await _tcs.Task;
            PermissionsMessagingCenter.PermissionsChanged = false;
            return finalResult;
        }

        public async Task<bool> CheckPermissionsIfChangedWhileIdle()
        {
            if (PermissionsMessagingCenter.PermissionsChanged)
            {
                return await HasPermissions();
            }

            return true;
        }

        public void SubscribePermissionsMessagingCenter(object subscriber, Action<object> action)
        {
            PermissionsMessagingCenter.SubscribeForPermissionsChanged(subscriber, action);
        }

        public void UnsubscribePErmissionsMessagingCenter(object subscriber)
        {
            PermissionsMessagingCenter.Unsubscribe(subscriber);
        }

        public async Task<bool> HasPermissionsWithoutDialogs()
        {
            return await IsBluetoothEnabled() && await IsLocationEnabled();
        }

        public async Task<bool> HasBluetoothSupportAsync()
        {
            if (await HasBluetoothAdapter() && BluetoothAdapter.DefaultAdapter.IsEnabled)
            {
                return true;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel
                {
                    Title = "PERMISSION_BLUETOOTH_NEEDED_TITLE".Translate(),
                    Body = "PERMISSION_ENABLE_LOCATION_AND_BLUETOOTH".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok),
                    CancelbtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Cancel)
                },
                GoToBluetoothSettings,
                CancelTask);
            return false;
        }

        public async Task<bool> HasBluetoothAdapter()
        {
            if (BluetoothAdapter.DefaultAdapter != null)
            {
                return true;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel
                {
                    Title = "NO_BLUETOOTH_TITLE".Translate(),
                    Body = "NO_BLUETOOTH_MSG".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok)
                });

            return false;
        }

        public async Task<bool> HasLocationPermissionsAsync()
        {
            if (await IsLocationEnabled())
            {
                return true;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel
                {
                    Title = "PERMISSION_LOCATION_NEEDED_TITLE".Translate(),
                    Body = "PERMISSION_ENABLE_LOCATION_AND_BLUETOOTH".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok)
                },
                GoToLocationSettings);

            return false;
        }

        private void CancelTask()
        {
            _tcs.TrySetResult(false);
        }

        private void GoToBluetoothSettings()
        {
            try
            {
                Current.Activity.StartActivityForResult(new Intent().SetAction(Settings.ActionBluetoothSettings),
                    BluetoothRequestCode);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e,
                    $"{nameof(PermissionUtils)}.{nameof(GoToBluetoothSettings)}: Failed to go to bluetooth settings");
            }
        }

        private void GoToLocationSettings()
        {
            try
            {
                Current.Activity.StartActivityForResult(new Intent().SetAction(Settings.ActionLocationSourceSettings),
                    LocationRequestCode);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e, "GoToLocationSettings");
            }
        }

        public async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == BluetoothRequestCode || requestCode == LocationRequestCode) &&
                resultCode != Result.FirstUser)
            {
                _tcs.TrySetResult(await HasPermissionsWithoutDialogs());
            }
        }
    }
}