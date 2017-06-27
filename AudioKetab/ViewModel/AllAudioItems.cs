using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace AudioKetab
{
public class AllAudioItems : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
	public ObservableCollection<AllAudioModel> _items;

	public ObservableCollection<AllAudioModel> Items
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

public AllAudioItems(List<AllAudioModel> itemList)
	{
		Items = new ObservableCollection<AllAudioModel>();
		foreach (AllAudioModel itm in itemList)
	{
		Items.Add(itm);
	}
}	}
}

