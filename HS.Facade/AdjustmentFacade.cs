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
    public class AdjustmentFacade : BaseFacade
    {
        public AdjustmentFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        AdjustmentDataAccess _AdjustmentDataAccess
        {
            get
            {
                return (AdjustmentDataAccess)_ClientContext[typeof(AdjustmentDataAccess)];
            }
        }
        AdjustmentRuleDataAccess _AdjustmentRuleDataAccess
        {
            get
            {
                return (AdjustmentRuleDataAccess)_ClientContext[typeof(AdjustmentRuleDataAccess)];
            }
        }
        AdjustmentSchemeDataAccess _AdjustmentSchemeDataAccess
        {
            get
            {
                return (AdjustmentSchemeDataAccess)_ClientContext[typeof(AdjustmentSchemeDataAccess)];
            }
        }

        OverrideDataAccess _OverrideDataAccess
        {
            get
            {
                return (OverrideDataAccess)_ClientContext[typeof(OverrideDataAccess)];
            }
        }

        OverrideRangeDataAccess _OverrideRangeDataAccess
        {
            get
            {
                return (OverrideRangeDataAccess)_ClientContext[typeof(OverrideRangeDataAccess)];
            }
        }

        public AdjustmentScheme GetAdjustmentSchemeById(int value)
        {
            return _AdjustmentSchemeDataAccess.Get(value);
        }

        public long InsertAdjustmentScheme(AdjustmentScheme ct)
        {
            return _AdjustmentSchemeDataAccess.Insert(ct);
        }

        public bool UpdateAdjustmentScheme(AdjustmentScheme ct)
        {
            return _AdjustmentSchemeDataAccess.Update(ct) > 0;
        }

        public List<AdjustmentScheme> GetAllAdjustmentScheme()
        {
            return _AdjustmentSchemeDataAccess.GetAll();
        }

        public void DeleteAdjustmentScheme(int id)
        {
            _AdjustmentSchemeDataAccess.Delete(id);
        }

        public List<AdjustmentScheme> GetAllAdjustmentSchemeWithCommissionSession()
        {
            DataTable dt = _AdjustmentDataAccess.GetAllAdjustmentSchemeWithCommissionSession();
            List<AdjustmentScheme> Responsiblelist = new List<AdjustmentScheme>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new AdjustmentScheme()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Name = dr["Name"].ToString(),
                                   ComissionSessionId = dr["ComissionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["ComissionSessionId"]) : 0,
                                   SessionName = dr["SessionName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public AdjustmentRule GetAdjustmentRuleById(int value)
        {
            return _AdjustmentRuleDataAccess.Get(value);
        }

        public long InsertAdjustmentRule(AdjustmentRule ct)
        {
            return _AdjustmentRuleDataAccess.Insert(ct);
        }

        public bool UpdateAdjustmentRule(AdjustmentRule ct)
        {
            return _AdjustmentRuleDataAccess.Update(ct) > 0;
        }

        public List<AdjustmentRule> GetAllAdjustmentRule()
        {
            return _AdjustmentRuleDataAccess.GetAll();
        }

        public void DeleteAdjustmentRule(int id)
        {
            _AdjustmentRuleDataAccess.Delete(id);
        }

        public List<AdjustmentRule> GetAllAdjustmentRuleWithCommissionSessionandScheme()
        {
            DataTable dt = _AdjustmentDataAccess.GetAllAdjustmentRuleWithCommissionSessionandScheme();
            List<AdjustmentRule> Responsiblelist = new List<AdjustmentRule>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new AdjustmentRule()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   ComissionSessionId = dr["ComissionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["ComissionSessionId"]) : 0,
                                   AdjustSchemeId = dr["AdjustSchemeId"] != DBNull.Value ? Convert.ToInt32(dr["AdjustSchemeId"]) : 0,
                                   TableName = dr["TableName"].ToString(),
                                   ColumnName = dr["ColumnName"].ToString(),
                                   ColumnValue = dr["ColumnValue"].ToString(),
                                   DataType = dr["DataType"].ToString(),
                                   CommandType = dr["CommandType"].ToString(),
                                   SessionName = dr["SessionName"].ToString(),
                                   SchemeName = dr["SchemeName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public Adjustment GetAdjustmentById(int value)
        {
            return _AdjustmentDataAccess.Get(value);
        }

        public long InsertAdjustment(Adjustment ct)
        {
            return _AdjustmentDataAccess.Insert(ct);
        }

        public bool UpdateAdjustment(Adjustment ct)
        {
            return _AdjustmentDataAccess.Update(ct) > 0;
        }

        public List<Adjustment> GetAllAdjustment()
        {
            return _AdjustmentDataAccess.GetAll();
        }

        public void DeleteAdjustment(int id)
        {
            _AdjustmentDataAccess.Delete(id);
        }

        public List<Adjustment> GetAllAdjustmentWithCommissionSessionandScheme()
        {
            DataTable dt = _AdjustmentDataAccess.GetAllAdjustmentWithCommissionSessionandScheme();
            List<Adjustment> Responsiblelist = new List<Adjustment>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new Adjustment()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   ComissionSessionId = dr["ComissionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["ComissionSessionId"]) : 0,
                                   AdjustSchemeId = dr["AdjustSchemeId"] != DBNull.Value ? Convert.ToInt32(dr["AdjustSchemeId"]) : 0,
                                   Description = dr["Description"].ToString(),
                                   Conduit = dr["Conduit"].ToString(),
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                                   Multiple = dr["Multiple"] != DBNull.Value ? Convert.ToDouble(dr["Multiple"]) : 0.0,
                                   AppliedTo = dr["AppliedTo"].ToString(),
                                   StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                   EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                   SessionName = dr["SessionName"].ToString(),
                                   SchemeName = dr["SchemeName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public Override GetOverrideById(int value)
        {
            return _OverrideDataAccess.Get(value);
        }

        public long InsertOverride(Override ct)
        {
            return _OverrideDataAccess.Insert(ct);
        }

        public bool UpdateOverride(Override ct)
        {
            return _OverrideDataAccess.Update(ct) > 0;
        }

        public List<Override> GetAllOverride()
        {
            return _OverrideDataAccess.GetAll();
        }

        public void DeleteOverride(int id)
        {
            _OverrideDataAccess.Delete(id);
        }

        public OverrideRange GetOverrideRangeById(int value)
        {
            return _OverrideRangeDataAccess.Get(value);
        }

        public long InsertOverrideRange(OverrideRange ct)
        {
            return _OverrideRangeDataAccess.Insert(ct);
        }

        public bool UpdateOverrideRange(OverrideRange ct)
        {
            return _OverrideRangeDataAccess.Update(ct) > 0;
        }

        public List<OverrideRange> GetAllOverrideRange()
        {
            return _OverrideRangeDataAccess.GetAll();
        }

        public void DeleteOverrideRange(int id)
        {
            _OverrideRangeDataAccess.Delete(id);
        }

        public List<OverrideRange> GetAllOverrideRangeWithOverrideName()
        {
            DataTable dt = _AdjustmentDataAccess.GetAllOverrideRangeWithOverrideName();
            List<OverrideRange> Responsiblelist = new List<OverrideRange>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new OverrideRange()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   OverrideId = dr["OverrideId"] != DBNull.Value ? Convert.ToInt32(dr["OverrideId"]) : 0,
                                   RangeStart = dr["RangeStart"] != DBNull.Value ? Convert.ToInt32(dr["RangeStart"]) : 0,
                                   RangeEnd = dr["RangeEnd"] != DBNull.Value ? Convert.ToInt32(dr["RangeEnd"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   OverName = dr["OverName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public List<AdjustmentRule> GetAllAdjustmentRuleBySchemeId(string id, string sid)
        {
            DataTable dt = _AdjustmentDataAccess.GetAllAdjustmentRuleBySchemeId(id, sid);
            List<AdjustmentRule> Responsiblelist = new List<AdjustmentRule>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new AdjustmentRule()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   ComissionSessionId = dr["ComissionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["ComissionSessionId"]) : 0,
                                   AdjustSchemeId = dr["AdjustSchemeId"] != DBNull.Value ? Convert.ToInt32(dr["AdjustSchemeId"]) : 0,
                                   TableName = dr["TableName"].ToString(),
                                   ColumnName = dr["ColumnName"].ToString(),
                                   ColumnValue = dr["ColumnValue"].ToString(),
                                   DataType = dr["DataType"].ToString(),
                                   CommandType = dr["CommandType"].ToString(),
                                   SessionName = dr["SessionName"].ToString(),
                                   SchemeName = dr["SchemeName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }

        public List<Adjustment> GetAllAdjustmentBySchemeId(string id, string sid)
        {
            DataTable dt = _AdjustmentDataAccess.GetAllAdjustmentBySchemeId(id, sid);
            List<Adjustment> Responsiblelist = new List<Adjustment>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new Adjustment()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   ComissionSessionId = dr["ComissionSessionId"] != DBNull.Value ? Convert.ToInt32(dr["ComissionSessionId"]) : 0,
                                   AdjustSchemeId = dr["AdjustSchemeId"] != DBNull.Value ? Convert.ToInt32(dr["AdjustSchemeId"]) : 0,
                                   Description = dr["Description"].ToString(),
                                   Conduit = dr["Conduit"].ToString(),
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                                   Multiple = dr["Multiple"] != DBNull.Value ? Convert.ToDouble(dr["Multiple"]) : 0.0,
                                   AppliedTo = dr["AppliedTo"].ToString(),
                                   StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                   EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                   SessionName = dr["SessionName"].ToString(),
                                   SchemeName = dr["SchemeName"].ToString()
                               }).ToList();
            return Responsiblelist;
        }
    }
}
