using System;
using System.Collections.Generic;

namespace AudioKetab
{

	public class PlaylistModel
	{

		public string result { get; set; }
		public List<Playlist> playlist { get; set; }

	}
	public class Playlist
	{
		public string playlist_ctegoryid { get; set; }
		public string playlist_userid { get; set; }
		public string playlist_category { get; set; }
		public string playlist_Date { get; set; }
		public List<AudioModel> audio { get; set; }


	}
	public class AudioModel
	{
		public string playlist_id { get; set; }
		public string playlist_categoryid { get; set; }
		public string user_id { get; set; }
		public string song_id { get; set; }
		public string date { get; set; }
		public string s_id { get; set; }
		public string typeof_audio { get; set; }
		public string book_name { get; set; }
		public string author_name { get; set; }
		public string category { get; set; }
		public string other_category { get; set; }
		public string comment { get; set; }
		public string song_path { get; set; }
		public string image_path { get; set; }
		public string recorded_audio { get; set; }
		public string article_url { get; set; }
		public string video_url { get; set; }
		public string count_like { get; set; }
		public string count_comment { get; set; }
		public string count_playlist { get; set; }
		public string count_share { get; set; }
		public string count_listioner { get; set; }
		public string status { get; set; }
		public string delete_status { get; set; }
		public string notifications_status { get; set; }
		public string playlist_ctegoryid { get; set; }
		public string playlist_userid { get; set; }
		public string playlist_category { get; set; }
		public string playlist_Date { get; set; }
	}
}
