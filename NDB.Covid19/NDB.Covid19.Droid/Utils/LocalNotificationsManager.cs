using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.LocalBroadcastManager.Content;
using Java.Lang;
using NDB.Covid19.Droid.Views;
using NDB.Covid19.Droid.Views.Messages;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.ViewModels;
using XamarinShortcutBadger;
using static NDB.Covid19.ViewModels.NotificationChannelsViewModel;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using TaskStackBuilder = Android.App.TaskStackBuilder;

namespace NDB.Covid19.Droid.Utils
{
    public class LocalNotificationsManager : ILocalNotificationsManager
    {
        private const string _broadcastName =
            "com.netcompany.smittestop_exposure_notification.background_notification";

        private readonly string _backgroundFetchChannelId = "4_background_channel";

        private readonly Context _context;
        private readonly string _countdownChannelId = "3_countdown_channel";
        private readonly string _exposureChannelId = "0_exposure_channel";
        private readonly string _permissionsChannelId = "1_permissions_channel";
        private readonly string _reminderChannelId = "2_reminder_channel";

        public LocalNotificationsManager(Context context = null)
        {
            _context = context;
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            CreateChannels();
        }

        private Context NotificationContext => _context ?? Current.Activity ?? Current.AppContext;

        public void GenerateLocalNotification(NotificationViewModel notificationViewModel, long triggerInSeconds)
        {
            BroadcastNotification(notificationViewModel, NotificationType.Local);
        }

        public void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel)
        {
            BroadcastNotification(viewModel, NotificationType.InBackground);
        }

        public void GenerateLocalPermissionsNotification(NotificationViewModel viewModel)
        {
            BroadcastNotification(viewModel, NotificationType.Permissions);
        }

        public void GenerateDelayedNotification(NotificationViewModel viewModel, long ticks)
        {
            Intent intent = new Intent();
            intent.SetAction(_broadcastName);
            intent.PutExtra("type", (int) NotificationType.ForegroundWithUpdates);
            intent.PutExtra("data", (int) viewModel.Type);
            intent.PutExtra("ticks", ticks);
            LocalBroadcastManager.GetInstance(Current.Activity ?? Current.AppContext).SendBroadcast(intent);
        }

        private void CreateChannels()
        {
            NotificationManager notificationManager =
                (NotificationManager) NotificationContext.GetSystemService(Context.NotificationService);

            NotificationChannel exposureChannel =
                new NotificationChannel(
                    _exposureChannelId,
                    NOTIFICATION_CHANNEL_EXPOSURE_NAME,
                    NotificationImportance.High)
                {
                    Description = NOTIFICATION_CHANNEL_EXPOSURE_DESCRIPTION
                };
            exposureChannel.SetShowBadge(true);

            NotificationChannel permissionsChannel =
                new NotificationChannel(
                    _permissionsChannelId,
                    NOTIFICATION_CHANNEL_PERMISSIONS_NAME,
                    NotificationImportance.High)
                {
                    Description = NOTIFICATION_CHANNEL_PERMISSIONS_DESCRIPTION
                };
            permissionsChannel.SetShowBadge(true);

            NotificationChannel backgroundChannel =
                new NotificationChannel(
                    _backgroundFetchChannelId,
                    NOTIFICATION_CHANNEL_BACKGROUND_FETCH_NAME,
                    NotificationImportance.Low)
                {
                    Description = NOTIFICATION_CHANNEL_BACKGROUND_FETCH_DESCRIPTION
                };
            backgroundChannel.SetShowBadge(false);

            NotificationChannel countdownChannel =
                new NotificationChannel(
                    _countdownChannelId,
                    NOTIFICATION_CHANNEL_COUNTDOWN_NAME,
                    NotificationImportance.Low)
                {
                    Description = NOTIFICATION_CHANNEL_COUNTDOWN_DESCRIPTION
                };
            backgroundChannel.SetShowBadge(false);

            NotificationChannel remindersChannel =
                new NotificationChannel(
                    _reminderChannelId,
                    NOTIFICATION_CHANNEL_REMINDER_NAME,
                    NotificationImportance.High)
                {
                    Description = NOTIFICATION_CHANNEL_REMINDER_DESCRIPTION
                };
            permissionsChannel.SetShowBadge(true);

            notificationManager?.CreateNotificationChannels(
                new List<NotificationChannel>
                {
                    exposureChannel,
                    permissionsChannel,
                    backgroundChannel,
                    countdownChannel,
                    remindersChannel
                });
        }

