using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace GaslandsHQ.Pages2
{
	[ContentProperty("ChildContent")]
	public partial class BasePopupPage : PopupPage
	{ 
		public View ChildContent { get; set; }

		public BasePopupPage()
		{
			InitializeComponent();
		}

		void OnChildContentChanged()
		{
			this.FrameContainer.Content = this.ChildContent;
		}

		void OnCloseButtonTapped(object sender, EventArgs e)
		{
			this.OnClose();
		}

		protected virtual async void OnClose()
		{
			var type = this.BindingContext?.GetType();

			if(type != null)
			{
				var nav = DependencyService.Get<INavigationService>();

				var dismiss  =  nav.GetType().GetRuntimeMethods()
					.FirstOrDefault(x => x.Name == "Dismiss");

				var method = dismiss.MakeGenericMethod(type);

				method.Invoke(nav, new object[] { this.BindingContext });
			}
		}
	}
}
