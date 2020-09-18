using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public bool CanSelectSponsor => SelectedSponsor == null || this.Vehicles.Count == 0;

        public int TotalCans { get; set; }

        // todo: build summation logic
        public int CurrentCans => this.Vehicles?.Sum(x => x.TotalCost) ?? 0;

        public ICommand AddVehicle => new Command(ExecuteAddVehicleAsync, (v) => SelectedSponsor != null);

        public ICommand EditVehicle => new Command(ExecuteEditVehicleAsync);

        public ICommand DeleteVehicle => new Command(ExecuteDeleteVehicleAsync);

        public ObservableCollection<AddVehicleViewModel> Vehicles { get; }

        public string VehiclesDisplayText
        {
            get
            {
                var count = Vehicles?.Count ?? 0;
                var st =  count != 1 ? "Vehicles: " : "Vehicle: ";
                if (Vehicles != null)
                {
                    st += string.Join(", ", this.Vehicles.Select(x => $"{x.Name} ({x.SelectedVehicleType?.vtype})"));
                }

                return st;
            }
        }

        public ICommand Save => new Command(ExecuteSaveAsync);

        public ICommand Delete => new Command(ExecuteDeleteAsync);

        public ICommand Dismiss => new Command(ExecuteDismissAsync);

        public AddTeamViewModel()
        {
            this.Id = Guid.NewGuid();

            this.TeamName = "New Team";

            this.Sponsors = Constants.AllSponsors;
            this.Vehicles = new ObservableCollection<AddVehicleViewModel>();

            this.TotalCans = 50;

            MessagingCenter.Subscribe<AddVehicleViewModel>(this, "VEHICLESAVED", OnVehicleSaved);
            MessagingCenter.Subscribe<AddVehicleViewModel>(this, "VEHICLEDELETED", (i) => this.ExecuteDeleteVehicleAsync(i));
        }

        public AddTeamViewModel(UserTeam userTeamToRestore) : this()
        {
            this.Id = userTeamToRestore.Id;
            this.TeamName = userTeamToRestore.TeamName;
            this.TotalCans = userTeamToRestore.Cans;
            this.SelectedSponsor = this.Sponsors.FirstOrDefault(x => x.name == userTeamToRestore.Sponsor?.name);

            foreach (var v in userTeamToRestore.Vehicles)
            {
                var vm = new AddVehicleViewModel(this, v);

                this.Vehicles.Add(vm);
            }
        }

        async void ExecuteAddVehicleAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new AddVehicleViewModel(this);

            await nav.Navigate(vm);
        }

        async void ExecuteEditVehicleAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = obj as AddVehicleViewModel;

            if (vm == null) return;

            await nav.Navigate(vm);
        }

        void ExecuteDeleteVehicleAsync(object obj)
        {
            var vm = obj as AddVehicleViewModel;
            var match = this.Vehicles.FirstOrDefault(x => x.Id == vm.Id);

            if (match != null)
            {
                this.Vehicles.Remove(match);

                this.RaiseAllPropertiesChanged();
            }
        }

        void OnVehicleSaved(AddVehicleViewModel obj)
        {
            var match = this.Vehicles.FirstOrDefault(x => x.Id == obj.Id);

            if (match != null)
            {
                var idx = this.Vehicles.IndexOf(match);

                this.Vehicles.RemoveAt(idx);
                this.Vehicles.Insert(idx, obj);
            }
            else if(obj.Team == this)
                this.Vehicles.Add(obj);

            this.RaiseAllPropertiesChanged();
        }

        void OnSelectedSponsorChanged()
            => (this.AddVehicle as Command).ChangeCanExecute();

        void ExecuteSaveAsync()
        {
            var t = this;
            var team = new UserTeam
            {
                Id  = t.Id,
                TeamName = t.TeamName,
                Cans = t.TotalCans,
                Sponsor = t.SelectedSponsor,
                Vehicles = t.Vehicles.Select(v => new UserVehicle
                {
                    VehicleName = v.Name,
                    VehicleType = v.SelectedVehicleType,
                    Trailers = v.Trailers.Select(tr => tr.GetModel()).ToList(),
                    Weaposn = v.Weapons.Select(x => x.GetModel()).ToList(),
                    Perks = v.Perks.Select(x => x.GetModel()).ToList(),
                    Upgrades = v.Upgrades.Select(x => x.GetModel()).ToList()
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

            MessagingCenter.Send(this, "TEAMSAVED");

            DependencyService.Get<INavigationService>().Dismiss(this);
        }

        async void ExecuteDeleteAsync(object obj)
        {
            var dialogs = DependencyService.Get<IDialogsService>();
            if (await dialogs
                .ConfirmAsync("Are you sure you want to delete this team?", "Delete", "Cancel", true))
            {
                MessagingCenter.Send(this, "TEAMDELETED");
                await DependencyService.Get<INavigationService>().Dismiss(this);
            }
        }

        async void ExecuteDismissAsync(object obj)
        {
            var dialogs = DependencyService.Get<IDialogsService>();
            if (await dialogs
                .ConfirmAsync("Are you sure you want to cancel? You must save this team to keep any changes.", "Close", "Stay Here", true))
            {
                await DependencyService.Get<INavigationService>().Dismiss(this);
            }
        }
    }
}
