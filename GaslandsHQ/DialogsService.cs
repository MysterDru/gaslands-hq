using System;
using System.Threading.Tasks;
using GaslandsHQ;
using Xamarin.Forms;

[assembly: Dependency(typeof(DialogsService))]
namespace GaslandsHQ
{
    public interface IDialogsService
    {
        Task AlertAsync(string title, string message, string cancelText);

        Task<bool> ConfirmAsync(string message, string okay = "Okay", string cancel = "Cancel", bool asDestructive = false);
    }

    public class DialogsService :  IDialogsService
    {
        public async Task AlertAsync(string title, string message, string cancelText)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, cancelText);
        }

        public async Task<bool> ConfirmAsync(string message, string okay, string cancel, bool asDestructive = false)
        {
            var result = await Xamarin.Forms.Application.Current.MainPage.DisplayActionSheet(
                message,
                cancel,
                asDestructive ? okay : null,
                asDestructive ? null : okay);

            return result == okay;
        }
    }
}
