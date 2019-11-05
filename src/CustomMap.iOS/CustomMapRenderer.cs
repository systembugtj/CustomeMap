using System;
using Journey.Extended.Map.iOS.Renderers;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Journey.Extended.Map.ExtendedMap), typeof(CustomMapRenderer))]
namespace Journey.Extended.Map.iOS.Renderers
{

    public class CustomMapRenderer: MapRenderer
    {

        private readonly UITapGestureRecognizer _tapRecogniser;

        public CustomMapRenderer()
        {
            _tapRecogniser = new UITapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var cgPoint = recognizer.LocationInView(Control);

            var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

            ((ExtendedMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
        }

        private void CenterChanged(object sender, EventArgs e)
        {
            if (Element != null)
            {
                ((ExtendedMap)Element).OnRegionChanged();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            if (Control != null)
            {
                Control.RemoveGestureRecognizer(_tapRecogniser);
                ((MKMapView)Control).RegionChanged -= CenterChanged;
            }

            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.AddGestureRecognizer(_tapRecogniser);
                ((MKMapView)Control).RegionChanged += CenterChanged;
            }
        }
    }

}
