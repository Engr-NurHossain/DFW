using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class CustomerThirdPartyAgencyDataAccess
	{
        public CustomerThirdPartyAgencyDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllCustomerThirdPartyAgencyByCustomerId(Guid CustomerId, string PlatformId)
        {
            string sqlQuery = @"select ca.*,lkagency.DisplayText as AgencytypeVal,lkperm.DisplayText as PermTypeVal  from CustomerThirdPartyAgency ca
                                left join Lookup lkagency on lkagency.DataValue = ca.Agencytype and lkagency.DataKey = 'BrinksAgencyType'
                                left join Lookup lkperm on lkperm.DataValue = ca.PermType and lkperm.DataKey = 'BrinksPermTypes'
                                where ca.CustomerId='{0}' and ca.Platform ={1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, PlatformId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
