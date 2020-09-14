using System;
using System.Collections.Generic;
using GaslandsHQ.ViewModels2;
using Xamarin.Forms;

namespace GaslandsHQ.Pages2
{
    public partial class AddVehiclePage : ContentPage
    {
        public AddVehiclePage()
        {
            InitializeComponent();
        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {
                (this.BindingContext as AddVehicleViewModel).EditWeapon.Execute(e.SelectedItem);
            }

            weaponsList.SelectedItem = null;
        }

        void ListView_ItemSelected_1(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                (this.BindingContext as AddVehicleViewModel).EditUpgrade.Execute(e.SelectedItem);
            }

            this.upgradesList.SelectedItem = null;
        }

        void ListView_ItemSelected_2(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                (this.BindingContext as AddVehicleViewModel).EditPerk.Execute(e.SelectedItem);
            }

            perksList.SelectedItem = null;
        }

        void trailerList_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                (this.BindingContext as AddVehicleViewModel).EditTrailer.Execute(e.SelectedItem);
            }

            trailerList.SelectedItem = null;
        }
    }
}
