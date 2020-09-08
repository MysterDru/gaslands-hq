using System;
using System.Collections.Generic;

namespace GaslandsHQ.Models
{
    public class UserTeam
    {
        public Guid  Id { get; set; }

        public string TeamName { get; set; }

        public Sponsor Sponsor { get; set; }

        public int Cans { get; set; }

        public List<UserVehicle> Vehicles { get; set; }
    }

    public class UserVehicle
    {
        public VehicleType VehicleType { get; set; }

        public string VehicleName { get; set; }

        public List<UserTrailer> Trailers { get; set; }

        public List<UserWeapon> Weaposn { get; set; }

        public List<Perk> Perks { get; set; }

        public List<Upgrade> Upgrades { get; set; }
    }

    public class UserWeapon
    {
        public Weapon Weapon { get; set; }

        public string Facing { get; set; }

        public string Location { get; set; }
    }

    public class UserTrailer
    {
        public Trailer Trailer { get; set; }

        public Cargo Cargo { get; set; }
    }
}

    