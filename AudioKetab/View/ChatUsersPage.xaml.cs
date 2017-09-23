using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class ChatUsersPage : ContentPage
	{
		List<RootObject> _list = null;
		public ChatUsersPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			getChatUserList().Wait();
			flowlistview.HeightRequest = App.ScreenHeight - 100;
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
			btnMsg.Clicked+= BtnMsg_Clicked;

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

		async void BtnMsg_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new SearchChatUser());
		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			string profile = string.Empty;
		var item=	e.Item as RootObject;
string name = item.users.first_name + " " + item.users.last_name;
			if (!string.IsNullOrEmpty(item.users.profile_pic))
			{
				 profile = Constants.PRO_PIC_IMG_URL + item.users.profile_pic;
			}
			else
			{
				profile = "defaultprofile.png";
			}
				
            string id = item.users.u_id;
			await Navigation.PushModalAsync(new ChatPage(Convert.ToInt32( id),name,profile));
		}

		private async Task getChatUserList()
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
					_list = WebService.GetChatUserList();
					}).ContinueWith(
					t =>
					{
						Device.BeginInvokeOnMainThread(() =>
				{
					if (_list != null)
					{
						for (int i = 0; i < _list.Count; i++)
						{
                            _list[i].users.first_name = _list[i].users.first_name + " " + _list[i].users.last_name;
							if (!string.IsNullOrEmpty(_list[i].users.profile_pic))
								_list[i].users.profile_pic = Constants.PRO_PIC_IMG_URL + _list[i].users.profile_pic;
							else
								_list[i].users.profile_pic = "defaultprofile.png";	

						
						}
						flowlistview.FlowItemsSource = _list;
					}


				});

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}
}
