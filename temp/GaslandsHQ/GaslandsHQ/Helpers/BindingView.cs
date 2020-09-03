using System;
using Xamarin.Forms;

namespace GaslandsHQ.Helpers
{
	public class BindingView : ContentView
	{
		public BindingView()
		{
			var info = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
			WidthRequest = info.Width / info.Density;
			HorizontalOptions = LayoutOptions.FillAndExpand;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (BindingContext is View v)
			{
				this.Content = v;
			}
		}
	}
}
