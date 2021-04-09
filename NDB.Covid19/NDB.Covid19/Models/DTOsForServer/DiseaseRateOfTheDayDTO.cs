namespace NDB.Covid19.Models.DTOsForServer
{
    public class DiseaseRateOfTheDayDTO
    {
        public SSIStatisticsDTO SSIStatistics { get; set; }
        public AppStatisticsDTO AppStatistics { get; set; }
        public SSIStatisticsVaccinationDTO SSIStatisticsVaccination { get; set; }
    }
}