using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class ForgotPasswordPage : ContentPage
	{
		public ForgotPasswordPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			frame.HeightRequest = App.ScreenHeight/1.5;
			txtEmail.Focused+= TxtEmail_Focused;
			txtEmail.TextChanged+= TxtEmail_TextChanged;
			if (StaticMethods.IsIpad())
			{ 
				txtEmail.HeightRequest = 60;
				btnSubmit.HeightRequest = 60;
			}
		}

		void TxtEmail_Focused(object sender, FocusEventArgs e)
		{
			txtEmail.PlaceholderColor = Color.Gray;
			lblErrorMessage.IsVisible = false;
		}

		void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!string.IsNullOrEmpty(txtEmail.Text))
			{
				if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
				{

					txtEmail.TextColor = Color.Red;

				}
				else
				{ 
					txtEmail.TextColor = Color.Gray;
				}
			}
		}

		async void Cancel_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PopModalAsync();
		}
     async void Submit_Clicked(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(txtEmail.Text))
			{
				lblErrorMessage.IsVisible = true;
				lblErrorMessage.Text = "Email required!";
				txtEmail.PlaceholderColor = Color.Red;
			}

			else
			{
				if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
				{
					lblErrorMessage.IsVisible = true;
					lblErrorMessage.Text = "Invalid Email!";
					txtEmail.TextColor = Color.Red;

				}
				else
				{
					SubmitProcess();
				}
			}

		}
private async Task SubmitProcess()
{
			string ret = string.Empty;
	StaticMethods.ShowLoader();
	Task.Factory.StartNew(
			// tasks allow you to use the lambda syntax to pass wor
			() =>
			{
				ret = WebService.ForgetPassword(txtEmail.Text);
			}).ContinueWith(
			t =>
			{
				if (ret == "success")
				{
					 Navigation.PushAsync(new InfoPage());
				}
				else
				{ 
				 lblErrorMessage.Text = "Invalid Email!";
					lblErrorMessage.IsVisible = true;
					txtEmail.TextColor = Color.Red;
				}


			}, TaskScheduler.FromCurrentSynchronizationContext()
		);
		}
	}
}
