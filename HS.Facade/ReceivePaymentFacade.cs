using AuthorizeNet.Api.Contracts.V1;
using Forte;
using Forte.Entities;
using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using HS.Payments.PaymentTransactions;
using HS.Payments.RecurringBilling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Forte.ForteTransaction;

namespace HS.Facade
{
    public class ReceivePaymentFacade : BaseFacade
    {
        public ReceivePaymentFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        GlobalSettingDataAccess _GlobalSettingDataAccess
        {
            get
            {
                return (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }
        PaymentInfoCustomerDataAccess _PaymentInfoCustomerDataAccess
        {
            get
            {
                return (PaymentInfoCustomerDataAccess)_ClientContext[typeof(PaymentInfoCustomerDataAccess)];
            }
        }
        PaymentInfoDataAccess _PaymentInfoDataAccess
        {
            get
            {
                return (PaymentInfoDataAccess)_ClientContext[typeof(PaymentInfoDataAccess)];
            }
        }
        /// <summary>
        /// CompanyId CustomerGId PaymentMethod required
        /// </summary>
        public ReceivePaymentResponse ReceivePayment(ReceivePaymentModel Model)
        {

            ReceivePaymentResponse sendResponse = new ReceivePaymentResponse();

            #region Data Check
            if (Model.CompanyId == new Guid())
            {
                sendResponse.TransactionSuccess = false;
                sendResponse.Message = "Company id not found";
            }
            if (Model.CustomerGId == new Guid())
            {
                sendResponse.TransactionSuccess = false;
                sendResponse.Message = "Customer id not found";
            }
            if (string.IsNullOrWhiteSpace(Model.PaymentMethod))
            {
                sendResponse.TransactionSuccess = false;
                sendResponse.Message = "PaymentMethod  not found";
            }
            if (Model.ACHInfo == null && Model.CardInfo == null)
            {
                sendResponse.TransactionSuccess = false;
                sendResponse.Message = "ACH/CC info not found";
            }
            #endregion
            CustomerFacade cusFacade = new CustomerFacade();
            Customer cust = cusFacade.GetCustomerById(Model.CustomerId);
            string first_Name = cust.FirstName;
            string last_Name = cust.LastName;

            cust.FirstName = cust.FirstName.Contains("\"") || cust.FirstName.Contains("'") || cust.FirstName.Contains("(") || cust.FirstName.Contains(")")
                ? cust.FirstName.Replace("\"", "").Replace("'", "").Replace("(", "").Replace(")", "")
                : cust.FirstName;

            cust.LastName = cust.LastName.Contains("\"") || cust.LastName.Contains("'") || cust.LastName.Contains("(") || cust.LastName.Contains(")")
                ? cust.LastName.Replace("\"", "").Replace("'", "").Replace("(", "").Replace(")", "")
                : cust.LastName;
            GlobalSettingsFacade globsetfacade = new GlobalSettingsFacade();
            string TransactionKey = globsetfacade.GetAuthTransactionKeyByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));
            string APILoginId = globsetfacade.GetAuthAPILoginIdByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));
            string ForteTransactionKey = globsetfacade.GetForteTransactionKeyByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));
            string ForteAPILoginId = globsetfacade.GetForteAPILoginIdByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));
            string ForteOrganizationId = globsetfacade.GetForteOrganizationIdByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));
            string ForteLocationId = globsetfacade.GetForteLocationIdByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));
            string ForteAuthAccountId = globsetfacade.GetForteAuthAccountIdByCompanyId(Model.CompanyId, (Model.PaymentMethod.ToLower() == "ach"));

            GlobalSetting paymentGetway = globsetfacade.GetGlobalSettingsByKey(Model.CompanyId, "PaymentGetway");
            if (paymentGetway.Value == "Authorize.Net")
            {
                #region Authorize.net
                GlobalSetting globset = globsetfacade.GetGlobalSettingsByKey(Model.CompanyId, "Authorize.NetInProduction");
                if (globset == null)
                {
                    globset = new GlobalSetting();
                }

                if (Model.PaymentMethod.ToLower() == "ach")
                {
                    #region ACH Payment
                    createTransactionResponse response = DebitBankAccount.Run(APILoginId, TransactionKey, Model.ACHInfo, globset.Value.ToLower() == "true");

                    if (response != null)
                    {
                        if (response.messages.resultCode == messageTypeEnum.Ok)
                        {
                            if (response.transactionResponse.messages != null)
                            {
                                sendResponse.TransactionSuccess = true;
                                sendResponse.TransactionId = response.transactionResponse.transId;
                            }
                            else
                            {
                                if (response.transactionResponse.errors != null)
                                {
                                    sendResponse.TransactionSuccess = false;
                                    sendResponse.TransactionId = "";
                                    sendResponse.Message = response.transactionResponse.errors[0].errorText;

                                }
                                else
                                {
                                    sendResponse.TransactionSuccess = false;
                                    sendResponse.TransactionId = "";
                                    sendResponse.Message = "Transaction failed.";
                                }
                            }
                        }
                        else
                        {
                            if (response.transactionResponse != null && response.transactionResponse.errors != null)
                            {
                                sendResponse.TransactionSuccess = false;
                                sendResponse.TransactionId = "";
                                sendResponse.Message = response.transactionResponse.errors[0].errorText;

                            }
                            else
                            {
                                sendResponse.TransactionSuccess = false;
                                sendResponse.TransactionId = "";
                                sendResponse.Message = response.messages.message[0].text;
                                //return Json(new { result = false, transactionSuccess = TransactionSuccess, message = response.messages.message[0].text });
                            }
                        }
                    }
                    else
                    {
                        sendResponse.TransactionSuccess = false;
                        sendResponse.TransactionId = "";
                        sendResponse.Message = "Transaction failed.";
                        //return Json(new { result = false, transactionSuccess = TransactionSuccess, message = "Transaction failed." });
                    }
                    #endregion ACH
                }
                else if (Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard" || Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                {
                    #region CC Payment Authorize.net Part

                    String post_url = "https://test.authorize.net/gateway/transact.dll";
                    if (globset != null && globset.Value.ToLower() == "true")
                    {
                        post_url = "https://secure.authorize.net/gateway/transact.dll";
                    }

                    Dictionary<string, string> post_values = new Dictionary<string, string>();

                    post_values.Add("x_login", APILoginId);
                    post_values.Add("x_tran_key", TransactionKey);


                    post_values.Add("x_delim_data", "TRUE");
                    post_values.Add("x_delim_char", "|");
                    post_values.Add("x_relay_response", "FALSE");

                    post_values.Add("x_type", "AUTH_CAPTURE");
                    post_values.Add("x_method", "CC");

                    //Credit Card Number
                    post_values.Add("x_card_num", Model.CardInfo.CardNumber);

                    //ccv
                    post_values.Add("x_card_code", Model.CardInfo.SecurityCode);

                    //Expiration Date Card Number
                    post_values.Add("x_exp_date", Model.CardInfo.ExpiredDate);
                    //Order Amount
                    post_values.Add("x_amount", Math.Round(Model.CardInfo.Amount, 2).ToString());
                    post_values.Add("x_description", Model.CardInfo.Description);

                    post_values.Add("x_invoice_num", Model.CardInfo.InvoiceNo);

                    post_values.Add("x_first_name", Model.CardInfo.FirstName);
                    post_values.Add("x_last_name", Model.CardInfo.Lastname);
                    post_values.Add("x_cust_id", Model.CardInfo.CustomerId.ToString());
                    post_values.Add("x_email", Model.CardInfo.EmailAddress);
                    //x_invoice_num  //The merchant-assigned invoice number for the transaction.

                    if (!string.IsNullOrWhiteSpace(Model.CardInfo.Company))
                    {
                        post_values.Add("x_company", Model.CardInfo.Company);
                    }
                    if (!string.IsNullOrWhiteSpace(Model.CardInfo.Phone))
                    {
                        post_values.Add("x_phone", Model.CardInfo.Phone);
                    }
                    //if (!string.IsNullOrWhiteSpace(Model.CardInfo.BillingAddress))
                    //{
                    //    post_values.Add("x_address", Model.CardInfo.BillingAddress);
                    //}
                    //if (!string.IsNullOrWhiteSpace(Model.CardInfo.City))
                    //{
                    //    post_values.Add("x_city", Model.CardInfo.City);
                    //}
                    //if (!string.IsNullOrWhiteSpace(Model.CardInfo.State))
                    //{
                    //    post_values.Add("x_state", Model.CardInfo.State);
                    //}
                    //if (!string.IsNullOrWhiteSpace(Model.CardInfo.Zipcode))
                    //{
                    //    post_values.Add("x_zip", Model.CardInfo.Zipcode);
                    //} 

                    //x_email //customers valid email address
                    //post_values.Add("x_address", "1234 Street");
                    //post_values.Add("x_state", "WA");
                    //post_values.Add("x_zip", "98004");
                    //x_cust_id The merchant assigned customer ID


                    // This section takes the input fields and converts them to the proper format
                    // for an http post.  For example: "x_login=username&x_tran_key=a1B2c3D4"
                    String post_string = "";

                    foreach (KeyValuePair<string, string> post_value in post_values)
                    {
                        post_string += post_value.Key + "=" +
                        HttpUtility.UrlEncode(post_value.Value) + "&";
                    }
                    post_string = post_string.TrimEnd('&');
                    const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                    const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                    ServicePointManager.SecurityProtocol = Tls12;

                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(post_url);
                    objRequest.Method = "POST";
                    objRequest.ContentLength = post_string.Length;
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                     
                    //ServicePointManager.Expect100Continue = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    StreamWriter myWriter = null;
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(post_string);
                    myWriter.Close();
                    String post_response;
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                    {
                        post_response = responseStream.ReadToEnd();
                        responseStream.Close();
                    }
                    string[] response_array = post_response.Split('|');

                    if (response_array[0] == "2" || response_array[0] == "3")
                    {
                        sendResponse.TransactionSuccess = false;
                        sendResponse.TransactionId = "";
                        sendResponse.Message = response_array[3];
                        //return Json(new { result = false, transactionSuccess = TransactionSuccess, message = response_array[3] });
                    }
                    else
                    {
                        sendResponse.TransactionId = response_array[6];
                        sendResponse.TransactionSuccess = true;
                        sendResponse.Message = "Payment received successfully.";
                    }
                    #endregion
                }
                else if(Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "customerprofile")
                {
                    #region CustomerProfile Payment

                    if(string.IsNullOrWhiteSpace(Model.CustomerProfileId) 
                        || string.IsNullOrWhiteSpace(Model.CustomerPaymentProfileId))
                    {
                        if (string.IsNullOrWhiteSpace(Model.AuthorizeSubscriptionId))
                        {
                            sendResponse.TransactionId = "";
                            sendResponse.TransactionSuccess = false;
                            sendResponse.Message = "Authorize.net subscriptionId required.";
                            return sendResponse;
                        }

                        ARBGetSubscriptionResponse subscriptionresponse = GetSubscription.Run(APILoginId,TransactionKey,Model.AuthorizeSubscriptionId, globset.Value.ToLower() == "true");

                        if (subscriptionresponse != null && subscriptionresponse.messages.resultCode == messageTypeEnum.Ok)
                        {
                            if (subscriptionresponse.subscription != null)
                            {
                                //Console.WriteLine("Subscription returned : " + subscriptionresponse.subscription.name);
                                Model.CustomerPaymentProfileId = subscriptionresponse.subscription.profile.paymentProfile.customerPaymentProfileId;
                                Model.CustomerProfileId = subscriptionresponse.subscription.profile.customerProfileId;

                            }
                        }
                        else
                        {
                            sendResponse.TransactionId = "";
                            sendResponse.TransactionSuccess = false;
                            sendResponse.Message = "Authorize.net subscriptionId not found. Try another payment method.";
                            return sendResponse;
                        }
                    }

                    createTransactionResponse response = ChargeCustomerProfile.Run(APILoginId, TransactionKey, Model.CustomerProfileId, Model.CustomerPaymentProfileId, (decimal)Model.AmoutReceived, globset.Value.ToLower() == "true", Model.InvoiceList,Model.Description);
                    if (response != null)
                    {
                        if (response.messages.resultCode == messageTypeEnum.Ok)
                        {
                            if (response.transactionResponse.messages != null)
                            {
                                sendResponse.TransactionSuccess = true;
                                sendResponse.Message = "Transaction successful.";
                                sendResponse.TransactionId = response.transactionResponse.transId;
                            }
                            else
                            {
                                if (response.transactionResponse.errors != null)
                                {
                                    sendResponse.TransactionSuccess = false;
                                    sendResponse.TransactionId = "";
                                    sendResponse.Message = response.transactionResponse.errors[0].errorText;

                                }
                                else
                                {
                                    sendResponse.TransactionSuccess = false;
                                    sendResponse.TransactionId = "";
                                    sendResponse.Message = "Transaction failed.";
                                }
                            }
                        }
                        else
                        {
                            if (response.transactionResponse != null && response.transactionResponse.errors != null)
                            {
                                sendResponse.TransactionSuccess = false;
                                sendResponse.TransactionId = "";
                                sendResponse.Message = response.transactionResponse.errors[0].errorText;

                            }
                            else
                            {
                                sendResponse.TransactionSuccess = false;
                                sendResponse.TransactionId = "";
                                sendResponse.Message = response.messages.message[0].text;
                                //return Json(new { result = false, transactionSuccess = TransactionSuccess, message = response.messages.message[0].text });
                            }
                        }
                    }
                    else
                    {
                        sendResponse.TransactionSuccess = false;
                        sendResponse.TransactionId = "";
                        sendResponse.Message = "Transaction failed.";
                        //return Json(new { result = false, transactionSuccess = TransactionSuccess, message = "Transaction failed." });
                    }

                    #endregion
                }
                return sendResponse;
                #endregion
            }
            else
            {
                #region Forte
                GlobalSetting globset = globsetfacade.GetGlobalSettingsByKey(Model.CompanyId, "ForteInProduction");
                if (globset == null)
                {
                    globset = new GlobalSetting();
                }

                if (Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard" 
                    || Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard"
                    || Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach")
                {
                    #region Credit Card
                    ForteOptions forte = new ForteOptions();
                    forte.Organization_ID = ForteOrganizationId;
                    forte.Location_Id = ForteLocationId;
                    forte.AuthAccountId = ForteAuthAccountId;

                    forte.UserId = ForteAPILoginId;
                    forte.Password = ForteTransactionKey;

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3
                                       | System.Net.SecurityProtocolType.Tls
                                       | System.Net.SecurityProtocolType.Tls11
                                       | System.Net.SecurityProtocolType.Tls12;
             
                    if (globset != null && globset.Value.ToLower() == "true")
                    {
                        forte.Server = "https://api.forte.net/v3";
                    }
                    else
                    {
                        forte.Server = "https://sandbox.forte.net/api/v3";
                    }


                    #region Customer Token Generate On Forte
       
                    ForteCustomerCreate customercreate = new ForteCustomerCreate();
                    if (string.IsNullOrEmpty(cust.CustomerToken) || cust.CustomerToken == "")
                    {

                        ForteCustomerService forteCustomer = new ForteCustomerService(forte);
                        ForteCustomer forteCust = new ForteCustomer();
                        //Possible Action: sale,disburse,authorize,verify,inquiry
                        forteCust.first_name = cust.FirstName;
                        forteCust.last_name = cust.LastName;
                        forteCust.company_name = Model.CompanyName;
                        forteCust.customer_id = cust.Id.ToString();
                        forteCust.location_id = ForteLocationId;


                        #region Create  
                        customercreate = cusFacade.GetCustomerTokenForForte(forteCust, forteCustomer);


                        #endregion

                        if (!string.IsNullOrEmpty(customercreate.CustomerToken))
                        {
                            cust.CustomerToken = customercreate.CustomerToken;
                            cust.FirstName = first_Name;
                            cust.LastName = last_Name;
                            cusFacade.UpdateCustomer(cust);

                            cust.FirstName = cust.FirstName.Contains("\"") || cust.FirstName.Contains("'") || cust.FirstName.Contains("(") || cust.FirstName.Contains(")")
                                ? cust.FirstName.Replace("\"", "").Replace("'", "").Replace("(", "").Replace(")", "")
                                : cust.FirstName;

                            cust.LastName = cust.LastName.Contains("\"") || cust.LastName.Contains("'") || cust.LastName.Contains("(") || cust.LastName.Contains(")")
                                ? cust.LastName.Replace("\"", "").Replace("'", "").Replace("(", "").Replace(")", "")
                                : cust.LastName;

                        }
                        else
                        {
                            sendResponse.Message = customercreate.Massege;
                            sendResponse.TransactionSuccess = false;
                            return sendResponse;
                        }
          
                    }
                    else
                    {
                        customercreate.CustomerToken = cust.CustomerToken;
                    }
                    forte.Customer_Token = customercreate.CustomerToken;


                    #endregion

          

                    FortePaymethod fortepay = new FortePaymethod();
             
                    ForteTransaction forteTrns = new ForteTransaction();
                    //Possible Action: sale,disburse,authorize,verify,inquiry
                    forteTrns.action = "sale";
                    
                    if (Model.PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard")
                    {
                        forteTrns.authorization_amount = Model.CardInfo.Amount.ToString();
                        forteTrns.billing_addressList = new List<billing_addressClass>
                        {
                            new billing_addressClass
                            {
                                first_name = Model.CardInfo.FirstName,
                                last_name = Model.CardInfo.Lastname
                            },
                        };
                        forteTrns.billing_address = JsonConvert.SerializeObject(forteTrns.billing_addressList);
                        string[] ExpDate = new string[10];
                        if (Model.CardInfo.ExpiredDate.Contains('/'))
                        {
                            ExpDate = Model.CardInfo.ExpiredDate.Split('/');
                        }
                        else
                        {
                            string expMonth = Model.CardInfo.ExpiredDate.Substring(0, 2);
                            string expYear = Model.CardInfo.ExpiredDate.Substring(2, 2);

                            ExpDate[0] = expMonth;
                            ExpDate[1] = expYear;
                        }
                        var CardType = "";
                        if(Model.CardInfo.CardType.ToLower() == "mastercard" || Model.CardInfo.CardType == "mast")
                        {
                            CardType = "mast";
                        }
                        else if(Model.CardInfo.CardType.ToLower() == "amex" || Model.CardInfo.CardType == "amex")
                        {
                            CardType = "amex"; 
                        }
                        else if (Model.CardInfo.CardType.ToLower() == "discover" || Model.CardInfo.CardType == "disc")
                        {
                            CardType = "disc";
                        }
                        else
                        {
                            CardType = "visa";
                        }
                        if(!string.IsNullOrEmpty(Model.CardInfo.SecurityCode))
                        {
                            forteTrns.CardList = new List<CardClass>
                            {
                                new CardClass
                                {
                                    name_on_card = Model.CardInfo.FirstName+ " "+ Model.CardInfo.Lastname,
                                    card_type = CardType,
                                    account_number = Model.CardInfo.CardNumber,
                                    expire_month = ExpDate[0].ToString(),
                                    expire_year = ExpDate[1].ToString(),
                                    card_verification_value = Model.CardInfo.SecurityCode
                                },
                            };

                            fortepay.CardList = new List<ForteCard>
                            {
                                new ForteCard
                                {
                                    name_on_card = Model.CardInfo.NameOnCard,
                                    card_type = CardType,
                                    account_number = Model.CardInfo.CardNumber.Replace("-",""),
                                    expire_month = ExpDate[0].ToString(),
                                    expire_year = "20"+ExpDate[1].ToString(),
                                    card_verification_value = Model.CardInfo.SecurityCode
                                },
                            };

                        }
                        else
                        {
                            forteTrns.CardList = new List<CardClass>
                            {
                                new CardClass
                                {
                                    card_type = CardType,
                                    account_number = Model.CardInfo.CardNumber.Replace("-",""),
                                    expire_month = ExpDate[0].ToString(),
                                    expire_year = ExpDate[1].ToString(),
                                    //card_verification_value = Model.CardInfo.SecurityCode
                                },
                            };

                            fortepay.CardList = new List<ForteCard>
                            {
                                new ForteCard
                                {
                                    name_on_card = Model.CardInfo.FirstName+ " "+ Model.CardInfo.Lastname,
                                    card_type = CardType,
                                    account_number = Model.CardInfo.CardNumber.Replace("-",""),
                                    expire_month = ExpDate[0].ToString(),
                                    expire_year = "20"+ExpDate[1].ToString(),
                                    //card_verification_value = Model.CardInfo.SecurityCode
                                },
                            };

                        }
                        
                        forteTrns.card = JsonConvert.SerializeObject(forteTrns.CardList);
                        fortepay.card = JsonConvert.SerializeObject(fortepay.CardList);
                    }
                    else if ((Model.PaymentMethod == "ACH"))
                    {
                        forteTrns.authorization_amount = Model.ACHInfo.Amount.ToString();
                        forteTrns.billing_addressList = new List<billing_addressClass>
                        {
                            new billing_addressClass
                            {
                                first_name = Model.ACHInfo.FirstName,
                                last_name = Model.ACHInfo.Lastname
                            },
                        };
                        forteTrns.billing_address = JsonConvert.SerializeObject(forteTrns.billing_addressList);
                        forteTrns.AchCardList = new List<ForteAchTrans>
                                {
                                    new ForteAchTrans
                                    {
                                        account_holder = Model.ACHInfo.AccountName,
                                        routing_number = Model.ACHInfo.RoutingNo,
                                        account_number = Model.ACHInfo.AccountNo,
                                        account_type = Model.ACHInfo.AccountType,
                                        sec_code = Model.ACHInfo.ECheckType

                                    },
                                };

                        forteTrns.echeck = JsonConvert.SerializeObject(forteTrns.AchCardList);
                        fortepay.echeck = JsonConvert.SerializeObject(forteTrns.AchCardList);
                    }

                    #region Payment Token Generate on Forte
                    FortePaymethodsService payService = new FortePaymethodsService(forte);
                    FortePaymentCreate payresponse = new FortePaymentCreate();
                    if (string.IsNullOrEmpty(cust.PaymentToken) || cust.PaymentToken == "")
                    {
                        payresponse = payService.Create(fortepay);
                        if (payresponse.Massege.Contains("Successfully"))
                        {
                            var finalResponse = payresponse.Massege.Split('|');
                            int index = finalResponse.Length - 1;
                            FortePaymentResponse forteResponse = new FortePaymentResponse();
                            forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                            payresponse.PaymentToken = forteResponse.paymethod_token;
                        }
                        else
                        {
                            var finalResponse = payresponse.Massege.Split("#ERROR#");
                            int index = finalResponse.Length - 1;
                            ForteErrorResoponse forteResponse = new ForteErrorResoponse();
                            forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteErrorResoponse>(finalResponse[1]);
                            payresponse.ErrorMessage = forteResponse.response.response_desc;
                        

                        }

                        if (!string.IsNullOrEmpty(payresponse.PaymentToken))
                        {
                            cust.PaymentToken = customercreate.PaymentToken;
                            cust.FirstName = first_Name;
                            cust.LastName = last_Name;
                            cusFacade.UpdateCustomer(cust);

                            cust.FirstName = cust.FirstName.Contains("\"") || cust.FirstName.Contains("'") || cust.FirstName.Contains("(") || cust.FirstName.Contains(")")
                                ? cust.FirstName.Replace("\"", "").Replace("'", "").Replace("(", "").Replace(")", "")
                                : cust.FirstName;

                            cust.LastName = cust.LastName.Contains("\"") || cust.LastName.Contains("'") || cust.LastName.Contains("(") || cust.LastName.Contains(")")
                                ? cust.LastName.Replace("\"", "").Replace("'", "").Replace("(", "").Replace(")", "")
                                : cust.LastName;

                        }
                        else
                        {
                            sendResponse.Message = payresponse.ErrorMessage;
                            sendResponse.TransactionSuccess = false;
                            return sendResponse;
                        }

                    }
                    else
                    {
                        payresponse.PaymentToken = cust.PaymentToken;
                    }
                    //forte.Paymethod_Token = customercreate.PaymentToken;
                    #endregion




                    ForteTransactionService forteTransaction = new ForteTransactionService(forte);

                    FortePaymentCreate response = forteTransaction.Create(forteTrns);
                   // string message = "{'transaction_id':'trn_dc8d28cf-ca1e-43bf-b425-ca79cea7cbec','location_id':'loc_224960','action':'sale','authorization_amount':136.40,'entered_by':'80857e62848e41fb5dfec50b35956859','billing_address':{'first_name':'reza'},'card':{'last_4_account_number':'4242','masked_account_number':'****4242','expire_month':12,'expire_year':21,'card_type':'visa'},'response':{'environment':'sandbox','response_type':'E','response_code':'F01','response_desc':'MANDITORY FIELD MISSING: billing_address.last_name'}}";
                  
                    if (response.Massege.Contains("Successfully"))
                        {
                            var finalResponse = response.Massege.Split('|');
                            int index = finalResponse.Length - 1;
                            FortePaymentResponse forteResponse = new FortePaymentResponse();
                            forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                            sendResponse.TransactionId = forteResponse.transaction_id;
                            sendResponse.TransactionSuccess = true;
                            sendResponse.Message = "Payment received successfully.";
                        }
                        else
                        {

                            var finalResponse = response.Massege.Split('#');
                            int index = finalResponse.Length - 1;
                            FortePaymentResponse forteResponse = new FortePaymentResponse();
                            forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                            sendResponse.TransactionSuccess = false;
                            sendResponse.Message = forteResponse.response.response_desc;       
                      
                        }
                        return sendResponse;
                    #endregion 
                }
                else
                {
                    sendResponse.Message = "For now, Fortey payments only supports credit card/ debit card payments.";
                    sendResponse.TransactionSuccess = false;
                    return sendResponse;
                }
                #endregion

            }
        }

        /// <summary>
        /// CompanyId CustomerGId One PaymentMethod (ACH/CC) required
        /// </summary>
        public PaymentProfileCustomer GetPaymentProfileCustomerByPaymentInfo(ReceivePaymentModel model)
        {
            PaymentProfileCustomer paymentProfileCustomer = new PaymentProfileCustomer();

            List<PaymentProfileCustomer> PPCList = _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", model.CustomerGId));
            List<PaymentInfoCustomer> PICList = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", model.CustomerGId));

            List<int> PaymentInfoIdList = new List<int>();

            #region Listing all PaymentInfo IDs
            if (PPCList != null && PPCList.Count > 0)
            {
                PaymentInfoIdList.AddRange(PPCList.Select(x => x.PaymentInfoId));
            }

            if (PICList != null && PICList.Count > 0)
            {
                PaymentInfoIdList.AddRange(PICList.Select(x => x.PaymentInfoId));
            }
            #endregion

            string paymentInfoSQl = string.Format(" Id in ({0})", string.Join(",", PaymentInfoIdList));
            List<PaymentInfo> PaymentInfoList = _PaymentInfoDataAccess.GetByQuery(paymentInfoSQl);
            PaymentInfo PInfo = null;
            string methodtype = "";
            if (PaymentInfoList != null)
            {
                if (model.CardInfo != null && model.CardInfo.CardNumber.Length > 4)
                {
                    string CardNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.CardInfo.CardNumber);
                    PInfo = PaymentInfoList.Where(x => x.CardNumber == CardNumber).FirstOrDefault();
                    if (PInfo == null)
                    {
                        PInfo = new PaymentInfo()
                        {
                            CompanyId = model.CompanyId,
                            CardNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.CardInfo.CardNumber),
                            CardExpireDate = model.CardInfo.ExpiredDate,
                            AccountName = model.CardInfo.NameOnCard,
                            CardType = model.CardInfo.CardType,
                            CardSecurityCode = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.CardInfo.SecurityCode),

                        };
                        PInfo.Id = (int)_PaymentInfoDataAccess.Insert(PInfo);
                        PInfo.CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(PInfo.CardNumber);
                        PInfo.CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(PInfo.CardSecurityCode);

                        methodtype = "CC" + "_" + model.CardInfo.NameOnCard + "_" + model.CardInfo.CardNumber.Substring(Math.Max(0, model.CardInfo.CardNumber.Length - 4));
                    }
                }
                else if (model.ACHInfo != null && model.ACHInfo.AccountNo.Length > 2)
                {
                    //string accountNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.ACHInfo.AccountNo);
                    //string routingNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.ACHInfo.RoutingNo);
                    PInfo = PaymentInfoList.Where(x => x.AcountNo == model.ACHInfo.AccountNo && x.RoutingNo == model.ACHInfo.RoutingNo).FirstOrDefault();
                    if (PInfo == null)
                    {
                        PInfo = new PaymentInfo()
                        {
                            CompanyId = model.CompanyId,
                            AccountName = model.ACHInfo.AccountName,
                            BankAccountType = model.ACHInfo.AccountType,
                            RoutingNo = model.ACHInfo.RoutingNo,
                            EcheckType = model.ACHInfo.ECheckType,
                            BankName = model.ACHInfo.BankName,
                            AcountNo = model.ACHInfo.AccountNo
                        };
                        PInfo.Id = (int)_PaymentInfoDataAccess.Insert(PInfo);

                        methodtype = "ACH" + "_" + model.ACHInfo.AccountName + "_" + model.ACHInfo.AccountNo.Substring(Math.Max(0, model.ACHInfo.AccountNo.Length - 2));
                    }
                }
                else
                {
                    return null; //will return null because this function will only works for ACH & CC
                }

                if (PInfo != null)
                {
                    paymentProfileCustomer = PPCList.Where(x => x.PaymentInfoId == PInfo.Id).FirstOrDefault();
                    if (paymentProfileCustomer == null)
                    {
                        paymentProfileCustomer = new PaymentProfileCustomer()
                        {
                            CompanyId = model.CompanyId,
                            PaymentInfoId = PInfo.Id,
                            CustomerId = model.CustomerGId,
                            IsDefault = false,
                            Type = methodtype,
                        };
                        paymentProfileCustomer.Id = (int)_PaymentProfileCustomerDataAccess.Insert(paymentProfileCustomer);
                    }
                }
            }
            return paymentProfileCustomer;
        }
    }
}


     
      
        

    

