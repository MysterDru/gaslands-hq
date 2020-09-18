using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GaslandsHQ.Pages2
{
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();


            this.RemoveCancelItem();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.RemoveCancelItem();
        }

        private void RemoveCancelItem()
        {
            if (Device.RuntimePlatform != "iOS" && this.ToolbarItems.Contains(this.cancelItem))
                this.ToolbarItems.Remove(this.cancelItem);
        }
    }
}
