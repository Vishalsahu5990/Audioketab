using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
namespace AudioKetab
{
	public partial class SearchPage : ContentPage
	{
		private int itemsPerPage = 20;
		private int pageNumber;
		private bool dataLoading;
		private static int pagenumber = 1;
string type = string.Empty;
string category = string.Empty;
		AllAudioItems items = null;
		string[] arrayCategory;
		string[] arrayTypeofAudio;

		List<AllAudioModel> listAudio = null;
		public SearchPage(MainPage context)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowColumnMinWidth = App.ScreenWidth / 3;
			flowlistview.FlowItemsSource = Enumerable.Range(0, 27).ToList();
			//this.BindingContext = new ExampleViewModel { };
			SetData();
		}
		public SearchPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			flowlistview.FlowColumnMinWidth = App.ScreenWidth / 3;
			flowlistview.FlowItemsSource = Enumerable.Range(0, 27).ToList();
			SetData();
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			pagenumber = 1;
			flowlistview.FlowItemAppearing += Flowlistview_FlowItemAppearing;
			flowlistview.FlowItemTapped += Flowlistview_FlowItemTapped;
			categoryypicker.SelectedIndexChanged+= Categoryypicker_SelectedIndexChanged;
			typepicker.SelectedIndexChanged+= Typepicker_SelectedIndexChanged;
			btnSearch.Clicked+= BtnSearch_Clicked;
			GetAllAudio(pagenumber).Wait();
		}
protected override void OnDisappearing()
{
	base.OnDisappearing();
			flowlistview.FlowItemAppearing -= Flowlistview_FlowItemAppearing;
			flowlistview.FlowItemTapped -= Flowlistview_FlowItemTapped;
			categoryypicker.SelectedIndexChanged-= Categoryypicker_SelectedIndexChanged;
			typepicker.SelectedIndexChanged-= Typepicker_SelectedIndexChanged;
			btnSearch.Clicked-= BtnSearch_Clicked;
		}

		void BtnSearch_Clicked(object sender, EventArgs e)
		{
			SearchAudio().Wait();
		}

		void Categoryypicker_SelectedIndexChanged(object sender, EventArgs e)
		{
var item = categoryypicker.Items[categoryypicker.SelectedIndex];
			lblCategory.Text = item;
		}

		void Typepicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = typepicker.Items[typepicker.SelectedIndex];
			lblType.Text = item;
		}

		async void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
		{

			try
			{
				Book_summariesModel _model = new Book_summariesModel();
				var item = e.Item as AllAudioModel;
				_model.user_id = item.user_id;
				//_model.s_id = item.s_id;
				_model.song_path = item.song_path;

				_model.image_path = item.image_path;
				_model.book_name = item.book_name;
				_model.author_name = item.author_name;
				_model.count_like = item.count_like;
				_model.article_url = item.article_url;
				_model.video_url = item.video_url;
					_model.comment = item.comment;
				await Navigation.PushModalAsync(new AudioPlayerPage(StaticDataModel.CurrentContext, _model));
			}
			catch (Exception ex)
			{

			}
		}



		void Flowlistview_ItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			var item = e.Item as List<AllAudioModel>;
			if (item != null)
			{
				var count = item.Count;

			}
		}

		void Flowlistview_FlowItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			try
			{
				int currentIdx = 0;
				if (e.Item != "0")
				{
					currentIdx = items.Items.IndexOf(e.Item as AllAudioModel);
					Debug.WriteLine(currentIdx.ToString());
					var lastItem = items.Items.Count - 1;
					if (currentIdx == lastItem)
					{
						if (pagenumber != 0)
							LoadMoreProcess().Wait(); // Make the rest api call to get more data to list
					}
				}

			}
			catch (Exception ex)
			{

			}
			//int currentIdx=0;
			//if(e.Item !="0")
			// currentIdx = listAudio.IndexOf(e.Item as AllAudioModel);
			//Debug.WriteLine(currentIdx.ToString());

			//MessagingCenter.Send<SearchPage,AllAudioModel>(this,"LoadItems", e.Item as AllAudioModel);
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
		async void slType_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				Device.BeginInvokeOnMainThread(async() => { typepicker.Focus(); });
			}
			catch (Exception ex)
			{


			}
		}
private void SetData()
{
			try
			{

				arrayCategory = StaticMethods.GetStaticCateogories();
				for (int i = 0; i < arrayCategory.Length; i++)
				{
					categoryypicker.Items.Add(arrayCategory[i]);
				}

				arrayTypeofAudio= StaticMethods.GetStaticTypeofAudio();
				for (int i = 0; i<arrayTypeofAudio.Length; i++)
				{
					typepicker.Items.Add(arrayTypeofAudio[i]);
				}
			}
			catch (Exception ex)
			{

			}
		}
		private async Task GetAllAudio(int paging)
		{

			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						listAudio = WebService.GetAllAudios(paging);
					}).ContinueWith(
					t =>
					{
						if (listAudio != null)
						{

							for (int i = 0; i < listAudio.Count; i++)
							{
								listAudio[i].image_path = Constants.SERVER_IMG_URL + listAudio[i].image_path;
							}
							items = new AllAudioItems(listAudio);
							flowlistview.FlowItemsSource = items.Items;

						}
						else
						{
							StaticMethods.ShowToast("Something went wrong. Plese try agan later!");

						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private async Task LoadMoreProcess()
		{
			List<AllAudioModel> _model = null;
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						_model = WebService.GetAllAudios(pagenumber);
					}).ContinueWith(
					t =>
					{
						if (_model != null)
						{

							for (int i = 0; i < _model.Count; i++)
							{
								_model[i].image_path = Constants.SERVER_IMG_URL + _model[i].image_path;
								items.Items.Add(_model[i]);
							}
							//Device.BeginInvokeOnMainThread(() => {items = new AllAudioItems(listAudio);
							//flowlistview.FlowItemsSource = items.Items; });

							pagenumber++;
						}
						else
						{
							pagenumber = 0;
							StaticMethods.ShowToast("No more data found");

						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
private async Task SearchAudio()
{
			pagenumber = 0;
		
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


				for (int j = 0; j<arrayTypeofAudio.Length; j++)
				{
					if (arrayTypeofAudio[j] == lblType.Text)
					{
						type = j.ToString();
					}
				}

				if (type == "0")
					type = "";

				if (category == "0")
				{
					category = "";
				}
				else if (category == "13")
				{
					category = "0";
				}

				listAudio = WebService.GetSearchAudios(type,category,txtSearch.Text);
			}).ContinueWith(
			t =>
			{
				if (listAudio != null)
				{
					if (category == "0")
						pagenumber = 1;


					for (int i = 0; i < listAudio.Count; i++)
					{
						listAudio[i].image_path = Constants.SERVER_IMG_URL + listAudio[i].image_path;
					}
					items = new AllAudioItems(listAudio);
					flowlistview.FlowItemsSource = items.Items;

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
