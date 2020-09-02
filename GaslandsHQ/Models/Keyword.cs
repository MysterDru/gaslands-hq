using System;
namespace GaslandsHQ.Models
{
    public class Keyword
    {
        public KeywordData[] vehicle { get; set; }

        public KeywordData[] sponsor { get; set; }
    }

    public class KeywordData
    {
        public string ktype { get; set; }

        public string phase { get; set; }

        public string rules { get; set; }

        public string ruleset { get; set; }
    }
}
