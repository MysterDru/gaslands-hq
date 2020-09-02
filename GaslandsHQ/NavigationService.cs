using System;
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
    }

    public class NavigationService : INavigationService
    {
        public enum View
        {
            AddTeam,
            AddEditVehicle,
            AddEditWeapon,
            AddEditUpgrade,
            AddEditPerk
        }

        public async Task Navigate<TViewModel>(object parameter = null)
        {
            TViewModel viewModel = Activator.CreateInstance<TViewModel>();

            await this.Navigate(viewModel);
        }

        public async Task Navigate<TViewModel>(TViewModel viewModel)
        {
            Page page = null;

            if (typeof(TViewModel) == typeof(AddTeamViewModel))
                page = new AddTeamPage();
            else if (typeof(TViewModel) == typeof(MainViewModel))
                page = new MainPage();
            else if (typeof(TViewModel) == typeof(ManageVehicleViewModel))
                page = new ManageVehiclePage();
            else if (typeof(TViewModel) == typeof(AddWeaponViewModel))
                page = new AddWeaponPage();

            else
                return;

            page.BindingContext = viewModel;


            var nav = Xamarin.Forms.Application.Current.MainPage as NavigationPage;
            var current = nav?.CurrentPage;
            if (nav == null)
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else
                await current.Navigation.PushAsync(page, true);
        }
    }
}