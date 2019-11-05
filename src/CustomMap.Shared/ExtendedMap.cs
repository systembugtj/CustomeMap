using System;
using Xamarin.Forms.Maps;

namespace Journey.Extended.Map
{
    /// <summary>
    /// Event args used with maps, when the user tap on it
    /// </summary>
    public class MapTapEventArgs : EventArgs
    {
        public Position Position { get; set; }
    }

    /// <summary>
    /// Event args used with maps, when map is loaded
    /// </summary>
    public class MapRegionChangedEventArgs : EventArgs
    {
        public MapSpan Region { get; set; }
    }

    public class ExtendedMap : Xamarin.Forms.Maps.Map
    {
		public CirclePin Circle { get; set; }

        /// <summary>
        /// Event thrown when the user taps on the map
        /// </summary>
        public event EventHandler<MapTapEventArgs> Tapped;

        /// <summary>
        /// Event thrown when map visible region is changed.
        /// </summary>
        public event EventHandler<MapRegionChangedEventArgs> RegionChanged;

        public void OnTap(Position coordinate)
        {
            OnTap(new MapTapEventArgs { Position = coordinate });
        }

        public void OnRegionChanged()
        {
            OnRegionChanged(new MapRegionChangedEventArgs { Region = this.VisibleRegion });
        }

        protected virtual void OnTap(MapTapEventArgs e)
        {
            Tapped?.Invoke(this, e);
        }

        protected virtual void OnRegionChanged(MapRegionChangedEventArgs e)
        {
            RegionChanged?.Invoke(this, e);
        }
    }
}
