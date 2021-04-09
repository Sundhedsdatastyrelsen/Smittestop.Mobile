using System.Collections.Generic;
using CommonServiceLocator;
using NDB.Covid19.Interfaces;
using Xamarin.Essentials;

namespace NDB.Covid19.Utils
{
    public class ConnectivityHelper
    {
        private static IEnumerable<ConnectionProfile> _connectionProfiles;
        private static IConnectivity _connectivity => ServiceLocator.Current.GetInstance<IConnectivity>();

        public static IEnumerable<ConnectionProfile> ConnectionProfiles =>
            _connectionProfiles ?? _connectivity.ConnectionProfiles;

        public static void MockConnectionProfiles(List<ConnectionProfile> mockedConnectionProfiles)
        {
            _connectionProfiles = mockedConnectionProfiles;
        }

        public static void ResetConnectionProfiles()
        {
            _connectionProfiles = null;
        }
    }
}