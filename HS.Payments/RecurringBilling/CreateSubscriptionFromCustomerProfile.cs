using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Security.Authentication;
using System.Net;

namespace HS.Payments.RecurringBilling
{
    public class CreateSubscriptionFromCustomerProfile
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, short intervalLength,
            string customerProfileId, string customerPaymentProfileId, string customerAddressId , bool InProduction)
        {
            //Console.WriteLine("Create Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey
            };

            paymentScheduleTypeInterval interval = new paymentScheduleTypeInterval();

            interval.length = intervalLength;                        // months can be indicated between 1 and 12
            interval.unit   = ARBSubscriptionUnitEnum.days;

            paymentScheduleType schedule = new paymentScheduleType
            {
                interval            = interval,
                startDate           = DateTime.Now.AddDays(1),      // start date should be tomorrow
                totalOccurrences    = 9999,                          // 999 indicates no end date
                trialOccurrences     = 3
            };

            #region Payment Information
            var creditCard = new creditCardType
            {
                cardNumber      = "4111111111111111",
                expirationDate  = "0718"
            };

            //standard api call to retrieve response
            paymentType cc = new paymentType { Item = creditCard };
            #endregion

            customerProfileIdType customerProfile = new customerProfileIdType()
            {
                customerProfileId = customerProfileId,
                customerPaymentProfileId = customerPaymentProfileId,
                customerAddressId = customerAddressId
            };

            ARBSubscriptionType subscriptionType = new ARBSubscriptionType()
            {
                amount = 35.55m,
                trialAmount = 0.00m,
                paymentSchedule = schedule,
                profile = customerProfile
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new ARBCreateSubscriptionRequest {subscription = subscriptionType };

            var controller = new ARBCreateSubscriptionController(request);          // instantiate the controller that will call the service
            controller.Execute();

            ARBCreateSubscriptionResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)
           
            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, Subscription ID : " + response.subscriptionId.ToString());
                }
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
