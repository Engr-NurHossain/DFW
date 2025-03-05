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
	public partial class CompanyBranchDataAccess
	{
        public CompanyBranchDataAccess(string ConnectionStr):base(ConnectionStr) { }
        public CompanyBranchDataAccess() { }
        public DataTable GetAllCompanyBranchWithStateAndTimeZone(Guid CompanyId)
        {
            string sqlQuery = @"select CB.*
                    ,LUp.DisplayText as TimeZoneDisplay from 
                    CompanyBranch CB 
                    left join Lookup LUp 
	                    on LUp.DataValue = CB.TimeZone and LUp.DataKey = 'TimeZone'
                    where CB.CompanyId = '{0}' 
                    --and LUp.DataKey = 'TimeZone'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
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
        public DataTable GetCityTaxRate(Guid CustomerId,Guid CompanyId)
        {
             string sqlQuery = @"select CB.* from 
	                                Customer Cus 
	                                left join CustomerCompany CC
	                                on Cus.CustomerId = cc.CustomerId 
	                                left join CityTax CB 
	                                on CC.CompanyId = CB.CompanyId 
                                    where Cus.CustomerId = '{0}' 
                                    and CC.CompanyId = '{1}' 
                                    and Cus.City = cb.City  ";
            if(CustomerId == new Guid())
            {
                sqlQuery = @"select * from CityTax where CompanyId = '{1}'";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId);
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

        public bool RemoveAllMainBranchOfaCompanyExcludingId(int id, Guid companyId)
        {
            string sqlQuery = @"update CompanyBranch set IsMainBranch = 0
                                where CompanyId ='{1}'
                                and id != {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, id, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }	
}
