using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Framework;
using HS.DataAccess;
using HS.Entities;
using System.Data;

namespace HS.Facade
{
    public class ActivityFacade : BaseFacade
    {
        ActivityDataAccess _ActivityDataAccess = null;
        public ActivityFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_ActivityDataAccess == null)
                _ActivityDataAccess = (ActivityDataAccess)_ClientContext[typeof(ActivityDataAccess)];
        }
        public ActivityFacade(string ConStr)
        {
            if (_ActivityDataAccess == null)
                _ActivityDataAccess = new ActivityDataAccess(ConStr);
        }
        public int InsertActivity(Activity Activity)
        {
            return (int)_ActivityDataAccess.Insert(Activity);
        }

        public Activity GetActivityById(int value)
        {
            return _ActivityDataAccess.Get(value);
        }
        public List<Activity> GetAllActivitybyAssignTo(Guid ActivityId)
        {
            return _ActivityDataAccess.GetByQuery(string.Format(" AssignedTo ='{0}'", ActivityId)).ToList();
        }
        public bool UpdateActivity(Activity activity)
        {
            return _ActivityDataAccess.Update(activity) > 0;
        }
        public ActivityModel GetActivities(ActivityFilter filter)
        {
            return _ActivityDataAccess.GetActivities(filter);
        }
        public List<Activity> GetFilteredActivities(ActivityFilter filter)
        {
            return _ActivityDataAccess.GetFilteredActivities(filter);
        }
        public long DeleteActivity(int Id)
        {
            return _ActivityDataAccess.Delete(Id);
        }
        public List<Activity> GetAllActivityListByIds(string IdList)
        {
            DataTable dt = _ActivityDataAccess.GetAllActivityListByIds(IdList);
            List<Activity> ActivityList = new List<Activity>();
            ActivityList = (from DataRow dr in dt.Rows
                            select new Activity()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                ActivityTypeVal = dr["ActivityTypeVal"].ToString(),
                                AssociatedTypeVal = dr["AssociatedTypeVal"].ToString(),
                                AssociatedType = dr["AssociatedType"].ToString(),
                                Description = dr["Description"].ToString(),
                                Note = dr["Note"].ToString(),
                                Status = dr["Status"].ToString(),
                                AssociatedWithVal = dr["AssociatedWithVal"].ToString(),
                                AssignedToVal = dr["AssignedToVal"].ToString(),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                AssociatedOpportunityVal = dr["AssociatedOpportunityVal"].ToString(),
                                DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            }).ToList();


            return ActivityList;
        }
        public Activity GetActivityInfoById(int id)
        {
            DataTable dt = _ActivityDataAccess.GetActivityInfoByIds(id);
            List<Activity> ActivityList = new List<Activity>();
            ActivityList = (from DataRow dr in dt.Rows
                            select new Activity()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                AssignedTo = (Guid)dr["AssignedTo"],
                                ActivityTypeVal = dr["ActivityTypeVal"].ToString(),
                                AssociatedTypeVal = dr["AssociatedTypeVal"].ToString(),
                                AssociatedType = dr["AssociatedType"].ToString(),
                                Description = dr["Description"].ToString(),
                                Note = dr["Note"].ToString(),
                                Status = dr["Status"].ToString(),
                                AssociatedWithVal = dr["AssociatedWithVal"].ToString(),
                                AssignedToVal = dr["AssignedToVal"].ToString(),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                AssociatedOpportunityVal = dr["AssociatedOpportunityVal"].ToString(),
                                DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                OriginVal = dr["OriginVal"].ToString()
                            }).ToList();


            return ActivityList.FirstOrDefault();
        }
        public DataTable GetAllActivityForExport(ActivityFilter filter)
        {
            return _ActivityDataAccess.GetAllActivityForExport(filter);
        }

        public List<Activity> GetAllActivityListByAssociatWith(Guid customerId)
        {
            DataTable dt = _ActivityDataAccess.GetAllActivityListByAssociatWith(customerId);
            List<Activity> ActivityList = new List<Activity>();
            ActivityList = (from DataRow dr in dt.Rows
                            select new Activity()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                ActivityTypeVal = dr["ActivityTypeVal"].ToString(),
                                AssociatedTypeVal = dr["AssociatedTypeVal"].ToString(),
                                AssociatedType = dr["AssociatedType"].ToString(),
                                Description = dr["Description"].ToString(),
                                Note = dr["Note"].ToString(),
                                Status = dr["Status"].ToString(),
                                AssociatedWithVal = dr["AssociatedWithVal"].ToString(),
                                AssignedToVal = dr["AssignedToVal"].ToString(),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                AssociatedOpportunityVal = dr["AssociatedOpportunityVal"].ToString(),
                                DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            }).ToList(); 
            return ActivityList;
        }

        public ActivityListFilterModel GetAllActivitiesReport(int pageno, int pagesize, DateTime? start, DateTime? end, string type, string status, string origin, string soldBy)
        {
            DataSet ds = _ActivityDataAccess.GetAllActivitiesReport(pageno, pagesize, start, end, type, status, origin, soldBy);
            ActivityListFilterModel model = new ActivityListFilterModel();
            model.ListActivity = (from DataRow dr in ds.Tables[0].Rows
                                  select new Activity()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      ActivityTypeVal = dr["ActivityTypeVal"].ToString(),
                                      EmpName = dr["EmpName"].ToString(),
                                      Description = dr["Description"].ToString(),
                                      Note = dr["Note"].ToString(),
                                      Status = dr["Status"].ToString(),
                                      AssociatedCustomer = dr["AssociatedCustomer"].ToString(),
                                      AssociatedOpportunity = dr["AssociatedOpportunity"].ToString(),
                                      CreatedByVal = dr["CreatedByVal"].ToString(),
                                      AssociatedContact = dr["AssociatedContact"].ToString(),
                                      DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                      CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                      OriginVal = dr["OriginVal"].ToString(),
                                      DepartmentVal = dr["DepartmentVal"].ToString(),

                                  }).ToList();
            model.TotalActivityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                             select new TotalActivityCountModel()
                                             {
                                                 TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                             }).FirstOrDefault();
            return model;
        }

        public DataTable GetAllActivitiesReportExport(DateTime? start, DateTime? end, string type, string status, string actorigin, string soldBy)
        {
            return _ActivityDataAccess.GetAllActivitiesReportExport(start, end, type, status, actorigin, soldBy);
        }
    }
}
