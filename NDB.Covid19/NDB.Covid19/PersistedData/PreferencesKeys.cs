using System;

namespace NDB.Covid19.PersistedData
{
    public class PreferencesKeys
    {
        public static readonly string MIGRATION_COUNT = "MIGRATION_COUNT";

        public static readonly string MESSAGES_LAST_UPDATED_PREF = "MESSAGES_LAST_UPDATED_PREF";

        public static readonly string SSI_DATA_HAS_NEVER_BEEN_CALLED = "SSI_DATA_HAS_NEVER_BEEN_CALLED";
        public static readonly string SSI_DATA_LAST_UPDATED_PREF = "SSI_DATA_LAST_UPDATED_PREF";
        public static readonly string SSI_DATA_CONFIRMED_CASES_TODAY_PREF = "SSI_DATA_CONFIRMED_CASES_TODAY_PREF";
        public static readonly string SSI_DATA_CONFIRMED_CASES_TOTAL_PREF = "SSI_DATA_CONFIRMED_CASES_TOTAL_PREF";
        public static readonly string SSI_DATA_DEATHS_TODAY_PREF = "SSI_DATA_DEATHS_TODAY_PREF";
        public static readonly string SSI_DATA_DEATHS_TOTAL_PREF = "SSI_DATA_DEATHS_TOTAL_PREF";
        public static readonly string SSI_DATA_TESTS_CONDUCTED_TODAY_PREF = "SSI_DATA_TESTS_CONDUCTED_TODAY_PREF";
        public static readonly string SSI_DATA_TESTS_CONDUCTED_TOTAL_PREF = "SSI_DATA_TESTS_CONDUCTED_TOTAL_PREF";
        public static readonly string SSI_DATA_PATIENTS_ADMITTED_TODAY_PREF = "SSI_DATA_PATIENTS_ADMITTED_TODAY_PREF";
        public static readonly string SSI_DATA_VACCINATED_ENTRY_DATE_PREF = "SSI_DATA_VACCINATED_ENTRY_DATE_PREF";
        public static readonly string SSI_DATA_VACCINATED_FIRST_PREF = "SSI_DATA_VACCINATED_FIRST_PREF";
        public static readonly string SSI_DATA_VACCINATED_SECOND_PREF = "SSI_DATA_VACCINATED_SECOND_PREF";

        public static readonly string APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_LAST_7_DAYS_PREF =
            "APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_LAST_7_DAYS_PREF";

        public static readonly string APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_TOTAL_PREF =
            "APP_DATA_NUMBER_OF_POSITIVE_TESTS_RESULTS_TOTAL_PREF";

        public static readonly string APP_DATA_SMITTESTOP_DOWNLOADS_TOTAL_PREF =
            "APP_DATA_SMITTESTOP_DOWNLOADS_TOTAL_PREF";

        public static readonly string APP_DOWNLOAD_NUMBERS_LAST_UPDATED_PREF = "APP_DOWNLOAD_NUMBERS_LAST_UPDATED_PREF";

        public static readonly string IS_SCROLL_DOWN_SHOWN_PREF = "SCROLL_DOWN_SHOWN_PREF";

        public static readonly string IS_ONBOARDING_COMPLETED_PREF = "isOnboardingCompleted";
        public static readonly string IS_ONBOARDING_COUNTRIES_COMPLETED_PREF = "isOnboardingCountriesCompleted";

        public static readonly string USE_MOBILE_DATA_PREF = "USE_MOBILE_DATA_PREF";

        // The difference between LAST_DOWNLOAD_ZIPS_* and LAST_PULLED_KEYS_SUCCEEDED_DATE_TIME is that the first one is the date of the last try and
        // the second one is the last day when it succeeded
        public static readonly string LAST_PULL_KEYS_SUCCEEDED_DATE_TIME = "LAST_PULL_KEYS_SUCCEEDED_DATE_TIME";
        public static readonly string LAST_DIAGNOSIS_KEY_DATA_MAPPING_DATE_TIME = "LAST_DIAGNOSIS_KEY_DATA_MAPPING_DATE_TIME";
        public static readonly string LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED = "LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED";
        public static readonly string LAST_PULLED_BATCH_NUMBER_SUBMITTED = "LAST_PULLED_BATCH_NUMBER_SUBMITTED";
        public static readonly string LAST_PULLED_BATCH_TYPE = "LAST_PULLED_BATCH_TYPE";

        public static readonly string APP_LANGUAGE = "APP_LANGUAGE";

        public static readonly string DEV_TOOLS_PULL_KEYS_HISTORY = "DEV_TOOLS_PULL_KEYS_HISTORY";

        public static readonly string DEV_TOOLS_PULL_KEYS_HISTORY_LAST_RECORD =
            "DEV_TOOLS_PULL_KEYS_HISTORY_LAST_RECORD";

        public static readonly string TERMS_NOTIFICATION_WAS_SENT = "TERMS_NOTIFICATION_WAS_SENT";

        public static readonly string LAST_PERMISSIONS_NOTIFICATION_DATE_TIME =
            "LAST_PERMISSIONS_NOTIFICATION_DATE_TIME";
        
        public static readonly string CORRELATION_ID = "CORRELATION_ID";

        public static readonly string LAST_NTP_UTC_DATE_TIME = "LAST_NTP_DATE_TIME";

        public static readonly string FETCHING_ACROSS_DATES_204_FIRST_BATCH = "FETCHING_ACROSS_DATES_204_FIRST_BATCH";

        //Deprecated. Only used in migrations:
        [Obsolete] public static readonly string LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF = "LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF";

        [Obsolete] public static readonly string LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF =
            "LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF";

        [Obsolete] public static readonly string CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF =
            "CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF";

        [Obsolete] public static readonly string CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF =
            "CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF";

        [Obsolete] public static readonly string CURRENT_DOWNLOAD_DAY_BATCH_PREF = "CURRENT_DOWNLOAD_DAY_BATCH_PREF";
        
        // EN API v1 configuration parameters
        public static readonly string EXPOSURE_TIME_THRESHOLD = "EXPOSURE_TIME_THRESHOLD";
        public static readonly string LOW_ATTENUATION_DURATION_MULTIPLIER = "LOW_ATTENUATION_DURATION_MULTIPLIER";
        public static readonly string MIDDLE_ATTENUATION_DURATION_MULTIPLIER = "MIDDLE_ATTENUATION_DURATION_MULTIPLIER";
        public static readonly string HIGH_ATTENUATION_DURATION_MULTIPLIER = "HIGH_ATTENUATION_DURATION_MULTIPLIER";

        // EN API v2 configuration parameters
        public static readonly string SCORE_SUM_THRESHOLD = "SCORE_SUM_THRESHOLD";
    }
}