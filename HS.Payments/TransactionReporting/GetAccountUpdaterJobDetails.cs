﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace HS.Payments.TransactionReporting
{
    public class GetAccountUpdaterJobDetails
    {
        /*
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Account Updater job details sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            // parameters for request
            string month = "2017-06";
            string modifiedTypeFilter = "all";

            var request = new getAccountUpdaterJobDetailsRequest();
            request.month = month;
            request.modifiedTypeFilter = modifiedTypeFilter;
            request.paging = new Paging
            {
                limit = 1000,
                offset = 1
            };

            // instantiate the controller that will call the service
            var controller = new getAccountUpdaterJobDetailsController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.auDetails == null)
                    return response;

                foreach (var update in response.auDetails.auUpdate)
                {
                    Console.WriteLine("Profile ID / Payment Profile ID: {0} / {1}", update.customerProfileID, update.customerPaymentProfileID);
                    Console.WriteLine("Update Time (UTC): {0}", update.updateTimeUTC);
                    Console.WriteLine("Reason Code: {0}", update.auReasonCode);
                    Console.WriteLine("Reason Description: {0}", update.reasonDescription);
                }

                foreach (var delete in response.auDetails.auDelete)
                {
                    Console.WriteLine("Profile ID / Payment Profile ID: {0} / {1}", delete.customerProfileID, update.customerPaymentProfileID);
                    Console.WriteLine("Update Time (UTC): {0}", delete.updateTimeUTC);
                    Console.WriteLine("Reason Code: {0}", delete.auReasonCode);
                    Console.WriteLine("Reason Description: {0}", delete.reasonDescription);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                    response.messages.message[0].text);
            }

            return response;
        }*/
    }
}
