using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using Moq;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData;
using NDB.Covid19.WebServices.ExposureNotification;

namespace NDB.Covid19.Test.Helpers
{
    public class ZipDownloaderHelper
    {
        private static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        public void SetLastPulledDate(DateTime lastSuccessPullDate, int lastSuccessPullBatch)
        {
            _preferences.Set(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, lastSuccessPullDate);
            _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_SUBMITTED, lastSuccessPullBatch);
            LocalPreferencesHelper.GetLastPullKeysSucceededDateTime();
        }

        /// <summary>
        ///     Provide the status codes to set for each defined request string.
        /// </summary>
        /// <param name="mockResults">
        ///     A list of objects that define what to mock for a given request
        ///     <returns>For status 200 and 204, a zip is returned. For other statuses, null is returned in the Data object</returns>
        public ExposureNotificationWebService MockedService(List<PullKeysMockData> mockResults)
        {
            Mock<ExposureNotificationWebService> mock = new Mock<ExposureNotificationWebService>();

            foreach (PullKeysMockData data in mockResults)
            {
                mock.Setup(service =>
                        service.GetDiagnosisKeys(data.RequestString, It.IsAny<CancellationToken>()))
                    .Returns(
                        () =>
                        {
                            var ms = new MemoryStream();
                            using (var zipFile = new ZipArchive(ms, ZipArchiveMode.Create, true))
                            {
                                // Copy the bin contents into the entry
                                var binEntry = zipFile.CreateEntry("mock", CompressionLevel.Optimal);
                                using (var binStream = binEntry.Open())
                                {
                                    using (var mockContent = new MemoryStream())
                                    {
                                        mockContent.Write(Encoding.Default.GetBytes("mockdata"));
                                        mockContent.CopyTo(binStream);
                                    }
                                }
                            }

                            ms.Seek(0, SeekOrigin.Begin);

                            HttpHeaders headers = new HttpClient().DefaultRequestHeaders;
                            if (data.LastBatchReturned != null)
                            {
                                headers.Add(ZipDownloader.LastBatchReturnedHeader,
                                    ((int) data.LastBatchReturned).ToString());
                            }

                            if (data.MoreBatchesExistForDate != null)
                            {
                                headers.Add(ZipDownloader.MoreBatchesExistHeader,
                                    data.MoreBatchesExistForDate.ToString());
                            }

                            if (data.StatusCode == 200 || data.StatusCode == 204)
                            {
                                //In messages, this time will be shown as "last updated", to show the user when we last checked for exposures.
                                LocalPreferencesHelper.UpdateLastUpdatedDate();
                            }

                            return Task.Run(() =>
                                new ApiResponse<Stream>(data.RequestString, HttpMethod.Get)
                                {
                                    StatusCode = data.StatusCode,
                                    Data = data.StatusCode == 200 ? ms : null,
                                    Headers = headers
                                });
                        });
            }

            return mock.Object;
        }
    }

    public class PullKeysMockData
    {
        public PullKeysMockData(DateTime date, int requestBatchNumber, BatchType batchType)
        {
            RequestString = GetRequest(date, requestBatchNumber, batchType);
        }

        public PullKeysMockData(DateTime date, int requestBatchNumber)
        {
            RequestString = GetRequest(date, requestBatchNumber);
        }

        public string RequestString { get; set; }
        public int? LastBatchReturned { get; set; }
        public bool? MoreBatchesExistForDate { get; set; }
        public int StatusCode { get; set; }

        public PullKeysMockData HttpStatusCode(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public PullKeysMockData WithLastBatchHeader(int lastBatch)
        {
            LastBatchReturned = lastBatch;
            return this;
        }

        public PullKeysMockData WithMoreBatchesExistHeader(bool moreBatchesExist)
        {
            MoreBatchesExistForDate = moreBatchesExist;
            return this;
        }

        private string GetRequest(DateTime date, int batchNum, BatchType batchType = BatchType.DK)
        {
            return new PullKeysParams
            {
                Date = date,
                BatchNumber = batchNum,
                BatchType = batchType
            }.ToBatchFileRequest();
        }
    }
}