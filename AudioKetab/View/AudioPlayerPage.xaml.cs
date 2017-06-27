using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using Plugin.SecureStorage;
using Plugin.SimpleAudioPlayer.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class AudioPlayerPage : ContentPage
	{
		
		CircleImage profileImage = null;
		int height = 55;
		int width = 55;
		int x = 70;
		int y = 50;
		private IPlaybackController PlaybackController => CrossMediaManager.Current.PlaybackController;
		MainPage _context;
		Book_summariesModel _model;
		public static int isLiked = 0;
		public int _playlistStatus = 0;
		bool isFollowing = false;
		UserDetailsModel _list = null;
		string _profilePic = string.Empty;
		public AudioPlayerPage()
		{
			InitializeComponent();
		}
public AudioPlayerPage(MainPage context, Book_summariesModel book_summeriesModel, string profile_pic)
{
	InitializeComponent();

			_profilePic = profile_pic;
			_model = book_summeriesModel;
			_context = context;
            InitializeComponent();
NavigationPage.SetHasNavigationBar(this, false);
CrossMediaManager.Current.PlayingChanged += Current_PlayingChanged;
GetUserDetails();
SetData();
InitControls();
getLikeStatus();
PrepareUI();
		}
		public AudioPlayerPage(MainPage context, Book_summariesModel book_summeriesModel)
		{
			_model = book_summeriesModel;
			_context = context;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			CrossMediaManager.Current.PlayingChanged += Current_PlayingChanged;
			GetUserDetails();
			SetData();
			InitControls();
			getLikeStatus();
			PrepareUI();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();


			await CrossMediaManager.Current.Play(Constants.SERVER_IMG_URL + _model.song_path);
			//CheckFollow(Convert.ToInt32( _model.u_id)).Wait();

		}
		protected async override void OnDisappearing()
		{
			base.OnDisappearing();
			await CrossMediaManager.Current.Stop();
		}
		public void PrepareHeaderView()
		{
			try
			{  
				var w = App.ScreenWidth;
				var h = App.ScreenHeight;
				if (w > 350) 
				{
					imgMic.Margin = new Thickness(8, 0, 0, 0);
					_slHeaderIcons.Spacing = 10;
				}
				if (StaticMethods.IsIpad())
				{ 
					_slMsgNotification.Spacing = 30;
					 
					height = 80;
					width = 80;
					x = 100;
					y = 30;
					_slHeaderIcons.Spacing = 25;
					lblUsername.FontSize = 22;
					lblDesc.FontSize = 18;
					lblFollower_count.FontSize = 16;
					lblPlaylist_count.FontSize = 16;
					lblFollowing_count.FontSize = 16;
					lblUploadedAudio_count.FontSize = 16;
				}
				var centerX = Constraint.RelativeToParent(parent => parent.Width - x);
				var centerY = Constraint.RelativeToParent(parent => y);
				profileImage = new CircleImage
				{ 
					HeightRequest = height,
					WidthRequest = width,
					BorderColor = Color.White,
					BorderThickness = 2,
					Aspect = Aspect.AspectFill,
					Source = "defaultprofile.png"
				};
				_MainRelativeLayout.Children.Add(profileImage, centerX, centerY);


			}
			catch (Exception ex)
			{

			}
		}
		private void PrepareUI()
		{
			try 
			{
				
				PrepareHeaderView();
				if (App.ScreenWidth == 320)
				{
					imgIcList.HeightRequest = 25;
					imgIcList.WidthRequest = 30;
				}
				//var profilepic = CrossSecureStorage.Current.GetValue("profilePic", null);
				if (string.IsNullOrEmpty(_profilePic))
				{
					if (!string.IsNullOrEmpty(_model.profile_pic))
						profileImage.Source = Constants.PRO_PIC_IMG_URL + _model.profile_pic;
					else
						profileImage.Source = "defaultprofile.png";
				}
				else
				{
					profileImage.Source = _profilePic;
				}

				var model = StaticMethods.GetLocalSavedData();
				if (model != null)
				{
					lblUsername.Text = model.first_name + " " + model.last_name;
					lblDesc.Text = model.description;
				}
var ret = WebService.CheckFollow(Convert.ToInt32(_model.u_id));
			if (ret == "follow")
			{
				imgMic.Source = "follow"; 
				isFollowing = true;

			}
			else
			{
				imgMic.Source = "unfollow";
				isFollowing = false;
			}
			}
			catch (Exception ex)
			{

			}
		}
		void InitControls()
		{
			CrossMediaManager.Current.Play(Constants.SERVER_IMG_URL + _model.song_path);
			btnPlay.Clicked += BtnPlayClicked;
			btnPause.Clicked += BtnPause_Clicked;
			sliderPosition.ValueChanged += SliderPostionValueChanged;
			sliderPosition.IsEnabled = true;

		}
		private void SetData()
		{
			try
			{
				imgAlbum.HeightRequest = App.ScreenWidth /2;
				imgAlbum.WidthRequest = App.ScreenWidth /2;
				imgAlbum.Source = _model.image_path;
				if (_model != null)
				{

					lblLikeCount.Text = _model.count_like;
					if (_model.count_comment != null)
						lblCommentCount.Text = _model.count_comment;
					else
						lblCommentCount.Text = "0";

					if (_model.count_share != null)
						lblShareCount.Text = _model.count_share;
					else
						lblShareCount.Text = "0";

					if (_model.count_playlist != null)
						lblListCount.Text = _model.count_playlist;
					else
						lblListCount.Text = "0";


				} 

				if (string.IsNullOrEmpty(_model.video_url))
					imgVideourl.IsVisible = false;

				if (string.IsNullOrEmpty(_model.article_url))
					imgArticleurl.IsVisible = false;
			}
			catch (Exception ex)
			{
				lblCommentCount.Text = "0";
					lblShareCount.Text = "0";
			}
		}
		private async void BtnPlayClicked(object sender, EventArgs e)
		{
			try
			{

				await CrossMediaManager.Current.Play();
				btnPlay.IsVisible = false;
				btnPause.IsVisible = true;

				//if (player != null)
				//{
				//	player.Play();
				//sliderPosition.Maximum = CrossMediaManager.Current.AudioPlayer.Duration.TotalHours;
				//sliderPosition.IsEnabled = true;
				//	Device.StartTimer(TimeSpan.FromSeconds(0.5), UpdatePosition);
				//}
			}
			catch (Exception ex)
			{
				StaticMethods.ShowToast("Something went wrong, Please trye again later!");
			}

		}

		async void BtnPause_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine(CrossMediaManager.Current.AudioPlayer.Status);
			await CrossMediaManager.Current.Pause();
			btnPlay.IsVisible = true;
			btnPause.IsVisible = false;
		}
		async void Album_Tapped(object sender, EventArgs e)
		{
			AlbumInfoPopup s = new AlbumInfoPopup(_model.book_name,_model.author_name,_model.comment);
            Navigation.PushPopupAsync(s);	
		}

		void Current_PlayingChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.PlayingChangedEventArgs e)
		{
			var _duration = e.Duration;
			var _position = e.Position;
			lblDuration.Text = _duration.Hours + ":" + _duration.Minutes + ":" + _duration.Seconds;
			lblPosition.Text = _position.Hours + ":" + _position.Minutes + ":" + _position.Seconds;
			sliderPosition.Value = e.Progress;
			if (lblDuration.Text == lblPosition.Text)
			{
				btnPlay.IsVisible = true;
				btnPause.IsVisible = false;
			}

		}

		private void SliderPostionValueChanged(object sender, ValueChangedEventArgs e)
		{
			if (sliderPosition.Value != CrossMediaManager.Current.AudioPlayer.Duration.TotalHours)
				sliderPosition.Value = sliderPosition.Value;
		}
		bool UpdatePosition()
		{
			lblPosition.Text = CrossMediaManager.Current.AudioPlayer.Position.TotalHours.ToString(@"hh\:mm");
			lblDuration.Text = CrossMediaManager.Current.AudioPlayer.Duration.TotalHours.ToString(@"hh\:mm");
			sliderPosition.ValueChanged -= SliderPostionValueChanged;
			sliderPosition.Value = CrossMediaManager.Current.AudioPlayer.Duration.TotalHours;
			sliderPosition.ValueChanged += SliderPostionValueChanged;

			return true;
		}


		async void menu_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				Navigation.PopModalAsync();

			}
			catch (Exception ex)
			{


			}
		}
		async void Playlist_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new PlaylistPage(_context));
			}
			catch (Exception ex)
			{


			}
		}
		async void Followers_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new FollowersPage());
			}
			catch (Exception ex)
			{


			}
		}
		async void Following_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new FollowingPage());
			}
			catch (Exception ex)
			{


			}
		}
		async void uplodedaudio_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new UploadedAudioPage(Convert.ToInt32( _model.u_id)));
			}
			catch (Exception ex)
			{


			}
		}
		async void messages_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new ChatUsersPage());
			}
			catch (Exception ex)
			{


			}
		}
		async void notification_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new NotificationPage());
			}
			catch (Exception ex)
			{


			}
		}
		 void Audio_Tapped(object sender, System.EventArgs e)
		{
			//unFollowUser(Convert.ToInt32(_model.u_id)).Wait();
					}
