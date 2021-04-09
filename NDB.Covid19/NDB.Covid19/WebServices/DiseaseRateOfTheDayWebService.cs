using System;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;

namespace NDB.Covid19.WebServices
{
    public class DiseaseRateOfTheDayWebService : BaseWebService
    {
        public async Task<DiseaseRateOfTheDayDTO> GetSSIData(DateTime packageDate)
        {
            ApiResponse<DiseaseRateOfTheDayDTO> response =
                await Get<DiseaseRateOfTheDayDTO>(
                    $"{Conf.URL_GET_SSI_DATA}?packageDate={packageDate.ToString("dd'-'MM'-'yyyy")}");
            HandleErrorsSilently(response);
            return response?.Data;
        }

        public async Task<DiseaseRateOfTheDayDTO> GetSSIData()
        {
            ApiResponse<DiseaseRateOfTheDayDTO>
                response = await Get<DiseaseRateOfTheDayDTO>($"{Conf.URL_GET_SSI_DATA}");
            HandleErrorsSilently(response);
            return response?.Data;
        }
    }
}