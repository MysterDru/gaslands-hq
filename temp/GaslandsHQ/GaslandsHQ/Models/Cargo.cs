using System;
namespace GaslandsHQ.Models
{
    public class Cargo
    {
        public string ctype { get; set; }

        public string specialRules { get; set; }

        public string[] keywords { get; set; }

        public string ruleset { get; set; }

        public string optionText => ctype + (ruleset != "BASE" ? ("(" + ruleset + ")") : string.Empty);
    }
}
