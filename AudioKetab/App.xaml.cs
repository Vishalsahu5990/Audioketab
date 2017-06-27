using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class App : Application
	{
		public static double ScreenHeight;
		public static double ScreenWidth;
		public App() 
		{
			InitializeComponent(); 
          
			 
			MainPage = new NavigationPage( new SplashScreenPage());//To test  

		}

		protected override void OnStart()  
		{ 
			// Handle when your app starts
		}

		protected override void OnSleep() 
		{
			// Handle when your app sleeps
		}

		protected override void OnResume() 
		{ 
			// Handle when your app resumes
		}
	}
}
