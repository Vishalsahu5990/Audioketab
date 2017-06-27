using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class SplashScreenPage : ContentPage
	{
		public SplashScreenPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (StaticMethods.IsIpad())
				await SlideLogo(this.imgLogo, 130, 10, 5);
			else
				await SlideLogo(this.imgLogo, 50, 10, 5);

			var exists = CrossSecureStorage.Current.HasKey("userId");
			if (!exists)
				await Navigation.PushModalAsync(new LoginPage());
			else
				await Navigation.PushModalAsync(new MainPage());

		}
		public static async Task SlideLogo(View view, int Top, int reduce, int time)
		{

			do
			{

				await view.TranslateTo(view.TranslationX, view.TranslationY - Top, 500, Easing.Linear);


				//await view.TranslateTo(view.TranslationX, view.TranslationY + Top, 300, Easing.Linear);

				//await view.TranslateTo(view.TranslationX, view.TranslationY - Top, 300, Easing.Linear);

				//await view.TranslateTo(view.TranslationX, view.TranslationY + Top, 150, Easing.Linear);

				//await view.TranslateTo(view.TranslationX, view.TranslationY - Top, 150, Easing.Linear);

				//await view.TranslateTo(view.TranslationX, view.TranslationY + Top, 100, Easing.Linear);

				Top = Top - reduce;
				time--;
			} while (time != 0); 




			//await view.FadeTo(-0, 1000);
		}
	}
}
