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
	public partial class ToppingCategoryDataAccess
	{
        public ToppingCategoryDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllToppingCategoryByMenuId(Guid MenuId, Guid itemid, Guid comid)
        {
            string sqlQuery = @"
                                select tc.Id as ToppingCategoryId,tc.ToppingCategory as CategoryName  from RestMenuItemCategoryTopping md 
                                left join RestToppingCategory tc
	                                on md.ToppingCategoryId = tc.ToppingCategoryId
                                where md.MenuId='{0}' and md.ItemId = '{1}' and tc.CompanyId = '{2}'
                                ";
            //string subquery = "";

            sqlQuery = string.Format(sqlQuery, MenuId, itemid, comid);
            try
            {
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
