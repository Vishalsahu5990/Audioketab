using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AudioKetab
{
	public partial class NotificationPage : ContentPage
	{
		public NotificationPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
		async void back_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				await Navigation.PopModalAsync();
			}
			catch (Exception ex)
			{


			}
		}
	}
}
