using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsHQ.Models;
using PropertyChanged;

namespace GaslandsHQ.ViewModels2
{
    public class AddWeaponViewModel : BaseViewModel
    {
        private ManageVehicleViewModel vehicle;

        public List<Weapon> Weapons { get; }

        public Weapon SelectedWeapon { get; set; }

        public bool CanSelect { get; }

        public string Facing { get; set; } = "Front";

        public int Cost
        {
            get
            {
                if (SelectedWeapon == null) return 0;

                if (Facing == "360")
                    return SelectedWeapon.cost * 3;

                return SelectedWeapon.cost;
            }
        }

        public string Range => SelectedWeapon?.range;

        public string Attack => SelectedWeapon?.attack;

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

        public int Slots => SelectedWeapon?.slots ?? 0;

        public AddWeaponViewModel(ManageVehicleViewModel vehicle, Weapon defaultWeapon = null)
        {
            this.vehicle = vehicle;

            CanSelect = defaultWeapon != null && defaultWeapon.always ? false : true;

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
        }
    }
}
