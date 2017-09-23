using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class SearchPeoplePage : ContentPage
	{
		List<UserSearchModel> listUser = null;
		public SearchPeoplePage()
		{
			InitializeComponent();
		}
		public SearchPeoplePage(MainPage context)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowColumnMinWidth = App.ScreenWidth;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			btnSearch.Clicked += BtnSearch_Clicked;
			flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			btnSearch.Clicked -= BtnSearch_Clicked;
			flowlistview.FlowItemTapped-= Flowlistview_FlowItemTapped;
		}
		void BtnSearch_Clicked(object sender, EventArgs e)
		{
           SearchPeople().Wait();
		}

		async	void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
				var item = e.Item as UserSearchModel;
                
				await Navigation.PushModalAsync(new UploadedAudioPage(Convert.ToInt32(item.u_id)));
			}
			catch (Exception ex)
			{

			}
		}

		async void Back_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				StaticDataModel.CurrentContext.MenuTapped.Execute(StaticDataModel.CurrentContext.MenuTapped);
			}
			catch (Exception ex)
			{


			}
		}
		private async Task SearchPeople()
		{


			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{


						listUser = WebService.SearchPeople(txtSearch.Text);
					}).ContinueWith(
					t =>
					{
				if (listUser != null)
				{
					

					for (int i = 0; i < listUser.Count; i++)
					{
						listUser[i].profile_pic = Constants.PRO_PIC_IMG_URL + listUser[i].profile_pic;
						listUser[i].first_name = listUser[i].first_name + " " + listUser[i].last_name;
					}

					flowlistview.FlowItemsSource = listUser;

				}
				else
				{
					StaticMethods.ShowToast("Something went wrong. Plese try agan later!");

				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}

	}
}
