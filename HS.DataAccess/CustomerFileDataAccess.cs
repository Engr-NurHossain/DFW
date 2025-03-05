using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class CustomerFileDataAccess
    {
        public CustomerFileDataAccess(string ConnectionStr) : base(ConnectionStr) { }
        public bool ReseedCustomerFileTable()
        {
            string SqlQuery = @"Delete from CustomerFile
                                DBCC CHECKIDENT('CustomerFile', RESEED, 0)";
            try
            {
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

        public DataTable GetAllCustomerFileNameByCustomerId(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select _cst.Filename as CustomerFileName,
                                _cst.FileDescription as CustomerFileDescription
                                from CustomerFile _cst
                                where _cst.CustomerId = '{0}' and _cst.CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId);
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

        public DataTable GetAllCustomerAWSFileCustomerId(List<int> CustomerIdList, Guid CompanyId) /// Added by Mayur Rokade 19 May 2020
        {
            string IDQuery="";
            DataSet dsResult = new DataSet();

            for (int i = 0; i < CustomerIdList.Count; i++)
            {
                IDQuery = IDQuery + CustomerIdList[i].ToString() + "','";

            }
            IDQuery = IDQuery.Substring(0, IDQuery.LastIndexOf(','));
            IDQuery = IDQuery.Substring(0, IDQuery.LastIndexOf('\''));

            string sqlQuery = @"select 
                                _cst.Id,
                                _cst.Filename,
                                _cst.FileDescription,
                                _cst.AWSProcessStatus,
                                _cst.WMStatus,
                                _cst.AWSUploadTS,
                                Customer.Id as IntCustID
                                from CustomerFile _cst
                                INNER JOIN Customer ON _cst.CustomerID = Customer.CustomerId
                                where Customer.Id In ('{0}') and _cst.CompanyId = '{1}' 
                                and LOWER(RIGHT(_cst.Filename,3))='pdf'
                                and ( _cst.WMStatus='Pending' or  _cst.WMStatus='Processing')";
            try
            {
                sqlQuery = string.Format(sqlQuery, IDQuery, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }
        public DataSet GetMediaANdNoteListByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"select
                                FileDescription as Note,
                                [Filename] as [Path],
                                FileFullName as [Address],
                                Emp.FirstName +' '+ Emp.LastName as Assigner,
                                FORMAT(CF.CreatedDate,'MM/dd/yyyy') as CreatedDate 
                                from CustomerFile CF
                                Left Join Employee Emp on Emp.UserId = CF.CreatedBy
                                where CF.CustomerId ='{0}' and GeeseFileType = 'Media' and [Filename] != ''

                                select
                                FileDescription as Note,
                                Emp.FirstName +' '+ Emp.LastName as Assigner,
                                FORMAT(CF.CreatedDate,'MM/dd/yyyy') as CreatedDate
                                from CustomerFile CF
                                Left Join Employee Emp on Emp.UserId = CF.CreatedBy
                                where CF.CustomerId ='{0}' and GeeseFileType = 'Note' and FileDescription !=''";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllFilesByCustomerIdAndCompanyId(Guid customerId, string SearchText, bool IsActive)//testobayedq
        {
            string SearchQuery = "";
            string IsActiveQuery = "";
            if (!string.IsNullOrEmpty(SearchText) && SearchText!="undefind")
            {
                SearchQuery = string.Format(" And cf.FileDescription like '%{0}%'", SearchText);
            }
            if (IsActive == true)
            {
                IsActiveQuery = " And cf.IsActive = 1";
            }
            else if (IsActive == false)
            {
                IsActiveQuery = " And cf.IsActive = 0";
            }
            string sqlQuery = @"select 
                                cf.*,
                                custque.ExpirationDate,
                                emp.FirstName+' '+emp.LastName as CreatedName
                                from CustomerFile cf
                                LEFT JOIN Employee emp on emp.UserId=cf.CreatedBy
                                LEFT JOIN CustomerCancellationQueue custque on cf.FileId = custque.FileId
                                where  cf.CustomerId='{0}' {1} {2} order by cf.UploadedDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, SearchQuery, IsActiveQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllFilesForWMByCustomerIdAndCompanyId(List<int> CustomerIdList, Guid CompanyId)
        {
            string IDQuery = "";
            DataSet dsResult = new DataSet();

            for (int i = 0; i < CustomerIdList.Count; i++)
            {
                IDQuery = IDQuery + CustomerIdList[i].ToString() + "','";

            }
            IDQuery = IDQuery.Substring(0, IDQuery.LastIndexOf(','));
            IDQuery = IDQuery.Substring(0, IDQuery.LastIndexOf('\''));

            string sqlQuery = @"select 
                                _cst.*,
                                Customer.Id as IntCustID
                                from CustomerFile _cst
                                INNER JOIN Customer ON _cst.CustomerID = Customer.CustomerId
                                where Customer.Id In ('{0}') and _cst.CompanyId = '{1}' 
                                and LOWER(RIGHT(_cst.Filename,3))='pdf'
                                and ( _cst.WMStatus='Pending' or  _cst.WMStatus='Processing')";
            try
            {
                sqlQuery = string.Format(sqlQuery, IDQuery, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    dsResult = GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return dsResult;
        }

    }
}
