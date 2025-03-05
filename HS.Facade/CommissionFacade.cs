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
    public class CommissionFacade : BaseFacade
    {
        public CommissionFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CommisionDataAccess _CommisionDataAccess
        {
            get
            {
                return (CommisionDataAccess)_ClientContext[typeof(CommisionDataAccess)];
            }
        }
        TechCommissionDataAccess _TechCommissionDataAccess
        {
            get
            {
                return (TechCommissionDataAccess)_ClientContext[typeof(TechCommissionDataAccess)];
            }
        }
        SalesCommissionDataAccess _SalesCommissionDataAccess
        {
            get
            {
                return (SalesCommissionDataAccess)_ClientContext[typeof(SalesCommissionDataAccess)];
            }
        }
        CommisionTypeDataAccess _CommisionTypeDataAccess
        {
            get
            {
                return (CommisionTypeDataAccess)_ClientContext[typeof(CommisionTypeDataAccess)];
            }
        }
        CommisionSessionDataAccess _CommisionSessionDataAccess
        {
            get
            {
                return (CommisionSessionDataAccess)_ClientContext[typeof(CommisionSessionDataAccess)];
            }
        }
        CommisionRangeDataAccess _CommisionRangeDataAccess
        {
            get
            {
                return (CommisionRangeDataAccess)_ClientContext[typeof(CommisionRangeDataAccess)];
            }
        }

        CreditClassDataAccess _CreditClassDataAccess
        {
            get
            {
                return (CreditClassDataAccess)_ClientContext[typeof(CreditClassDataAccess)];
            }
        }

        public CommisionType GetCommissionTypeById(int value)
        {
            return _CommisionTypeDataAccess.Get(value);
        }

        public long InsertCommissionType(CommisionType ct)
        {
            return _CommisionTypeDataAccess.Insert(ct);
        }

        public bool UpdateCommissionType(CommisionType ct)
        {
            return _CommisionTypeDataAccess.Update(ct) > 0;
        }

        public List<CommisionType> GetAllCommissionType()
        {
            return _CommisionTypeDataAccess.GetAll();
        }

        public void DeleteCommissionType(int id)
        {
            _CommisionTypeDataAccess.Delete(id);
        }

        public CommisionSession GetCommissionSessionById(int value)
        {
            return _CommisionSessionDataAccess.Get(value);
        }

        public long InsertCommissionSession(CommisionSession ct)
        {
            return _CommisionSessionDataAccess.Insert(ct);
        }

        public bool UpdateCommissionSession(CommisionSession ct)
        {
            return _CommisionSessionDataAccess.Update(ct) > 0;
        }

        public List<CommisionSession> GetAllCommissionSession()
        {
            return _CommisionSessionDataAccess.GetAll();
        }

        public void DeleteCommissionSession(int id)
        {
            _CommisionSessionDataAccess.Delete(id);
        }

        public CommisionRange GetCommissionRangeById(int value)
        {
            return _CommisionRangeDataAccess.Get(value);
        }

        public long InsertCommissionRange(CommisionRange ct)
        {
            return _CommisionRangeDataAccess.Insert(ct);
        }

        public bool UpdateCommissionRange(CommisionRange ct)
        {
            return _CommisionRangeDataAccess.Update(ct) > 0;
        }

        public List<CommisionRange> GetAllCommissionRange()
        {
            return _CommisionRangeDataAccess.GetAll();
        }

        public void DeleteCommissionRange(int id)
        {
            _CommisionRangeDataAccess.Delete(id);
        }

        public List<CommisionRange> GetAllCommissionRangeWithCommissionTypeAndSession()
        {
            DataTable dt = _CommisionDataAccess.GetAllCommissionRangeWithCommissionTypeAndSession();
            List<CommisionRange> Responsiblelist = new List<CommisionRange>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new CommisionRange()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CommisionTypeId = dr["CommisionTypeId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionTypeId"]) : 0,
                                   CommisionSessionId = dr["CommisionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionSessionId"]) : 0,
                                   RangeStart = dr["RangeStart"] != DBNull.Value ? Convert.ToInt32(dr["RangeStart"]) : 0,
                                   RangeEnd = dr["RangeEnd"] != DBNull.Value ? Convert.ToInt32(dr["RangeEnd"]) : 0,
                                   Upfront = dr["Upfront"] != DBNull.Value ? Convert.ToDouble(dr["Upfront"]) : 0.0,
                                   Backend = dr["Backend"] != DBNull.Value ? Convert.ToDouble(dr["Backend"]) : 0.0,
                                   Bonus = dr["Bonus"] != DBNull.Value ? Convert.ToDouble(dr["Bonus"]) : 0.0,
                                   RentBonus = dr["RentBonus"] != DBNull.Value ? Convert.ToDouble(dr["RentBonus"]) : 0.0,
                                   TypeName = dr["TypeName"].ToString(),
                                   SessionName = dr["SessionName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public Commision GetCommissionById(int value)
        {
            return _CommisionDataAccess.Get(value);
        }

        public long InsertCommission(Commision ct)
        {
            return _CommisionDataAccess.Insert(ct);
        }

        public bool UpdateCommission(Commision ct)
        {
            return _CommisionDataAccess.Update(ct) > 0;
        }

        public List<Commision> GetAllCommission()
        {
            return _CommisionDataAccess.GetAll();
        }

        public void DeleteCommission(int id)
        {
            _CommisionDataAccess.Delete(id);
        }

        public List<Commision> GetAllCommissionWithCommissionTypeAndSession()
        {
            DataTable dt = _CommisionDataAccess.GetAllCommissionWithCommissionTypeAndSession();
            List<Commision> Responsiblelist = new List<Commision>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new Commision()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CommisionTypeId = dr["CommisionTypeId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionTypeId"]) : 0,
                                   CommisionSessionId = dr["CommisionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionSessionId"]) : 0,
                                   Name = dr["Name"].ToString(),
                                   TimeFrame = dr["TimeFrame"].ToString(),
                                   IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                   TypeName = dr["TypeName"].ToString(),
                                   SessionName = dr["SessionName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public CreditClass GetCreditClassById(int value)
        {
            return _CreditClassDataAccess.Get(value);
        }

        public long InsertCreditClass(CreditClass ct)
        {
            return _CreditClassDataAccess.Insert(ct);
        }

        public bool UpdateCreditClass(CreditClass ct)
        {
            return _CreditClassDataAccess.Update(ct) > 0;
        }

        public List<CreditClass> GetAllCreditClass()
        {
            return _CreditClassDataAccess.GetAll();
        }

        public void DeleteCreditClass(int id)
        {
            _CreditClassDataAccess.Delete(id);
        }

        public List<CommisionRange> GetAllCommissionRangeByTypeId(string id, string sid)
        {
            DataTable dt = _CommisionDataAccess.GetAllCommissionRangeByTypeId(id, sid);
            List<CommisionRange> Responsiblelist = new List<CommisionRange>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new CommisionRange()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CommisionTypeId = dr["CommisionTypeId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionTypeId"]) : 0,
                                   CommisionSessionId = dr["CommisionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionSessionId"]) : 0,
                                   RangeStart = dr["RangeStart"] != DBNull.Value ? Convert.ToInt32(dr["RangeStart"]) : 0,
                                   RangeEnd = dr["RangeEnd"] != DBNull.Value ? Convert.ToInt32(dr["RangeEnd"]) : 0,
                                   Upfront = dr["Upfront"] != DBNull.Value ? Convert.ToDouble(dr["Upfront"]) : 0.0,
                                   Backend = dr["Backend"] != DBNull.Value ? Convert.ToDouble(dr["Backend"]) : 0.0,
                                   Bonus = dr["Bonus"] != DBNull.Value ? Convert.ToDouble(dr["Bonus"]) : 0.0,
                                   RentBonus = dr["RentBonus"] != DBNull.Value ? Convert.ToDouble(dr["RentBonus"]) : 0.0,
                                   TypeName = dr["TypeName"].ToString(),
                                   SessionName = dr["SessionName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public List<Commision> GetAllCommissionByTypeId(string id, string sid)
        {
            DataTable dt = _CommisionDataAccess.GetAllCommissionByTypeId(id, sid);
            List<Commision> Responsiblelist = new List<Commision>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new Commision()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CommisionTypeId = dr["CommisionTypeId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionTypeId"]) : 0,
                                   CommisionSessionId = dr["CommisionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["CommisionSessionId"]) : 0,
                                   Name = dr["Name"].ToString(),
                                   TimeFrame = dr["TimeFrame"].ToString(),
                                   IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                   TypeName = dr["TypeName"].ToString(),
                                   SessionName = dr["SessionName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public int GetLastBatchNo()
        {
            return _SalesCommissionDataAccess.GetLastBatchNo();
        }

        public int GetLastTechBatchNo()
        {
            return _SalesCommissionDataAccess.GetLastTechBatchNo();
        }

        public int GetLastSalesBatchNo()
        {
            return _SalesCommissionDataAccess.GetLastSalesBatchNo();
        }

        public List<TechCommission> GetTechCommissionListByIdList(List<int> idList)
        {
            return _TechCommissionDataAccess.GetByQuery(string.Format(" Id in ({0})",string.Join(",",idList)));
        }
        public List<TechCommission> GetTechCommissionListByTicketIdList(List<Guid> idList)
        {
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(string.Join(",", idList));
            var query = string.Format(" TicketId in ('{0}') and IsPaid = 0", strHTMLContent.ToString());
            return _TechCommissionDataAccess.GetByQuery(query);
        }
        public bool UpdateTechCommission(TechCommission item)
        {
            return _TechCommissionDataAccess.Update(item)>0;
        }

        public List<SalesCommission> GetSalesCommissionListByIdList(List<int> idList)
        {
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(string.Join(",", idList));
            var query = string.Format(" TicketId in ('{0}') and IsPaid = 0", strHTMLContent.ToString());
            return _SalesCommissionDataAccess.GetByQuery(query);
        }
        public List<SalesCommission> GetSalesCommissionListByTicketIdList(List<Guid> idList)
        {
            return _SalesCommissionDataAccess.GetByQuery(string.Format(" TicketId in ('{0}') and IsPaid = 0", string.Join(",", idList)));
        }
        public bool UpdateSalesCommission(SalesCommission item)
        {
            return _SalesCommissionDataAccess.Update(item) > 0;
        }

        public bool DeleteAllCommissionByCustomerIdAndTicketId(Guid customerId, Guid ticketId)
        {
            return _SalesCommissionDataAccess.DeleteAllCommissionByCustomerIdAndTicketId(customerId, ticketId);
        }
    }
}
