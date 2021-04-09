﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.InfectionStatus;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using Xamarin.Essentials;
using XamarinShortcutBadger;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using Object = Java.Lang.Object;

namespace NDB.Covid19.Droid.Views.Messages
{
    [Activity(
        Theme = "@style/AppTheme",
        ParentActivity = typeof(InfectionStatusActivity),
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class MessagesActivity : AppCompatActivity
    {
        private MessagesAdapter _adapterMessages;
        private ViewGroup _closeButton;
        private ListView _messagesList;
        private LinearLayout _noItemsLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = MessagesViewModel.MESSAGES_HEADER;
            SetContentView(Resource.Layout.messages_page);
            Init();
            LogUtils.LogMessage(LogSeverity.INFO, "User opened Messages", null);
        }

        protected override void OnDestroy()
        {
            MessagesViewModel.UnsubscribeMessages(this);
            Task.Run(() => MessagesViewModel.MarkAllMessagesAsRead());
            base.OnDestroy();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await MessageUtils.RemoveAllOlderThan(Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES);
            CloseLocalNotification();
            Update();
            RemoveBadgeCounterOnOlderPlatforms();
        }

        private void RemoveBadgeCounterOnOlderPlatforms()
        {
            bool isLowerVersion = Build.VERSION.SdkInt < BuildVersionCodes.O;
            bool isBadgeCounterSupported = ShortcutBadger.IsBadgeCounterSupported(ApplicationContext);

            // Use Plugin for badges on older platforms that support them
            if (isLowerVersion && isBadgeCounterSupported)
            {
                ShortcutBadger.RemoveCount(ApplicationContext);
            }
        }

        private void CloseLocalNotification()
        {
            NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(Current.Activity);
            notificationManagerCompat.Cancel((int) NotificationsEnum.NewMessageReceived);
        }

        private void Init()
        {
            SetLogoBasedOnAppLanguage();

            MessagesViewModel.SubscribeMessages(this, ClearAndAddNewMessages);

            FindViewById<TextView>(Resource.Id.messages_page_title).Text = MessagesViewModel.MESSAGES_HEADER;
            FindViewById<TextView>(Resource.Id.message_last_update).Text = MessagesViewModel.LastUpdateString;

            FindViewById<TextView>(Resource.Id.no_items_title).Text = MessagesViewModel.MESSAGES_NO_ITEMS_TITLE;
            FindViewById<TextView>(Resource.Id.no_items_description).Text =
                MessagesViewModel.MESSAGES_NO_ITEMS_DESCRIPTION;

            _messagesList = FindViewById<ListView>(Resource.Id.messages_list);
            _noItemsLayout = FindViewById<LinearLayout>(Resource.Id.no_items_message);

            _closeButton = FindViewById<ViewGroup>(Resource.Id.close_cross_btn);
            _closeButton.Click += new StressUtils.SingleClick(OnCloseBtnClicked).Run;
            _closeButton.ContentDescription = MessagesViewModel.MESSAGES_ACCESSIBILITY_CLOSE_BUTTON;

            _adapterMessages = new MessagesAdapter(this, new MessageItemViewModel[0]);
            _messagesList.Adapter = _adapterMessages;
            _messagesList.OnItemClickListener = new ItemClickListener(_adapterMessages);
            ShowList(false);
        }

        private void SetLogoBasedOnAppLanguage()
        {
            View logo = FindViewById<View>(Resource.Id.message_logo);
            string appLanguage = LocalesService.GetLanguage();
            logo?.SetBackgroundResource(appLanguage != null && appLanguage.ToLower() == "en"
                ? Resource.Drawable.patient_logo_en
                : Resource.Drawable.patient_logo_da);
        }

        private void HandleBeforeActivityClose()
        {
            MessagesViewModel.MarkAllMessagesAsRead();
        }

        public override void OnBackPressed()
        {
            HandleBeforeActivityClose();
            base.OnBackPressed();
        }

        private void OnCloseBtnClicked(object arg1, EventArgs arg2)
        {
            HandleBeforeActivityClose();
            Finish();
        }

        public void ClearAndAddNewMessages(List<MessageItemViewModel> messages)
        {
            _adapterMessages.ClearList();
            ShowList(messages.Count > 0);
            _adapterMessages.AddItems(messages);
            FindViewById<TextView>(Resource.Id.message_last_update).Text = MessagesViewModel.LastUpdateString;
        }

        public async void Update()
        {
            ClearAndAddNewMessages(await MessagesViewModel.GetMessages());
        }

        private void ShowList(bool isShown)
        {
            _messagesList.Visibility = isShown ? ViewStates.Visible : ViewStates.Invisible;
            _noItemsLayout.Visibility = isShown ? ViewStates.Invisible : ViewStates.Visible;
        }

        private class ItemClickListener : Object, AdapterView.IOnItemClickListener
        {
            private readonly MessagesAdapter _adapterMessages;

            public ItemClickListener(MessagesAdapter adapterMessages)
            {
                _adapterMessages = adapterMessages;
            }

            public async void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                await ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(_adapterMessages[position].MessageLink,
                    BrowserLaunchMode.SystemPreferred);
                _adapterMessages[position].IsRead = true;
                _adapterMessages.NotifyDataSetChanged();
            }
        }
    }
}