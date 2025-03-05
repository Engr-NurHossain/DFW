using System;
using HS.Entities;
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
    public class DebitBankAccount
    {
        public static createTransactionResponse Run(String ApiLoginID, String ApiTransactionKey,ACHInfo ACHInfo,bool InProduction)
        {
            //Console.WriteLine("Debit Bank Account Transaction");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = InProduction ? AuthorizeNet.Environment.PRODUCTION : AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey
            };

            var bankAccount = new bankAccountType
            {
                //accountType     = bankAccountTypeEnum.checking,
                routingNumber   = ACHInfo.RoutingNo,
                accountNumber   = ACHInfo.AccountNo,
                nameOnAccount   = ACHInfo.AccountName,
                //echeckType      = echeckTypeEnum.WEB,   // change based on how you take the payment (web, telephone, etc)
                bankName        = ACHInfo.BankName,
                
                // checkNumber     = "101"                 // needed if echeckType is "ARC" or "BOC"
            };
            if (!string.IsNullOrWhiteSpace(ACHInfo.AccountType))
            {
                if (ACHInfo.AccountType == "checking")
                {
                    bankAccount.accountType = bankAccountTypeEnum.checking;
                }
                else if (ACHInfo.AccountType == "savings")
                {
                    bankAccount.accountType = bankAccountTypeEnum.savings;
                }
                else if (ACHInfo.AccountType == "businessChecking")
                {
                    bankAccount.accountType = bankAccountTypeEnum.businessChecking;
                }
            }
            else if (!string.IsNullOrWhiteSpace(ACHInfo.ECheckType))
            {
                if (ACHInfo.ECheckType == "PPD")
                {
                    bankAccount.echeckType = echeckTypeEnum.PPD;
                }
                else if (ACHInfo.ECheckType == "WEB")
                {
                    bankAccount.echeckType = echeckTypeEnum.WEB;
                }
            }

            // standard api call to retrieve response
            var paymentType = new paymentType { Item = bankAccount };
            orderType OrderInfo = new orderType()
            {
                description = ACHInfo.Description,
                invoiceNumber = ACHInfo.InvoiceNo,
                
            };
            customerAddressType addressInfo = new customerAddressType()
            {
                email = ACHInfo.EmailAddress,
                firstName = ACHInfo.FirstName,
                lastName = ACHInfo.Lastname,
                address = ACHInfo.BillingAddress,
                city = ACHInfo.City,
                state = ACHInfo.State,
                zip = ACHInfo.Zipcode,
                company = ACHInfo.Company,
                phoneNumber = ACHInfo.Phone,
            };
            customerDataType customerInfo = new customerDataType()
            {
                id=ACHInfo.CustomerId,
                email = ACHInfo.EmailAddress,
            };
            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // refund type
                payment = paymentType,
                amount = (Decimal)ACHInfo.Amount,
                order = OrderInfo,
                billTo = addressInfo,
                customer = customerInfo,
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var request = new createTransactionRequest {
                transactionRequest = transactionRequest,
            };

            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            createTransactionResponse response = controller.GetApiResponse();

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
						//Console.WriteLine("Success, Transaction Code : " + response.transactionResponse.transId);
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
