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
        public string Title => "Gaslands HQ (Beta)";

        public ICommand AddTeam => new Command(ExecuteAddTeamAsync);

        public ObservableCollection<AddTeamViewModel> Teams { get; }

        public ICommand EditTeam => new Command(ExecuteEditTeamAsync);

        public ICommand DeleteTeam => new Command(ExecuteDeleteTeamAsync);

        public ICommand Feedback => new Command(ExecuteFeedbackAsync);

        public ICommand ShowInfo => new Command(ExecuteShowInfoAsync);

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
                        teamList.ForEach(team =>
                        {
                            // fix teams
                            // older update, ids could have been empty when ids were added to all of the objects.
                            if (team.Id == Guid.Empty)
                                team.Id = Guid.NewGuid();

                            team.Vehicles.ForEach(v =>
                            {
                                if (v.Id == Guid.Empty)
                                    v.Id = Guid.NewGuid();

                                v.Weaposn.ForEach(w =>
                                {
                                    if (w.Id == Guid.Empty)
                                        w.Id = Guid.NewGuid();
                                });
                                v.Upgrades.ForEach(u =>
                                {
                                    if (u.Id == Guid.Empty)
                                        u.Id = Guid.NewGuid();
                                });
                                v.Perks.ForEach(p =>
                                {
                                    if (p.Id == Guid.Empty)
                                        p.Id = Guid.NewGuid();
                                });
                                v.Trailers.ForEach(t =>
                                {
                                    if (t.Id == Guid.Empty)
                                        t.Id = Guid.NewGuid();
                                });
                            });

                            this.Teams.Add(new AddTeamViewModel(team));
                        });
                    }
                }
            }

            MessagingCenter.Subscribe<AddTeamViewModel>(this, "TEAMSAVED", OnTeamSaved);
            MessagingCenter.Subscribe<AddTeamViewModel>(this, "TEAMDELETED", (i) => this.ExecuteDeleteTeamAsync(i));
        }

        async void ExecuteAddTeamAsync(object obj)
        {
            var vm = new AddTeamViewModel();

            await DependencyService.Get<INavigationService>().Navigate(vm);
        }

        async void ExecuteEditTeamAsync(object obj)
        {
            var vm = obj as AddTeamViewModel;

            await DependencyService.Get<INavigationService>().Navigate(vm);
        }

        async void ExecuteFeedbackAsync(object obj)
        {
            await Xamarin.Essentials.Email.ComposeAsync(new Xamarin.Essentials.EmailMessage("GaslandsHQ Feedback", null, "gaslandshq@drewfrisk.dev"));
        }

        async void ExecuteShowInfoAsync(object obj)
        {
            await DependencyService.Get<INavigationService>().Navigate<InfoViewModel>();
        }

        void OnTeamSaved(AddTeamViewModel obj)
        {
            var match = this.Teams.FirstOrDefault(x => x.Id == obj.Id);

            if (match != null)
            {
                var idx = this.Teams.IndexOf(match);

                this.Teams.RemoveAt(idx);
                this.Teams.Insert(idx, obj);
            }
            else
                this.Teams.Add(obj);

            this.RaiseAllPropertiesChanged();
        }

        void ExecuteDeleteTeamAsync(object obj)
        {
            var vm = obj as AddTeamViewModel;

            var match = this.Teams.FirstOrDefault(x => x.Id == vm.Id);

            if (match != null)
            {
                this.Teams.Remove(match);

                this.RaiseAllPropertiesChanged();

                List<UserTeam> teamList = null;
                var currentJson = Xamarin.Essentials.Preferences.Get("TEAMDATA", (string)null);
                if (!string.IsNullOrEmpty(currentJson))
                {
                    teamList = JsonConvert.DeserializeObject<List<UserTeam>>(currentJson);
                }
                else
                    teamList = new List<UserTeam>();

                var userTeam = teamList.FirstOrDefault(x => x.Id == vm.Id);
                if (userTeam != null)
                {
                    teamList.Remove(userTeam);
                    var newjson = JsonConvert.SerializeObject(teamList);

                    Xamarin.Essentials.Preferences.Set("TEAMDATA", newjson);
                }
            }
        }
    }
}
