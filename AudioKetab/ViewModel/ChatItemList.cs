using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace AudioKetab
{
	public class ChatItemList: INotifyPropertyChanged
	{
public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<ChatModel> _items;

public ObservableCollection<ChatModel> Items
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

public ChatItemList(List<ChatModel> itemList)
{
	Items = new ObservableCollection<ChatModel>();
	foreach (ChatModel itm in itemList)
	{
		Items.Add(itm);
	}
}	}
}
