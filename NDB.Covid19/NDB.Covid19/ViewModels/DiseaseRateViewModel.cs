using System;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class DiseaseRateViewModel
    {
        private static readonly DiseaseRateOfTheDayWebService WebService;

        static DiseaseRateViewModel()
        {
            WebService = new DiseaseRateOfTheDayWebService();
        }

        public static string DISEASE_RATE_HEADER => "DISEASE_RATE_HEADER".Translate();
        public static string DISEASE_RATE_SUBHEADER => "DISEASE_RATE_SUBHEADER".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_NEW => "KEY_FEATURE_ONE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_ALL => "KEY_FEATURE_ONE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_ONE_LABEL => "KEY_FEATURE_ONE_LABEL".Translate();
        public static string KEY_FEATURE_TWO_UPDATE_NEW => "KEY_FEATURE_TWO_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_TWO_UPDATE_ALL => "KEY_FEATURE_TWO_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_TWO_LABEL => "KEY_FEATURE_TWO_LABEL".Translate();
        public static string KEY_FEATURE_THREE_UPDATE_NEW => "KEY_FEATURE_THREE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_THREE_UPDATE_ALL => "KEY_FEATURE_THREE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_THREE_LABEL => "KEY_FEATURE_THREE_LABEL".Translate();
        public static string KEY_FEATURE_FOUR_UPDATE_NEW => "KEY_FEATURE_FOUR_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_FOUR_LABEL => "KEY_FEATURE_FOUR_LABEL".Translate();

        public static string KEY_FEATURE_FOUR_FIRST_VACCINATION_LABEL =>
            "KEY_FEATURE_FOUR_FIRST_VACCINATION_LABEL".Translate();

        public static string KEY_FEATURE_FOUR_SECOND_VACCINATION_LABEL =>
            "KEY_FEATURE_FOUR_SECOND_VACCINATION_LABEL".Translate();

        public static string KEY_FEATURE_FOUR_FIRST_VACCINATION_NUMBER =>
            "KEY_FEATURE_FOUR_FIRST_VACCINATION_NUMBER".Translate();

        public static string KEY_FEATURE_FOUR_SECOND_VACCINATION_NUMBER =>
            "KEY_FEATURE_FOUR_SECOND_VACCINATION_NUMBER".Translate();

        public static string KEY_FEATURE_FIVE_UPDATE_NEW => "KEY_FEATURE_FIVE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_FIVE_UPDATE_ALL => "KEY_FEATURE_FIVE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_FIVE_LABEL => "KEY_FEATURE_FIVE_LABEL".Translate();
        public static string KEY_FEATURE_SIX_UPDATE_ALL => "KEY_FEATURE_SIX_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_SIX_LABEL => "KEY_FEATURE_SIX_LABEL".Translate();
        public static string DISEASE_RATE_SUBSUBHEADER => "DISEASE_RATE_SUBSUBHEADER".Translate();

        public static DateTime LastUpdateSSINumbersDateTime => SSILastUpdateDateTime.ToLocalTime();
        public static DateTime LastUpdateDownloadsNumbersDateTime => APPDownloadNumberLastUpdateDateTime.ToLocalTime();

        public static string LastUpdateStringSubHeader =>
            LastUpdateSSINumbersDateTime != DateTime.MinValue.ToLocalTime()
                ? string.Format(DISEASE_RATE_SUBHEADER,
                    $"{DateUtils.GetDateFromDateTime(LastUpdateSSINumbersDateTime, "m")}",
                    $"{DateUtils.GetDateFromDateTime(LastUpdateSSINumbersDateTime, "t")}")
                : "";

        public static string LastUpdateStringSubSubHeader =>
            LastUpdateDownloadsNumbersDateTime != DateTime.MinValue.ToLocalTime()
                ? string.Format(DISEASE_RATE_SUBSUBHEADER,
                    $"{DateUtils.GetDateFromDateTime(LastUpdateDownloadsNumbersDateTime, "m")}",
                    $"{DateUtils.GetDateFromDateTime(LastUpdateDownloadsNumbersDateTime, "t")}")
                : "";

        public static string ConfirmedCasesToday => string.Format(KEY_FEATURE_ONE_UPDATE_NEW,
            $"{DiseaseRateOfTheDay.SSIConfirmedCasesToday:N0}");

        public static string ConfirmedCasesTotal => string.Format(KEY_FEATURE_ONE_UPDATE_ALL,
            $"{DiseaseRateOfTheDay.SSIConfirmedCasesTotal:N0}");

        public static string DeathsToday =>
            string.Format(KEY_FEATURE_TWO_UPDATE_NEW, $"{DiseaseRateOfTheDay.SSIDeathsToday:N0}");

        public static string DeathsTotal =>
            string.Format(KEY_FEATURE_TWO_UPDATE_ALL, $"{DiseaseRateOfTheDay.SSIDeathsTotal:N0}");

        public static string TestsConductedToday => string.Format(KEY_FEATURE_THREE_UPDATE_NEW,
            $"{DiseaseRateOfTheDay.SSITestsConductedToday:N0}");

        public static string TestsConductedTotal => string.Format(KEY_FEATURE_THREE_UPDATE_ALL,
            $"{DiseaseRateOfTheDay.SSITestsConductedTotal:N0}");

        public static string VaccinatedFirst => string.Format(KEY_FEATURE_FOUR_FIRST_VACCINATION_NUMBER,
            $"{DiseaseRateOfTheDay.SSIVaccinatedFirst:N1}");

        public static string VaccinatedSecond => string.Format(KEY_FEATURE_FOUR_SECOND_VACCINATION_NUMBER,
            $"{DiseaseRateOfTheDay.SSIVaccinatedSecond:N1}");

        public static string NumberOfPositiveTestsResultsLast7Days => string.Format(KEY_FEATURE_FIVE_UPDATE_NEW,
            $"{DiseaseRateOfTheDay.APPNumberOfPositiveTestsResultsLast7Days:N0}");

        public static string NumberOfPositiveTestsResultsTotal => string.Format(KEY_FEATURE_FIVE_UPDATE_ALL,
            $"{DiseaseRateOfTheDay.APPNumberOfPositiveTestsResultsTotal:N0}");

        public static string SmittestopDownloadsTotal => string.Format(KEY_FEATURE_SIX_UPDATE_ALL,
            $"{DiseaseRateOfTheDay.APPSmittestopDownloadsTotal:N0}");

        public static async Task<bool> UpdateSSIDataAsync()
        {
            try
            {
                DiseaseRateOfTheDayDTO ssiData = await (WebService ?? new DiseaseRateOfTheDayWebService()).GetSSIData();
                if (ssiData?.SSIStatistics == null || ssiData.AppStatistics == null ||
                    ssiData.SSIStatisticsVaccination == null)
                {
                    return false;
                }

                DiseaseRateOfTheDay.UpdateAll(ssiData);
                MessagingCenter.Send(new object(), MessagingCenterKeys.KEY_UPDATE_DISEASE_RATE);
                return true;
            }
            catch (NullReferenceException e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e,
                    $"{nameof(DiseaseRateViewModel)}.{nameof(UpdateSSIDataAsync)}: Failed to fetch the data.");
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e,
                    $"{nameof(DiseaseRateViewModel)}.{nameof(UpdateSSIDataAsync)}: Unidentified exception.");
            }

            return false;
        }
    }
}