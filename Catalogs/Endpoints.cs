namespace PruebaTecnica_Saon.Catalogs
{
    public class Endpoints
    {
        /// <summary>
        /// Reports by date an country/province (Cities data is available for the USA only).
        /// </summary>
        public const string Reports = "reports";

        /// <summary>
        /// Total data for the entire world for particular date
        /// </summary>
        public const string TotalReport = "reports/total";

        /// <summary>
        /// List of provinces by country ISO code.
        /// </summary>
        public const string Provinces = "provinces";

        /// <summary>
        /// List of region names.
        /// </summary>
        public const string Regions = "regions";
    }
}
