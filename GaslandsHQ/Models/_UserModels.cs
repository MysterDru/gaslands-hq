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
        public Guid Id { get; set; }

        public VehicleType VehicleType { get; set; }

        public string VehicleName { get; set; }

        public List<UserTrailer> Trailers { get; set; }

        public List<UserWeapon> Weaposn { get; set; }

        public List<UserPerk> Perks { get; set; }

        public List<UserUpgrade> Upgrades { get; set; }
    }

    public class UserWeapon
    {
        public Guid Id { get; set; }

        public Weapon Weapon { get; set; }

        public string Facing { get; set; }

        public string Location { get; set; }
    }

    public class UserPerk
    {
        public Perk Perk { get; set; }

        public Guid Id { get; set; }
    }

    public class UserUpgrade
    {
        public Upgrade Upgrade { get; set; }

        public Guid Id { get; set; }
    }

    public class UserTrailer
    {
        public Guid Id { get; set; }

        public Trailer Trailer { get; set; }

        public Cargo Cargo { get; set; }
    }
}

    