using System;
using GaslandsHQ.ViewModels2;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GaslandsHQ
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string droidAppCenterKey = Config.AppCenterAndroidKey;
#if RELEASE
            AppCenter.Start($"android={droidAppCenterKey}", typeof(Distribute), typeof(Crashes));
#endif

            //MainPage = new NavigationPage(new Pages.MainPage());
            DependencyService.Get<INavigationService>()
                .Navigate(new MainViewModel())
                .Wait();

#if RELEASE
            Distribute.CheckForUpdate();
#endif
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
