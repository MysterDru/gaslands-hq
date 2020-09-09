using System;
namespace GaslandsHQ.Models
{
    public class Upgrade
    {
        public string utype { get; set; }

        public int slots { get; set; }

        public int ammo { get; set; }

        public string specialRules { get; set; }

        public int cost { get; set; }

        public int? limit { get; set; }

        public string[] allowedSponsors { get; set; }

        public string[] allowedVehicles { get; set; }

        public string ruleset { get; set; }

        public int hull { get; set; }

        public int crew { get; set; }

        public int handling { get; set; }

        public int maxGear { get; set; }

        public string optionText => utype + (ruleset != "BASE" ? ("(" + ruleset + ")") : string.Empty);

        public override string ToString() => utype;
    }
}

