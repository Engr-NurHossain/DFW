using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
 public class AdditionalContactFacade : BaseFacade
    {
        public AdditionalContactFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerAdditionalContactDataAccess _CustomerAdditionalContactDataAccess
        {
            get
            {
                return (CustomerAdditionalContactDataAccess)_ClientContext[typeof(CustomerAdditionalContactDataAccess)];
            }
        }
        public CustomerAdditionalContact GetContactByCustomerId(Guid customerId)
        {
            return _CustomerAdditionalContactDataAccess.GetByQuery(string.Format(" CustomerId ='{0}'", customerId)).FirstOrDefault();
        }
        public List<CustomerAdditionalContact> GetAllAdditionalContactByCustomerId(Guid customerId)
        {
            List<CustomerAdditionalContact> additionalContact = _CustomerAdditionalContactDataAccess.GetByQuery(string.Format(" CustomerId ='{0}'", customerId)).ToList();
            return additionalContact;
        }
        public bool HasUsedSecondaryCreditCheck(Guid CustomerId)
        {
            List<CustomerAdditionalContact> contactList = _CustomerAdditionalContactDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsCreditUsed = 1", CustomerId)).ToList();
            if (contactList != null && contactList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int InsertAdditionalContact(CustomerAdditionalContact additionalInfo)
        {
            return (int)_CustomerAdditionalContactDataAccess.Insert(additionalInfo);
        }

        public int UpdateAdditionalContact(CustomerAdditionalContact additionalInfo)
        {
            return (int)_CustomerAdditionalContactDataAccess.Update(additionalInfo);
        }

        public int DeleteAdditionalContact(int additionalInfo)
        {
            return (int)_CustomerAdditionalContactDataAccess.Delete(additionalInfo);
        }

        public CustomerAdditionalContact GetById(int Id)
        {
            return _CustomerAdditionalContactDataAccess.Get(Id);
        }
    }


}
