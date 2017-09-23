using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioKetab
{
	public class BindingSourceModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<Book_summariesModel> _items;

		public ObservableCollection<Book_summariesModel> Items
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
		public BindingSourceModel(List<Book_summariesModel> lbm)
		{

			// Here you can have your data form db or something else,
			// some data that you already have to put in the list
			Items = new ObservableCollection<Book_summariesModel>();
			foreach (Book_summariesModel itm in lbm)
			{
				Items.Add(itm);
			}

		}
		public BindingSourceModel()
		{



		}



	}
public class Categories
{
	public int Id { get; set; }

	public string Name { get; set; }

	public List<CategoryItems> CategoryItem { get; set; }
}

public class CategoryItems
{
	public int Id { get; set; }

	public string ItemName { get; set; }

	public int CatId { get; set; }

	public string ImageUrl { get; set; }	}
}

