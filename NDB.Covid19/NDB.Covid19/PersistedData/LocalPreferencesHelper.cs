using System;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.WebServices.ExposureNotification;

namespace NDB.Covid19.PersistedData
{
    public class LocalPreferencesHelper
    {
        private static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        public static int MigrationCount
        {
            get => _preferences.Get(PreferencesKeys.MIGRATION_COUNT, 0);
            set => _preferences.Set(PreferencesKeys.MIGRATION_COUNT, value);
        }

        //Used to determine if onboarding should be shown, or the user should be sent directly to the result page.
        public static bool IsOnboardingCompleted
        {
            get => _preferences.Get(PreferencesKeys.IS_ONBOARDING_COMPLETED_PREF, false);
            set => _preferences.Set(PreferencesKeys.IS_ONBOARDING_COMPLETED_PREF, value);
        }

        // USed to distinguish if user already has seen WhatIsNew page
        public static bool IsOnboardingCountriesCompleted
        {
            get => _preferences.Get(PreferencesKeys.IS_ONBOARDING_COUNTRIES_COMPLETED_PREF, false);
            set => _preferences.Set(PreferencesKeys.IS_ONBOARDING_COUNTRIES_COMPLETED_PREF, value);
        }

        // The date of when the last update to numbers pulled from Statens Serum Institut (SSI) were, which is used on the "Disease rate of the day" page.
        public static DateTime SSILastUpdateDateTime
        {
            get => _preferences.Get(PreferencesKeys.SSI_DATA_LAST_UPDATED_PREF, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.SSI_DATA_LAST_UPDATED_PREF, value);
        }

        // The date of when the last update to the download numbers were, which is used on the "Disease rate of the day" page.
        public static DateTime APPDownloadNumberLastUpdateDateTime
        {
            get => _preferences.Get(PreferencesKeys.APP_DOWNLOAD_NUMBERS_LAST_UPDATED_PREF, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.APP_DOWNLOAD_NUMBERS_LAST_UPDATED_PREF, value);
        }

        public static bool HasNeverSuccessfullyFetchedSSIData
        {
            get => _preferences.Get(PreferencesKeys.SSI_DATA_HAS_NEVER_BEEN_CALLED, true);
            set => _preferences.Set(PreferencesKeys.SSI_DATA_HAS_NEVER_BEEN_CALLED, value);
        }

        public static bool IsScrollDownShown
        {
            get => _preferences.Get(PreferencesKeys.IS_SCROLL_DOWN_SHOWN_PREF, false);
            set => _preferences.Set(PreferencesKeys.IS_SCROLL_DOWN_SHOWN_PREF, value);
        }

        //The last batch that was successfully fetched but not yet submitted to the EN API.
        public static int LastPullKeysBatchNumberNotSubmitted
        {
            get => _preferences.Get(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED, 0);
            set => _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED, value);
        }

