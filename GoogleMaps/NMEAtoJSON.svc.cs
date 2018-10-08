using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace GoogleMaps
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NMEAtoJSON" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select NMEAtoJSON.svc or NMEAtoJSON.svc.cs at the Solution Explorer and start debugging.
    public class NMEAtoJSON : INMEAtoJSON
    {
        [WebMethod]
        public string Convert(string val)
        {
            return new JavaScriptSerializer().Serialize(val);
        }
    }
}
