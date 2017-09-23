using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using AudioKetab;
using Plugin.SecureStorage;

namespace AudioKetab
{
	public partial class MainPage : MasterDetailPage
	{
		UserModel _usermodel = null;
		AudioKetab.MasterPage menuPage;
		public static readonly BindableProperty PinTappedCommandProperty = BindableProperty.Create<HomePage, ICommand>(x => x.OpenMenuCommand, null);
		public static readonly BindableProperty ChangeMenuCommandProperty = BindableProperty.Create<ContactusPage, ICommand>(x => x.ChangeMenuCommand, null);

		public Command MenuTapped
		{
			get { return (Command)GetValue(PinTappedCommandProperty); }
			set
			{
				SetValue(PinTappedCommandProperty, value);
			}
		}
		public Command ChangeMenu
		{
			get { return (Command)GetValue(ChangeMenuCommandProperty); }
			set
			{
				SetValue(ChangeMenuCommandProperty, value);
			}
		}

		public MainPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			StaticDataModel.CurrentContext = this;
			GetUserData();
			menuPage = new MasterPage();
			Master = menuPage;
			Detail = new NavigationPage(new HomePage(this));

			menuPage.ListView.ItemSelected += OnItemSelected;
			menuPage.Logout.Clicked += Logout_Clicked;
			menuPage.tw.Clicked += Tw_Clicked;
			menuPage.fb.Clicked += Fb_Clicked;
			menuPage.instagram.Clicked += Instagram_Clicked;
			MenuTapped = new Command(async (x) =>
			{
				Device.BeginInvokeOnMainThread(async () =>
											   {
												   this.IsPresented = true;
											   });
			});

			ChangeMenu = new Command(async (x) =>
			{
				Device.BeginInvokeOnMainThread(async () =>
											   {
												   this.IsPresented = false;
												   Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage), this));

											   });
			});
			_usermodel = StaticMethods.GetLocalSavedData();
		}
		private void GetUserData()
		{

			try
			{
				var exists = CrossSecureStorage.Current.HasKey("userId");
				if (exists)
				{
					var id = CrossSecureStorage.Current.GetValue("userId", null);
					if (id != null)
						StaticDataModel.UserId = Convert.ToInt32(id);

               

				}
			}
			catch (Exception ex)
			{

			}
		}
		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			try
			{
				var item = e.SelectedItem as MasterPageItem;
				if (item != null)
				{
					if (item.TargetType==typeof(AudioKetabPage)) 
					{
						IsPresented = false;
						//menuPage.ListView.SelectedItem = null;
						DependencyService.Get<IiOSMethods>().SendAppInvitation();
						//DisplayAlert("Message", "Out of memory exception!", "Ok");
					}
					else
					{
						if (item.TargetType == typeof(HomePage))
						{
							if (!StaticDataModel.IsFromHomePage)
							{
								Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, this));
								//menuPage.ListView.SelectedItem = null;
								IsPresented = false;
							}
							else
							{
								//menuPage.ListView.SelectedItem = null;
								IsPresented = false;
							}
						}
						else
						{
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, this));
							//menuPage.ListView.SelectedItem = null;
							IsPresented = false;
						}

					}
				}
			}
			catch (Exception ex)
			{

			}
			finally 
			{ 
			    IsPresented = false;
				//menuPage.ListView.SelectedItem = null;
			}
		}

		void Tw_Clicked(object sender, EventArgs e)
		{
			if (_usermodel != null)
			{
				if (!string.IsNullOrEmpty(_usermodel.facebook_url))
					Device.OpenUri(new Uri(_usermodel.facebook_url));
			}


		}

		void Fb_Clicked(object sender, EventArgs e)
		{
			if (_usermodel != null)
			{
				if (!string.IsNullOrEmpty(_usermodel.twitter_url))
					Device.OpenUri(new Uri(_usermodel.twitter_url));
			}
		}

		void Instagram_Clicked(object sender, EventArgs e)
		{
			if (_usermodel != null)
			{
				if (!string.IsNullOrEmpty(_usermodel.instagram_url))
					Device.OpenUri(new Uri(_usermodel.instagram_url));
			}
		}

		async void Logout_Clicked(object sender, EventArgs e)
		{
			var action = await DisplayAlert("aleart", "Confirm Logout?", "Yes", "No");
			if (action)
			{
				CrossSecureStorage.Current.DeleteKey("userId");
				CrossSecureStorage.Current.DeleteKey("profilePic");
				CrossSecureStorage.Current.DeleteKey("firstName");
				CrossSecureStorage.Current.DeleteKey("lastName");
				CrossSecureStorage.Current.DeleteKey("userEmail");
				App.Current.MainPage = new NavigationPage(new LoginPage());

			}

		}
	}


}

