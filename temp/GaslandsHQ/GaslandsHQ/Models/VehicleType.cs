using System;

namespace GaslandsHQ.Models
{
    public class VehicleType
    {
        public string vtype { get; set; }

        public string weight { get; set; }

        public int hull { get; set; }

        public int handling { get; set; }

        public int maxGear { get; set; }

        public int slots { get; set; }

        public int crew { get; set; }

        public int cost { get; set; }

        public string[] Keywords { get; set; }

        public string ruleset { get; set; }
    }
}
