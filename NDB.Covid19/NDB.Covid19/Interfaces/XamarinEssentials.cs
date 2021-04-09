using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Essentials;

namespace NDB.Covid19.Interfaces
{
    //Generated from http://essential-interfaces.azurewebsites.net/
    public interface IEssentialsImplementation
    {
    }

    public interface IAccelerometer
    {
        bool IsMonitoring { get; }
        void Start(SensorSpeed sensorSpeed);
        void Stop();
        event EventHandler<AccelerometerChangedEventArgs> ReadingChanged;
        event EventHandler ShakeDetected;
    }

    public interface IAppInfo
    {
        string PackageName { get; }
        string Name { get; }
        string VersionString { get; }
        Version Version { get; }
        string BuildString { get; }
        AppTheme RequestedTheme { get; }
        void ShowSettingsUI();
    }

    public interface IBarometer
    {
        bool IsMonitoring { get; }
        void Start(SensorSpeed sensorSpeed);
        void Stop();
        event EventHandler<BarometerChangedEventArgs> ReadingChanged;
    }

    public interface IBattery
    {
        double ChargeLevel { get; }
        BatteryState State { get; }
        BatteryPowerSource PowerSource { get; }
        EnergySaverStatus EnergySaverStatus { get; }
        event EventHandler<BatteryInfoChangedEventArgs> BatteryInfoChanged;
        event EventHandler<EnergySaverStatusChangedEventArgs> EnergySaverStatusChanged;
    }

    public interface IBrowser
    {
        Task OpenAsync(string uri);
        Task OpenAsync(string uri, BrowserLaunchMode launchMode);
        Task OpenAsync(string uri, BrowserLaunchOptions options);
        Task OpenAsync(Uri uri);
        Task OpenAsync(Uri uri, BrowserLaunchMode launchMode);
        Task<bool> OpenAsync(Uri uri, BrowserLaunchOptions options);
    }

    public interface IClipboard
    {
        bool HasText { get; }
        Task SetTextAsync(string text);
        Task<string> GetTextAsync();
        event EventHandler<EventArgs> ClipboardContentChanged;
    }

    public interface ICompass
    {
        bool IsMonitoring { get; }
        void Start(SensorSpeed sensorSpeed);
        void Start(SensorSpeed sensorSpeed, bool applyLowPassFilter);
        void Stop();
        event EventHandler<CompassChangedEventArgs> ReadingChanged;
    }

    public interface IConnectivity
    {
        NetworkAccess NetworkAccess { get; }
        IEnumerable<ConnectionProfile> ConnectionProfiles { get; }
        event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }

    public interface IDeviceDisplay
    {
        bool KeepScreenOn { get; set; }
        DisplayInfo MainDisplayInfo { get; }
        event EventHandler<DisplayInfoChangedEventArgs> MainDisplayInfoChanged;
    }

    public interface IDeviceInfo
    {
        string Model { get; }
        string Manufacturer { get; }
        string Name { get; }
        string VersionString { get; }
        Version Version { get; }
        DevicePlatform Platform { get; }
        DeviceIdiom Idiom { get; }
        DeviceType DeviceType { get; }
    }

    public interface IEmail
    {
        Task ComposeAsync();
        Task ComposeAsync(string subject, string body, params string[] to);
        Task ComposeAsync(EmailMessage message);
    }

    public interface IFileSystem
    {
        string CacheDirectory { get; }
        string AppDataDirectory { get; }
        Task<Stream> OpenAppPackageFileAsync(string filename);
    }

    public interface IFlashlight
    {
        Task TurnOnAsync();
        Task TurnOffAsync();
    }

