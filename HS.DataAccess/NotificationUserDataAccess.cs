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
	public partial class NotificationUserDataAccess
	{
        public NotificationUserDataAccess(string ConnectionStr) : base(ConnectionStr) { }
        public DataSet GetNotificationCountByUserId(Guid userId)
        {
            string sqlQuery = @"select count(id) as Count from NotificationUser 
                                where NotificationPerson = '{0}' and IsRead = 0

                                select Count(Id) as CountAnnouncement from Announcement 
                                where GetDate() between StartTime and EndTime";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool DeleteAssignedArticleNotificationByNotificationUrl(string NotificationUrl)
        {
            string sqlQuery = @"delete from [Notification] where NotificationUrl ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, NotificationUrl);

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public bool MarkAllAsRead(Guid userId)
        {
            string sqlQuery = @"update NotificationUser set IsRead = 1 
                                where NotificationPerson ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }	
}
