using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content;
using AndroidX.Work;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using Xamarin.Essentials;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.Droid.Utils
{
    internal class BackgroundFetchScheduler
    {
        private static readonly string uniqueWorkName = "exposurenotification";

        public static void ScheduleBackgroundFetch()
        {
            if(Conf.APP_DISABLED)
            {
                Debug.Print($"APP_DISABLED: Not scheduling background work");
                DeviceUtils.StopScanServices(); // Stop scan services if running.
                WorkManager.GetInstance(Platform.AppContext).CancelUniqueWork(uniqueWorkName); // Stop work if running.
                return;
            }

            Debug.Print($"{nameof(BackgroundFetchScheduler)}: Scheduling background work for fetching keys.");

            //The interval has to be minimum 15 minutes.
            //Note that execution may be delayed because WorkManager is subject to OS battery optimizations,
            //such as doze mode.
            PeriodicWorkRequest.Builder periodicWorkRequestBuilder = new PeriodicWorkRequest.Builder(
                typeof(BackgroundFetchWorker),
                Conf.BACKGROUND_FETCH_REPEAT_INTERVAL_ANDROID);

            periodicWorkRequestBuilder
                //Start time is when the first time will be
                .SetPeriodStartTime(TimeSpan.FromSeconds(1))
                //If Result.InvokeRetry() is called it will linearly double the amount of time specified below before it tries again.
                .SetBackoffCriteria(BackoffPolicy.Linear, TimeSpan.FromSeconds(10))
                .SetConstraints(new Constraints.Builder()
                    //Only run if connected to the internet
                    .SetRequiredNetworkType(NetworkType.Connected)
                    .Build());

            PeriodicWorkRequest periodicWorkRequest = periodicWorkRequestBuilder.Build();

            WorkManager workManager = WorkManager.GetInstance(Platform.AppContext);
            workManager.EnqueueUniquePeriodicWork(uniqueWorkName,
                ExistingPeriodicWorkPolicy.Keep,
                periodicWorkRequest);
        }

        private class BackgroundFetchWorker : Worker
        {
            private static int _runAttemptCount;
            private static readonly int _minimalDisplayTime = 3000;

            public BackgroundFetchWorker(Context context, WorkerParameters workerParameters)
                : base(context, workerParameters)
            {
            }

            public override Result DoWork()
            {
                try
                {
                    SetForegroundAsync(CreateForegroundInfo());
                    Task.Run(DoAsyncWork).GetAwaiter().GetResult();
                    return Result.InvokeSuccess();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                    if (_runAttemptCount < 3)
                    {
                        LogUtils.LogException(LogSeverity.WARNING, ex,
                            $"{nameof(BackgroundFetchScheduler)}.{nameof(BackgroundFetchWorker)}.{nameof(DoWork)}: Failed to perform key background fetch. Retrying now.");
                        ++_runAttemptCount;
                        return Result.InvokeRetry();
                    }

                    LogUtils.LogException(LogSeverity.WARNING, ex,
                        $"{nameof(BackgroundFetchScheduler)}.{nameof(BackgroundFetchWorker)}.{nameof(DoWork)}: Failed to perform key background fetch. Pull aborted. BG Task is rescheduled");
                    _runAttemptCount = 0;
                    return Result.InvokeFailure();
                }
            }

            private ForegroundInfo CreateForegroundInfo()
            {
                return new ForegroundInfo(
                    (int) NotificationsEnum.BackgroundFetch,
                    new LocalNotificationsManager().CreateNotification(NotificationsEnum.BackgroundFetch.Data())
                        .GetAwaiter().GetResult());
            }

            private async Task DoAsyncWork()
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                try
                {
                    Debug.Print($"UpdateKeysFromServer: Current time of day: {DateTime.Now.TimeOfDay}");
                    if (await ExposureNotification.IsEnabledAsync())
                    {
                        // SetDiagnosisKeysDataMappingAsync should be used on Android with EN API v2 to configure
                        // how the Exposure Notifications system translates diagnosis key data
                        // to the corresponding fields in ExposureWindow
                        // src: https://developers.google.com/android/exposure-notifications/meaningful-exposures#map-diag-keys
                        await DiagnosisKeysDataMappingUtils.SetDiagnosisKeysDataMappingAsync();

                        // UpdateKeysFromServer() does:
                        //     run ExposureNotificationHandler.FetchExposureKeyBatchFilesFromServerAsync()
                        //     try to find matches by doing DetectExposuresAsync()
                        //         DetectExposuresAsync() uses a config from ExposureNotificationHandler.GetConfigurationAsync()
                        //     if matches:
                        //         run ExposureNotificationHandler.ExposureDetectedAsync
                        await ExposureNotification.UpdateKeysFromServer();
                    }
                    else
                    {
                        LogUtils.LogMessage(LogSeverity.WARNING,
                            $"{nameof(BackgroundFetchScheduler)}.{nameof(DoAsyncWork)} (Android): EN API is not enabled. Aborting pull.");
                    }
                }
                catch (Exception e)
                {
                    // To make it not crash on devices with normal Play Services before the app is whitelisted
                    if (!e.HandleExposureNotificationException(nameof(BackgroundFetchScheduler), nameof(DoAsyncWork)))
                    {
#if DEBUG
                        throw;
#endif
                    }
                }

                timer.Stop();

                if (timer.ElapsedMilliseconds < _minimalDisplayTime)
                {
                    await Task.Delay((int) (_minimalDisplayTime - timer.ElapsedMilliseconds));
                }
            }
        }
    }
}