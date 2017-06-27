using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
namespace AudioKetab
{
	public static class WebService
	{
		static HttpClient client;
		public static JToken reading_mentor = null;
		public static JToken book_summaries = null;
		public static JToken lectureandtraining = null;
		public static JToken newsletter = null;
		public static JToken recent_added = null;
		public static JToken most_played = null;
		public static JToken recent_followers = null;
		public static PlaylistModel playlistModel = null;
		static WebService()
		{
			client = new HttpClient();
			//client.MaxResponseContentBufferSize = 256000;
		}
		public static UserModel Login(string email, string password)
		{
			UserModel um = new UserModel();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "login");
				j.Add("user_email", email);
				j.Add("user_pass", password);
				j.Add("device_token", StaticDataModel.DeviceToken);
				j.Add("device_type", "ios");


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						um.user_id = Convert.ToInt32(DataValues["user_id"].ToString());
						um.profile_pic = DataValues["profile_pic"].ToString();
						um.first_name = DataValues["first_name"].ToString();
						um.last_name = DataValues["last_name"].ToString();
						um.description = DataValues["description"].ToString();
						um.user_email = DataValues["user_email"].ToString();
						um.facebook_url = DataValues["facebook_url"].ToString();
						um.twitter_url = DataValues["twitter_url"].ToString();
						um.instagram_url = DataValues["instagram_url"].ToString();


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return um;
		}
		public static List<string> GetCountry()
		{
			List<string> list_countris = new List<string>();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "getcountry");

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);
						var data = DataValues["country_list"];
						foreach (var item in data)
						{

							list_countris.Add(item["country_name"].ToString());
						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return list_countris;
		}
		public static string SignUp(UserModel um)
		{
			string ret = string.Empty;

			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "signup");
				j.Add("user_email", um.user_email);
				j.Add("user_pass", um.user_pass);
				j.Add("first_name", um.first_name);
				j.Add("last_name", um.last_name);
				j.Add("dob", um.dob);
				j.Add("mobile", um.mobile);
				j.Add("country", um.country);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string ForgetPassword(string user_email)
		{
			string ret = string.Empty;

			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "forgotpassword");
				j.Add("user_email", user_email);


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static  string GetHomePageAudio()
		{
			string ret = string.Empty;

			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "homepage_audio");

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
             
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);
						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							reading_mentor = DataValues["reading_mentor"];
							book_summaries = DataValues["book_summries"];
							lectureandtraining = DataValues["lectureandtraining"];
							newsletter = DataValues["newsletter"];


						}

					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<AudioModel> GetPlaylist()
		{
			playlistModel = new PlaylistModel();
			List<AudioModel> audioModel = new List<AudioModel>();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "get_playlist");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);
						var category_id = DataValues["playlist"][0]["playlist_ctegoryid"].ToString();
						var data = DataValues["playlist"][0]["audio"];

						playlistModel = new PlaylistModel();
						playlistModel.playlist = DataValues["playlist"].ToObject<List<Playlist>>();
						///model.user_info=	DataValues["user_info"].ToObject<List<UserInfo>>();

						for (int i = 0; i < playlistModel.playlist.Count; i++)
						{
							for (int k = 0; k < playlistModel.playlist[i].audio.Count; k++)
							{
								audioModel.Add(playlistModel.playlist[i].audio[k]);
							}


						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return audioModel;
		}
		public static List<FollowersModel> GetFollowers()
		{
			List<FollowersModel> followerslist = new List<FollowersModel>();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "followers");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						var data = DataValues["followers"];
						var profile = string.Empty;
						foreach (var item in data)
						{
							if (string.IsNullOrEmpty(item["profile_pic"].ToString()))
							{
								profile = "default.png";  
							}
							else
							{
								profile = Constants.PRO_PIC_IMG_URL + item["profile_pic"].ToString();
							}
							followerslist.Add(new FollowersModel
							{

								follow_id = item["follow_id"].ToString(),
								user_by = item["user_by"].ToString(),
								user_to = item["user_to"].ToString(),
								date = item["date"].ToString(),
								u_id = item["u_id"].ToString(),
								first_name = item["first_name"].ToString(),
								last_name = item["last_name"].ToString(),
								email = item["email"].ToString(),
								profile_pic = profile,
								status = item["status"].ToString(),

							});

						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return followerslist;
		}
		public static List<FollowingModel> GetFollowings()
		{
			List<FollowingModel> followerslist = new List<FollowingModel>();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "following");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						var data = DataValues["following"];
						var profile = string.Empty;
						foreach (var item in data)
						{
							if (string.IsNullOrEmpty(item["profile_pic"].ToString()))
							{
								profile = "default.png";
							}
							else
							{
								profile = Constants.PRO_PIC_IMG_URL + item["profile_pic"].ToString();
							}
							followerslist.Add(new FollowingModel
							{

								follow_id = item["follow_id"].ToString(),
								user_by = item["user_by"].ToString(),
								user_to = item["user_to"].ToString(),
								date = item["date"].ToString(),
								u_id = item["u_id"].ToString(),
								first_name = item["first_name"].ToString(),
								last_name = item["last_name"].ToString(),
								email = item["email"].ToString(),
								profile_pic = profile,
								status = item["status"].ToString(),

							});

						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return followerslist;
		}

		public static List<Reading_mentorModel> GetMoreReadingMentors()
		{
			List<Reading_mentorModel> mentorslist = new List<Reading_mentorModel>();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "reading_mentors");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						var data = DataValues["reading_mentor"];
						var profile = string.Empty;
						var alreadyFollowing = false;
						var willFollow = false;
						foreach (var item in data)
						{
							if (string.IsNullOrEmpty(item["profile_pic"].ToString()))
							{
								profile = "defaultprofile.png";
							}
							else
							{
								profile = Constants.PRO_PIC_IMG_URL + item["profile_pic"].ToString();
							}
							if (item["following_status"].ToString() == "1")
							{
								alreadyFollowing = true;
								willFollow = false;
							}
							else
							{
								alreadyFollowing = false;
								willFollow = true;
							}
							mentorslist.Add(new Reading_mentorModel
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
								profile_pic = profile,
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
								following_status = item["following_status"].ToString(),
								already_following = alreadyFollowing,
								Willfollow = willFollow

							});

						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return mentorslist;
		}
		public static string GetMoreBookSummeries()
		{
			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "book_summries");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							recent_added = DataValues["recent_added"];
							most_played = DataValues["most_played"];
							recent_followers = DataValues["recent_followers"];
						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string GetMoreLectureandTraining()
		{
			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "lectureand_training");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							recent_added = DataValues["recent_added"];
							most_played = DataValues["most_played"];
							recent_followers = DataValues["recent_followers"];
						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string GetMoreNewsLetter()
		{
			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "newsletter");
				j.Add("user_id", StaticDataModel.UserId);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{
							recent_added = DataValues["recent_added"];
							most_played = DataValues["most_played"];
							recent_followers = DataValues["recent_followers"];
						}


					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<Book_summariesModel> GetAll_recent_added(string method_name, int typeofAudio)
		{
			string ret = string.Empty;
			List<Book_summariesModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", method_name);
				j.Add("typeof_audio", typeofAudio);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{
							if (method_name == "showall_booksumrecentadded")
							{
								_list = DataValues["recent_added"].ToObject<List<Book_summariesModel>>();
							}
							else
							{
								_list = DataValues["most_played"].ToObject<List<Book_summariesModel>>();

							}


						}

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static List<FollowersModel> GetAll_Followers()
		{
			string ret = string.Empty;
			List<FollowersModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "showall_recentfollwers");
				j.Add("user_id", StaticDataModel.UserId);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["recent_followers"].ToObject<List<FollowersModel>>();



						}

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static CounterModel GetAll_Counts()
		{
			string ret = string.Empty;
			CounterModel _list = new CounterModel();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "get_count");
				j.Add("user_id", StaticDataModel.UserId);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);
						_list.result = DataValues["result"].ToString();
						_list.playlist_count = Convert.ToInt32(DataValues["playlist_count"].ToString());
						_list.follower_count = Convert.ToInt32(DataValues["follower_count"].ToString());
						_list.following_count = Convert.ToInt32(DataValues["following_count"].ToString());
						_list.myaudio_count = Convert.ToInt32(DataValues["myaudio_count"].ToString());
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static string getAboutus()
		{
			string ret = string.Empty;
			CounterModel _list = new CounterModel();
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "about_usdetails");


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);
						ret = DataValues["about_detail"][0]["about_us_desc_ar"].ToString();
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string followUser(int follow_userid)
		{
			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "follow_users");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("follow_userid", follow_userid);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string unFollowUser(int unfollow_userid)
		{
			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "unfollow_users");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("follow_userid", unfollow_userid);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string contactUs(string name, string email, string subject, string msg)
		{
			string ret = string.Empty;

			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "contact_us");
				j.Add("name", name);
				j.Add("email", email);
				j.Add("subject", subject);
				j.Add("message", msg);


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				StaticMethods.DismissLoader();
			}
			finally
			{

			}

			return ret;
		}
		public static List<Book_summariesModel> GetUploadedAudio(int userid)
		{
			string ret = string.Empty;
			List<Book_summariesModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "my_audio");
				j.Add("user_id", userid);



				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["audio"].ToObject<List<Book_summariesModel>>();

						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				StaticMethods.DismissLoader();
			}
			finally
			{

			}

			return _list;
		}
		public static ProfileModel GetProfile(int userid)
		{
			string ret = string.Empty;
			ProfileModel _model = new ProfileModel();

			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "show_profile");
				j.Add("user_id", userid);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_model = DataValues["users_details"].ToObject<ProfileModel>();



						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
				StaticMethods.DismissLoader();
			}
			finally
			{

			}

			return _model;
		}
		public static ProfileModel UpdateProfile(ProfileModel user_model)
		{
			string ret = string.Empty;
			ProfileModel _model = new ProfileModel();

			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "update_profile");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("first_name", user_model.first_name);
				j.Add("last_name", user_model.last_name);
				j.Add("country_name", user_model.country_name);
				j.Add("mobile_number", user_model.mobile_no);
				j.Add("dob", user_model.dob);
				j.Add("description", user_model.description);
				j.Add("profile_pic", user_model.profile_pic);


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);


						_model = DataValues["users_details"].ToObject<ProfileModel>();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();

			}

			return _model;
		}
		public static async Task<string> UploadAudio(Book_summariesModel bm)
		{
			try
			{

				using (var client = new HttpClient())
				{
					using (var content =
						new MultipartFormDataContent())
					{
						content.Add(new StringContent("add_audio"), "method");
						content.Add(new StringContent(StaticDataModel.UserId.ToString()), "user_id");
						content.Add(new StringContent(bm.book_name), "book_name");
						content.Add(new StringContent(bm.author_name), "author_name");
						content.Add(new StringContent(""), "comment");
						content.Add(new StringContent(bm.image_path), "image_path");
						if (bm.byte_recorded_audio == null)
							content.Add(new StringContent("0"), "recorded_audio");
						else
							content.Add(new StringContent("1"), "recorded_audio");
						content.Add(new StringContent(""), "other_category");
						content.Add(new StringContent(bm.typeof_audio), "typeof_audio");
						content.Add(new StringContent(bm.article_url), "article_url");
						content.Add(new StringContent(bm.article_url), "video_url");
						content.Add(new StreamContent(new MemoryStream(bm.byte_recorded_audio)), "song_path", "upload.mp3");


						using (

						   var message =
							await client.PostAsync(Constants.ADD_AUIDO_URL, content))
						{
							if (message.IsSuccessStatusCode)
							{
								StaticMethods.ShowToast("File uploaded successfully");

							}

							var input = await message.Content.ReadAsStringAsync();

							return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;
						}
					}
				}

			}

			catch (Exception ex)
			{

			}
			finally
			{
				StaticMethods.DismissLoader();
			}
			return null;
		}
		public static string LikeAudio(int s_id, int status, int song_userid)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "like_song");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("songuser_id", song_userid);
				j.Add("s_id", s_id);
				j.Add("status", status);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string GetLikeStatus(int s_id)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "get_likestatus");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("s_id", s_id);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["like_status"].ToString() + "," + DataValues["playlist_status"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<CommentModel> GetAllComments(int s_id)
		{
			string ret = string.Empty;
			List<CommentModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "comment_data");
				j.Add("s_id", s_id);



				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["tracklist"].ToObject<List<CommentModel>>();

						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static string SendComment(int s_id, string comment)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "comment_song");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("s_id", s_id);
				j.Add("comment", comment);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string DeleteComment(int s_id, int comment_id)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "delete_comment");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("s_id", s_id);
				j.Add("comment_id", comment_id);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<AddtoPlaylistModel> getAlradyInPlaylist()
		{
			string ret = string.Empty;
			List<AddtoPlaylistModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "show_userplaylist");
				j.Add("user_id", StaticDataModel.UserId);



				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["playlist"].ToObject<List<AddtoPlaylistModel>>();

						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static string AddToPlaylist(int s_id, string playlist, int status)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "addto_playlist");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("s_id", s_id);
				j.Add("playlist", playlist);
				j.Add("status", status);
				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<RootObject> GetChatUserList()
		{
			string ret = string.Empty;
			List<RootObject> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "getmsgusers_list");
				j.Add("user_id", StaticDataModel.UserId);



				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["users_list"].ToObject<List<RootObject>>();

						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static List<ChatModel> GetChatConversation(int userid)
		{
			string ret = string.Empty;
			List<ChatModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "userschat_details");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("chatuser_id", userid);



				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["msg_list"].ToObject<List<ChatModel>>();

						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static string SendMessage(int reciever_id, string msg)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "msg_send");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("receiver_id", reciever_id);
				j.Add("msg", msg);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<SearchUserModel> SearchChatUser(string name)
		{
			string ret = string.Empty;
			List<SearchUserModel> _list = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "search_users");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("search_val", name);


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{

							_list = DataValues["users_list"].ToObject<List<SearchUserModel>>();

						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return _list;
		}
		public static UserDetailsModel GetUserDetails(int userid)
		{
			string ret = string.Empty;
			UserDetailsModel model = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "get_usersdetails");
				j.Add("user_id", userid);



				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						var DataValues = JValue.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{


							model = new UserDetailsModel();
							model.audio_list = DataValues["audio_list"].ToObject<List<AudioList>>();
							model.user_info = DataValues["user_info"].ToObject<List<UserInfo>>();
							model.follower_count =Convert.ToInt32( DataValues["follower_count"].ToString());
							model.following_count = Convert.ToInt32( DataValues["following_count"].ToString());
							model.myaudio_count =Convert.ToInt32( DataValues["myaudio_count"].ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);

			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return model;
		}
		public static string CheckFollow(int follow_userid)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "chk_follow");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("follow_userid", follow_userid);


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						ret = DataValues["responseMessage"].ToString();
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string UpdatePlaylistCatefory(int playlistCategoryId, string playlistName)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "updateplaylist_category");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("playlistcategory_id", playlistCategoryId);
				j.Add("playlistname", playlistName);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static string RemoveFromPlaylist(int playlistId)
		{

			string ret = string.Empty;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "removeplaylist_audio");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("playlist_id", playlistId);


				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();

					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return ret;
		}
		public static List<AllAudioModel> GetAllAudios(int paging)
		{

			string ret = string.Empty;
			List<AllAudioModel> listAudio = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "all_audio");
				j.Add("page", paging);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{
							listAudio = DataValues["tracklist"].ToObject<List<AllAudioModel>>();
						}
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return listAudio;
		}
		public static List<AllAudioModel> GetSearchAudios(string typeof_audio, string category, string search_val)
		{

			string ret = string.Empty;
			List<AllAudioModel> listAudio = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "all_audiosearch");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("typeof_audio", typeof_audio);
				j.Add("category", category);
				j.Add("search_val", search_val);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{
							listAudio = DataValues["tracklist"].ToObject<List<AllAudioModel>>();
						}
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return listAudio;
		}
		public static List<UserSearchModel> SearchPeople(string search_val)
		{

			string ret = string.Empty;
			List<UserSearchModel> listPeople = null;
			try
			{
				HttpResponseMessage response = null;
				JObject j = new JObject();
				j.Add("method", "search_users");
				j.Add("user_id", StaticDataModel.UserId);
				j.Add("search_val", search_val);

				var json = JsonConvert.SerializeObject(j);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = client.PostAsync(Constants.BASE_URL, content).Result;
				if (response.IsSuccessStatusCode)
				{
					using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
					{
						var contents = reader.ReadToEnd();
						JObject DataValues = JObject.Parse(contents);

						ret = DataValues["result"].ToString();
						if (ret == "success")
						{
							listPeople = DataValues["users_list"].ToObject<List<UserSearchModel>>();
						}
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			finally
			{
				StaticMethods.DismissLoader();
			}

			return listPeople;
		}
public static string  SearchByCategory(string search_val,string typeof_audio,string category)
{

	string ret = string.Empty;
	List<UserSearchModel> listPeople = null;
	try
	{
		HttpResponseMessage response = null;
		JObject j = new JObject();
		j.Add("method", "search_bycategory");
		j.Add("user_id", StaticDataModel.UserId);
		j.Add("typeof_audio", typeof_audio); 
		j.Add("category", category);  
		j.Add("search_val", search_val);  

		var json = JsonConvert.SerializeObject(j);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		response = client.PostAsync(Constants.BASE_URL, content).Result;
		if (response.IsSuccessStatusCode)
		{
			using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
			{
				var contents = reader.ReadToEnd();
				JObject DataValues = JObject.Parse(contents);

				ret = DataValues["result"].ToString();
				if (ret == "success")
				{
					        recent_added = DataValues["recent_added"];
							most_played = DataValues["most_played"];
							recent_followers = DataValues["recent_followers"];
				}
			}

		}
	}
	catch (Exception ex)
	{
		Debug.WriteLine(@"ERROR {0}", ex.Message);
	}
	finally
	{
		StaticMethods.DismissLoader();
	}

	return ret;
		}
	}
}
