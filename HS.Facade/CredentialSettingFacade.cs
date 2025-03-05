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
   public class CredentialSettingFacade: BaseFacade
    {
        public CredentialSettingFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CredentialSettingDataAccess _CredentialSettingDataAccess
        {
            get
            {
                return (CredentialSettingDataAccess)_ClientContext[typeof(CredentialSettingDataAccess)];
            }
        }

        RMRTagDataAccess _RMRTagDataAccess
        {
            get
            {
                return (RMRTagDataAccess)_ClientContext[typeof(RMRTagDataAccess)];
            }
        }
        KnowledgebaseRMRTagDataAccess _KnowledgebaseRMRTagDataAccess
        {
            get
            {
                return (KnowledgebaseRMRTagDataAccess)_ClientContext[typeof(KnowledgebaseRMRTagDataAccess)];
            }
        }
        BuildLogDataAccess _BuildLogDataAccess
        {
            get
            {
                return (BuildLogDataAccess)_ClientContext[typeof(BuildLogDataAccess)];
            }
        }
        KnowledgebaseRMRTagMapDataAccess _KnowledgebaseRMRTagMapDataAccess
        {
            get
            {
                return (KnowledgebaseRMRTagMapDataAccess)_ClientContext[typeof(KnowledgebaseRMRTagMapDataAccess)];
            }
        }
        RMRTagMapDataAccess _RMRTagMapDataAccess
        {
            get
            {
                return (RMRTagMapDataAccess)_ClientContext[typeof(RMRTagMapDataAccess)];
            }
        }

        public long InsertCredentialSetting(CredentialSetting eq)
        {
            return _CredentialSettingDataAccess.Insert(eq);
        }
        public bool UpdateCredentialSetting(CredentialSetting eq)
        {
            return _CredentialSettingDataAccess.Update(eq) > 0;
        }
        public bool DeleteCredentialSetting(int Id)
        {
            return _CredentialSettingDataAccess.Delete(Id) > 0;
        }
        public CredentialSetting GetById(int value)
        {
            return _CredentialSettingDataAccess.Get(value);
        }
        public List<CredentialSetting> GetAllAccountHolderByCompanyId(Guid companyId)
        {
            return _CredentialSettingDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
        public List<CredentialSetting> GetAllCredentialSettingListByCompanyId(Guid companyId)
        {
            DataTable dt = _CredentialSettingDataAccess.GetAllCredentialSettingListByCompanyId(companyId);
            List<CredentialSetting> AppointmentEquipList = new List<CredentialSetting>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CredentialSetting()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        CompanyId = (Guid)dr["CompanyId"],
                                        AcountHolderId = dr["AcountHolderId"] != DBNull.Value ? Convert.ToInt32(dr["AcountHolderId"]) : 0,
                                        UserName = dr["UserName"].ToString(),
                                        Password = dr["Password"].ToString(),
                                        Description = dr["Description"].ToString(),
                                        DisplayName = dr["DisplayName"].ToString(),
                                        IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]):false
                                    }).ToList();
            return AppointmentEquipList;
        }

        public RMRTag GetRMRTagById(int value)
        {
            return _RMRTagDataAccess.Get(value);
        }
        public KnowledgebaseRMRTag GetKnowledgebaseRMRTagById(int value)
        {
            return _KnowledgebaseRMRTagDataAccess.Get(value);
        }
        public BuildLog GetBuilLogById(int value)
        {
            return _BuildLogDataAccess.Get(value);
        }
        public BuildLog GetBuilLogByMaxId(long value)
        {
            return _BuildLogDataAccess.GetByQuery(string.Format("Id = '{0}'", value)).FirstOrDefault(); 
        }
        public long InsertKnowledgebaseRMRTag(KnowledgebaseRMRTag tag)
        {
            return _KnowledgebaseRMRTagDataAccess.Insert(tag);
        }
        public long InsertBuildLog(BuildLog blog)
        {
            return _BuildLogDataAccess.Insert(blog);
        }
        public long InsertRMRTag(RMRTag tag)
        {
            return _RMRTagDataAccess.Insert(tag);
        }
        public long GetMaxVersion()
        {
            return _BuildLogDataAccess.GetMaxId();
        }
        public bool UpdateKnowledgebaseRMRTagMap(KnowledgebaseRMRTagMap map)
        {
            return _KnowledgebaseRMRTagMapDataAccess.Update(map) > 0;
        }
        public bool UpdateBuildLog(BuildLog blog)
        {
            return _BuildLogDataAccess.Update(blog) > 0;
        }
        public bool UpdateRMRTag(RMRTag tag)
        {
            return _RMRTagDataAccess.Update(tag) > 0;
        }
        public bool UpdateKnowledgebaseRMRTag(KnowledgebaseRMRTag tag)
        {
            return _KnowledgebaseRMRTagDataAccess.Update(tag) > 0;
        }
        public List<RMRTag> GetAllRMRTag(string search)
        {
            DataTable dt = _RMRTagDataAccess.GetAllRMRTag(search);
            List<RMRTag> AppointmentEquipList = new List<RMRTag>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new RMRTag()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        TagIdentifier = (Guid)dr["TagIdentifier"],
                                        TagName = dr["TagName"].ToString(),
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedUser = dr["CreatedUser"].ToString()
                                    }).ToList();
            return AppointmentEquipList;
        }
        public List<KnowledgebaseRMRTag> GetAllKnowledgebaseRMRTag(string search, string order)
        {
            DataTable dt = _RMRTagDataAccess.GetAllKnowledgebaseRMRTag(search,order);
            List<KnowledgebaseRMRTag> AppointmentEquipList = new List<KnowledgebaseRMRTag>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new KnowledgebaseRMRTag()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        TagName = dr["TagName"].ToString(),
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedUser = dr["CreatedUser"].ToString(),
                                        IsFavourite = dr["IsFavourite"] != DBNull.Value ? Convert.ToBoolean(dr["IsFavourite"]) : false,
                                        UsedCount = dr["Used"] != DBNull.Value ? Convert.ToInt32(dr["Used"]) : 0,
                                    }).ToList();
            return AppointmentEquipList;
        }

        public List<BuildLog> GetAllBuildLog(string search, string order)
        {
            DataTable dt = _RMRTagDataAccess.GetAllVersion(search, order);
            List<BuildLog> AppointmentEquipList = new List<BuildLog>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new BuildLog()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Version = dr["Version"].ToString(),
                                        BuildDate = dr["BuildDate"] != DBNull.Value ? Convert.ToDateTime(dr["BuildDate"]) : new DateTime(),
                                    }).ToList();
            return AppointmentEquipList;
        }

        public DataTable GetAllTagListByQuery(string query, string glist)
        {
            return _RMRTagDataAccess.GetAllTagListByQuery(query, glist);
        }

        public bool DeleteRmrTag(int value)
        {
            return _RMRTagDataAccess.Delete(value) > 0;
        }
        public bool DeleteKnowledgebaseRMRTag(int value)
        {
            return _KnowledgebaseRMRTagDataAccess.Delete(value) > 0;
        }
        public bool DeleteBuilLog(int value)
        {
            return _BuildLogDataAccess.Delete(value) > 0;
        }
        public List<RMRTag> GetAllTag()
        {
            return _RMRTagDataAccess.GetAll();
        }

        public List<RMRTag> GetTagByIdentifier(Guid value)
        {
            return _RMRTagDataAccess.GetByQuery(string.Format("TagIdentifier = '{0}'", value)).ToList();
        }

        public List<RMRTagMap> GetAllTagMapByContactid(Guid id)
        {
            return _RMRTagMapDataAccess.GetByQuery(string.Format("ContactId = '{0}'", id)).ToList();
        }

        public bool DeleteTagMapById(int value)
        {
            return _RMRTagMapDataAccess.Delete(value) > 0;
        }

        public long InsertTagMap(RMRTagMap map)
        {
            return _RMRTagMapDataAccess.Insert(map);
        }
        public long InsertKnowledgebaseRMRTagMap(KnowledgebaseRMRTagMap map)
        {
            return _KnowledgebaseRMRTagMapDataAccess.Insert(map);
        }
        public List<RMRTag> GetAllTagListByContactId(Guid contactid)
        {
            DataTable dt = _RMRTagDataAccess.GetAllTagListByContactId(contactid);
            List<RMRTag> AppointmentEquipList = new List<RMRTag>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new RMRTag()
                                    {
                                        TagIdentifier = (Guid) dr["TagIdentifier"],
                                        TagName = dr["TagName"].ToString()
                                    }).ToList();
            return AppointmentEquipList;
        }
    }
}
