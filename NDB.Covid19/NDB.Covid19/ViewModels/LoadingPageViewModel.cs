using System;
using System.Timers;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public static class LoadingPageViewModel
    {
        private static Timer _timer;
        private static int _textChangeSeconds = 4;

        private static Action _onFinished;

        public static string LOADING_PAGE_TEXT_NORMAL =>
            "LOADING_PAGE_TEXT_NORMAL".Translate();

        public static string LOADING_PAGE_TEXT_TIME_EXTENDED =>
            "LOADING_PAGE_TEXT_TIME_EXTENDED".Translate();

        public static void StartTimer(Action onFinished)
        {
            _timer = new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            _timer.Elapsed += TimerOnElapsed;
            _onFinished = onFinished;
            _textChangeSeconds = 4;
            _timer.Start();
        }

        private static void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (--_textChangeSeconds == 0)
            {
                _onFinished?.Invoke();
                _timer?.Stop();
                _timer = null;
            }
        }

        public static void ValidateData(Action onSuccess, Action onFail)
        {
            if (AuthenticationState.PersonalData.Validate())
            {
                onSuccess?.Invoke();
            }
            else
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "Loading Page - no miba data or token expired",
                    null,
                    GetCorrelationId());
                onFail?.Invoke();
            }
        }
    }
}