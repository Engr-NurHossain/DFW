using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NmcCreateResponse
    {
        [XmlRoot(ElementName = "SiteSystem_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class SiteSystem_Response
        {
            [XmlElement(ElementName = "err_msg", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Err_msg { get; set; }
            public string err_code{ get; set; }
            public string addl_err_info { get; set; }
        }

        [XmlRoot(ElementName = "SiteSystem", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class SiteSystem
        {
            [XmlElement(ElementName = "SiteSystem_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public SiteSystem_Response SiteSystem_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "SiteSystem", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public SiteSystem SiteSystem { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
