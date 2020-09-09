using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsHQ.Models;
using PropertyChanged;

namespace GaslandsHQ.ViewModels2
{
    public class AddWeaponViewModel : BaseViewModel
    {
        public string Title => "Weapon";

        private ManageVehicleViewModel vehicle;

        public List<Weapon> Weapons { get; private set; }

        public Weapon SelectedWeapon { get; set; }

        public bool CanSelect { get; }

        public string Facing { get; set; } = "Front";

        public List<string> Facings
        {
            get
            {
                var type = this.SelectedWeapon?.wtype;
                if (this.SelectedWeapon?.crewFired == true || type == "Thumper" || type == "Wall Of Amplifiers" || type == "Handgun")
                    return new List<string> { "360" };
                else if (this.SelectedWeapon?.attackType == "Dropped")
                {
                    if (this.vehicle.Upgrades.Any(x => x.SelectedUpgrade?.utype == "Improvised Sludge Thrower"))
                        return new List<string> { "360" };
                    else
                        return new List<string> { "Rear", "Sides" };
                }
                else if (type == "BFG")
                    return new List<string> { "Front" };
                else if (this.SelectedWeapon?.attackType == "Shooting")
                    return new List<string> { "Front", "Rear", "Sides", "Turret/360" };
                else // smash
                    return new List<string> { "Front", "Rear", "Sides" };
            }

        }

        public List<string> Locations => new List<string> { "Cab", "Trailer" };

        public bool ShowLocation { get; set; }

        public string Location { get; set; }

        public bool CanEditFacing
        {
            get
            {
                if (this.SelectedWeapon?.wtype == "BFG")
                    return false;

                return CanSelect && SelectedWeapon?.crewFired == false;
            }
        }

        public int Cost
        {
            get
            {
                if (SelectedWeapon == null) return 0;

                int multiplier = 1;

                var cost = SelectedWeapon.cost;

                if (this.Facing == "Turret/360" && (!this.vehicle.SelectedVehicleType.Keywords.Any(k => k == "Turret")
                    || this.FindMostExpensiveTurret() != this))
                    multiplier = 3;

                return SelectedWeapon.cost * multiplier;
            }
        }

        public string Range {
            get
            {
                if (this.AttackType == "Dropped" && this.vehicle.Upgrades.Any(x => x.SelectedUpgrade?.utype == "Improvised Sludge Thrower"))
                    return "Medium/" + SelectedWeapon?.range;
                else
                    return SelectedWeapon?.range;

            }
        }

        public string Attack
        {
            get
            {
                if (SelectedWeapon?.wtype == "Mortar" && this.vehicle.Upgrades.Any(x => x.SelectedUpgrade?.utype == "Cluster Bombs"))
                    return SelectedWeapon?.attack + "/2D6";
                else
                    return SelectedWeapon?.attack;
            }
        }

        public string AttackType => SelectedWeapon?.attackType;

        public int Ammo
        {
            get
            {
                if (vehicle.Team.SponsorMode && vehicle.Team.SelectedSponsor.name == "Rutherford"
                    && SelectedWeapon?.ammo == 3)
                    return 4;

                return SelectedWeapon?.ammo ?? 0;
            }
        }

        public int Slots
        {
            get
            {
                if(this.vehicle.SelectedVehicleType?.Keywords?.Contains("Bombs Away") ==  true &&  this.AttackType ==  "Dropped")
                    return 0;

                if (this.SelectedWeapon?.wtype == "Ram" && this.vehicle.Team.SelectedSponsor?.keywords?.Contains("Spiked Fist") == true)
                    return 0;

                return SelectedWeapon?.slots ?? 0;
            }
        }

        public string Rules => SelectedWeapon?.specialRules;

        public string DisplayText
        {
            get
            {
                if (string.IsNullOrEmpty(Location))
                    return $"{SelectedWeapon?.wtype} - {Facing}";
                else
                    return $"{SelectedWeapon?.wtype} - {Facing} - {Location}";

            }
        }

        public AddWeaponViewModel(ManageVehicleViewModel vehicle, Weapon defaultWeapon = null)
        {
            this.vehicle = vehicle;

            vehicle.PropertyChanged += Vehicle_PropertyChanged;

            CanSelect = defaultWeapon != null && defaultWeapon.always ? false : true;

            this.RefreshOptions(defaultWeapon);
        }

        public AddWeaponViewModel(ManageVehicleViewModel vehicle, UserWeapon userWeapon)
            : this(vehicle, userWeapon?.Weapon)
        {
            this.Facing = userWeapon?.Facing;
            this.Location = userWeapon?.Location;
        }

        private void Vehicle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(ManageVehicleViewModel.SelectedVehicleType))
            //{
            //    this.RefreshOptions(this.SelectedWeapon);
            //}

            if (e.PropertyName == nameof(ManageVehicleViewModel.Trailers))
            {
                this.RefreshLocation();
            }
        }

        void RefreshOptions(Weapon defaultWeapon)
        {
            var currentWeaponType = this.SelectedWeapon?.wtype;

            this.Weapons = Constants.AllWeapons.Where(w =>
            {
                var matchingTypes = vehicle.Weapons.Where(x => x.SelectedWeapon?.wtype == w.wtype);

                if (w.limit.HasValue && w.limit >= matchingTypes.Count())
                    return false;

                // light vehicles can't take exploding rams
                if (vehicle.SelectedVehicleType?.weight == "Light" && w.wtype == "Exploding Ram")
                    return false;

                if (vehicle.Team.SponsorMode)
                {
                    if (w.allowedSponsors == null || w.allowedSponsors.Length == 0)
                        return true;

                    return w.allowedSponsors.Contains(vehicle.Team.SelectedSponsor.name);
                }

                return true;
            }).ToList();

            if (defaultWeapon != null)
                this.SelectedWeapon = this.Weapons.FirstOrDefault(x => x.wtype == defaultWeapon.wtype);
            else
                this.SelectedWeapon = this.Weapons.FirstOrDefault(x => x.wtype == "Heavy Machine Gun");


            this.RefreshLocation();
        }

        void RefreshLocation()
        {
            this.ShowLocation = this.vehicle.Trailers.Count > 0 && this.SelectedWeapon?.wtype != "Handgun";
            this.Location = ShowLocation ? this.Locations[0] : null;
        }

        void OnSelectedWeaponChanged()
        {
            if (this.Facings?.Count == 1)
                this.Facing = this.Facings.First();
        }

        AddWeaponViewModel FindMostExpensiveTurret()
        {
            var turrets = this.vehicle.Weapons.Where(x => x.Facing == "Turret/360");

            if (turrets.Any())
            {
                return turrets.OrderByDescending(x => x.SelectedWeapon.cost).FirstOrDefault();
            }

            return null;
        }
    }
}
