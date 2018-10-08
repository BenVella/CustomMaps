using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleMaps.Controllers
{
    public class GPSController : Controller
    {
        // GET: GPS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Convert (string val)
        {
            var data = new NMEAtoJSON().Convert(val);
            return Json(data);
        }
    }
}