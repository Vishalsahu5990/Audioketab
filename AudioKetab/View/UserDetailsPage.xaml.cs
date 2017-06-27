using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class UserDetailsPage : ContentPage
	{
		CircleImage profileImage = null;
		int height = 55;
		int width = 55;
		int x = 70;
		int y = 50;
		UserDetailsModel _list = null;
		MainPage _context = null;
		int _userid = 0;
		bool isFollowing = false;
		string _profilePic = string.Empty; 
		public UserDetailsPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			GetPlayList();

			flowlistview.FlowColumnMinWidth = App.ScreenWidth / 3;

		}
		public UserDetailsPage(int userid, MainPage context)
		{
			_context = context;
			_userid = userid;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			GetPlayList();

			flowlistview.FlowColumnMinWidth = App.ScreenWidth / 3;


		}
		public UserDetailsPage(int userid, MainPage context, string profilePic)
		{
			_context = context;
			_userid = userid;
			_profilePic = profilePic;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			GetPlayList();

			flowlistview.FlowColumnMinWidth = App.ScreenWidth / 3;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			flowlistview.FlowItemTapped-= Flowlistview_FlowItemTapped;
		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
				Book_summariesModel _model = new Book_summariesModel();
				var item = e.Item as AudioList;
                _model.user_id = item.user_id;
				_model.s_id = item.s_id;
				_model.song_path = item.song_path;

				_model.image_path = item.image_path;
				_model.book_name = item.book_name;
				_model.author_name = item.author_name;
				_model.count_like = item.count_like;
					_model.comment = item.comment;
				if(string.IsNullOrEmpty(_profilePic))
				await Navigation.PushModalAsync(new AudioPlayerPage(StaticDataModel.CurrentContext, _model));
				else
					await Navigation.PushModalAsync(new AudioPlayerPage(StaticDataModel.CurrentContext, _model,_profilePic));	
			}
			catch (Exception ex)
			{

			}
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

				if (string.IsNullOrEmpty(_profilePic))
				{
					var profilepic = CrossSecureStorage.Current.GetValue("profilePic", null);
					profileImage.Source = Constants.PRO_PIC_IMG_URL + profilepic;
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
				var ret = WebService.CheckFollow(Convert.ToInt32(_userid));
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
				SetData();
			}
			catch (Exception ex)
			{

			}
		}
		private void SetData()
		{
			try
			{
				

				var model = StaticMethods.GetLocalSavedData();
				if (model != null)
				{
					lblUsername.Text = _list.user_info[0].first_name + " " + _list.user_info[0].last_name;
					lblDesc.Text = _list.user_info[0].description;
				}
				lblFollower_count.Text = _list.follower_count.ToString();
				lblFollowing_count.Text = _list.following_count.ToString();
				lblUploadedAudio_count.Text = _list.myaudio_count.ToString();
			}
			catch (Exception ex)
			{

			}
		}
		async void Audio_Tapped(object sender, System.EventArgs e)
		{
			//await Navigation.PushModalAsync(new AudioRecordingPage(_context));
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
				await Navigation.PushModalAsync(new UploadedAudioPage(Convert.ToInt32( _userid)));
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

		private void GetPlayList()
		{
			List<PlaylistModel> playlist = null;
			string ret = string.Empty;
			StaticMethods.ShowLoader();

						_list = WebService.GetUserDetails(_userid);
					
						if (_list != null)
						{


							for (int i = 0; i < _list.audio_list.Count; i++)
							{
								_list.audio_list[i].image_path = Constants.SERVER_IMG_URL + _list.audio_list[i].image_path;
							}
							flowlistview.FlowItemsSource = _list.audio_list;
							Device.BeginInvokeOnMainThread(() =>
							{
								PrepareUI();
							});

						}
						else
						{
							StaticMethods.ShowToast("No post found!");
						}


					
		}

	}
}
