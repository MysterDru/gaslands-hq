using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class SelectUpgradeViewModel : BaseViewModel
    {
        public string Title => "Upgrade";

        public AddVehicleViewModel Vehicle { get; }

        public Guid Id { get; set; }

        public List<Upgrade> Upgrades { get; }

        public Upgrade SelectedUpgrade { get; set; }

        public bool CanSelect { get; }

        public int Slots
        {
            get
            {
                if (this.SelectedUpgrade?.utype == "Roll Cage" && this.Vehicle.SelectedVehicleType.vtype == "Buggy")
                    return 0;

                return this.SelectedUpgrade?.slots ?? 0;
            }
        }

        public int Cost
        {
            get
            {
                if (this.SelectedUpgrade?.utype == "Roll Cage" && this.Vehicle.SelectedVehicleType.vtype == "Buggy")
                    return 0;
                else if (this.SelectedUpgrade?.utype == "Nitro Booster" && this.Vehicle?.Team?.SelectedSponsor?.name == "Idris")
                    return this.SelectedUpgrade.cost / 2;
                else if (this.SelectedUpgrade?.utype == "Extra Crewmember" && this.Vehicle?.Team?.SelectedSponsor?.name == "Scarlett Annie")
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

        public ICommand Save => new Command(OnSaveAsync);

        public ICommand Delete => new Command(OnDeleteAsync);

        public SelectUpgradeViewModel(AddVehicleViewModel vehicle, Upgrade defaultUpgrade = null)
        {
            this.Vehicle = vehicle;

            if (this.Vehicle.SelectedVehicleType.vtype == "Buggy"
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
                || u.allowedSponsors.Contains(this.Vehicle.Team.SelectedSponsor.name);

                if (allowed)
                {
                    allowed = u.allowedVehicles == null
                    || u.allowedVehicles.Length == 0
                    || u.allowedVehicles.Contains(this.Vehicle.SelectedVehicleType.vtype);
                }

                // allow up to double of the original crew amount
                if(u.utype == "Extra Crewmember")
                {
                    var crew = vehicle.SelectedVehicleType?.crew ?? 0;
                    var extraCrewCount = vehicle.Upgrades.Where(x => x.SelectedUpgrade?.utype == "Extra Crewmember").Count();

                    if (extraCrewCount < crew)
                        return true;
                }

                return allowed;
            }).ToList();

            this.SelectedUpgrade = this.Upgrades.FirstOrDefault(x => x.utype == defaultUpgrade?.utype) ?? this.Upgrades.FirstOrDefault();

            this.Id = Guid.NewGuid();
        }

        public SelectUpgradeViewModel(AddVehicleViewModel vehicle, UserUpgrade defaultUpgrade) : this(vehicle, defaultUpgrade?.Upgrade)
        {
            this.Id = defaultUpgrade?.Id ?? Guid.NewGuid();
        }

        internal UserUpgrade GetModel()
        {
            return new UserUpgrade
            {
                Upgrade = this.SelectedUpgrade,
                Id = this.Id
            };
        }

        async void OnSaveAsync(object arg)
        {
            MessagingCenter.Send(this, "UPGRADESAVED");
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }

        async void OnDeleteAsync(object obj)
        {
            var dialogs = DependencyService.Get<IDialogsService>();
            if (await dialogs
                .ConfirmAsync("Are you sure you want to delete this upgrade?", "Delete", "Cancel", true))
            {
                MessagingCenter.Send(this, "UPGRADEDELETED");
                await DependencyService.Get<INavigationService>().Dismiss(this);
            }
        }
    }
}
