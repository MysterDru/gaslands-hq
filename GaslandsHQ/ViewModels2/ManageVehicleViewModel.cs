using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class ManageVehicleViewModel : BaseViewModel
    {
        public AddTeamViewModel Team { get; }

        public List<VehicleType> VehicleTypes { get; }

        public VehicleType SelectedVehicleType { get; set; }

        public string Name { get; set; }

        public bool CanAddAddons => SelectedVehicleType != null;

        public int TotalCost
        {
            get
            {
                if (SelectedVehicleType == null) return 0;

                return SelectedVehicleType.cost
                     + (this.Weapons?.Sum(x => x.Cost) ?? 0);
            }
        }

        public int UsedSlots => this.Weapons?.Sum(x => x.Slots) ?? 0;

        public int AvailableSlots => SelectedVehicleType?.slots ?? 0;

        public int Handling => SelectedVehicleType?.handling ?? 0;

        public int Hull => SelectedVehicleType?.hull ?? 0; // todo: calculate

        public int Crew => SelectedVehicleType?.crew ?? 0; // todo: calculate

        public int MaxGear => SelectedVehicleType?.maxGear ?? 0; // todo: calculate

        public string WeightClass => SelectedVehicleType?.weight ?? "Empty";

        public ObservableCollection<AddWeaponViewModel> Weapons { get; }

        public ICommand AddWeapon => new Command(ExecuteAddWeaponAsync);

        public ICommand EditWeapon => new Command(ExecuteEditWeaponAsync);

        public string WeaponsDisplayText => string.Join(", ", this.Weapons?.Select(x => x.SelectedWeapon?.wtype)?.ToArray() ?? new string[0]);

        public ManageVehicleViewModel(AddTeamViewModel team)
        {
            this.Weapons = new ObservableCollection<AddWeaponViewModel>();

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
        }

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

        private void OnWeaponPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddWeaponViewModel.Cost))
                this.RaisePropertyChanged(nameof(TotalCost));

            if (e.PropertyName == nameof(AddWeaponViewModel.SelectedWeapon))
                this.RaisePropertyChanged(nameof(WeaponsDisplayText));

            if (e.PropertyName == nameof(AddWeaponViewModel.Slots))
                this.RaisePropertyChanged(nameof(UsedSlots));
        }
    }
}
