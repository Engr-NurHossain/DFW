using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    [XmlRoot(ElementName = "GetAccountInfo_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
    public class GetAccountInfo_Response
    {
        [XmlElement(ElementName = "site_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Site_name { get; set; }
        [XmlElement(ElementName = "site_addr1", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Site_addr1 { get; set; }
        [XmlElement(ElementName = "street_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Street_no { get; set; }
        [XmlElement(ElementName = "street_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Street_name { get; set; }
        [XmlElement(ElementName = "sitestat_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Sitestat_id { get; set; }
        [XmlElement(ElementName = "country_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Country_name { get; set; }
        [XmlElement(ElementName = "timezone_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Timezone_no { get; set; }
        [XmlElement(ElementName = "city_name", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string City_name { get; set; }
        [XmlElement(ElementName = "state_id", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string State_id { get; set; }
    
        public string zip_code { get; set; }
        
        public string receiver_phone { get; set; }
        public string panel_phone { get; set; }
        public string phone1 { get; set; }
        public string panel_location { get; set; }
        public string sitetype_id { get; set; }
        public string systype_id { get; set; }
        public string cspart_no { get; set; }
        public string County_name { get; set; }
        [XmlElement(ElementName = "orig_install_date", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Orig_install_date { get; set; }
        [XmlElement(ElementName = "cs_no", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Cs_no { get; set; }
        [XmlElement(ElementName = "timezone_descr", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Timezone_descr { get; set; }
        [XmlElement(ElementName = "opt_2", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Opt_2 { get; set; }
        [XmlElement(ElementName = "opt_3", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Opt_3 { get; set; }
        [XmlElement(ElementName = "opt_4", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Opt_4 { get; set; }
        [XmlElement(ElementName = "opt_5", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Opt_5 { get; set; }
        [XmlElement(ElementName = "opt_6", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Opt_6 { get; set; }
        [XmlElement(ElementName = "opt_7", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public string Opt_7 { get; set; }
    }

    [XmlRoot(ElementName = "GetAccountInfo", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
    public class GetAccountInfo
    {
        [XmlElement(ElementName = "GetAccountInfo_Response", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public GetAccountInfo_Response GetAccountInfo_Response { get; set; }
    }

    [XmlRoot(ElementName = "NMCNexusDocument", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
    public class NMCNexusDocument
    {
        [XmlElement(ElementName = "GetAccountInfo", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
        public GetAccountInfo GetAccountInfo { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "SiteSystem_Request", Namespace = "http://www.nmccentral.com/webservices/nmcmapi")]
    public class SetUpNmc
    {
        public string CsNo { get; set; }
        public string Site_name { get; set; }
        public string Site_addr1 { get; set; }
        public string Zip_code { get; set; }
        public string City_name { get; set; }
        public string State_id { get; set; }
        public string Sitestat_id { get; set; }
        public string Sitetype_id { get; set; }
        public string Systype_id { get; set; }
        public string cspart_no { get; set; }
        public string phone1 { get; set; }
        public string country_name { get; set; }

        public string receiver_phone { get; set; }
        public string panel_phone { get; set; }
        public string panel_location { get; set; }

        public Guid CustomerId { get; set; }
    }
}
