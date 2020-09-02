using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsHQ.Models;

namespace GaslandsHQ.ViewModels2
{
    public class Constants
    {
        public static List<Sponsor> AllSponsors { get; }
        public static List<VehicleType> AllVehicletypes { get; }
        public static List<Weapon> AllWeapons { get; }
        public static List<Perk> AllPerks { get; }
        public static List<Upgrade> AllUpgrades{ get; }
        public static List<Trailer> AllTrailers { get; }
        public static List<Cargo> AllCargo { get; }

        static Constants()
        {
            var data = new Core.Data.LookupRepository();

            AllSponsors = data.GetValues<Sponsor>().ToList();
            AllVehicletypes = data.GetValues<VehicleType>().ToList();
            AllWeapons = data.GetValues<Weapon>().ToList();
            AllPerks = data.GetValues<Perk>().ToList();
            AllUpgrades = data.GetValues<Upgrade>().ToList();
            AllTrailers = data.GetValues<Trailer>().ToList();
            AllCargo = data.GetValues<Cargo>().ToList();
        }
    }
}
