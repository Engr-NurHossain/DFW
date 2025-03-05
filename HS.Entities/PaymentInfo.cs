using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class PaymentInfo
    {
        public Guid PaymentCustomerId { get; set; }
        public string BillMethod { get; set; }
        public string PayFor { set; get; }
        public string PaymentMethod { set; get; }

        public string Type { set; get; }
        public int? PaymentInfoCustomerId { set; get; }
        public string MethodType { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerBillingStreet { get; set; }
        public string CustomerBillingZip { get; set; }
        public string CustomerBillingCity { get; set; }
        public string CustomerBillingState { get; set; }
        public bool IsFromBooking { get; set; }
    
    }
    public static class PaymentInfoExtentions
    {
        public static bool IsACH(this PaymentInfo PaymentInfo)
        {
            if (string.IsNullOrWhiteSpace(PaymentInfo.RoutingNo))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PaymentInfo.AcountNo))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PaymentInfo.BankAccountType))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PaymentInfo.AccountName))
            {
                return false;
            }
            return true;
        }
        public static bool IsCC(this PaymentInfo PaymentInfo)
        {
            if (string.IsNullOrWhiteSpace(PaymentInfo.AccountName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PaymentInfo.CardNumber))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PaymentInfo.CardExpireDate))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(PaymentInfo.CardSecurityCode))
            {
                return false;
            }
            return true;
        }
    }
}
