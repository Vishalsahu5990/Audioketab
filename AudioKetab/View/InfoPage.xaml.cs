using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AudioKetab
{
	public partial class InfoPage : ContentPage
	{
		public InfoPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
		async void Submit_Clicked(object sender, System.EventArgs e)
		{


			await Navigation.PushAsync(new LoginPage());


		}
	}
}
