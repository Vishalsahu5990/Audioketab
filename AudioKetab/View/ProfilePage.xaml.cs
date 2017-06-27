using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Media;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class ProfilePage : ContentPage
	{
		CircleImage profileImage = null;
		int height = 55;
		int width = 55;
		int x = 70;
		int y = 50;
		MainPage _context;
		Plugin.Media.Abstractions.MediaFile profileData = null;
		double _PicSize = 0;
		public ProfilePage()
		{
			InitializeComponent();
		}
		public ProfilePage(MainPage context)
		{
			_context = context;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			datepicker.DateSelected += Datepicker_DateSelected;
			countrypicker.SelectedIndexChanged += Countrypicker_SelectedIndexChanged;
			txtFirstName.Focused += TxtFirstName_Focused;
			txtLastName.Focused += TxtLastName_Focused;
			txtDob.Focused += TxtDob_Focused;
			txtEmail.Focused += TxtEmail_Focused;
			txtMobileNo.Focused += TxtMobileNo_Focused;
			txtMobileNo.TextChanged += TxtMobileNo_TextChanged;
			txtDescription.Focused += TxtDescription_Focused;
			_PicSize = App.ScreenWidth / 3;
			imgProfile.WidthRequest = _PicSize;
			imgProfile.HeightRequest = _PicSize;
			GetProfileData();
			GetCountries();
			PrepareUI();
			var model = WebService.GetAll_Counts();
			if (model != null)
			{ 
			lblPlaylist_count.Text = model.playlist_count.ToString();
						lblFollower_count.Text = model.follower_count.ToString();
						lblFollowing_count.Text = model.following_count.ToString();
						lblUploadedAudio_count.Text = model.myaudio_count.ToString();
			}
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			//GetCounts();
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
		void TxtMobileNo_TextChanged(object sender, TextChangedEventArgs e)
		{
			int _limit = 10;    //Enter text limit

			string _text = txtMobileNo.Text;      //Get Current Text
			if (_text.Length > _limit)       //If it is more than your character restriction
			{
				_text = _text.Remove(_text.Length - 1);  // Remove Last character
				txtMobileNo.Text = _text;        //Set the Old value
			}
		}



		void TxtFirstName_Focused(object sender, FocusEventArgs e)
		{
			txtFirstName.PlaceholderColor = Color.Gray;
		}

		void TxtLastName_Focused(object sender, FocusEventArgs e)
		{
			txtLastName.PlaceholderColor = Color.Gray;
		}

		void TxtDob_Focused(object sender, FocusEventArgs e)
		{
			txtDob.PlaceholderColor = Color.Gray;
		}

		void TxtEmail_Focused(object sender, FocusEventArgs e)
		{
			txtEmail.PlaceholderColor = Color.Gray;
			txtEmail.TextColor = Color.Gray;
		}



		void TxtMobileNo_Focused(object sender, FocusEventArgs e)
		{
			txtMobileNo.PlaceholderColor = Color.Gray;
		}

		void TxtDescription_Focused(object sender, FocusEventArgs e)
		{
			txtDescription.PlaceholderColor = Color.Gray;
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
		async void DOB_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				datepicker.Focus();
			});
		}
		async void Country_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				countrypicker.Focus();
			});
		}

		void Countrypicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = countrypicker.Items[countrypicker.SelectedIndex];
			txtCountry.Text = item;
		}

		void Datepicker_DateSelected(object sender, DateChangedEventArgs e)
		{
			txtDob.Text = e.NewDate.ToString("yyyy-MM-dd");
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

		async void ChangeProfileClicked(object sender, System.EventArgs e)
		{
			try
			{

				GetPicture();
			}
			catch (Exception ex)
			{


			}
		}
		private async void GetPicture()
		{
			try
			{
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}
				profileData = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
				{
					PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
				});


				if (profileData == null)
					return;

				imgProfile.Source = ImageSource.FromStream(() =>
		{
			var stream = profileData.GetStream();

			//file.Dispose();
			return stream;
		});

			}
			catch (Exception ex)
			{

			}
		}
		async void btnSendClicked(object sender, System.EventArgs e)
		{
			lblErrorMessage.IsVisible = false;
			try
			{

				if (IsValidate())
				{
					UpdateProfile();
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
			if (string.IsNullOrEmpty(txtFirstName.Text))
			{

				txtFirstName.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtLastName.Text))
			{

				txtLastName.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtDob.Text))
			{

				txtDob.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtEmail.Text))
			{

				txtEmail.PlaceholderColor = Color.Red;
				return false;

			}

			else if (string.IsNullOrEmpty(txtCountry.Text))
			{

				txtCountry.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtMobileNo.Text))
			{

				txtMobileNo.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtDescription.Text))
			{

				txtDescription.PlaceholderColor = Color.Red;
				return false;

			}

			if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
			{

				txtEmail.TextColor = Color.Red;
				return false;
			}


			else
			{
				return true;
			}
		}
		private async Task GetCountries()
		{
			List<string> list = null;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						list = WebService.GetCountry();
					}).ContinueWith(
					t =>
					{
						if (list != null)
						{
							foreach (var item in list)
							{
								countrypicker.Items.Add(item);
							}
						}



					}, TaskScheduler.FromCurrentSynchronizationContext()
							);
		}
		private async Task GetProfileData()
		{
			List<string> list = null;
			ProfileModel _model = null;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						_model = WebService.GetProfile(StaticDataModel.UserId);
					}).ContinueWith(
					t =>
					{
						if (_model != null)
						{
							SetData(_model);
						}

						StaticMethods.DismissLoader();


					}, TaskScheduler.FromCurrentSynchronizationContext()
							);
		}
		private void SetData(ProfileModel model)
		{
			try
			{
				txtFirstName.Text = model.first_name;
				txtLastName.Text = model.last_name;
				txtDob.Text = model.dob;
				txtEmail.Text = model.email;
				txtCountry.Text = model.country_name;
				txtMobileNo.Text = model.mobile_no;
				txtDescription.Text = model.description;
				imgProfile.Source = Constants.PRO_PIC_IMG_URL + model.profile_pic;
			}
			catch (Exception ex)
			{

			}
		}
		private async Task UpdateProfile()
		{
			List<string> list = null;
			string ret = string.Empty;
			string base64 = string.Empty;
			ProfileModel _model = new ProfileModel();
			ProfileModel model = null;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						_model.first_name = txtFirstName.Text;
						_model.last_name = txtLastName.Text;
						_model.email = txtEmail.Text;
						_model.mobile_no = txtMobileNo.Text;
						_model.description = txtDescription.Text;
						_model.dob = txtDob.Text;
						_model.country_name = txtCountry.Text;
						if (profileData != null)
						{
							var bytearray = StreamToByte(profileData.GetStream());
							base64 = Convert.ToBase64String(bytearray);
							_model.profile_pic = base64;
						}

						model = WebService.UpdateProfile(_model);

					}).ContinueWith(
					t =>
					{
						if (model != null)
						{
							CrossSecureStorage.Current.SetValue("userId", StaticDataModel.UserId.ToString());
							CrossSecureStorage.Current.SetValue("profilePic", model.profile_pic.ToString());
							CrossSecureStorage.Current.SetValue("firstName", model.first_name.ToString());
							CrossSecureStorage.Current.SetValue("lastName", model.last_name.ToString());
							CrossSecureStorage.Current.SetValue("userEmail", model.email.ToString());

							StaticMethods.ShowToast("Profile updated Successfully");
					profileImage.Source = ImageSource.FromStream(() =>
		{
			var stream = profileData.GetStream();

			//file.Dispose();
			return stream;
		});
						}
						else
						{
							StaticMethods.ShowToast("Something went wrong, please try again later!");
							_context.ChangeMenu.Execute(_context.ChangeMenu);
						}
						StaticMethods.DismissLoader();

					}, TaskScheduler.FromCurrentSynchronizationContext()
							);
		}
		public static byte[] StreamToByte(Stream input)
		{
			try
			{


				using (MemoryStream ms = new MemoryStream())
				{
					input.CopyTo(ms);
					return ms.ToArray();
				}

			}
			catch (Exception ex)
			{
				return null;
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
