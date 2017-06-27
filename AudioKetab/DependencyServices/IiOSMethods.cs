using System;
using System.IO;
using System.Threading.Tasks;

namespace AudioKetab
{
	public interface IiOSMethods
	{
		void ShowToast(string ToastMsg);
		void ShowLoader();
		void StartRecording();
		byte [] StopRecording();
		UserModel RetriveLocalData();
		void SaveLocalData(UserModel um);
		void DeleteLocalData();
        Task<Stream> GetStreamFromUrl(string url);
     void SendAppInvitation();
 	}
}
