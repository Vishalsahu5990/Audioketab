using System;
using Acr.UserDialogs;
using Plugin.SecureStorage;
using Xamarin.Forms;
namespace AudioKetab
{
	public static class StaticMethods
	{
		public static bool IsIpad()
		{
			if (Device.Idiom == TargetIdiom.Phone)
				return false;
			else
				return true;
		}
		public static void ShowToast(string msg)
		{
			try
			{

				UserDialogs.Instance.Toast(msg);

			}
			catch (Exception ex)
			{

			}
		}
		public static void ShowLoader()
		{
			try
			{

				UserDialogs.Instance.ShowLoading();

			}
			catch (Exception ex)
			{

			}
		}
		public static void DismissLoader()
		{
			try
			{

				UserDialogs.Instance.HideLoading();

			}
			catch (Exception ex)
			{

			}
		}
		public static UserModel GetLocalSavedData()
		{
			UserModel um = null;
			try
			{
				um = new UserModel();
				um.user_id = Convert.ToInt32(CrossSecureStorage.Current.GetValue("userId", null));
				um.profile_pic = CrossSecureStorage.Current.GetValue("profilePic", null);
				um.first_name = CrossSecureStorage.Current.GetValue("firstName", null);
				um.last_name = CrossSecureStorage.Current.GetValue("lastName", null);
				um.description = CrossSecureStorage.Current.GetValue("description", null);
				um.user_email = CrossSecureStorage.Current.GetValue("userEmail", null);
				um.facebook_url = CrossSecureStorage.Current.GetValue("fb", null);
				um.twitter_url = CrossSecureStorage.Current.GetValue("tw", null);
				um.instagram_url = CrossSecureStorage.Current.GetValue("insta", null);
				return um;
			}
			catch (Exception ex)
			{
				return null;
			}


		}
		public static void DeleteLocalSavedData()
		{
			UserModel um = null;
			try
			{

				DependencyService.Get<IiOSMethods>().DeleteLocalData();

			}
			catch (Exception ex)
			{

			}


		}
		public static string[] GetStaticCateogories()
		{
			String[] items = null;
			try
			{

				items = new String[] { "آخرون", "الأدب والشعر", "سياسة واقتصاد", "الأسرة وعلم الاجتماع والمجتمع", "تطوير الذات", "علوم طبية", "الفكر والثقافة", "التاريخ والجغرافيا", "الكمبيوتر والتكنولوجيا", "القصص والروايات", "سير و مذكرات", "العلوم والهندسة", "الكتب الإسلامية", "اختر الفئة" };


			}
			catch (Exception ex)
			{

			}
			return items;
		}
		public static string[] GetStaticTypeofAudio()
		{
			String[] items = null;
			try
			{

				items = new String[] { "آخرون", "Book summaries", "Lectures and Training", "Newsletter and talks" };


			}
			catch (Exception ex)
			{

			}
			return items;
		}

	}
}
