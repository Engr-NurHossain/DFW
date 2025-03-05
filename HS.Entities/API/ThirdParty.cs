using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.API
{
    public class ThirdParty
    {
        public class ThirdPartyCustomerAPIModel
        {
            public ThirdPartyCustomerModel GeneralInformation { set; get; }
            public List<EmergencyContactsModel> EmergencyContacts { set; get; }
            public List<AgenciesModel> Agencies { set; get; }
            public List<SecurityZonesModel> SecurityZones { set; get; }
            public List<UCCEquipmentModel> Equipments { set; get; }
            
        }
        public class ThirdPartyCustomerModel
        {
            public string CSAccountNumber { set; get; }
            public string SiteName { set; get; }
            public string Address { set; get; }
            public string Address2 { set; get; }
            public string Street { set; get; }
            public string City { set; get; }
            public string State { set; get; }
            public string ZipCode { set; get; }
            public string County { set; get; }
            public string SiteType { set; get; }
            public string UccDispatchType { set; get; }
            public string UccDeviceType { set; get; }
            public string ReceiverPhone { set; get; }
            public string PanelPhone { set; get; }
            public string AbortCode { set; get; }

        }
        public class EmergencyContactsModel
        {
            public string FirstName { set; get; }
            public string LastName { set; get; }
            public string Phone { set; get; }

        }
        public class AgenciesModel
        {
            public string Number { set; get; }
            public string Name { set; get; }
            public string Phone { set; get; }

        }
        public class SecurityZonesModel
        {
            public string Number { set; get; }
            public string EventCode { set; get; }
            public string Location { set; get; }

        }
        public class UCCEquipmentModel
        {
            public string TransmitterCode { set; get; }
            public string DeviceType { set; get; }
            public string ReceiverPhone { set; get; }
            public string PanelPhone { set; get; }

        }
    }
}
