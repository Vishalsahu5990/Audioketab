using System;
using System;
using AudioKetab;
using AudioKetab.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ChatEntry), typeof(ChatEntryRenderer))]
namespace AudioKetab.iOS
{
public class ChatEntryRenderer : EntryRenderer
{
	protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == VisualElement.IsFocusedProperty.PropertyName)
		{
			if (Control != null)
			{
				// do whatever you want to the UITextField here!
				Control.Layer.CornerRadius = 8;
				Control.Layer.BorderWidth = 0;
				Control.ReturnKeyType = UIReturnKeyType.Send;
				var chatEntryView = this.Element as ChatEntry;
				Control.ShouldEndEditing = (UITextField textField) =>
				{
					return !ChatEntry.keepOpen; // KeepOpen is a custom property I added and is set on my ViewModel if I want the Entry Keyboard to be kept open then I just set this to true.
					};

			}
		}

		base.OnElementPropertyChanged(sender, e);
	}

}
}
