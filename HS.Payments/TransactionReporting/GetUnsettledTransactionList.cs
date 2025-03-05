using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Security.Authentication;
using System.Net;

namespace HS.Payments.TransactionReporting
{
    public class GetUnsettledTransactionList
    { 
        public static getUnsettledTransactionListResponse Run(String ApiLoginID, String ApiTransactionKey,bool InProduction)
        {
            //Console.WriteLine("Get unsettled transaction list sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new getUnsettledTransactionListRequest();
            request.status  = TransactionGroupStatusEnum.any;
            //request.statusSpecified = true;
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
            // instantiate the controller that will call the service
            var controller = new getUnsettledTransactionListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            /*if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null) 
                    return response;

                foreach (var item in response.transactions)
                {
                    Console.WriteLine("Transaction Id: {0} was submitted on {1}", item.transId,
                        item.submitTimeLocal);
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