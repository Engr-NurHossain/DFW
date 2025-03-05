using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCContactResponse
    {
        [XmlRoot(ElementName = "GetAccountContacts_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetAccountContacts_Response
        {
            [XmlElement(ElementName = "last_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Last_name { get; set; }
            [XmlElement(ElementName = "first_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string First_name { get; set; }
            [XmlElement(ElementName = "cs_seqno", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Cs_seqno { get; set; }
            [XmlElement(ElementName = "has_key_flag", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Has_key_flag { get; set; }
            [XmlElement(ElementName = "phone1", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Phone1 { get; set; }
            [XmlElement(ElementName = "phonetype1", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Phonetype1 { get; set; }
            [XmlElement(ElementName = "phone1_seqno", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Phone1_seqno { get; set; }
            [XmlElement(ElementName = "email_address", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Email_address { get; set; }
            [XmlElement(ElementName = "ctaclink_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Ctaclink_no { get; set; }
            [XmlElement(ElementName = "contact_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Contact_no { get; set; }
            [XmlElement(ElementName = "user_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string User_id { get; set; }
            public string relation_id { get; set; }
        }

        [XmlRoot(ElementName = "GetAccountContacts", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class GetAccountContacts
        {
            [XmlElement(ElementName = "GetAccountContacts_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public List<GetAccountContacts_Response> GetAccountContacts_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "GetAccountContacts", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public GetAccountContacts GetAccountContacts { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
