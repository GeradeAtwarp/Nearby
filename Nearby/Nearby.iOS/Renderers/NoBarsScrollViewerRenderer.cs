using Nearby.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Nearby.Controls;

[assembly: ExportRenderer(typeof(NoBarsScrollViewer), typeof(NoBarsScrollViewerRenderer))]
namespace Nearby.iOS
{
	public class NoBarsScrollViewerRenderer : ScrollViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
			{
				return;
			}

			ShowsHorizontalScrollIndicator = false;
			ShowsVerticalScrollIndicator = false;
		}
	}
}
