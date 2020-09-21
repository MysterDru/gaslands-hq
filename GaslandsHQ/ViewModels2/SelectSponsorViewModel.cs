using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class SelectSponsorViewModel : BaseViewModel
    {
        public AddTeamViewModel Team { get; }

        public string Title { get; }

        public List<Sponsor> Sponsors { get; }

        public Sponsor SelectedSponsor { get; set; }

        [DependsOn(nameof(SelectedSponsor))]
        public List<KeywordData> Keywords
        {
            get
            {
                if (SelectedSponsor != null && SelectedSponsor.keywords != null && SelectedSponsor.keywords.Length > 0)
                {
                    return Constants.AllKeywords.Where(x => SelectedSponsor.keywords.Contains(x.ktype))
                        .ToList();
                }

                return new List<KeywordData>();
            }
        }

        public bool CanSelect => this.Team.Vehicles.Count == 0;

        public ICommand Save => new Command(OnSaveAsync);

        public SelectSponsorViewModel(AddTeamViewModel team, Sponsor sponsor = null)
        {
            this.Team = team;

            this.Title = "Sponsor";
            this.Sponsors = Constants.AllSponsors;

            this.SelectedSponsor = Sponsors.FirstOrDefault(x => x.name == sponsor?.name) ?? this.Sponsors.First(x => x.name == "None");
        }

        async void OnSaveAsync(object arg)
        {
            MessagingCenter.Send(this, "SPONSORSAVED");
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }
    }
}
