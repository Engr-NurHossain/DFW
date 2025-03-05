using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCAccAgencyResponse
    {
        [XmlRoot(ElementName = "GetAccountAgency_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetAccountAgency_Response
        {
            [XmlElement(ElementName = "agency_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Agency_no { get; set; }
            [XmlElement(ElementName = "agencytype_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Agencytype_id { get; set; }
            [XmlElement(ElementName = "agency_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Agency_name { get; set; }
            [XmlElement(ElementName = "phone1", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Phone1 { get; set; }
            [XmlElement(ElementName = "permit_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Permit_no { get; set; }
            [XmlElement(ElementName = "permtype_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Permtype_id { get; set; }
            [XmlElement(ElementName = "effective_date", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Effective_date { get; set; }
            [XmlElement(ElementName = "permstat_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Permstat_id { get; set; }
            [XmlElement(ElementName = "descr", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Descr { get; set; }
            [XmlElement(ElementName = "expire_date", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Expire_date { get; set; }
        }

        [XmlRoot(ElementName = "GetAccountAgency", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetAccountAgency
        {
            [XmlElement(ElementName = "GetAccountAgency_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public List<GetAccountAgency_Response> GetAccountAgency_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "GetAccountAgency", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public GetAccountAgency GetAccountAgency { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
