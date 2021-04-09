using System.Threading.Tasks;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Test.Mocks
{
    internal class PermissionsMock : IPermissionsHelper
    {
        public bool BluetoothEnabled { private get; set; }
        public bool LocationEnabled { private get; set; }
        public bool AllPermissionsGranted => BluetoothEnabled && LocationEnabled;

        public Task<bool> IsBluetoothEnabled()
        {
            return Task.FromResult(BluetoothEnabled);
        }

        public Task<bool> IsLocationEnabled()
        {
            return Task.FromResult(LocationEnabled);
        }

        public Task<bool> AreAllPermissionsGranted()
        {
            return Task.FromResult(AllPermissionsGranted);
        }
    }
}