    public interface IGeocoding
    {
        Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location);
        Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude);
        Task<IEnumerable<Location>> GetLocationsAsync(string address);
    }

    public interface IGeolocation
    {
        Task<Location> GetLastKnownLocationAsync();
        Task<Location> GetLocationAsync();
        Task<Location> GetLocationAsync(GeolocationRequest request);
        Task<Location> GetLocationAsync(GeolocationRequest request, CancellationToken cancelToken);
    }

    public interface IGyroscope
    {
        bool IsMonitoring { get; }
        void Start(SensorSpeed sensorSpeed);
        void Stop();
        event EventHandler<GyroscopeChangedEventArgs> ReadingChanged;
    }

    public interface ILauncher
    {
        Task<bool> CanOpenAsync(string uri);
        Task<bool> CanOpenAsync(Uri uri);
        Task OpenAsync(string uri);
        Task OpenAsync(Uri uri);
        Task OpenAsync(OpenFileRequest request);
        Task<bool> TryOpenAsync(string uri);
        Task<bool> TryOpenAsync(Uri uri);
    }

    public interface IMagnetometer
    {
        bool IsMonitoring { get; }
        void Start(SensorSpeed sensorSpeed);
        void Stop();
        event EventHandler<MagnetometerChangedEventArgs> ReadingChanged;
    }

    public interface IMainThread
    {
        bool IsMainThread { get; }
        void BeginInvokeOnMainThread(Action action);
        Task InvokeOnMainThreadAsync(Action action);
        Task<T> InvokeOnMainThreadAsync<T>(Func<T> func);
        Task InvokeOnMainThreadAsync(Func<Task> funcTask);
        Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask);
        Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync();
    }

    public interface IMap
    {
        Task OpenAsync(Location location);
        Task OpenAsync(Location location, MapLaunchOptions options);
        Task OpenAsync(double latitude, double longitude);
        Task OpenAsync(double latitude, double longitude, MapLaunchOptions options);
        Task OpenAsync(Placemark placemark);
        Task OpenAsync(Placemark placemark, MapLaunchOptions options);
    }

    public interface IOrientationSensor
    {
        bool IsMonitoring { get; }
        void Start(SensorSpeed sensorSpeed);
        void Stop();
        event EventHandler<OrientationSensorChangedEventArgs> ReadingChanged;
    }

    public interface IPermissions
    {
        Task<PermissionStatus> CheckStatusAsync<TPermission>() where TPermission : Permissions.BasePermission, new();
        Task<PermissionStatus> RequestAsync<TPermission>() where TPermission : Permissions.BasePermission, new();
    }

    public interface IPhoneDialer
    {
        void Open(string number);
    }

    public interface IPreferences
    {
        bool ContainsKey(string key);
        void Remove(string key);
        void Clear();
        string Get(string key, string defaultValue);
        bool Get(string key, bool defaultValue);
        int Get(string key, int defaultValue);
        double Get(string key, double defaultValue);
        float Get(string key, float defaultValue);
        long Get(string key, long defaultValue);
        void Set(string key, string value);
        void Set(string key, bool value);
        void Set(string key, int value);
        void Set(string key, double value);
        void Set(string key, float value);
        void Set(string key, long value);
        bool ContainsKey(string key, string sharedName);
        void Remove(string key, string sharedName);
        void Clear(string sharedName);
        string Get(string key, string defaultValue, string sharedName);
        bool Get(string key, bool defaultValue, string sharedName);
        int Get(string key, int defaultValue, string sharedName);
        double Get(string key, double defaultValue, string sharedName);
        float Get(string key, float defaultValue, string sharedName);
        long Get(string key, long defaultValue, string sharedName);
        void Set(string key, string value, string sharedName);
        void Set(string key, bool value, string sharedName);
        void Set(string key, int value, string sharedName);
        void Set(string key, double value, string sharedName);
        void Set(string key, float value, string sharedName);
        void Set(string key, long value, string sharedName);
        DateTime Get(string key, DateTime defaultValue);
        void Set(string key, DateTime value);
        DateTime Get(string key, DateTime defaultValue, string sharedName);
        void Set(string key, DateTime value, string sharedName);
    }

    public interface ISecureStorage
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
        bool Remove(string key);
        void RemoveAll();
    }

    public interface IShare
    {
        Task RequestAsync(string text);
        Task RequestAsync(string text, string title);
        Task RequestAsync(ShareTextRequest request);
        Task RequestAsync(ShareFileRequest request);
    }

    public interface ISms
    {
        Task ComposeAsync();
        Task ComposeAsync(SmsMessage message);
    }

    public interface ITextToSpeech
    {
        Task<IEnumerable<Locale>> GetLocalesAsync();
        Task SpeakAsync(string text, CancellationToken cancelToken = default);
        Task SpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken = default);
    }

    public interface IVersionTracking
    {
        bool IsFirstLaunchEver { get; }
        bool IsFirstLaunchForCurrentVersion { get; }
        bool IsFirstLaunchForCurrentBuild { get; }
        string CurrentVersion { get; }
        string CurrentBuild { get; }
        string PreviousVersion { get; }
        string PreviousBuild { get; }
        string FirstInstalledVersion { get; }
        string FirstInstalledBuild { get; }
        IEnumerable<string> VersionHistory { get; }
        IEnumerable<string> BuildHistory { get; }
        void Track();
        bool IsFirstLaunchForVersion(string version);
        bool IsFirstLaunchForBuild(string build);
    }

    public interface IVibration
    {
        void Vibrate();
        void Vibrate(double duration);
        void Vibrate(TimeSpan duration);
        void Cancel();
    }

    public interface IWebAuthenticator
    {
        Task<WebAuthenticatorResult> AuthenticateAsync(Uri url, Uri callbackUrl);
    }

