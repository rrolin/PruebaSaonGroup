using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PruebaTecnica_Saon.Models
{
    public partial class ReportModel
    {
        public string locationHeader { get; set; }
        public string location { get; set; }

        [DisplayName("CASES")]
        public int cases { get; set; }

        [DisplayName("DEATHS")]
        public int deaths { get; set; }
    }
}
