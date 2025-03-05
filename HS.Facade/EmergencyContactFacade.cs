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
    public class EmergencyContactFacade : BaseFacade
    {
        public EmergencyContactFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EmergencyContactDataAccess _EmergencyContactDataAccess
        {
            get
            {
                return (EmergencyContactDataAccess)_ClientContext[typeof(EmergencyContactDataAccess)];
            }
        }

        EmergencyContactDraftDataAccess _EmergencyContactDraftDataAccess
        {
            get
            {
                return (EmergencyContactDraftDataAccess)_ClientContext[typeof(EmergencyContactDraftDataAccess)];
            }
        }
        public long InsertEmergencyContact(EmergencyContact Emergency)
        {
            return _EmergencyContactDataAccess.Insert(Emergency);
        }
        public long InsertEmergencyContactDraft(EmergencyContactDraft Emergency)
        {
            return _EmergencyContactDraftDataAccess.Insert(Emergency);
        }
        public long UpdateEmergencyContact(EmergencyContact Emergency)
        {
            return _EmergencyContactDataAccess.Update(Emergency);
        }
        public EmergencyContact GetEmergencyContactByCustomerIdAndCompanyId(Guid CustomerId, Guid CompamyId)
        {
            return _EmergencyContactDataAccess.GetByQuery(string.Format("CustomerId='{0}' and CompanyId = '{1}'", CustomerId, CompamyId)).OrderByDescending(x => x.Id).FirstOrDefault();
        }
        public EmergencyContact GetEmergencyContactById(int id)
        {
            return _EmergencyContactDataAccess.Get(id);
        }
        public bool UpdateCustomerChangeEmergencyContact(int Id, string ColumnName, string NewValue)
        {
            return _EmergencyContactDataAccess.UpdateEmergencyContact(Id, ColumnName, NewValue);
        }
        public EmergencyContactDraft GetEmergencyContactDraftByCustomerIdAndCompanyId(Guid CustomerId, Guid CompamyId)
        {
            return _EmergencyContactDraftDataAccess.GetByQuery(string.Format("CustomerId='{0}' and CompanyId = '{1}'", CustomerId, CompamyId)).OrderByDescending(x => x.Id).FirstOrDefault();
        }
        public List<EmergencyContact> GetAllEmergencyContactByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _EmergencyContactDataAccess.GetAllEmergencyContactByCustomerIdAndCompanyId(CustomerId, CompanyId);
            List<EmergencyContact> contactList = new List<EmergencyContact>();
            contactList = (from DataRow dr in dt.Rows
                           select new EmergencyContact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CustomerId = (Guid)dr["CustomerId"],
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               RelationShip = dr["RelationShip"].ToString(),
                               HasKey = dr["HasKey"].ToString(),
                               Phone = dr["Phone"].ToString(),
                               Platform = dr["Platform"].ToString(),
                               PhoneType = dr["PhoneType"].ToString(),
                               Email = dr["Email"].ToString(),
                               ContactNo = dr["ContactNo"].ToString()
                           }).ToList();
            return contactList;
        }
        public List<EmergencyContactDraft> GetAllEmergencyContactDraftByCustomerIdAndCompanyId(Guid CustomerId, Guid CompamyId)
        {
            return _EmergencyContactDraftDataAccess.GetByQuery(string.Format("CustomerId='{0}' and CompanyId = '{1}' Order By Id DESC", CustomerId, CompamyId)).ToList();
        }
        public EmergencyContact GetEmergencyContactByCustomerIdAndCompanyIdAndId(Guid CustomerId, Guid CompamyId,int Id)
        {
            return _EmergencyContactDataAccess.GetByQuery(string.Format("CustomerId='{0}' and CompanyId = '{1}' and Id='{2}'", CustomerId, CompamyId,Id)).FirstOrDefault();
        }
        public EmergencyContact GetEmergencyContactByFirstNameAndEmail(string FirstName, string Email)
        {
            return _EmergencyContactDataAccess.GetByQuery(string.Format("FirstName='{0}' and Email = '{1}'", FirstName, Email)).FirstOrDefault();
        }
        public EmergencyContactDraft GetEmergencyContactDraftByCustomerIdAndCompanyIdAndId(Guid CustomerId, Guid CompamyId, int Id)
        {
            return _EmergencyContactDraftDataAccess.GetByQuery(string.Format("CustomerId='{0}' and CompanyId = '{1}' and Id='{2}'", CustomerId, CompamyId, Id)).FirstOrDefault();
        }
        public List<LeadEmergencyDetail> GetAllLeadEmergencyDetailByLeadIdandCompanyId(Guid companyid, int id)
        {
            DataTable dt = _EmergencyContactDataAccess.GetAllLeadEmergencyDetailByLeadIdandCompanyId(companyid, id);
            List<LeadEmergencyDetail> Edetail = new List<LeadEmergencyDetail>();
            Edetail = (from DataRow dr in dt.Rows
                       select new LeadEmergencyDetail()
                       {
                         
                           ContactName = dr["ContactName"].ToString(),
                           ContactRelation = dr["ContactRelation"].ToString(),
                           ContactPhone = dr["ContactPhone"].ToString(),
                           ContactHaskey = dr["ContactHaskey"].ToString()
                       }).ToList();
            return Edetail;
        }

        public List<LeadEmergencyDetail> GetAllCustomerEmergencyDetailByLeadIdandCompanyId(Guid companyid, int id)
        {
            DataTable dt = _EmergencyContactDataAccess.GetAllCustomerEmergencyDetailByLeadIdandCompanyId(companyid, id);
            List<LeadEmergencyDetail> Edetail = new List<LeadEmergencyDetail>();
            Edetail = (from DataRow dr in dt.Rows
                       select new LeadEmergencyDetail()
                       {
                           ContactName = dr["ContactName"].ToString(),
                           ContactRelation = dr["ContactRelation"].ToString(),
                           ContactPhone = dr["ContactPhone"].ToString(),
                           ContactHaskey = dr["ContactHaskey"].ToString()
                       }).ToList();
            return Edetail;
        }

        public bool DeleteEmergencyContactById(int id)
        {
            return _EmergencyContactDataAccess.Delete(id) > 0;
        }
        public bool DeleteEmergencyContactDraftById(int id)
        {
            return _EmergencyContactDraftDataAccess.Delete(id) > 0;
        }

        public List<EmergencyContact> GetAllEmergencyContactByCustomerId(Guid customerid)
        {

            DataTable dt = _EmergencyContactDataAccess.GetAllEmergencyContactByCustomerId(customerid);
            List<EmergencyContact> contactList = new List<EmergencyContact>();
            contactList = (from DataRow dr in dt.Rows
                       select new EmergencyContact()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           CustomerId = (Guid)dr["CustomerId"],
                           FirstName = dr["FirstName"].ToString(),
                           LastName = dr["LastName"].ToString(),
                           RelationShip = dr["RelationShip"].ToString(),
                           CompanyId = (Guid)dr["CompanyId"],
                           HasKey = dr["HasKey"].ToString(),
                           RelationShipVal = dr["RelationShipVal"].ToString(),
                           Phone = dr["Phone"].ToString(),
                           Platform = dr["Platform"].ToString(),
                           PhoneType = dr["PhoneType"].ToString(),
                           Email = dr["Email"].ToString(),
                           ContactNo = dr["ContactNo"].ToString()
                       }).ToList();
            return contactList;
        }
        public List<EmergencyContact> GetAllEmergencyContactByCustomerIdandPlatform(Guid customerid,string Platform)
        {

            DataTable dt = _EmergencyContactDataAccess.GetAllEmergencyContactByCustomerIdAndPlatform(customerid,Platform);
            List<EmergencyContact> contactList = new List<EmergencyContact>();
            contactList = (from DataRow dr in dt.Rows
                           select new EmergencyContact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CustomerId = (Guid)dr["CustomerId"],
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               RelationShip = dr["RelationShip"].ToString(),
                               CompanyId = (Guid)dr["CompanyId"],
                               HasKey = dr["HasKey"].ToString(),
                               RelationShipVal = dr["RelationShipVal"].ToString(),
                               Phone = dr["Phone"].ToString(),
                               Platform = dr["Platform"].ToString(),
                               PhoneType = dr["PhoneType"].ToString(),
                               Email = dr["Email"].ToString(),
                               ContactNo = dr["ContactNo"].ToString()
                           }).ToList();
            return contactList;
        }
    }
}
