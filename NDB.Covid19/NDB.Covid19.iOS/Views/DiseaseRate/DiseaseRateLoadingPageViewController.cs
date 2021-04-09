using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.DiseaseRate
{
    public partial class DiseaseRateLoadingPageViewController : BaseViewController
    {
        private DiseaseRateViewModel _diseaseRateOfTodayData;

        private UIActivityIndicatorView _spinner;

        public DiseaseRateLoadingPageViewController(IntPtr handle) : base(handle)
        {
        }

        public static DiseaseRateLoadingPageViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("DiseaseRateLoadingPage", null);
            DiseaseRateLoadingPageViewController vc =
                storyboard.InstantiateInitialViewController() as DiseaseRateLoadingPageViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _spinner = StyleUtil.ShowSpinner(View, UIActivityIndicatorViewStyle.WhiteLarge);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FetchDataForDiseaseRateViewModel();
        }

        public async void FetchDataForDiseaseRateViewModel()
        {
            _diseaseRateOfTodayData = new DiseaseRateViewModel();

            try
            {
                bool isSuccess = await DiseaseRateViewModel.UpdateSSIDataAsync();
                if (!isSuccess && LocalPreferencesHelper.HasNeverSuccessfullyFetchedSSIData)
                {
                    OnError(new NullReferenceException("No SSI data"));
                    return;
                }

                LogUtils.LogMessage(LogSeverity.INFO, "Data for the disease rate of the day is loaded");

                OnSuccess();
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        private void Cleanup()
        {
            _spinner?.RemoveFromSuperview();
        }

        private void OnError(Exception e)
        {
            if (LocalPreferencesHelper.HasNeverSuccessfullyFetchedSSIData)
            {
                AuthErrorUtils.GoToTechnicalErrorSSINumbers(this, LogSeverity.ERROR, e,
                    "Could not load data for disease rate of the day, showing technical error page");
            }
            else
            {
                LogUtils.LogException(LogSeverity.ERROR, e,
                    "Could not load data for disease rate of the day, showing old data");
                UINavigationController vc =
                    DiseaseRateViewController.GetDiseaseRatePageControllerInNavigationController();
                PresentViewController(vc, true, null);
            }
        }

        private void OnSuccess()
        {
            Cleanup();
            UINavigationController vc = DiseaseRateViewController.GetDiseaseRatePageControllerInNavigationController();
            PresentViewController(vc, true, null);
        }
    }
}