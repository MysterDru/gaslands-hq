using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GaslandsHQ.Core.Data;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.Pages
{
    public partial class AddTeamPage : ContentPage
    {
        public Team Team { get; } = new Team()
        {
            Vehicles = new ObservableCollection<Vehicle>()
        };

        public string TeamName { get; set; }

        public string Cans { get; set; }

        public List<Sponsor> Sponsors { get; set; }

        public List<VehicleType> VehicleTypes { get; set; }

        public List<Weapon> Weapons { get; set; }

        public Sponsor SelectedSponsor { get; set; }

        public Command AddVehicle => new Command(ExecuteAddVehicle);

        public Command AddWeapon => new Command<Vehicle>(ExecuteAddWeapon);

        public Command RemoveWeapon => new Command<VehicleWeapon>(ExecuteRemoveWeapon);

        public Command AddUpgrade => new Command<Vehicle>(ExecuteAddUpgrade);
        
        public Command AddPerk => new Command<Vehicle>(ExecuteAddPerk);        

        public AddTeamPage()
        {
            InitializeComponent();

            var lookups = new LookupRepository();
            this.Sponsors = lookups.GetValues<Sponsor>()?.ToList();
            this.VehicleTypes = lookups.GetValues<VehicleType>()?.ToList();
            this.Weapons = lookups.GetValues<Weapon>()?.ToList();

            this.BindingContext = this;
        }

        void ExecuteAddVehicle(object obj)
        {
            this.Team.Vehicles.Add(new Vehicle()
            {
                Name = $"Vehicle {this.Team.Vehicles.Count + 1}",
                Weapons = new ObservableCollection<VehicleWeapon>()
                {
                    new VehicleWeapon
                    {
                        Weapon = this.Weapons.First(x => x.Name == "Handgun"),
                        Facing = "360"
                    }                     
                },
                Perks = new ObservableCollection<Perk>(),
                Upgrades = new ObservableCollection<Upgrade>(),
                Type = this.VehicleTypes.First(),
            });

        }

        private void WeaponLabel_Tapped(object sender, EventArgs e)
        {
            var picker= (sender as Label).Parent.FindByName("weaponPicker") as Picker;

            picker.Focus();
        }

        void ExecuteAddWeapon(Vehicle obj)
        {
            //obj.Weapons.Add(new VehicleWeapon { Facing = "Front" });
            obj.Weapons.Add(new VehicleWeapon
            {
                Weapon = this.Weapons.First(x => x.Name == "Heavy Machine Gun"),
                Facing = "Font"
            });
        }

        void ExecuteRemoveWeapon(VehicleWeapon obj)
        {
            Vehicle mathingV = null;
            foreach(var vehicles in Team.Vehicles)
			{
                if (vehicles.Weapons.Contains(obj))
                {
                    mathingV = vehicles;
                    break;
                }
			}

            if (mathingV != null)
                mathingV.Weapons.Remove(obj);
        }

        void ExecuteAddUpgrade(Vehicle obj)
        {
            
        }

        void ExecuteAddPerk(Vehicle obj)
        {
            
        }

        
    }
}
