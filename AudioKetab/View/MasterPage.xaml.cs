using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AudioKetab
{
public partial class MasterPage : ContentPage
{
	public ListView ListView { get { return listView; } }
		public Button Logout { get { return btnLogout; } }
		public Button fb { get { return btnTw; } }
		public Button tw { get { return btnFb; } }
		public Button instagram { get { return btnInsta; } }
	public MasterPage()
	{
		InitializeComponent();
		var masterPageItems = new List<MasterPageItem>();
		masterPageItems.Add(new MasterPageItem
		{
			Title = "الصفحة الرئيسية",
			IconSource = "truck.png",
				TargetType = typeof(HomePage),
				 IsVisible=true
			
		});
		masterPageItems.Add(new MasterPageItem
		{
			Title = "تعديل الملف الشخصي",
			IconSource = "checkout.png",
				TargetType = typeof(ProfilePage),
				 IsVisible=true
			
		});
			masterPageItems.Add(new MasterPageItem
		{
			Title = "دعوة مستمعمين جدد",
			IconSource = "checkout.png",
				TargetType = typeof(AudioKetabPage),
				 IsVisible=true
			
		});
		masterPageItems.Add(new MasterPageItem
		{
			Title = "استكشاف",
			IconSource = "settings.png",
				TargetType = typeof(SearchPage),
				 IsVisible=true
			
		});
		masterPageItems.Add(new MasterPageItem
		{
			Title = "البحث عن مستخدمين آخرين",
			IconSource = "checkin.png",
				TargetType = typeof(SearchPeoplePage),
				 IsVisible=true
			
		});
			      masterPageItems.Add(new MasterPageItem
		{
			Title = "اتصل بنا",
			IconSource = "checkin.png",
				TargetType = typeof(ContactusPage),
				 IsVisible=true
			
		});
			      masterPageItems.Add(new MasterPageItem
		{
			Title = "عنا",
			IconSource = "checkin.png",
				TargetType = typeof(AboutusPage),
				 IsVisible=true
			
		});
		listView.ItemsSource = masterPageItems.Where(m => m.IsVisible == true);
	} }
}
