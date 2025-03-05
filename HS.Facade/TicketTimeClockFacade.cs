using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HS.Facade
{
    public class TicketTimeClockFacade : BaseFacade
    {
        TicketTimeClockDataAccess _TicketTimeClockDataAccess = null;

        public TicketTimeClockFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_TicketTimeClockDataAccess == null)
                _TicketTimeClockDataAccess = (TicketTimeClockDataAccess)_ClientContext[typeof(TicketTimeClockDataAccess)];

        }
        public int InsertTimeClock(TicketTimeClock TC)
        {
            return (int)_TicketTimeClockDataAccess.Insert(TC);
        }

        public TicketTimeClock GetLastClockInByUserIdAndTicketId(Guid UserId,Guid TicketId)
        {
            return _TicketTimeClockDataAccess.GetLastClockInByUserIdAndTicketId(UserId, TicketId); 
        }
        public List<TicketTimeClock> GetLastClockOutByUserIdAndTicketId(Guid UserId, Guid TicketId)
        {
            DataTable dt = _TicketTimeClockDataAccess.GetLastClockOutByUserIdAndTicketId(UserId, TicketId);
            List<TicketTimeClock> asd = (from DataRow dr in dt.Rows
                                         select new TicketTimeClock()
                                         {

                                             Time = dr["Time"] != DBNull.Value ? Convert.ToDateTime(dr["Time"]) : new DateTime(),
                                             Type = dr["Type"].ToString(),
                                             ClockedInMinutes = dr["ClockedInMinutes"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInMinutes"]) : 0,

                                             Note = dr["Note"].ToString(),

                                         }).ToList();
            return asd;
        }

        public TicketTimeClock GetLastTicketTimeClockByTicketId(Guid TicketId)
        {
            return _TicketTimeClockDataAccess.GetLastTicketTimeClockByTicketId(TicketId);
        }

        public List<TicketTimeClock> GetTicketTimeClockByTicketId(Guid TicketId)
        {
            return _TicketTimeClockDataAccess.GetByQuery(string.Format("TicketId = '{0}'", TicketId));
        }
    }
}
