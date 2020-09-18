using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class InfoViewModel : BaseViewModel
    {
        public string Title => "Gaslands HQ (Beta)";

        public ICommand ViewOnGithub => new Command(ExecuteViewOnGithub);

        public ICommand Dismiss => new Command(ExecuteDismissAsync);

        public string AppVersion => $"{Xamarin.Essentials.AppInfo.VersionString} ({Xamarin.Essentials.AppInfo.BuildString})";

        async void ExecuteViewOnGithub(object obj)
        {
            await Xamarin.Essentials.Browser.OpenAsync("https://github.com/MysterDru/gaslands-hq", Xamarin.Essentials.BrowserLaunchMode.External);
        }

        async void ExecuteDismissAsync(object obj)
        {
            await DependencyService.Get<INavigationService>().Dismiss(this);
        }
    }
}
