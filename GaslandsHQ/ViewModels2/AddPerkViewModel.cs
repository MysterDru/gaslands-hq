using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class AddPerkViewModel : BaseViewModel
    {
        public string Title => "Perk";

        public AddVehicleViewModel Vehicle { get; }

        public Guid Id { get; set; }

        public List<Perk> Perks { get; }

        public Perk SelectedPerk { get; set; }

        public int Cost => SelectedPerk?.cost ?? 0;

        public string Rules => SelectedPerk?.rules;

        public string ShortRules => SelectedPerk?.shortRules;

        public int Handling => this.SelectedPerk?.ptype == "Expertise" ? 1 : 0;

        public ICommand Save => new Command(OnSaveAsync);

        public ICommand Delete => new Command(OnDeleteAsync);

        public AddPerkViewModel(AddVehicleViewModel vehicle, Perk defaultPerk = null)
        {
            this.Vehicle = vehicle;

            this.Perks = Constants.AllPerks.Where(p =>
            {
                if (vehicle.Team.SelectedSponsor == null)
                    return false;

                return vehicle.Team.SelectedSponsor.perkClasses.Contains(p.@class);
            }).ToList();

            if (defaultPerk != null)
                this.SelectedPerk = defaultPerk;

            this.Id = Guid.NewGuid();
        }

        public AddPerkViewModel(AddVehicleViewModel vehicle, UserPerk defaultPerk)
            : this(vehicle, defaultPerk?.Perk)
        {
            this.Id = defaultPerk?.Id ?? Guid.NewGuid();
        }

        internal UserPerk GetModel()
        {
            return new UserPerk
            {
                Id = this.Id,
                Perk = this.SelectedPerk
            };
        }

        async void OnSaveAsync(object arg)
        {
            MessagingCenter.Send(this, "PERKSAVED");
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }

        async void OnDeleteAsync(object obj)
        {
            var dialogs = DependencyService.Get<IDialogsService>();
            if (await dialogs
                .ConfirmAsync("Are you sure you want to delete this perk?", "Delete", "Cancel", true))
            {
                MessagingCenter.Send(this, "PERKDELETED");
                await DependencyService.Get<INavigationService>().Dismiss(this);
            }
        }
    }
}
