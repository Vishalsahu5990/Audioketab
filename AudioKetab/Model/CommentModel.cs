using System;
namespace AudioKetab
{
	public class CommentModel
	{
		public string comment_id { get; set; }
		public string user_id { get; set; }
		public string song_id { get; set; }
		public string comment_description { get; set; }
		public string c_date { get; set; }
		public string u_id { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public object user_name { get; set; }
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
		public bool isVisible { get; set; }

	}
}
