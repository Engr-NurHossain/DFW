using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Net;
using System.Security.Authentication;

namespace HS.Payments.RecurringBilling
{
    public class GetSubscription
    {
        public static ARBGetSubscriptionResponse Run(String ApiLoginID, String ApiTransactionKey, string subscriptionId,bool InProduction)
        {
            //Console.WriteLine("Get Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new ARBGetSubscriptionRequest { subscriptionId = subscriptionId };
            request.includeTransactions = true;

            var controller = new ARBGetSubscriptionController(request);          // instantiate the controller that will call the service
            controller.Execute();

            ARBGetSubscriptionResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)

            // validate response
            /*if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.subscription != null)
                {
                    Console.WriteLine("Subscription returned : " + response.subscription.name);
                }
            }*/

           /* else if (response != null)
            {
                if (response.messages.message.Length > 0)
                {
                    Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                      response.messages.message[0].text);
                }
            }
            else
            {
                if (controller.GetErrorResponse().messages.message.Length > 0)
                {
                    Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                }
            }*/

            return response;
        }
    }
}
