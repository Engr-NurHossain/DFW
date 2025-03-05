using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCAccountZoneResponse
    {
        [XmlRoot(ElementName = "GetAccountZones_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetAccountZones_Response
        {
            [XmlElement(ElementName = "zone_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Zone_id { get; set; }
            [XmlElement(ElementName = "equiploc_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Equiploc_id { get; set; }
            [XmlElement(ElementName = "restore_reqd_flag", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Restore_reqd_flag { get; set; }
            [XmlElement(ElementName = "trouble_state_flag", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Trouble_state_flag { get; set; }
            [XmlElement(ElementName = "bypass_state_flag", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Bypass_state_flag { get; set; }
            [XmlElement(ElementName = "trip_count", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Trip_count { get; set; }
            [XmlElement(ElementName = "comment", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Comment { get; set; }
            [XmlElement(ElementName = "equiptype_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Equiptype_id { get; set; }
            [XmlElement(ElementName = "zonestate_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Zonestate_id { get; set; }
            [XmlElement(ElementName = "event_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Event_id { get; set; }
        }

        [XmlRoot(ElementName = "GetAccountZones", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetAccountZones
        {
            [XmlElement(ElementName = "GetAccountZones_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public List<GetAccountZones_Response> GetAccountZones_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "GetAccountZones", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public GetAccountZones GetAccountZones { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
