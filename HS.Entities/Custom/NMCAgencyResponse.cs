using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCAgencyResponse
    {
        [XmlRoot(ElementName = "GetDispatchAgencies_Response")]
        public class GetDispatchAgencies_Response
        {
            [XmlElement(ElementName = "agency_no")]
            public string Agency_no { get; set; }
            [XmlElement(ElementName = "agencytype_id")]
            public string Agencytype_id { get; set; }
            [XmlElement(ElementName = "agency_name")]
            public string Agency_name { get; set; }
            [XmlElement(ElementName = "city_name")]
            public string City_name { get; set; }
            [XmlElement(ElementName = "state_id")]
            public string State_id { get; set; }
           
            [XmlElement(ElementName = "zip_code")]
            public string zip_code { get; set; }
            [XmlElement(ElementName = "phone1")]
            public string Phone1 { get; set; }
            [XmlElement(ElementName = "phone2")]
            public string Phone2 { get; set; }
            [XmlElement(ElementName = "change_date")]
            public string Change_date { get; set; }
        
            
        }

        [XmlRoot(ElementName = "GetDispatchAgencies")]
        public class GetDispatchAgencies
        {
            [XmlElement(ElementName = "GetDispatchAgencies_Response")]
            public List<GetDispatchAgencies_Response> GetDispatchAgencies_Response { get; set; }
        }
        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "GetDispatchAgencies", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public GetDispatchAgencies GetDispatchAgencies { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
