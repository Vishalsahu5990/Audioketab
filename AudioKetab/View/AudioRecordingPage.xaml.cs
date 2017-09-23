using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class AudioRecordingPage : ContentPage
	{
		public static Book_summariesModel _uploadAudioModel = null;
		MainPage _context;
		Plugin.Media.Abstractions.MediaFile picture_Data = null;
		byte[] pictureStream = null;
		public AudioRecordingPage()
		{

			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
		public AudioRecordingPage(MainPage context)
		{
			_context = context;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			SetData();
			categoryypicker.SelectedIndexChanged += Categoryypicker_SelectedIndexChanged;
			btnSubmit.Clicked+= BtnSubmit_Clicked;
			_uploadAudioModel = new Book_summariesModel(); 

		}
		private void SetData()
		{
			try
			{

				var array = StaticMethods.GetStaticCateogories();
				for (int i = 0; i < array.Length; i++)
				{
					categoryypicker.Items.Add(array[i]);
				}
			}
			catch (Exception ex)
			{ 

			} 
		}

		async void menu_Tapped(object sender, System.EventArgs e)
		{
			try
			{
                await Navigation.PopAsync();
				//_context.MenuTapped.Execute(_context.MenuTapped);
			}
			catch (Exception ex)
			{


			}
		}
		async void recorder_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				lblRecording.Text = string.Empty;
				_uploadAudioModel.byte_recorded_audio = null;
				RecorderPopup s = new RecorderPopup();
				Navigation.PushPopupAsync(s);
			}
			catch (Exception ex)
			{


			}
		}
		async void rbtnBoolsummeries_Tapped(object sender, System.EventArgs e)
		{
			
				r1.Source = "radio_check";
			    r2.Source = "radio_uncheck";
			    r3.Source = "radio_uncheck";
			txtBookname.Placeholder = "Book name";
			txtAuthorname.Placeholder = "Author name";
		}
		async void rbtnLectures_Tapped(object sender, System.EventArgs e)
		{
			
				r1.Source = "radio_uncheck";
			    r2.Source = "radio_check";
			    r3.Source = "radio_uncheck";

			txtBookname.Placeholder = "Lecture or training name";
			txtAuthorname.Placeholder = "Lecturer or trainer name";

		}
		async void rbtnNewsletter_Tapped(object sender, System.EventArgs e)
		{
			
				r1.Source = "radio_uncheck";
			    r2.Source = "radio_uncheck";
			    r3.Source = "radio_check";
			txtBookname.Placeholder = "Article name";
			txtAuthorname.Placeholder = "Author name";
		}
		async void categorypicker_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				categoryypicker.Focus();
			});
		}
		async void uploadpicture_Tapped(object sender, System.EventArgs e)
		{
			GetPicture();
		}
		async void chooseaudio_Tapped(object sender, System.EventArgs e)
		{
           
                DependencyService.Get<IiOSMethods>().ShowLoader();
           
		}
		void Categoryypicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = categoryypicker.Items[categoryypicker.SelectedIndex];
			txtCountry.Text = item;
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
				picture_Data = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
				{
					PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
				});


				if (picture_Data == null)
					return;
				
			
				var filename = Path.GetFileName(picture_Data.Path);

				 pictureStream = ReadFully(picture_Data.GetStream());
				lblUPloadbookpicture.Text = filename;
			}
			catch (Exception ex)
			{

			}
		}
		public static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		void BtnSubmit_Clicked(object sender, EventArgs e)
		{
			if (IsValidate())
			{
				if (_uploadAudioModel.byte_recorded_audio != null)
				{
					UploadAudio();
				}
				else
				{ 
				StaticMethods.ShowToast("Please upload or record audio!");
				}
			}
			else
			{
				StaticMethods.ShowToast("All fields are required!");
			}
		}
		private bool IsValidate()
		{
			if (string.IsNullOrEmpty(txtCountry.Text))
			{

				txtCountry.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtBookname.Text))
			{

				txtBookname.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtAuthorname.Text))
			{

				txtAuthorname.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtDesc.Text))
			{

				txtDesc.PlaceholderColor = Color.Red;
				return false;

			}
			//else if (string.IsNullOrEmpty(txtArticleurl.Text))
			//{

			//	txtArticleurl.PlaceholderColor = Color.Red;
			//	return false;

			//}
			//else if (string.IsNullOrEmpty(txtVideourl.Text))
			//{

			//	txtVideourl.PlaceholderColor = Color.Red;
			//	return false;

			//}



			else
			{
				return true;
			}
		}
		private async Task UploadAudio()
		{
			
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				_uploadAudioModel.country_name = txtCountry.Text;
				_uploadAudioModel.book_name = txtBookname.Text;
				_uploadAudioModel.author_name = txtAuthorname.Text;
				_uploadAudioModel.article_url = txtArticleurl.Text;
				_uploadAudioModel.typeof_audio = "mp3";
				_uploadAudioModel.article_url = txtArticleurl.Text;
				_uploadAudioModel.video_url = txtVideourl.Text;
				_uploadAudioModel.image_path = Convert.ToBase64String(pictureStream);
				 WebService.UploadAudio(_uploadAudioModel);
					}).ContinueWith(
					t =>
					{
						

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
            MessagingCenter.Subscribe<string, byte[]>(this, "NotificationRecieved",(arg1, arg2)  =>
			{
                Device.BeginInvokeOnMainThread(() =>
                {
                    _uploadAudioModel.byte_recorded_audio = arg2;
                    lblUPloadAudioFile.Text = arg1;
                });


			});
		}
	}
}
