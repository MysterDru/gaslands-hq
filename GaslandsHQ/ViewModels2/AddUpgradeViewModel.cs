using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsHQ.Models;

namespace GaslandsHQ.ViewModels2
{
    public class AddUpgradeViewModel : BaseViewModel
    {
        public string Title => "Upgrade";

        private ManageVehicleViewModel vehicle;

        public List<Upgrade> Upgrades { get; }

        public Upgrade SelectedUpgrade { get; set; }

        public bool CanSelect { get; }

        public int Slots
        {
            get
            {
                if (this.SelectedUpgrade?.utype == "Roll Cage" && this.vehicle.SelectedVehicleType.vtype == "Buggy")
                    return 0;

                return this.SelectedUpgrade?.slots ?? 0;
            }
        }

        public int Cost
        {
            get
            {
                if (this.SelectedUpgrade?.utype == "Roll Cage" && this.vehicle.SelectedVehicleType.vtype == "Buggy")
                    return 0;
                else if (this.SelectedUpgrade?.utype == "Nitro Booster" && this.vehicle?.Team?.SelectedSponsor?.name == "Idris")
                    return this.SelectedUpgrade.cost / 2;
                else if (this.SelectedUpgrade?.utype == "Extra Crewmember" && this.vehicle?.Team?.SelectedSponsor?.name == "Scarlett Annie")
                    return this.SelectedUpgrade.cost / 2;
                else
                    return this.SelectedUpgrade?.cost ?? 0;
            }
        }

        public int Ammo => this.SelectedUpgrade?.ammo ?? 0;

        public string Rules => SelectedUpgrade?.specialRules;

        public int Hull => SelectedUpgrade?.hull ?? 0;

        public int Handling => SelectedUpgrade?.handling ?? 0;

        public int Crew => SelectedUpgrade?.crew ?? 0;

        public int MaxGear => SelectedUpgrade?.maxGear ?? 0;

        public string SpecialRules => SelectedUpgrade?.specialRules;

        public AddUpgradeViewModel(ManageVehicleViewModel vehicle, Upgrade defaultUpgrade = null)
        {
            this.vehicle = vehicle;

            if (this.vehicle.SelectedVehicleType.vtype == "Buggy"
                && defaultUpgrade?.utype == "Roll Cage")
                CanSelect = false;
            else
                CanSelect = true;

            this.Upgrades = Constants.AllUpgrades.Where(u =>
            {
                if (!vehicle.Team.SponsorMode)
                    return true;

                bool allowed = u.allowedSponsors == null
                || u.allowedSponsors.Length == 0
                || u.allowedSponsors.Contains(this.vehicle.Team.SelectedSponsor.name);

                if (allowed)
                {
                    allowed = u.allowedVehicles == null
                    || u.allowedVehicles.Length == 0
                    || u.allowedVehicles.Contains(this.vehicle.SelectedVehicleType.vtype);
                }

                return allowed;
            }).ToList();

            if (defaultUpgrade != null)
                this.SelectedUpgrade = defaultUpgrade;
        }
    }
}
