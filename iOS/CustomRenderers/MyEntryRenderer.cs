using System;
using AudioKetab;
using AudioKetab.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace AudioKetab.iOS
{
	public class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				// do whatever you want to the UITextField here!
				//Control.BackgroundColor = UIColor.Gray;
				Control.Layer.CornerRadius = 10;
				Control.Layer.BorderWidth = 0;
				Control.BorderStyle = UITextBorderStyle.None;
				//Control.TextAlignment = UITextAlignment.Right;
				Control.LeftView = new UIView(new CGRect(0, 0, 15, 0));
				Control.LeftViewMode = UITextFieldViewMode.Always;
				Control.RightView = new UIView(new CGRect(0, 0, 15, 0));
				Control.RightViewMode = UITextFieldViewMode.Always;
			}
		}
	}
}
