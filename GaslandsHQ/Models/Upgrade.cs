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

        public string ruleset { get; set; }

        //public int Hull { get; set; }

        //public int Crew { get; set; }

        //public int Handling { get; set; }

        //public int Gear { get; set; }

        public string optionText => utype + (ruleset != "BASE" ? ("(" + ruleset + ")") : string.Empty);
    }
}

