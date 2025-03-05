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
	public partial class PaymentProfileCustomerDataAccess
	{
        public PaymentProfileCustomerDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllPaymentProfileByType(Guid companyId, Guid customerid,string type, bool AllowOnlyACHAndCC = false)
        {
            string sqlQuery = @"select 
                                ISNULL(lp.DisplayText,ppc.Type) as Type,
                                ppc.PaymentInfoId,
                                ISNULL(lp.DataValue,'') as DataValue 
                                Into #TempPProfile
                                from PaymentProfileCustomer ppc
                                LEFT JOIN Lookup lp on lp.DataValue=ppc.Type and lp.DataKey='{1}'
                                where CustomerId='{0}'

                                select * from #TempPProfile
                                ";

            if (AllowOnlyACHAndCC)
            {
                sqlQuery += @" where DataValue !='Cash' 
                                AND Type != 'Cash' 
                                AND DataValue !='Invoice'
                                AND CHARINDEX( 'CHK', Type) !=1; ";
            }
            sqlQuery += "drop table #TempPProfile";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid,type);
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
