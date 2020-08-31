using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GaslandsHQ.Models
{
    public class Team : NotifyPropertyChangedModel
    {
        public string Name { get; set; }

        public Sponsor Sponsor { get; set; }

        public int Cans { get; set; }

        public int TotalCost => this.Vehicles?.Sum(x => x.TotalCost) ?? 0;

        public ObservableCollection<Vehicle> Vehicles { get; set; }

        public Team()
        {
        }

        #region PropertyChanged Values

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            if(propertyName == nameof(Vehicles))
            {
                if(before !=null)
                    (before as ObservableCollection<Vehicle>).CollectionChanged -= Team_CollectionChanged;

                if(after != null)
                    (after as ObservableCollection<Vehicle>).CollectionChanged += Team_CollectionChanged;
            }

            //Perform property validation
            this.RaisePropertyChanged(propertyName);
        }

        void Team_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems != null)
                foreach(var old in e.OldItems)
                    (old as Vehicle).PropertyChanged -= Vehicle_PropertyChanged;

            if(e.NewItems != null)
                foreach (var @new in e.NewItems)
                    (@new as Vehicle).PropertyChanged += Vehicle_PropertyChanged;

            this.RaisePropertyChanged(nameof(TotalCost));
        }

        private void Vehicle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vehicle.TotalCost))
                this.RaisePropertyChanged(nameof(TotalCost));
        }

        #endregion
    }
}
