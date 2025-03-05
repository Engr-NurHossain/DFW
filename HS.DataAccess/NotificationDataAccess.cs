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
	public partial class NotificationDataAccess
	{
        public NotificationDataAccess (string ConnectionStr) : base(ConnectionStr) { }

        public DataSet GetUserNotificationsByUserId(Guid userId,int Limit)
        {
            string LimitQuery = "";
            string IsReadSql = "";
            if (Limit > 0)
            {
                LimitQuery = string.Format("top({0})",Limit);
                //IsReadSql = "and nu.IsRead = 0"; //no need anymore, need to show top 5 always
            }

            string sqlQuery = @"DECLARE @UserId uniqueidentifier
                                set @UserId = '{0}'
 
                                select {1} notf.*
                                ,CASE notf.[Type] 
	                                WHEN 'Employee' THEN emp.FirstName+' '+emp.LastName 
	                                WHEN 'Customer' 
	                                THEN 
		                                CASE cus.[Type]
			                                WHEN 'Commercial'
				                                THEN cus.BusinessName
			                                ELSE cus.FirstName+ ' '+ cus.LastName
		                                END
	                                ELSE '' 
                                END as WhoVal  
                                ,nu.IsRead as IsRead

                                into #NotificationData
                                from [Notification] notf
                                left join notificationuser nu 
                                    on notf.NotificationId = nu.NotificationId
                                left join employee emp 
                                    on notf.who = emp.UserId
                                left join Customer cus
                                    on cus.CustomerId = notf.Who

                                where nu.NotificationPerson = @UserId
                                --{2}
                                order by nu.IsRead  asc, CreatedDate desc
                                 
                                select * from #NotificationData
                                    order by CreatedDate DESC
                                /*Update IsRead when shows*/
                                update NotificationUser set IsRead = 1 
                                where NotificationPerson = @UserId 
                                and NotificationId in (select NotificationId from #NotificationData)
                                drop table #NotificationData
                                
                                /*Un read notification count*/
                                select count(id) as TotalCount from NotificationUser 
                                where NotificationPerson = @UserId and IsRead = 0 
                                ";
             
            try
            {
                sqlQuery = string.Format(sqlQuery, userId, LimitQuery,IsReadSql);
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
        public DataSet GetUserNotificationsReadByUserId(Guid userId, int Limit)
        {
            string LimitQuery = "";
            string IsReadSql = "";
            if (Limit > 0)
            {
                LimitQuery = string.Format("top({0})", Limit);
                //IsReadSql = "and nu.IsRead = 0"; //no need anymore, need to show top 5 always
            }

            string sqlQuery = @"DECLARE @UserId uniqueidentifier
                                set @UserId = '{0}'
 
                                select {1} notf.*
                                ,CASE notf.[Type] 
	                                WHEN 'Employee' THEN emp.FirstName+' '+emp.LastName 
	                                WHEN 'Customer' 
	                                THEN 
		                                CASE cus.[Type]
			                                WHEN 'Commercial'
				                                THEN cus.BusinessName
			                                ELSE cus.FirstName+ ' '+ cus.LastName
		                                END
	                                ELSE '' 
                                END as WhoVal  
                                ,nu.IsRead as IsRead
                                ,(select ka.IsRead  from KnowledgebaseAccountability ka
								 where ka.AssignedUser = @UserId
								 and ka.KnowledgebaseId = iif(notf.[Type] = 'Accountability',REPLACE(notf.NotificationUrl, '/knowledgebase/Id=', ''),'0')) as [KnowledgebaseIsRead]
                                into #NotificationData
                                from [Notification] notf
                                left join notificationuser nu 
                                    on notf.NotificationId = nu.NotificationId
                                left join employee emp 
                                    on notf.who = emp.UserId
                                left join Customer cus
                                    on cus.CustomerId = notf.Who

                                where nu.NotificationPerson = @UserId
                                --{2}
                                order by nu.IsRead  asc, CreatedDate desc
                                 
                                select * from #NotificationData
                                    order by CreatedDate DESC
                                /*Update IsRead when shows*/
                                update NotificationUser set IsRead = 1 
                                where NotificationPerson = @UserId 
                                and NotificationId in (select NotificationId from #NotificationData)
                                drop table #NotificationData
                                
                                /*Un read notification count*/
                                select count(id) as TotalCount from NotificationUser 
                                where NotificationPerson = @UserId and IsRead = 0 
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, userId, LimitQuery, IsReadSql);
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
    }	
}
