using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class QuestionnaireCountriesViewModel
    {
        public static QuestionnaireCountriesVisitedEnum Selection = QuestionnaireCountriesVisitedEnum.No;

        public static string COUNTRY_QUESTIONAIRE_HEADER_TEXT =>
            "REGISTER_COUNTRY_QUESTIONAIRE_HEADER_TEXT".Translate();

        public static string COUNTRY_QUESTIONAIRE_INFORMATION_TEXT =>
            "REGISTER_COUNTRY_QUESTIONAIRE_INFORMATION_TEXT".Translate();

        public static string COUNTRY_QUESTIONAIRE_BUTTON_TEXT =>
            "REGISTER_COUNTRY_QUESTIONAIRE_BUTTON_TEXT".Translate();

        public static string COUNTRY_QUESTIONAIRE_FOOTER =>
            "REGISTER_COUNTRY_QUESTIONAIRE_FOOTER".Translate();

        public static string COUNTRY_QUESTIONAIRE_YES_RADIO_BUTTON =>
            "COUNTRY_QUESTIONAIRE_YES_RADIO_BUTTON".Translate();

        public static string COUNTRY_QUESTIONAIRE_NO_RADIO_BUTTON =>
            "COUNTRY_QUESTIONAIRE_NO_RADIO_BUTTON".Translate();

        public DialogViewModel CloseDialogViewModel => new DialogViewModel
        {
            Title = ErrorViewModel.REGISTER_LEAVE_HEADER,
            Body = ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
            OkBtnTxt = ErrorViewModel.REGISTER_LEAVE_CONFIRM,
            CancelbtnTxt = ErrorViewModel.REGISTER_LEAVE_CANCEL
        };

        /// <summary>
        ///     Calls the server and requests a list of countries to show in the questionnaire
        /// </summary>
        /// <returns>A list of countries, or an empty list if the service call fails</returns>
        public async Task<List<CountryDetailsViewModel>> GetListOfCountriesAsync()
        {
            CountryListDTO countryList = await new CountryListService().GetCountryList();
            List<CountryDetailsViewModel> countries = countryList?
                .CountryCollection?
                .Select(x =>
                    new CountryDetailsViewModel
                    {
                        Name = x.GetName(),
                        Code = x.Code
                    })
                .OrderBy(model => 
                    model.Name,
                    StringComparer.Create(new CultureInfo(LocalesService.GetLanguage()), true))
                .ToList();
            return countries ?? new List<CountryDetailsViewModel>();
        }

        /// <summary>
        ///     Saves the questionnaire answers in memory to be used on the next page.
        /// </summary>
        /// <param name="onSuccess">Platform logic to navigate to the next page</param>
        /// <param name="selectedCountriesList">The list of country view models from the questionnaire</param>
        public void InvokeNextButtonClick(Action onSuccess, Action onFail,
            List<CountryDetailsViewModel> selectedCountriesList)
        {
            if (AuthenticationState.PersonalData != null && AuthenticationState.PersonalData.Validate())
            {
                AuthenticationState.PersonalData.VisitedCountries =
                    selectedCountriesList.Where(x => x.Checked).Select(x => x.Code).ToList();

                onSuccess?.Invoke();
            }
            else
            {
                LogUtils.LogMessage(
                    LogSeverity.INFO,
                    "Questionnaire Countries - no miba data or token expired",
                    null,
                    GetCorrelationId());
                onFail?.Invoke();
            }
        }
    }
}