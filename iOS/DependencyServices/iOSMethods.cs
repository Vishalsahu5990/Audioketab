using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AssetsLibrary;
using AudioKetab.iOS;
using AudioToolbox;
using AVFoundation;
using BigTed;
using Firebase.Invites;
using Foundation;
using Google.SignIn;
using MediaPlayer;
using PerpetualEngine.Storage;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSMethods))]
namespace AudioKetab.iOS
{
	public class iOSMethods : UIViewController, IiOSMethods, IInviteDelegate,ISignInUIDelegate
	{
		private string Key = "AudioKetab_LocalData";
		// declarations
		AVAudioRecorder recorder;
		AVPlayer player;
		NSDictionary settings;
		Stopwatch stopwatch = null;
		NSUrl audioFilePath = null;
		NSObject observer;

[Export("signInWillDispatch:error:")]
public void WillDispatch(SignIn signIn, NSError error)
{
//myActivityIndicator.StopAnimating();
}

[Export("signIn:presentViewController:")]
public void PresentViewController(SignIn signIn, UIViewController viewController)
{
PresentViewController(viewController, true, null);
}

[Export("signIn:dismissViewController:")]
public void DismissViewController(SignIn signIn, UIViewController viewController)
{
	DismissViewController(true, null);
}
public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
{
			if (user != null && error == null)
			{ 
			
			}
		// Disable the SignInButton
}
		public void ShowToast(string msg)
		{
			//BTProgressHUD.ShowToast(msg, false, 1000);
		}
		public void ShowLoader()
		{
			
			MPMediaPickerController mediaController = new MPMediaPickerController(MPMediaType.AnyAudio);
			mediaController.AllowsPickingMultipleItems = false;
			mediaController.Delegate = new MediaPickerDelegate(UIApplication.SharedApplication.KeyWindow.RootViewController);
			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentModalViewController(mediaController, true);


		}
		public void SendAppInvitation()
		{
			try
			{
				
				//// When you create the invitation dialog, you must specify the title
				//// of the invitation dialog and the invitation message to send. 
				//// You can also customize the image and deep link URL that 
				//// get sent in the invitation
				//var inviteDialog = Invites.GetInviteDialog();
				//inviteDialog.SetInviteDelegate (this);
				//inviteDialog.SetTitle("Audioketab");
				//var anUrl = "";
				//// A message hint for the dialog. Note this manifests differently depending on the
				//// received invitation type. For example, in an email invite this appears as the subject.
				//inviteDialog.SetMessage($"Try this out! {anUrl}");

				//// These following values are optionals and are only sent via email
				////inviteDialog.SetDeepLink("http://appstore.com/audioketab");
				//inviteDialog.SetDescription("A description of the app");
				////inviteDialog.SetCustomImage("The url of the image");
				//inviteDialog.SetCallToActionText("Invite friend");

				//// If you have an Android version of your app and you want to send
				//// an invitation that can be opened on Android in addition to iOS
				////var targetApp = new InvitesTargetApplication
				////{
				////	AndroidClientId = "Android ID"
				////};
				////inviteDialog.SetOtherPlatformsTargetApplication(targetApp);

				//inviteDialog.Open();
				SignIn.SharedInstance.UIDelegate = this;
				//SignIn.SharedInstance.Delegate =this ;

// Sign the user in automatically
SignIn.SharedInstance.SignInUserSilently ();
			}
			catch (Exception ex)
			{

			}
		}
		[Export("inviteFinishedWithInvitations:error:")]
		public void InviteFinished(string[] invitationIds, NSError error)
		{
			if (error == null)
			{
				foreach (var id in invitationIds)
					System.Console.WriteLine(id);
			}
			else
			{
				System.Console.WriteLine(error.LocalizedDescription);
			}

		}

