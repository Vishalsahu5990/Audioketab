using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class UploadedAudioPage : ContentPage
	{
		int _userId = 0;
		public UploadedAudioPage()
		{
			InitializeComponent();
			GetMyAudios(StaticDataModel.UserId).Wait();
		}
public UploadedAudioPage(int userid)
{
	InitializeComponent();
	GetMyAudios(userid).Wait();
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
				await Navigation.PushModalAsync(new UploadedAudioPage( _userId));
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



async void Audio_Tapped(object sender, System.EventArgs e)
{
	
}
private async Task GetMyAudios(int user_id)
{
			List<Book_summariesModel> _model = null;
	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				 _model = WebService.GetUploadedAudio(user_id);
			}).ContinueWith(
			t =>
			{
				if (_model != null)
				{

					flowlistview.FlowItemsSource = _model;
				}

				StaticMethods.DismissLoader();
			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
