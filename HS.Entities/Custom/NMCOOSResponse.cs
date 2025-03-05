using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCOOSResponse
    {
        [XmlRoot(ElementName = "OOS_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class OOS_Response
        {
            [XmlElement(ElementName = "err_msg", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Err_msg { get; set; }
        }

        [XmlRoot(ElementName = "OOS", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class OOS
        {
            [XmlElement(ElementName = "OOS_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public OOS_Response OOS_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "OOS", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public OOS OOS { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
