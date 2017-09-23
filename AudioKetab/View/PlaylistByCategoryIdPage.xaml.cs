using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class PlaylistByCategoryIdPage : ContentPage
	{
		int _categoryId = 0;
		Playlist audioList = null;
		AudioPlayItemList items;
        List<AudioModel> playlist = null;
		public PlaylistByCategoryIdPage()
		{
			InitializeComponent();
		}
		public PlaylistByCategoryIdPage(Playlist audio_list)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			audioList = audio_list;
			flowlistview.FlowColumnMinWidth = App.ScreenWidth/3;
			flowlistview.FlowItemTapped += Flowlistview_FlowItemTapped;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			for (int i = 0; i < audioList.audio.Count; i++)
			{
				audioList.audio[i].image_path = Constants.SERVER_IMG_URL + audioList.audio[i].image_path;
			}
			items = new AudioPlayItemList(audioList.audio);
			flowlistview.FlowItemsSource = items.Items;
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

void Delete_Clicked(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () => { 
			
var item = (Xamarin.Forms.Button)sender;
var playlist_id = item.CommandParameter.ToString();
AudioModel listitem = (from itm in items.Items where itm.playlist_id == item.CommandParameter select itm).FirstOrDefault<AudioModel>();
items.Items.Remove(listitem);
				DeletePlaylist(Convert.ToInt32( listitem.playlist_id)).Wait();
			});
			
		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
				Book_summariesModel _model = new Book_summariesModel();
				var item = e.Item as AudioModel;
				_model.user_id = item.user_id;
				_model.s_id = item.song_id;
				_model.song_path = item.song_path;

				_model.image_path = item.image_path;
				_model.book_name = item.book_name;
				_model.author_name = item.author_name;
				_model.count_like = item.count_like;
				_model.article_url = item.article_url;
				_model.video_url = item.video_url;
					_model.comment = item.comment;
				await Navigation.PushModalAsync(new AudioPlayerPage(StaticDataModel.CurrentContext, _model));
			}
			catch (Exception ex)
			{

			}
		}

private async Task DeletePlaylist( int id)
{

	string ret = string.Empty;

	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				ret = WebService.RemoveFromPlaylist(id);
			}).ContinueWith(
			t =>
			{
				if (ret == "success")
				{
					StaticMethods.ShowToast("Deleted successfully");
					GetPlayList().Wait();
				}
				else
				{
					StaticMethods.ShowToast("failed to delete Playlist");
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
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
						;
						for (int i = 0; i< WebService.playlistModel.playlist.Count ;i++)
						{
							if (WebService.playlistModel.playlist[i].playlist_ctegoryid == audioList.playlist_ctegoryid)
							{
								audioList.audio = WebService.playlistModel.playlist[i].audio;
							}


							for (int j = 0; j<audioList.audio.Count; j++)
			{
				audioList.audio[j].image_path = Constants.SERVER_IMG_URL + audioList.audio[j].image_path;
			}
			items = new AudioPlayItemList(audioList.audio);
flowlistview.FlowItemsSource = items.Items;
						}

					}
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
