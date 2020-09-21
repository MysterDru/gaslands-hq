using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class SelectVehicleViewModel : BaseViewModel
    {
        public AddVehicleViewModel Vehicle { get; }

        public string Title { get; }

        public List<VehicleType> VehicleTypes { get; }

        public VehicleType SelectedVehicleType { get; set; }

        [DependsOn(nameof(SelectedVehicleType))]
        public List<KeywordData> Keywords
        {
            get
            {
                if (SelectedVehicleType != null)
                {
                    if (SelectedVehicleType.Keywords != null && SelectedVehicleType.Keywords.Length > 0)
                    {
                        return Constants.AllKeywords.Where(x => SelectedVehicleType.Keywords.Contains(x.ktype))
                            .ToList();
                    }
                    else if(SelectedVehicleType.vtype == "War Rig")
                    {
                        return new List<KeywordData> { new KeywordData { ktype = "Special Rules", rules = "See rulebook for special rules related to War Rigs. Articulated, Ponderous & Piledriver Attack." } };
                    }
                    else
                    {
                        return new List<KeywordData> { new KeywordData { ktype = "None", rules = "This vehicle contains no special rules." } };
                    }
                }

                return new List<KeywordData>();
            }

        }

        public ICommand Save => new Command(OnSaveAsync);

        public SelectVehicleViewModel(AddVehicleViewModel parent, VehicleType vehicle = null)
        {
            this.Vehicle = parent;

            this.Title = "Vehicle Type";

            this.VehicleTypes = Constants.AllVehicletypes.Where(v =>
            {
                if (parent.Team.SponsorMode == false)
                    return true;
                else if (parent.Team.SelectedSponsor.name == "Rutherford")
                    return v.weight != "Light";
                else if (parent.Team.SelectedSponsor.name != "Rutherford" && new string[] { "Tank", "Helicopter" }.Contains(v.vtype))
                    return false;
                else if (parent.Team.SelectedSponsor.name == "Miyazaki")
                    return v.handling > 2;
                else if (parent.Team.SelectedSponsor.name == "Idris")
                    return !new string[] { "Gryrocopter" }.Contains(v.vtype);

                // only 1 tank or helicopter allowed in sponsor mode
                else if (parent.Team.SponsorMode && v.vtype == "Tank" && parent.Team.Vehicles.Count(x => x.SelectedVehicleType?.vtype == "Tank") == 0)
                    return true;
                else if (parent.Team.SponsorMode && v.vtype == "Helicopter" && parent.Team.Vehicles.Count(x => x.SelectedVehicleType?.vtype == "Helicopter") == 0)
                    return true;

                else
                    return !new string[] { "Tank", "Helicopter" }.Contains(v.vtype);

            }).ToList();

            this.SelectedVehicleType = VehicleTypes.FirstOrDefault(x => x.vtype == vehicle?.vtype) ?? this.VehicleTypes.FirstOrDefault();
        }

        async void OnSaveAsync(object arg)
        {
            MessagingCenter.Send(this, "VEHICLETYPESAVED");
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }
    }
}
