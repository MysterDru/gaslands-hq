using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GaslandsHQ.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class MainViewModel : BaseViewModel
    {
        public string Title => "Gaslands HQ";

        public ICommand AddTeam => new Command(ExecuteAddTeamAsync);

        public ObservableCollection<AddTeamViewModel> Teams { get; }

        public ICommand EditTeam => new Command(ExecuteEditTeamAsync);

        public ICommand DeleteTeam => new Command(ExecuteDeleteTeamAsync);

        public MainViewModel()
        {
            this.Teams = new ObservableCollection<AddTeamViewModel>();

            var json = Xamarin.Essentials.Preferences.Get("TEAMDATA", (string)null);
            if (!string.IsNullOrEmpty(json))
            {
                var teamList = JsonConvert.DeserializeObject<List<UserTeam>>(json);
                if (teamList.Count > 0)
                {
                    this.Teams.Clear();
                    if (teamList != null)
                    {
                        foreach (var team in teamList)
                        {
                            this.Teams.Add(new AddTeamViewModel(team));
                        }
                    }
                }
            }

            MessagingCenter.Subscribe(this, "SAVETEAM", (AddTeamViewModel t) =>
            {
                var exisiting = this.Teams.FirstOrDefault(x => x.Id == t.Id);
                if (exisiting == null)
                {
                    this.Teams.Add(t);
                }
            });
        }

        async void ExecuteAddTeamAsync(object obj)
        {
            var vm = new AddTeamViewModel();
            //Teams.Add(vm);

            await DependencyService.Get<INavigationService>().Navigate(vm);
        }

        private void ExecuteDeleteTeamAsync(object obj)
        {
            var vm = obj as AddTeamViewModel;
            this.Teams.Remove(vm);

            List<UserTeam> teamList = null;
            var currentJson = Xamarin.Essentials.Preferences.Get("TEAMDATA", (string)null);
            if (!string.IsNullOrEmpty(currentJson))
            {
                teamList = JsonConvert.DeserializeObject<List<UserTeam>>(currentJson);
            }
            else
                teamList = new List<UserTeam>();

            var match = teamList.FirstOrDefault(x => x.Id == vm.Id);
            if (match != null)
            {
                teamList.Remove(match);
                var newjson = JsonConvert.SerializeObject(teamList);

                Xamarin.Essentials.Preferences.Set("TEAMDATA", newjson);
            }
        }

        async void ExecuteEditTeamAsync(object obj)
        {
            var vm = obj as AddTeamViewModel;

            await DependencyService.Get<INavigationService>().Navigate(vm);
        }
    }
}
