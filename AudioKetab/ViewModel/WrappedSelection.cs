using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AudioKetab
{
	public class WrappedSelection<TypeofBikeModel> : INotifyPropertyChanged
	{
		
		public TypeofBikeModel Item { get; set; }
		bool isSelected = false;
		bool isVisible = true;
		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
			set
			{
				if (isSelected != value)
				{
					isSelected = value;
					PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
					//                      PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
				}
			}
		}
		public bool IsVisible
		{
			get
			{
				return isVisible;
			}
			set
			{
				if (isVisible != value)
				{
					isVisible = value;
					PropertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
					//                      PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}
}
