using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class PlaylistPage : ContentPage
	{
		List<AudioModel> playlist = null;
		MainPage _context = null;
		string selectedCategoryId = string.Empty;
		PlaylistItems items;
		public List<WrappedSelection<Playlist>> _WrappedItems;
		public PlaylistPage()
		{

			InitializeComponent();

		}
		public PlaylistPage(MainPage context)
		{
			_context = context;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			GetPlayList();
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;
			flowlistview.FlowItemTapped += Flowlistview_FlowItemTapped;
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			UpdatePlaylistPopup.SaveChanged+= UpdatePlaylistPopup_SaveChanged1;
		}
		protected async override void OnDisappearing()
		{
			base.OnDisappearing();
			UpdatePlaylistPopup.SaveChanged -= UpdatePlaylistPopup_SaveChanged1;
		}

		void UpdatePlaylistPopup_SaveChanged1(object sender, string e)
		{
			//Playlist listitem = (from itm in items.Items where itm.playlist_ctegoryid == selectedCategoryId select itm).FirstOrDefault<Playlist>();      
			Device.BeginInvokeOnMainThread(() =>
			{ 
            var item = items.Items.FirstOrDefault(i => i.playlist_ctegoryid == selectedCategoryId);
			if (item != null)
			{
				item.playlist_category = e;
			}
			});

			UpdatePlaylistCategory(e, Convert.ToInt32(selectedCategoryId)).Wait();
		}



		async void Playlist_Tapped(object sender, System.EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{


			}
		}
		async void Back_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PopModalAsync();
			}
			catch (Exception ex)
			{


			}
		}

		void Edit_Clicked(object sender, System.EventArgs e)
		{
			var item = (Xamarin.Forms.Button)sender;
 selectedCategoryId = item.CommandParameter.ToString();
			string categoryName = string.Empty;
			for (int i = 0; i < WebService.playlistModel.playlist.Count; i++)
			{
				if (WebService.playlistModel.playlist[i].playlist_ctegoryid == selectedCategoryId)
				{
					categoryName = WebService.playlistModel.playlist[i].playlist_category;
					break;
				}
			}
			UpdatePlaylistPopup ups = new UpdatePlaylistPopup(categoryName);
			Navigation.PushPopupAsync(ups);
		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
				//Book_summariesModel _model = new Book_summariesModel();
				var item = e.Item as Playlist;
				//_model.user_id = item.user_id;
				//_model.s_id = item.song_id;
				//_model.song_path = item.song_path;

				//_model.image_path = item.image_path;
				//_model.book_name = item.book_name;
				//_model.author_name = item.author_name;
				//_model.count_like = item.count_like;
				//_model.article_url = item.article_url;
				//_model.video_url = item.video_url;
				await Navigation.PushModalAsync(new PlaylistByCategoryIdPage(item));
			}
			catch (Exception ex)
			{

			}
		}

		private async Task GetPlayList()
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						playlist = WebService.GetPlaylist();
					}).ContinueWith(
					t =>
					{
						if (playlist != null)
						{
							//for (int i = 0; i < playlist.Count; i++)
							//{
							//	playlist[i].image_path = Constants.SERVER_IMG_URL + playlist[i].image_path;
							//}
							//flowlistview.FlowItemsSource =playlist;
							if (WebService.playlistModel != null)
							{
								items = new PlaylistItems(WebService.playlistModel.playlist);
						        flowlistview.FlowItemsSource = items.Items;
							}
						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task UpdatePlaylistCategory(string name,int id)
		{

			string ret = string.Empty;

			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				ret = WebService.UpdatePlaylistCatefory(id,name);
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{
							StaticMethods.ShowToast("Playlist updated successfully");
							GetPlayList().Wait();
						}
						else
						{ 
				StaticMethods.ShowToast("failed to updated Playlist");
				}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}
}
