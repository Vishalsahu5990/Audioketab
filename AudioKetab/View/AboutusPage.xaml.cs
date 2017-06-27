using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{

	public partial class AboutusPage : ContentPage
	{
		MainPage _context;
		public AboutusPage()
		{

			InitializeComponent();     
			NavigationPage.SetHasNavigationBar(this, false);
		}
		public AboutusPage(MainPage context)
		{
			_context = context;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			//GetAudio().Wait();
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
		private async Task GetAudio()
		{
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				ret = WebService.getAboutus();
					}).ContinueWith(
					t =>
					{
				if (!string.IsNullOrEmpty(ret))
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								lblAboutus.Text = ret;	

							});

						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}

}
