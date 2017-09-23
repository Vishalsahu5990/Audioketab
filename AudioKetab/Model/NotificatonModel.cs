using System;
using System.Collections.Generic;

namespace AudioKetab
{
    public class NotificatonModel
    {
        public class RemoteNoti
        {
          
			public string message_id { get; set; }
			public string msg { get; set; }
			public string notification_type { get; set; }
			public string registration_ids { get; set; }
			public string to_id { get; set; }
			public string user_id { get; set; }
        }
		public class OtherNotification
		{
			public string id { get; set; }
			public string notification_sender { get; set; }
			public string notification_type { get; set; }
			public string users_id { get; set; }
			public string notification_desc { get; set; }
			public string notification_descarabic { get; set; }
			public string song_id { get; set; }
			public string delete_status { get; set; }
		} 

		
			public string result { get; set; }
			public int msg_notificationcount { get; set; }
			public List<object> msg_notification { get; set; }
			public int other_notificationcount { get; set; }
			public List<OtherNotification> other_notification { get; set; }
		
    } 
} 
