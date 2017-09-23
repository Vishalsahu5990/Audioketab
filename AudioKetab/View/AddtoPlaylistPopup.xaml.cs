using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace AudioKetab
{
	public partial class AddtoPlaylistPopup  : PopupPage
	{
		List<AddtoPlaylistModel> _list = null;
		public List<WrappedSelection<AddtoPlaylistModel>> _WrappedItems;
		public WrappedSelection<AddtoPlaylistModel> o;
		int _sid = 0;
		public AddtoPlaylistPopup()
		{

			InitializeComponent();



		}
		public AddtoPlaylistPopup(int s_id)
		{
			_sid = s_id;
			InitializeComponent();


			getAlreadyinPlaylist().Wait();
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
			btnSend.Clicked+= BtnSend_Clicked;
		}
		async void close_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PopPopupAsync();
			}
			catch (Exception ex)
			{


			}
		}

	async	void BtnSend_Clicked(object sender, EventArgs e)
		{
			if (o != null || !string.IsNullOrEmpty(txtplaylistname.Text))
			{
				if (o == null)
				{
					addToPlaylist(txtplaylistname.Text, _sid, 1);
				}
				else
				{ 
					addToPlaylist(o.Item.playlist_ctegoryid, _sid, 0);
				}
			}
			await Navigation.PopPopupAsync();

		}

		private async Task getAlreadyinPlaylist()
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				_list = WebService.getAlradyInPlaylist();
					}).ContinueWith(
					t =>
					{
						Device.BeginInvokeOnMainThread(() =>
				{
					if (_list != null)
					{
					 _WrappedItems = _list.Select(item => new WrappedSelection<AddtoPlaylistModel>() { Item = item, IsSelected = false }).ToList();

						flowlistview.FlowItemsSource = _WrappedItems;
					}


				});

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}

		private async Task addToPlaylist(string playlist,int sid,int status)
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				ret = WebService.AddToPlaylist(sid,playlist,status);
					}).ContinueWith(
					t =>
					{
						Device.BeginInvokeOnMainThread(() =>
				{
					if (ret == "success")
						StaticMethods.ShowToast("Successfully added to playlist.");


				});

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
				foreach (var wi in _WrappedItems)
				{
					wi.IsSelected = false;
					wi.IsVisible = true;
				}
				 o = (WrappedSelection<AddtoPlaylistModel>)e.Item;
				//var o = (WrappedSelection<List< AddtoPlaylistModel>>)e.Item;

				o.IsSelected = !o.IsSelected;
				o.IsVisible = !o.IsVisible;
			}
			catch (Exception ex)
			{

			}

		}
	}
}
