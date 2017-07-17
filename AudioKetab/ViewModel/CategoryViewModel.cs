using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioKetab
{
	public class CategoryViewModel : BaseViewModel
	{
		List<Reading_mentorModel> list_ReadingMentor = null;
		List<Book_summariesModel> list_BookSummeries = null;
		List<LectureandTraingingModel> list_LectureTraining = null;
		List<NewsLetterModel> list_NewsLetter = null;
		private double cellSize = 0;
		private ObservableCollection<LectureandTraingingModel> _list_LectureTraining = new ObservableCollection<LectureandTraingingModel>();
		private ObservableCollection<Reading_mentorModel> _list_ReadingMentor = new ObservableCollection<Reading_mentorModel>();
		private ObservableCollection<Book_summariesModel> _list_BookSummeries = new ObservableCollection<Book_summariesModel>();
		private ObservableCollection<NewsLetterModel> _list_NewsLetter = new ObservableCollection<NewsLetterModel>();



		public ObservableCollection<LectureandTraingingModel> List_LectureTraining
		{
			get { return _list_LectureTraining; }
			set
			{
				value = _list_LectureTraining;
				ChangeAndNotify(ref this._list_LectureTraining, value, "List_LectureTraining");
			}
		}
		public ObservableCollection<Reading_mentorModel> List_ReadingMentor
		{
			get { return _list_ReadingMentor; }
			set
			{
				value = _list_ReadingMentor;
				ChangeAndNotify(ref this._list_ReadingMentor, value, "List_ReadingMentor");
			}
		}
		public ObservableCollection<Book_summariesModel> List_BookSummeries
		{
			get { return _list_BookSummeries; }
			set
			{
				value = _list_BookSummeries;
				ChangeAndNotify(ref this._list_BookSummeries, value, "List_BookSummeries");
			}
		}
		public ObservableCollection<NewsLetterModel> List_NewsLetter
		{
			get { return _list_NewsLetter; }
			set
			{
				value = _list_NewsLetter;
				ChangeAndNotify(ref this._list_NewsLetter, value, "List_NewsLetter");
			}
		}
		private Command _selectedItemCommand;

		public Command SelectedItemCommand
		{
			get
			{
				return _selectedItemCommand ?? (_selectedItemCommand = new Command(async () =>
				{
					//TODO: Selected item
				}));
			}
		}

		public CategoryViewModel()
		{
			cellSize = App.ScreenWidth / 4.3;
			this.BindData();
		}

		private async void BindData()
		{

			//await GetAudio();

			var ret = WebService.GetHomePageAudio();
			if (ret == "success")
			{

				ProcessResult();



			}
		}
		private async Task GetAudio()
		{
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
						ret = WebService.GetHomePageAudio();
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{
							Device.BeginInvokeOnMainThread(() =>
					{
						ProcessResult();
					});




						}


					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
		private void ProcessResult()
		{
			list_ReadingMentor = new List<Reading_mentorModel>();
			list_BookSummeries = new List<Book_summariesModel>();
			list_LectureTraining = new List<LectureandTraingingModel>();
			list_NewsLetter = new List<NewsLetterModel>();
			try
			{
				var _readingmentor = WebService.reading_mentor;
				var _booksummeries = WebService.book_summaries;
				var _lectureandTraining = WebService.lectureandtraining;
				var _newsLetter = WebService.newsletter;
				if (_readingmentor != null)
				{
					foreach (var item in _readingmentor) 
					{
						list_ReadingMentor.Add(new Reading_mentorModel
						{
							u_id = item["u_id"].ToString(),
							first_name = item["first_name"].ToString(),
							last_name = item["last_name"].ToString(),
							user_name = item["user_name"].ToString(),
							country_name = item["country_name"].ToString(),
							mobile_no = item["mobile_no"].ToString(),
							email = item["email"].ToString(),
							password = item["password"].ToString(),
							status = item["status"].ToString(),
							dob = item["dob"].ToString(),
							from = item["from"].ToString(),
							profile_pic = Constants.PRO_PIC_IMG_URL + item["profile_pic"].ToString(),
							cover_pic = item["cover_pic"].ToString(),
							description = item["description"].ToString(),
							delete_status = item["delete_status"].ToString(),
							register_date = item["register_date"].ToString(),
							features_users = item["features_users"].ToString(),
							v_status = item["v_status"].ToString(),
							forgotpassword_status = item["forgotpassword_status"].ToString(),
							invite_status = item["invite_status"].ToString(),
							gmail_invite = item["gmail_invite"].ToString(),
							outlook_invite = item["outlook_invite"].ToString(),
							device_token = item["device_token"].ToString(),
							device_type = item["device_type"].ToString(),
							cell_size = cellSize, 
							listview_height = cellSize + 20 

						});
					}




				}

				if (_booksummeries != null)
				{
					foreach (var item in _booksummeries)
					{
						list_BookSummeries.Add(new Book_summariesModel
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
							u_id = item["u_id"].ToString(),
							profile_pic = item["profile_pic"].ToString(),
							last_name = item["last_name"].ToString(),
							first_name = item["first_name"].ToString(),
							delete_status = item["delete_status"].ToString(),
							article_url = item["article_url"].ToString(),
							video_url = item["video_url"].ToString(),
							description = item["description"].ToString(),
							cell_size = cellSize,
							listview_height = cellSize + 20 

						});
					}

				}
				if (_lectureandTraining != null)
				{
					foreach (var item in _lectureandTraining)
					{
						list_LectureTraining.Add(new LectureandTraingingModel
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
							u_id = item["u_id"].ToString(),
							profile_pic = item["profile_pic"].ToString(),
							last_name = item["last_name"].ToString(),
							first_name = item["first_name"].ToString(),
							delete_status = item["delete_status"].ToString(),
							article_url = item["article_url"].ToString(),
							video_url = item["video_url"].ToString(),
							description = item["description"].ToString(),
							cell_size = cellSize,
							listview_height = cellSize + 20 

						});
					}

				}
				if (_newsLetter != null)
				{
					foreach (var item in _newsLetter)
					{
						list_NewsLetter.Add(new NewsLetterModel
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
							u_id = item["u_id"].ToString(),
							profile_pic = item["profile_pic"].ToString(),
							last_name = item["last_name"].ToString(),
							first_name = item["first_name"].ToString(),
							delete_status = item["delete_status"].ToString(),
							article_url = item["article_url"].ToString(),
							video_url = item["video_url"].ToString(),
							cell_size = cellSize,
							listview_height = cellSize + 20

						});
					}

				}



				foreach (LectureandTraingingModel itm in list_LectureTraining)
				{
					List_LectureTraining.Add(itm);
				}

				List_ReadingMentor = new ObservableCollection<Reading_mentorModel>();
				foreach (Reading_mentorModel itm in list_ReadingMentor)
				{
					List_ReadingMentor.Add(itm);
				}

				List_BookSummeries = new ObservableCollection<Book_summariesModel>();
				foreach (Book_summariesModel itm in list_BookSummeries)
				{
					List_BookSummeries.Add(itm);
				}

				List_NewsLetter = new ObservableCollection<NewsLetterModel>();
				foreach (NewsLetterModel itm in list_NewsLetter)
				{
					List_NewsLetter.Add(itm);
				}


			}
			catch (Exception ex)
			{


			}

		}
	}
}

