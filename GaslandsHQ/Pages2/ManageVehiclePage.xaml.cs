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

    }
}
