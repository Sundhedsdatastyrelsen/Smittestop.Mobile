namespace NDB.Covid19.WebServices.ExposureNotification
{
    public enum BatchType
    {
        ALL,
        DK
    }

    public static class BatchTypeExtensions
    {
        public static string ToTypeString(this BatchType type)
        {
            if (type == BatchType.ALL)
            {
                return "all";
            }

            return "dk";
        }

        public static BatchType ToBatchType(this string type)
        {
            if (type == "all")
            {
                return BatchType.ALL;
            }

            return BatchType.DK;
        }
    }
}