using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class InvoiceNoteDataAccess
	{
		public DataTable GetAllInvoiceNoteByInvoiceIdAndCompanyId (int InvId,Guid ComId)
        {
            string sqlQuery = @"   
                    select inNote.*, 
                    case when emp.firstname != ''
                    then  emp.firstname + ' ' +emp.lastname 
                    else
                    cus.firstname +' '+ cus.lastname
                    end as AddedByText
                    from invoicenote inNote 
                    left join employee emp 
                    on emp.UserId = inNote.AddedBy
                    left join Customer cus
                    on cus.CustomerId = inNote.AddedBy
                    where inNote.companyid = '{0}'
                    and inNote.invoiceid = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, ComId, InvId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
