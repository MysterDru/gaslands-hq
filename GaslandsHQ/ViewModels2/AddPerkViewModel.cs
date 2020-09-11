using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsHQ.Models;

namespace GaslandsHQ.ViewModels2
{
    public class AddPerkViewModel : BaseViewModel
    {
        public string Title => "Perk";

        private ManageVehicleViewModel vehicle;

        public List<Perk> Perks { get; }

        public Perk SelectedPerk { get; set; }

        public int Cost => SelectedPerk?.cost ?? 0;

        public string Rules => SelectedPerk?.rules;

        public string ShortRules => SelectedPerk?.shortRules ?? Rules;

        public int Handling => this.SelectedPerk?.ptype == "Expertise" ? 1 : 0;

        public AddPerkViewModel(ManageVehicleViewModel vehicle, Perk defaultPerk = null)
        {
            this.vehicle = vehicle;


            this.Perks = Constants.AllPerks.Where(p =>
            {
                if (vehicle.Team.SelectedSponsor == null)
                    return false;

                return vehicle.Team.SelectedSponsor.perkClasses.Contains(p.@class);
            }).ToList();

            if (defaultPerk != null)
                this.SelectedPerk = defaultPerk;
        }
    }
}
