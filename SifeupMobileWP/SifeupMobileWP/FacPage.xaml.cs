using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Interactivity;

namespace SifeupMobileWP
{
    public partial class FacPage : PhoneApplicationPage
    {
        //double initialAngle;
        double initialScale;

        public FacPage()
        {
            InitializeComponent();
        }


        private void OnTap(object sender, GestureEventArgs e)
        {
            transform.TranslateX = transform.TranslateY = 0;
        }

        private void OnDoubleTap(object sender, GestureEventArgs e)
        {
            transform.ScaleX = transform.ScaleY = 1;
        }

        private void OnHold(object sender, GestureEventArgs e)
        {
            transform.TranslateX = transform.TranslateY = 0;
            transform.ScaleX = transform.ScaleY = 1;
            transform.Rotation = 0;
        }

        private void OnDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            image.Opacity = 0.6;
        }

        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            transform.TranslateX += e.HorizontalChange;
            transform.TranslateY += e.VerticalChange;
            //if (transform.TranslateY < -40)
            //    transform.TranslateY = -40;
        }

        private void OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            image.Opacity = 1.0;
        }

        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            Point point0 = e.GetPosition(image, 0);
            Point point1 = e.GetPosition(image, 1);
            Point midpoint = new Point((point0.X + point1.X) / 2, (point0.Y + point1.Y) / 2);
            image.RenderTransformOrigin = new Point(midpoint.X / image.ActualWidth, midpoint.Y / image.ActualHeight);
            //initialAngle = transform.Rotation;
            initialScale = transform.ScaleX;
            image.Opacity = 0.8;
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            //transform.Rotation = initialAngle + e.TotalAngleDelta;
            transform.ScaleX = transform.ScaleY = initialScale * e.DistanceRatio;
        }

        private void OnPinchCompleted(object sender, PinchGestureEventArgs e)
        {
            image.Opacity = 1.0;
        }
    }
}