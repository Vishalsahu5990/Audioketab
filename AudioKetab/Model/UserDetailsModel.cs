using System;
using System.Collections.Generic;

namespace AudioKetab
{
	public class UserDetailsModel
	{
public string result { get; set; }
public List<UserInfo> user_info { get; set; }
public List<AudioList> audio_list { get; set; }
public int follower_count { get; set; }
public int following_count { get; set; }
public int myaudio_count { get; set; }	
	}
public class UserInfo
{
	public string u_id { get; set; }
	public string first_name { get; set; }
	public string last_name { get; set; }
	public string user_name { get; set; }
	public string country_name { get; set; }
	public string mobile_no { get; set; }
	public string email { get; set; }
	public string password { get; set; }
	public string password_dicrypt { get; set; }
	public string status { get; set; }
	public string dob { get; set; }
	public string from { get; set; }
	public string profile_pic { get; set; }
	public string cover_pic { get; set; }
	public string description { get; set; }
	public string delete_status { get; set; }
	public string register_date { get; set; }
	public string features_users { get; set; }
	public string v_status { get; set; }
	public string forgotpassword_status { get; set; }
	public string invite_status { get; set; }
	public string gmail_invite { get; set; }
	public string outlook_invite { get; set; }
	public string device_token { get; set; }
	public string device_type { get; set; }
}

public class AudioList
{
	public string s_id { get; set; }
	public string user_id { get; set; }
	public object typeof_audio { get; set; }
	public string book_name { get; set; }
	public string author_name { get; set; }
	public string category { get; set; }
	public string other_category { get; set; }
	public string comment { get; set; }
	public string song_path { get; set; }
	public string image_path { get; set; }
	public string recorded_audio { get; set; }
	public object article_url { get; set; }
	public object video_url { get; set; }
	public string count_like { get; set; }
	public string count_comment { get; set; }
	public string count_playlist { get; set; }
	public string count_share { get; set; }
	public string count_listioner { get; set; }
	public string status { get; set; }
	public string delete_status { get; set; }
	public string date { get; set; }
	public string notifications_status { get; set; }
}


}
