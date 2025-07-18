﻿using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace HS.Payments.CustomerProfiles
{
    public class DeleteCustomerPaymentProfile
    {
        public static string Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId, string customerPaymentProfileId)
        {
            //Console.WriteLine("DeleteCustomerPaymentProfile Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //please update the subscriptionId according to your sandbox credentials
            var request = new deleteCustomerPaymentProfileRequest
            {
                customerProfileId = customerProfileId,
                customerPaymentProfileId = customerPaymentProfileId
            };

            //Prepare Request
            var controller = new deleteCustomerPaymentProfileController(request);
            controller.Execute();

            //Send Request to EndPoint
            string Message = "";
            deleteCustomerPaymentProfileResponse response = controller.GetApiResponse();
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    //Console.WriteLine("Success, ResultCode : " + response.messages.resultCode.ToString());
                    Message = "Success, ResultCode : " + response.messages.resultCode.ToString();
                }
            }
            else if(response != null)
            {
                //Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                Message = "Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text;
            }
            return Message;
            //return response;
        }
    }
}
