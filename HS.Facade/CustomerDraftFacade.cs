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
    public class CustomerDraftFacade : BaseFacade
    {
        public CustomerDraftFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }


        CustomerDraftDataAccess _CustomerDraftDataAccess
        {
            get
            {
                return (CustomerDraftDataAccess)_ClientContext[typeof(CustomerDraftDataAccess)];
            }
        }
        CustomerReferDataAccess _CustomerReferDataAccess
        {
            get
            {
                return (CustomerReferDataAccess)_ClientContext[typeof(CustomerReferDataAccess)];
            }
        }

        public CustomerDraft GetById(int id)
        {
            return _CustomerDraftDataAccess.Get(id);
        }
        public CustomerRefer GetCustomerReferById(int id)
        {
            return _CustomerReferDataAccess.Get(id);
        }
        public CustomerDraft GetCustomerDraftByCustomerNo(string customerNo)
        {
            return _CustomerDraftDataAccess.GetByQuery(string.Format(" CustomerNo ='{0}'", customerNo)).FirstOrDefault();
        }

        public int InsertCustomerDraftAndReturnId(CustomerDraft customer)
        {
            return (int)_CustomerDraftDataAccess.Insert(customer);
        }
        public int InsertCustomerRefer(CustomerRefer customer)
        {
            return (int)_CustomerReferDataAccess.Insert(customer);
        }
        public List<CustomerRefer> GetAllReferedFriend()
        {
            return _CustomerReferDataAccess.GetAll();
        }
        public bool UpdateCustomerDraft(CustomerDraft customer)
        {
            return _CustomerDraftDataAccess.Update(customer) > 0;
        }
        public bool UpdateCustomerRefer(CustomerRefer customer)
        {
            return _CustomerReferDataAccess.Update(customer) > 0;
        }
        public CustomerDraft GetCustomerDraftByCustomerId(Guid customerid)
        {
            return _CustomerDraftDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }
    }
}
