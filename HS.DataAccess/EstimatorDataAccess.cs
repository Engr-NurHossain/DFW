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
	public partial class EstimatorDataAccess
	{
        public EstimatorDataAccess(string ConStr) : base(ConStr) { }
        public DataSet GetAllEstimatorDetailCountByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, EstimateFilter filter)
        {
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = "";
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}'
                                set @CompanyId = '{1}'

                                select 
                                emp.FirstName + ' '+ emp.LastName as CreatedByName,
                                _est.* 
                                INTO #tempestimator from Estimator _est
                                left join Employee emp on emp.UserId = _est.CreatedBy
                                where _est.CustomerId = @CustomerId
                                and _est.CompanyId = @CompanyId
                                and _est.Status != 'Init'
                                 
                                {2} 
                                {3}
                                order by _est.Id Desc
                                SELECT * FROM #tempestimator
                                SELECT COUNT(*) AS OpenCount                                FROM #tempestimator                                WHERE IsApproved = 0 and Status != 'Declined'
                                SELECT COUNT(*) AS AcceptedCount                                FROM #tempestimator                                WHERE IsApproved = 1 and Status != 'Declined'                                SELECT COUNT(*) AS DeclinedCount                                FROM #tempestimator                                WHERE Status = 'Declined'                                DROP TABLE #tempestimator";

            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                searchSql = string.Format(" and _est.EstimatorId like '%{0}%'", filter.SearchText);
            }
            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and _est.StartDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }

            //if (!string.IsNullOrWhiteSpace(filter.estimateStatus))
            //{
            //    if (filter.estimateStatus == "Open")
            //    {
            //        subquery = string.Format("and _est.Status = 'Open'");
            //    }
            //    else if (filter.estimateStatus == "Pending")
            //    {
            //        subquery = string.Format("and _est.Status = 'Pending'");
            //    }
            //    else if (filter.estimateStatus == "Accepted")
            //    {
            //        subquery = string.Format("and _est.Status = 'Accepted'");
            //    }
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId, searchSql, dateRange);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;//.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllEstimatorDetailListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, EstimateFilter filter)
        {
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = ""; 
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}'
                                set @CompanyId = '{1}'

                                select 
                                emp.FirstName + ' '+ emp.LastName as CreatedByName,
                                _est.* 
                                into #tempestimator from Estimator _est
                                left join Employee emp on emp.UserId = _est.CreatedBy
                                where _est.CustomerId = @CustomerId
                                and _est.CompanyId = @CompanyId
                                and _est.Status != 'Init'
                                {4}
                                {2} 
                                {3}
                                
                                select * from #tempestimator order by Id Desc
                                Select Count(*) As TotalCount from #tempestimator
								Drop table #tempestimator";

            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                searchSql = string.Format(" and _est.EstimatorId like '%{0}%'", filter.SearchText);
            }
            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and _est.StartDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.estimateStatus))
            {
                if (filter.estimateStatus == "Open")
                {
                    subquery = string.Format(" and _est.IsApproved = 0 and _est.Status != 'Declined'");
                }
                else if (filter.estimateStatus == "Declined")
                {
                    subquery = string.Format(" and _est.Status = 'Declined'");
                }
                else if (filter.estimateStatus == "Accepted")
                {
                    subquery = string.Format(" and _est.IsApproved = 1 and _est.Status != 'Declined'");
                }
            } 
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId, searchSql, dateRange, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;//.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllEstimatorListForDashboard(Guid companyId, EstimatorFilter filter, int pageno, int pagesize, string status, string overxprice, string startdate, string enddate)
        {
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = "";
            string statusquery = "";
            if (!string.IsNullOrWhiteSpace(overxprice) && overxprice != "undefined")
            {

                subquery += string.Format("and _est.TotalPrice >= '{0}'", overxprice);
            }
            if (!string.IsNullOrWhiteSpace(status) && status != "undefined")
            {
               
                statusquery += string.Format("and _est.Status = '{0}'",status);
            }
            if (!string.IsNullOrWhiteSpace(enddate) && !string.IsNullOrWhiteSpace(startdate) && startdate != "undefined" && enddate != "undefined")
            {
                var date = Convert.ToDateTime(enddate);
                var datemin = Convert.ToDateTime(startdate);
                dateRange += string.Format("and _est.EstimateDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(enddate) && enddate != "undefined")
            {
                var date = Convert.ToDateTime(enddate);
                dateRange += string.Format("and _est.EstimateDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(startdate) && startdate != "undefined")
            {
                var date = Convert.ToDateTime(startdate);
                dateRange += string.Format("and _est.EstimateDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            int pagestart = (pageno - 1) * pagesize;
            int pageend = pagesize;
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CompanyId ='{0}'
                              

                                select 
                                emp.FirstName + ' '+ emp.LastName as CreatedByName,
                                CASE 
                                WHEN cu.DBA !='' THEN cu.DBA
		                        WHEN cu.BusinessName !='' THEN cu.BusinessName
                                ELSE cu.FirstName +' '+cu.LastName
                                END as [CustomerName],
                                _est.* 
                                 into #estimatorData
                                from Estimator _est
                                left join Employee emp on emp.UserId = _est.CreatedBy
                                left join Customer cu on cu.CustomerId = _est.CustomerId
                                where 
                               _est.CompanyId = @CompanyId
                                and _est.Status != 'Init'
                                {9}
                                {1} 
                                {2}
                                {3}
                                     order by _est.Id Desc

	                       select * into #estimatorIdData from #estimatorData where Id> 0 
								select top({6}) * from #estimatorIdData
								where Id not in (Select TOP ({7}) Id from #estimatorData #es)
                                order by EstimateDate Desc
                                select Count(Id) As TotalCount from #estimatorData
                                select Count(Id) as CountEstimator from #estimatorData 
                                   drop table #estimatorData
                                   drop table #estimatorIdData

                                 ";

            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                searchSql = string.Format(" and _est.EstimatorId like '%{0}%'", filter.SearchText);
            }
            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and _est.StartDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.estimateStatus))
            {
                if (filter.estimateStatus == "Open")
                {
                    subquery = string.Format("and _est.Status = 'Open'");
                }
                else if (filter.estimateStatus == "Pending")
                {
                    subquery = string.Format("and _est.Status = 'Pending'");
                }
                else if (filter.estimateStatus == "Accepted")
                {
                    subquery = string.Format("and _est.Status = 'Accepted'");
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, searchSql, dateRange, subquery, NameSql, pageno, pagesize, pagestart, pageend, statusquery);
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
        public DataSet GetAllChildEstimatorDetailByEstimatorId(string EstimatorId)
        {
            string sqlQuery = @" select iif(SS.CompanyName is null, 'Supplier', SS.CompanyName) as SupplierVal, ET.Name as CategoryVal,ED.* from EstimatorDetail ED
                                left join EquipmentType ET on ET.Id =  ED.CategoryId
                                left join Supplier SS on SS.SupplierId = ED.SupplierId
                                Where EstimatorId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, EstimatorId);
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
        public DataTable GetEstimatorList(Guid CustomerId, Guid CompanyId, int[] IdList, EstimatorFilter filter)
        {
            //DataTable dt = new DataTable();
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = "";
            string InIdListFilter = "";
            if (IdList != null && IdList.Length > 0)
            {
                string Ids = "";
                foreach (int id in IdList)
                {
                    Ids += id + ",";
                }
                Ids += "0";
                InIdListFilter = "And _est.Id in(" + Ids + ")";
            }
            
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}'
                                set @CompanyId = '{1}'

                                select 
                                _est.EstimatorId as [Estimator],
								_est.[Status],
                                emp.FirstName + ' '+ emp.LastName +' '+ CONVERT(nvarchar, _est.LastUpdatedDate, 101) as [User/Timestamp],
                                _est.TotalPrice as [Total]
                                --emp.FirstName + ' '+ emp.LastName as CreatedByName,
                                --_est.* 
                                from Estimator _est
                                left join Employee emp on emp.UserId = _est.CreatedBy
                                where _est.CustomerId = @CustomerId
                                and _est.CompanyId = @CompanyId
                                and _est.Status != 'Init' 
                                {2}
                                {3}
                                order by _est.Id Desc
                           ";
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                searchSql = string.Format(" and _est.EstimatorId like '%{0}%'", filter.SearchText);
            }
            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and _est.StartDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.estimateStatus))
            {
                if (filter.estimateStatus == "Open")
                {
                    subquery = string.Format("and _est.Status = 'Open'");
                }
                else if (filter.estimateStatus == "Pending")
                {
                    subquery = string.Format("and _est.Status = 'Pending'");
                }
                else if (filter.estimateStatus == "Accepted")
                {
                    subquery = string.Format("and _est.Status = 'Accepted'");
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, InIdListFilter, dateRange);
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
            //return dt;
        }

        public DataSet ExportEstimatorByEstimatorId(string EstimatorId)
        {
            string sqlQuery = @"(Select cust.BusinessName As [Business Name],cust.CustomerNo As [Account Number],est.EstimatorId [Estimator Number] from Estimator est
                                    Left join Customer cust on est.CustomerId = cust.CustomerId 

                                    Where est.EstimatorId = '{0}')

                                    (Select estdtl.PartDescription  AS [Equipment],estdtl.Qunatity,(estdtl.UnitCost + (estdtl.UnitCost * (estdtl.OverheadRate / 100))) As [Unit Price],estdtl.TotalPrice AS [Total],estdtl.UnitCost as [Unit Cost], estdtl.Qunatity [Qunatity 2],estdtl.TotalCost As [Total Cost],(estdtl.TotalPrice - estdtl.TotalCost)  As [Profit] from EstimatorDetail  estdtl 
                                    Left join Equipment eqip on estdtl.EquipmentId = eqip.EquipmentId
                                    Where estdtl.EstimatorId= '{0}')
                                  ";
            try
            {
                sqlQuery = string.Format(sqlQuery, EstimatorId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return dsResult;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            //return dt;
        }

        //public DataTable ExportEstimatorByEstimatorId(string EstimatorId)
        //{
        //    string sqlQuery = @"
        //                        (Select cust.BusinessName As [Business Name], cust.CustomerNo As [Account Number], est.EstimatorId As [Estimator Number]
        //                        from Estimator est
        //                        Left join Customer cust on est.CustomerId = cust.CustomerId
        //                        Where est.EstimatorId = '{0}')

        //                        (Select estdtl.PartDescription,estdtl.Qunatity,(estdtl.UnitCost * Overhead) As [Unit Price],estdtl.TotalPrice,estdtl.Qunatity [Qunatity 2],estdtl.TotalCost,estdtl.Profit from Estimator  est 
        //                         Left join  EstimatorDetail estdtl on est.EstimatorId = estdtl.EstimatorId
        //                        Where est.EstimatorId = '{0}')
        //                        ";

        //    try
        //    {
        //        // Format the SQL query with the EstimatorId parameter
        //        sqlQuery = string.Format(sqlQuery, EstimatorId);

        //        // Execute the query and get the DataSet
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);

        //            // Create a new DataTable for the merged result
        //            DataTable mergedTable = new DataTable();

        //            // Add the columns from the first table (Table[0])
        //            foreach (DataColumn column in dsResult.Tables[0].Columns)
        //            {
        //                mergedTable.Columns.Add(column.ColumnName, column.DataType);
        //            }

        //            // Add the columns from the second table (Table[1])
        //            foreach (DataColumn column in dsResult.Tables[1].Columns)
        //            {
        //                mergedTable.Columns.Add(column.ColumnName, column.DataType);
        //            }

        //            // Add rows from the first table (Table[0])
        //            foreach (DataRow row in dsResult.Tables[0].Rows)
        //            {
        //                DataRow newRow = mergedTable.NewRow();

        //                // Copy the values from the first table to the new row
        //                for (int i = 0; i < dsResult.Tables[0].Columns.Count; i++)
        //                {
        //                    newRow[i] = row[i];
        //                }

        //                // Add empty values for the second table's columns
        //                for (int i = dsResult.Tables[0].Columns.Count; i < mergedTable.Columns.Count; i++)
        //                {
        //                    newRow[i] = DBNull.Value;
        //                }

        //                mergedTable.Rows.Add(newRow);
        //            }

        //            // Add rows from the second table (Table[1])
        //            foreach (DataRow row in dsResult.Tables[1].Rows)
        //            {
        //                DataRow newRow = mergedTable.NewRow();

        //                // Add empty values for the first table's columns
        //                for (int i = 0; i < dsResult.Tables[0].Columns.Count; i++)
        //                {
        //                    newRow[i] = DBNull.Value;
        //                }

        //                // Copy the values from the second table to the new row
        //                for (int i = dsResult.Tables[1].Columns.Count; i < mergedTable.Columns.Count; i++)
        //                {
        //                    newRow[i] = row[i - dsResult.Tables[0].Columns.Count];
        //                }

        //                mergedTable.Rows.Add(newRow);
        //            }

        //            return mergedTable;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception, e.g., logging it
        //        Console.WriteLine("Error: " + ex.Message);
        //        return null;
        //    }
        //}


        public bool DeleteEstimatorByEstimatorId(string estimatorId)
        {
            string SqlQuery = @"delete from Estimator where EstimatorId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, estimatorId);
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

    }	
}
