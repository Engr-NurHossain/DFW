using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class NMCSignalHIstoryResponse
    {
        [XmlRoot(ElementName = "SignalHistory_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class SignalHistory_Response
        {
            [XmlElement(ElementName = "sig_acct", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Sig_acct { get; set; }
            [XmlElement(ElementName = "sig_date", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Sig_date { get; set; }
            [XmlElement(ElementName = "sig_code", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Sig_code { get; set; }
            [XmlElement(ElementName = "event", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Event { get; set; }
            [XmlElement(ElementName = "eventhistcomment", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Eventhistcomment { get; set; }
            [XmlElement(ElementName = "test_seqno", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public string Test_seqno { get; set; }
        }

        [XmlRoot(ElementName = "SignalHistory", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class SignalHistory
        {
            [XmlElement(ElementName = "SignalHistory_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public List<SignalHistory_Response> SignalHistory_Response { get; set; }
        }

        [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public class NMCNexusDocument
        {
            [XmlElement(ElementName = "SignalHistory", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
            public SignalHistory SignalHistory { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
