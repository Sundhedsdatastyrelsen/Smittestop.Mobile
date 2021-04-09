using System;
using Foundation;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;
using Xamarin.ExposureNotifications;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.ViewModels.LoadingPageViewModel;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class LoadingPageViewController : BaseViewController
    {
        private static int? _refusedCount = 0;

        private bool _isRunning;

        private UIActivityIndicatorView _spinner;

        public LoadingPageViewController(IntPtr handle) : base(handle)
        {
        }

        public static LoadingPageViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("LoadingPage", null);
            LoadingPageViewController vc = storyboard.InstantiateInitialViewController() as LoadingPageViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            StyleUtil.InitLabel(LoadingText,
                StyleUtil.FontType.FontBold,
                LOADING_PAGE_TEXT_NORMAL,
                16,
                22);

            LoadingText.TextColor = UIColor.White;
            _spinner = StyleUtil.ShowSpinner(
                Spinner,
                UIActivityIndicatorViewStyle.WhiteLarge,
                true,
                false);

            AddObservers();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
            if (!_isRunning)
            {
                StartTimer(OnFinished);
                ValidateData(RunBackgroundActivity, OnFail);
                _isRunning = true;
            }
        }

        private void OnFail()
        {
            OnError(new Exception("Validation Failed"), true);
        }

        private void OnFinished()
        {
            InvokeOnMainThread(() => { LoadingText.Text = LOADING_PAGE_TEXT_TIME_EXTENDED; });
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        private void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
                });
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE,
                _ =>
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Loading Page", null, GetCorrelationId());
                });
        }

        public async void RunBackgroundActivity()
        {
            try
            {
                await ExposureNotification.SubmitSelfDiagnosisAsync();
                LogUtils.LogMessage(LogSeverity.INFO, "The user agreed to share keys", null, GetCorrelationId());
                OnSuccess();
            }
            catch (Exception e)
            {
                if (e is NSErrorException nsErrorEx && nsErrorEx.Code == 4)
                {
                    if (_refusedCount == null)
                    {
                        _refusedCount = 0;
                    }

                    LogUtils.LogMessage(LogSeverity.INFO, "The user refused to share keys",
                        (_refusedCount++).ToString(), GetCorrelationId());
                    NavigationController?.PushViewController(QuestionnaireConfirmLeaveViewController.Create(), true);
                }
                else
                {
                    _refusedCount = 0;
                    OnError(e);
                }
            }
        }

        private void Cleanup()
        {
            _spinner?.RemoveFromSuperview();
        }

        private void OnError(Exception e, bool isOnFail = false)
        {
            if (!isOnFail)
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "Something went wrong during key sharing (INFO with correlation id)",
                    e.Message,
                    GetCorrelationId());
            }

            Cleanup();
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, e, "Pushing keys failed");
        }

        private void OnSuccess()
        {
            _refusedCount = 0;
            Cleanup();
            NavigationController?.PushViewController(UploadCompletedViewController.Create(), true);
        }
    }
}