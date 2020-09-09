using System;
namespace GaslandsHQ.Models
{
    public class Weapon
    {
        public string wtype { get; set; }

        public string attackType { get; set; }

        public string attack { get; set; }

        public string range { get; set; }

        public int slots { get; set; }

        public int ammo { get; set; }

        public bool crewFired { get; set; }

        public int  cost { get; set; }

        public string[] allowedSponsors { get; set; }

        public string ruleset { get; set; }

        public int? limit { get; set; }

        public string specialRules { get; set; }

        public string optionText => wtype + (ruleset != "BASE" ? ("(" + ruleset + ")") : string.Empty);

        public bool always { get; set; }

        public override string ToString() => wtype;
    }
}
