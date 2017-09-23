using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class More_ReadingMentorPage : ContentPage
	{
MainPage _context;
List<Reading_mentorModel> mentorslist = null;
public  More_ReadingMentorPage()
{
	InitializeComponent();
	
	NavigationPage.SetHasNavigationBar(this, false);
	flowlistview.FlowColumnMinWidth = App.ScreenWidth;
	flowlistview.FlowItemTapped += Flowlistview_FlowItemTapped;
	GetMoreMentors();
		}
		public  More_ReadingMentorPage(MainPage context)
		{
			InitializeComponent();
			_context = context;
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
			GetMoreMentors();
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
async void follow_unfollowTapped(object sender, System.EventArgs e)
{
	try
	{
				var result = await DisplayAlert("Alert!", "Do you want to follow?", "YES", "CANCEL");

                if (result)
                {
                    var item = (Xamarin.Forms.Button)sender;
                    var model = (from itm in mentorslist where itm.u_id == item.CommandParameter select itm).FirstOrDefault<Reading_mentorModel>();
                    //var action = await DisplayAlert("aleart", "You are about to follow?", "Yes", "No");
                    //if (action)
                    //{
                    FollowUser(Convert.ToInt32(model.u_id));
                }
		//}
	}
	catch (Exception ex)
	{
	}
		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
var item = e.Item as Reading_mentorModel;

await Navigation.PushModalAsync(new UserDetailsPage(Convert.ToInt32(item.u_id), _context,item.profile_pic));

		}

		private async Task GetMoreMentors()
{
			
	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				mentorslist = WebService.GetMoreReadingMentors();
			}).ContinueWith(
			t =>
			{
				if (mentorslist != null)
				{

					flowlistview.FlowItemsSource = mentorslist;
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
private async Task FollowUser(int userid)
{

	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				ret = WebService.followUser(userid);
			}).ContinueWith(
			t =>
			{
				if (ret == "success")
				{

GetMoreMentors();
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
