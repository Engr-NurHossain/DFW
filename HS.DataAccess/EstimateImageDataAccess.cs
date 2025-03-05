using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class EstimateImageDataAccess
	{
        public EstimateImageDataAccess(string ConnectionStr):base(ConnectionStr) { }

        public DataTable GetEstimateImageListByInvoiceId(string invoiceId)
        {

            string sqlQuery = @"select * from EstimateImage as EsImage

                                left join Invoice inv on EsImage.InvoiceId = inv.InvoiceId
                            
                                where EsImage.InvoiceId ='{0}' 
                                
                                order by EsImage.id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, invoiceId);
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
        public bool DeleteEstimateImageByKnowledgeId(string id, bool IsDocument)
        {
            string condiyional = "";
            if (IsDocument)
            {
                condiyional = "and IsDocument = 1";
            }
            else
            {
                condiyional = "and IsDocument = 0";
            }
            string SqlQuery = @"delete from EstimateImage where InvoiceId = {0} {1}";
            try
            {
                SqlQuery = string.Format(SqlQuery, id, condiyional);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        } 
    }
}
