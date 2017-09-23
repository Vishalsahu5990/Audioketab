using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class LoginPage : ContentPage
	{
		UserModel usermodel = null;
		public LoginPage()
		{
			InitializeComponent();
			//wire up events
			txtEmail.Focused += TxtEmail_Focused;
			txtEmail.Unfocused += TxtEmail_Unfocused;
			txtPassword.Focused += TxtPassword_Focused;
			txtPassword.Unfocused+= TxtPassword_Unfocused;

			NavigationPage.SetHasNavigationBar(this, false);
			PrepareUI();

			//txtEmail.Text = "mishraprabhakar10@gmail.com";
			//txtPassword.Text = "123@123";
		}
		void TxtEmail_Focused(object sender, FocusEventArgs e)
		{
			txtEmail.PlaceholderColor = Color.Gray;
			//_slMainLayout.VerticalOptions = LayoutOptions.CenterAndExpand; 
		}

		void TxtEmail_Unfocused(object sender, FocusEventArgs e)
		{
			if (!string.IsNullOrEmpty(txtEmail.Text))
			{
				if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
				{

					txtEmail.TextColor = Color.Red;
				}
			}
			//_slMainLayout.VerticalOptions = LayoutOptions.EndAndExpand; 
		}

		void TxtPassword_Focused(object sender, FocusEventArgs e)
		{
			txtPassword.PlaceholderColor = Color.Gray;	
			//_slMainLayout.VerticalOptions = LayoutOptions.CenterAndExpand; 
		}

		void TxtPassword_Unfocused(object sender, FocusEventArgs e)
		{
			//_slMainLayout.VerticalOptions = LayoutOptions.EndAndExpand; 
		}

		async void ForgotPassword_Tapped(object sender, System.EventArgs e)
		{

			await Navigation.PushModalAsync(new SignUpPage());
		}

		async void SignUp_Tapped(object sender, System.EventArgs e)
		{
            await Navigation.PushAsync(new ForgotPasswordPage());
		}


		private void PrepareUI()
		{
			_slScrollLayout.HeightRequest = App.ScreenHeight / 2;
			if (StaticMethods.IsIpad())
			{
				txtEmail.HeightRequest = 60;
				txtPassword.HeightRequest = 60;
				btnLogin.HeightRequest = 60;
			}
		}

		async void Login_Clicked(object sender, System.EventArgs e)
		{
			
			if (IsValidate())
			{
				

					LoginProcess();

			}

		}
		private async Task LoginProcess()
		{
			try
			{
				StaticMethods.ShowLoader();
				Task.Factory.StartNew(
						// tasks allow you to use the lambda syntax to pass wor
						() =>
						{
							
							usermodel = WebService.Login(txtEmail.Text, txtPassword.Text);

						}).ContinueWith(
						t =>
						{
							if (usermodel.user_id > 0)
							{
						CrossSecureStorage.Current.SetValue("userId",usermodel.user_id.ToString());
						CrossSecureStorage.Current.SetValue("profilePic",usermodel.profile_pic.ToString());
						CrossSecureStorage.Current.SetValue("firstName",usermodel.first_name.ToString());
						CrossSecureStorage.Current.SetValue("lastName",usermodel.last_name.ToString());
						CrossSecureStorage.Current.SetValue("userEmail",usermodel.user_email.ToString());
						CrossSecureStorage.Current.SetValue("description",usermodel.description.ToString());
						CrossSecureStorage.Current.SetValue("fb",usermodel.facebook_url.ToString());
						CrossSecureStorage.Current.SetValue("tw",usermodel.twitter_url.ToString());
						CrossSecureStorage.Current.SetValue("insta",usermodel.instagram_url.ToString());
							
						StaticDataModel.UserId = usermodel.user_id;
						App.Current.MainPage = new NavigationPage( new MainPage());
							} 
							else
							{  
								StaticMethods.ShowToast("incorrect email or password"); 
					        }
							
					 StaticMethods.DismissLoader();
						}, TaskScheduler.FromCurrentSynchronizationContext()
					);
			}
			catch (Exception ex)
			{

			}

		}
		private bool IsValidate()
		{
			if (txtEmail.Text == string.Empty || txtEmail.Text == null)
			{
				txtEmail.PlaceholderColor = Color.Red;
				return false;

			}
			else if (txtPassword.Text == string.Empty || txtPassword.Text == null)
			{
				txtPassword.PlaceholderColor = Color.Red;

				return false;

			}
			else if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
			{
				txtEmail.TextColor = Color.Red;
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
