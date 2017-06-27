using System;
using System.ComponentModel;

namespace AudioKetab
{
	public class ReadingMentorsViewModel<Reading_mentorModel> : INotifyPropertyChanged
{
public Reading_mentorModel Item { get; set; }
	bool isSelected = false;
	public bool IsVisible
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
				PropertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
				//						PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
			}
		}
	}
	public event PropertyChangedEventHandler PropertyChanged = delegate { };	}
}
