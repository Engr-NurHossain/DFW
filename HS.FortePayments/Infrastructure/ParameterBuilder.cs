using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;

namespace Forte
{
	internal static class ParameterBuilder
	{
        private static string newUrl;
        public static string ApplyAllParameters(ForteOptions obj)
		{
            if (obj != null)
            {
                newUrl = "";
                foreach (var property in obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    foreach (var attribute in property.GetCustomAttributes(false))
                    {
                        if (attribute.GetType() != typeof(JsonPropertyAttribute)) continue;

                        var JsonPropertyAttribute = (JsonPropertyAttribute)attribute;

                        var value = property.GetValue(obj, null);
                        if (value == null) continue;
                        if (string.IsNullOrWhiteSpace(value.ToString())) continue;

                        switch ((JsonPropertyAttribute.PropertyName).ToLower())
                        {
                            case "account_id":
                                newUrl = newUrl + "/accounts/" + (value.ToString().StartsWith("act_") ? value.ToString() : "act_" + value.ToString());
                                break;
                            case "organization_id":
                                newUrl = newUrl + "/organizations/" + (value.ToString().StartsWith("org_") ? value.ToString() : "org_" + value.ToString());
                                break;
                            case "location_id":
                                newUrl = newUrl + "/locations/" + (value.ToString().StartsWith("loc_") ? value.ToString() : "loc_" + value.ToString());
                                break;
                            case "customer_token":
                                newUrl = newUrl + "/customers/" + (value.ToString().StartsWith("cst_") ? value.ToString() : "cst_" + value.ToString());
                                break;
                            case "address_id":
                                newUrl = newUrl + "/addresses/" + (value.ToString().StartsWith("add_") ? value.ToString() : "add_" + value.ToString());
                                break;
                            case "paymethod_token":
                                //newUrl = newUrl + "/paymethods/" + (value.ToString().StartsWith("pmt_") ? value.ToString() : "pmt_" + value.ToString());
                                newUrl = newUrl + "/paymethods/" + (value.ToString().StartsWith("") ? value.ToString() : "" + value.ToString());
                                break;
                            case "transaction_id":
                                newUrl = newUrl + "/transactions/" + (value.ToString().StartsWith("trn_") ? value.ToString() : "trn_" + value.ToString());
                                break;
                            case "schedule_id":
                                newUrl = newUrl + "/schedules/" + (value.ToString().StartsWith("sch_") ? value.ToString() : "sch_" + value.ToString());
                                break;
                            case "schedule_item_id":
                                newUrl = newUrl + "/scheduleitems/" + (value.ToString().StartsWith("sci_") ? value.ToString() : "sci_" + value.ToString());
                                break;
                            case "resource":
                                if (!newUrl.Contains(value.ToString()))
                                {
                                    newUrl = newUrl + "/" + value.ToString() + "/";
                                }

                                break;
                            default:
                                break;
                        }
                        
                    }
                }
                    return newUrl;
            }
            else
            {
                return "Error";
            }
           
		}
	}
}
