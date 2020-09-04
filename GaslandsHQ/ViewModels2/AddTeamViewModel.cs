using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class AddTeamViewModel : BaseViewModel
    {
        public string Title => "Team";

        public string TeamName { get; set; }

        public List<Sponsor> Sponsors { get; }

        public bool SponsorMode => this.SelectedSponsor?.name != null && this.SelectedSponsor?.name != "None";

        public Sponsor SelectedSponsor { get; set; }

        public bool CanSelectSponsor => SelectedSponsor == null || this.Vehicles.Count == 0;

        public int TotalCans { get; set; }

        // todo: build summation logic
        public int CurrentCans => this.Vehicles?.Sum(x => x.TotalCost) ?? 0;

        public ICommand AddVehicle => new Command(ExecuteAddVehicleAsync, (v) => SelectedSponsor != null);

        public ICommand EditVehicle => new Command(ExecuteEditVehicleAsync);

        public ICommand DeleteVehicle => new Command(ExecuteDeleteVehicleAsync);

        public ObservableCollection<ManageVehicleViewModel> Vehicles { get; }

        public AddTeamViewModel()
        {
            this.TeamName = "New Team";

            this.Sponsors = Constants.AllSponsors;
            this.Vehicles = new ObservableCollection<ManageVehicleViewModel>();

            this.TotalCans = 50;

            //this.SelectedSponsor = Sponsors.First(x => x.name == "None");
        }

        async void ExecuteAddVehicleAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new ManageVehicleViewModel(this);

            vm.PropertyChanged += OnVehiclePropertyChanged;

            await nav.Navigate(vm);

            this.Vehicles.Add(vm);

            RaisePropertyChanged(nameof(CanSelectSponsor));
        }

        async void ExecuteEditVehicleAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = obj as ManageVehicleViewModel;

            if (vm == null) return;

            await nav.Navigate(vm);
        }

        void ExecuteDeleteVehicleAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = obj as ManageVehicleViewModel;
            this.Vehicles.Remove(vm);
            vm.PropertyChanged -= OnVehiclePropertyChanged;

            RaisePropertyChanged(nameof(CanSelectSponsor));
        }

        private void OnVehiclePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ManageVehicleViewModel.TotalCost))
                this.RaisePropertyChanged(nameof(CurrentCans));
        }

        void OnSelectedSponsorChanged()
            => (this.AddVehicle as Command).ChangeCanExecute();
    }
}
