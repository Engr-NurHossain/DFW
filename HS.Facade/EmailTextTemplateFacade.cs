using HS.DataAccess;
using HS.Entities;
using HS.Entities.List;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class EmailTextTemplateFacade:BaseFacade
    {
        public EmailTextTemplateFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
      
        EmailTextTemplateDataAccess _EmailTextTemplateDataAccess
        {
            get
            {
                return (EmailTextTemplateDataAccess)_ClientContext[typeof(EmailTextTemplateDataAccess)];
            }
        }
        EmailTemplateDataAccess _EmailTemplateDataAccess
        {
            get
            {
                return (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            }
        }
        public EmailTextTemplate GetById(int value)
        {
            return _EmailTextTemplateDataAccess.Get(value);
        }
        public EmailTemplateList GetByQuery(String query)
        {
            return _EmailTemplateDataAccess.GetByQuery(query);
        }
        public List<EmailTextTemplate> GetAllEmailTemplateText()
        {
            return _EmailTextTemplateDataAccess.GetAll();
        }
        public EmailTextTemplate GetTextTemplateById(int value)
        {
            var result = _EmailTextTemplateDataAccess.Get(value);
            return result;
        }
        public bool UpdateTextTemplate(EmailTextTemplate cn)
        {
            return _EmailTextTemplateDataAccess.Update(cn) > 0;
        }
        public long InsertTextTemplate(EmailTextTemplate cn)
        {
            return _EmailTextTemplateDataAccess.Insert(cn);
        }

        public List<EmailTextTemplate> GetAllLeadEmailTextTemplateByCompanyIdAndLeadKey(Guid CompanyID)
        {
            return _EmailTextTemplateDataAccess.GetByQuery(string.Format("CompanyID = '{0}' AND Type = 'Lead'", CompanyID));
        }

    }
}