async void videourl_tapped(object sender, System.EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(_model.video_url))
					Device.OpenUri(new System.Uri(_model.video_url));

			}
			catch (Exception ex)
			{

			}

		}

		async void Articleurl_tapped(object sender, System.EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(_model.article_url))
					Device.OpenUri(new System.Uri(_model.article_url));
			}
			catch (Exception ex)
			{

			}
		}
		async void like_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				if (isLiked == 0)
				{
					imgLike.Source = "like.png";
					LikeAudio(1, Convert.ToInt32(_model.user_id));

				}
				else
				{
					imgLike.Source = "unlike.png";
					LikeAudio(0, Convert.ToInt32(_model.user_id));
				}
			}
			catch (Exception ex)
			{


			}
		}
		async void comment_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PushModalAsync(new CommentPage(Convert.ToInt32(_model.s_id)));
			}
			catch (Exception ex)
			{


			}
		}
		async void mute_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				DisplayAlert("Not supported!", "Unable to mute device", "Ok");
			}
			catch (Exception ex)
			{


			}
		}
		async void share_Tapped(object sender, System.EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{


			}
		}
		async void addplaylist_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				if (_playlistStatus == 0)
				{
					AddtoPlaylistPopup s = new AddtoPlaylistPopup(Convert.ToInt32(_model.s_id));
					Navigation.PushPopupAsync(s);
				}

				else
				{
					StaticMethods.ShowToast("Already added to playlist.");
				}

			}
			catch (Exception ex)
			{


			}
		}
		private async Task getLikeStatus()
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						ret = WebService.GetLikeStatus(Convert.ToInt32(_model.s_id));
						var data = ret.Split(',');
						if (!string.IsNullOrEmpty(data[1]))
							_playlistStatus = Convert.ToInt32(data[1]);

					}).ContinueWith(
					t =>
					{
						if (ret != "0")
						{

							isLiked = Convert.ToInt32(ret);
						}
						else
						{
							isLiked = 0;

						}


					}, TaskScheduler.FromCurrentSynchronizationContext()

				);
		}
		private async Task LikeAudio(int status, int song_userid)
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						ret = WebService.LikeAudio(Convert.ToInt32(_model.s_id), status, song_userid);
					}).ContinueWith(
					t =>
					{
						Device.BeginInvokeOnMainThread(() =>
				{
					if (ret == "success")
					{
						if (isLiked == 0)
						{
							isLiked = 1;
							var count = Convert.ToInt32(_model.count_like) + 1;
							_model.count_like = count.ToString();
							StaticMethods.ShowToast("Audio liked successfully.");
						}
						else
						{
							isLiked = 0;
							var count = Convert.ToInt32(_model.count_like) + -1;
							_model.count_like = count.ToString();
							StaticMethods.ShowToast("Audio unliked successfully.");
						}


						lblLikeCount.Text = _model.count_like;
					}


				});

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
private async Task CheckFollow(int follow_userid)
{

	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				ret = WebService.CheckFollow(follow_userid);
			}).ContinueWith(
			t =>
			{
				Device.BeginInvokeOnMainThread(() => 
		{
			if (ret == "follow")
			{
				imgMic.Source = "follow";
				isFollowing = true;

			}
			else
			{
				imgMic.Source = "unfollow";
				isFollowing = false;
			}


		});

			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}

private async Task unFollowUser(int follow_userid)
{

	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				if (isFollowing)
				ret = WebService.unFollowUser(follow_userid);
				else
					ret = WebService.followUser(follow_userid);	
			}).ContinueWith(
			t =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{ 
				
				if (ret == "success")
				{
					if (isFollowing)
					{
						imgMic.Source = "unfollow";
						isFollowing = false;

					}
					else
					{
						imgMic.Source = "follow";
						isFollowing = true;
					}



				}
				
				});

			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
private void GetUserDetails()
{
	List<PlaylistModel> playlist = null;
	string ret = string.Empty;
	StaticMethods.ShowLoader();

			_list = WebService.GetUserDetails(Convert.ToInt32( _model.u_id));

	if (_list != null)
	{


		
		Device.BeginInvokeOnMainThread(() =>
		{
			    lblFollower_count.Text = _list.follower_count.ToString();
				lblFollowing_count.Text = _list.following_count.ToString();
				lblUploadedAudio_count.Text = _list.myaudio_count.ToString();
		});

	}
	else
	{
		StaticMethods.ShowToast("No post found!");
	}

		}
	}
}
