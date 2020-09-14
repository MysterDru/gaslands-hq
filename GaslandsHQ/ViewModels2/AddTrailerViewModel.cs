using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsHQ.Models;

namespace GaslandsHQ.ViewModels2
{
    public class AddTrailerViewModel : BaseViewModel
    {
        private AddVehicleViewModel vehicle;

        public string Title { get; }

        public bool CanSelectTrailer { get; }

        public List<Trailer> Trailers { get; }

        public Trailer SelectedTrailer { get; set; }

        public List<Cargo> Cargos { get; }

        public Cargo SelectedCargo { get; set; }

        public int Cost => SelectedTrailer?.cost ?? 0;

        public int Slots => SelectedTrailer?.slots ?? 0;

        public AddTrailerViewModel(AddVehicleViewModel vehicle, Trailer defaultTrailer = null, Cargo defaultCargo = null)
        {
            this.vehicle = vehicle;

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
        }
    }
}
