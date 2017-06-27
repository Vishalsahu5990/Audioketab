using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class More_NewsLetterPage : ContentPage
	{
		List<Book_summariesModel> list_recentadded = null;
		List<Book_summariesModel> list_mostplayed = null;
		List<Book_summariesModel> list_recentfollower = null;
		private double cellSize = 0;
		string[] arrayCategory;
		public More_NewsLetterPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			cellSize = App.ScreenWidth / 4.5;
			//StaticMethods.ShowLoader();
			var ret = WebService.GetMoreNewsLetter();
			if (ret == "success")
			{
				ProcessResult();

			}
			btnMoreRecentAdded.Clicked += BtnMoreRecentAdded_Clicked;
			btnMoreMostPlayed.Clicked += BtnMoreMostPlayed_Clicked;
			btnMoreRecentFollowers.Clicked += BtnMoreRecentFollowers_Clicked;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			categoryypicker.SelectedIndexChanged += Categoryypicker_SelectedIndexChanged;
			btnSearch.Clicked += BtnSearch_Clicked;
			arrayCategory = StaticMethods.GetStaticCateogories();
			for (int i = 0; i < arrayCategory.Length; i++)
			{
				categoryypicker.Items.Add(arrayCategory[i]);
			}

		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			categoryypicker.SelectedIndexChanged -= Categoryypicker_SelectedIndexChanged;
			btnSearch.Clicked -= BtnSearch_Clicked;
		}
		async void BtnSearch_Clicked(object sender, EventArgs e)
		{
			SearchByCategory().Wait();
		}
		async void slCategory_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				Device.BeginInvokeOnMainThread(async () => { categoryypicker.Focus(); });
			}
			catch (Exception ex)
			{

			}
		}

		void Categoryypicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = categoryypicker.Items[categoryypicker.SelectedIndex];
			lblCategory.Text = item;
		}
		async void BtnMoreRecentAdded_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new AllItemsPage(13));
		}
		async void BtnMoreMostPlayed_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new AllItemsPage(23));
		}

		async void BtnMoreRecentFollowers_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new AllItemsPage(33));
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
		async void dropdown_Tapped(object sender, System.EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{


			}
		}

		private void ProcessResult()
		{
			list_recentadded = new List<Book_summariesModel>();
			list_mostplayed = new List<Book_summariesModel>();
			list_recentfollower = new List<Book_summariesModel>();

			try
			{
				var _recentAdded = WebService.recent_added;
				var _mostPlayed = WebService.most_played;
				var _recentFollower = WebService.recent_followers;

				if (_recentAdded != null)
				{
					foreach (var item in _recentAdded)
					{
						list_recentadded.Add(new Book_summariesModel
						{
							s_id = item["s_id"].ToString(),
							user_id = item["user_id"].ToString(),
							typeof_audio = item["typeof_audio"].ToString(),
							book_name = item["book_name"].ToString(),
							author_name = item["author_name"].ToString(),
							category = item["category"].ToString(),
							comment = item["comment"].ToString(),
							song_path = item["song_path"].ToString(),
							image_path = Constants.SERVER_IMG_URL + item["image_path"].ToString(),
							count_like = item["count_like"].ToString(),

							cell_size = cellSize
						});
					}


				}
				listview_recentadded.ItemsSource = list_recentadded;

				if (_mostPlayed != null)
				{
					foreach (var item in _mostPlayed)
					{
						list_mostplayed.Add(new Book_summariesModel
						{
							s_id = item["s_id"].ToString(),
							user_id = item["user_id"].ToString(),
							typeof_audio = item["typeof_audio"].ToString(),
							book_name = item["book_name"].ToString(),
							author_name = item["author_name"].ToString(),
							category = item["category"].ToString(),
							comment = item["comment"].ToString(),
							song_path = item["song_path"].ToString(),
							image_path = Constants.SERVER_IMG_URL + item["image_path"].ToString(),
							count_like = item["count_like"].ToString(),

							cell_size = cellSize
						});
					}

				}
				listview_mostplayed.ItemsSource = list_mostplayed;
				if (_recentFollower != null)
				{
					foreach (var item in _recentFollower)
					{
						list_recentfollower.Add(new Book_summariesModel
						{


							s_id = item["follow_id"].ToString(),
							user_id = item["u_id"].ToString(),
							last_name = item["last_name"].ToString(),

							first_name = item["first_name"].ToString() +" "+item["last_name"].ToString(),
							image_path = Constants.PRO_PIC_IMG_URL + item["image_path"].ToString(),

							user_to = item["user_to"].ToString(),
							user_by = item["user_by"].ToString(),

							cell_size = cellSize
						});
					}

				}



				listview_recentfollowers.ItemsSource = list_recentfollower;





			}
			catch (Exception ex)
			{

			}
			finally
			{
				//StaticMethods.DismissLoader();
			}
		}
		private async Task SearchByCategory()
		{
			string category = string.Empty;

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						for (int i = 0; i < arrayCategory.Length; i++)
						{
							if (arrayCategory[i] == lblCategory.Text)
							{
								category = i.ToString();
							}
						}


						if (category == "0")
						{
							category = "";
						}
						else if (category == "13")
						{
							category = "0";
						}

						ret = WebService.SearchByCategory(txtSearch.Text, "1", category);
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{
							Device.BeginInvokeOnMainThread(async () =>
					{
						ProcessResult();
					});




						}
						else
						{
							StaticMethods.ShowToast("No result found!");

						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
	}
}