        //The last batch that was successfully fetched AND submitted to the EN API.
        public static int LastPullKeysBatchNumberSuccessfullySubmitted
        {
            get => _preferences.Get(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_SUBMITTED, 0);
            set => _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_NUMBER_SUBMITTED, value);
        }

        //The last pulled batch type, which is needed to know when to reset the batch number to 1 after approving the terms. See PullKeysParams.cs
        public static BatchType LastPulledBatchType
        {
            get => _preferences.Get(PreferencesKeys.LAST_PULLED_BATCH_TYPE, "dk").ToBatchType();
            set => _preferences.Set(PreferencesKeys.LAST_PULLED_BATCH_TYPE, value.ToTypeString());
        }

        public static bool TermsNotificationWasShown
        {
            get => _preferences.Get(PreferencesKeys.TERMS_NOTIFICATION_WAS_SENT, false);
            set => _preferences.Set(PreferencesKeys.TERMS_NOTIFICATION_WAS_SENT, value);
        }

        public static double ExposureTimeThreshold
        {
            get => _preferences.Get(PreferencesKeys.EXPOSURE_TIME_THRESHOLD, Conf.EXPOSURE_TIME_THRESHOLD);
            set => _preferences.Set(PreferencesKeys.EXPOSURE_TIME_THRESHOLD, value);
        }

        public static double LowAttenuationDurationMultiplier
        {
            get => _preferences.Get(PreferencesKeys.LOW_ATTENUATION_DURATION_MULTIPLIER,
                Conf.LOW_ATTENUATION_DURATION_MULTIPLIER);
            set => _preferences.Set(PreferencesKeys.LOW_ATTENUATION_DURATION_MULTIPLIER, value);
        }

        public static double MiddleAttenuationDurationMultiplier
        {
            get => _preferences.Get(PreferencesKeys.MIDDLE_ATTENUATION_DURATION_MULTIPLIER,
                Conf.MIDDLE_ATTENUATION_DURATION_MULTIPLIER);
            set => _preferences.Set(PreferencesKeys.MIDDLE_ATTENUATION_DURATION_MULTIPLIER, value);
        }

        public static double HighAttenuationDurationMultiplier
        {
            get => _preferences.Get(PreferencesKeys.HIGH_ATTENUATION_DURATION_MULTIPLIER,
                Conf.HIGH_ATTENUATION_DURATION_MULTIPLIER);
            set => _preferences.Set(PreferencesKeys.HIGH_ATTENUATION_DURATION_MULTIPLIER, value);
        }

        public static DateTime LastPermissionsNotificationDateTimeUtc
        {
            get => _preferences.Get(PreferencesKeys.LAST_PERMISSIONS_NOTIFICATION_DATE_TIME, DateTime.MinValue);
            set => _preferences.Set(PreferencesKeys.LAST_PERMISSIONS_NOTIFICATION_DATE_TIME, value);
        }

        public static DateTime LastNTPUtcDateTime
        {
            get => _preferences.Get(PreferencesKeys.LAST_NTP_UTC_DATE_TIME, Conf.DATE_TIME_REPLACEMENT);
            set => _preferences.Set(PreferencesKeys.LAST_NTP_UTC_DATE_TIME, value);
        }

        public static bool DidFirstFileOfTheDayEndedWith204
        {
            get => _preferences.Get(PreferencesKeys.FETCHING_ACROSS_DATES_204_FIRST_BATCH, false);
            set => _preferences.Set(PreferencesKeys.FETCHING_ACROSS_DATES_204_FIRST_BATCH, value);
        }

        public static bool GetIsDownloadWithMobileDataEnabled()
        {
            return _preferences.Get(PreferencesKeys.USE_MOBILE_DATA_PREF, true);
        }

        public static void SetIsDownloadWithMobileDataEnabled(bool isDownloadWithMobileDataEnabled)
        {
            _preferences.Set(PreferencesKeys.USE_MOBILE_DATA_PREF, isDownloadWithMobileDataEnabled);
        }

        // This is the date of the last fetch, which is displayed to the user on the Messages screen.
        //  The keys are not submitted yet
        public static DateTime GetUpdatedDateTime()
        {
            return _preferences.Get(PreferencesKeys.MESSAGES_LAST_UPDATED_PREF, DateTime.MinValue);
        }

        public static void UpdateLastUpdatedDate()
        {
            _preferences.Set(PreferencesKeys.MESSAGES_LAST_UPDATED_PREF, SystemTime.Now());
        }

        // The date of the last successful pulling of the keys
        // Keys that were successfully pulled AND submitted to the EN API.
        public static DateTime GetLastPullKeysSucceededDateTime()
        {
            return _preferences.Get(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, DateTime.MinValue);
        }

        public static void UpdateLastPullKeysSucceededDateTime()
        {
            _preferences.Set(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, SystemTime.Now());

            int batchNumber = LastPullKeysBatchNumberNotSubmitted;
            LastPullKeysBatchNumberSuccessfullySubmitted = batchNumber;
        }

        public static string GetAppLanguage()
        {
            return _preferences.Get(PreferencesKeys.APP_LANGUAGE, null);
        }

        public static void SetAppLanguage(string language)
        {
            _preferences.Set(PreferencesKeys.APP_LANGUAGE, language);
        }

        // The id is used to identify authentication/submission flow in the logs.
        public static string GetCorrelationId()
        {
            return _preferences.Get(PreferencesKeys.CORRELATION_ID, null);
        }

        public static void UpdateCorrelationId(string correlationId)
        {
            _preferences.Set(PreferencesKeys.CORRELATION_ID, correlationId);
        }

        //Data from SSI
        public static class DiseaseRateOfTheDay
        {
            public static int SSIConfirmedCasesToday
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_CONFIRMED_CASES_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_CONFIRMED_CASES_TODAY_PREF, value);
            }

            public static int SSIConfirmedCasesTotal
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_CONFIRMED_CASES_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_CONFIRMED_CASES_TOTAL_PREF, value);
            }

            public static int SSIDeathsToday
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_DEATHS_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_DEATHS_TODAY_PREF, value);
            }

            public static int SSIDeathsTotal
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_DEATHS_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_DEATHS_TOTAL_PREF, value);
            }

            public static int SSITestsConductedToday
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_TESTS_CONDUCTED_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_TESTS_CONDUCTED_TODAY_PREF, value);
            }

            public static int SSITestsConductedTotal
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_TESTS_CONDUCTED_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_TESTS_CONDUCTED_TOTAL_PREF, value);
            }

            [Obsolete]
            public static int SSIPatientsAdmittedToday
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_PATIENTS_ADMITTED_TODAY_PREF, 0);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_PATIENTS_ADMITTED_TODAY_PREF, value);
            }

            public static DateTime SSIVaccinatedEntryDate
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_VACCINATED_ENTRY_DATE_PREF, DateTime.MinValue);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_VACCINATED_ENTRY_DATE_PREF, value);
            }

            public static double SSIVaccinatedFirst
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_VACCINATED_FIRST_PREF, 0d);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_VACCINATED_FIRST_PREF, value);
            }

            public static double SSIVaccinatedSecond
            {
                get => _preferences.Get(PreferencesKeys.SSI_DATA_VACCINATED_SECOND_PREF, 0d);
                set => _preferences.Set(PreferencesKeys.SSI_DATA_VACCINATED_SECOND_PREF, value);
            }

            public static int APPNumberOfPositiveTestsResultsLast7Days
            {
                get => _preferences.Get(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_LAST_7_DAYS_PREF, 0);
                set => _preferences.Set(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_LAST_7_DAYS_PREF,
                    value);
            }

            public static int APPNumberOfPositiveTestsResultsTotal
            {
                get => _preferences.Get(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_TOTAL_PREF, value);
            }

            public static int APPSmittestopDownloadsTotal
            {
                get => _preferences.Get(PreferencesKeys.APP_DATA_SMITTESTOP_DOWNLOADS_TOTAL_PREF, 0);
                set => _preferences.Set(PreferencesKeys.APP_DATA_SMITTESTOP_DOWNLOADS_TOTAL_PREF, value);
            }

            public static void UpdateAll(DiseaseRateOfTheDayDTO dto)
            {
                SSILastUpdateDateTime = dto.SSIStatistics.EntryDate;
                SSIConfirmedCasesToday = dto.SSIStatistics.ConfirmedCasesToday;
                SSIConfirmedCasesTotal = dto.SSIStatistics.ConfirmedCasesTotal;
                SSIDeathsToday = dto.SSIStatistics.DeathsToday;
                SSIDeathsTotal = dto.SSIStatistics.DeathsTotal;
                SSITestsConductedToday = dto.SSIStatistics.TestsConductedToday;
                SSITestsConductedTotal = dto.SSIStatistics.TestsConductedTotal;
                SSIPatientsAdmittedToday = dto.SSIStatistics.patientsAdmittedToday;

                SSIVaccinatedEntryDate = dto.SSIStatisticsVaccination.EntryDate;
                SSIVaccinatedFirst = decimal.ToDouble(dto.SSIStatisticsVaccination.VaccinationFirst);
                SSIVaccinatedSecond = decimal.ToDouble(dto.SSIStatisticsVaccination.VaccinationSecond);

                APPNumberOfPositiveTestsResultsLast7Days = dto.AppStatistics.NumberOfPositiveTestsResultsLast7Days;
                APPNumberOfPositiveTestsResultsTotal = dto.AppStatistics.NumberOfPositiveTestsResultsTotal;
                APPSmittestopDownloadsTotal = dto.AppStatistics.SmittestopDownloadsTotal;
                APPDownloadNumberLastUpdateDateTime = dto.AppStatistics.EntryDate;
                HasNeverSuccessfullyFetchedSSIData = false;
            }
        }
    }
}