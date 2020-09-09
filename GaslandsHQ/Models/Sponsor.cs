using System;

namespace GaslandsHQ.Models
{
    public class Sponsor
    {
        public string name { get; set; }

        public string[] perkClasses { get; set; }

        public string[] keywords { get; set; }

        public string ruleset { get; set; }

        public override string ToString() => name;
    }
}
