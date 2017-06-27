using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SlideOverKit;
using Xamarin.Forms;
using Plugin.SecureStorage.Abstractions;
using Plugin.SecureStorage;
using ImageCircle.Forms.Plugin.Abstractions;
using System.Collections.ObjectModel;

namespace AudioKetab
{
	public partial class HomePage : ContentPage
	{
		
       CategoryViewModel viewModel;
		CircleImage profileImage = null;
		int height = 55;
		int width = 55;
		int x = 70;
		int y = 50;
		Book_summariesModel _model;
		ControlTemplate headerTemplate;
		MainPage _context;
		public ICommand OpenMenuCommand { get; private set; }

		List<Reading_mentorModel> list_ReadingMentor = null;
		List<Book_summariesModel> list_BookSummeries = null;
		List<LectureandTraingingModel> list_LectureTraining = null;
		List<NewsLetterModel> list_NewsLetter = null;
		private double cellSize = 0;

		async void mentor_clicked(object sender, System.EventArgs e)
		{
			try
			{
var item = (Xamarin.Forms.Button)sender;
			for (int i = 0; i<list_ReadingMentor.Count; i++)
			{
				if (item.CommandParameter.ToString() == list_ReadingMentor[i].u_id)
				{ 
	await Navigation.PushModalAsync(new UserDetailsPage(Convert.ToInt32(list_ReadingMentor[i].u_id), _context,list_ReadingMentor[i].profile_pic));

				}
			}
			}
			catch (Exception ex)
			{

			}
		}
		async void booksummeries_clicked(object sender, System.EventArgs e)
		{
			try
			{
				var item = (Xamarin.Forms.Button)sender;
				for (int i = 0; i < list_BookSummeries.Count; i++)
				{
					if (item.CommandParameter.ToString() == list_BookSummeries[i].s_id)
					{
						await Navigation.PushModalAsync(new AudioPlayerPage(_context, list_BookSummeries[i]));
					}
				}
			}
			catch (Exception ex)
			{

			}


		}
		async void lecture_clicked(object sender, System.EventArgs e)
		{
 try
				 {

var item1 = (Xamarin.Forms.Button)sender;
				for (int i = 0; i<list_LectureTraining.Count; i++)
			{
					if (item1.CommandParameter.ToString() == list_LectureTraining[i].s_id)
				{ 
						 _model = new Book_summariesModel();
var item = list_LectureTraining[i];
_model.u_id = item.u_id;
					 _model.s_id = item.s_id;
					 _model.song_path = item.song_path;
					 _model.profile_pic = item.profile_pic;
					 _model.image_path = item.image_path;
					 _model.book_name = item.book_name;
					 _model.author_name = item.author_name;
					 _model.count_like = item.count_like;
					 _model.first_name = item.first_name;
					 _model.last_name = item.last_name;
					 _model.article_url = item.article_url;
					 _model.video_url = item.video_url;
					 _model.comment = item.comment;
					 await Navigation.PushModalAsync(new AudioPlayerPage(_context, _model));
				}
			}

					
				 }
				 catch (Exception ex)
				 {

				 }
		}
async void newsletter_clicked(object sender, System.EventArgs e)
		{
			 try
				 {
                   var item1 = (Xamarin.Forms.Button)sender;
				for (int i = 0; i < list_NewsLetter.Count; i++)
				{
					if (item1.CommandParameter.ToString() == list_NewsLetter[i].s_id)
					{
						 _model = new Book_summariesModel();
var item =list_NewsLetter[i];
_model.u_id = item.u_id;
					 _model.s_id = item.s_id;
					 _model.song_path = item.song_path;
					 _model.profile_pic = item.profile_pic;
					 _model.image_path = item.image_path;
					 _model.book_name = item.book_name;
					 _model.author_name = item.author_name;
					 _model.count_like = item.count_like;
					 _model.first_name = item.first_name;
					 _model.last_name = item.last_name;
					 _model.article_url = item.article_url;
					 _model.video_url = item.video_url;
					 _model.comment = item.comment;
					 await Navigation.PushModalAsync(new AudioPlayerPage(_context, _model));
					}
				}
					
				 }
				 catch (Exception ex)
				 {

				 }

		}
		public HomePage()
		{
			
			InitializeComponent();

		}
		public HomePage(MainPage context)
		{
			_context = context;
			_context.IsGestureEnabled = false;
			InitializeComponent();
            viewModel = new CategoryViewModel();
			 this.BindingContext = viewModel;
			NavigationPage.SetHasNavigationBar(this, false);
			PrepareUI();

			//var ret = WebService.GetHomePageAudio();

			//if (ret == "success")
			//{
			//	ProcessResult();

			//}

			//listview_mentor.ItemSelected += async (sender, e) =>
			//  {
			//	var item = e.SelectedItem as Reading_mentorModel;
			//	await Navigation.PushModalAsync(new UserDetailsPage(Convert.ToInt32(item.u_id), _context,item.profile_pic));

			//  };

			//listview_booksummeries.ItemSelected += async (sender, e) =>
			// {
			//	var item = e.Item as Book_summariesModel;
			//	 await Navigation.PushModalAsync(new AudioPlayerPage(_context, item));
			// };
			//lecture_training.ItemSelected += async (sender, e) =>
			// {
			//	 try
			//	 {
			//		 _model = new Book_summariesModel();
			//		 var item = e.Item as LectureandTraingingModel;

			//		 _model.u_id = item.u_id;
			//		 _model.s_id = item.s_id;
			//		 _model.song_path = item.song_path;
			//		 _model.profile_pic = item.profile_pic;
			//		 _model.image_path = item.image_path;
			//		 _model.book_name = item.book_name;
			//		 _model.author_name = item.author_name;
			//		 _model.count_like = item.count_like;
			//		 _model.first_name = item.first_name;
			//		 _model.last_name = item.last_name;
			//		 _model.article_url = item.article_url;
			//		 _model.video_url = item.video_url;
			//		 _model.comment = item.comment;

			//		 await Navigation.PushModalAsync(new AudioPlayerPage(_context, _model));
			//	 }
			//	 catch (Exception ex)
			//	 {

			//	 }
			// };
			//news_letters.ItemSelected += async (sender, e) =>
			// {
			//	 try
			//	 {
			//		 _model = new Book_summariesModel();
			//		 var item = e.Item as NewsLetterModel;
			//		 _model.u_id = item.u_id;
			//		 _model.s_id = item.s_id;
			//		 _model.song_path = item.song_path;
			//		 _model.profile_pic = item.profile_pic;
			//		 _model.image_path = item.image_path;
			//		 _model.book_name = item.book_name;
			//		 _model.author_name = item.author_name;
			//		 _model.count_like = item.count_like;
			//		 _model.first_name = item.first_name;
			//		 _model.last_name = item.last_name;
			//		 _model.article_url = item.article_url;
			//		 _model.video_url = item.video_url;
			//		 _model.comment = item.comment;
			//		 await Navigation.PushModalAsync(new AudioPlayerPage(_context, _model));
			//	 }
			//	 catch (Exception ex)
			//	 {

			//	 }
			// };
			var model = WebService.GetAll_Counts();
			if (model != null)
			{
				lblPlaylist_count.Text = model.playlist_count.ToString();
				lblFollower_count.Text = model.follower_count.ToString();
				lblFollowing_count.Text = model.following_count.ToString();
				lblUploadedAudio_count.Text = model.myaudio_count.ToString();
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
				cellSize = App.ScreenWidth / 4.3;
				//imgMic.HeightRequest = App.ScreenWidth / 4;
				//imgMic.WidthRequest = App.ScreenWidth / 4;
				var profilepic = CrossSecureStorage.Current.GetValue("profilePic", null);
				profileImage.Source = Constants.PRO_PIC_IMG_URL + profilepic;
				var model = StaticMethods.GetLocalSavedData();
				if (model != null)
				{
					lblUsername.Text = model.first_name + " " + model.last_name;
					lblDesc.Text = model.description;
				}
			}
			catch (Exception ex)
			{

			}
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			StaticDataModel.IsFromHomePage = true;
			// GetCounts().Wait();


			//GetAudio().Wait();


		}
private async Task GetAudio()
{
	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				ret = WebService.GetHomePageAudio();

			}).ContinueWith(
			t =>
			{
				if (ret == "success")
				{
					Device.BeginInvokeOnMainThread(() => 
			{
				ProcessResult();

				
			});




				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
		protected override void OnDisappearing()
		{
			base.OnAppearing();
			StaticDataModel.IsFromHomePage = false;

		}

async void Tapped(object sender, System.EventArgs e)
{
	
		}
		async void menu_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				_context.MenuTapped.Execute(_context.MenuTapped);

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
				if (lblUploadedAudio_count.Text != "0")
					await Navigation.PushModalAsync(new UploadedAudioPage(StaticDataModel.UserId));
				else
					DisplayAlert("Message","you not uploaded any audio yet.","OK");
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


		async void Audio_Tapped(object sender, System.EventArgs e)
		{
			await Navigation.PushModalAsync(new AudioRecordingPage(_context));
		}


		async void MoreMentors_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PushModalAsync(new More_ReadingMentorPage(_context));
		}
		async void MoreBookSummeries_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PushModalAsync(new More_BookSummeriesPage(_context));
		}
		void MoreLectures_Clicked(object sender, System.EventArgs e)
		{
			Navigation.PushModalAsync(new More_LecturesTrainingPage());
		}
		async void MoreNewsletter_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PushModalAsync(new More_NewsLetterPage());
		}


