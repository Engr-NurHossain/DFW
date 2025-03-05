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
	public partial class CustomerRouteDataAccess
	{
        public CustomerRouteDataAccess(string ConnectionString) : base(ConnectionString)
		{

        }
        public DataSet GetAllCustomerRoutes(int PageNo, int PageSize, Guid UserId)
        {
            string User = "";
            if (UserId != new Guid())
            {
                User = string.Format("WHERE ER.UserId = '{0}'", UserId);
            }
            string sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
								DECLARE @pageno int
	                            DECLARE @pagesize int

                                SET @pageno = {0}
								SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                SELECT Distinct
                                CR.Id 
                                ,CR.RouteId
                                ,CR.Name
                                ,(select top(1) LastVisit from CustomerRoute where RouteId = CR.RouteId order by Id desc) as LastVisit
                                
                                INTO #CustomerData
                                FROM GeeseRoute CR
                                Left Join EmployeeRoute ER on ER.RouteId = CR.RouteId
                                {2}

                                SELECT TOP (@pagesize)
                                  *  Into #CustomerResultData
                                FROM #CustomerData

                                where Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData #cd)

                                select * from #CustomerResultData

                                select count(*) [TotalCount]
                                from #CustomerData

                                DROP TABLE #CustomerData
                                DROP TABLE #CustomerResultData";
            try
            {
                sqlQuery = string.Format(sqlQuery,PageNo, PageSize,User);
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

        public DataSet GetAllCustomerByRouteId(Guid CustomerId, int PageNo, int PageSize)
        {
            string sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
								DECLARE @pageno int
	                            DECLARE @pagesize int

                                SET @pageno = {0}
								SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct 
                                CR.Id
                                ,CR.CustomerId
                                ,Cus.FirstName +' '+ Cus.LastName as Name
                                ,Cus.EmailAddress as Email
                                ,Cus.PrimaryPhone as Phone
                                ,(select Count(*) from CustomerFile CF where CustomerId = CR.CustomerId and CF.GeeseFileType = 'Media') as TotalMedia
                                ,(select Count(*) from CustomerFile CF2 where CustomerId = CR.CustomerId and CF2.GeeseFileType ='Note') as TotalNote
                                ,CE.GeeseLead
                                ,Cus.Street
                                ,Cus.City
                                ,Cus.[State]
                                ,Cus.ZipCode
                                ,Cus.ProfileImage
                                ,(select top(1) [GeeseCount] from CustomerCheckLog CL where CL.CustomerId = CR.CustomerId order by id desc) as GeeseCount
                                ,(select top(1) CASE WHEN CheckType = 'In' THEN 'True'
									WHEN CheckType = '' THEN 'False'
									WHEN CheckType = 'Out' THEN 'False'
									WHEN CheckType is NULL THEN 'False'
									ELSE 'False' end from CustomerCheckLog CL where CL.CustomerId = CR.CustomerId order by id desc) as IsCheckIn
                                ,CR.LastVisit
                                ,(select top(1) [Filename] from CustomerFile CF where CustomerId = CR.CustomerId and CF.GeeseFileType = 'Media' order by CF.Id desc) as LastMedia
                                ,(select top(1) FileDescription from CustomerFile CF where CustomerId = CR.CustomerId and CF.GeeseFileType ='Note' order by CF.Id desc) as LastNote
                                INTO #CustomerData
                                from CustomerRoute CR
                                left join Customer Cus on Cus.CustomerId = CR.CustomerId
                                left join CustomerExtended CE on CE.CustomerId = Cus.CustomerId
                                Where CR.CustomerId ='{2}'

                                SELECT TOP (@pagesize)
                                  *  Into #CustomerResultData
                                FROM #CustomerData
                                where Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData #cd)

                                select * from #CustomerResultData

                                select CustomerId,
								[FileName] as Url
								,Uploadeddate as UploadedDate
								,FileDescription as Note
                                ,Emp.FirstName +' '+ Emp.LastName as Assigner
								from CustomerFile CFF
                                left join Employee Emp on Emp.UserId = CFF.CreatedBy
								where GeeseFileType = 'Media'
								and CFF.CustomerId  = '{2}'

								select CustomerId,
								Uploadeddate as UploadedDate
								,FileDescription as Note
                                ,Emp.FirstName +' '+ Emp.LastName as Assigner
								from CustomerFile CFF
                                left join Employee Emp on Emp.UserId = CFF.CreatedBy
								where GeeseFileType ='Note'
								and CFF.CustomerId = '{2}'

                                DROP TABLE #CustomerData
                                DROP TABLE #CustomerResultData";
            try
            {
                sqlQuery = string.Format(sqlQuery, PageNo, PageSize, CustomerId);
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

        public DataSet GetCustomerDetailsByCustomerId(Guid CustomerId, DateTime? StartDate, DateTime? EndDate)
        {
            string DateQ = "";
            if (StartDate != null && EndDate != null && StartDate != new DateTime() && EndDate != new DateTime())
            {
                string daystart = StartDate.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string dayend = EndDate.Value.ToString("yyyy-MM-dd 23:59:59.999");
                DateQ = string.Format("and CL.CreatedDate between '{0}' and '{1}'", daystart, dayend);
            }
            string sqlQuery = @"select
                                Cus.Id
                                ,Cus.CustomerId
                                ,Cus.FirstName +' '+ Cus.LastName as Name
                                ,Emp.FirstName +' '+ Emp.LastName as UserName
                                ,CE.GeeseLead
                                ,CL.GeeseCount
                                ,iif(CL.CheckType = 'In', 'true', 'false') as IsCheckIn
                                ,CR.RouteId
                                ,CL.CheckInTime as LastVisit
                                ,CL.CheckInTime
								,CL.CheckOutTime
                                INTO #CustomerData
                                from CustomerCheckLog CL
								left join Customer Cus on Cus.CustomerId = CL.CustomerId
								left join CustomerRoute CR on Cus.CustomerId = CR.CustomerId
                                left join CustomerExtended CE on CE.CustomerId = Cus.CustomerId
                                Left join Employee Emp on Emp.UserId = Cl.UserId
                                Where Cus.CustomerId ='{0}'
                                {1}
                                SELECT * Into #CustomerResultData
                                FROM #CustomerData

                                select * from #CustomerResultData order by CheckInTime Desc

                                DROP TABLE #CustomerData
                                DROP TABLE #CustomerResultData";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, DateQ);
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

        public DataTable GetAllCustomerIdByRouteId(Guid RouteId)
        {
            string sqlQuery = @"select CustomerId from CustomerRoute Where RouteId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, RouteId);
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
        public DataTable GetAllCustomerIdByUserId(Guid UserId)
        {
            string sqlQuery = @"select CustomerId from CustomerRoute Where UserId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
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
        public DataSet GetCustomerListByRouteId(Guid RouteId)
        {
            string sqlQuery = @"SELECT
                                Cus.Id 
                                ,CR.RouteId
                                ,CR.Name
                                ,Cus.Firstname +' '+ Cus.Lastname as CustomerName
                                FROM Customer cus
                                left join CustomerRoute CR on Cus.CustomerId = CR.CustomerId 
                                WHERE CR.RouteId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, RouteId);
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
    }	
}
