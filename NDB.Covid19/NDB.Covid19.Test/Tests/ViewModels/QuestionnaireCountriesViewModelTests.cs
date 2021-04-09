using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.OAuth2;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NDB.Covid19.Test.Tests.ViewModels
{
    public class QuestionnaireCountriesViewModelTests : IDisposable
    {
        private static readonly CountryListDTO Countries = new CountryListDTO
        {
            CountryCollection = new List<CountryDetailsDTO>
            {
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Østrig",
                    Name_EN = "Austria"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "A",
                    Name_EN = "Tjekkiet"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Estland",
                    Name_EN = "Estonia"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Tyskland",
                    Name_EN = "Germany"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Irland",
                    Name_EN = "Ireland"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Italien",
                    Name_EN = "Italy"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Letland",
                    Name_EN = "Latvia"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Holland",
                    Name_EN = "Netherlands"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Polen",
                    Name_EN = "Poland"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Spanien",
                    Name_EN = "Spain"
                },
                new CountryDetailsDTO
                {
                    Code = "",
                    Name_DA = "Sverige",
                    Name_EN = "Sweden"
                }
            }
        };

        private readonly QuestionnaireCountriesViewModel _viewModel;

        public QuestionnaireCountriesViewModelTests()
        {
            DependencyInjectionConfig.Init();
            _viewModel = new QuestionnaireCountriesViewModel();
            ApiStubHelper.StartServer();
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    new CountryDetailsViewModel {Checked = true, Code = "dk", Name = "Denmark"}
                },
                new object[]
                {
                    new CountryDetailsViewModel {Checked = true, Code = "dk", Name = "Denmark"},
                    new CountryDetailsViewModel {Checked = true, Code = "en", Name = "England"}
                },
                new object[]
                {
                    new CountryDetailsViewModel {Checked = true, Code = "dk", Name = "Denmark"},
                    new CountryDetailsViewModel {Checked = true, Code = "en", Name = "England"},
                    new CountryDetailsViewModel {Checked = true, Code = "pl", Name = "Poland"}
                },
                new object[]
                {
                    new CountryDetailsViewModel {Checked = true, Code = "dk", Name = "Denmark"},
                    new CountryDetailsViewModel {Checked = true, Code = "en", Name = "England"},
                    new CountryDetailsViewModel {Checked = true, Code = "pl", Name = "Poland"},
                    new CountryDetailsViewModel {Checked = true, Code = "de", Name = "Germany"}
                }
            };

        public void Dispose()
        {
            ApiStubHelper.StopServer();
        }

        [Fact]
        public async void CountryListService_EndpointWorks()
        {
            var testApiPath = "/countries";

            var mockedData = new CountryListDTO
            {
                CountryCollection = new List<CountryDetailsDTO>
                {
                    new CountryDetailsDTO
                    {
                        Name_DA = "Danmark",
                        Name_EN = "Denmark",
                        Code = "dk"
                    }
                }
            };

            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(testApiPath).UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBody(JsonConvert.SerializeObject(mockedData))
                );
            var baseService = new BaseWebService();

            ApiResponse<CountryListDTO> response =
                await baseService.Get<CountryListDTO>(ApiStubHelper.StubServerUrl + testApiPath);
            Assert.True(response.IsSuccessfull);
            Assert.Equal(mockedData.CountryCollection.Count, response.Data.CountryCollection.Count);
            Assert.Equal(mockedData.CountryCollection[0].Code, response.Data.CountryCollection[0].Code);
            Assert.Equal(mockedData.CountryCollection[0].Name_DA, response.Data.CountryCollection[0].Name_DA);
            Assert.Equal(mockedData.CountryCollection[0].Name_EN, response.Data.CountryCollection[0].Name_EN);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void QuestionnaireCountriesViewModel_InvokeNextButtonClick_UpdatesList(
            params CountryDetailsViewModel[] list)
        {
            AuthenticationState.PersonalData = new PersonalDataModel
            {
                Covid19_smitte_start = DateTime.Now.ToString(),
                TokenExpiration = DateTime.Now.AddMinutes(10)
            };

            _viewModel.InvokeNextButtonClick(null, null, list.ToList());

            Assert.NotNull(AuthenticationState.PersonalData.VisitedCountries);
            Assert.True(
                AuthenticationState.PersonalData.VisitedCountries.SequenceEqual(list.Select(c => c.Code).ToList()));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async void QuestionnaireCountriesViewModel_InvokeNextButtonClick_OnSuccessCalled(
            params CountryDetailsViewModel[] list)
        {
            AuthenticationState.PersonalData = new PersonalDataModel
            {
                Covid19_smitte_start = DateTime.Now.ToString(),
                TokenExpiration = DateTime.Now.AddMinutes(10)
            };

            TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

            _viewModel.InvokeNextButtonClick(() => { _tcs.SetResult(true); }, () => { _tcs.SetResult(false); },
                list.ToList());

            Assert.True(await _tcs.Task);
            Assert.NotNull(AuthenticationState.PersonalData.VisitedCountries);
            Assert.True(
                AuthenticationState.PersonalData.VisitedCountries.SequenceEqual(list.Select(c => c.Code).ToList()));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async void QuestionnaireCountriesViewModel_InvokeNextButtonClick_OnFailCalled(
            params CountryDetailsViewModel[] list)
        {
            AuthenticationState.PersonalData = null;

            TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

            _viewModel.InvokeNextButtonClick(() => { _tcs.SetResult(false); }, () => { _tcs.SetResult(true); },
                list.ToList());

            Assert.True(await _tcs.Task);
            Assert.Null(AuthenticationState.PersonalData?.VisitedCountries);
        }

        [Theory]
        [InlineData("da")]
        [InlineData("en")]
        public async void GetListOfCountriesAsync_ShouldBeOrderedCorrectly(string language)
        {
            string url = Conf.URL_GET_COUNTRY_LIST.Replace(ApiStubHelper.StubServerUrl, "");
            LocalPreferencesHelper.SetAppLanguage(language);

            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBody(JsonConvert.SerializeObject(Countries))
                );

            List<CountryDetailsViewModel> countryDetailsViewModels = await _viewModel.GetListOfCountriesAsync();
            Assert.Equal(countryDetailsViewModels.Count, Countries.CountryCollection.Count);
            countryDetailsViewModels.Should().BeInAscendingOrder(model => model.Name);
        }
    }
}