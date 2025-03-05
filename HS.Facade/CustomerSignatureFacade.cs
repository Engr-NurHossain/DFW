
using HS.DataAccess;
using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.SMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class CustomerSignatureFacade :BaseFacade
    {
        CustomerSignatureDataAccess _CustomerSignatureDataAccess = null;


        public CustomerSignatureFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_CustomerSignatureDataAccess == null)
                _CustomerSignatureDataAccess = (CustomerSignatureDataAccess)_ClientContext[typeof(CustomerSignatureDataAccess)];
           
        }
        public CustomerSignatureFacade(string ConStr)
        {
            if (_CustomerSignatureDataAccess == null)
                _CustomerSignatureDataAccess = new CustomerSignatureDataAccess(ConStr);
            
        }
        public CustomerSignature GetCustomerSignatureByReferenceIdcharCustomerIdType(Guid customerId,string referenceCharId, string type)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and ReferenceIdnvarchar='{1}' and Type='{2}' order by Id desc", customerId, referenceCharId,type)).FirstOrDefault();
        }
       
        public bool InsertCustomerSignature(CustomerSignature cs)
        {
            return _CustomerSignatureDataAccess.Insert(cs) > 0;
        }
        public bool UpdateCustomerSignature(CustomerSignature cs)
        {
            return _CustomerSignatureDataAccess.Update(cs) > 0;
        }

        public CustomerSignature GetRecreateCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] = 'Recreate'", customerid)).FirstOrDefault();
        }


        public CustomerSignature GetFirstPageCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] = 'First Page'", customerid)).FirstOrDefault();
        }
        public CustomerSignature GetCommercialCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] = 'Commercial'", customerid)).FirstOrDefault();
        }
        public CustomerSignature GetCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }

        public bool DeleteAllSignatureByType(Guid customerId, string Type)
        {
            return _CustomerSignatureDataAccess.DeleteAllSignatureByType(customerId, Type);
        }
    }
}
