using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forte.Entities
{
    public class ForteScheduleV3
    {
        public string schedule_id { set; get; }
        public string location_id { set; get; }
        public string customer_token { set; get; }
        public string paymethod_token { set; get; }
        public string action { set; get; }
        public string schedule_quantity { set; get; }
        public string schedule_frequency { set; get; }
        public string schedule_amount { set; get; }
        public string schedule_service_fee_amount { set; get; }
        public string schedule_authorization_amount { set; get; }
        public string schedule_created_date { set; get; }
        public string schedule_start_date { set; get; }
        public string schedule_status { set; get; }
        public string item_description { set; get; }
        public string reference_id { set; get; }
        public string order_number { set; get; }
        public List<ScheduleSummary> schedule_summary { set; get; }
    }
    public class ScheduleSummary
    {
        public float schedule_next_amount { set; get; }
        public float schedule_next_authorization_amount { set; get; }
        public string schedule_next_date { set; get; }
        //public string schedule_last_amount { set; get; }
        //public string schedule_last_authorization_amount { set; get; }
        //public string schedule_last_date { set; get; }
        public float schedule_successful_amount { set; get; }
        public float schedule_successful_authorization_amount { set; get; }
        public int schedule_successful_quantity { set; get; }
        public float schedule_failed_amount { set; get; }
        public float schedule_failed_authorization_amount { set; get; }
        public int schedule_failed_quantity { set; get; }
        public float schedule_remaining_amount { set; get; }
        public float schedule_remaining_authorization_amount { set; get; }
        public int schedule_remaining_quantity { set; get; }
        public float schedule_suspended_amount { set; get; }
        public float schedule_suspended_authorization_amount { set; get; }
        public int schedule_suspended_quantity { set; get; }
    }
}

/*

{
    "schedule_id": "sch_81f2bff7-11e2-4cd8-b451-07c317edbd7b",
  "location_id": "loc_192642",
  "customer_token": "cst_h_TrrHANEU6XjmMV_EMVrA",
  "paymethod_token": "mth_cp459q53Q0W5wJdMG35f1w",
  "action": "sale",
  "schedule_quantity": 12,
  "schedule_frequency": "monthly",
  "schedule_amount": 25,
  "schedule_service_fee_amount": 0,
  "schedule_authorization_amount": 25,
  "schedule_created_date": "2017-09-08T09:07:19.52",
  "schedule_start_date": "2018-12-11T08:00:00",
  "schedule_status": "active",
  "item_description": "Car Payment",
  "reference_id": "INV-123",
  "order_number": "98762222",
  "schedule_summary": {
    "schedule_next_amount": 25,
    "schedule_next_authorization_amount": 25,
    "schedule_next_date": "2018-12-11T08:00:00",
    "schedule_last_amount": 25,
    "schedule_last_authorization_amount": 25,
    "schedule_last_date": "2019-11-11T08:00:00",

    "schedule_successful_amount": 0,
    "schedule_successful_authorization_amount": 0,
    "schedule_successful_quantity": 0,
    "schedule_failed_amount": 0,
    "schedule_failed_authorization_amount": 0,
    "schedule_failed_quantity": 0,
    "schedule_remaining_amount": 300,

    "schedule_remaining_authorization_amount": 300,
    "schedule_remaining_quantity": 12,
    "schedule_suspended_amount": 0,
    "schedule_suspended_authorization_amount": 0,
    "schedule_suspended_quantity": 0
  },
  "xdata": {
        "xdata_1": "inv-321",
    "xdata_2": "Southern Branch"
  },
  "response": {
        "environment": "live",
    "response_desc": "Get Successful."
  },
  "links": {
        "scheduleitems": "https://api.forte.net/v3/schedules/sch_81f2bff7-11e2-4cd8-b451-07c317edbd7b/scheduleitems",
    "self": "https://api.forte.net/v3/schedules/sch_81f2bff7-11e2-4cd8-b451-07c317edbd7b"
  }
}
*/