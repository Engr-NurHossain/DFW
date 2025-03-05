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
    public class CreateSubscription
    {
        public static ARBCreateSubscriptionResponse Run(String ApiLoginID, String ApiTransactionKey, ARBSubscription Model,bool InProduction)
        {
            //Console.WriteLine("Create Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            paymentScheduleTypeInterval interval = new paymentScheduleTypeInterval()
            {
                length = Model.SubscriptionInterval,                     // months can be indicated between 1 and 12
                unit = ARBSubscriptionUnitEnum.months
            }; 
            paymentScheduleType schedule = new paymentScheduleType
            {
                interval            = interval,
                startDate           = Model.SubscriptionStartDate,      // start date should be tomorrow
               
                totalOccurrences    = Model.TotalOccurrences,       // 999 indicates no end date
                trialOccurrences     = Model.TrialOccurrences
            };


            #region Payment Information
            bankAccountType bankAccount ;
            creditCardType creditCard ;
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
                else if (Model.BankAccountType == "WEB")
                {
                    bankAccount.echeckType = echeckTypeEnum.WEB;
                }
                else if (Model.BankAccountType == "CCD")
                {
                    bankAccount.echeckType = echeckTypeEnum.CCD;
                }
                #endregion

                cc = new paymentType { Item = bankAccount };
            }
            else
            {
                creditCard = new creditCardType
                {
                    cardNumber = Model.CardNumber,
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
                zip = Model.State
            };
            orderType order = new orderExType()
            { 
                invoiceNumber = Model.CustomerId,//Model.Invoice,
                description = Model.Description,
            };
            customerType customer = new customerType()
            {
                id = Model.CustomerId,//Model.Invoice,
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
                name = string.Concat(Model.FirstName," ",Model.LastName),

            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new ARBCreateSubscriptionRequest {subscription = subscriptionType };

            var controller = new ARBCreateSubscriptionController(request);          // instantiate the controller that will call the service
            controller.Execute();

            ARBCreateSubscriptionResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)
            if (response == null)
            { 
                var Error = controller.GetErrorResponse();
                if (Error != null && Error.messages != null)
                {
                    response = new ARBCreateSubscriptionResponse();
                    response.messages = new messagesType()
                    {
                        resultCode = messageTypeEnum.Error,
                        message = Error.messages.message,

                    };
                    response.subscriptionId = "";
                } 
            }
            // validate response
            //if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            //{
            //    if (response != null && response.messages.message != null)
            //    {
            //        Console.WriteLine("Success, Subscription ID : " + response.subscriptionId.ToString());
            //    }
            //}
            //else if(response != null)
            //{
            //    //Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            //}

            return response;
        }
    }
}
