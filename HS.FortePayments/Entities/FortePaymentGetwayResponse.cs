using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forte.Entities
{
    public class FortePaymentGetwayResponse
    {
        public bool Result { get; set; }
        public string Massege { get; set; }
    }
    public class ForteCustomerCreate
    {
        public bool Result { get; set; }
        public string Massege { get; set; }
        public string CustomerToken { get; set; }
        public string PaymentToken { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class FortePaymentCreate
    {
        public bool Result { get; set; }
        public string Massege { get; set; }
        public string PaymentToken { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ForteResponseDetails
    {
        public bool Result { get; set; }
        public string Massege { get; set; }
        public string ScheduleToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
