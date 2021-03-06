﻿using System;
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

            this.RemoveCancelItem();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.RemoveCancelItem();
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

        private void RemoveCancelItem()
        {
            if (Device.RuntimePlatform != "iOS" && this.ToolbarItems.Contains(this.cancelItem))
                this.ToolbarItems.Remove(this.cancelItem);
        }
    }
}
