﻿using System;
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
            NavigationPage.SetHasNavigationBar(this, false);
			GetMyAudios(StaticDataModel.UserId).Wait();
		}
		public UploadedAudioPage(int userid)
		{ 
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			GetMyAudios(userid).Wait(); 
		}
async void Back_Tapped(object sender, System.EventArgs e)
{
	try
	{
				Navigation.PopModalAsync();

	}
	catch (Exception ex)
	{


	}
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
					for (int i = 0; i < _model.Count; i++)
					{
						_model[i].image_path = Constants.SERVER_IMG_URL + _model[i].image_path;
					}
					flowlistview.FlowItemsSource = _model;
				}

				StaticMethods.DismissLoader();
			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
