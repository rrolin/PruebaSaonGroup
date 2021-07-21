using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PruebaTecnica_Saon.Models
{
    public partial class ReportByRegionModel
    {
        public List<RegionReport> data { get; set; }
    }

    // Sub models as property

    public partial class RegionReport
    {
        public string date { get; set; }

        [DisplayName("CASES")]
        public int confirmed { get; set; }

        [DisplayName("DEATHS")]
        public int deaths { get; set; }
        public int recovered { get; set; }
        public int confirmed_diff { get; set; }
        public int deaths_diff { get; set; }
        public int recovered_diff { get; set; }
        public DateTime last_update { get; set; }
        public int active { get; set; }
        public int active_diff { get; set; }
        public double fatality_rate { get; set; }
        public RegionData region { get; set; }
    }

    public partial class RegionData
    {
        public string iso { get; set; }

        [DisplayName("REGION")]
        public string name { get; set; }

        [DisplayName("PROVINCE")]
        public string province { get; set; }
        public decimal? lat { get; set; }

        [JsonProperty("long")]
        public decimal? longitude { get; set; }
    }
}