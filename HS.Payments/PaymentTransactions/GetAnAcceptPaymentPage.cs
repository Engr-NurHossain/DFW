﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System.Security.Authentication;
using System.Net;

namespace HS.Payments.PaymentTransactions.CustomerProfiles
{
    public class GetAnAcceptPaymentPage
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount,bool InProduction)
        {
            //Console.WriteLine("GetAnAcceptPaymentPage Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            settingType[] settings = new settingType[2];

            settings[0] = new settingType();
            settings[0].settingName = settingNameEnum.hostedPaymentButtonOptions.ToString();
            settings[0].settingValue = "{\"text\": \"Pay\"}";
            
            settings[1] = new settingType();
            settings[1].settingName = settingNameEnum.hostedPaymentOrderOptions.ToString();
            settings[1].settingValue = "{\"show\": false}";

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize capture only
                amount = amount
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new getHostedPaymentPageRequest();
            request.transactionRequest = transactionRequest;
            request.hostedPaymentSettings = settings;

            // instantiate the controller that will call the service
            var controller = new getHostedPaymentPageController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("Message code : " + response.messages.message[0].code);
                Console.WriteLine("Message text : " + response.messages.message[0].text);
                Console.WriteLine("Token : " + response.token);
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                Console.WriteLine("Failed to get hosted payment page");
            }

            return response;
        }
    }
}