		private void ProcessResult()
		{
			list_ReadingMentor = new List<Reading_mentorModel>();
			list_BookSummeries = new List<Book_summariesModel>();
			list_LectureTraining = new List<LectureandTraingingModel>();
			list_NewsLetter = new List<NewsLetterModel>();
			try
			{
				var _readingmentor = WebService.reading_mentor;
				var _booksummeries = WebService.book_summaries;
				var _lectureandTraining = WebService.lectureandtraining;
				var _newsLetter = WebService.newsletter;
				if (_readingmentor != null)
				{
					foreach (var item in _readingmentor)
					{
						list_ReadingMentor.Add(new Reading_mentorModel
						{
							u_id = item["u_id"].ToString(),
							first_name = item["first_name"].ToString(),
							last_name = item["last_name"].ToString(),
							user_name = item["user_name"].ToString(),
							country_name = item["country_name"].ToString(),
							mobile_no = item["mobile_no"].ToString(),
							email = item["email"].ToString(),
							password = item["password"].ToString(),
							status = item["status"].ToString(),
							dob = item["dob"].ToString(),
							from = item["from"].ToString(),
							profile_pic = Constants.PRO_PIC_IMG_URL + item["profile_pic"].ToString(),
							cover_pic = item["cover_pic"].ToString(),
							description = item["description"].ToString(),
							delete_status = item["delete_status"].ToString(),
							register_date = item["register_date"].ToString(),
							features_users = item["features_users"].ToString(),
							v_status = item["v_status"].ToString(),
							forgotpassword_status = item["forgotpassword_status"].ToString(),
							invite_status = item["invite_status"].ToString(),
							gmail_invite = item["gmail_invite"].ToString(),
							outlook_invite = item["outlook_invite"].ToString(),
							device_token = item["device_token"].ToString(),
							device_type = item["device_type"].ToString(),
							cell_size = cellSize

						});
					}




				}

				if (_booksummeries != null)
				{
					foreach (var item in _booksummeries)
					{
						list_BookSummeries.Add(new Book_summariesModel
						{
							s_id = item["s_id"].ToString(),
							user_id = item["user_id"].ToString(),
							typeof_audio = item["typeof_audio"].ToString(),
							book_name = item["book_name"].ToString(),
							author_name = item["author_name"].ToString(),
							category = item["category"].ToString(),
							comment = item["comment"].ToString(),
							song_path = item["song_path"].ToString(),
							image_path = Constants.SERVER_IMG_URL + item["image_path"].ToString(),
							count_like = item["count_like"].ToString(),
							u_id = item["u_id"].ToString(),
							profile_pic = item["profile_pic"].ToString(),
							last_name = item["last_name"].ToString(),
							first_name = item["first_name"].ToString(),
							delete_status = item["delete_status"].ToString(),
							article_url = item["article_url"].ToString(),
							video_url = item["video_url"].ToString(),
							description = item["description"].ToString(),
							cell_size = cellSize
						});
					}

				}
				if (_lectureandTraining != null)
				{
					foreach (var item in _lectureandTraining)
					{
						list_LectureTraining.Add(new LectureandTraingingModel
						{
							s_id = item["s_id"].ToString(),
							user_id = item["user_id"].ToString(),
							typeof_audio = item["typeof_audio"].ToString(),
							book_name = item["book_name"].ToString(),
							author_name = item["author_name"].ToString(),
							category = item["category"].ToString(),
							comment = item["comment"].ToString(),
							song_path = item["song_path"].ToString(),
							image_path = Constants.SERVER_IMG_URL + item["image_path"].ToString(),
							count_like = item["count_like"].ToString(),
							u_id = item["u_id"].ToString(),
							profile_pic = item["profile_pic"].ToString(),
							last_name = item["last_name"].ToString(),
							first_name = item["first_name"].ToString(),
							delete_status = item["delete_status"].ToString(),
							article_url = item["article_url"].ToString(),
							video_url = item["video_url"].ToString(),
							description = item["description"].ToString(),
							cell_size = cellSize
						});
					}

				}
				if (_newsLetter != null)
				{
					foreach (var item in _newsLetter)
					{
						list_NewsLetter.Add(new NewsLetterModel
						{
							s_id = item["s_id"].ToString(),
							user_id = item["user_id"].ToString(),
							typeof_audio = item["typeof_audio"].ToString(),
							book_name = item["book_name"].ToString(),
							author_name = item["author_name"].ToString(),
							category = item["category"].ToString(),
							comment = item["comment"].ToString(),
							song_path = item["song_path"].ToString(),
							image_path = Constants.SERVER_IMG_URL + item["image_path"].ToString(),
							count_like = item["count_like"].ToString(),
							u_id = item["u_id"].ToString(),
							profile_pic = item["profile_pic"].ToString(),
							last_name = item["last_name"].ToString(),
							first_name = item["first_name"].ToString(),
							delete_status = item["delete_status"].ToString(),
							article_url = item["article_url"].ToString(),
							video_url = item["video_url"].ToString(),
							cell_size = cellSize
						});
					}

				}


				listview_mentor.ItemsSource = list_ReadingMentor;
				listview_booksummeries.ItemsSource = list_BookSummeries;
				lecture_training.ItemsSource = list_LectureTraining;
				news_letters.ItemsSource = list_NewsLetter;

				//viewModel.List_LectureTraining = new ObservableCollection<LectureandTraingingModel>();
				foreach (LectureandTraingingModel itm in list_LectureTraining)
				{
					viewModel.List_LectureTraining.Add(itm);
				}

//viewModel.List_ReadingMentor = new ObservableCollection<Reading_mentorModel>();
				foreach (Reading_mentorModel itm in list_ReadingMentor)
				{
viewModel.List_ReadingMentor.Add(itm);
				}

//viewModel.List_BookSummeries = new ObservableCollection<Book_summariesModel>();
				foreach (Book_summariesModel itm in list_BookSummeries)
				{
               viewModel. List_BookSummeries.Add(itm);
				}

//viewModel.List_NewsLetter = new ObservableCollection<NewsLetterModel>();
				foreach (NewsLetterModel itm in list_NewsLetter)
				{
                viewModel.List_NewsLetter.Add(itm);
				}
			}
			catch (Exception ex)
			{

			}

		}
		private async Task GetCounts()
		{
			CounterModel model = null;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						model = WebService.GetAll_Counts();
					}).ContinueWith(
					t =>
					{
						if (model != null)
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								lblPlaylist_count.Text = model.playlist_count.ToString();
								lblFollower_count.Text = model.follower_count.ToString();
								lblFollowing_count.Text = model.following_count.ToString();
								lblUploadedAudio_count.Text = model.myaudio_count.ToString();
								//lblPlaylist_count.Text = "5";
								//			lblFollower_count.Text = "5";
								//			lblFollowing_count.Text ="5";
								//			lblUploadedAudio_count.Text = "5";
							});

						}

						StaticMethods.DismissLoader();
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}
}
