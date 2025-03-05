using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCTestAccountResponse
    {
        [XmlRoot(ElementName = "Test_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class Test_Response
        {
            [XmlElement(ElementName = "err_msg", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Err_msg { get; set; }
            [XmlElement(ElementName = "effective_date", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Effective_date { get; set; }
            [XmlElement(ElementName = "expire_date", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Expire_date { get; set; }
            [XmlElement(ElementName = "test_seqno", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Test_seqno { get; set; }
        }

        [XmlRoot(ElementName = "Test", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class Test
        {
            [XmlElement(ElementName = "Test_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public Test_Response Test_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "Test", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public Test Test { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
