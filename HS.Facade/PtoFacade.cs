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
    public class PtoFacade:BaseFacade
    {
        public PtoFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        PtoDataAccess _PtoDataAccess
        {
            get
            {
                return (PtoDataAccess)_ClientContext[typeof(PtoDataAccess)];
            }
        }
        EmployeeOperationsDataAccess _EmployeeOperationsDataAccess
        {
            get
            {
                return (EmployeeOperationsDataAccess)_ClientContext[typeof(EmployeeOperationsDataAccess)];
            }
        }
        public int InsertPto(Pto pto)
        {
            return (int)_PtoDataAccess.Insert(pto);
        }

        public Pto GetPtoById(int? id)
        {
            return _PtoDataAccess.Get(id.Value);
        }
        public List<Pto> GetAllPto()
        {
            return _PtoDataAccess.GetAll();
            //return _PtoDataAccess.GetByQuery(string.Format("UserId='{0}'",userId));
        }

        public PtoFilterModel GetAllPtoByUserId(Guid userId, PayrollFilterModel filter, bool? getreport)
        {
            DataSet ds = _PtoDataAccess.GetAllPtoByUserId(userId, filter, getreport);

            PtoFilterModel model = new PtoFilterModel();
            model.ListPto = (from DataRow dr in ds.Tables[0].Rows
                         select new Pto()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = (Guid)dr["UserId"],
                             StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                             Type = dr["Type"].ToString(),
                             EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                             LeaveTime = dr["LeaveTime"].ToString(),
                             TimeFrom = dr["TimeFrom"].ToString(),
                             TimeTo = dr["TimeTo"].ToString(),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             Notes = dr["Notes"].ToString(),
                             Status = dr["Status"].ToString(),
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                             Payable = dr["Payable"] != DBNull.Value ? Convert.ToBoolean(dr["Payable"]) : false,
                             RejectNote = dr["RejectNote"].ToString(),
                             Minute = dr["Minute"] != DBNull.Value ? Convert.ToInt32(dr["Minute"]) : 0,
                             CreatedByVal = dr["CreatedByVal"].ToString(),
                             TimeFromVal = dr["TimeFromVal"].ToString(),
                             TimeToVal = dr["TimeToVal"].ToString(),
                             LeaveType = dr["LeaveType"].ToString()
                         }).ToList();
            model.TotalCountPto = (from DataRow dr in ds.Tables[1].Rows
                          select new TotalCountPto()
                          {
                              CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                          }).FirstOrDefault();
            return model;
        }
        public List<Pto> GetAllPtoListByUserId(Guid userId)
        {
            
            return _PtoDataAccess.GetByQuery(string.Format("UserId='{0}'",userId));
        }
        public List<Pto> GetAllPtoListByUserIdForTicket(Guid userId)
        {

            return _PtoDataAccess.GetByQuery(string.Format("UserId = '{0}' AND Status = 'Accepted'", userId));
        }
        public List<EmployeeOperations> GetAllEmployeeOperationsListByUserIdForTicket(Guid userId,DateTime date)
        {
            var data = _EmployeeOperationsDataAccess.GetByQuery(string.Format("EmployeeId = '{0}' AND IsDayOff = 1 and SelectedDate = '{1}'", userId, date));
            return data;
        }
        public PtoFilterModel GetAllEmployeesPtoBySupervisorId(Guid userId, PayrollFilterModel filter,bool IsHrManager, bool? getreport)
        {
            DataSet ds = _PtoDataAccess.GetAllEmployeesPtoBySupervisorId(userId, filter, IsHrManager, getreport);

            List<Pto> buildList = new List<Pto>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new Pto()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = (Guid)dr["UserId"],
                             StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                             Type = dr["Type"].ToString(),
                             EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                             LeaveTime = dr["LeaveTime"].ToString(),
                             TimeFrom = dr["TimeFrom"].ToString(),
                             TimeTo = dr["TimeTo"].ToString(),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             Notes = dr["Notes"].ToString(),
                             Status = dr["Status"].ToString(),
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                             Payable = dr["Payable"] != DBNull.Value ? Convert.ToBoolean(dr["Payable"]) : false,
                             RejectNote = dr["RejectNote"].ToString(),
                             Minute = dr["Minute"] != DBNull.Value ? Convert.ToInt32(dr["Minute"]) : 0,
                             CreatedByVal = dr["CreatedByVal"].ToString(),
                             TimeFromVal = dr["TimeFromVal"].ToString(),
                             TimeToVal = dr["TimeToVal"].ToString(),
                             LeaveType = dr["LeaveType"].ToString(),
                             PtoRemain = dr["PtoRemain"].ToString(),
                             RequestedByVal = dr["RequestedByVal"].ToString()
                         }).ToList();

            TotalCountPto TotalCountPto = new TotalCountPto();
            TotalCountPto = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCountPto()
                             {
                                 CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                             }).FirstOrDefault();
            PtoFilterModel PtoFilterModel = new PtoFilterModel();
            PtoFilterModel.ListPto = buildList;
            PtoFilterModel.TotalCountPto = TotalCountPto;
            return PtoFilterModel;
        }

        public List<Pto> GetAllEmployeesHoliday(string UserIdList, DateTime SelectedDate, string status)
        {
            DataSet ds = _PtoDataAccess.GetAllHolidayForCalendar(UserIdList, SelectedDate, status);
            List<Pto> buildList = new List<Pto>();
            if (ds != null)
            {
                buildList = (from DataRow dr in ds.Tables[0].Rows
                             select new Pto()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 UserId = (Guid)dr["UserId"],
                                 StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                 Type = dr["Type"].ToString(),
                                 EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                 LeaveTime = dr["LeaveTime"].ToString(),
                                 TimeFrom = dr["TimeFrom"].ToString(),
                                 TimeTo = dr["TimeTo"].ToString(),
                                 CreatedBy = (Guid)dr["CreatedBy"],
                                 Notes = dr["Notes"].ToString(),
                                 Status = dr["Status"].ToString(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 Payable = dr["Payable"] != DBNull.Value ? Convert.ToBoolean(dr["Payable"]) : false,
                                 RejectNote = dr["RejectNote"].ToString(),
                                 Minute = dr["Minute"] != DBNull.Value ? Convert.ToInt32(dr["Minute"]) : 0
                             }).ToList();
            }
            return buildList;
        }
        public List<Pto> GetEmployeeTotalPtoHour(Guid UserId)
        {
            DataTable dt = _PtoDataAccess.GetEmployeePtoHourByUserId(UserId);
            List<Pto> Emplist = new List<Pto>();
            Emplist = (from DataRow dr in dt.Rows
                       select new Pto()
                       {
                           TotalMinute = dr["TotalMinute"] != DBNull.Value ? Convert.ToDouble(dr["TotalMinute"]) : 0,
                       }).ToList();
            return Emplist;
        }
        public bool UpdatePTO(Pto pto)
        {
            return _PtoDataAccess.Update(pto)>0;
        }

        public bool DeletePto(int id)
        {
            return _PtoDataAccess.Delete(id) > 0;
        }
    }
}
