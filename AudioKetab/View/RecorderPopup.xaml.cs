using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace AudioKetab
{
	public partial class RecorderPopup : PopupPage
	{
		public static string  timerText = string.Empty;
		public static bool flag = false;
		static int miniute = 0;
		static int second = 0;
		public RecorderPopup()
		{
			InitializeComponent();
			btnRecord.Clicked+= BtnRecord_Clicked;
			btnStop.Clicked+= BtnStop_Clicked;
			btnStop.IsEnabled = false;
		}

		void BtnRecord_Clicked(object sender, EventArgs e)
		{
			btnStop.IsEnabled = true;
			btnRecord.IsEnabled = false;
			DependencyService.Get<IiOSMethods>().StartRecording();
			flag = true;
			StartTimer();
		
		}

		async void BtnStop_Clicked(object sender, EventArgs e)
		{
			btnRecord.IsEnabled = true;
			var data=	DependencyService.Get<IiOSMethods>().StopRecording();
			AudioRecordingPage._uploadAudioModel.byte_recorded_audio = data;
			StopTimer();
			await Navigation.PopPopupAsync();
		}
		private void StartTimer()
		{
			
			Device.StartTimer(TimeSpan.FromSeconds(1), () =>
			{
				//var text=	TimeSpan.FromSeconds(1).ToString("c");
				if (second < 60)
				{
					second = second + 1;
					if(second>9)
					lblTimer.Text = miniute.ToString()+":"+second.ToString();
					else
						lblTimer.Text = miniute.ToString() + ":0" + second.ToString();
				}
				else
				{
					second = 0;
					miniute = miniute + 1;
					if(miniute>9)
					lblTimer.Text = miniute.ToString()+ ":" + second.ToString();
					else
					lblTimer.Text = "0"+miniute.ToString() + ":" + second.ToString();	
				}
				if (flag)
				{
					return flag;
				}
				else
				{
					return flag;
					lblTimer.Text = "00:00";//not continue
				}

			});
		}
		private void StopTimer()
		{
			flag = false;
			lblTimer.Text = "00:00";
			btnStop.IsEnabled = false;
		}
		private void ProcessTimer()
		{ 
		try
			{

			}
			catch (Exception ex)
			{

			}
		}
	}
}
