using System;

namespace GaslandsHQ.Models
{
    public class VehicleType : BaseCostModel
    {
        public string Type { get; set; }

        public string Weight { get; set; }

        public int Hull { get; set; }

        public int Handling { get; set; }

        public int MaxGear { get; set; }

        public int Slots { get; set; }

        public int Crew { get; set; }

        public string[] Keywords { get; set; }
    }
}
