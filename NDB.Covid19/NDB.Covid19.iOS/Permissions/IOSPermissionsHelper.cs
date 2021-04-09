using System.Threading.Tasks;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.iOS.Permissions
{
    internal class IOSPermissionsHelper : IPermissionsHelper
    {
        public Task<bool> IsBluetoothEnabled()
        {
            return Task.FromResult(!new IOSPermissionManager().PoweredOff().GetAwaiter().GetResult());
        }

        public Task<bool> IsLocationEnabled()
        {
            // not required on iOS
            return Task.FromResult(true);
        }

        public Task<bool> AreAllPermissionsGranted()
        {
            return Task.Run(
                async () =>
                    await IsLocationEnabled() && await IsBluetoothEnabled());
        }
    }
}