using System;
using Newtonsoft.Json;

namespace GaslandsHQ.Models
{
    public class Perk
    {
        [JsonProperty("class")]
        public string @class {  get;set;}

        public string ptype { get; set; }

        public int  cost { get; set; }

        public string rules { get; set; }

        public string shortRules { get; set; }

        public string ruleset { get; set; }

        public override string ToString() => ptype;
    }
}
