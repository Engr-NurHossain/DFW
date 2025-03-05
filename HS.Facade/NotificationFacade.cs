using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class NotificationFacade:BaseFacade
    {
        public NotificationFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        public NotificationFacade() { }
        NotificationDataAccess _NotificationDataAccess
        {
            get
            {
                return (NotificationDataAccess)_ClientContext[typeof(NotificationDataAccess)];
            }
        }

        NotificationUserDataAccess _NotificationUserDataAccess
        {
            get
            {
                return (NotificationUserDataAccess)_ClientContext[typeof(NotificationUserDataAccess)];
            }
        }
        public int InsertNotification(Notification Notification)
        {
            return (int)_NotificationDataAccess.Insert(Notification);
        }
        public int InsertNotificationUser(NotificationUser NotificationUser)
        {
            return (int)_NotificationUserDataAccess.Insert(NotificationUser);
        }

        public bool MarkAllAsRead(Guid userId)
        {
            return _NotificationUserDataAccess.MarkAllAsRead(userId);
        }

        public NotificationViewModel GetUserNotificationsByUserId(Guid userId,int Limit=5)
        {
            //DataSet ds = _NotificationDataAccess.GetUserNotificationsByUserId(userId, Limit);
            DataSet ds = _NotificationDataAccess.GetUserNotificationsReadByUserId(userId, Limit);
            NotificationViewModel Model = new NotificationViewModel();
            Model.Notifications = (from DataRow dr in ds.Tables[0].Rows
                                               select new Notification()
                                               {
                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                   CompanyId = (Guid)dr["CompanyId"],
                                                   NotificationId = (Guid)dr["NotificationId"],
                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                   Type = dr["Type"].ToString(),
                                                   What = string.Format(dr["What"].ToString(), dr["WhoVal"]),
                                                   Who = (Guid)dr["Who"],
                                                   WhoVal = dr["WhoVal"].ToString(),
                                                   NotificationUrl = dr["NotificationUrl"].ToString(),
                                                   IsRead = dr["IsRead"] != DBNull.Value ? Convert.ToBoolean(dr["IsRead"]) : false,
                                                   KnowledgebaseIsRead = dr["KnowledgebaseIsRead"] != DBNull.Value ? Convert.ToBoolean(dr["KnowledgebaseIsRead"]) : false,
                                               }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            //return _TicketDataAccess.Get(TicketId); 
            return Model;
        }
        public bool DeleteAssignedArticleNotificationByNotificationUrl(string NotificationUrl)
        {
            return _NotificationUserDataAccess.DeleteAssignedArticleNotificationByNotificationUrl(NotificationUrl);
        }
        public CountNotification GetNotificationCountByUserId(Guid userId)
        {
            CountNotification count = new CountNotification();
            if (userId == Guid.Empty)
                return count;
            DataSet dts = _NotificationUserDataAccess.GetNotificationCountByUserId(userId);
            if(dts!=null && dts.Tables[0]!=null && dts.Tables[1] !=null)
            {
                DataTable dt = dts.Tables[0];
                DataTable dt2 = dts.Tables[1];
                count.NotificationCount = dt.Rows[0]["Count"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Count"]) : 0;
                count.AnnouncementCount = dt2.Rows[0]["CountAnnouncement"] != DBNull.Value ? Convert.ToInt32(dt2.Rows[0]["CountAnnouncement"]) : 0;
            }
            return count;
        }
    }
}
