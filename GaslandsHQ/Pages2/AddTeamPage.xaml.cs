using System;
using System.Collections.Generic;
using GaslandsHQ.ViewModels2;
using Xamarin.Forms;

namespace GaslandsHQ.Pages2
{
    public partial class AddTeamPage : ContentPage
    {
        public AddTeamPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform != "iOS")
                this.ToolbarItems.Remove(this.cancelItem);
        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var vm = this.BindingContext as AddTeamViewModel;
                vm.EditVehicle.Execute(e.SelectedItem);
            }

            VehicleList.SelectedItem = null;
        }
    }
}
