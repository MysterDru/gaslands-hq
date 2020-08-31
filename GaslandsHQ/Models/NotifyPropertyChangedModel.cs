using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GaslandsHQ.Models
{
    public class NotifyPropertyChangedModel : INotifyPropertyChanged
    {
		private bool suppress;

		public event PropertyChangedEventHandler PropertyChanged;

		

		public void RaisePropertyChanged([CallerMemberName] string whichProperty = "")
		{
			var changedArgs = new PropertyChangedEventArgs(whichProperty);
			RaisePropertyChanged(changedArgs);
		}

		public virtual void RaiseAllPropertiesChanged()
		{
			var allProperties = this.GetType().GetRuntimeProperties();
			foreach (var property in allProperties)
				RaisePropertyChanged(property.Name);
		}

		public virtual void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
		{
			if (!InterceptRaisePropertyChanged(changedArgs) && !this.suppress)
			{
				Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
				{
					PropertyChanged?.Invoke(this, changedArgs);
				});
			}
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
			{
				return false;
			}

			storage = value;
			RaisePropertyChanged(propertyName);
			return true;
		}

		protected virtual bool InterceptRaisePropertyChanged(PropertyChangedEventArgs changedArgs)
		{
			return false;
		}

		public void SuppressPropertyChanged(bool suppress)
		{
			this.suppress = suppress;
		}
	}
}
