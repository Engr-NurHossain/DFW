using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System.Security.Authentication;
using System.Net;

namespace HS.Payments.TransactionReporting
{
    public class GetAccountUpdaterJobSummary
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey,bool InProduction)
        {
            //Console.WriteLine("Get Account Updater job summary sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            // Set a valid month for the request
            string month = "2017-07";

            // Build tbe request object
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new getAUJobSummaryRequest();
            request.month = month;

            // Instantiate the controller that will call the service
            var controller = new getAUJobSummaryController(request);
            controller.Execute();

            // Get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("SUCCESS: Get Account Updater Summary for Month : " + month);
                if (response.auSummary == null)
                {
                    Console.WriteLine("No Account Updater summary for this month.");
                    return response;
                }

                // Displaying the summary of each response in the list
                foreach (var result in response.auSummary)
                {
                    Console.WriteLine("		Reason Code        : " + result.auReasonCode);
                    Console.WriteLine("		Reason Description : " + result.reasonDescription);
                    Console.WriteLine("		# of Profiles updated for this reason : " + result.profileCount);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("ERROR :  Invalid response");
                Console.WriteLine("Response : " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
