using System;
namespace GaslandsHQ.Models
{
    public class Upgrade : BaseCostModel
    {
        public string Name { get; set; }

        public int Slots { get; set; }

        public int Ammo { get; set; }

        public string Rules { get; set; }

        public int  Hull { get; set; }

        public int Crew { get; set; }

        public int Handling { get; set; }

        public int Gear { get; set; }
    }
}
