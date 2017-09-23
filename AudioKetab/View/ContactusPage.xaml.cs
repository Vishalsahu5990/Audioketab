using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class ContactusPage : ContentPage
	{
		CircleImage profileImage = null;
		int height = 65;
		int width = 65;
		int x = 75;
		int y = 45;
		MainPage _context;
		public ICommand ChangeMenuCommand { get; private set; }
		public ContactusPage()
		{

			InitializeComponent();

		}
		public ContactusPage(MainPage context)
		{
			_context = context;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			txtName.Focused += TxtName_Focused;
			txtEmail.Focused += TxtEmail_Focused;
			txtSubject.Focused += TxtSubject_Focused;
			txtMessage.Focused += TxtMessage_Focused;
			PrepareUI();

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			GetAllCounts().Wait();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
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
		void TxtName_Focused(object sender, FocusEventArgs e)
		{
			txtName.PlaceholderColor = Color.Gray;
			lblErrorMessage.IsVisible = false;
		}

		void TxtEmail_Focused(object sender, FocusEventArgs e)
		{
			txtEmail.PlaceholderColor = Color.Gray;
			lblErrorMessage.IsVisible = false;
		}

		void TxtSubject_Focused(object sender, FocusEventArgs e)
		{
			txtSubject.PlaceholderColor = Color.Gray;
			lblErrorMessage.IsVisible = false;
		}

		void TxtMessage_Focused(object sender, FocusEventArgs e)
		{
			txtMessage.PlaceholderColor = Color.Gray;
			lblErrorMessage.IsVisible = false;
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
				await Navigation.PushModalAsync(new UploadedAudioPage(StaticDataModel.UserId));
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

		async void btnSendClicked(object sender, System.EventArgs e)
		{
			try
			{

				if (IsValidate())
				{
					Contactus(txtName.Text, txtEmail.Text, txtSubject.Text, txtMessage.Text).Wait();
				}
				else
				{

					lblErrorMessage.IsVisible = true;
				}
			}
			catch (Exception ex)
			{


			}
		}
		private bool IsValidate()
		{
			if (string.IsNullOrEmpty(txtName.Text))
			{

				txtName.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtEmail.Text))
			{

				txtEmail.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtSubject.Text))
			{

				txtSubject.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtMessage.Text))
			{

				txtMessage.PlaceholderColor = Color.Red;
				return false;

			}

			if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
			{

				txtEmail.TextColor = Color.Red;
				lblErrorMessage.Text = "Invalid email!";
				return false;
			}


			else
			{
				return true;
			}
		}
		private async Task Contactus(string name, string email, string subject, string msg)
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						ret = WebService.contactUs(name, email, subject, msg);
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{

							_context.ChangeMenu.Execute(_context.ChangeMenu);
						}
						StaticMethods.DismissLoader();

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task GetAllCounts()
		{

			string ret = string.Empty;
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
							lblPlaylist_count.Text = model.playlist_count.ToString();
							lblFollower_count.Text = model.follower_count.ToString();
							lblFollowing_count.Text = model.following_count.ToString();
							lblUploadedAudio_count.Text = model.myaudio_count.ToString();
						}
						StaticMethods.DismissLoader();

						if (HomePage.layoutHeigh > 0)
							_rlHeader.HeightRequest = HomePage.layoutHeigh;
				
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}

	}
}
