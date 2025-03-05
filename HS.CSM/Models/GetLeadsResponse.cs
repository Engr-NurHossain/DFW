using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.CSM.Models
{
    public class GetLeadsResponse
    {
        public int id { set; get; }
        public DateTime timestamp { set; get; }
        public string firstName { set; get; }
        public string lastName { set; get; }
        public string phone { set; get; }
        public string email { set; get; }
        public string street { set; get; }
        public string city { set; get; }
        public string state { set; get; }
        public string zip { set; get; }
        public string comment { set; get; }
        public List<CSMNote> notes { set; get; }
        public string campaignID { set; get; }
        public string campaign { set; get; }
        public string ServiceType { set; get; }
        public string country { set; get; }
        public string formType { set; get; }
        public int? callDuration { set; get; }
        public string recordingUrl { set; get; }
        public int? valid { set; get; }
        public string billable { set; get; }
        public string leadCapturedOn { set; get; } 
        public int? isAppointmentSet { set; get; }
        public string appointmentDate { set; get; }
        public int? soldStatus { set; get; }
    }

    public class CSMNote
    {
        public int noteID { set; get; }
        public int userId { set; get; }
        public string userName { set; get; }
        public string userEmail { set; get; }
        public string comment { set; get; }
        public DateTime dateTime { set; get; }
    }
    public class CSMAdditionalNote
    {
        [JsonProperty("Lead ID")]
        public int LeadId { set; get; }
        public List<CSMNote> notes { set; get; }
    }

    public class GetLeadsResponseList
    {
        [JsonProperty("Leads")]
        public List<GetLeadsResponse> GetLeadsResponse { set; get; }

        [JsonProperty("Notes")] 
        public List<CSMAdditionalNote> GetNotesResponse { set; get; }
    }

    public class JupiterResponse
    {
        [JsonProperty("Data")]
        public List<GetLeadsResponseList> GetLeadsResponseList { set; get; }
    }
}
