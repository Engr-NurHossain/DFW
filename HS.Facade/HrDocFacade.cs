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
    public class HrDocFacade : BaseFacade
    {
        public HrDocFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        HrDocDataAccess _HrDocDataAccess
        {
            get
            {
                return (HrDocDataAccess)_ClientContext[typeof(HrDocDataAccess)];
            }
        }

        public List<HrDoc> GetAllUserFilesByCompanyId(Guid companyid, string user)
        {
            DataTable dt = _HrDocDataAccess.GetAllUserFilesByCompanyId(companyid, user);
            List<HrDoc> HrDocList = new List<HrDoc>();
            HrDocList = (from DataRow dr in dt.Rows
                              select new HrDoc()
                              {
                                  FileDescription = dr["FileDescription"].ToString(),
                                  Filename = dr["Filename"].ToString(),
                                
                                  UserName = dr["UserName"].ToString(),
                                  CompanyId = (Guid)dr["CompanyId"],
                            
                                  Uploadeddate = dr["Uploadeddate"] != DBNull.Value ? Convert.ToDateTime(dr["Uploadeddate"]) : DateTime.Now,
                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                              
                                  CreatedByName = dr["CreatedByName"].ToString(),
                         
                                  DocCategory = dr["DocCategory"].ToString(),
                              }).ToList();
            return HrDocList;
           
        }
        public List<HrDoc> GetAllUserFilesByCompanyIdAndFilter(Guid companyid, string user,string SearchText,string FilterText)
        {
            DataTable dt = _HrDocDataAccess.GetAllUserFilesByCompanyIdAndFilter(companyid, user,SearchText,FilterText);
            List<HrDoc> HrDocList = new List<HrDoc>();
            HrDocList = (from DataRow dr in dt.Rows
                         select new HrDoc()
                         {
                             FileDescription = dr["FileDescription"].ToString(),
                             Filename = dr["Filename"].ToString(),

                             UserName = dr["UserName"].ToString(),
                             CompanyId = (Guid)dr["CompanyId"],

                             Uploadeddate = dr["Uploadeddate"] != DBNull.Value ? Convert.ToDateTime(dr["Uploadeddate"]) : DateTime.Now,
                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                             CreatedByName = dr["CreatedByName"].ToString(),

                             DocCategory = dr["DocCategory"].ToString(),
                         }).ToList();
            return HrDocList;

        }
        public long InsertHrDoc(HrDoc eq)
        {
            return _HrDocDataAccess.Insert(eq);
        }
        public bool UpdateHrDoc(HrDoc eq)
        {
            return _HrDocDataAccess.Update(eq) > 0;
        }

        public string GetUserLoginNameById(int id)
        {
            string usernum = "";
            DataTable dt = _HrDocDataAccess.GetUserLoginNameById(id);
            usernum = dt.Rows[0]["UserName"].ToString();
            return usernum;
        }
        public bool DeleteUser(int Id)
        {
            return _HrDocDataAccess.Delete(Id) > 0;
        }

        public HrDoc GetFileNameById(int v)
        {
            return _HrDocDataAccess.Get(v);
        }

        public HrDoc GetHrDocByUsernameDescriptionAndCompanyId(string username, string Desc, Guid CompanyId)
        {
            return _HrDocDataAccess.GetByQuery(string.Format("UserName='{0}' and CompanyId ='{1}' and FileDescription='{2}'",username, CompanyId,Desc)).FirstOrDefault();
        }
        public HrDoc GetHrDocByUsernameAndCategory(string username,Guid CompanyId,string Category)
        {
            return _HrDocDataAccess.GetByQuery(string.Format("UserName='{0}' and CompanyId ='{1}' and Category='{2}'", username, CompanyId,Category)).FirstOrDefault();
        }
        public HrDoc GetHrDocById(int id)
        {
            return _HrDocDataAccess.Get(id);
        }
    }
}
