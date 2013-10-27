using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;

namespace ImageGesturesWP7
{
	public class AutoClipBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.SizeChanged += new SizeChangedEventHandler(AssociatedObject_SizeChanged);

			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.SizeChanged -= new SizeChangedEventHandler(AssociatedObject_SizeChanged);

			base.OnDetaching();
		}

		private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			FrameworkElement element = AssociatedObject;

			if (element != null)
			{
				var rectangleGeometry = new RectangleGeometry();
				rectangleGeometry.Rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight);
				element.Clip = rectangleGeometry;
			}
		}
	}
}
