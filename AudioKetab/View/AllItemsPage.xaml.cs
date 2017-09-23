using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class AllItemsPage : ContentPage
	{
		int _identifier = 0;
        double cellSize = 0;
		public AllItemsPage()
		{

			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
		public AllItemsPage(int identifier)
		{
			InitializeComponent();
			_identifier = identifier;
            flowlistview.FlowColumnMinWidth= App.ScreenWidth / 3;
            cellSize = App.ScreenWidth / 3;
		
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowItemsSource = Enumerable.Range(0, 18).ToList();
			TaskSelector(_identifier);
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
        }

        void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Book_summariesModel;
            if (!ReferenceEquals(item,null))
            {
                Navigation.PushModalAsync(new AudioPlayerPage(StaticDataModel.CurrentContext,item));
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            flowlistview.FlowItemTapped -= Flowlistview_FlowItemTapped;
        }
		private void TaskSelector(int identifier)
		{
			try
			{
				switch (identifier)
				{
					case 11:
                        GetAllItem("showall_booksumrecentadded",1).Wait();
						break;
					case 12:
                        GetAllItem("showall_booksumrecentadded",2).Wait();
						break;
					case 13:
                        GetAllItem("showall_booksumrecentadded",3).Wait();
						break;
					case 21:
                        GetAllItem("showall_booksummostplayed",1).Wait();
						break;
					case 22:
                        GetAllItem("showall_booksummostplayed",2).Wait();
						break;
					case 23:
                        GetAllItem("showall_booksummostplayed",3).Wait();
						break;
					case 31:
                        GetAllRecentFollowers().Wait();
						break;
					case 32:
                        GetAllRecentFollowers().Wait();
						break;
					case 33:
                        GetAllRecentFollowers().Wait();
						break;
				}
			}
			catch (Exception ex)
			{

			}
		}
		private async Task GetAllItem(string method,int typeofAudio)
		{
			List<Book_summariesModel> list = null;

			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						list = WebService.GetAll_recent_added(method,typeofAudio);
					}).ContinueWith(
					t =>
					{
						if (list != null)
						{
							for (int i = 0; i < list.Count; i++)
							{
								list[i].image_path = Constants.SERVER_IMG_URL + list[i].image_path;
                                list[i].cell_size = cellSize;
							}
							flowlistview.FlowItemsSource = list;
						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
private async Task GetAllRecentFollowers()
{
			List<FollowersModel> list = null;
	string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				list = WebService.GetAll_Followers();
			}).ContinueWith(
			t =>
			{
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].image_path = Constants.PRO_PIC_IMG_URL + list[i].profile_pic;
						list[i].book_name =  list[i].first_name;
					}
					flowlistview.FlowItemsSource = list;
				}


			}, TaskScheduler.FromCurrentSynchronizationContext() 
		);
		}
	}
}
