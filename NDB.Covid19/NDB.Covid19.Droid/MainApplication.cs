using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.LocalBroadcastManager.Content;
using NDB.Covid19.Droid.Services;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views;
using NDB.Covid19.Droid.Views.AuthenticationFlow;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using Xamarin.Essentials;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Droid
{
    [Application]
    internal class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private int _activityReferences;
        private BackgroundNotificationBroadcastReceiver _backgroundNotificationBroadcastReceiver;
        private IntentFilter _filter;
        private FlightModeHandlerBroadcastReceiver _flightModeBroadcastReceiver;
        private bool _isActivityChangingConfigurations;
        private BroadcastReceiver _permissionsBroadcastReceiver;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public MainApplication()
        {
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            AccessibilityUtils.AdjustFontScale(activity);
            MessagingCenter.Subscribe<object>(activity, MessagingCenterKeys.KEY_FORCE_UPDATE,
                o => OnForceUpdate(activity));
        }

        public void OnActivityDestroyed(Activity activity)
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            ++_activityReferences;
            if (_activityReferences == 1 && !_isActivityChangingConfigurations)
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user has opened the app", null);

                // Log LoadPageActivity entered foreground after being put into background
                // because onResume() in LoadPageActivity is not called due to the pop-up window on the activity
                if (activity is LoadingPageActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
                }
            }
        }

        public void OnActivityStopped(Activity activity)
        {
            // check if the app entered background
            _isActivityChangingConfigurations = activity.IsChangingConfigurations;
            if (--_activityReferences == 0 && !_isActivityChangingConfigurations)
            {
                if (activity is QuestionnairePageActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire", null, GetCorrelationId());
                }
                else if (activity is QuestionnaireCountriesSelectionActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Countries Selection", null,
                        GetCorrelationId());
                }
                else if (activity is LoadingPageActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Loading Page", null, GetCorrelationId());
                }
                else if (activity is QuestionnaireConfirmLeaveActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Confirm Leave", null,
                        GetCorrelationId());
                }
                else if (activity is QuestionnairePreShareActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Pre Share", null,
                        GetCorrelationId());
                }
            }
        }

        private void Init()
        {
            _filter = new IntentFilter();
            _filter.AddAction("android.bluetooth.adapter.action.STATE_CHANGED");
            _filter.AddAction("android.location.PROVIDERS_CHANGED");

            AppDomain.CurrentDomain.UnhandledException += LogUtils.OnUnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += OnUnhandledAndroidException;


            DroidDependencyInjectionConfig.Init();
            Platform.Init(this);
            Current.Init(this);
            LocalesService.Initialize();

            new MigrationService().Migrate();

            _permissionsBroadcastReceiver = new PermissionsBroadcastReceiver();
            _flightModeBroadcastReceiver = new FlightModeHandlerBroadcastReceiver();
            _backgroundNotificationBroadcastReceiver = new BackgroundNotificationBroadcastReceiver();

            LogUtils.SendAllLogs();

            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                BackgroundFetchScheduler.ScheduleBackgroundFetch();
            }
        }

        private void OnUnhandledAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            if (e?.Exception != null)
            {
                string correlationId = GetCorrelationId();
                if (!string.IsNullOrEmpty(correlationId))
                {
                    LogUtils.LogMessage(
                        LogSeverity.INFO,
                        "The user has experienced native Android crash",
                        null,
                        correlationId);
                }

                string message = $"{nameof(MainApplication)}.{nameof(OnUnhandledAndroidException)}: "
                                 + (!e.Handled
                                     ? "Native unhandled crash"
                                     : "Native unhandled exception - not crashing");

                LogSeverity logLevel = e.Handled
                    ? LogSeverity.WARNING
                    : LogSeverity.ERROR;

                LogUtils.LogException(logLevel, e.Exception, message);
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Init();
            RegisterActivityLifecycleCallbacks(this);
            ManualGarbageCollectionTool();
            RegisterReceiver(_permissionsBroadcastReceiver, _filter);
            RegisterReceiver(_flightModeBroadcastReceiver, new IntentFilter("android.intent.action.AIRPLANE_MODE"));
            LocalBroadcastManager
                .GetInstance(ApplicationContext)
                .RegisterReceiver(
                    _backgroundNotificationBroadcastReceiver,
                    new IntentFilter("com.netcompany.smittestop_exposure_notification.background_notification"));
        }

        public override void OnTerminate()
        {
            UnregisterReceiver(_permissionsBroadcastReceiver);
            UnregisterReceiver(_flightModeBroadcastReceiver);
            LocalBroadcastManager
                .GetInstance(ApplicationContext)
                .UnregisterReceiver(_backgroundNotificationBroadcastReceiver);
            base.OnTerminate();
        }

        private void ManualGarbageCollectionTool()
        {
            #region MANUALLY GC

            // TODO: For memory management purpose this is saved to uncomment when needing constantly garbage collection
            //var constantGC = new System.Timers.Timer()
            //{
            //    Interval = 1000,
            //    AutoReset = true,
            //    Enabled = true
            //};
            //constantGC.Elapsed += GarbageCollect;

            #endregion MANUALLY GC
        }

        private void OnForceUpdate(Activity activity)
        {
            activity.RunOnUiThread(() =>
            {
                Intent intent = new Intent(this, typeof(ForceUpdateActivity));
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask | ActivityFlags.ClearTop);
                StartActivity(intent);
            });
            activity.Finish();
        }
    }
}