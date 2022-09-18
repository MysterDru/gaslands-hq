using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GaslandsHQ.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class AddVehicleViewModel : BaseViewModel
    {
        public string Title => "Vehicle";

        public Guid Id { get; set; }

        public AddTeamViewModel Team { get; }

        public SelectVehicleViewModel VehicleType { get; }

        public VehicleType SelectedVehicleType => VehicleType?.SelectedVehicleType;

        public bool CanSelectVehicleType
        {
            get
            {
                if (SelectedVehicleType == null) return true;

                if (Weapons.Count > 1 || Perks.Count > 0 || Trailers.Count > 0
                    // for upgrades, buggy gets the rollcage automatically
                    || ((this.SelectedVehicleType.vtype == "Buggy" && Upgrades.Count == 1) ? false : Upgrades.Count > 0))
                    return false;

                if (this.Weapons.Count == 1 && this.Weapons[0].SelectedWeapon?.wtype == "Handgun")
                    return true;

                return true;
            }
        }

        public List<string> AllowedAddons { get; private set; }

        public int SelectedAddonIndex { get; set; }

        public bool ShowWeapons => CanAddAddons && SelectedAddonIndex == 0;
        public bool ShowUpgrades => CanAddAddons && SelectedAddonIndex == 1;
        public bool ShowPerks => CanAddAddons && SelectedAddonIndex == 2;
        public bool ShowTrailers => CanAddAddons && ((!AllowedAddons.Contains("Perks") && SelectedAddonIndex == 2) || (AllowedAddons.Contains("Perks") && SelectedAddonIndex == 3));

        public string Name { get; set; }

        public bool CanAddAddons => SelectedVehicleType != null;

        public int TotalCost
        {
            get
            {
                if (SelectedVehicleType == null) return 0;

                var cst = SelectedVehicleType.cost + this.Weapons.Sum(x => x.Cost) + this.Upgrades.Sum(x => x.Cost) + this.Perks.Sum(x => x.Cost) + this.Trailers.Sum(x => x.Cost);

                return cst;
            }
        }

        public string SpecialRules
        {
            get
            {
                if (this.SelectedVehicleType?.Keywords != null && this.SelectedVehicleType.Keywords.Length > 0)
                {
                    var matchingKeywords = Constants.AllKeywords.Where(x => SelectedVehicleType.Keywords.Contains(x.ktype));

                    var sb = new StringBuilder();
                    foreach (var k in matchingKeywords)
                    {
                        sb.AppendLine($"{k.ktype}: {k.rules}");
                    }

                    return sb.ToString();
                }

                return null;
            }
        }

        public int UsedSlots => this.Weapons.Sum(x => x.Slots)+ this.Upgrades.Sum(x => x.Slots);

        public int AvailableSlots => (SelectedVehicleType?.slots ?? 0) + this.Trailers.Sum(x => x.Slots);

        public int Handling => (SelectedVehicleType?.handling ?? 0) + this.Upgrades.Sum(x => x.Handling) + this.Perks.Sum(x => x.Handling);

        public int Hull => (SelectedVehicleType?.hull ?? 0) + (this.Upgrades.Sum(x => x.Hull));// todo: calculate

        public int Crew => (SelectedVehicleType?.crew ?? 0) + this.Upgrades.Sum(x => x.Crew); // todo: calculate

        public int MaxGear => (SelectedVehicleType?.maxGear ?? 0) + this.Upgrades.Sum(x => x.MaxGear); // todo: calculate

        public string WeightClass => SelectedVehicleType?.weight ?? "N/A";

        public ICommand SelectVehicle => new Command(ExecuteSelectVehicleTypeAsync);

        #region Weapons

        public ObservableCollection<SelectWeaponViewModel> Weapons { get; }

        public ICommand AddWeapon => new Command(ExecuteAddWeaponAsync);

        public ICommand EditWeapon => new Command(ExecuteEditWeaponAsync);

        public ICommand DeleteWeapon => new Command(ExecuteDeleteWeapon);

        public string WeaponsDisplayText => string.Join(", ", this.Weapons?.Select(x => x.SelectedWeapon?.wtype)?.ToArray() ?? new string[0]);

        #endregion

        #region Upgrade

        public ObservableCollection<SelectUpgradeViewModel> Upgrades { get; }

        public ICommand AddUpgrade => new Command(ExecuteAddUpgradeAsync);

        public ICommand EditUpgrade => new Command(ExecuteEditUpgradeAsync);

        public ICommand DeleteUpgrade => new Command(ExecuteDeleteUpgrade);

        public string UpgradesDisplayText => string.Join(", ", this.Upgrades?.Select(x => x.SelectedUpgrade?.utype)?.ToArray() ?? new string[0]);

        #endregion

        #region Perks

        public ObservableCollection<SelectPerkViewModel> Perks { get; }

        public ICommand AddPerk => new Command(ExecuteAddPerkAsync);

        public ICommand EditPerk => new Command(ExecuteEditPerkAsync);

        public ICommand DeletePerk => new Command(ExecuteDeletePerk);

        public string PerksDisplayText => string.Join(", ", this.Perks?.Select(x => x.SelectedPerk?.ptype)?.ToArray() ?? new string[0]);

        #endregion

        #region Trailer

        [DependsOn(nameof(SelectedVehicleType))]
        public bool TrailersSupported
        {
            get
            {
                if (this.Team.SponsorMode && this.Team.SelectedSponsor.keywords.Contains("Trailer Trash"))
                    return true;
                else if (this.SelectedVehicleType?.vtype == "War Rig")
                    return true;

                return false;
            }
        }

        public ObservableCollection<SelectTrailerViewModel> Trailers { get; }

        public ICommand AddTrailer => new Command(ExecuteAddTrailer, (v) => CanAddAdditionalTrailers);

        public ICommand EditTrailer => new Command(ExecuteEditTrailer);

        public ICommand DeleteTrailer => new Command(ExecuteDeleteTrailer, (v) => (v as SelectTrailerViewModel)?.SelectedTrailer?.ttype != "War Rig");

        public bool CanAddAdditionalTrailers { get; private set; }

        public string TrailerDisplayText
        {
            get
            {
                if (this.Trailers?.Count == 1)
                    return $"{this.Trailers[0].SelectedTrailer.ttype} Trailer, {this.Trailers[0].SelectedCargo.ctype}";

                return null;
            }
        }

        #endregion

        public ICommand Save => new Command(OnSaveAsync);

        public ICommand Delete => new Command(OnDeleteAsync);

        public AddVehicleViewModel(AddTeamViewModel team)
        {
            this.Weapons = new ObservableCollection<SelectWeaponViewModel>();
            this.Upgrades = new ObservableCollection<SelectUpgradeViewModel>();
            this.Perks = new ObservableCollection<SelectPerkViewModel>();
            this.Trailers = new ObservableCollection<SelectTrailerViewModel>();

            this.Team = team;

            this.Name = $"Vehicle {team.Vehicles.Count + 1}";

            this.VehicleType = new SelectVehicleViewModel(this);

            // add default handgun
            this.Weapons.Add(new SelectWeaponViewModel(this, Constants.AllWeapons.First(x => x.wtype == "Handgun")));

            AllowedAddons = new List<string>
            {
                "Weapons",
                "Upgrades"
            };

            if (team.SponsorMode)
            {
                AllowedAddons.Add("Perks");
            }

            this.Id = Guid.NewGuid();

            MessagingCenter.Subscribe<SelectVehicleViewModel>(this, "VEHICLETYPESAVED", OnVehicleTypeSaved);

            MessagingCenter.Subscribe<SelectWeaponViewModel>(this, "WEAPONSAVED", OnWeaponSaved);
            MessagingCenter.Subscribe<SelectWeaponViewModel>(this, "WEAPONDELETED", (i) => this.ExecuteDeleteWeapon(i));

            MessagingCenter.Subscribe<SelectUpgradeViewModel>(this, "UPGRADESAVED", OnUpgradeSaved);
            MessagingCenter.Subscribe<SelectUpgradeViewModel>(this, "UPGRADEDELETED", (i) => this.ExecuteDeleteUpgrade(i));

            MessagingCenter.Subscribe<SelectPerkViewModel>(this, "PERKSAVED", OnPerkSaved);
            MessagingCenter.Subscribe<SelectPerkViewModel>(this, "PERKDELETED", (i) => this.ExecuteDeletePerk(i));

            MessagingCenter.Subscribe<SelectTrailerViewModel>(this, "TRAILERSAVED", OnTrailerSaved);
            MessagingCenter.Subscribe<SelectTrailerViewModel>(this, "TRAILERDELETED", (i) => this.ExecuteDeleteTrailer(i));

            // trigger vehicle tyep save logic
            this.OnVehicleTypeSaved(this.VehicleType);
        }

        // restore data
        public AddVehicleViewModel(AddTeamViewModel team, UserVehicle userVehicle) : this(team)
        {
            this.Id = userVehicle.Id;
            this.Name = userVehicle.VehicleName;

            this.VehicleType = new SelectVehicleViewModel(this, userVehicle.VehicleType);

            //this.SelectedVehicleType = this.VehicleTypes.FirstOrDefault(x => x.vtype == userVehicle.VehicleType?.vtype);

            this.Weapons.Clear();
            foreach (var w in userVehicle.Weaposn)
            {
                var vm = new SelectWeaponViewModel(this, w);
                this.Weapons.Add(vm);
            }

            this.Perks.Clear();
            foreach (var p in userVehicle.Perks)
            {
                var vm = new SelectPerkViewModel(this, p);
                this.Perks.Add(vm);
            }

            this.Upgrades.Clear();
            foreach (var u in userVehicle.Upgrades)
            {
                var vm = new SelectUpgradeViewModel(this, u);
                this.Upgrades.Add(vm);
            }

            this.Trailers.Clear();
            foreach (var t in userVehicle.Trailers)
            {
                var vm = new SelectTrailerViewModel(this, t.Trailer, t.Cargo);
                this.Trailers.Add(vm);

                // run trailer support logic
                this.AddTrailerSupport();
            }

            // todo: restore
        }

        void AddTrailerSupport()
        {
            this.CanAddAdditionalTrailers = true;

            if (!AllowedAddons.Contains("Trailers"))
            {
                var addons = this.AllowedAddons.ToList();
                addons.Add("Trailers");

                this.AllowedAddons = addons;

                if (this.SelectedVehicleType?.vtype == "War Rig")
                {
                    // default add a trailer. user can customize
                    this.Trailers.Add(new SelectTrailerViewModel(this, null, null));
                    CanAddAdditionalTrailers = false;
                }
            }
        }

        void RemoveTrailerSupport()
        {
            CanAddAdditionalTrailers = false;
            if (AllowedAddons.Contains("Trailers"))
            {
                var addons = this.AllowedAddons.ToList();
                addons.Remove("Trailers");

                this.AllowedAddons = addons;
            }
            this.Trailers.Clear();
        }

        async void ExecuteSelectVehicleTypeAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            await nav.Navigate(this.VehicleType);
        }

        void OnVehicleTypeSaved(SelectVehicleViewModel obj)
        {
            if (this.VehicleType == obj)
            {
                // need  to add or remoe free roll  cage based on being a buggy
                var rollCage = this.Upgrades.FirstOrDefault(x => x.SelectedUpgrade?.utype == "Roll Cage");
                var rcModel = Constants.AllUpgrades.First(x => x.utype == "Roll Cage");

                if (this.SelectedVehicleType?.vtype == "Buggy")
                {
                    if (rollCage == null)
                    {
                        this.Upgrades.Add(new SelectUpgradeViewModel(this, rcModel));
                    }
                }

                // if not  buggy, and roll cage is free, remove it
                else if (rollCage != null)
                    this.Upgrades.Remove(rollCage);

                this.RaiseAllPropertiesChanged();
            }
        }

        #region Weapons

        async void ExecuteAddWeaponAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new SelectWeaponViewModel(this);
            await nav.Navigate(vm);
        }

        async void ExecuteEditWeaponAsync(object obj)
        {
            var vm = obj as SelectWeaponViewModel;

            if (vm != null)
            {
                var model = vm.GetModel();

                var editVm = new SelectWeaponViewModel(this, model);
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(editVm);
            }
        }

        void ExecuteDeleteWeapon(object obj)
        {
            var vm = obj as SelectWeaponViewModel;

            var match = this.Weapons.FirstOrDefault(x => x.Id == vm.Id);

            if (match != null && match.CanSelect)
            {
                this.Weapons.Remove(match);

                this.RaiseAllPropertiesChanged();
            }
        }

        void OnWeaponSaved(SelectWeaponViewModel obj)
        {
            var match = this.Weapons.FirstOrDefault(x => x.Id == obj.Id);

            if (match != null)
            {
                var idx = this.Weapons.IndexOf(match);

                this.Weapons.RemoveAt(idx);
                this.Weapons.Insert(idx, obj);
            }
            else if (obj.Vehicle == this)
                this.Weapons.Add(obj);

            this.RaiseAllPropertiesChanged();
        }

        #endregion

        #region Upgrades

        async void ExecuteAddUpgradeAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new SelectUpgradeViewModel(this);

            await nav.Navigate(vm);
        }

        async void ExecuteEditUpgradeAsync(object obj)
        {
            var vm = obj as SelectUpgradeViewModel;

            if (vm != null)
            {
                var edit = new SelectUpgradeViewModel(this, vm.GetModel());
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(edit);
            }
        }

        void ExecuteDeleteUpgrade(object obj)
        {
            var vm = obj as SelectUpgradeViewModel;

            var match = this.Upgrades.FirstOrDefault(x => x.Id == vm.Id);

            if (match != null && match.CanSelect)
            {
                this.Upgrades.Remove(match);

                this.RaiseAllPropertiesChanged();
            }
        }

        void OnUpgradeSaved(SelectUpgradeViewModel obj)
        {
            var match = this.Upgrades.FirstOrDefault(x => x.Id == obj.Id);

            if (match != null)
            {
                var idx = this.Upgrades.IndexOf(match);

                this.Upgrades.RemoveAt(idx);
                this.Upgrades.Insert(idx, obj);
            }
            else if (obj.Vehicle == this)
                this.Upgrades.Add(obj);

            this.RaiseAllPropertiesChanged();
        }

        #endregion

        #region Perks

        async void ExecuteAddPerkAsync(object obj)
        {
            var nav = DependencyService.Get<INavigationService>();

            var vm = new SelectPerkViewModel(this);

            await nav.Navigate(vm);
        }

        async void ExecuteEditPerkAsync(object obj)
        {
            var vm = obj as SelectPerkViewModel;

            if (vm != null)
            {
                var edit = new SelectPerkViewModel(this, vm.GetModel());
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(edit);
            }
        }

        void ExecuteDeletePerk(object obj)
        {
            var vm = obj as SelectPerkViewModel;

            var match = this.Perks.FirstOrDefault(x => x.Id == vm.Id);

            if (match != null)
            {
                this.Perks.Remove(match);

                this.RaiseAllPropertiesChanged();
            }
        }

        void OnPerkSaved(SelectPerkViewModel obj)
        {
            var match = this.Perks.FirstOrDefault(x => x.Id == obj.Id);

            if (match != null)
            {
                var idx = this.Perks.IndexOf(match);

                this.Perks.RemoveAt(idx);
                this.Perks.Insert(idx, obj);
            }
            else if(obj.Vehicle == this)
                this.Perks.Add(obj);

            this.RaiseAllPropertiesChanged();
        }

        #endregion

        #region Trailer

        void ExecuteAddTrailer(object obj)
        {
            var vm = new SelectTrailerViewModel(this);

            this.CanAddAdditionalTrailers = false;

            DependencyService.Get<INavigationService>().Navigate(vm);
        }

        async void ExecuteEditTrailer(object obj)
        {
            var vm = obj as SelectTrailerViewModel;

            if (vm != null)
            {
                var edit = new SelectTrailerViewModel(this, vm.GetModel());
                var nav = DependencyService.Get<INavigationService>();
                await nav.Navigate(vm);
            }
        }

        void ExecuteDeleteTrailer(object obj)
        {
            var vm = obj as SelectTrailerViewModel;

            var match = this.Trailers.FirstOrDefault(x => x.Id == vm.Id);

            if (match != null && match.CanSelectTrailer)
            {
                this.Trailers.Remove(match);

                this.CanAddAdditionalTrailers = true;
                this.RaiseAllPropertiesChanged();
            }
        }

        void OnTrailerSaved(SelectTrailerViewModel obj)
        {
            var match = this.Trailers.FirstOrDefault(x => x.Id == obj.Id);

            if (match != null)
            {
                var idx = this.Trailers.IndexOf(match);

                this.Trailers.RemoveAt(idx);
                this.Trailers.Insert(idx, obj);
            }
            else if (obj.Vehicle == this)
                this.Trailers.Add(obj);

            (this.AddTrailer as Command).ChangeCanExecute();
            (this.DeleteTrailer as Command).ChangeCanExecute();

            this.RaiseAllPropertiesChanged();
        }

        #endregion

        public override void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            base.RaisePropertyChanged(changedArgs);

            if (changedArgs.PropertyName == nameof(CanAddAdditionalTrailers))
                (this.AddTrailer as Command).ChangeCanExecute();

            if (changedArgs.PropertyName == nameof(SelectedVehicleType))
            {
                if (TrailersSupported == true)
                    AddTrailerSupport();
                else
                    RemoveTrailerSupport();
            }
        }

        async void OnSaveAsync(object arg)
        {
            MessagingCenter.Send(this, "VEHICLESAVED");
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }

        async void OnDeleteAsync(object obj)
        {
            var dialogs =  DependencyService.Get<IDialogsService>();
            if (await  dialogs
                .ConfirmAsync("Are you sure you want to delete this vehicle?", "Delete", "Cancel", true))
            {
                MessagingCenter.Send(this, "VEHICLEDELETED");
                await DependencyService.Get<INavigationService>().Dismiss(this);
            }
        }
    }

}
