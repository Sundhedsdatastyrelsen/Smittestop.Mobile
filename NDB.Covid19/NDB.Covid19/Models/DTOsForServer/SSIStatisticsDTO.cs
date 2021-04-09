﻿using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class SSIStatisticsDTO
    {
        public DateTime EntryDate { get; set; }
        public int ConfirmedCasesToday { get; set; }
        public int ConfirmedCasesTotal { get; set; }
        public int DeathsToday { get; set; }
        public int DeathsTotal { get; set; }
        public int TestsConductedToday { get; set; }
        public int TestsConductedTotal { get; set; }

        [Obsolete] public int patientsAdmittedToday { get; set; }
    }
}