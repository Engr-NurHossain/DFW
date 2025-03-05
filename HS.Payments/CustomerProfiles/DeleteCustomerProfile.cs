using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace HS.Payments.CustomerProfiles
{
    public class DeleteCustomerProfile
    {
        public static string Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId)
        {
            //Console.WriteLine("DeleteCustomerProfile Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            //please update the subscriptionId according to your sandbox credentials
            var request = new deleteCustomerProfileRequest
            {
                customerProfileId = customerProfileId
            };

            //Prepare Request
            var controller = new deleteCustomerProfileController(request);
            controller.Execute();
            string message = "";

             //Send Request to EndPoint
            deleteCustomerProfileResponse response = controller.GetApiResponse(); 
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    //Console.WriteLine("Success, ResultCode : " + response.messages.resultCode.ToString());
                    message = "Success, ResultCode : " + response.messages.resultCode.ToString();
                }
            }
            else if (response != null && response.messages.message != null)
            {
                //Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                message = "Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text;
            }
            return message;

            //return response;
        }
    }
}
