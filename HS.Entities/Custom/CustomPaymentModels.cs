using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public class CustomPaymentModels
    {

    }
    public class ARBSubscription
    {
        public string CustomerId { set; get; }
        //ACH
        //Debit Card
        public string PaymentMethod { set; get; }

        #region Credit Card Info
        public string CardNumber { set; get; }
        public string ExpirationDate { set; get; }
        public string CardPassword { set; get; }
        #endregion

        #region Bank Account Info
        public string BankAccountNumber { set; get; }
        public string NameOnBankAccount { set; get; }
        public string BankARBRoutingNumber { set; get; }
        //checking
        //savings
        //businessChecking
        public string BankAccountType { set; get; }
        //PPD = 0,
        //WEB = 1,
        //CCD = 2,
        //TEL = 3,
        //ARC = 4,
        //BOC = 5
        public string ECheckType { set; get; }
        public string BankName { set; get; }
        #endregion
        
        public Decimal SubscritptionAmount { set; get; }
        public Decimal TrialAmount { set; get; } 
        public string Invoice { set; get; }
        public string Description { set; get; }
        
        public DateTime SubscriptionStartDate { set; get; }
        public short SubscriptionInterval { set; get; }
        

        public short TrialOccurrences { set; get; }
        public short TotalOccurrences { set; get; }

        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string BillingFirstName { set; get; }
        public string BillingLastName { set; get; }

        public string Address { set; get; }
        public string City { set; get; }
        public string Country { set; get; }
        public string CompanyName { set; get; }
        public string State { set; get; }
        public string Zip { set; get; }

        public string CustomerProfileId { set; get; }
        public string CustomerPaymentProfileId { set; get; }
        public string CustomerAddressId { set; get; }

        public string EmailAddress { set; get; }

    }
}
 
