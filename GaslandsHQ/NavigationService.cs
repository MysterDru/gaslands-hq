using System;
using System.Linq;
using System.Threading.Tasks;
using GaslandsHQ.Pages2;
using GaslandsHQ.ViewModels2;
using Xamarin.Forms;

[assembly: Dependency(typeof(GaslandsHQ.NavigationService))]

namespace GaslandsHQ
{
    public interface INavigationService
    {
        Task Navigate<TViewModel>(object parameter = null);

        Task Navigate<TViewModel>(TViewModel viewMOdel);

        Task Dismiss<TViewModel>(TViewModel @this);
    }

    public class NavigationService : INavigationService
    {
        public async Task Dismiss<TViewModel>(TViewModel @this)
        {
            var nav = Xamarin.Forms.Application.Current.MainPage as NavigationPage;

            var modalNav = nav.Navigation.ModalStack.FirstOrDefault() as NavigationPage;
            if (modalNav != null)
            {
                var match = modalNav.Navigation.NavigationStack.FirstOrDefault(x => x.BindingContext?.GetType() == typeof(TViewModel));

                if (match != null && modalNav.Navigation.NavigationStack.Count == 1)
                    await nav.Navigation.PopModalAsync();
                else
                    modalNav.Navigation.RemovePage(match);
            }
            else
            {
                var match = nav.Navigation.NavigationStack.FirstOrDefault(x => x.BindingContext?.GetType() == typeof(TViewModel));

                if (match != null)
                    nav.Navigation.RemovePage(match);
            }
        }

        public async Task Navigate<TViewModel>(object parameter = null)
        {
            TViewModel viewModel = Activator.CreateInstance<TViewModel>();

            await this.Navigate(viewModel);
        }

        public async Task Navigate<TViewModel>(TViewModel viewModel)
        {
            Page page = null;
            bool modal = false;

            if (typeof(TViewModel) == typeof(AddTeamViewModel))
            {
                page = new AddTeamPage();
                modal = true;
            }
            else if (typeof(TViewModel) == typeof(MainViewModel))
                page = new MainPage();
            else if (typeof(TViewModel) == typeof(AddVehicleViewModel))
            {
                page = new AddVehiclePage();
                modal = true;
            }
            else if (typeof(TViewModel) == typeof(AddWeaponViewModel))
            {
                page = new AddWeaponPage();
                modal = true;
            }
            else if (typeof(TViewModel) == typeof(AddUpgradeViewModel))
            {
                page = new AddUpgradePage();
                modal = true;
            }
            else if (typeof(TViewModel) == typeof(AddPerkViewModel))
            {
                page = new AddPerkPage();
                modal = true;
            }
            else if (typeof(TViewModel) == typeof(AddTrailerViewModel))
            {
                page = new AddTrailerPage();
                modal = true;
            }
            else if (typeof(TViewModel) == typeof(InfoViewModel))
            {
                page = new InfoPage();
                modal = true;
            }
            else
                return;

            page.BindingContext = viewModel;

            var nav = Application.Current.MainPage as NavigationPage;
            var current = nav?.CurrentPage;
            if (page is MainPage)
            {
                nav = new NavigationPage(page)
                {
                    BarBackgroundColor = Color.FromHex("#2196F3"),
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = nav;
            }
            else
            {
                var modalNav = nav.Navigation.ModalStack.FirstOrDefault() as NavigationPage;
                if(modalNav == null)                
                {
                    modalNav = new NavigationPage(page)
                    {
                        BarBackgroundColor = Color.FromHex("#2196F3"),
                        BarTextColor = Color.White
                    };
                    await nav.Navigation.PushModalAsync(modalNav);
                }
                else
                {
                    await modalNav.Navigation.PushAsync(page);
                }
            }
        }
    }
}