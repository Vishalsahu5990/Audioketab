using System;
using AudioKetab.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AudioKetab;

[assembly: ExportRenderer(typeof(TLScrollView), typeof(TLScrollViewRenderer))]
namespace AudioKetab.iOS
{
	public class TLScrollViewRenderer : ScrollViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			var element = e.NewElement as TLScrollView;
			element?.Render();
		}
	}
}
