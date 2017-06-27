using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class SignUpPage : ContentPage
	{
		int Count = 0;
		public SignUpPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			datepicker.DateSelected += Datepicker_DateSelected;
			countrypicker.SelectedIndexChanged+= Countrypicker_SelectedIndexChanged;
			txtFirstName.Focused+= TxtFirstName_Focused;
			txtLastName.Focused+= TxtLastName_Focused;
			txtDob.Focused+= TxtDob_Focused;
			txtEmail.Focused+= TxtEmail_Focused;
			txtPassword.Focused+= TxtPassword_Focused;
			txtMobileNo.Focused+= TxtMobileNo_Focused;
			txtMobileNo.TextChanged+= TxtMobileNo_TextChanged;
			txtPassword.TextChanged+= TxtPassword_TextChanged;
			PrepareUI();
			GetCountries();
		}
async void Back_Tapped(object sender, System.EventArgs e)
{
	try
	{
				Navigation.PopModalAsync();

	}
	catch (Exception ex)
	{


	}
		}
		private void PrepareUI()
		{
			try
			{
				if (StaticMethods.IsIpad())
			{
					txtFirstName.HeightRequest = 60;
					txtLastName.HeightRequest = 60;
					txtDob.HeightRequest = 60;
					txtEmail.HeightRequest = 60;
					txtPassword.HeightRequest = 60;
					txtCountry.HeightRequest = 60;
					txtMobileNo.HeightRequest = 60;
					_slCountry.HeightRequest = 60;
					_rlCountry.HeightRequest = 60;
				    btnSignUp.HeightRequest = 60;
			}

			}
			catch (Exception ex)
			{

			}
		}

		void TxtMobileNo_TextChanged(object sender, TextChangedEventArgs e)
		{
			int _limit = 10;    //Enter text limit

			string _text = txtMobileNo.Text;      //Get Current Text
			if (_text.Length > _limit)       //If it is more than your character restriction
			{
				_text = _text.Remove(_text.Length - 1);  // Remove Last character
				txtMobileNo.Text = _text;        //Set the Old value
			}
		}

		void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
		{
			int _limit = 5;
			string _text = txtPassword.Text;
			if (_text.Length < _limit)       //If it is more than your character restriction
			{
				txtPassword.TextColor = Color.Red;
			}
			else
			{ 
				txtPassword.TextColor = Color.Gray;
			}
		}

		void TxtFirstName_Focused(object sender, FocusEventArgs e)
		{
			txtFirstName.PlaceholderColor = Color.Gray;
		}

		void TxtLastName_Focused(object sender, FocusEventArgs e)
		{
			txtLastName.PlaceholderColor = Color.Gray;
		}

		void TxtDob_Focused(object sender, FocusEventArgs e)
		{
			txtDob.PlaceholderColor = Color.Gray;
		}

		void TxtEmail_Focused(object sender, FocusEventArgs e)
		{
			txtEmail.PlaceholderColor = Color.Gray;
			txtEmail.TextColor = Color.Gray;
		}

		void TxtPassword_Focused(object sender, FocusEventArgs e)
		{
			lblErrorMessage.IsVisible = false;
			txtPassword.PlaceholderColor = Color.Gray;
		}

		void TxtMobileNo_Focused(object sender, FocusEventArgs e)
		{
			txtMobileNo.PlaceholderColor = Color.Gray;
		}

		async void DOB_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				datepicker.Focus();
			});
		}
		async void Country_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				countrypicker.Focus();
			});
		}

		void Countrypicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = countrypicker.Items[countrypicker.SelectedIndex];
			txtCountry.Text = item;
		}

		void Datepicker_DateSelected(object sender, DateChangedEventArgs e)
		{
			txtDob.Text = e.NewDate.ToString("yyyy-MM-dd");
		}
		async void CheckBox_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				lblErrorMessage.IsVisible = false;
				if (Count % 2 == 0)
				{

					imgCheckBox.Source = "chckbox_check.png";
				}
				else
				{
					imgCheckBox.Source = "checkbox_uncheck.png";

				}
				Count++;
			});
		}
async void PrivacyPolicy_Tapped(object sender, System.EventArgs e)
{
	Device.BeginInvokeOnMainThread(async () =>
	{
				Device.OpenUri(new Uri("http://www.audioketab.com/home/privacy_policy"));
	});
		}

		async void Signup_Clicked(object sender, System.EventArgs e)
		{
			if (IsValidate())
			{
				if (Count % 2 != 0)
				{
					SignUpProcess();

				}
				else
				{
					lblErrorMessage.IsVisible = true;
					lblErrorMessage.Text = "Please accept terms and conditions.";
				}
			}
		}
		private async Task SignUpProcess()
		{
			UserModel um = new UserModel();
			string ret = string.Empty;
			StaticMethods.ShowLoader();
			Task.Factory.StartNew(
					// tasks allow you to use the lambda syntax to pass wor
					() =>
					{
				um.first_name = txtFirstName.Text;
				um.last_name = txtLastName.Text;
				um.dob = txtDob.Text;
				um.user_email = txtEmail.Text;
				um.user_pass = txtPassword.Text;
				um.country = txtCountry.Text;
				um.mobile = txtMobileNo.Text;
				ret=	WebService.SignUp(um);
					}).ContinueWith(
					t =>
					{
						if (ret == "success")
						{
					        Navigation.PopAsync(false);
							 DisplayAlert("alert", "Verification link sent on you registered email address please check your inbox and click on the link", "Ok");
				        }
		
							  

					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
		}
			private async Task GetCountries()
			{
				List<string> list=null;
				StaticMethods.ShowLoader();
				Task.Factory.StartNew(
						// tasks allow you to use the lambda syntax to pass wor
						() =>
						{
					list=	WebService.GetCountry();
						}).ContinueWith(
						t =>
						{
							if (list != null)
							{
								foreach (var item in list)
								{
									countrypicker.Items.Add(item);
						      }
					       }
			
									

						}, TaskScheduler.FromCurrentSynchronizationContext()
					);
			}
		private bool IsValidate()
		{
			if (string.IsNullOrEmpty(txtFirstName.Text))
			{

			  txtFirstName.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtLastName.Text))
			{

				txtLastName.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtDob.Text))
			{

				txtDob.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtEmail.Text))
			{

				txtEmail.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtPassword.Text))
			{

			txtPassword.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtCountry.Text))
			{

				txtCountry.PlaceholderColor = Color.Red;
				return false;

			}
			else if (string.IsNullOrEmpty(txtMobileNo.Text))
			{

				txtMobileNo.PlaceholderColor = Color.Red;
				return false;

			}
			else if (txtPassword.Text.Length<5)
			{

				lblErrorMessage.Text = "Your password must be at least 5 characters long";
				lblErrorMessage.IsVisible = true;
				return false;

			}
			if (!Regex.Match(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
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
