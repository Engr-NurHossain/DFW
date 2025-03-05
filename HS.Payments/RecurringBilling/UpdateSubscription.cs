using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using HS.Entities;
using System.Security.Authentication;
using System.Net;

namespace HS.Payments.RecurringBilling
{
    public class UpdateSubscription
    {
        public static ARBUpdateSubscriptionResponse Run(string ApiLoginID, string ApiTransactionKey, string subscriptionId, ARBSubscription Model,bool InProduction,bool UseExistingPaymentProfile = false)
        {
            //Console.WriteLine("Update Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction? AuthorizeNet.Environment.PRODUCTION: AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };
            paymentScheduleTypeInterval interval = new paymentScheduleTypeInterval()
            {
                length = Model.SubscriptionInterval,                     // months can be indicated between 1 and 12
                unit = ARBSubscriptionUnitEnum.months
            };
            paymentScheduleType schedule = new paymentScheduleType
            {
                //interval = interval, //interval can't be changed in update
                startDate = Model.SubscriptionStartDate,      // start date should be tomorrow

                totalOccurrences = Model.TotalOccurrences,       // 999 indicates no end date
                trialOccurrences = Model.TrialOccurrences
            };

            #region Payment Information
            bankAccountType bankAccount;
            creditCardType creditCard;
            paymentType cc;
            if (Model.PaymentMethod == "ACH")
            {
                bankAccount = new bankAccountType
                {
                    nameOnAccount = Model.NameOnBankAccount,
                    routingNumber = Model.BankARBRoutingNumber,
                    accountNumber = Model.BankAccountNumber,
                    bankName = Model.BankName,

                };

                #region BankAccountType
                if (Model.BankAccountType == "checking")
                {
                    bankAccount.accountType = bankAccountTypeEnum.checking;
                }
                else if (Model.BankAccountType == "savings")
                {
                    bankAccount.accountType = bankAccountTypeEnum.savings;
                }
                else if (Model.BankAccountType == "businessChecking")
                {
                    bankAccount.accountType = bankAccountTypeEnum.businessChecking;
                }
                #endregion

                #region eCheckType
                if (Model.ECheckType == "PPD")
                {
                    bankAccount.echeckType = echeckTypeEnum.PPD;
                }
                else if (Model.ECheckType == "WEB")
                {
                    bankAccount.echeckType = echeckTypeEnum.WEB;
                }
                #endregion

                cc = new paymentType { Item = bankAccount };
            }
            else
            {
                creditCard = new creditCardType
                {
                    cardNumber = Model.CardNumber.Replace("-","").Replace(" ",""),
                    expirationDate = Model.ExpirationDate, //MMYY
                    cardCode = Model.CardPassword,

                };
                cc = new paymentType { Item = creditCard };
            }

            //standard api call to retrieve response
            #endregion

            nameAndAddressType addressInfo = new nameAndAddressType()
            {
                //firstName = Model.FirstName,
                //lastName = Model.LastName,
                firstName = Model.BillingFirstName,
                lastName = Model.BillingLastName,
                address = Model.Address,
                city = Model.City,
                company = Model.CompanyName,
                country = Model.Country,
                state = Model.State,
                zip = Model.State,
                
            };

            #region CustomerProfile
            //customerProfileIdType customerProfile = null;
            //if (!string.IsNullOrWhiteSpace(Model.CustomerProfileId)
            //    || !string.IsNullOrWhiteSpace(Model.CustomerPaymentProfileId) 
            //    || !string.IsNullOrWhiteSpace(Model.CustomerAddressId))
            //{
            //    customerProfile = new customerProfileIdType(){ };
            //}
            ////customerProfileId = "1232312",
            ////        customerPaymentProfileId = "2132132",
            ////        customerAddressId = "1233432"

            #endregion

            orderType order = new orderExType()
            {
                invoiceNumber = Model.CustomerId,//previous//Model.Invoice,
                description = Model.Description,
            };
            customerType customer = new customerType()
            {
                id = Model.CustomerId,
                email = Model.EmailAddress,
                
            };

            ARBSubscriptionType subscriptionType = new ARBSubscriptionType()
            {
                amount = Model.SubscritptionAmount,
                trialAmount = Model.TrialAmount,

                paymentSchedule = schedule,
                billTo = addressInfo,
                payment = cc,
                shipTo = addressInfo,
                order = order,
                customer = customer,
                
                name = string.Concat(Model.FirstName, " ", Model.LastName),
            };
            if(UseExistingPaymentProfile == true)
            {
                subscriptionType = new ARBSubscriptionType()
                {
                    amount = Model.SubscritptionAmount,
                    trialAmount = Model.TrialAmount,

                    paymentSchedule = schedule,
                    billTo = addressInfo,
                    //payment = cc, this will be empty
                    shipTo = addressInfo,
                    order = order,
                    customer = customer,

                    name = string.Concat(Model.FirstName, " ", Model.LastName),
                };
            }

            #region For updating CustomerID
            //ARBSubscriptionType subscriptionType = new ARBSubscriptionType()
            //{
            //    //amount = Model.SubscritptionAmount,
            //    //trialAmount = Model.TrialAmount,

            //    //paymentSchedule = schedule,
            //    //billTo = addressInfo,
            //    //payment = cc,
            //    //shipTo = addressInfo,
            //    order = order,
            //    customer = customer,

            //    name = string.Concat(Model.FirstName/*, " ", Model.LastName*/),
            //};

            #endregion

            //Please change the subscriptionId according to your request

            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new ARBUpdateSubscriptionRequest { subscription = subscriptionType, subscriptionId = subscriptionId };
            var controller = new ARBUpdateSubscriptionController(request);         
            controller.Execute();

            ARBUpdateSubscriptionResponse response = controller.GetApiResponse();

            #region Response
            //// validate response
            //if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            //{
            //    if (response != null && response.messages.message != null)
            //    {
            //        Console.WriteLine("Success, RefID Code : " + response.refId);
            //    }
            //}
            //else if(response != null)
            //{
            //    Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            //}
            #endregion

            return response;
        }
    }
}
