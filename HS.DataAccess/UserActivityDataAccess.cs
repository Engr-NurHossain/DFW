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
    public partial class UserActivityDataAccess
    {
        public UserActivityDataAccess(string ConnStr) : base(ConnStr) { }
        public UserActivity GetUserActivityByLoginAction(string username)
        {
            string sqlQuery = @"select  TOP(1)* from UserActivity where Action = 'LogIn' 
                                and UserName = '{0}' 
                                and Id not in (select top(1) Id From UserActivity where Action = 'LogIn'  and UserName = '{0}' order by Id desc )
                                order by Id desc";
            
            try
            {
                sqlQuery = string.Format(sqlQuery, username);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            UserActivity UserActivity = new UserActivity();
                            FillObject(UserActivity, reader); 
                            return UserActivity;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
