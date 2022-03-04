using System;
using System.Diagnostics;
using System.Linq;
using CommonServiceLocator;
using Foundation;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Managers;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.InfectionStatus;
using NDB.Covid19.iOS.Views.MessagePage;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using Security;
using UIKit;
using UserNotifications;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using SecureStorageImplementation = Plugin.SecureStorage.SecureStorageImplementation;

namespace NDB.Covid19.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate
    {
        private iOSLocalNotificationsManager _notifMgn;

        public static bool ShouldOperateIn12_5Mode => !UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        public static bool DidEnterBackgroundState { get; private set; }

        [Export("window")] public UIWindow Window { get; set; }

        [Export("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            IOSDependencyInjectionConfig.Init();
            LocalesService.Initialize();
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication
                .BackgroundFetchIntervalMinimum);

            new MigrationService().Migrate();

            LogUtils.SendAllLogs();
            AppDomain.CurrentDomain.UnhandledException += LogUtils.OnUnhandledException;

            SecureStorageImplementation.DefaultAccessible = SecAccessible.AfterFirstUnlockThisDeviceOnly;

            HandleLocalNotifications();

            BackgroundServiceHandler.PlatformScheduleFetch();

            HandleNotifications(application);

            return true;
        }

        private void HandleLocalNotifications()
        {

            if (Conf.APP_DISABLED)
            {
                return;
            }
            // Request notification permissions from the user. Dialog will only show if user has not already answered
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge,
                (approved, err) => { Console.WriteLine("Notifications approve = " + approved); });

            // Watch for notifications while the app is active
            _notifMgn =
                ServiceLocator.Current.GetInstance<ILocalNotificationsManager>() as iOSLocalNotificationsManager;
            UNUserNotificationCenter.Current.Delegate = _notifMgn;
            _notifMgn.OnNotificationTappedHandler += HandleNotificationTapped;
        }

        private void HandleNotifications(UIApplication application)
        {
            if (Conf.APP_DISABLED)
            {
                return;
            }
            UIUserNotificationSettings notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
            );
            application.RegisterUserNotificationSettings(notificationSettings);
            application.BeginBackgroundTask("showNotification", () => { });
        }

        private bool IsRegularNotification(string notificationId)
        {
            return Enum.GetValues(typeof(NotificationsEnum))
                .Cast<NotificationsEnum>()
                .Where(el => !el.Equals(NotificationsEnum.NewMessageReceived))
                .Select(e => e.ToString())
                .Contains(notificationId);
        }

        /// <summary>
        ///     We check for NotificationHasBeenTapped, and if so we segue into the messages module
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleNotificationTapped(object sender, string e)
        {
            UIViewController topController = NavigationHelper.TopController();

            if (topController is MessagePageViewController && !IsRegularNotification(e))
            {
                return;
            }

            if (topController is InfectionStatusViewController && IsRegularNotification(e))
            {
                return;
            }

            UINavigationController vc =
                e == iOSLocalNotificationsManager.NewMessageIdentifier
                    ? MessagePageViewController.GetMessagePageControllerInNavigationController()
                    : InfectionStatusViewController.GetInfectionSatusPageControllerInNavigationController();
            topController.PresentViewController(vc, true, null);
        }

        // Below are AppDelegate application lifecycle-methods that are callen in pre-iOS 13.
        // In  iOS 13 SceneDelegate is handling this.

        /// <summary>
        ///     Method that is used before iOS 13 to detect application entering background.
        ///     Corresponds to iOS 13+ SceneDelegate's DidEnterBackground(UIScene:) method.
        /// </summary>
        [Export("applicationDidEnterBackground:")]
        public void DidEnterBackground(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.DidEnterBackground called");
            DidEnterBackgroundState = true;
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_WILL_ENTER_BACKGROUND);
        }

        /// <summary>
        ///     Method that is used before iOS 13 to detect application returning to foreground from background.
        ///     Corresponds to iOS 13+ SceneDelegate's WillEnterForeground(UIScene:) method.
        /// </summary>
        [Export("applicationWillEnterForeground:")]
        public void WillEnterForeground(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.WillEnterForeground called");

            LogUtils.LogMessage(LogSeverity.INFO, "The user has opened the app", null);

            DidEnterBackgroundState = false;
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND);
        }

        /// <summary>
        ///     Method that is used before iOS 13 to detect application has become active.
        ///     Corresponds to iOS 13+ SceneDelegate's DidBecomeActive(UIScene:) method.
        /// </summary>
        [Export("applicationDidBecomeActive:")]
        public void DidBecomeActive(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.DidBecomeActive called");

            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
        }

        /// <summary>
        ///     Method that is used before iOS 13 to detect application is about to become inactive.
        ///     Corresponds to iOS 13+ SceneDelegate's WillResignActive(UIScene:) method.
        /// </summary>
        [Export("applicationWillResignActive:")]
        public void WillResignActive(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.WillResignActive called");

            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        /// <summary>
        ///     Method that is used before iOS 13 to request application open a resource specified by a URL.
        ///     Corresponds to iOS 13+ SceneDelegate's OpenUrlContexts(scene:urlContexts:) method.
        /// </summary>
        [Export("application:openURL:options:")]
        public void OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            Debug.WriteLine("AppDelegate.OpenUrl called");

            try
            {
                Uri uri_netfx = new Uri(url.AbsoluteString);
                AuthenticationState.Authenticator.OnPageLoading(uri_netfx);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e,
                    $"{nameof(AppDelegate)}.{nameof(OpenUrl)}: Failed to redirect the user to the app after NemID flow in browser");
            }
        }

        // UISceneSession Lifecycle

        [Export("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession,
            UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create("Default Configuration", connectingSceneSession.Role);
        }

        [Export("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions(UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }

        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application,
            UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        [Export("application:performFetchWithCompletionHandler:")]
        public void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        [Export("applicationWillTerminate:")]
        public void AppWillTerminate(UIApplication application)
        {
            string correlationId = GetCorrelationId();
            if (!string.IsNullOrEmpty(correlationId))
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "The user has closed the app",
                    null,
                    correlationId);
            }

            LogUtils.SendAllLogs();
        }
    }
}