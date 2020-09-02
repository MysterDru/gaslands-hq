using System;
using GaslandsHQ.ViewModels2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GaslandsHQ
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new Pages.MainPage());
            DependencyService.Get<INavigationService>()
                .Navigate(new MainViewModel())
                .Wait();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
