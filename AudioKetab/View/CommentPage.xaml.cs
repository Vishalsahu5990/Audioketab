using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class CommentPage : ContentPage
	{
		public int _s_id;
		public static List<CommentModel> _list = null;
		public CommentPage()
		{

			InitializeComponent();

		}
		public CommentPage(int sid)
		{
			_s_id = sid;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			btnSend.Clicked+= BtnSend_Clicked;

			flowlistview.HeightRequest = App.ScreenHeight - 100;
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;
			getComments(_s_id).Wait();

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
		private async Task getComments(int s_id)
		{
			
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				_list = WebService.GetAllComments(s_id);
					}).ContinueWith(
					t =>
					{
						Device.BeginInvokeOnMainThread(() =>
				{
					if (_list!=null)
					{ 
						for (int i = 0; i < _list.Count; i++)
						{
							_list[i].profile_pic = Constants.PRO_PIC_IMG_URL + _list[i].profile_pic;
							if (_list[i].user_id == StaticDataModel.UserId.ToString())
								_list[i].isVisible = true;
							else
								_list[i].isVisible = false;

						}
						flowlistview.FlowItemsSource = _list;
					} 


				});  
				 
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}

			void BtnSend_Clicked(object sender, EventArgs e)
			{
				if (!string.IsNullOrEmpty(txtComment.Text))
				{
					sendComment();

				}
			}

		void DeleteComment_Clicked(object sender, System.EventArgs e)
		{
			try
			{
				var item = (Xamarin.Forms.Button)sender;
				CommentModel listitem = (from itm in _list where itm.user_id == item.CommandParameter select itm).FirstOrDefault<CommentModel>();
				DeleteComment(Convert.ToInt32( listitem.comment_id));
			}
			catch (Exception ex)
			{

			}
		}
		private async Task sendComment()
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				ret = WebService.SendComment(Convert.ToInt32(_s_id),txtComment.Text);
					}).ContinueWith(
					t =>
					{
						if (ret== "success")
						{

							getComments(_s_id);
							txtComment.Text = string.Empty;
						}


					}, TaskScheduler.FromCurrentSynchronizationContext()

				);
		}
		private async Task DeleteComment(int comment_id)
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				ret = WebService.DeleteComment(Convert.ToInt32(_s_id), comment_id);
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{

							getComments(_s_id);
							txtComment.Text = string.Empty;
						}


					}, TaskScheduler.FromCurrentSynchronizationContext()

				);
		}
	}
}
