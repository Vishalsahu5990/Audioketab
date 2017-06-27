using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class UpdatePlaylistPopup : PopupPage
	{
		public static event EventHandler<string> SaveChanged;

		public UpdatePlaylistPopup()
		{
			InitializeComponent();

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

			btnSend.Clicked+= BtnSend_Clicked;
			btnCancel.Clicked+= BtnCancel_Clicked;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			btnSend.Clicked-= BtnSend_Clicked;
			btnCancel.Clicked-= BtnCancel_Clicked;
		}
		public UpdatePlaylistPopup(string category_name)
		{
			InitializeComponent();
			txtplaylistname.Text = category_name;
		}

		void BtnSend_Clicked(object sender, EventArgs e)
		{
			
            SaveChanged(this,txtplaylistname.Text);
			Navigation.PopPopupAsync();
		}

		void BtnCancel_Clicked(object sender, EventArgs e)
		{
			
			Navigation.PopPopupAsync();
		}
	}
}
