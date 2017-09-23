using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace AudioKetab
{
	public class WrappedEditing<Playlist> : INotifyPropertyChanged
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

		public WrappedEditing(List<Playlist> itemList)
		{
			Items = new ObservableCollection<Playlist>();
			foreach (Playlist itm in itemList)
			{
				Items.Add(itm);
			}
		}
		//Resion start
		public Playlist Item { get; set; }
		string playlist_category = string.Empty;

		public string PlaylistCategory
		{
			get
			{
				return playlist_category;
			}
			set
			{
				if (playlist_category != value)
				{
					playlist_category = value;
					PropertyChangeds(this, new PropertyChangedEventArgs("PlaylistCategory"));
					//                      PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChangeds = delegate { };

		//Resion end
	}
}
