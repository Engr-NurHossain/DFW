using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.Custom
{
    public class NMCOverview
    {
        public GetAccountInfo_Response SiteInfo { get; set; }
        public NMCContactResponse.GetAccountContacts ContactList { get; set; }

        public NMCAccountZoneResponse.GetAccountZones ZoneList { get; set; }
        public NMCAccAgencyResponse.GetAccountAgency AccAgencyList { get; set; }
    }
}
