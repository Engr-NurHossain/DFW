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
    public class ContactFacade:BaseFacade
    {
        public ContactFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        ContactDataAccess _ContactDataAccess
        {
            get
            {
                return (ContactDataAccess)_ClientContext[typeof(ContactDataAccess)];
            }
        }
        UserContactDataAccess _UserContactDataAccess
        {
            get
            {
                return (UserContactDataAccess)_ClientContext[typeof(UserContactDataAccess)];
            }
        }
        public ContactModel GetContacts(ContactFilter filter)
        {
            return _ContactDataAccess.GetContacts(filter);
        }
        public List<Contact> GetFilteredContacts(ContactFilter filter)
        {
            return _ContactDataAccess.GetFilteredContacts(filter);
        }
        public Contact GetContactbyContactId(Guid ContactId)
        {
            return _ContactDataAccess.GetByQuery(string.Format(" ContactId ='{0}'", ContactId)).FirstOrDefault();
        }
        public List<Contact> GetAllContactListByIds(string IdList)
        {
            DataTable dt = _ContactDataAccess.GetAllContactListByIds(IdList);
            List<Contact> ContactList = new List<Contact>();
            ContactList = (from DataRow dr in dt.Rows
                           select new Contact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               Work = dr["Work"].ToString(),
                               Mobile = dr["Mobile"].ToString(),
                               Email = dr["Email"].ToString(),
                               Name = dr["Name"].ToString(),
                               CreatedByName = dr["CreatedByName"].ToString(),
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                           }).ToList();


            return ContactList;
        }

        public List<Contact> GetAllContactListByContactIds(string IdList)
        {
            DataTable dt = _ContactDataAccess.GetAllContactListByContactIds(IdList);
            List<Contact> ContactList = new List<Contact>();
            ContactList = (from DataRow dr in dt.Rows
                           select new Contact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               Work = dr["Work"].ToString(),
                               Mobile = dr["Mobile"].ToString(),
                               Email = dr["Email"].ToString(),
                               Name = dr["Name"].ToString(),
                               CreatedByName = dr["CreatedByName"].ToString(),
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                           }).ToList();


            return ContactList;
        }
        public List<UserContact> GetAllUserContactListByContactId(Guid ContactId)
        {
            DataTable dt = _ContactDataAccess.GetAllUserContactListByContactId(ContactId);
            List<UserContact> ContactList = new List<UserContact>();
            ContactList = (from DataRow dr in dt.Rows
                           select new UserContact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               UserId = (Guid)dr["UserId"],
                               ContactId = (Guid)dr["ContactId"],
                               UserType = dr["UserType"].ToString(),
                               OpportunityName = dr["OpportunityName"].ToString(),
                               UserFirstName = dr["UserFirstName"].ToString(),
                               UserLastName = dr["UserLastName"].ToString(),
                               UserBusinessName = dr["UserBusinessName"].ToString(),
                               UserIntId = dr["UserIntId"] != DBNull.Value ? Convert.ToInt32(dr["UserIntId"]) : 0,
                            
                           }).ToList();


            return ContactList;
        }
        
        public List<Contact> GetAllContactbyContactId(Guid ContactId)
        {
            return _ContactDataAccess.GetByQuery(string.Format(" ContactId ='{0}'", ContactId)).ToList();
        }
        public List<Contact> GetAllContactbyContactOwner(Guid ContactOwner)
        {
            return _ContactDataAccess.GetByQuery(string.Format(" ContactOwner ='{0}'", ContactOwner)).ToList();
        }
        public bool ExistEmailorCellNo(string Email,string Mobile,string work)
        {
            return _ContactDataAccess.ExistEmailorCellNo(Email, Mobile,work);
        }
        public Contact GetContactsByContactOwner(Guid ContactOwner)
        {
            return _ContactDataAccess.GetByQuery(string.Format(" ContactOwner ='{0}'", ContactOwner)).FirstOrDefault();
        }
        public List<Contact> GetAllContacts()
        {
            return _ContactDataAccess.GetAll();
        }
        public Contact GetContactById(int id)
        {
            return _ContactDataAccess.Get(id);
        }
        public Contact GetContactInfoById(int id)
        {
            DataTable dt = _ContactDataAccess.GetContactInfoById(id);
            List<Contact> ContactList = new List<Contact>();
            ContactList = (from DataRow dr in dt.Rows
                           select new Contact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               ContactOwner = (Guid)dr["ContactOwner"],
                               ContactId = (Guid)dr["ContactId"],
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               Email = dr["Email"].ToString(),
                               Mobile = dr["Mobile"].ToString(),
                               Work = dr["Work"].ToString(),
                               ContactOwnerVal = dr["ContactOwnerVal"].ToString(),

                               Facebook = dr["Facebook"].ToString(),
                               Twitter = dr["Twitter"].ToString(),
                               Instagram = dr["Instagram"].ToString(),
                               Suffix = dr["Suffix"].ToString(),
                               Title = dr["Title"].ToString(),

                               Role = dr["Role"].ToString(),
                               LinkedIN = dr["LinkedIN"].ToString(),
                               CreatedBy = (Guid)dr["CreatedBy"],
                               ContactType = dr["ContactType"].ToString(),
                               Notes = dr["Notes"].ToString(),
                               Ext = dr["Ext"].ToString(),
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                           }).ToList();


            return ContactList.FirstOrDefault();
        }
        public bool InsertContacts(Contact contacts)
        {
            return _ContactDataAccess.Insert(contacts) > 0;
        }
        public bool InsertUserContacts(UserContact UserContacts)
        {
            return _UserContactDataAccess.Insert(UserContacts) > 0;
        }
        public bool DeleteContactsByContactId(Guid contactId)
        {
            return _ContactDataAccess.DeleteUserContactByContactId(contactId);
        }
        public long DeleteContact(int Id)
        {
            return _ContactDataAccess.Delete(Id);
        }
        public UserContact GetUserContactsByCustomerId(Guid CustomerId)
        {
            return _UserContactDataAccess.GetByQuery(string.Format(" UserId ='{0}'", CustomerId)).FirstOrDefault();
        }
        public List<UserContact> GetAllUserContactsByCustomerId(Guid CustomerId)
        {
            return _UserContactDataAccess.GetByQuery(string.Format(" UserId ='{0}'", CustomerId)).ToList();
        }
        public List<UserContact> GetAllUserContactsByContactId(Guid ContactId)
        {
            return _UserContactDataAccess.GetByQuery(string.Format(" ContactId ='{0}'", ContactId)).ToList();
        }
        public DataTable GetAllContactList(string query)
        {
            return _ContactDataAccess.GetAllContactList(query);
        }
        public bool UpdateContacts(Contact contacts)
        {
            return _ContactDataAccess.Update(contacts) > 0;
        }
        public DataTable GetAllContactForExport()
        {
            return _ContactDataAccess.GetAllContactForExport();
        }
        public DataTable GetAllContactDatabaseForExport()
        {
            return _ContactDataAccess.GetAllContactDatabaseForExport();
        }
        public List<Contact> GetContactsBySearchKey(string key, string employeeTag, Guid empid, string type)
        {
            return _ContactDataAccess.GetContactsBySearchKey(key, employeeTag, empid, type);
        }
    }
}
