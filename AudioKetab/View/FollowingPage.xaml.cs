using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class FollowingPage : ContentPage
	{
		List<FollowersModel> followinglist = null;
		public FollowingPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowColumnMinWidth = App.ScreenWidth/3;
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
			GetFollowings();

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

		void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
var item = e.Item as FollowersModel;
				Navigation.PushModalAsync(new UserDetailsPage(Convert.ToInt32( item.u_id), StaticDataModel.CurrentContext));
		
			}
			catch (Exception ex)
			{

			}
		}


private async Task GetFollowings()
{
			
	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				followinglist = WebService.GetFollowers();
			}).ContinueWith(
			t =>
			{
				if (followinglist != null)
				{

					flowlistview.FlowItemsSource = followinglist;
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
private async Task unFollowUser(int userid)
{

	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				ret = WebService.unFollowUser(userid);
			}).ContinueWith(
			t =>
			{
				if (ret=="success")
				{

					GetFollowings();
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
