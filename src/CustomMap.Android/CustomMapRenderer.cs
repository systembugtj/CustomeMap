using System;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Runtime;
using Journey.Extended.Map.Droid.Renderers;
using Journey.Extended.Map;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(Journey.Extended.Map.ExtendedMap), typeof(ExtendedMapRenderer))]
namespace Journey.Extended.Map.Droid.Renderers
{
    /// <summary>
    /// Renderer for the xamarin map.
    /// Enable user to get a position by taping on the map.
    /// </summary>
    [Preserve]
    public class ExtendedMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        public ExtendedMapRenderer(Context context) : base(context)
        {

        }
        // We use a native google map for Android
        private GoogleMap _map;

        protected override void OnMapReady(GoogleMap map)
        {
            _map = map;

            if (_map != null)
            {
                _map.MapClick += GoogleMap_MapClick;
                _map.CameraChange += GoogleMap_CameraChange;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            if (_map != null)
                _map.MapClick -= GoogleMap_MapClick;

            base.OnElementChanged(e);

            if (Control != null)
                ((MapView)Control).GetMapAsync(this);
        }

        private void GoogleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtendedMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }

        void UpdateVisibleRegion(LatLng pos)
        {
            GoogleMap map = NativeMap;
            if (map == null)
            {
                return;
            }
            Projection projection = map.Projection;
            int width = Control.Width;
            int height = Control.Height;
            LatLng ul = projection.FromScreenLocation(new global::Android.Graphics.Point(0, 0));
            LatLng ur = projection.FromScreenLocation(new global::Android.Graphics.Point(width, 0));
            LatLng ll = projection.FromScreenLocation(new global::Android.Graphics.Point(0, height));
            LatLng lr = projection.FromScreenLocation(new global::Android.Graphics.Point(width, height));
            double dlat = Math.Max(Math.Abs(ul.Latitude - lr.Latitude), Math.Abs(ur.Latitude - ll.Latitude));
            double dlong = Math.Max(Math.Abs(ul.Longitude - lr.Longitude), Math.Abs(ur.Longitude - ll.Longitude));
            Element.SetVisibleRegion(new MapSpan(new Position(pos.Latitude, pos.Longitude), dlat, dlong));
        }

        public void GoogleMap_CameraChange(object sender, CameraChangeEventArgs e)
        {
            UpdateVisibleRegion(e.Position.Target);
            ((ExtendedMap)Element).OnRegionChanged();
        }
    }
}
