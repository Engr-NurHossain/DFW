﻿using System;
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
    public class GetTransactionList
    {
        public static getTransactionListResponse Run(String ApiLoginID, String ApiTransactionKey,string batchId,bool InProduction)
        {
            //Console.WriteLine("Get transaction list sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            // unique batch id
            //string batchId = "72309";

            var request = new getTransactionListRequest();
            request.batchId = batchId;
            request.paging = new Paging
            {
                limit = 1000,
                offset = 1
            };

            request.sorting = new TransactionListSorting
            {
                orderBy = TransactionListOrderFieldEnum.id,
                orderDescending = true
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            // instantiate the controller that will call the service
            var controller = new getTransactionListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            getTransactionListResponse response = controller.GetApiResponse();

            /*if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null)
                    return response;

                foreach (var transaction in response.transactions)
                {
                    Console.WriteLine("Transaction Id: {0}", transaction.transId);
                    Console.WriteLine("Submitted on (Local): {0}", transaction.submitTimeLocal);
                    Console.WriteLine("Status: {0}", transaction.transactionStatus);
                    Console.WriteLine("Settle amount: {0}", transaction.settleAmount);
                }
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }*/

            return response;
        }
    }
}
