using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;

namespace HS.Facade
{
    public class LeadCorrespondenceFacade :BaseFacade
    {
        public LeadCorrespondenceFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        LeadCorrespondenceDataAccess _LeadCorrespondenceDataAccess
        {
            get
            {
                return (LeadCorrespondenceDataAccess)_ClientContext[typeof(LeadCorrespondenceDataAccess)];
            }
        }
        ActivityDataAccess _ActivityDataAccess
        {
            get
            {
                return (ActivityDataAccess)_ClientContext[typeof(ActivityDataAccess)];
            }
        }

        public List<LeadCorrespondence> GetAllMailCorrespondenceByCompanyIdAndCustomerId(Guid comid, Guid cusid)
        {
            DataTable dt = _LeadCorrespondenceDataAccess.GetAllMailCorrespondenceByCompanyIdAndCustomerId(comid, cusid);
            List<LeadCorrespondence> LeadCorrespondenceList = new List<LeadCorrespondence>();
            LeadCorrespondenceList = (from DataRow dr in dt.Rows
                                      select new LeadCorrespondence()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          CustomerId = (Guid)dr["CustomerId"],
                                          //ContactName = dr["ContactName"].ToString(),
                                          TemplateKey = dr["TemplateKey"].ToString(),
                                          Type = dr["Type"].ToString(),
                                          ToEmail = dr["ToEmail"].ToString(),
                                          CcEmail = dr["CcEmail"].ToString(),
                                          BccEmail = dr["BccEmail"].ToString(),
                                          FromEmail = dr["FromEmail"].ToString(),
                                          ToMobileNo = dr["ToMobileNo"].ToString(),
                                          FromMobileNo = dr["FromMobileNo"].ToString(),
                                          FromName = dr["FromName"].ToString(),
                                          Subject = dr["Subject"].ToString(),
                                          BodyContent = dr["BodyContent"].ToString(),
                                          EmpName = dr["EmpName"].ToString(),
                                          IsRead = (dr["IsRead"] != DBNull.Value ? Convert.ToBoolean(dr["IsRead"]) : false),
                                          LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                          SentDate = dr["SentDate"] != DBNull.Value ? Convert.ToDateTime(dr["SentDate"]) : new DateTime(),
                                          SentBy = (Guid)dr["SentBy"]
                                      }).ToList();

            return LeadCorrespondenceList;
        }

        public long InsertCorrespondence(LeadCorrespondence lc)
        {
            try
            {
                _ActivityDataAccess.Insert(new Activity()
                {
                    ActivityId = Guid.NewGuid(),
                    ActivityType = lc.Type,
                    AssignedTo = lc.SentBy,
                    AssociatedWith = lc.CustomerId,
                    AssociatedType = lc.AssociatedType,
                    Status = "Completed",
                    CreatedBy = lc.SentBy,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    Note = lc.BodyContent,
                    NotifyBy = "",
                });
            }
            catch (Exception) { } 
            return _LeadCorrespondenceDataAccess.Insert(lc);
        }
        public bool UpdateCorrespondence(LeadCorrespondence lc)
        {
            return _LeadCorrespondenceDataAccess.Update(lc) > 0;
        }
        public LeadCorrespondence GetCorrespondenceById(int value)
        {
            return _LeadCorrespondenceDataAccess.Get(value);
        }
        public LeadCorrespondence GetLeadCorrespondenceByCustomerId(Guid CustomerId)
        {
            return _LeadCorrespondenceDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
        }
    }
}
