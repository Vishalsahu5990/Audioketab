using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class FollowersPage : ContentPage
	{
		public FollowersPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowItemsSource = Enumerable.Range(0, 40).ToList();
			flowlistview.FlowColumnMinWidth = App.ScreenWidth/3;
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
			GetPlayList();
		}

		void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
var item = e.Item as FollowingModel;
				Navigation.PushModalAsync(new UserDetailsPage(Convert.ToInt32( item.u_id), StaticDataModel.CurrentContext));
		
			}
			catch (Exception ex)
			{

			}
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

private async Task GetPlayList()
		{
			List<FollowingModel> followerslist = null;
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				   followerslist = WebService.GetFollowings();
					}).ContinueWith(
					t =>
					{
						if (followerslist != null)
						{

							flowlistview.FlowItemsSource = followerslist;
						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}
}
