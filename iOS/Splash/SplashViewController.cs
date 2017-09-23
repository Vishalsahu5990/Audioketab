using System;
using CoreGraphics;
using UIKit;

namespace AudioKetab.iOS
{
	public partial class SplashViewController : UIViewController
	{
		public SplashViewController() : base("SplashViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			AnimateImageView();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
		private void AnimateImageView()
		{ 
		var	pt = logo.Center;
		try
			{
				UIView.Animate (3.0f, 1.0f, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.Autoreverse,
                () => {
					logo.Center =
						         new CGPoint(UIScreen.MainScreen.Bounds.Right - logo.Frame.Width / 2, logo.Center.Y);},
                () => {
					logo.Center = pt;

						
				}
            );
			}
			catch (Exception ex)
			{

			}
		}
	}
}

