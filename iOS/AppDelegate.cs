using System;
using System.Collections.Generic;
using System.Linq;
using CarouselView.FormsPlugin.iOS;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.MediaManager;
using UIKit;
using Websockets.Ios;
using Firebase;
using Firebase.Invites;
using Firebase.Analytics;
using Google.SignIn;

namespace AudioKetab.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			PrepareRemoteNotification();
			SlideOverKit.iOS.SlideOverKit.Init ();
			CarouselViewRenderer.Init ();
			ImageCircleRenderer.Init();
			FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
			WebsocketConnection.Link();
			//Getting device screen size
			App.ScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
			App.ScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
			SetupTitleBar();




// You can get the GoogleService-Info.plist file at https://developers.google.com/mobile/add
var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString ();

			Firebase.Analytics.App.Configure();
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
var openUrlOptions = new UIApplicationOpenUrlOptions(options);
	return OpenUrl(app, url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);

		}
		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
// Handle App Invite requests
var invite = Invites.HandleUrl(url, sourceApplication, annotation);

	if (invite != null) {
		var matchType = invite.MatchType == ReceivedInviteMatchType.Weak ? "Weak" : "Strong";
var message = $"Deep link from {sourceApplication}\nInvite ID: {invite.InviteId}\nApp Url: {invite.DeepLink}\nMatch Type: {matchType}";
System.Console.WriteLine ($"Depp-Link Data: {message}");

		return true;
	}

	// Handle Sign In
	return SignIn.SharedInstance.HandleUrl (url, sourceApplication, annotation);
		}
		private void SetupTitleBar()
		{


			UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(0.301f, 0.301f, 0.301f); //bar background
			UINavigationBar.Appearance.TintColor = UIColor.FromRGB(1f, 0.377f, 0.376f);//Tint color of button items
			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
			{
				TextColor = UIColor.FromRGB(0.76f, 0.76f, 0.76f)

			});
		}
		private void PrepareRemoteNotification()
		{
			try
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				{
					var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
						UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
						new NSSet());

					UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
					UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
					UIApplication.SharedApplication.RegisterForRemoteNotifications();
				}
				else
				{
					UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
					UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);

				}
			}
			catch (Exception ex)
			{

			}
		}
		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			// Get current device token
			var DeviceToken = deviceToken.Description;
			if (!string.IsNullOrWhiteSpace(DeviceToken))
			{
				DeviceToken = DeviceToken.Trim('<').Trim('>');
				DeviceToken = DeviceToken.Replace(" ", "");
				StaticDataModel.DeviceToken = DeviceToken.ToString();
				Console.WriteLine(DeviceToken);
			}

			// Get previous device token
			var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

			// Has the token changed?
			if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
			{
				//TODO: Put your own logic here to notify your server that the device token has changed/been created!
			}

			// Save new device token 
			NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			//new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary options, Action<UIBackgroundFetchResult> completionHandler)
		{
			

		}

	}
}
