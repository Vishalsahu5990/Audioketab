using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AudioKetab
{
public class AudioPlayItemList : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<AudioModel> _items;

	public ObservableCollection<AudioModel> Items
	{
		get { return _items; }
		set { _items = value; OnPropertyChanged("Items"); }
	}

	protected virtual void OnPropertyChanged(string propertyName)
	{
		if (PropertyChanged == null)
			return;
		PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

public AudioPlayItemList(List<AudioModel> itemList)
	{
		Items = new ObservableCollection<AudioModel>();
		foreach (AudioModel itm in itemList)
	{
		Items.Add(itm);
	}
}	}
}
