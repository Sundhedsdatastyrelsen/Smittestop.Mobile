using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class SSIStatisticsVaccinationDTO
    {
        public DateTime EntryDate { get; set; }
        public decimal VaccinationFirst { get; set; }
        public decimal VaccinationSecond { get; set; }
    }
}