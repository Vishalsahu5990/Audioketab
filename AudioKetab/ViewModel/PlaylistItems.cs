using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace AudioKetab
{
	public class PlaylistItems : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<Playlist> _items;

		public ObservableCollection<Playlist> Items
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

		public PlaylistItems(List<Playlist> itemList)
		{
			Items = new ObservableCollection<Playlist>();
			foreach (Playlist itm in itemList)
			{
				Items.Add(itm);
			}
		}
	}
}
