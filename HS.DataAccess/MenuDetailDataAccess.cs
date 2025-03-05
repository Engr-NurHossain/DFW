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
	public partial class MenuDetailDataAccess
	{
        public MenuDetailDataAccess(string ConStr) : base(ConStr) { }

        public bool DeleteByMenuId(int menuId)
        {

            string SqlQuery = @"delete from MenuDetail where MenuId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, menuId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DeleteByCategoryId(int categoryId)
        {

            string SqlQuery = @"delete from MenuDetail where ToppingCategoryId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, categoryId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }	
}
