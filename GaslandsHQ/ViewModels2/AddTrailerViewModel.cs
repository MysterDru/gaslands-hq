using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GaslandsHQ.Models;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class AddTrailerViewModel : BaseViewModel
    {
        public AddVehicleViewModel Vehicle { get; }

        public Guid Id { get; private set; }

        public string Title { get; }

        public bool CanSelectTrailer { get; }

        public List<Trailer> Trailers { get; }

        public Trailer SelectedTrailer { get; set; }

        public List<Cargo> Cargos { get; }

        public Cargo SelectedCargo { get; set; }

        public int Cost => SelectedTrailer?.cost ?? 0;

        public int Slots => SelectedTrailer?.slots ?? 0;

        public ICommand Save => new Command(OnSaveAsync);

        public ICommand Delete => new Command(OnDeleteAsync);

        public AddTrailerViewModel(AddVehicleViewModel vehicle, Trailer defaultTrailer = null, Cargo defaultCargo = null)
        {
            this.Vehicle = vehicle;

            this.Trailers = Constants.AllTrailers;
            this.Cargos = Constants.AllCargo;

            this.Title = "Trailer";

            if (vehicle.SelectedVehicleType.vtype == "War Rig")
            {
                this.Trailers.Add(new Trailer
                {
                    ttype = "War Rig",
                    cost = 0,
                    slots = 0
                });

                this.SelectedTrailer = this.Trailers.Last();
            }
            else
            {
                this.SelectedTrailer = this.Trailers.FirstOrDefault(x => x.ttype == defaultTrailer?.ttype) ?? Trailers[0];
            }

            // default to none
            this.SelectedCargo = this.Cargos.FirstOrDefault(x => x.ctype == defaultCargo?.ctype) ?? Cargos[0];

            this.CanSelectTrailer = this.SelectedTrailer?.ttype != "War Rig";

            this.Id = Guid.NewGuid();
        }

        public AddTrailerViewModel(AddVehicleViewModel vehicle, UserTrailer defaultTrailer) : this(vehicle, defaultTrailer?.Trailer, defaultTrailer?.Cargo)
        {
            this.Id = defaultTrailer?.Id ?? Guid.NewGuid();
        }

        public UserTrailer GetModel()
        {
            return new UserTrailer
            {
                Id = this.Id,
                Trailer = this.SelectedTrailer,
                Cargo = this.SelectedCargo
            };
        }

        async void OnSaveAsync(object arg)
        {
            MessagingCenter.Send(this, "TRAILERSAVED");
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }

        async void OnDeleteAsync(object obj)
        {
            var dialogs = DependencyService.Get<IDialogsService>();
            if (await dialogs
                .ConfirmAsync("Are you sure you want to delete this trailer?", "Delete", "Cancel", true))
            {
                MessagingCenter.Send(this, "TRAILERDELETED");
                await DependencyService.Get<INavigationService>().Dismiss(this);
            }
        }
    }
}
