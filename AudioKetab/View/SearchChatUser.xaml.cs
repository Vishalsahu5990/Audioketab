using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class SearchChatUser : ContentPage
	{
		List<SearchUserModel> _list=null;
		public SearchChatUser()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			btnSearch.Clicked+= BtnSearch_Clicked;
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;

		}



		async void Back_Tapped(object sender, System.EventArgs e)
{
	try
	{
		ChatEntry.keepOpen = false;
		await Navigation.PopModalAsync();
	}
	catch (Exception ex)
	{


	}
		}

		void BtnSearch_Clicked(object sender, EventArgs e)
		{
			try
			{
				if(!string.IsNullOrEmpty(txtSearch.Text))
                    SearchUser().Wait();
			}
			catch (Exception ex)
			{

			}
		}
		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as SearchUserModel;
string profile = string.Empty;

string name = item.first_name + " " + item.last_name;
			if (!string.IsNullOrEmpty(item.profile_pic))
			{
				 profile = Constants.PRO_PIC_IMG_URL + item.profile_pic;
			}
			else
			{
				profile = "defaultprofile.png";
			}
				
string id = item.u_id;
await Navigation.PushModalAsync(new ChatPage(Convert.ToInt32( id),name,profile));

		}
private async Task SearchUser()
{

	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				 _list = WebService.SearchChatUser(txtSearch.Text);
			}).ContinueWith(
			t =>
			{
				Device.BeginInvokeOnMainThread(() =>
		{
			if (_list != null)
			{
				for (int i = 0; i < _list.Count; i++)
				{
					if (!string.IsNullOrEmpty(_list[i].profile_pic))
						_list[i].profile_pic = Constants.PRO_PIC_IMG_URL + _list[i].profile_pic;
					else
						_list[i].profile_pic = "defaultprofile.png";


				}
				flowlistview.FlowItemsSource = _list;
			}
			else
			{
				StaticMethods.ShowToast("No records found!");
					}


		});

			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
