using System;
using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class AlbumInfoPopup : PopupPage
	{
		public AlbumInfoPopup()
		{
			InitializeComponent();
		}
public AlbumInfoPopup(string albumTitle,string authorName,string Description)
{
	InitializeComponent();
			lblTitle.Text = albumTitle;
			lblAuthorName.Text = authorName;
			lblDescription.Text = Description; 
		}
async void Cross_Tapped(object sender, EventArgs e)
{
	Navigation.PopPopupAsync();
		}
	}
}
