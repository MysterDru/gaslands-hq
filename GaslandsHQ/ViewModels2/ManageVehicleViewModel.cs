﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class ManageVehicleViewModel : BaseViewModel
    {
        public string Title => "Vehicle";

        public AddTeamViewModel Team { get; }

        public List<VehicleType> VehicleTypes { get; }

        public VehicleType SelectedVehicleType { get; set; }

        public List<string> AllowedAddons { get; }

        public int SelectedAddonIndex { get; set; }

        public bool ShowWeapons => CanAddAddons && SelectedAddonIndex == 0;
        public bool ShowUpgrades => CanAddAddons && SelectedAddonIndex == 1;
        public bool ShowPerks => CanAddAddons && SelectedAddonIndex == 2;

        public string Name { get; set; }

        public bool CanAddAddons => SelectedVehicleType != null;

        public int TotalCost
        {
            get
            {
                if (SelectedVehicleType == null) return 0;

                return SelectedVehicleType.cost
                     + (this.Weapons?.Sum(x => x.Cost) ?? 0)
                     + (this.Upgrades?.Sum(x => x.Cost) ?? 0)
                     + (this.Perks?.Sum(x => x.Cost) ?? 0);
            }
        }

        public string SpecialRules
        {
            get
            {
                if(this.SelectedVehicleType?.Keywords != null && this.SelectedVehicleType.Keywords.Length > 0)
                {
                    var matchingKeywords = Constants.AllKeywords.Where(x => SelectedVehicleType.Keywords.Contains(x.ktype));

                    var sb = new StringBuilder();
                    foreach(var k in matchingKeywords)
                    {
                        sb.AppendLine($"{k.ktype}: {k.rules}");
                    }

                    return sb.ToString();
                }

                return null;
            }
        }

        public int UsedSlots => (this.Weapons?.Sum(x => x.Slots) ?? 0)
            + (this.Upgrades?.Sum(x => x.Slots) ?? 0);

        public int AvailableSlots => SelectedVehicleType?.slots ?? 0;

        public int Handling => (SelectedVehicleType?.handling ?? 0)
             + (this.Upgrades?.Sum(x => x.Handling) ?? 0);

        public int Hull => (SelectedVehicleType?.hull ?? 0)
            + (this.Upgrades?.Sum(x => x.Hull) ?? 0);// todo: calculate

        public int Crew => (SelectedVehicleType?.crew ?? 0)
             + (this.Upgrades?.Sum(x => x.Crew) ?? 0); // todo: calculate

        public int MaxGear => (SelectedVehicleType?.maxGear ?? 0)
             + (this.Upgrades?.Sum(x => x.MaxGear) ?? 0); // todo: calculate

        public string WeightClass => SelectedVehicleType?.weight ?? "Empty";

        #region Weapons

        public ObservableCollection<AddWeaponViewModel> Weapons { get; }

        public ICommand AddWeapon => new Command(ExecuteAddWeaponAsync);

        public ICommand EditWeapon => new Command(ExecuteEditWeaponAsync);

        public ICommand DeleteWeapon => new Command(ExecuteDeleteWeapon);

        public string WeaponsDisplayText => string.Join(", ", this.Weapons?.Select(x => x.SelectedWeapon?.wtype)?.ToArray() ?? new string[0]);

        #endregion

        #region Upgrade

        public ObservableCollection<AddUpgradeViewModel> Upgrades { get; }

        public ICommand AddUpgrade => new Command(ExecuteAddUpgradeAsync);

        public ICommand EditUpgrade => new Command(ExecuteEditUpgradeAsync);

        public ICommand DeleteUpgrade => new Command(ExecuteDeleteUpgrade);

        public string UpgradesDisplayText => string.Join(", ", this.Upgrades?.Select(x => x.SelectedUpgrade?.utype)?.ToArray() ?? new string[0]);

        #endregion

        #region Perks

        public ObservableCollection<AddPerkViewModel> Perks { get; }

        public ICommand AddPerk => new Command(ExecuteAddPerkAsync);

        public ICommand EditPerk => new Command(ExecuteEditPerkAsync);

        public ICommand DeletePerk => new Command(ExecuteDeletePerk);

        #endregion

        public ManageVehicleViewModel(AddTeamViewModel team)
        {
            this.Weapons = new ObservableCollection<AddWeaponViewModel>();
            this.Upgrades = new ObservableCollection<AddUpgradeViewModel>();
            this.Perks = new ObservableCollection<AddPerkViewModel>();

            this.Team = team;

            this.Name = $"Vehicle {team.Vehicles.Count + 1}";

            this.VehicleTypes = Constants.AllVehicletypes.Where(v =>
            {
                if (team.SponsorMode == false)
                    return true;
                else if (team.SelectedSponsor.name == "Rutherford")
                    return v.weight != "Light";
                else if (team.SelectedSponsor.name != "Rutherford" && new string[] { "Tank", "Helicopter" }.Contains(v.vtype))
                    return false;
                else if (team.SelectedSponsor.name == "Miyazaki")
                    return v.handling > 2;
                else if (team.SelectedSponsor.name == "Idris")
                    return !new string[] { "Gryrocopter" }.Contains(v.vtype);
                else
                    return !new string[] { "Tank", "Helicopter" }.Contains(v.vtype);
            }).ToList();

            // add default handgun
            this.Weapons.Add(new AddWeaponViewModel(this, Constants.AllWeapons.First(x => x.wtype == "Handgun")));

            AllowedAddons = new List<string>
            {
                "Weapons",
                "Upgrades"
            };
            if (team.SponsorMode)
                AllowedAddons.Add("Perks");
        }

        #region Weapons

        async void ExecuteAddWeaponAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new AddWeaponViewModel(this);

            vm.PropertyChanged += OnWeaponPropertyChanged;

            await nav.Navigate(vm);

            this.Weapons.Add(vm);
        }

        async void ExecuteEditWeaponAsync(object obj)
        {
            var vm = obj as AddWeaponViewModel;

            if (vm != null)
            {
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(vm);
            }
        }

        void ExecuteDeleteWeapon(object obj)
        {
            var vm = obj as AddWeaponViewModel;

            if (vm != null)
            {
                vm.PropertyChanged -= this.OnWeaponPropertyChanged;
                this.Weapons.Remove(vm);
            }
        }

        private void OnWeaponPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaiseAllPropertiesChanged();
        }

        #endregion

        #region Upgrades

        async void ExecuteAddUpgradeAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new AddUpgradeViewModel(this);

            vm.PropertyChanged += OnUpgradePropertyChanged;

            await nav.Navigate(vm);

            this.Upgrades.Add(vm);
        }

        async void ExecuteEditUpgradeAsync(object obj)
        {
            var vm = obj as AddUpgradeViewModel;

            if (vm != null)
            {
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(vm);

                this.RaiseAllPropertiesChanged();
            }
        }

        void ExecuteDeleteUpgrade(object obj)
        {
            var vm = obj as AddUpgradeViewModel;

            if (vm != null)
            {
                vm.PropertyChanged -= this.OnUpgradePropertyChanged;
                this.Upgrades.Remove(vm);

                this.RaiseAllPropertiesChanged();
            }
        }

        private void OnUpgradePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaiseAllPropertiesChanged();
        }

        #endregion

        void OnSelectedVehicleTypeChanged()
        {
            // need  to add or remoe free roll  cage based on being a buggy
            var rollCage = this.Upgrades.FirstOrDefault(x => x.SelectedUpgrade?.utype == "Roll Cage");

            var rcModel = Constants.AllUpgrades.First(x => x.utype == "Roll Cage");

            if (this.SelectedVehicleType?.vtype == "Buggy")
            {
                if (rollCage == null)
                {
                    this.Upgrades.Add(new AddUpgradeViewModel(this, rcModel));
                }
            }

            // if not  buggy, and roll cage is free, remove it
            else if (rollCage != null)
                this.Upgrades.Remove(rollCage);
        }

        #region Perks

        async void ExecuteAddPerkAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new AddPerkViewModel(this);

            vm.PropertyChanged += OnPerkPropertyChanged;

            await nav.Navigate(vm);

            this.Perks.Add(vm);
        }

        async void ExecuteEditPerkAsync(object obj)
        {
            var vm = obj as AddPerkViewModel;

            if (vm != null)
            {
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(vm);
            }
        }

        void ExecuteDeletePerk(object obj)
        {
            var vm = obj as AddPerkViewModel;

            if (vm != null)
            {
                vm.PropertyChanged -= this.OnPerkPropertyChanged;
                this.Perks.Remove(vm);

                this.RaiseAllPropertiesChanged();
            }
        }

        void OnPerkPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaiseAllPropertiesChanged();
        }

        #endregion
    }
}