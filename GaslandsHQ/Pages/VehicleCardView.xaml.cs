using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GaslandsHQ.Pages
{
	public partial class VehicleCardView : ContentView
	{
		public static readonly BindableProperty PageProperty = BindableProperty.Create(nameof(Page), typeof(AddTeamPage), typeof(VehicleCardView) );

		public AddTeamPage Page
		{
			get => GetValue(PageProperty) as AddTeamPage;
			set => SetValue(PageProperty, value);
		}


		public VehicleCardView()
		{
			InitializeComponent();

			this.addOnsSegments.OnSegmentSelected += AddOnsSegments_OnSegmentSelected;
		}

		private void AddOnsSegments_OnSegmentSelected(object sender, Plugin.Segmented.Event.SegmentSelectEventArgs e)
		{
			this.weaponsView.IsVisible = false;
			this.upgradesView.IsVisible = false;
			this.perksView.IsVisible = false;
			switch (this.addOnsSegments.SelectedSegment)
			
			{
				case 0: this.weaponsView.IsVisible = true; break;
				case 1: this.upgradesView.IsVisible = true; break;
				case 2: this.perksView.IsVisible = true; break;
			}
		}
	}
}
