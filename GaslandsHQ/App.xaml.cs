using System;
using GaslandsHQ.ViewModels2;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
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

#if RELEASE
            string iosAppCenterKey = Config.AppCenteriOSKey;
            string droidAppCenterKey = Config.AppCenterAndroidKey;
            AppCenter.Start($"ios={iosAppCenterKey};android={droidAppCenterKey}", typeof(Analytics), typeof(Crashes));
#endif

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
