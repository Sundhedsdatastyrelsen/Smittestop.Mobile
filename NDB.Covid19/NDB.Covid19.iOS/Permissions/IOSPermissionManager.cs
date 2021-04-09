using System;
using System.Threading.Tasks;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.iOS.Permissions
{
    public class IOSPermissionManager
    {
        public async Task<bool> PermissionUnknown()
        {
            try
            {
                return await ExposureNotification.GetStatusAsync() == Status.Unknown;
            }
            catch (Exception e)
            {
                LogUtils.LogException(
                    LogSeverity.WARNING, e, "Error during EN PermissionUnknown status check");
                return true;
            }
        }

        public async Task<bool> PoweredOff()
        {
            try
            {
                return await ExposureNotification.GetStatusAsync() == Status.BluetoothOff;
            }
            catch (Exception e)
            {
                LogUtils.LogException(
                    LogSeverity.WARNING, e, "Error during EN PoweredOff status check");
                return true;
            }
        }

        /// <summary>
        ///     Returns true if Status.Active || status == Status.Disabled || status == Status.Restricted.
        ///     Meaning that everything is ready for either starting or stopping the scanner.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> PoweredOn()
        {
            try
            {
                Status status = await ExposureNotification.GetStatusAsync();
                return status == Status.Active || status == Status.Disabled;
            }
            catch (Exception e)
            {
                LogUtils.LogException(
                    LogSeverity.WARNING, e, "Error during EN PoweredOn status check");
                return false;
            }
        }
    }
}