using System;
using System.Collections.Generic;
using GaslandsHQ.ViewModels2;
using Xamarin.Forms;

namespace GaslandsHQ.Pages2
{
    public partial class ManageVehiclePage : ContentPage
    {
        public ManageVehiclePage()
        {
            InitializeComponent();
        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {
                (this.BindingContext as ManageVehicleViewModel).EditWeapon.Execute(e.SelectedItem);
            }

            weaponsList.SelectedItem = null;
        }

        void ListView_ItemSelected_1(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                (this.BindingContext as ManageVehicleViewModel).EditUpgrade.Execute(e.SelectedItem);
            }

            this.upgradesList.SelectedItem = null;
        }

        void ListView_ItemSelected_2(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                (this.BindingContext as ManageVehicleViewModel).EditPerk.Execute(e.SelectedItem);
            }

            perksList.SelectedItem = null;
        }
    }
}
