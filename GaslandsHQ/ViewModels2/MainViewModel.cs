using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GaslandsHQ.ViewModels2
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand AddTeam => new Command(ExecuteAddTeamAsync);

        public ObservableCollection<AddTeamViewModel> Teams { get; }

        public ICommand EditTeam => new Command(ExecuteEditTeamAsync);

        public MainViewModel()
        {
            this.Teams = new ObservableCollection<AddTeamViewModel>();
        }

        async void ExecuteAddTeamAsync(object obj)
        {
            var vm = new AddTeamViewModel();
            Teams.Add(vm);

            await DependencyService.Get<INavigationService>().Navigate(vm);
        }

        async void ExecuteEditTeamAsync(object obj)
        {
            var vm = obj as AddTeamViewModel;

            await DependencyService.Get<INavigationService>().Navigate(vm);
        }
    }
}
