using System;
namespace GaslandsHQ.Models
{
    public class Weapon : BaseCostModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Attack { get; set; }

        public string Range { get; set; }

        public int Ammo { get; set; }

        public int Slots { get; set; }

        public bool CrewFired { get; set; }

        public string SpecialRules { get; set; }    
    }
}
