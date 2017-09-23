using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using WebSocket.Portable;
using WebSocket.Portable.Interfaces;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace AudioKetab
{
	public partial class ChatPage : ContentPage
	{

		Websockets.IWebSocketConnection connection = null;
		int _userid = 0;
		string _name = string.Empty;
		string _profile = string.Empty;
		List<ChatModel> _list = null;
		ChatModel cm = null;
		ChatItemList items;
		public ChatPage()   
		{
            InitializeComponent();
		}
		public ChatPage(int userid, string name, string profile)
		{
			
			//StaticDataModel.UserId = 1;//for testing 
			_userid = userid;
			_name = name;
			_profile = profile;
			InitializeComponent();
			
			Connect();
			NavigationPage.SetHasNavigationBar(this, false);

			btnSend.Clicked += BtnSend_Clicked;

			flowlistview.HeightRequest = App.ScreenHeight - 150;
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;

			txtComment.Focused+= TxtComment_Focused;
			txtComment.Unfocused+= TxtComment_Unfocused;
			txtComment.Completed+= TxtComment_Completed;
			txtComment.TextChanged+= TxtComment_TextChanged;
		}

		void TxtComment_Focused(object sender, FocusEventArgs e)
		{
			ChatEntry.keepOpen = true;
		}

		void TxtComment_Unfocused(object sender, FocusEventArgs e)
		{
			ChatEntry.keepOpen = false;
		}

		void TxtComment_TextChanged(object sender, TextChangedEventArgs e)
		{
			onType();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ChatEntry.keepOpen = false;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			ChatEntry.keepOpen = true;
			lblUsername.Text = _name;
			imgProfile.Source = _profile;

            getConversation(_userid).Wait();

            MessagingCenter.Subscribe<object, NotificatonModel.RemoteNoti>(this, "NotificationRecieved", (object arg1, NotificatonModel.RemoteNoti arg2) =>
               {
                   var cm = new ChatModel
                   {
                       Incoming = true,
                       Outgoing = false,
                    msg_desc = arg2.msg,
                    msg_datetime=DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                   };
                   if (_list == null)
                       _list = new List<ChatModel>();


                   _list.Add(cm);
                   items = new ChatItemList(_list);
                   flowlistview.FlowItemsSource = items.Items;
                   var lastItem = flowlistview.FlowItemsSource.OfType<object>().Last();
                   Device.BeginInvokeOnMainThread(() => flowlistview.ScrollTo(lastItem, ScrollToPosition.End, false));
                  
            
            });
		}

		void TxtComment_Completed(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtComment.Text))
			{
				Send();
			}
		}
		void onType()
		{ 
		 try
			{
JObject jsonObject = new JObject();
jsonObject.Add("msg", "");
 			jsonObject.Add("type", "type");
			jsonObject.Add("sender_id", StaticDataModel.UserId);
			jsonObject.Add("reciever_id", _userid);
			var json = jsonObject.ToString();
connection.Send(json);
			}
			catch (Exception ex)
			{

			}

		}

		void Connect()
		{
			// 2) Get a websocket from your PCL library via the factory
			connection = Websockets.WebSocketFactory.Create();
			connection.OnOpened += Connection_OnOpened;
			connection.OnMessage += Connection_OnMessage;
			connection.Open("ws://fazaasa.com:9005/server1.php");
		}

		void Send()
		{
			try
			{
JObject jsonObject = new JObject();
jsonObject.Add("msg", txtComment.Text);
			jsonObject.Add("type", "message");
			jsonObject.Add("sender_id", StaticDataModel.UserId);
			jsonObject.Add("reciever_id", _userid);
			var json = jsonObject.ToString();
connection.Send(json);


			cm=new ChatModel
			{
				Incoming = false,
				Outgoing = true,
								msg_desc = txtComment.Text
											 ,
				msg_datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")        
							};
				if (_list == null)
					_list = new List<ChatModel>();


			_list.Add(cm);
			items = new ChatItemList(_list);
flowlistview.FlowItemsSource = items.Items;
				var lastItem = flowlistview.FlowItemsSource.OfType<object>().Last();
Device.BeginInvokeOnMainThread(() =>flowlistview.ScrollTo(lastItem, ScrollToPosition.End, false));
			sendMessage().Wait();
			}
			catch (Exception ex)
			{

			}
		}

		private void Connection_OnOpened()
		{
			Debug.WriteLine("Opened !");
		}

		private void Connection_OnMessage(string obj)
		{
			processMessasge(obj);

		}
		private void processMessasge(string obj)
		{ 
		try
			{
				var data = JObject.Parse(obj);
				var type = data["type"].ToString();
				var sender_id = data["sender_id"].ToString();
				var msg = data["msg"].ToString();
				var reciever_id = data["reciever_id"].ToString();
				if (!string.IsNullOrEmpty(sender_id))
				{
					if (Convert.ToInt32( sender_id) != StaticDataModel.UserId)
					{
						if (type == "message")
						{
							_list.Add(new ChatModel
							{
								Incoming = true,
								Outgoing = false,
								msg_desc = msg
							});
							items = new ChatItemList(_list);
							flowlistview.FlowItemsSource = items.Items;
						}
					}
				}
			}
			catch (Exception ex)
			{

			}
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
		async void BtnSend_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtComment.Text))
			{
				Send();
                txtComment.Text = string.Empty;
			}
		}
		private async Task getConversation(int u_id)
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
					_list = WebService.GetChatConversation(u_id);
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
							
							if (_list[i].users_id == StaticDataModel.UserId.ToString())
							{ 
							_list[i].Outgoing = true;
								_list[i].Incoming = false;
							}
								
							else
							{ 
							_list[i].Incoming = true;
							_list[i].Outgoing = false;
							}
							//_list[i].profile_pic = Constants.PRO_PIC_IMG_URL + _list[i].profile_pic;	

						}
var list = _list.OrderBy(x => x.msg_id).ToList();
						items = new ChatItemList(list);
						flowlistview.FlowItemsSource =items.Items;
var lastItem = flowlistview.ItemsSource.OfType<object>().Last();
//System.Threading.Tasks.Task.Factory.StartNew(() =>
//{
flowlistview.ScrollTo(lastItem, ScrollToPosition.End, false);
					//});
					}


				});

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task sendMessage()
		{

			string ret = string.Empty;
			//StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				ret = WebService.SendMessage(_userid, txtComment.Text);
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{

							
						}


					}, TaskScheduler.FromCurrentSynchronizationContext()

				);
		}
	}
}
