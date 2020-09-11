using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GaslandsHQ.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class AddTeamViewModel : BaseViewModel
    {
        public Guid Id { get; private set; }

        public string Title => "Team";

        public string TeamName { get; set; }

        public List<Sponsor> Sponsors { get; }

        public bool SponsorMode => this.SelectedSponsor?.name != null && this.SelectedSponsor?.name != "None";

        public Sponsor SelectedSponsor { get; set; }

        public bool HideSponsorRules { get; private set; }

        public IEnumerable<KeywordData> SponsorRules
        {
            get
            {
                if (SelectedSponsor == null) return null;

                var k = Constants.AllKeywords.Where(x => SelectedSponsor.keywords.Contains(x.ktype));

                return k?.ToArray();
            }
        }

        public bool CanSelectSponsor => SelectedSponsor == null || this.Vehicles.Count == 0;

        public int TotalCans { get; set; }

        // todo: build summation logic
        public int CurrentCans => this.Vehicles?.Sum(x => x.TotalCost) ?? 0;

        public ICommand AddVehicle => new Command(ExecuteAddVehicleAsync, (v) => SelectedSponsor != null);

        public ICommand EditVehicle => new Command(ExecuteEditVehicleAsync);

        public ICommand DeleteVehicle => new Command(ExecuteDeleteVehicleAsync);

        public ObservableCollection<ManageVehicleViewModel> Vehicles { get; }

        public ICommand SaveTeam => new Command(ExecuteSaveTeamAsync);

        public ICommand Dismiss => new Command(ExecuteDismissAsync);

        public ICommand ToggleSponsorRules => new Command(() => this.HideSponsorRules = !this.HideSponsorRules);

        public AddTeamViewModel()
        {
            this.Id = Guid.NewGuid();

            this.TeamName = "New Team";

            this.Sponsors = Constants.AllSponsors;
            this.Vehicles = new ObservableCollection<ManageVehicleViewModel>();

            this.TotalCans = 50;

            this.HideSponsorRules = true;
        }

        public AddTeamViewModel(UserTeam userTeamToRestore) : this()
        {
            this.Id = userTeamToRestore.Id;
            this.TeamName = userTeamToRestore.TeamName;
            this.TotalCans = userTeamToRestore.Cans;
            this.SelectedSponsor = this.Sponsors.FirstOrDefault(x => x.name == userTeamToRestore.Sponsor?.name);

            foreach (var v in userTeamToRestore.Vehicles)
            {
                var vm = new ManageVehicleViewModel(this, v);
                vm.PropertyChanged += OnVehiclePropertyChanged;

                this.Vehicles.Add(vm);
            }
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

        void ExecuteSaveTeamAsync()
        {
            var t = this;
            var team = new UserTeam
            {
                Id = t.Id,
                TeamName = t.TeamName,
                Cans = t.TotalCans,
                Sponsor = t.SelectedSponsor,
                Vehicles = t.Vehicles.Select(v => new UserVehicle
                {
                    VehicleName = v.Name,
                    VehicleType = v.SelectedVehicleType,
                    Trailers = v.Trailers.Select(tr => new UserTrailer
                    {
                        Trailer = tr.SelectedTrailer,
                        Cargo = tr.SelectedCargo
                    }).ToList(),
                    Weaposn = v.Weapons.Select(x => new UserWeapon
                    {
                        Weapon = x.SelectedWeapon,
                        Facing = x.Facing,
                        Location = x.Location
                    }).ToList(),
                    Perks = v.Perks.Select(x => x.SelectedPerk).ToList(),
                    Upgrades = v.Upgrades.Select(x => x.SelectedUpgrade).ToList()
                }).ToList()
            };

            List<UserTeam> teamList = null;
            var currentJson = Xamarin.Essentials.Preferences.Get("TEAMDATA", (string)null);
            if (!string.IsNullOrEmpty(currentJson))
            {
                teamList = JsonConvert.DeserializeObject<List<UserTeam>>(currentJson);
            }
            else
                teamList = new List<UserTeam>();

            teamList.Add(team);

            var newjson = JsonConvert.SerializeObject(teamList);

            Xamarin.Essentials.Preferences.Set("TEAMDATA", newjson);

            MessagingCenter.Send(this, "SAVETEAM");

            DependencyService.Get<INavigationService>().Dismiss(this);
        }

        void ExecuteDismissAsync(object obj)
        {
            DependencyService.Get<INavigationService>().Dismiss(this);
        }
    }
}