        private string SelectChannel(NotificationsEnum type)
        {
            return type switch
            {
                NotificationsEnum.NewMessageReceived => _exposureChannelId,
                NotificationsEnum.BackgroundFetch => _backgroundFetchChannelId,
                NotificationsEnum.TimedReminder => _countdownChannelId,
                NotificationsEnum.TimedReminderFinished => _reminderChannelId,
                _ => _permissionsChannelId
            };
        }

        private NotificationPriority SelectPriorityForLowerVersions(NotificationsEnum type)
        {
            return type switch
            {
                NotificationsEnum.NewMessageReceived => NotificationPriority.Default,
                NotificationsEnum.BackgroundFetch => NotificationPriority.Low,
                _ => NotificationPriority.Default
            };
        }

        public Task<Notification> CreateNotification(NotificationViewModel notificationViewModel)
        {
            PendingIntent resultPendingIntent = InitResultIntentBasingOnViewModel(notificationViewModel);
            NotificationCompat.Builder builder =
                new NotificationCompat.Builder(NotificationContext, SelectChannel(notificationViewModel.Type))
                    .SetAutoCancel(
                        true) // Dismiss the notification from the notification area when the user clicks on it
                    .SetContentTitle(notificationViewModel.Title) // Set the title
                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(notificationViewModel.Body))
                    .SetContentText(notificationViewModel.Body) // the message to display.
                    .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                    .SetVibrate(null)
                    .SetSound(null)
                    .SetNumber(1)
                    .SetCategory(NotificationCompat.CategoryMessage)
                    .SetOnlyAlertOnce(true);

            // This is the icon to display
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                builder.SetColor(Resource.Color.colorPrimary);
            }

            builder.SetSmallIcon(Resource.Drawable.ic_smittestop);

            bool isLowerVersion = Build.VERSION.SdkInt < BuildVersionCodes.O;
            // Lower android versions should have priority instead of importance as they do not support channels
            if (isLowerVersion)
            {
                builder.SetPriority((int) SelectPriorityForLowerVersions(notificationViewModel.Type));
            }

            Notification notification = builder.Build();

            bool isBadgeCounterSupported = ShortcutBadger.IsBadgeCounterSupported(NotificationContext);
            bool isMessage = notificationViewModel.Type == NotificationsEnum.NewMessageReceived;
            bool areNotificationsEnabled =
                NotificationManagerCompat.From(NotificationContext).AreNotificationsEnabled();

            // Use Plugin for badges on older platforms that support them
            if (isLowerVersion &&
                isBadgeCounterSupported &&
                isMessage &&
                areNotificationsEnabled)
            {
                ShortcutBadger.ApplyNotification(NotificationContext, notification, 1);
            }

            return Task.FromResult(notification);
        }

        public Notification CreateNotificationWithExtraLongData(
            NotificationViewModel notificationViewModel, long ticks = 0)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ticks);

            PendingIntent resultPendingIntent = InitResultIntentBasingOnViewModel(notificationViewModel);
            NotificationCompat.Builder builder =
                new NotificationCompat.Builder(NotificationContext, SelectChannel(notificationViewModel.Type))
                    .SetContentTitle(notificationViewModel.Title) // Set the title
                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(string.Format(notificationViewModel.Body,
                        t.ToString("hh':'mm':'ss"))))
                    .SetContentText(string.Format(notificationViewModel.Body,
                        t.ToString("hh':'mm':'ss"))) // the message to display.
                    .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                    .SetVibrate(null)
                    .SetSound(null)
                    .SetCategory(NotificationCompat.CategoryStatus)
                    .SetOnlyAlertOnce(true);

            // This is the icon to display
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                builder.SetColor(Resource.Color.colorPrimary);
            }

            builder.SetSmallIcon(Resource.Drawable.ic_smittestop);

            return builder.Build();
        }

        private PendingIntent InitResultIntentBasingOnViewModel(NotificationViewModel notificationViewModel)
        {
            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent;

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(NotificationContext);

            if (notificationViewModel.Type == NotificationsEnum.NewMessageReceived.Data().Type)
            {
                resultIntent = new Intent(NotificationContext, typeof(MessagesActivity));
                stackBuilder.AddParentStack(Class.FromType(typeof(MessagesActivity)));
            }
            else
            {
                resultIntent = new Intent(NotificationContext, typeof(InitializerActivity));
                stackBuilder.AddParentStack(Class.FromType(typeof(InitializerActivity)));
            }

            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            return stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
        }

        private static void BroadcastNotification(NotificationViewModel viewModel, NotificationType type)
        {
            Intent intent = new Intent();
            intent.SetAction(_broadcastName);
            intent.PutExtra("type", (int) type);
            intent.PutExtra("data", (int) viewModel.Type);
            LocalBroadcastManager.GetInstance(Current.Activity ?? Current.AppContext).SendBroadcast(intent);
        }
    }
}