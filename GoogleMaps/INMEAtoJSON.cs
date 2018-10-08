using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GoogleMaps
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INMEAtoJSON" in both code and config file together.
    [ServiceContract]
    public interface INMEAtoJSON
    {
        [OperationContract]
        string Convert(string val);
    }
}
