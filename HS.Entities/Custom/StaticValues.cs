using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public static class StaticValues
    {
        
    }
    public static class AppoinmentType
    {
        public static string Sales { get { return "Sales"; } }
        public static string ServiceOrder { get { return "ServiceOrder"; } }
        public static string WorkOrder { get { return "WorkOrder"; } }
    }
    public static class FundingType
    {
        public static string Income { get { return "Income";  } }
        public static string Expense { get { return "Expense"; } }
    }
    public static class AccountName
    {
        public static string AlarmDotCom { get { return "Alarm.com"; } }
        public static string Monitronics { get { return "Monitronics"; } }
        public static string CentralStation { get { return "Central Station"; } }
    }
    public static class TextTemplateType
    {
        public static string Lead { get { return "Lead"; } }
        public static string Customer { get { return "Customer"; } }
    }
    public static class CustomerBillType
    {
        public static string BillType { get { return "CustomerBill"; } }
    }
    public static class ThirdPartyType
    {
        public static string AlarmCom { get { return "AlarmDotcom"; } }
        public static string AuthorizeNet { get { return "AuthorizeDotNet"; } }
        public static string TechScheduleSetting { get { return "TechSchedule"; } }
    }

    public static class MethodBilling
    {
        public static string ACH { get { return "ACH"; } }
        public static string EFT { get { return "EFT"; } }
        public static string CreditCard { get { return "Credit Card"; } }
        public static string DebitCard { get { return "Debit Card"; } }
        public static string Check { get { return "Check"; } }
        public static string Cash { get { return "Cash"; } }
    }

    public static class ScheduleType
    {
        public static string QA1 { get { return "QA1"; } }
        public static string QA2 { get { return "QA2"; } }
        public static string Installer { get { return "Installer"; } }
    }

    public static class ScheduleTitle
    {
        public static string QA1Required { get { return "QA1 Required For"; } }
        public static string QA2Required { get { return "QA2 Required For"; } }
        public static string InstallerRequired { get { return "Installer Required For"; } }
    }
}
