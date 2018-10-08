using GoogleMaps.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GoogleMaps.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            Entities mapEntity = new Entities();
            string userId = User.Identity.GetUserId();
            IEnumerable<Marker> userMarkers = mapEntity.Markers.Where(x => x.UserId == userId);

            return View(userMarkers);
        }

        [HttpPost]
        public ActionResult Search(string title)
        {
            Entities mapEntity = new Entities();
            string userId = User.Identity.GetUserId();
            var result = mapEntity.Markers.Where(x => x.title.StartsWith(title) && x.UserId == userId).ToList();

            var viewModel = result
                .Select(x => new {
                    id = x.Id,
                    title = x.title,
                    description = x.description,
                    longitude = x.longitude,
                    latitude = x.latitude,
                    type = x.type
                }).ToList();

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMarkers ()
        {
            Entities mapEntity = new Entities();

            string userId = User.Identity.GetUserId();
            var result = mapEntity.Markers.Where(x => x.UserId == userId).ToList();

            var viewModel = result
                .Select(x => new {
                    id = x.Id,
                    title = x.title,
                    desc = x.description,
                    longitude = x.longitude,
                    latitude = x.latitude,
                    type = x.type
                }).ToList();

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveMarker(MarkerViewModel mvm)
        {
            Entities mapEntity = new Entities();

            string userId = User.Identity.GetUserId();

            Marker m;
            
            if (mvm.markerId != 0)
            {
                m = mapEntity.Markers.SingleOrDefault(x => x.Id == mvm.markerId && x.UserId == userId);

                if (m != null)
                {
                    m.title = mvm.title;
                    m.description = mvm.description;
                    m.latitude = mvm.latitude;
                    m.longitude = mvm.longitude;
                    m.type = mvm.type;
                } else
                {
                    return Json("Incorrect Marker", JsonRequestBehavior.AllowGet);
                }
            }
            else // Create new one
            {
                m = new Marker();
                m.UserId = userId;
                m.latitude = mvm.latitude;
                m.longitude = mvm.longitude;
                m.title = mvm.title;
                m.description = mvm.description;
                m.type = mvm.type;

                mapEntity.Markers.Add(m);
            }
    
            mapEntity.SaveChanges();

            return Json(m.Id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NMEAtoModel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NMEAtoModel(HttpPostedFileBase file)
        {
            List<GpsToJsonViewModel> viewModel = new List<GpsToJsonViewModel>();

            StreamReader streamFile = new StreamReader(file.InputStream);
            string line;
            
            while ( (line = streamFile.ReadLine()) != null) {
                if (line.StartsWith("$GPGGA"))
                {
                    string[] vals = line.Split(',');
                    string time = vals[1];
                    string[] latitude = { vals[2], vals[3] };
                    string[] longitude = { vals[4], vals[5] };

                    double latitudeDec = decimalToDMS(Double.Parse(latitude[0]));
                    double longitudeDec = decimalToDMS(Double.Parse(longitude[0]));

                    if (latitude[1] == "S")
                        latitudeDec *= -1;
                    if (longitude[1] == "W")
                        longitudeDec *= -1;

                    viewModel.Add(new GpsToJsonViewModel { latitude = latitudeDec, longitude = longitudeDec, time = time });
                }
            }
           return View(viewModel);
        }

        private double decimalToDMS(double value)
        {
            double degValue = value / 100;
            int degrees = (int)(value / 100);

            double decMinutesSeconds = ((degValue - degrees)) / .60;
            double result = (double)degrees + decMinutesSeconds;
            return result;
        }

        [HttpGet]
        public ActionResult GetElevation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetElevation(double lat1, double lng1, double lat2, double lng2)
        {
            WebRequest request = WebRequest.Create("http://dev.virtualearth.net/REST/v1/Elevation/SeaLevel?points=" + lat1 + "," + lng1 + "," + lat2 + "," + lng2 + "&key=Ahw4Dz8lc3Zqm6u7rYXNXIwHS8FlgtghEyuNEbncT2fagTJzIeveXFOwsBtnK5cQ");

            using (WebResponse response = request.GetResponse())
            {
                // Create a dataStream to consume the response sent
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        // Read the content as a string.
                        string responseFromServer = reader.ReadToEnd();
                        return Json(responseFromServer);
                    }
                }
            }

        }
    }
}