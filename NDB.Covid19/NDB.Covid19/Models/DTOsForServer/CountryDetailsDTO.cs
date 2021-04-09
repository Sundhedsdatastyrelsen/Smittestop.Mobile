﻿namespace NDB.Covid19.Models.DTOsForServer
{
    public class CountryDetailsDTO
    {
        public string Name_DA { get; set; }
        public string Name_EN { get; set; }
        public string Code { get; set; }

        public string GetName()
        {
            string language = LocalesService.GetLanguage();

            switch (language)
            {
                case "da":
                    return Name_DA;
                case "en":
                    return Name_EN;
                default:
                    return Name_DA;
            }
        }
    }
}