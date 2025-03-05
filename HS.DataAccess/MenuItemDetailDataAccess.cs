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
	public partial class MenuItemDetailDataAccess
	{
        public MenuItemDetailDataAccess(string ConStr) : base(ConStr) { }

        public bool DeleteByMenuItemId(Guid menuItemId)
        {
            string SqlQuery = @"delete from MenuItemDetail where MenuItemId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, menuItemId);
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