		public void StartRecording()
		{
			try
			{
				AudioSession.Initialize();
				Console.WriteLine("Begin Recording");

				AudioSession.Category = AudioSessionCategory.RecordAudio;
				AudioSession.SetActive(true);

				if (!PrepareAudioRecording())
				{
					//RecordingStatusLabel.Text = "Error preparing";
					return;
				}

				if (!recorder.Record())
				{
					//RecordingStatusLabel.Text = "Error recording";
					return;
				}

				this.stopwatch = new Stopwatch();
				this.stopwatch.Start();
				RecorderPopup.timerText = stopwatch.ElapsedMilliseconds.ToString();
			}
			catch (Exception ex)
			{

			}

		}
		public byte[] StopRecording()
		{
			this.recorder.Stop();
			//this.LengthOfRecordingLabel.Text = string.Format("{0:hh\\:mm\\:ss}", this.stopwatch.Elapsed);
			this.stopwatch.Stop();
			if (!string.IsNullOrEmpty(audioFilePath.ToString()))
			{
				NSData data = NSData.FromUrl(audioFilePath);
				var array = ToByte(data);
				//var _basestring = Convert.ToBase64String(array);
				return array;
			}
			else
			{
				return null;
			}

		}
		public byte[] ToByte(NSData data)
		{
			MemoryStream ms = new MemoryStream();
			data.AsStream().CopyTo(ms);
			return ms.ToArray();
		}
		public void SaveLocalData(UserModel um)
		{
			try
			{


				//var storage = SimpleStorage.EditGroup(Key);
				//storage.Put("driver_id", um.driver_id.ToString());
				//storage.Put("firstname", um.firstname);
				//storage.Put("lastname", um.lastname);
				//storage.Put("email", um.email);
				//storage.Put("DOB", um.DOB);
				//storage.Put("city", um.city);
				//storage.Put("phonenumber", um.phonenumber.ToString());
				//storage.Put("address", um.address);
				//storage.Put("device", um.device);
				//storage.Put("vehicle", um.vehicle);
				//storage.Put("image", um.image);
			}
			catch (Exception ex)
			{

			}
		}
		public UserModel RetriveLocalData()
		{
			UserModel um = new UserModel();
			try
			{
				var storage = SimpleStorage.EditGroup(Key);
				string id = storage.Get("driver_id");
				//um.driver_id = Convert.ToInt32(id);
				//um.firstname = storage.Get("firstname");
				//um.lastname = storage.Get("lastname");
				//um.email = storage.Get("email");
				//um.DOB = storage.Get("DOB");
				//um.city = storage.Get("city");
				//um.phonenumber = Convert.ToInt32(storage.Get("phonenumber"));
				//um.address = storage.Get("address");
				//um.device = storage.Get("device");
				//um.vehicle = storage.Get("vehicle");
				//um.image = storage.Get("image");
				return um;
			}
			catch (Exception ex)
			{
				return um;
			}
		}
		public void DeleteLocalData()
		{
			string values = string.Empty;
			try
			{

				var storage = SimpleStorage.EditGroup(Key);
				storage.Delete(Key);

			}
			catch (Exception ex)
			{

			}
		}
		public async Task<Stream> GetStreamFromUrl(string url)
		{
			byte[] imageData = null;

			using (var wc = new System.Net.WebClient())
				return new MemoryStream(await wc.DownloadDataTaskAsync(url));
		}
		public class MediaPickerDelegate : MPMediaPickerControllerDelegate
		{
			MPMusicPlayerController _musicPlayer;
			UIViewController _viewController;

			public MediaPickerDelegate(UIViewController viewController) : base()
			{

				_viewController = viewController;
			}

