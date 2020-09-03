using System;
using System.Collections.Generic;
using GaslandsHQ.ViewModels2;
using Xamarin.Forms;

namespace GaslandsHQ.Pages2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void TeamList_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {
                (this.BindingContext as MainViewModel)
                    .EditTeam.Execute(e.SelectedItem);
            }

            this.TeamList.SelectedItem = null;

        }

    }
}
