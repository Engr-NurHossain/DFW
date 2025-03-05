using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Linq;

namespace HS.DataAccess
{
	public partial class EmailTemplateDataAccess
	{
        public EmailTemplateDataAccess(string ConnectionStr):base(ConnectionStr) { }

        public EmailTemplate GetEmailTemplateByKeyAndCompanyId(string companyId, string key)
        {
            return  this.GetByQuery( string.Format(" Name='{0}' and CompanyId = '{1}' ",key, companyId )).FirstOrDefault();
              

        }
        public EmailTemplate GetEmailTemplateByTemplateKeyAndCompanyId(string companyId, string key)
        {
            return this.GetByQuery(string.Format(" TemplateKey='{0}' and CompanyId = '{1}' ", key, companyId)).FirstOrDefault();


        }
        public bool UpdateBCCEmailByCompanyId(string emailAddress, Guid CompanyId)
        {
            string sqlQuery = @"update EmailTemplate set BccEmail = '{0}' where CompanyId ='{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, emailAddress, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;

            }

        }
        public bool UpdateReplyEmailByCompanyId(string emailAddress, Guid CompanyId)
        {
            string sqlQuery = @"update EmailTemplate set ReplyEmail = '{0}' where CompanyId ='{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, emailAddress, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;

            }

        }
    }	
}
