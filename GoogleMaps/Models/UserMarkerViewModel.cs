using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleMaps.Models
{
    public class UserMarkerViewModel
    {
    }

    public class LatLngViewModel
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class InfoWindowViewModel
    {
        public string markerId { get; set; }
        public string title { get; set; }
    }

    public class MarkerViewModel
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int markerId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }

    public class GpsToJsonViewModel
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string time { get; set; }
    }
}