			public override void MediaItemsPicked(MPMediaPickerController sender, MPMediaItemCollection mediaItemCollection)
			{
				var items = mediaItemCollection.Items;

				var assetUrl = items[0].AssetURL.AbsoluteUrl;
				NSData data = NSData.FromUrl(assetUrl);


				NSUrl soundFileURL = NSUrl.CreateFileUrl(@"%@/memo.mp3", assetUrl);
				//[NSURL fileURLWithPath:[NSString stringWithFormat: @"%@/memo.m4a", documentsDirectory]]; ;
				NSData myData = NSData.FromUrl(soundFileURL);
				GetAssetFromUrl(assetUrl);
				_viewController.DismissModalViewController(true);
			}

			public override void MediaPickerDidCancel(MPMediaPickerController sender)
			{
				_viewController.DismissModalViewController(true);
			}
			void GetAssetFromUrl(Foundation.NSUrl url)
			{

				try
				{
					var inputPath = url;
					var outputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/output.mp3";
					var outputURL = new NSUrl(outputPath);
					NSData data = NSData.FromUrl(outputURL);

					//compress the video file
					var asset = new AVUrlAsset(inputPath, (AVUrlAssetOptions)null);
					var exportSession = AVAssetExportSession.FromAsset(asset, "AVAssetExportPresetLowQuality");

					exportSession.OutputUrl = outputURL;
					exportSession.OutputFileType = AVFileType.CoreAudioFormat;

					exportSession.ExportAsynchronously(() =>
					{
						Console.WriteLine(exportSession.Status);//prints status "Failed"....

						exportSession.Dispose();
					});
					//var asset = new ALAssetsLibrary();
					//UIImage image;
					//asset.AssetForUrl(
					//				url,
					//				(ALAsset obj) =>
					//				{
					//					var assetRep = obj.DefaultRepresentation;
					//					var filename = assetRep.Filename;


					//				},
					//				(NSError err) =>
					//				{
					//					Console.WriteLine(err);
					//				}
					//			);
				}
				catch (Exception ex)
				{

				}
			}



		}
		public bool PrepareAudioRecording()
		{
			// You must initialize an audio session before trying to record
			var audioSession = AVAudioSession.SharedInstance();
			var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
			if (err != null)
			{
				Console.WriteLine("audioSession: {0}", err);
				return false;
			}
			err = audioSession.SetActive(true);
			if (err != null)
			{
				Console.WriteLine("audioSession: {0}", err);
				return false;
			}

			// Declare string for application temp path and tack on the file extension
			string fileName = string.Format("recording{0}.aac", DateTime.Now.ToString("yyyyMMddHHmmss"));
			string tempRecording = Path.Combine(Path.GetTempPath(), fileName);

			Console.WriteLine(tempRecording);
			audioFilePath = NSUrl.FromFilename(tempRecording);

			//set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
			NSObject[] values = new NSObject[]
			{
				NSNumber.FromFloat(44100.0f),
				NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.MPEG4AAC),
				NSNumber.FromInt32(1),
				NSNumber.FromInt32((int)AVAudioQuality.High)
			};
			//Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
			NSObject[] keys = new NSObject[]
			{
				AVAudioSettings.AVSampleRateKey,
				AVAudioSettings.AVFormatIDKey,
				AVAudioSettings.AVNumberOfChannelsKey,
				AVAudioSettings.AVEncoderAudioQualityKey
			};
			//Set Settings with the Values and Keys to create the NSDictionary
			settings = NSDictionary.FromObjectsAndKeys(values, keys);

			//Set recorder parameters
			NSError error;
			recorder = AVAudioRecorder.Create(this.audioFilePath, new AudioSettings(settings), out error);
			if ((recorder == null) || (error != null))
			{
				Console.WriteLine(error);
				return false;
			}

			//Set Recorder to Prepare To Record
			if (!recorder.PrepareToRecord())
			{
				recorder.Dispose();
				recorder = null;
				return false;
			}

			recorder.FinishedRecording += delegate (object sender, AVStatusEventArgs e)
			{
				recorder.Dispose();
				recorder = null;
				Console.WriteLine("Done Recording (status: {0})", e.Status);
			};

			return true;
		}
	}

}
