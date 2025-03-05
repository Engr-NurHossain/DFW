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
	public partial class FileTemplateDataAccess
	{
        public FileTemplateDataAccess() { }
        public FileTemplateDataAccess(string ConnectionStr):base(ConnectionStr) { }

        public DataTable GetAllTemplateForDropdown()
        {
            string sqlQuery = @"select Id,FileName
                                from FileTemplate
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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
        public DataTable GetTemplateWithoutPermissionForDropdown()
        {
            string sqlQuery = @"select Id,FileName
                               from FileTemplate
                               where FileName!= 'Smart Agreement DFW' and FileName !='Cancellation' and FileName !='Service Call Completion Checklist' and FileName != 'Installation Completion Checklist'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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
        //public DataTable GetAllFileTemplateListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId)
        //{
        //    string sqlQuery = @"Declare @CustomerId uniqueidentifier
        //                        Declare @CompanyId uniqueidentifier
        //                        set @CustomerId ='{0}' 
        //                        set @CompanyId = '{1}'

        //                        select _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName
        //                        ,_Booking.*
        //                        ,(select top 1 AddedDate from CustomerAgreement where InvoiceId = _Booking.BookingId and CompanyId = @CompanyId order by id desc) as CustomerViewedTime
        //                        ,(select top 1 Type from CustomerAgreement where InvoiceId = _Booking.BookingId and CompanyId = @CompanyId order by id desc) as CustomerViewedType
        //                        ,emp.FirstName + ' ' + emp.LastName as UserNum

        //                        from Booking _Booking
        //                        left join Customer _Customer 
        //                        on _Booking.CustomerId = _Customer.CustomerId
        //                        left join Employee emp
        //                        on emp.UserId = _Booking.CreatedBy

        //                        where _Booking.CompanyId =  @CompanyId
        //                        and _Booking.CustomerId = @CustomerId
        //                        and _Booking.Status != 'Init'
        //                        order by _Booking.Id Desc  ";
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, CustomerId, companyId);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
    
}
