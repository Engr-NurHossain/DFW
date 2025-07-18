﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Security.Authentication;
using System.Net;

namespace HS.Payments.PaymentTransactions
{
    public class ChargeCustomerProfile
    {
        public static createTransactionResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId,
            string customerPaymentProfileId, decimal Amount,bool InProduction,string InvoiceId ="", string Description ="")
        {


            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey
            };

            //create a customer payment profile
            customerProfilePaymentType profileToCharge = new customerProfilePaymentType();
            profileToCharge.customerProfileId = customerProfileId;
            profileToCharge.paymentProfile = new paymentProfile {
                paymentProfileId = customerPaymentProfileId
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // refund type
                amount = Amount,
                profile = profileToCharge,
                order = new orderType()
                {
                    invoiceNumber = InvoiceId,
                    description =Description,
                }
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the collector that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
      //      if (response != null)
      //      {
      //          if (response.messages.resultCode == messageTypeEnum.Ok)
      //          {
      //              if(response.transactionResponse.messages != null)
      //              {
      //                  Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
      //                  Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
      //                  Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
      //                  Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
						//Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
      //              }
      //              else
      //              {
      //                  Console.WriteLine("Failed Transaction.");
      //                  if (response.transactionResponse.errors != null)
      //                  {
      //                      Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
      //                      Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
      //                  }
      //              }
      //          }
      //          else
      //          {
      //              Console.WriteLine("Failed Transaction.");
      //              if (response.transactionResponse != null && response.transactionResponse.errors != null)
      //              {
      //                  Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
      //                  Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
      //              }
      //              else
      //              {
      //                  Console.WriteLine("Error Code: " + response.messages.message[0].code);
      //                  Console.WriteLine("Error message: " + response.messages.message[0].text);
      //              }
      //          }
      //      }
      //      else
      //      {
      //          Console.WriteLine("Null Response.");
      //      }

            return response;
        }
    }
}
