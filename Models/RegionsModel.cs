using System.Collections.Generic;

namespace PruebaTecnica_Saon.Models
{
    public partial class RegionsModel
    {
        public List<Region> data { get; set; }
    }

    // Sub models as property

    public partial class Region
    {
        public string iso { get; set; }
        public string name { get; set; }
    }
}
