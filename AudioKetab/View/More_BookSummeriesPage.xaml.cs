using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
    public partial class More_BookSummeriesPage : ContentPage
    {
        List<Book_summariesModel> list_recentadded = null;
        List<Book_summariesModel> list_mostplayed = null;
        List<Book_summariesModel> list_recentfollower = null;
        BindingSourceModel items = null;
        BindingSourceModel items1 = null;
        BindingSourceModel items2 = null;
        private double cellSize = 0;
        string[] arrayCategory;
        MainPage _context;
        public More_BookSummeriesPage()
        {

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //this.BindingContext = new BindingSourceModel() { };
        }
        async void one_clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (Xamarin.Forms.Button)sender;
                for (int i = 0; i < list_recentadded.Count; i++)
                {
                    if (item.CommandParameter.ToString() == list_recentadded[i].s_id)
                    {
                        if (item != null)
                        {
                            await Navigation.PushModalAsync(new AudioPlayerPage(_context, list_recentadded[i]));

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        async void two_clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (Xamarin.Forms.Button)sender;
                for (int i = 0; i < list_mostplayed.Count; i++)
                {
                    if (item.CommandParameter.ToString() == list_mostplayed[i].s_id)
                    {
                        if (item != null)
                        {
                            await Navigation.PushModalAsync(new AudioPlayerPage(_context, list_mostplayed[i]));

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        async void three_clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (Xamarin.Forms.Button)sender;
                for (int i = 0; i < list_recentfollower.Count; i++)
                {
                    if (item.CommandParameter.ToString() == list_recentfollower[i].s_id)
                    {
                        if (item != null)
                        {
                            await Navigation.PushModalAsync(new AudioPlayerPage(_context, list_recentfollower[i]));

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
            GetAllData().Wait();
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

        void Categoryypicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = categoryypicker.Items[categoryypicker.SelectedIndex];
            lblCategory.Text = item;
        }
        public More_BookSummeriesPage(MainPage context)
        {
            _context = context;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            cellSize = App.ScreenWidth / 4.5;
            //StaticMethods.ShowLoader();

            //listview_recentadded.ItemSelected += async (sender, e) =>
            //  {

            //	  try
            //	  {
            //		  var item = e.Item as Book_summariesModel;
            //		  if (item != null)
            //		  {
            //			  await Navigation.PushModalAsync(new AudioPlayerPage(_context, item));

            //		  }
            //	  }
            //	  catch (Exception ex)
            //	  {

            //	  }

            //  };
            //listview_mostplayed.ItemSelected += async (sender, e) =>
            //{
            // try
            // {
            //  var item = e.Item as Book_summariesModel;
            //  if (item != null)
            //  {
            //	  await Navigation.PushModalAsync(new AudioPlayerPage(_context, item));
            //  }
            // }
            // catch (Exception ex)
            // {

            // }
            //};
            //listview_recentfollowers.ItemSelected += (sender, e) =>
            //{
            // var item = e.Item as Book_summariesModel;

            //};
            btnMoreRecentAdded.Clicked += BtnMoreRecentAdded_Clicked;
            btnMoreMostPlayed.Clicked += BtnMoreMostPlayed_Clicked;
            btnMoreRecentFollowers.Clicked += BtnMoreRecentFollowers_Clicked;
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
        async void BtnMoreRecentAdded_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AllItemsPage(11));
        }
        async void BtnMoreMostPlayed_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AllItemsPage(21));
        }

        async void BtnMoreRecentFollowers_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AllItemsPage(31));
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

        private void ProcessResult(bool flag)
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
                            article_url = item["article_url"].ToString(),
                            video_url = item["video_url"].ToString(),
                            count_comment = item["count_comment"].ToString(),
                            count_playlist = item["count_playlist"].ToString(),
                            count_share = item["count_share"].ToString(),
                            count_listioner = item["count_listioner"].ToString(),
                            status = item["status"].ToString(),
                            notifications_status = item["notifications_status"].ToString(),
                            delete_status = item["delete_status"].ToString(),

                            cell_size = cellSize
                        });
                    }


                }
                //listview_recentadded.ItemsSource = list_recentadded;
                //listview_recentadded.BindingContext = new BindingSourceModel()
                //{
                //	_list_recentadded = list_recentadded
                //};
                //items = new BindingSourceModel(list_recentadded);
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
                            article_url = item["article_url"].ToString(),
                            video_url = item["video_url"].ToString(),
                            count_comment = item["count_comment"].ToString(),
                            count_playlist = item["count_playlist"].ToString(),
                            count_share = item["count_share"].ToString(),
                            count_listioner = item["count_listioner"].ToString(),
                            status = item["status"].ToString(),
                            notifications_status = item["notifications_status"].ToString(),
                            delete_status = item["delete_status"].ToString(),

                            cell_size = cellSize
                        });
                    }

                }
                //items1 = new BindingSourceModel(list_mostplayed);
                listview_mostplayed.ItemsSource = list_mostplayed;
                if (_recentFollower != null)
                {
                    foreach (var item in _recentFollower)
                    {
                        list_recentfollower.Add(new Book_summariesModel
                        {


                            s_id = item["follow_id"].ToString(),
                            user_id = item["u_id"].ToString(),
                            first_name = item["first_name"].ToString() + " " + item["last_name"].ToString(),
                            image_path = Constants.PRO_PIC_IMG_URL + item["image_path"].ToString(),

                            last_name = item["last_name"].ToString(),
                            user_to = item["user_to"].ToString(),
                            user_by = item["user_by"].ToString(),

                            cell_size = cellSize
                        });
                    }

                }


                //items2 = new BindingSourceModel(list_recentfollower);
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
                        ProcessResult(false);
                    });




                        }
                        else
                        {
                            StaticMethods.ShowToast("No result found!");

                        }


                    }, TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
        private async Task GetAllData()
        {
            string category = string.Empty;

            string ret = string.Empty;
            StaticMethods.ShowLoader();
            Task.Factory.StartNew(
                    // tasks allow you to use the lambda syntax to pass wor
                    () =>
                    {
                        ret = WebService.GetMoreBookSummeries();

                    }).ContinueWith(
                    t =>
                    {
                        if (ret == "success")
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                ProcessResult(true);
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
