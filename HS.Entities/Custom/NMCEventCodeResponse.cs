using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCEventCodeResponse
    {
        [XmlRoot(ElementName = "GetEventCode_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetEventCode_Response
        {
            [XmlElement(ElementName = "event_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Event_id { get; set; }
            [XmlElement(ElementName = "descr", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Descr { get; set; }
            [XmlElement(ElementName = "response_code", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Response_code { get; set; }
            [XmlElement(ElementName = "priority", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Priority { get; set; }
        }

        [XmlRoot(ElementName = "GetEventCode", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetEventCode
        {
            [XmlElement(ElementName = "GetEventCode_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public List<GetEventCode_Response> GetEventCode_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "GetEventCode", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public GetEventCode GetEventCode { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