//Generated from http://essential-interfaces.azurewebsites.net/
    public class AccelerometerImplementation : IEssentialsImplementation, IAccelerometer
    {
        [Preserve(Conditional = true)]
        public AccelerometerImplementation()
        {
        }

        void IAccelerometer.Start(SensorSpeed sensorSpeed)
        {
            Accelerometer.Start(sensorSpeed);
        }

        void IAccelerometer.Stop()
        {
            Accelerometer.Stop();
        }

        bool IAccelerometer.IsMonitoring
            => Accelerometer.IsMonitoring;

        event EventHandler<AccelerometerChangedEventArgs> IAccelerometer.ReadingChanged
        {
            add => Accelerometer.ReadingChanged += value;
            remove => Accelerometer.ReadingChanged -= value;
        }

        event EventHandler IAccelerometer.ShakeDetected
        {
            add => Accelerometer.ShakeDetected += value;
            remove => Accelerometer.ShakeDetected -= value;
        }
    }

    public class AppInfoImplementation : IEssentialsImplementation, IAppInfo
    {
        [Preserve(Conditional = true)]
        public AppInfoImplementation()
        {
        }

        void IAppInfo.ShowSettingsUI()
        {
            AppInfo.ShowSettingsUI();
        }

        string IAppInfo.PackageName
            => AppInfo.PackageName;

        string IAppInfo.Name
            => AppInfo.Name;

        string IAppInfo.VersionString
            => AppInfo.VersionString;

        Version IAppInfo.Version
            => AppInfo.Version;

        string IAppInfo.BuildString
            => AppInfo.BuildString;

        AppTheme IAppInfo.RequestedTheme
            => AppInfo.RequestedTheme;
    }

    public class BarometerImplementation : IEssentialsImplementation, IBarometer
    {
        [Preserve(Conditional = true)]
        public BarometerImplementation()
        {
        }

        void IBarometer.Start(SensorSpeed sensorSpeed)
        {
            Barometer.Start(sensorSpeed);
        }

        void IBarometer.Stop()
        {
            Barometer.Stop();
        }

        bool IBarometer.IsMonitoring
            => Barometer.IsMonitoring;

        event EventHandler<BarometerChangedEventArgs> IBarometer.ReadingChanged
        {
            add => Barometer.ReadingChanged += value;
            remove => Barometer.ReadingChanged -= value;
        }
    }

    public class BatteryImplementation : IEssentialsImplementation, IBattery
    {
        [Preserve(Conditional = true)]
        public BatteryImplementation()
        {
        }

        double IBattery.ChargeLevel
            => Battery.ChargeLevel;

        BatteryState IBattery.State
            => Battery.State;

        BatteryPowerSource IBattery.PowerSource
            => Battery.PowerSource;

        EnergySaverStatus IBattery.EnergySaverStatus
            => Battery.EnergySaverStatus;

        event EventHandler<BatteryInfoChangedEventArgs> IBattery.BatteryInfoChanged
        {
            add => Battery.BatteryInfoChanged += value;
            remove => Battery.BatteryInfoChanged -= value;
        }

        event EventHandler<EnergySaverStatusChangedEventArgs> IBattery.EnergySaverStatusChanged
        {
            add => Battery.EnergySaverStatusChanged += value;
            remove => Battery.EnergySaverStatusChanged -= value;
        }
    }

    public class BrowserImplementation : IEssentialsImplementation, IBrowser
    {
        [Preserve(Conditional = true)]
        public BrowserImplementation()
        {
        }

        Task IBrowser.OpenAsync(string uri)
        {
            return Browser.OpenAsync(uri);
        }

        Task IBrowser.OpenAsync(string uri, BrowserLaunchMode launchMode)
        {
            return Browser.OpenAsync(uri, launchMode);
        }

        Task IBrowser.OpenAsync(string uri, BrowserLaunchOptions options)
        {
            return Browser.OpenAsync(uri, options);
        }

        Task IBrowser.OpenAsync(Uri uri)
        {
            return Browser.OpenAsync(uri);
        }

        Task IBrowser.OpenAsync(Uri uri, BrowserLaunchMode launchMode)
        {
            return Browser.OpenAsync(uri, launchMode);
        }

        Task<bool> IBrowser.OpenAsync(Uri uri, BrowserLaunchOptions options)
        {
            return Browser.OpenAsync(uri, options);
        }
    }

    public class ClipboardImplementation : IEssentialsImplementation, IClipboard
    {
        [Preserve(Conditional = true)]
        public ClipboardImplementation()
        {
        }

        Task IClipboard.SetTextAsync(string text)
        {
            return Clipboard.SetTextAsync(text);
        }

        Task<string> IClipboard.GetTextAsync()
        {
            return Clipboard.GetTextAsync();
        }

        bool IClipboard.HasText
            => Clipboard.HasText;

        event EventHandler<EventArgs> IClipboard.ClipboardContentChanged
        {
            add => Clipboard.ClipboardContentChanged += value;
            remove => Clipboard.ClipboardContentChanged -= value;
        }
    }

    public class CompassImplementation : IEssentialsImplementation, ICompass
    {
        [Preserve(Conditional = true)]
        public CompassImplementation()
        {
        }

        void ICompass.Start(SensorSpeed sensorSpeed)
        {
            Compass.Start(sensorSpeed);
        }

        void ICompass.Start(SensorSpeed sensorSpeed, bool applyLowPassFilter)
        {
            Compass.Start(sensorSpeed, applyLowPassFilter);
        }

        void ICompass.Stop()
        {
            Compass.Stop();
        }

        bool ICompass.IsMonitoring
            => Compass.IsMonitoring;

        event EventHandler<CompassChangedEventArgs> ICompass.ReadingChanged
        {
            add => Compass.ReadingChanged += value;
            remove => Compass.ReadingChanged -= value;
        }
    }

    public class ConnectivityImplementation : IEssentialsImplementation, IConnectivity
    {
        [Preserve(Conditional = true)]
        public ConnectivityImplementation()
        {
        }

        NetworkAccess IConnectivity.NetworkAccess
            => Connectivity.NetworkAccess;

        IEnumerable<ConnectionProfile> IConnectivity.ConnectionProfiles
            => Connectivity.ConnectionProfiles;

        event EventHandler<ConnectivityChangedEventArgs> IConnectivity.ConnectivityChanged
        {
            add => Connectivity.ConnectivityChanged += value;
            remove => Connectivity.ConnectivityChanged -= value;
        }
    }

    public class DeviceDisplayImplementation : IEssentialsImplementation, IDeviceDisplay
    {
        [Preserve(Conditional = true)]
        public DeviceDisplayImplementation()
        {
        }

        bool IDeviceDisplay.KeepScreenOn
        {
            get => DeviceDisplay.KeepScreenOn;
            set => DeviceDisplay.KeepScreenOn = value;
        }

        DisplayInfo IDeviceDisplay.MainDisplayInfo
            => DeviceDisplay.MainDisplayInfo;

        event EventHandler<DisplayInfoChangedEventArgs> IDeviceDisplay.MainDisplayInfoChanged
        {
            add => DeviceDisplay.MainDisplayInfoChanged += value;
            remove => DeviceDisplay.MainDisplayInfoChanged -= value;
        }
    }

    public class DeviceInfoImplementation : IEssentialsImplementation, IDeviceInfo
    {
        [Preserve(Conditional = true)]
        public DeviceInfoImplementation()
        {
        }

        string IDeviceInfo.Model
            => DeviceInfo.Model;

        string IDeviceInfo.Manufacturer
            => DeviceInfo.Manufacturer;

        string IDeviceInfo.Name
            => DeviceInfo.Name;

        string IDeviceInfo.VersionString
            => DeviceInfo.VersionString;

        Version IDeviceInfo.Version
            => DeviceInfo.Version;

        DevicePlatform IDeviceInfo.Platform
            => DeviceInfo.Platform;

        DeviceIdiom IDeviceInfo.Idiom
            => DeviceInfo.Idiom;

        DeviceType IDeviceInfo.DeviceType
            => DeviceInfo.DeviceType;
    }

    public class EmailImplementation : IEssentialsImplementation, IEmail
    {
        [Preserve(Conditional = true)]
        public EmailImplementation()
        {
        }

        Task IEmail.ComposeAsync()
        {
            return Email.ComposeAsync();
        }

        Task IEmail.ComposeAsync(string subject, string body, params string[] to)
        {
            return Email.ComposeAsync(subject, body, to);
        }

        Task IEmail.ComposeAsync(EmailMessage message)
        {
            return Email.ComposeAsync(message);
        }
    }

    public class FileSystemImplementation : IEssentialsImplementation, IFileSystem
    {
        [Preserve(Conditional = true)]
        public FileSystemImplementation()
        {
        }

        Task<Stream> IFileSystem.OpenAppPackageFileAsync(string filename)
        {
            return FileSystem.OpenAppPackageFileAsync(filename);
        }

        string IFileSystem.CacheDirectory
            => FileSystem.CacheDirectory;

        string IFileSystem.AppDataDirectory
            => FileSystem.AppDataDirectory;
    }

    public class FlashlightImplementation : IEssentialsImplementation, IFlashlight
    {
        [Preserve(Conditional = true)]
        public FlashlightImplementation()
        {
        }

        Task IFlashlight.TurnOnAsync()
        {
            return Flashlight.TurnOnAsync();
        }

        Task IFlashlight.TurnOffAsync()
        {
            return Flashlight.TurnOffAsync();
        }
    }

    public class GeocodingImplementation : IEssentialsImplementation, IGeocoding
    {
        [Preserve(Conditional = true)]
        public GeocodingImplementation()
        {
        }

        Task<IEnumerable<Placemark>> IGeocoding.GetPlacemarksAsync(Location location)
        {
            return Geocoding.GetPlacemarksAsync(location);
        }

        Task<IEnumerable<Placemark>> IGeocoding.GetPlacemarksAsync(double latitude, double longitude)
        {
            return Geocoding.GetPlacemarksAsync(latitude, longitude);
        }

        Task<IEnumerable<Location>> IGeocoding.GetLocationsAsync(string address)
        {
            return Geocoding.GetLocationsAsync(address);
        }
    }

    public class GeolocationImplementation : IEssentialsImplementation, IGeolocation
    {
        [Preserve(Conditional = true)]
        public GeolocationImplementation()
        {
        }

        Task<Location> IGeolocation.GetLastKnownLocationAsync()
        {
            return Geolocation.GetLastKnownLocationAsync();
        }

        Task<Location> IGeolocation.GetLocationAsync()
        {
            return Geolocation.GetLocationAsync();
        }

        Task<Location> IGeolocation.GetLocationAsync(GeolocationRequest request)
        {
            return Geolocation.GetLocationAsync(request);
        }

        Task<Location> IGeolocation.GetLocationAsync(GeolocationRequest request, CancellationToken cancelToken)
        {
            return Geolocation.GetLocationAsync(request, cancelToken);
        }
    }

    public class GyroscopeImplementation : IEssentialsImplementation, IGyroscope
    {
        [Preserve(Conditional = true)]
        public GyroscopeImplementation()
        {
        }

        void IGyroscope.Start(SensorSpeed sensorSpeed)
        {
            Gyroscope.Start(sensorSpeed);
        }

        void IGyroscope.Stop()
        {
            Gyroscope.Stop();
        }

        bool IGyroscope.IsMonitoring
            => Gyroscope.IsMonitoring;

        event EventHandler<GyroscopeChangedEventArgs> IGyroscope.ReadingChanged
        {
            add => Gyroscope.ReadingChanged += value;
            remove => Gyroscope.ReadingChanged -= value;
        }
    }

    public class LauncherImplementation : IEssentialsImplementation, ILauncher
    {
        [Preserve(Conditional = true)]
        public LauncherImplementation()
        {
        }

        Task<bool> ILauncher.CanOpenAsync(string uri)
        {
            return Launcher.CanOpenAsync(uri);
        }

        Task<bool> ILauncher.CanOpenAsync(Uri uri)
        {
            return Launcher.CanOpenAsync(uri);
        }

        Task ILauncher.OpenAsync(string uri)
        {
            return Launcher.OpenAsync(uri);
        }

        Task ILauncher.OpenAsync(Uri uri)
        {
            return Launcher.OpenAsync(uri);
        }

        Task ILauncher.OpenAsync(OpenFileRequest request)
        {
            return Launcher.OpenAsync(request);
        }

        Task<bool> ILauncher.TryOpenAsync(string uri)
        {
            return Launcher.TryOpenAsync(uri);
        }

        Task<bool> ILauncher.TryOpenAsync(Uri uri)
        {
            return Launcher.TryOpenAsync(uri);
        }
    }

    public class MagnetometerImplementation : IEssentialsImplementation, IMagnetometer
    {
        [Preserve(Conditional = true)]
        public MagnetometerImplementation()
        {
        }

        void IMagnetometer.Start(SensorSpeed sensorSpeed)
        {
            Magnetometer.Start(sensorSpeed);
        }

        void IMagnetometer.Stop()
        {
            Magnetometer.Stop();
        }

        bool IMagnetometer.IsMonitoring
            => Magnetometer.IsMonitoring;

        event EventHandler<MagnetometerChangedEventArgs> IMagnetometer.ReadingChanged
        {
            add => Magnetometer.ReadingChanged += value;
            remove => Magnetometer.ReadingChanged -= value;
        }
    }

    public class MainThreadImplementation : IEssentialsImplementation, IMainThread
    {
        [Preserve(Conditional = true)]
        public MainThreadImplementation()
        {
        }

        void IMainThread.BeginInvokeOnMainThread(Action action)
        {
            MainThread.BeginInvokeOnMainThread(action);
        }

        Task IMainThread.InvokeOnMainThreadAsync(Action action)
        {
            return MainThread.InvokeOnMainThreadAsync(action);
        }

        Task<T> IMainThread.InvokeOnMainThreadAsync<T>(Func<T> func)
        {
            return MainThread.InvokeOnMainThreadAsync(func);
        }

        Task IMainThread.InvokeOnMainThreadAsync(Func<Task> funcTask)
        {
            return MainThread.InvokeOnMainThreadAsync(funcTask);
        }

        Task<T> IMainThread.InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask)
        {
            return MainThread.InvokeOnMainThreadAsync(funcTask);
        }

        Task<SynchronizationContext> IMainThread.GetMainThreadSynchronizationContextAsync()
        {
            return MainThread.GetMainThreadSynchronizationContextAsync();
        }

        bool IMainThread.IsMainThread
            => MainThread.IsMainThread;
    }

    public class MapImplementation : IEssentialsImplementation, IMap
    {
        [Preserve(Conditional = true)]
        public MapImplementation()
        {
        }

        Task IMap.OpenAsync(Location location)
        {
            return Map.OpenAsync(location);
        }

        Task IMap.OpenAsync(Location location, MapLaunchOptions options)
        {
            return Map.OpenAsync(location, options);
        }

        Task IMap.OpenAsync(double latitude, double longitude)
        {
            return Map.OpenAsync(latitude, longitude);
        }

        Task IMap.OpenAsync(double latitude, double longitude, MapLaunchOptions options)
        {
            return Map.OpenAsync(latitude, longitude, options);
        }

        Task IMap.OpenAsync(Placemark placemark)
        {
            return Map.OpenAsync(placemark);
        }

        Task IMap.OpenAsync(Placemark placemark, MapLaunchOptions options)
        {
            return Map.OpenAsync(placemark, options);
        }
    }

    public class OrientationSensorImplementation : IEssentialsImplementation, IOrientationSensor
    {
        [Preserve(Conditional = true)]
        public OrientationSensorImplementation()
        {
        }

        void IOrientationSensor.Start(SensorSpeed sensorSpeed)
        {
            OrientationSensor.Start(sensorSpeed);
        }

        void IOrientationSensor.Stop()
        {
            OrientationSensor.Stop();
        }

        bool IOrientationSensor.IsMonitoring
            => OrientationSensor.IsMonitoring;

        event EventHandler<OrientationSensorChangedEventArgs> IOrientationSensor.ReadingChanged
        {
            add => OrientationSensor.ReadingChanged += value;
            remove => OrientationSensor.ReadingChanged -= value;
        }
    }

    public class PermissionsImplementation : IEssentialsImplementation, IPermissions
    {
        [Preserve(Conditional = true)]
        public PermissionsImplementation()
        {
        }

        Task<PermissionStatus> IPermissions.CheckStatusAsync<TPermission>()
        {
            return Permissions.CheckStatusAsync<TPermission>();
        }

        Task<PermissionStatus> IPermissions.RequestAsync<TPermission>()
        {
            return Permissions.RequestAsync<TPermission>();
        }
    }

    public class PhoneDialerImplementation : IEssentialsImplementation, IPhoneDialer
    {
        [Preserve(Conditional = true)]
        public PhoneDialerImplementation()
        {
        }

        void IPhoneDialer.Open(string number)
        {
            PhoneDialer.Open(number);
        }
    }

    public class PreferencesImplementation : IEssentialsImplementation, IPreferences
    {
        [Preserve(Conditional = true)]
        public PreferencesImplementation()
        {
        }

        bool IPreferences.ContainsKey(string key)
        {
            return Preferences.ContainsKey(key);
        }

        void IPreferences.Remove(string key)
        {
            Preferences.Remove(key);
        }

        void IPreferences.Clear()
        {
            Preferences.Clear();
        }

        string IPreferences.Get(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        bool IPreferences.Get(string key, bool defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        int IPreferences.Get(string key, int defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        double IPreferences.Get(string key, double defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        float IPreferences.Get(string key, float defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        long IPreferences.Get(string key, long defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        void IPreferences.Set(string key, string value)
        {
            Preferences.Set(key, value);
        }

        void IPreferences.Set(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        void IPreferences.Set(string key, int value)
        {
            Preferences.Set(key, value);
        }

        void IPreferences.Set(string key, double value)
        {
            Preferences.Set(key, value);
        }

        void IPreferences.Set(string key, float value)
        {
            Preferences.Set(key, value);
        }

        void IPreferences.Set(string key, long value)
        {
            Preferences.Set(key, value);
        }

        bool IPreferences.ContainsKey(string key, string sharedName)
        {
            return Preferences.ContainsKey(key, sharedName);
        }

        void IPreferences.Remove(string key, string sharedName)
        {
            Preferences.Remove(key, sharedName);
        }

        void IPreferences.Clear(string sharedName)
        {
            Preferences.Clear(sharedName);
        }

        string IPreferences.Get(string key, string defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        bool IPreferences.Get(string key, bool defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        int IPreferences.Get(string key, int defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        double IPreferences.Get(string key, double defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        float IPreferences.Get(string key, float defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        long IPreferences.Get(string key, long defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        void IPreferences.Set(string key, string value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }

        void IPreferences.Set(string key, bool value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }

        void IPreferences.Set(string key, int value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }

        void IPreferences.Set(string key, double value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }

        void IPreferences.Set(string key, float value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }

        void IPreferences.Set(string key, long value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }

        DateTime IPreferences.Get(string key, DateTime defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        void IPreferences.Set(string key, DateTime value)
        {
            Preferences.Set(key, value);
        }

        DateTime IPreferences.Get(string key, DateTime defaultValue, string sharedName)
        {
            return Preferences.Get(key, defaultValue, sharedName);
        }

        void IPreferences.Set(string key, DateTime value, string sharedName)
        {
            Preferences.Set(key, value, sharedName);
        }
    }

    public class SecureStorageImplementation : IEssentialsImplementation, ISecureStorage
    {
        [Preserve(Conditional = true)]
        public SecureStorageImplementation()
        {
        }

        Task<string> ISecureStorage.GetAsync(string key)
        {
            return SecureStorage.GetAsync(key);
        }

        Task ISecureStorage.SetAsync(string key, string value)
        {
            return SecureStorage.SetAsync(key, value);
        }

        bool ISecureStorage.Remove(string key)
        {
            return SecureStorage.Remove(key);
        }

        void ISecureStorage.RemoveAll()
        {
            SecureStorage.RemoveAll();
        }
    }

    public class ShareImplementation : IEssentialsImplementation, IShare
    {
        [Preserve(Conditional = true)]
        public ShareImplementation()
        {
        }

        Task IShare.RequestAsync(string text)
        {
            return Share.RequestAsync(text);
        }

        Task IShare.RequestAsync(string text, string title)
        {
            return Share.RequestAsync(text, title);
        }

        Task IShare.RequestAsync(ShareTextRequest request)
        {
            return Share.RequestAsync(request);
        }

        Task IShare.RequestAsync(ShareFileRequest request)
        {
            return Share.RequestAsync(request);
        }
    }

    public class SmsImplementation : IEssentialsImplementation, ISms
    {
        [Preserve(Conditional = true)]
        public SmsImplementation()
        {
        }

        Task ISms.ComposeAsync()
        {
            return Sms.ComposeAsync();
        }

        Task ISms.ComposeAsync(SmsMessage message)
        {
            return Sms.ComposeAsync(message);
        }
    }

    public class TextToSpeechImplementation : IEssentialsImplementation, ITextToSpeech
    {
        [Preserve(Conditional = true)]
        public TextToSpeechImplementation()
        {
        }

        Task<IEnumerable<Locale>> ITextToSpeech.GetLocalesAsync()
        {
            return TextToSpeech.GetLocalesAsync();
        }

        Task ITextToSpeech.SpeakAsync(string text, CancellationToken cancelToken = default)
        {
            return TextToSpeech.SpeakAsync(text, cancelToken);
        }

        Task ITextToSpeech.SpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken = default)
        {
            return TextToSpeech.SpeakAsync(text, options, cancelToken);
        }
    }

    public class VersionTrackingImplementation : IEssentialsImplementation, IVersionTracking
    {
        [Preserve(Conditional = true)]
        public VersionTrackingImplementation()
        {
        }

        void IVersionTracking.Track()
        {
            VersionTracking.Track();
        }

        bool IVersionTracking.IsFirstLaunchForVersion(string version)
        {
            return VersionTracking.IsFirstLaunchForVersion(version);
        }

        bool IVersionTracking.IsFirstLaunchForBuild(string build)
        {
            return VersionTracking.IsFirstLaunchForBuild(build);
        }

        bool IVersionTracking.IsFirstLaunchEver
            => VersionTracking.IsFirstLaunchEver;

        bool IVersionTracking.IsFirstLaunchForCurrentVersion
            => VersionTracking.IsFirstLaunchForCurrentVersion;

        bool IVersionTracking.IsFirstLaunchForCurrentBuild
            => VersionTracking.IsFirstLaunchForCurrentBuild;

        string IVersionTracking.CurrentVersion
            => VersionTracking.CurrentVersion;

        string IVersionTracking.CurrentBuild
            => VersionTracking.CurrentBuild;

        string IVersionTracking.PreviousVersion
            => VersionTracking.PreviousVersion;

        string IVersionTracking.PreviousBuild
            => VersionTracking.PreviousBuild;

        string IVersionTracking.FirstInstalledVersion
            => VersionTracking.FirstInstalledVersion;

        string IVersionTracking.FirstInstalledBuild
            => VersionTracking.FirstInstalledBuild;

        IEnumerable<string> IVersionTracking.VersionHistory
            => VersionTracking.VersionHistory;

        IEnumerable<string> IVersionTracking.BuildHistory
            => VersionTracking.BuildHistory;
    }

    public class VibrationImplementation : IEssentialsImplementation, IVibration
    {
        [Preserve(Conditional = true)]
        public VibrationImplementation()
        {
        }

        void IVibration.Vibrate()
        {
            Vibration.Vibrate();
        }

        void IVibration.Vibrate(double duration)
        {
            Vibration.Vibrate(duration);
        }

        void IVibration.Vibrate(TimeSpan duration)
        {
            Vibration.Vibrate(duration);
        }

        void IVibration.Cancel()
        {
            Vibration.Cancel();
        }
    }

    public class WebAuthenticatorImplementation : IEssentialsImplementation, IWebAuthenticator
    {
        [Preserve(Conditional = true)]
        public WebAuthenticatorImplementation()
        {
        }

        Task<WebAuthenticatorResult> IWebAuthenticator.AuthenticateAsync(Uri url, Uri callbackUrl)
        {
            return WebAuthenticator.AuthenticateAsync(url, callbackUrl);
        }
    }
}