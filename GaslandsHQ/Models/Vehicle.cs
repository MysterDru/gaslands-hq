//using System;
//using System.Collections.ObjectModel;
//using System.Linq;
//using PropertyChanged;

//namespace GaslandsHQ.Models
//{
//	public class Vehicle : NotifyPropertyChangedModel
//	{
//		public string Name { get; set; }

//		[DependsOn(nameof(Type))]
//		public int AvailableSlots => this.Type?.Slots ?? 0;

//		public int Crew => (this.Type?.Crew ?? 0) + (this.Upgrades?.Sum(x => x.Upgrade?.Crew) ?? 0);

//		public int UsedSlots => (this.Weapons?.Sum(x => x.Weapon.Slots) ?? 0)
//					+ (this.Upgrades?.Sum(x => x.Upgrade?.Slots) ?? 0);

//		public int Hull => (this.Type?.Hull ?? 0) + (this.Upgrades?.Sum(x => x.Upgrade?.Hull) ?? 0);

//		public int MaxGear => (this.Type?.MaxGear ?? 0) + (this.Upgrades?.Sum(x => x.Upgrade?.Gear) ?? 0);

//		public int Handling => (this.Type?.Handling ?? 0) + (this.Upgrades?.Sum(x => x.Upgrade?.Handling) ?? 0);

//		public string WeightClass => this.Type?.Weight ;

//		public int TotalCost
//		{
//			get
//			{
//				return (Type?.Cost ?? 0) +
//					(this.Weapons?.Sum(x => x.Cost) ?? 0) +
//					(this.Perks?.Sum(x => x.Cost) ?? 0) +
//					(this.Upgrades?.Sum(x => x.Cost) ?? 0);
//			}
//		}

//		public VehicleType Type { get; set; }

//		public ObservableCollection<VehicleWeapon> Weapons { get; set; }

//		public ObservableCollection<Perk> Perks { get; set; }

//		public ObservableCollection<VehicleUpgrade> Upgrades { get; set; }

//		#region PropertyChanged Values

//		public void OnPropertyChanged(string propertyName, object before, object after)
//		{
//			if (propertyName == nameof(Weapon))
//			{
//				if (before != null)
//					(before as ObservableCollection<Weapon>).CollectionChanged -= CostedItemCollectionChanged;
//				if (after != null)
//					(after as ObservableCollection<Weapon>).CollectionChanged += CostedItemCollectionChanged;

//				RaisePropertyChanged(nameof(TotalCost));
//			}

//			if (propertyName == nameof(Perk))
//			{
//				if (before != null)
//					(before as ObservableCollection<Perk>).CollectionChanged -= CostedItemCollectionChanged;
//				if (after != null)
//					(after as ObservableCollection<Perk>).CollectionChanged += CostedItemCollectionChanged;

//				RaisePropertyChanged(nameof(TotalCost));
//			}

//			if (propertyName == nameof(Upgrade))
//			{
//				if (before != null)
//					(before as ObservableCollection<Upgrade>).CollectionChanged -= CostedItemCollectionChanged;

//				if (after != null)
//					(after as ObservableCollection<Upgrade>).CollectionChanged += CostedItemCollectionChanged;

//				RaisePropertyChanged(nameof(TotalCost));
//			}

//			//Perform property validation
//			this.RaisePropertyChanged(propertyName);
//		}

//		void CostedItemCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//		{
//			this.RaisePropertyChanged(nameof(TotalCost));
//			this.RaisePropertyChanged(nameof(UsedSlots));
//		}

//		#endregion
//	}
//}