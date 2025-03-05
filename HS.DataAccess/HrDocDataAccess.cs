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
	public partial class HrDocDataAccess
	{
        public DataTable GetUserLoginNameById(int id)
        {

            string sqlQuery = @"select ul.UserName
                                from UserLogin ul
                                where ul.Id='{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, id);
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
        public DataTable GetAllUserFilesByCompanyId(Guid companyid, string user)
        {

            string sqlQuery = @"select hd.*,emp.FirstName+' '+emp.LastName as CreatedByName,
                                lkCategory.DisplayText as DocCategory
                                from HrDoc hd
                                left join employee emp on emp.UserId = hd.Createdby
                                        left join [Lookup] lkCategory on hd.Category = lkCategory.DataValue 
								and lkCategory.DataKey ='DocCatagory'
                                where hd.CompanyId = '{0}' and hd.UserName = '{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, user);
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
        public DataTable GetAllUserFilesByCompanyIdAndFilter(Guid companyid, string user,string SearchText,string FilterText)
        {
            var SearchTextQuery = "";
            var FilterTextQuery = "";
            if(!string.IsNullOrEmpty(SearchText))
            {
                SearchTextQuery = string.Format("and hd.FileDescription like '%{0}%' or hd.Category like '%{0}%' and emp.FirstName+' '+emp.LastName like '%{0}%'", SearchText);
            }
            if(FilterText != null && FilterText != "-1")
            {
                FilterTextQuery = string.Format("and hd.Category = '{0}'", FilterText);
            }
            string sqlQuery = @"select hd.*,emp.FirstName+' '+emp.LastName as CreatedByName,
                                lkCategory.DisplayText as DocCategory
                                from HrDoc hd
                                left join employee emp on emp.UserId = hd.Createdby
                                        left join [Lookup] lkCategory on hd.Category = lkCategory.DataValue 
								and lkCategory.DataKey ='DocCatagory'
                                where hd.CompanyId = '{0}' and hd.UserName = '{1}' {2} {3}";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, user,SearchTextQuery,FilterTextQuery);
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
