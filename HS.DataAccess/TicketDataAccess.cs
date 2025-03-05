using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace HS.DataAccess
{
    public partial class TicketDataAccess
    {
        public TicketDataAccess(string ConStr) : base(ConStr) { }

        public DataSet GetTicketListByCustomerIdAndFilter(TicketFilter TicketFilters, string techid)
        {
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string techquery = "";
            string techqueryjoin = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                if (Regex.IsMatch(TicketFilters.SearchText, @"\d"))
                {
                    searchQuery += string.Format("and tk.Id = {0}", TicketFilters.SearchText);
                }
                else
                {
                    searchQuery += string.Format("and (CONVERT(nvarchar(11), tk.CompletionDate, 101) = '{0}' or tk.[Status] like '%{0}%')", TicketFilters.SearchText);
                }
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1")
            {
                ticketStatusQuery = string.Format("and tk.[Status] ='{0}'", TicketFilters.TicketStatus);
            }
            #endregion

            #region Assigned
            if (TicketFilters.Assigned != new Guid())
            {
                assignedQuery = string.Format("and (emp.UserId = '{0}' or _tu.UserId = '{0}')", TicketFilters.Assigned);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1")
            {
                ticketTypeQuery = string.Format("and tk.TicketType ='{0}'", TicketFilters.TicketType);
            }
            #endregion

            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedBy = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and emp.UserId = '{0}'", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedBy = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and emp.UserId = '{0}'", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedBy != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and emp.UserId = '{0}'", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] desc";
                    subquery1 = "order by [TicketType] desc";
                }
                else if (TicketFilters.order == "ascending/description")
                {
                    subquery = "order by #TicketData.[Description] asc";
                    subquery1 = "order by [Description] asc";
                }
                else if (TicketFilters.order == "descending/createdby")
                {
                    subquery = "order by #TicketData.[CreatedBy] desc";
                    subquery1 = "order by [CreatedBy] desc";
                }
                else if (TicketFilters.order == "ascending/assignto")
                {
                    subquery = "order by #TicketData.[AssignTo] asc";
                    subquery1 = "order by [AssignTo] asc";
                }
                else if (TicketFilters.order == "descending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/status")
                {
                    subquery = "order by #TicketData.[Status]  asc";
                    subquery1 = "order by Status asc";
                }

            }
            else
            {
                subquery = "order by #TicketData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(techid))
            {
                techquery = string.Format("and tu.UserId ='{0}'", techid);
                techqueryjoin = "left join TicketUser tu on tu.TiketId = tk.TicketId";
            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                --SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select * into #TicketData 
                                    from (--TicketTypeVal
		                                select tk.*
                                        ,(select count(id) from TicketFile where TicketId = tk.TicketId) as AttachmentsCount
                                        ,(select count(id) from TicketFile where TicketId = tk.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = tk.TicketId) as RepliesCount
                                        ,lktype.DisplayText as TicketTypeVal
                                        ,lkstatus.DisplayText as StatusVal
                                        ,lkpriority.DisplayText as PriorityVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = tk.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = tk.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
	                                    ,lkStartTime.DisplayText as AppointmentStartTimeVal
                                        ,CA.AppointmentStartTime as AppointmentStartTime
                                        ,lkEndTime.DisplayText as AppointmentEndTimeVal
                                        ,CA.AppointmentEndTime as AppointmentEndTime
                                        ,(select COUNT(cae.Id)
										from CustomerAppointmentEquipment cae
										LEFT JOIN Ticket t on t.TicketId=cae.AppointmentId
										LEFT JOIN TicketUser tu on tu.TiketId=t.TicketId and tu.IsPrimary=1
										where cae.AppointmentId=CA.AppointmentId
                                        AND cae.IsEquipmentRelease=0
										AND cae.Quantity>(ISNULL((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Add'  And invinner.TechnicianId=tu.UserId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Release'  And invinner.TechnicianId=tu.UserId),0))) as ExceedQuantity
                                            from Ticket tk
                                         LEFT JOIN TicketUser _tu on _tu.TiketId=tk.TicketId and _tu.IsPrimary=1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = tk.TicketType

                                        left join CustomerAppointment CA on  CA.AppointmentId = tk.TicketId

                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime

                                        left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        and lkEndTime.DataValue = CA.AppointmentEndTime

                                        left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        and lkstatus.DataValue = tk.[Status]
                                        
                                        {14}

                                        left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        and lkpriority.DataValue = tk.[Priority]

                                        left join Employee emp on tk.CreatedBy = emp.UserId
                                        
		                                where tk.CustomerId = @CustomerId
                                        and tk.CompanyId = @CompanyId
                                        {5}
                                        {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}

	                                ) a 

                                SELECT TOP (@pagesize) * FROM #TicketData
                                    where   Id NOT IN(Select TOP (@pagestart) Id from #TicketData {11}) 
                                     {12}
	                                --and (InvoiceIdStr like @SearchText or FirstName + ' '+ LastName like @SearchText)
	                           

	                            select  count(Id) as [TotalCount] from #TicketData 
                                DROP TABLE #TicketData
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        techquery,//13
                                        techqueryjoin//14
                                        );
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


        public DataSet GetTechTicketListByCustomerIdAndFilter(TicketFilter TicketFilters)
        {
            string searchQuery = "";

            string subquery = "";
            string subquery1 = "";
            string statusquery = "";
            string DateFilter = "";

            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), t.CompletionDate, 101) like @SearchText
								or t.[Status] like @SearchText)";
            }
            if (!string.IsNullOrEmpty(TicketFilters.TicketStatus))
            {
                if (TicketFilters.TicketStatus == "Closed")
                {
                    statusquery = " and (t.Status='Closed' or t.Status ='Completed')";
                }
                else if (TicketFilters.TicketStatus == "Open")
                {
                    statusquery = " and (t.Status != 'Completed' and t.Status != 'Closed')";
                }

            }


            DateTime FDate = TicketFilters.StartDate.ClientToUTCTime();
            string firstdate = "";
            string lastdate = "";
            if (FDate != new DateTime())
            {
                firstdate = FDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            DateTime LDate = TicketFilters.EndDate.SetMaxHour().ClientToUTCTime();
            if (LDate != new DateTime())
            {
                lastdate = LDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                DateFilter = string.Format(" and t.CreatedDate between '{0}' and '{1}'", firstdate, lastdate);
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }

                else if (TicketFilters.order == "ascending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] asc";
                    subquery1 = "order by [TicketType] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] desc";
                    subquery1 = "order by [TicketType] desc";
                }

                else if (TicketFilters.order == "descending/description")
                {
                    subquery = "order by #TicketData.[Message] desc";
                    subquery1 = "order by [Message] desc";
                }
                else if (TicketFilters.order == "ascending/description")
                {
                    subquery = "order by #TicketData.[Message] asc";
                    subquery1 = "order by [Message] asc";
                }

                else if (TicketFilters.order == "ascending/createdby")
                {
                    subquery = "order by #TicketData.[CreatedBy] asc";
                    subquery1 = "order by [CreatedBy] asc";
                }
                else if (TicketFilters.order == "descending/createdby")
                {
                    subquery = "order by #TicketData.[CreatedBy] desc";
                    subquery1 = "order by [CreatedBy] desc";
                }

                else if (TicketFilters.order == "descending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] desc";
                    subquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] asc";
                    subquery1 = "order by [AssignedTo] asc";
                }

                else if (TicketFilters.order == "ascending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/status")
                {
                    subquery = "order by #TicketData.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/status")
                {
                    subquery = "order by #TicketData.[Status]  desc";
                    subquery1 = "order by Status desc";
                }
            }
            else
            {
                subquery = "order by #TicketData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select * into #TicketData 
                                    from (select t.*
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = t.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = t.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
                                        ,lkstatus.DisplayText as StatusVal
                                            from TicketUser tu
                                            LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                            left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                            and lkstatus.DataValue = t.[Status]
                                            where tu.UserId=@CustomerId 
                                            and t.CompanyId=@CompanyId
                                            and  (t.TicketType Like '%Install%' or t.TicketType ='Service' or t.TicketType = 'Pick Up' or t.TicketType = 'Drop Off') {7} {6} 
	                                ) a 

                                SELECT TOP (@pagesize) * FROM #TicketData
                                    where   Id NOT IN(Select TOP (@pagestart) Id from #TicketData ) 
                                {5}
                                select  count(Id) as [TotalCount] from #TicketData 
                                DROP TABLE #TicketData
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                         TicketFilters.SearchText,//0
                                         TicketFilters.PageNo,//1
                                         TicketFilters.PageSize,//2
                                         TicketFilters.CompanyId,//3
                                         TicketFilters.CustomerId,//4
                                         subquery,//5
                                         statusquery,//6
                                         DateFilter//7
                                        );
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
        public DataTable GetAllTicketsForAPI(int Id, int CustomerId, Guid CustomerGuid)
        {
            string filterSql = "";

            if (Id != 0)
            {
                filterSql = "AND tic.Id = " + Id;
            }
            else if (CustomerId != 0)
            {
                filterSql = "AND cus.Id = " + CustomerId;
            }
            else if (CustomerGuid != new Guid())
            {
                filterSql = string.Format("AND tic.CustomerId = '{0}'", CustomerGuid);
            }

            string sqlQuery = @"select 
                                tic.Id as id, 
                                tic.CustomerId as customerguid,
                                cus.FirstName + ' '+ cus.LastName as customerName,
                                cus.BusinessName as customerBusinessName,
                                cus.Id as customerId,
                                TicketType as appoinmentType,
                                CompletionDate as appoinmentDate,
                                [Message] as [message],
                                tic.[Status] as [status],
                                tic.CreatedBy as [createdByGuid],
                                CreatedByEmp.FirstName + ' ' + CreatedByEmp.LastName as [createdByName],
                                tic.CreatedDate as [createdDate],
                                tu.UserId as [assignedToGuid],
                                AssignedEmp.FirstName + ' '+ AssignedEmp.LastName as [assignedToName],
                                cusApp.AppointmentStartTime as startTime,
                                cusApp.AppointmentEndTime as endTime
                                from Ticket tic 
                                left join TicketUser tu on tu.TiketId = tic.TicketId 
                                left join Employee AssignedEmp on tu.UserId = AssignedEmp.UserId
                                left join Employee CreatedByEmp on tic.CreatedBy = CreatedByEmp.UserId
                                left join Customer cus on tic.CustomerId = cus.CustomerId
                                left join CustomerAppointment cusApp on cusApp.AppointmentId = tic.TicketId

                                where tu.IsPrimary =1
                                and cus.Id is not null
                                {0}
                                order by id asc
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, filterSql);
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

        public DataTable GetTicketUserListByTicketId(Guid ticketId)
        {
            string sqlQuery = @"select tu.*,
                                emp.FirstName + ' '+emp.LastName as FullName 
                                from TicketUser tu left join Employee emp
                                on emp.UserId = tu.UserId
                                where tu.TiketId='{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
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

        public DataSet GetAllTicketReplyByTicketId(Guid ticketId, string search)
        {
            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(search))
            {
                subquery = string.Format("and tr.Message like '%{0}%'", search);
                subquery1 = string.Format("and tf.[Description] like '%{0}%'", search);
            }
            string sqlQuery = @"declare @TicketId uniqueidentifier
								set @TicketId = '{0}'
								select * into #ReplyTemp from 
                                (select tr.*,emp.ProfilePicture, 
                                    CASE WHEN emp.FirstName  Is not null THEN emp.FirstName+' '+emp.LastName
		                            ELSE cus.FirstName +' '+cus.LastName 
		                            end as CreatedByVal,
	                                'Reply' as TypeReply
                                    from TicketReply tr
                                    left join Employee emp 
                                    on emp.UserId = tr.UserId
                                    left join Customer cus on tr.UserId = cus.CustomerId

                                    where TicketId =  @TicketId
                                    {1}
	                                union
                                  select 
		                                tf.Id,
		                                TicketId,
		                                FileAddedBy as UserId,
		                                FileAddedDate as RepliedDate, 
		                                 CASE WHEN tf.[Description] Is null THEN N'<a href='+[FileLocation]+'>'+[FileName]+'</a><br />'
                                            ELSE N'<a class=""cus-anchor"" href=""/File/DownloadTicketFile?url=' + [FileLocation] + '"" target=""_blank""><i class=""fa fa-download""></i></a><br />' + tf.[Description]

                                             END AS[Message],
                                              0 as IsPrivate,
											 '' as ReplayType,
											  '' as LatLng,
											  0 as IsOverview,
											   emp.ProfilePicture,
                                         emp.FirstName + ' ' + emp.LastName as CreatedByVal,
											'File' as TypeReply
                                   from TicketFile tf
                                   left
                                   join Employee emp
                                on tf.FileAddedBy = emp.UserId 
                                  where TicketId = @TicketId
                                  {2}
                                  ) a

                                  select* from #ReplyTemp order by RepliedDate desc

                                  drop table #ReplyTemp";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId, subquery, subquery1);
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

        public Ticket GetTicketByTicketId(Guid ticketId)
        {
            string sqlQuery = @"
                            select tk.*
                            ,lktype.DisplayText as TicketTypeVal
                            ,lkstatus.DisplayText as StatusVal
                            ,lkpriority.DisplayText as PriorityVal
                            ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                            ,(select top(1) firstname + ' '+LastName from Employee where UserId = tu.UserId ) as AssignedTo
                            ,assEmp.UserId as AssignedToId
							,tu.IsReschedulePay as AssignedToIsReschedulePay                            
                            ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = tk.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
	                        from Ticket tk
                            left join Lookup lktype on  lktype.DataKey ='TicketType'  
                            and lktype.DataValue = tk.TicketType

                            left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                            and lkstatus.DataValue = tk.[Status]

                            left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                            and lkpriority.DataValue = tk.[Priority]

                            left join Employee emp on tk.CreatedBy = emp.UserId

                            left join TicketUser tu on tu.TiketId = tk.TicketId
                            and tu.IsPrimary = 1

                            left join Employee assEmp on assEmp.UserId = tu.UserId
		                    where tk.TicketId = '{0}'
                             
                        ";

            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);

                    using (reader)
                    {
                        if (reader.Read())
                        {
                            Ticket ticketObject = new Ticket();
                            FillObject(ticketObject, reader);

                            ticketObject.CreatedByVal = reader["CreatedByVal"].ToString();
                            ticketObject.TicketTypeVal = reader["TicketTypeVal"].ToString();
                            ticketObject.StatusVal = reader["StatusVal"].ToString();
                            ticketObject.PriorityVal = reader["PriorityVal"].ToString();
                            ticketObject.AssignedTo = reader["AssignedTo"].ToString();
                            ticketObject.AssignedToId = reader["AssignedToId"] != DBNull.Value ? (Guid)reader["AssignedToId"] : Guid.Empty;
                            ticketObject.AssignedToIsReschedulePay = reader["AssignedToIsReschedulePay"] != DBNull.Value ? Convert.ToBoolean(reader["AssignedToIsReschedulePay"]) : false;
                            ticketObject.AdditionalMembers = reader["AdditionalMembers"].ToString();
                            return ticketObject;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool UpdateAllTicketIsAgreementFalseByCustomerId(Guid CustomerId)
        {
            string SqlQuery = @"update Ticket set IsAgreementTicket=0 where CustomerId='{0}'";
            SqlQuery = string.Format(SqlQuery, CustomerId);
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
        public DataSet GetAllAssignedTicketByUserId(AssignTicketFilter filter)
        {
            string dateQuery = "";
            string filterQuery = "";

            if (filter.DateFrom != new DateTime() && filter.DateTo != new DateTime())
            {
                dateQuery = string.Format(" and tk.completionDate between '{0}' and '{1}'", filter.DateFrom.SetZeroHour(), filter.DateTo.SetMaxHour());
            }
            if (filter.TicketStatus != "-1" && !string.IsNullOrEmpty(filter.TicketStatus))
            {
                if (filter.TicketStatus == "Completed")
                {
                    filterQuery = " and (tk.Status = 'Completed' or tk.Status = 'Closed') ";
                }
                else
                {
                    filterQuery = " and (tk.Status != 'Completed' and tk.Status != 'Closed' and tk.Status != 'Incomplete') ";
                }
            }
            else
            {
                filterQuery = " and (tk.Status != 'Incomplete') ";
            }
            if (filter.TicketType != "-1" && !string.IsNullOrEmpty(filter.TicketType) && filter.TicketType != "undefined")
            {

                filterQuery += string.Format(" and (tk.TicketType = '{0}') ", filter.TicketType);

            }
            var TicketType = "";
            if (filter.IsInstallation == "true")
            {
                if (filter.JobLookupList != null && filter.JobLookupList.Count > 0)
                {
                    foreach (var item in filter.JobLookupList)
                    {
                        if (item.DataValue != "-1")
                        {
                            TicketType += string.Format("'{0}',", item.DataValue);
                        }
                    }
                    if (filter.JobLookupList.Count > 1)
                    {
                        TicketType = TicketType.Remove(TicketType.Length - 1, 1);

                    }
                    else
                    {
                        TicketType = "'-1'";
                    }
                    if (!string.IsNullOrWhiteSpace(TicketType))
                    {
                        filterQuery += " and tk.TicketType in (" + TicketType + ")";
                    }
                }
            }
            else
            {
                if (filter.TaskLookupList != null && filter.TaskLookupList.Count > 0)
                {
                    foreach (var item in filter.TaskLookupList)
                    {
                        if (item.DataValue != "-1")
                        {
                            TicketType += string.Format("'{0}',", item.DataValue);
                        }
                    }
                    if (filter.TaskLookupList.Count > 1)
                    {
                        TicketType = TicketType.Remove(TicketType.Length - 1, 1);
                    }
                    else
                    {
                        TicketType = "'-1'";
                    }
                    if (!string.IsNullOrWhiteSpace(TicketType))
                    {
                        filterQuery += " and tk.TicketType in (" + TicketType + ")";
                    }
                }
            }
            string sqlQuery = @"select 
		                        tk.Id
		                        ,lkTicketType.DisplayText as TicketType
                                ,tk.Status
                                ,tk.CompletionDate
		                        ,cu.Id as CustomerId
		                        ,CASE cu.BusinessName
                                    WHEN '' THEN cu.FirstName +' '+ cu.LastName
                                    ELSE cu.BusinessName 
                                END AS CustomerName
                             
		                        , empCreated.FirstName + ' '+ empCreated.LastName as CreatedBy
		                        , tk.CreatedDate
                                ,ca.AppointmentStartTime
								,ca.AppointmentEndTime                                ,tu.IsPrimary
								, tu.NotificationOnly
                                ,emp.FirstName + ' ' + emp.LastName as AssignedUserName
		                        from Ticket tk

                                LEFT JOIN Lookup lkTicketType 
								on lkTicketType.DataValue=tk.TicketType and lkTicketType.DataKey='TicketType'

		                        left join TicketUser tu 
		                        on tu.TiketId = tk.TicketId

		                        left join Employee emp 
		                        on tu.UserId = emp.UserId

		                        left join customer cu
		                        on cu.CustomerId = tk.CustomerId

		                        left join Employee empCreated 
		                        on empCreated.UserId = tk.CreatedBy
                                
                                left join CustomerAppointment ca
                                on ca.AppointmentId = tk.TicketId
                                            
		                        where (tu.UserId ='{0}' or tk.CreatedBy = '{0}') and cu.Id != 0
                                and emp.FirstName + ' ' + emp.LastName is not null
                                {1}{2} 
                                order by tk.completionDate desc, ca.AppointmentStartTime asc";
            try
            {
                sqlQuery = string.Format(sqlQuery, filter.UserId, dateQuery, filterQuery);
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

        public DataSet GetAllTicketByUserId(AssignTicketFilter filter, int PageNo, int PageSize)
        {
            string dateQuery = "";
            string filterQuery = "";
            string dateRange = "";
            int pagestart = (PageNo - 1) * PageSize;
            int pageend = PageSize;
            if (filter.DateFrom != new DateTime() && filter.DateTo != new DateTime())
            {
                dateQuery = string.Format(" and tk.completionDate between '{0}' and '{1}'", filter.DateFrom.SetZeroHour(), filter.DateTo.SetMaxHour());
            }
            if (!string.IsNullOrWhiteSpace(filter.ScheduleMaxDate) && !string.IsNullOrWhiteSpace(filter.ScheduleMinDate) && filter.ScheduleMinDate != "undefined" && filter.ScheduleMaxDate != "undefined")
            {
                var date = Convert.ToDateTime(filter.ScheduleMaxDate);
                var datemin = Convert.ToDateTime(filter.ScheduleMinDate);
                dateRange += string.Format("and tk.completionDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ScheduleMaxDate) && filter.ScheduleMaxDate != "undefined")
            {
                var date = Convert.ToDateTime(filter.ScheduleMaxDate);
                dateRange += string.Format("and tk.completionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ScheduleMinDate) && filter.ScheduleMinDate != "undefined")
            {
                var date = Convert.ToDateTime(filter.ScheduleMinDate);
                dateRange += string.Format("and tk.completionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            if (filter.TicketStatus != "-1" && !string.IsNullOrEmpty(filter.TicketStatus))
            {
                if (filter.TicketStatus == "Completed")
                {
                    filterQuery = " and (tk.Status = 'Completed' or tk.Status = 'Closed') ";
                }
                else
                {
                    filterQuery = " and (tk.Status != 'Completed' and tk.Status != 'Closed' and tk.Status != 'Incomplete') ";
                }
            }
            else
            {
                filterQuery = " and (tk.Status != 'Incomplete') ";
            }
            if (filter.TicketType != "-1" && !string.IsNullOrEmpty(filter.TicketType) && filter.TicketType != "undefined")
            {

                filterQuery += string.Format(" and (tk.TicketType = '{0}') ", filter.TicketType);

            }
            var TicketType = "";
            if (filter.IsInstallation == "true")
            {
                if (filter.TaskLookupList != null && filter.TaskLookupList.Count > 0)
                {
                    foreach (var item in filter.TaskLookupList)
                    {
                        if (item.DataValue != "-1")
                        {
                            TicketType += string.Format("'{0}',", item.DataValue);
                        }
                    }
                    if (filter.TaskLookupList.Count > 1)
                    {
                        TicketType = TicketType.Remove(TicketType.Length - 1, 1);
                    }
                    else
                    {
                        TicketType = "'-1'";
                    }

                }
                if (!string.IsNullOrWhiteSpace(TicketType))
                {
                    filterQuery += " and tk.TicketType in (" + TicketType + ")";
                }
            }
            else
            {
                if (filter.TaskLookupList != null && filter.TaskLookupList.Count > 0)
                {
                    foreach (var item in filter.JobLookupList)
                    {
                        if (item.DataValue != "-1")
                        {
                            TicketType += string.Format("'{0}',", item.DataValue);
                        }
                    }
                    if (filter.JobLookupList.Count > 1)
                    {
                        TicketType = TicketType.Remove(TicketType.Length - 1, 1);

                    }
                    else
                    {
                        TicketType = "'-1'";
                    }
                    if (!string.IsNullOrWhiteSpace(TicketType))
                    {
                        filterQuery += " and tk.TicketType in (" + TicketType + ")";
                    }
                }

                // filterQuery += " and tk.TicketType not like '%Install%' ";
            }
            string sqlQuery = @"select 
		                        tk.Id
		                        ,tk.TicketType
                                ,tk.Status
                                ,tk.CompletionDate
		                        ,cu.Id as CustomerId
		                        ,CASE cu.BusinessName
                                    WHEN '' THEN cu.FirstName +' '+ cu.LastName
                                    ELSE cu.BusinessName 
                                END AS CustomerName
                             
		                        , empCreated.FirstName + ' '+ empCreated.LastName as CreatedBy
		                        , tk.CreatedDate
                                ,ca.AppointmentStartTime
								,ca.AppointmentEndTime                                ,tu.IsPrimary
								, tu.NotificationOnly
                                ,emp.FirstName + ' ' + emp.LastName as AssignedUserName
		                       into #TicketData from Ticket tk

		                        left join TicketUser tu 
		                        on tu.TiketId = tk.TicketId

		                        left join Employee emp 
		                        on tu.UserId = emp.UserId

		                        left join customer cu
		                        on cu.CustomerId = tk.CustomerId

		                        left join Employee empCreated 
		                        on empCreated.UserId = tk.CreatedBy
                                
                                left join CustomerAppointment ca
                                on ca.AppointmentId = tk.TicketId
                                where tk.Id>0 {3}

                                            
		                     
                                

       select *,IDENTITY(Int,1,1) as paginationid into #TicketIdData from #TicketData where Id> 0 
								select top({4}) * from #TicketIdData
								where paginationid not in (Select TOP ({5}) paginationid from #TicketIdData  order by CompletionDate desc)
                                order by CompletionDate Desc

                                select Count(Id) As TotalCount from #TicketData
                                select Count(Id) as CountTicket from #TicketData 
                                   drop table #TicketData
                                   drop table #TicketIdData
";
            try
            {
                sqlQuery = string.Format(sqlQuery, filter.UserId, dateQuery, filterQuery, dateRange,PageSize,pagestart,pageend);
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

        public DataSet GetTicketById(int ticketId)
        {
            string sqlQuery = @"
                            select tk.*
                            ,lktype.DisplayText as TicketTypeVal
                            ,lkstatus.DisplayText as StatusVal
                            ,lkpriority.DisplayText as PriorityVal
                            ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                            ,(select top(1) firstname + ' '+LastName from Employee where UserId = tu.UserId ) as AssignedTo,
                            isnull(tu.UserId, '00000000-0000-0000-0000-000000000000') as AssignedToId
                            ,tk.IsClosed,
                            tk.Signature
                            ,ISNULL(tk.ReferenceTicketId, 0)	                            
                            from Ticket tk
                            left join Lookup lktype on  lktype.DataKey ='TicketType'  
                            and lktype.DataValue = tk.TicketType

                            left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                            and lkstatus.DataValue = tk.[Status]

                            left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                            and lkpriority.DataValue = tk.[Priority]

                            left join Employee emp on tk.CreatedBy = emp.UserId

                            left join TicketUser tu on tu.TiketId = tk.TicketId
                            and tu.IsPrimary = 1

                            left join Employee assEmp on assEmp.UserId = tu.UserId
		                    where tk.Id = '{0}'
                             
                        ";

            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
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

        public DataTable GetTicketSchedulesByUserListAndAppoinmentDate(List<Guid> assignedList, DateTime AppoinmentDate)
        {
            string sqlQuery = @"select ca.AppointmentId as TicketId
                        ,ca.AppointmentStartTime as StartTime
                        ,ca.AppointmentEndTime as EndTime
                        ,emp.FirstName + ' '+emp.LastName as EmployeeName
                        from CustomerAppointment ca 
                        left join TicketUser tu on tu.TiketId = ca.AppointmentId
                        left join Employee emp on tu.UserId = emp.UserId
                        where tu.UserId in('{0}')
                        and emp.Id is not null
                        and tu.Id is not null
                        and ca.Id is not null
                        and ca.AppointmentDate ='{1}'
                        group by ca.AppointmentId, AppointmentStartTime, AppointmentEndTime,emp.FirstName + ' '+emp.LastName";
            try
            {
                sqlQuery = string.Format(sqlQuery, string.Join("','", assignedList), AppoinmentDate.ToString("yyyy-MM-dd"));
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

        public DataTable GetTicketCountByCompanyId(Guid companyid, string starttime, string endtime, string date, Guid empid)
        {
            string sqlQuery = @"select COUNT(*) TicketAppCounter from CustomerAppointment
                                left join TicketUser on TiketId = AppointmentId
                                where CompanyId = '{0}'
                                and ((AppointmentStartTime > '{1}' and AppointmentStartTime < '{2}') or (AppointmentEndTime > '{1}' and AppointmentEndTime < '{2}'))
								and AppointmentDate = '{3}'
                                and NotificationOnly = 0
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, starttime, endtime, date, empid);
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

        public DataTable GetTicketCount1ByCompanyId(Guid companyid, string starttime, string endtime, string date, Guid empid)
        {
            string sqlQuery = @"select COUNT(*) TicketAppCounter1 from CustomerAppointment
                                left join TicketUser on TiketId = AppointmentId
                                where CompanyId = '{0}'
                                and ((CAST(AppointmentStartTime as time) > '{1}')
								or (CAST(AppointmentEndTime as time) > '{2}'))
								and AppointmentDate = '{3}'
                                and NotificationOnly = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, starttime, endtime, date, empid);
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

        public DataTable GetTicketCountUserByCompanyId(Guid companyid, string starttime, string endtime, string date, Guid empid)
        {
            string sqlQuery = @"select COUNT(*) TicketUserCounter from CustomerAppointment
								left join TicketUser on TiketId = AppointmentId
								where CompanyId = '{0}'
								and TiketId != '00000000-0000-0000-0000-000000000000'
                                and UserId = '{4}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, starttime, endtime, date, empid);
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

        public DataTable GetCustomerOverviewInformation(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"select tk.Id, tk.[TicketType], (select count(*) from TicketReply tr where tk.TicketId = tr.TicketId) as TicketReplyCount, lst.DisplayText as AppointmentStartTime, let.DisplayText as AppointmentEndTime, tk.CompletionDate,
                                (select emp.FirstName + ' ' + emp.LastName from TicketUser tu
                                left join employee emp on emp.UserId = tu.UserId
                                 where tu.TiketId = tk.TicketId
                                 and tu.IsPrimary = 1) as TicketUserName,Case When 
								 tk.TicketType = 'Pick Up' Then (select street+','+city+','+state+','+zipcode from customerAddress where refId = tk.BookingId and AddressType = 'PickUpLocation') 
								 when tk.TicketType = 'Drop Off' Then (select street+','+city+','+state+','+zipcode from customerAddress where refId = tk.BookingId and AddressType = 'DropOffLocation') 
								 else
								 (select street+','+city+','+state+','+zipcode from customerAddress where refId = tk.BookingId and AddressType = 'BillingAddress') end as Address 
                                 from Ticket tk
                                LEFT Join CustomerMigration cm on cm.CustomerId=tk.CustomerId
                                left join CustomerAppointment ca on ca.AppointmentId=tk.TicketId
                                left join Lookup lst on lst.DataValue=ca.AppointmentStartTime and lst.DataKey='TicketScheduleTime' and lst.DataValue!='-1'
                                left join Lookup let on let.DataValue=ca.AppointmentEndTime and let.DataKey='TicketScheduleTime' and let.DataValue!='-1'
                                where tk.CompanyId = '{0}'
                                and tk.CustomerId = '{1}'
                                and cm.Platform is NULL";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid);
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

        public DataSet CloneRescheduleTicketConfirmation(Guid oldticketid, string createdby, Guid createdbyuid, DateTime CompletionDate)
        {
            DataSet dsResult=null;
            string datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.000");
            string completionDT = CompletionDate.ToString("yyyy-MM-dd");
            string sqlQuery = @"declare @oldticketId uniqueidentifier 
                                set @oldticketId = '{0}'

                                declare @createdbyuid uniqueidentifier
                                set @createdbyuid = '{2}'

                                declare @createdby nvarchar(50)
                                set @createdby = '{1}'

                                declare @datetime datetime
                                set @datetime = '{3}'
                                
                                declare @completionDT datetime
                                set @completionDT = '{4}'

                                declare @newticketid uniqueidentifier
                                set @newticketid = NEWID()

                                --Ticket Clone
                                INSERT INTO Ticket ([TicketId],[CompanyId],[CustomerId],[TicketType],[Subject],[Message],[CreatedBy],[CreatedDate],[CompletionDate],[Status],[Priority],[LastUpdatedBy],[LastUpdatedDate],[HasInvoice],[HasSurvey],[IsClosed],[IsAgreementTicket],[CompletedDate],[Signature],[IsDispatch],[ReferenceTicketId],[BookingId],[Reason],[RackNo],[Locations],[RescheduleTicketId],[IsImportedTicket],[TicketSignatureDate]) SELECT @newticketid,[CompanyId],[CustomerId],[TicketType],[Subject],[Message],@createdbyuid,@datetime,@completionDT,[Status],[Priority],@createdbyuid,@datetime,[HasInvoice],[HasSurvey],[IsClosed],[IsAgreementTicket],[CompletedDate],[Signature],[IsDispatch],[ReferenceTicketId],[BookingId],[Reason],[RackNo],[Locations],0,[IsImportedTicket],[TicketSignatureDate] FROM Ticket where TicketId = @oldticketId

                                --Update Rescheduled Ticket
								update Ticket
								set [RescheduleTicketId] = (select Id from Ticket where TicketId = @newticketid)
								where TicketId = @oldticketId

                                --Ticket User Clone
                                INSERT INTO TicketUser ([TiketId],[UserId],[IsPrimary],[AddedDate],[AddedBy],[NotificationOnly],[IsReschedulePay]) SELECT @newticketid,[UserId],[IsPrimary],@datetime,@createdbyuid,[NotificationOnly],[IsReschedulePay] FROM TicketUser where TiketId = @oldticketId

                                --Customer Appointment Clocne
                                INSERT INTO CustomerAppointment ([AppointmentId],[CompanyId],[CustomerId],[EmployeeId],[AppointmentType],[AppointmentDate],[AppointmentStartTime],[AppointmentEndTime],[IsAllDay],[Notes],[Status],[TaxType],[TaxPercent],[TaxTotal],[TotalAmount],[TotalAmountTax],[CreatedBy],[LastUpdatedBy],[LastUpdatedDate],[Address]) SELECT @newticketid,[CompanyId],[CustomerId],[EmployeeId],[AppointmentType],[AppointmentDate],[AppointmentStartTime],[AppointmentEndTime],[IsAllDay],[Notes],[Status],[TaxType],[TaxPercent],[TaxTotal],[TotalAmount],[TotalAmountTax],@createdby,@createdby,@datetime,[Address] FROM CustomerAppointment where AppointmentId = @oldticketId

                                --Customer Appointment Equipment Clone
                                INSERT INTO CustomerAppointmentEquipment ([AppointmentId],[EquipmentId],[Quantity],[UnitPrice],[TotalPrice],[CreatedDate],[CreatedBy],[EquipName],[EquipDetail],[IsEquipmentRelease],[IsService],[CreatedByUid],[IsAgreementItem],[IsBaseItem],[IsBadInventory],[IsDefaultService],[IsCheckedEquipment],[IsTransfered],[QuantityLeftEquipment],[IsEquipmentExist],[OriginalUnitPrice],[IsInvoiceCreate],[ReferenceInvoiceId],[ReferenceInvDetailId],[IsBilling], [InstalledByUid]) SELECT @newticketid,[EquipmentId],[Quantity],[UnitPrice],[TotalPrice],@datetime,@createdby,[EquipName],[EquipDetail],[IsEquipmentRelease],[IsService],@createdbyuid,[IsAgreementItem],[IsBaseItem],[IsBadInventory],[IsDefaultService],[IsCheckedEquipment],[IsTransfered],[QuantityLeftEquipment],[IsEquipmentExist],[OriginalUnitPrice],[IsInvoiceCreate],[ReferenceInvoiceId],[ReferenceInvDetailId],[IsBilling],[InstalledByUid] FROM CustomerAppointmentEquipment where AppointmentId = @oldticketId

                                --Ticket Reply Clone
                                INSERT INTO TicketReply ([TicketId],[UserId],[RepliedDate],[Message],[IsPrivate],[ReplyType],[LatLng]) SELECT @newticketid,[UserId],[RepliedDate],[Message],[IsPrivate],[ReplyType],[LatLng] FROM TicketReply where TicketId = @oldticketId

                                --Ticket File Clone
                                INSERT INTO TicketFile ([TicketId],[FileName],[Filesize],[FileLocation],[Description],[FileAddedBy],[FileAddedDate]) SELECT @newticketid,[FileName],[Filesize],[FileLocation],[Description],[FileAddedBy],[FileAddedDate] FROM TicketFile where TicketId = @oldticketId

                                --Ticket TimeClock Clone
                                INSERT INTO TicketTimeClock ([TicketId],[UserId],[Time],[Type],[Lat],[Lng],[Note],[CreatedBy],[ClockedInMinutes],[LastUpdateBy],[LastUpdatedDate]) SELECT @newticketid,[UserId],[Time],[Type],[Lat],[Lng],[Note],@createdbyuid,[ClockedInMinutes],@createdbyuid,@datetime FROM TicketTimeClock where TicketId = @oldticketId

                                --Additional Members Appointment Clone
                                INSERT INTO AdditionalMembersAppointment([AppointmentId],[CompanyId],[CustomerId],[EmployeeId],[AppointmentDate],[AppointmentStartTime],[AppointmentEndTime],[CreatedBy],[LastUpdatedBy],[LastUpdatedDate],[MemberAppointmentId],[IsAllDay]) SELECT @newticketid,[CompanyId],[CustomerId],[EmployeeId],[AppointmentDate],[AppointmentStartTime],[AppointmentEndTime],@createdbyuid,@createdbyuid,@datetime,[MemberAppointmentId],[IsAllDay] FROM AdditionalMembersAppointment where AppointmentId = @oldticketId

                                --Sales Commission Clone
                                INSERT INTO SalesCommission ([SalesCommissionId],[TicketId],[CustomerId],[UserId],[CompletionDate],[RMRSold],[RMRCommission],[NoOfEquipment],[EquipmentCommission],[TotalCommission],[IsPaid],[CreatedBy],[CreatedDate],[Batch],[Adjustment],[RMRCommissionCalculation],[EquipmentCommissionCalculation],[PaidDate]) SELECT [SalesCommissionId],@newticketid,[CustomerId],[UserId],@completionDT,[RMRSold],[RMRCommission],[NoOfEquipment],[EquipmentCommission],[TotalCommission],[IsPaid],@createdbyuid,@datetime,[Batch],[Adjustment],[RMRCommissionCalculation],[EquipmentCommissionCalculation],[PaidDate] FROM SalesCommission where TicketId = @oldticketId

                                --Tech Commission Clone
                                INSERT INTO TechCommission ([TechCommissionId],[TicketId],[CustomerId],[UserId],[CompletionDate],[BaseRMR],[BaseRMRCommission],[AddedRMR],[AddedRMRCommission],[TotalCommission],[IsPaid],[CreatedBy],[CreatedDate],[Batch],[Adjustment],[BaseRMRCommissionCalculation],[AddedRMRCommissionCalculation],[PaidDate]) SELECT [TechCommissionId],@newticketid,[CustomerId],[UserId],@completionDT,[BaseRMR],[BaseRMRCommission],[AddedRMR],[AddedRMRCommission],[TotalCommission],[IsPaid],@createdbyuid,@datetime,[Batch],[Adjustment],[BaseRMRCommissionCalculation],[AddedRMRCommissionCalculation],[PaidDate] FROM TechCommission where TicketId = @oldticketId

                                --Reschedule Commission Clone
                                INSERT INTO RescheduleCommission ([RescheduleCommissionId],[TicketId],[CustomerId],[UserId],[CompletionDate],[Adjustment],[Commission],[IsPaid],[CreatedBy],[CreatedDate],[Batch],[CommissionCalculation],[PaidDate]) SELECT [RescheduleCommissionId],@newticketid,[CustomerId],[UserId],@completionDT,[Adjustment],[Commission],[IsPaid],@createdbyuid,@datetime,[Batch],[CommissionCalculation],[PaidDate] FROM RescheduleCommission where TicketId = @oldticketId

                                --Service Call Commission Clone
                                INSERT INTO ServiceCallCommission ([ServiceCallCommissionId],[TicketId],[CustomerId],[UserId],[CompletionDate],[Adjustment],[Commission],[IsPaid],[CreatedBy],[CreatedDate],[Batch],[CommissionCalculation],[PaidDate],[IsManual]) SELECT [ServiceCallCommissionId],@newticketid,[CustomerId],[UserId],@completionDT,[Adjustment],[Commission],[IsPaid],@createdbyuid,@datetime,[Batch],[CommissionCalculation],[PaidDate],[IsManual] FROM ServiceCallCommission where TicketId = @oldticketId

                                --Follow up Commission Clone
                                INSERT INTO FollowUpCommission ([FollowUpCommissionId],[TicketId],[CustomerId],[UserId],[CompletionDate],[Adjustment],[Commission],[IsPaid],[CreatedBy],[CreatedDate],[Batch],[CommissionCalculation],[PaidDate]) SELECT [FollowUpCommissionId],@newticketid,[CustomerId],[UserId],@completionDT,[Adjustment],[Commission],[IsPaid],@createdbyuid,@datetime,[Batch],[CommissionCalculation],[PaidDate] FROM FollowUpCommission where TicketId = @oldticketId

                                --Add Member Commission Clone
                                INSERT INTO AddMemberCommission ([AddMemberCommissionId],[TicketId],[CustomerId],[UserId],[CompletionDate],[Adjustment],[Commission],[IsPaid],[CreatedBy],[CreatedDate],[Batch],[CommissionCalculation],[PaidDate],[IsManual]) SELECT [AddMemberCommissionId],@newticketid,[CustomerId],[UserId],@completionDT,[Adjustment],[Commission],[IsPaid],@createdbyuid,@datetime,[Batch],[CommissionCalculation],[PaidDate],[IsManual] FROM AddMemberCommission where TicketId = @oldticketId

                                select Id from Ticket where TicketId = @newticketid
";
            try
            {
                sqlQuery = string.Format(sqlQuery, oldticketid, createdby, createdbyuid, datetime, completionDT);
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

        public DataTable GetAllAssignedTicketCountByUserId(Guid userid)
        {
            string sqlQuery = @"select COUNT(distinct tk.Id) as MyTicketCount from TicketUser tu

                                left join Employee emp on tu.UserId = emp.UserId

                                left join Ticket tk on tk.TicketId = tu.TiketId

                                where tu.UserId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid);
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
        public DataSet GetAllAssignedTicketListByUserId(Guid userid, int pageno, int pagesize)
        {
            string sqlQuery = @"select distinct top({1}*{2}) tk.Id, tk.TicketType, tk.[Message], createuser.FirstName + ' ' + createuser.LastName as CreatedUser 
                                , assignuser.FirstName + ' ' + assignuser.LastName as AssignUser
                                , tk.CompletionDate, tk.[Status]
                                from TicketUser tu

                                left join Employee emp on tu.UserId = emp.UserId

                                left join Ticket tk on tk.TicketId = tu.TiketId

								left join Employee createuser on createuser.UserId = tk.CreatedBy

								left join Employee assignuser on assignuser.UserId = tu.UserId

                                where tu.UserId ='{0}' and tk.TicketId is not null

                                select COUNT(distinct tk.Id) as TotalCount
								from TicketUser tu

                                left join Employee emp on tu.UserId = emp.UserId

                                left join Ticket tk on tk.TicketId = tu.TiketId

                                where tu.UserId ='{0}' and tk.TicketId is not null";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, pageno, pagesize);
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

        #region Installation Ticket Tech
        public DataTable GetInstallationTicketTechByCustomerId(Guid cusId)
        {
            string sqlQuery = @"select (emp.FirstName+ ' '+ emp.LastName) as TechName from Ticket t
                              left join TicketUser tu on tu.TiketId = t.TicketId
                              left join Employee emp on emp.UserId = tu.UserId
                              where CustomerId='{0}' and TicketType='Installation'
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, cusId);
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
        #endregion

        #region Ticket Summary Report

        public DataSet GetTicketSummaryList(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @" where ticket.Id = @SearchText or cus.Id = @SearchText or cus.CustomerNo = @SearchText";
            }
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '{0}' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select ticket.Id as TicketIntId,cus.Id as CustomerIntId,cus.CustomerNo into #TicketData
                                from Ticket ticket
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                {3}

                                SELECT TOP (@pagesize) #td.*
                                FROM #TicketData #td
                                where TicketIntId NOT IN(Select TOP (@pagestart) TicketIntId from #TicketData)
                                select  count(TicketIntId) as [TotalCount] from #TicketData
                                DROP TABLE #TicketData
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        searchQuery//3
                                        );
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
        public DataTable GetTicketSummaryListForReport(TicketFilter Filters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            if (!string.IsNullOrWhiteSpace(Filters.SearchText))
            {
                searchQuery = @" where ticket.Id = @SearchText or cus.Id = @SearchText or cus.CustomerNo = @SearchText";
            }
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '{0}'
 
                                select ticket.Id as TicketId,cus.Id as CustomerId,cus.CustomerNo
                                from Ticket ticket
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                {1}
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        Filters.SearchText,//0
                                        searchQuery//1
                                        );
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

        #endregion

        #region PR Report

        public DataSet GetPRReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            string DateQuery = "";
            string SearchText = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/ticket")
                {
                    orderquery = "order by #_TD.[Id] asc";
                    orderquery1 = "order by [Id] asc";
                }
                else if (order == "descending/ticket")
                {
                    orderquery = "order by #_TD.[Id] desc";
                    orderquery1 = "order by [Id] desc";
                }
                else if (order == "ascending/customer")
                {
                    orderquery = "order by #_TD.Name asc";
                    orderquery1 = "order by Name asc";
                }
                else if (order == "descending/customer")
                {
                    orderquery = "order by #_TD.Name desc";
                    orderquery1 = "order by Name desc";
                }
                else if (order == "ascending/type")
                {
                    orderquery = "order by #_TD.[TicketType] asc";
                    orderquery1 = "order by [TicketType] asc";
                }
                else if (order == "descending/type")
                {
                    orderquery = "order by #_TD.[TicketType] desc";
                    orderquery1 = "order by [TicketType] desc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #_TD.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #_TD.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }
                else if (order == "ascending/equipment")
                {
                    orderquery = "order by #_TD.EquipmentNames asc";
                    orderquery1 = "order by EquipmentNames asc";
                }
                else if (order == "descending/equipment")
                {
                    orderquery = "order by #_TD.EquipmentNames desc";
                    orderquery1 = "order by EquipmentNames desc";
                }
                else if (order == "ascending/total")
                {
                    orderquery = "order by #_TD.[TotalPayments] asc";
                    orderquery1 = "order by [TotalPayments] asc";
                }
                else if (order == "descending/total")
                {
                    orderquery = "order by #_TD.[TotalPayments] desc";
                    orderquery1 = "order by [TotalPayments] desc";
                }
                
                else
                {
                    orderquery = "order by #_TD.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #_TD.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (Start != new DateTime() && End != new DateTime())
            {
                DateQuery = string.Format("and Tk.CreatedDate between '{0}' and '{1}'", Start, End);
            }
            
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), Tk.Id) = '{0}' or  Cus.FirstName +' '+ Cus.LastName like '%{0}%')", searchtext);
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                    DECLARE @pagestart int
	                                    DECLARE @pageend int
	                                    DECLARE @pageno int
	                                    DECLARE @pagesize int

                                        SET @pageno = {0} --default 1
									    SET @pagesize = {1} --default 10
                                        SET @pagestart=(@pageno-1)* @pagesize 
                                        SET @pageend = @pagesize


                                        select
                                                Tk.Id
                                                ,Cus.Id as CustomerId
                                                ,Cus.CustomerNo
                                                ,Cus.FirstName +' '+ Cus.LastName as Name
												,Tk.TicketType
												,Tk.[Status]
                                                ,stuff((
                                                        select ',' + EquipName
                                                        from CustomerAppointmentEquipment  
                                                        where AppointmentId = Tk.TicketId
                                                        for xml path('')
                                                    ),1,1,'') as EquipmentNames
                                                
                                                ,(select SUM(Amount) from [Transaction] where CustomerId = Cus.CustomerId) as TotalPayments
                                                
                                                

                                        into #TicketData

                                        from Ticket Tk
                                            left join Customer Cus on Cus.CustomerId = Tk.CustomerId
                                       where Tk.Id is not null
									   {2}
                                       {3}
                                        SELECT TOP (@pagesize) #TD.* into #TestTable
	                                                FROM #TicketData #TD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #TicketData #_TD {4})
	                                                {5}
	                                                select count(Id) as [TotalCount] from #TicketData

													select * from #TestTable
													select sum(TotalPayments) as TotalPayment
													from #TestTable
	                                                DROP TABLE #TicketData
													DROP TABLE #TestTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
                                        );
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
        public DataTable GetPRReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                DateQuery = string.Format("and Tk.CreatedDate between '{0}' and '{1}'", Start, End);
            }

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), Tk.Id) = '{0}' or  Cus.FirstName +' '+ Cus.LastName like '%{0}%')", searchtext);
            }
            string sqlQuery = @"select
                                                Tk.Id as [Ticket Number]
                                                ,Cus.Id as [Customer Id]
                                                ,Cus.CustomerNo as [Customer No]
                                                ,Cus.FirstName +' '+ Cus.LastName as [Customer Name]
                                                ,Tk.TicketType as [Ticket Type]
												,Tk.[Status] as [Ticket Status]
                                                ,stuff((
                                                        select ',' + EquipName
                                                        from CustomerAppointmentEquipment  
                                                        where AppointmentId = Tk.TicketId
                                                        for xml path('')
                                                    ),1,1,'') as [Equipment Names]
                                                
                                                ,cast((select SUM(Amount) from [Transaction] where CustomerId = Cus.CustomerId) as decimal(10,2)) as [Total Payments]
                                           

                                        from Ticket Tk
                                            left join Customer Cus on Cus.CustomerId = Tk.CustomerId
                                       where Tk.Id is not null
                                        {0}
                                        {1}
                                        Order by Tk.Id desc";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
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

        #endregion

        #region Installed Ticket Report
        public DataSet InstalledTicketReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string PaymentMethod, string FundedStatus, Guid UserId,string order)
        {
            string DateQuery = "";
            string SearchText = "";
            string PaymentMethodQ = "";
            string FundedStatusQ = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customer")
                {
                    orderquery = "order by #_TD.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (order == "descending/customer")
                {
                    orderquery = "order by #_TD.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (order == "ascending/openar")
                {
                    orderquery = "order by #_TD.OpenerName asc";
                    orderquery1 = "order by OpenerName asc";
                }
                else if (order == "descending/openar")
                {
                    orderquery = "order by #_TD.OpenerName desc";
                    orderquery1 = "order by OpenerName desc";
                }
                else if (order == "ascending/usergroup")
                {
                    orderquery = "order by #_TD.[UserGroup] asc";
                    orderquery1 = "order by [UserGroup] asc";
                }
                else if (order == "descending/usergroup")
                {
                    orderquery = "order by #_TD.[UserGroup] desc";
                    orderquery1 = "order by [UserGroup] desc";
                }
                else if (order == "ascending/rep")
                {
                    orderquery = "order by #_TD.[Rep1] asc";
                    orderquery1 = "order by [Rep1] asc";
                }
                else if (order == "descending/rep")
                {
                    orderquery = "order by #_TD.[Rep1] desc";
                    orderquery1 = "order by [Rep1] desc";
                }
                else if (order == "ascending/date")
                {
                    orderquery = "order by #_TD.InstallDate asc";
                    orderquery1 = "order by InstallDate asc";
                }
                else if (order == "descending/date")
                {
                    orderquery = "order by #_TD.InstallDate desc";
                    orderquery1 = "order by InstallDate desc";
                }
                else if (order == "ascending/mmr")
                {
                    orderquery = "order by #_TD.[MonthlyMonitoringFee] asc";
                    orderquery1 = "order by [MonthlyMonitoringFee] asc";
                }
                else if (order == "descending/mmr")
                {
                    orderquery = "order by #_TD.[MonthlyMonitoringFee] desc";
                    orderquery1 = "order by [MonthlyMonitoringFee] desc";
                }
                else if (order == "ascending/equipinfo")
                {
                    orderquery = "order by #_TD.[EquipmentNames] asc";
                    orderquery1 = "order by [EquipmentNames] asc";
                }
                else if (order == "descending/equipinfo")
                {
                    orderquery = "order by #_TD.[EquipmentNames] desc";
                    orderquery1 = "order by [EquipmentNames] desc";
                }
                else if (order == "ascending/amount")
                {
                    orderquery = "order by #_TD.[TotalCollected] asc";
                    orderquery1 = "order by [TotalCollected] asc";
                }
                else if (order == "descending/amount")
                {
                    orderquery = "order by #_TD.[TotalCollected] desc";
                    orderquery1 = "order by [TotalCollected] desc";
                }
                else if (order == "ascending/term")
                {
                    orderquery = "order by #_TD.[ContractTeam] asc";
                    orderquery1 = "order by [ContractTeam] asc";
                }
                else if (order == "descending/term")
                {
                    orderquery = "order by #_TD.[ContractTeam] desc";
                    orderquery1 = "order by [ContractTeam] desc";
                }
                else if (order == "ascending/payment")
                {
                    orderquery = "order by #_TD.[PaymentMethod] asc";
                    orderquery1 = "order by [PaymentMethod] asc";
                }
                else if (order == "descending/payment")
                {
                    orderquery = "order by #_TD.[PaymentMethod] desc";
                    orderquery1 = "order by [PaymentMethod] desc";
                }
                else if (order == "ascending/technician")
                {
                    orderquery = "order by #_TD.[Technician] asc";
                    orderquery1 = "order by [Technician] asc";
                }
                else if (order == "descending/technician")
                {
                    orderquery = "order by #_TD.[Technician] desc";
                    orderquery1 = "order by [Technician] desc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #_TD.[CustomerFunded] asc";
                    orderquery1 = "order by [CustomerFunded] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #_TD.[CustomerFunded] desc";
                    orderquery1 = "order by [CustomerFunded] desc";
                }
                else
                {
                    orderquery = "order by #_TD.[RMRAccountNo]  desc";
                    orderquery1 = "order by RMRAccountNo desc";
                }

            }
            else
            {
                orderquery = "order by #_TD.[RMRAccountNo] desc";
                orderquery1 = "order by RMRAccountNo desc";
            }
            #endregion
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and tk.CompletedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), cus.Id) = '{0}' or CONVERT(nvarchar(MAX), cus.CustomerNo) = '{0}' or  Opener.FirstName +' '+ Opener.LastName like '%{0}%' or cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%{0}%')", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(PaymentMethod) && PaymentMethod != "null" && PaymentMethod != "-1")
            {
                PaymentMethodQ = string.Format("and cus.PaymentMethod = '{0}'", PaymentMethod);
            }
            if (!string.IsNullOrWhiteSpace(FundedStatus) && FundedStatus != "null")
            {
                if(FundedStatus == "1")
                {
                    FundedStatusQ = string.Format("and cus.CustomerFunded = '1'", FundedStatus);
                }
                if (FundedStatus == "0")
                {
                    FundedStatusQ = string.Format("and cus.CustomerFunded = '0'", FundedStatus);
                }
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select DISTINCT 
								--cusapp.Id
                                tk.Id 
								,cus.Id as RMRAccountNo
								,cus.CustomerNo
                                ,cus.CreditScoreValue  as CreditScore
								--,cusapp.CreatedBy as OpenerName
                                ,Opener.FirstName +' '+ Opener.LastName as OpenerName
								,SoldBy.Title +' '+ SoldBy.FirstName +' '+ SoldBy.LastName as Rep1
								,SoldBy2.Title +' '+ SoldBy2.FirstName +' '+ SoldBy2.LastName as Rep2
								,SoldBy3.Title +' '+ SoldBy3.FirstName +' '+ SoldBy3.LastName as Rep3
								,SoldBy4.Title +' '+ SoldBy4.FirstName +' '+ SoldBy4.LastName as Rep4
								,CASE cus.BusinessName
                                    WHEN '' THEN cus.Title +' '+ cus.FirstName +' '+ cus.LastName
                                    ELSE cus.BusinessName 
                                END AS CustomerName
								,tk.CompletedDate as InstallDate
								,dbo.MakeAddress(cus.Street,cus.StreetType,cus.Appartment,cus.City,cus.[State],cus.ZipCode) as [Address]
								,format(CAST(ISNULL(cus.MonthlyMonitoringFee,0) as float),'N2') as MonthlyMonitoringFee
								,stuff((
                                                        select ', ' + EquipName
                                                        from CustomerAppointmentEquipment  
                                                        where AppointmentId = tk.TicketId and IsService = 0
                                                        for xml path('')
                                                    ),1,1,'') as EquipmentNames
								,(select SUM(Eq.Point) from Equipment Eq where Eq.EquipmentId in 
	                                                (select Equipmentid from CustomerAppointmentEquipment where AppointmentId = tk.TicketId)) as EquipmentPoint
								,(select SUM(EqVen.Cost) from EquipmentVendor EqVen where EqVen.IsPrimary = 1 and EqVen.EquipmentId in 
	                                                (select Equipmentid from CustomerAppointmentEquipment where AppointmentId = tk.TicketId)) as EquipmentCost
								,(select SUM(Amount) from [Transaction] where CustomerId = Cus.CustomerId) as TotalCollected
                                ,(select count(*) from [Transaction] where CustomerId = Cus.CustomerId) as NumnberOfPayment
								,lp.DisplayText as ContractTeam
								,CASE WHEN cus.PaymentMethod = '-1' THEN ' ' ELSE cus.PaymentMethod end as PaymentMethod
							    ,CASE WHEN cus.PaymentMethod = '-1' THEN ' ' else( Case when cus.PaymentMethod = 'ACH' or cus.PaymentMethod = 'Invoice' or cus.PaymentMethod = 'Credit Card' or cus.PaymentMethod = 'Due at Install' or cus.PaymentMethod = 'Financed' then cus.PaymentMethod  else (select top (1) pp.Type from PaymentProfileCustomer pp where convert(nvarchar(100),pp.PaymentInfoId) = cus.PaymentMethod order by pp.Id desc )  end) End as PaymentMethodVal

								,CASE WHEN (Tech.Title +' '+ Tech.FirstName +' '+ Tech.LastName is null OR Tech.Title +' '+ Tech.FirstName +' '+ Tech.LastName ='') THEN 'System User' ELSE Tech.Title +' '+ Tech.FirstName +' '+ Tech.LastName end as Technician
								,cus.[Type] as CustomerType
                                ,CASE WHEN ((cus.AlarmRefId is not null AND cus.AlarmRefId !='') OR (cus.BrinksRefId is not null AND cus.BrinksRefId !='') OR (cus.UCCRefId is not null AND cus.UCCRefId !=''))  THEN 'Yes' ELSE 'No' end as AccountStatus
                                --,CASE WHEN (select count(tcus.Id) from ThirdPartyCustomer tcus where tcus.CustomerId = cus.CustomerId and tcus.AccountOnlineDate is not null) > 0  THEN 'Yes' ELSE 'No' end as AccountStatus
								,tk.CompletionDate as ServiceShedule
                                --,(select top(1) cusapp.CreatedDate from CustomerAppointmentEquipment where IsService=1 order by Id asc) as ServiceShedule
                                ,CASE WHEN cus.CustomerFunded = '1' THEN 'Yes' ELSE 'No' end as CustomerFunded
								--,(select [Name] from PermissionGroup where Id = (select PermissionGroupId from UserPermission where UserId = '{6}') ) as UserGroup
                               ,(select [Name] from PermissionGroup where Id =
									(select PermissionGroupId from UserPermission where UserId = 
										(select Soldby1 from Customer where CustomerId = 
											(select CustomerId from Customer where Id = cus.Id)))) as UserGroup
                                    into #TicketData

                                from Ticket tk 
                                left join TicketUser TU on TU.TiketId = tk.ticketId and TU.IsPrimary = 1
								--left join Ticket tk on tk.TicketId = cusapp.AppointmentId
								left join Customer cus on cus.CustomerId = tk.CustomerId
								left join CustomerExtended cusex on cusex.CustomerId = cus.CustomerId
								left join Employee SoldBy on CONVERT(nvarchar(MAX), SoldBy.UserId)  = Cus.SoldBy
								left join Employee SoldBy2 on SoldBy2.UserId = cus.SoldBy2
								left join Employee SoldBy3 on SoldBy3.UserId = cus.SoldBy3
								left join Employee SoldBy4 on SoldBy4.UserId = cusex.SalesPerson4
                                left join Employee Tech on Tech.UserId = TU.UserId
                                left join Employee Opener on Opener.UserId = tk.CreatedBy
                                left join Lookup lp on lp.DataValue=cus.ContractTeam and lp.DataKey='ContractTerm' and lp.DataValue!='-1'

                                where tk.status ='Completed'
                                and (cus.Id is not null OR cus.Id !='')
                                --where cusapp.QuantityLeftEquipment != 0
                                --and cusapp.IsEquipmentRelease=1
                                {2}
                                {3}
                                {4}
                                {5}
								SELECT TOP (@pagesize) #TD.* into #TestTable
	                                                FROM #TicketData #TD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #TicketData #_TD {7})
	                                                {8}
	                                                select count(Id) as [TotalCount] from #TicketData

													select * from #TestTable {8}
													select sum(EquipmentCost) as TotalEquipmentCost
													,sum(TotalCollected) as TotalAmount
													from #TestTable

	                                                DROP TABLE #TicketData
													DROP TABLE #TestTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        PaymentMethodQ,
                                        FundedStatusQ,
                                        UserId,
                                        orderquery,
                                        orderquery1
                                        );
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
        public DataTable GetInstalledTicketReportListForDownload(DateTime? Start, DateTime? End, string searchtext, string PaymentMethod, string FundedStatus, Guid UserId)
        {
            string DateQuery = "";
            string SearchText = "";
            string PaymentMethodQ = "";
            string FundedStatusQ = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and tk.CompletedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), cus.Id) = '{0}' or CONVERT(nvarchar(MAX), cus.CustomerNo) = '{0}' or  Opener.FirstName +' '+ Opener.LastName like '%{0}%' or cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%{0}%')", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(PaymentMethod) && PaymentMethod != "null" && PaymentMethod != "-1")
            {
                PaymentMethodQ = string.Format("and cus.PaymentMethod = '{0}'", PaymentMethod);
            }
            if (!string.IsNullOrWhiteSpace(FundedStatus) && FundedStatus != "null")
            {
                if (FundedStatus == "1")
                {
                    FundedStatusQ = string.Format("and cus.CustomerFunded = '1'", FundedStatus);
                }
                if (FundedStatus == "0")
                {
                    FundedStatusQ = string.Format("and cus.CustomerFunded = '0'", FundedStatus);
                }
            }
            string sqlQuery = @"select DISTINCT 
                                --cusapp.Id,
                                tk.Id,
								cus.Id as [RMA No.]
								,cus.CustomerNo as [CS Account No]
                                ,CASE cus.BusinessName
                                    WHEN '' THEN cus.Title +' '+ cus.FirstName +' '+ cus.LastName
                                    ELSE cus.BusinessName 
                                END AS [Customer Name]
                                ,cus.CreditScoreValue as [Credit Score]
                                ,cus.[Type] as [Customer Type]
                                ,dbo.MakeAddress(cus.Street,cus.StreetType,cus.Appartment,cus.City,cus.[State],cus.ZipCode) as [Address]
								,Opener.FirstName +' '+ Opener.LastName as [Opener Name]
                                --,cusapp.CreatedBy as [Opener Name]
                                --,(select [Name] from PermissionGroup where Id = (select PermissionGroupId from UserPermission where UserId = '{4}') ) as [User Group]
                                ,(select [Name] from PermissionGroup where Id =
									(select PermissionGroupId from UserPermission where UserId = 
										(select Soldby1 from Customer where CustomerId = 
											(select CustomerId from Customer where Id = cus.Id)))) as [User Group]
								,SoldBy.Title +' '+ SoldBy.FirstName +' '+ SoldBy.LastName as [Rep 01]
								,SoldBy2.Title +' '+ SoldBy2.FirstName +' '+ SoldBy2.LastName as [Rep 02]
								,SoldBy3.Title +' '+ SoldBy3.FirstName +' '+ SoldBy3.LastName as [Rep 03]
								,SoldBy4.Title +' '+ SoldBy4.FirstName +' '+ SoldBy4.LastName as [Rep 04]
							    --,cusapp.CreatedDate as [Installed]
								,convert(date,tk.CompletedDate) as [Installed Date]
                                ,convert(date,tk.CompletionDate) as [Service Shedule Date]
								--,convert(date,(select top(1) cusapp.CreatedDate from CustomerAppointmentEquipment where IsService=1 order by Id asc)) as [Service Shedule Date]
								,format(CAST(ISNULL(cus.MonthlyMonitoringFee,0) as float),'N2') as MMR
								,stuff((
                                                        select ', ' + EquipName
                                                        from CustomerAppointmentEquipment  
                                                        where AppointmentId = tk.TicketId and IsService = 0
                                                        for xml path('')
                                                    ),1,1,'') as [Equipment Names]
								,(select SUM(Eq.Point) from Equipment Eq where Eq.EquipmentId in 
	                                                (select Equipmentid from CustomerAppointmentEquipment where AppointmentId = tk.TicketId)) as [Equipment Point]

				                  ,cast((select SUM(EqVen.Cost) from EquipmentVendor EqVen where EqVen.IsPrimary = 1 and EqVen.EquipmentId in 
	                             (select Equipmentid from CustomerAppointmentEquipment where AppointmentId = tk.TicketId))as decimal(10,2)) as [Equipment Cost]


								,cast((select SUM(Amount) from [Transaction] where CustomerId = Cus.CustomerId) as decimal(10,2)) as [Total Collected]

                                ,lp.DisplayText as Team
								--,CASE WHEN cus.ContractTeam = '-1' THEN ' ' ELSE cus.ContractTeam end as [Term]
								--,CASE WHEN cus.PaymentMethod = '-1' THEN ' ' ELSE cus.PaymentMethod end as [Payment Method]
							    ,CASE WHEN cus.PaymentMethod = '-1' THEN ' ' else( Case when cus.PaymentMethod = 'ACH' or cus.PaymentMethod = 'Invoice' or cus.PaymentMethod = 'Credit Card' or cus.PaymentMethod = 'Due at Install' or cus.PaymentMethod = 'Financed' then cus.PaymentMethod  else (select top (1) pp.Type from PaymentProfileCustomer pp where convert(nvarchar(100),pp.PaymentInfoId) = cus.PaymentMethod order by pp.Id desc )  end) End as [Payment Method]

                                ,(select count(*) from [Transaction] where CustomerId = Cus.CustomerId) as [Numnber Of Payment]
								,CASE WHEN (Tech.Title +' '+ Tech.FirstName +' '+ Tech.LastName is null OR Tech.Title +' '+ Tech.FirstName +' '+ Tech.LastName ='') THEN 'System User' ELSE Tech.Title +' '+ Tech.FirstName +' '+ Tech.LastName end as Technician
                                --,CASE WHEN (select count(tcus.Id) from ThirdPartyCustomer tcus where tcus.CustomerId = cus.CustomerId and tcus.AccountOnlineDate is not null) > 0  THEN 'Yes' ELSE 'No' end as [Account Online]
                                ,CASE WHEN ((cus.AlarmRefId is not null AND cus.AlarmRefId !='') OR (cus.BrinksRefId is not null AND cus.BrinksRefId !='') OR (cus.UCCRefId is not null AND cus.UCCRefId !=''))  THEN 'Yes' ELSE 'No' end as [Account Online]
                                ,CASE WHEN cus.CustomerFunded = '1' THEN 'Yes' ELSE 'No' end as [Funded Status]
                                
                                from Ticket tk 
								left join TicketUser TU on TU.TiketId = tk.ticketId and TU.IsPrimary = 1
                                --from CustomerAppointmentEquipment cusapp
								--left join Ticket tk on tk.TicketId = cusapp.AppointmentId
								left join Customer cus on cus.CustomerId = tk.CustomerId
								left join CustomerExtended cusex on cusex.CustomerId = cus.CustomerId
								left join Employee SoldBy on CONVERT(nvarchar(MAX), SoldBy.UserId)  = Cus.SoldBy
								left join Employee SoldBy2 on SoldBy2.UserId = cus.SoldBy2
								left join Employee SoldBy3 on SoldBy3.UserId = cus.SoldBy3
								left join Employee SoldBy4 on SoldBy4.UserId = cusex.SalesPerson4
                                left join Employee Tech on Tech.UserId = TU.UserId
								left join Employee Opener on Opener.UserId = tk.CreatedBy
                                left join Lookup lp on lp.DataValue=cus.ContractTeam and lp.DataKey='ContractTerm' and lp.DataValue!='-1'

                                where tk.status ='Completed'
                                and (cus.Id is not null OR cus.Id !='')
                                --where cusapp.QuantityLeftEquipment != 0
                                --and cusapp.IsEquipmentRelease=1
                                {0}
                                {1}
                                {2}
                                {3}

                                Order by [RMA No.] desc";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText,
                                        PaymentMethodQ,
                                        FundedStatusQ,
                                        UserId
                                        );
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
        #endregion

        #region Paid Commission Report
        public DataSet PaidCommissionReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            string DateQuery = "";
            string SearchText = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/accountno")
                {
                    orderquery = "order by #_PD.[RMAAccountNo] asc";
                    orderquery1 = "order by [RMAAccountNo] asc";
                }
                else if (order == "descending/accountno")
                {
                    orderquery = "order by #_PD.[RMAAccountNo] desc";
                    orderquery1 = "order by [RMAAccountNo] desc";
                }
                else if (order == "ascending/rep")
                {
                    orderquery = "order by #_PD.RepName asc";
                    orderquery1 = "order by RepName asc";
                }
                else if (order == "descending/rep")
                {
                    orderquery = "order by #_PD.RepName desc";
                    orderquery1 = "order by RepName desc";
                }
                else if (order == "ascending/tech")
                {
                    orderquery = "order by #_PD.[TechName] asc";
                    orderquery1 = "order by [TechName] asc";
                }
                else if (order == "descending/tech")
                {
                    orderquery = "order by #cd.[TechName] desc";
                    orderquery1 = "order by [TechName] desc";
                }
                else if (order == "ascending/misc")
                {
                    orderquery = "order by #_PD.[OpenerCommission] asc";
                    orderquery1 = "order by [OpenerCommission] asc";
                }
                else if (order == "descending/misc")
                {
                    orderquery = "order by #_PD.[OpenerCommission] desc";
                    orderquery1 = "order by [OpenerCommission] desc";
                }
             
                else
                {
                    orderquery = "order by #_PD.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #_PD.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (Start != new DateTime() && End != new DateTime())
            {
                var StartDateQuery = Start.Value.SetClientZeroHourToUTC(); //ToString("yyyy-MM-dd 00:00:00.000");
                var EndDateQuery = End.Value.SetClientMaxHourToUTC(); //ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and PD.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), PD.RMAAccountNo) = '{0}' or  PD.RepName like '%{0}%')", searchtext);
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select 
                                                 PD.Id
                                                ,PD.RMAAccountNo
	                                            ,PD.RepName
	                                            ,PD.RepCommission
	                                            ,PD.RepHoldback
	                                            ,PD.OverrideRep1
	                                            ,PD.Override1
	                                            ,PD.OverrideRep2
	                                            ,PD.Override2
	                                            ,PD.RepPaidDate
	                                            ,PD.TechName
	                                            ,PD.TechPay
	                                            ,PD.TechHoldback
	                                            ,PD.TechPaidDate
	                                            ,PD.OpenerCommission
	                                            ,PD.MiscRep1
	                                            ,PD.MiscCommission1
	                                            ,PD.MiscRep2
	                                            ,PD.MiscCommission2
	                                            ,PD.MiscRep3
	                                            ,PD.MiscCommission3
	                                            ,PD.MiscRep4
	                                            ,PD.MiscCommission4
	                                            ,PD.MiscRep5
	                                            ,PD.MiscCommission5
                                                ,PD.MiscPaidDate
			
			                                             
								
                                into #PaidCommissionData

                                from PayrollDetail PD
                                where PD.RMAAccountNo is not null
                                {2}
                                {3}
								SELECT TOP (@pagesize) #PD.*
	                                                FROM #PaidCommissionData #PD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #PaidCommissionData #_PD {4})
	                                                {5}
	                                                select count(Id) as [TotalCount] from #PaidCommissionData
	                                                DROP TABLE #PaidCommissionData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
                                        );
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

        public DataSet CancelQueueReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            string DateQuery = "";
            string SearchText = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/csnumber")
                {
                    orderquery = "order by #_CD.[CustomerNo] asc";
                    orderquery1 = "order by [CustomerNo] asc";
                }
                else if (order == "descending/csnumber")
                {
                    orderquery = "order by #_CD.[CustomerNo] desc";
                    orderquery1 = "order by [CustomerNo] desc";
                }
                else if (order == "ascending/date")
                {
                    orderquery = "order by #_CD.BrinksCancelDate asc";
                    orderquery1 = "order by BrinksCancelDate asc";
                }
                else if (order == "descending/date")
                {
                    orderquery = "order by #_CD.BrinksCancelDate desc";
                    orderquery1 = "order by BrinksCancelDate desc";
                }
        
         

                else
                {
                    orderquery = "order by #_CD.[CustomerNo]  desc";
                    orderquery1 = "order by CustomerNo desc";
                }

            }
            else
            {
                orderquery = "order by #_CD.[CustomerNo] desc";
                orderquery1 = "order by CustomerNo desc";
            }
            #endregion
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusExt.BrinksCancelDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo like '%{0}%'", searchtext);
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select 
                                                 cus.Id
                                                ,cus.CustomerNo
	                                            ,cusExt.BrinksCancelDate
	                               
                                into #CancelQueueData

                                from Customer cus
                                left join CustomerExtended cusExt on cusExt.CustomerId =  cus.CustomerId
                              where cus.CustomerNo != '' and cus.CustomerNo is not null and cusExt.BrinksCancelDate != ''
                                {2}
                                {3}
								SELECT TOP (@pagesize) *
	                                                FROM #CancelQueueData #CD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #CancelQueueData #_CD {4})
	                                                {5}
	                                                select count(Id) as [TotalCount] from #CancelQueueData
	                                                DROP TABLE #CancelQueueData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
                                        );
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

        public DataSet BrinksCustomerReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            string DateQuery = "";
            string SearchText = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/account")
                {
                    orderquery = "order by #_CD.[CustomerNo] asc";
                    orderquery1 = "order by [CustomerNo] asc";
                }
                else if (order == "descending/account")
                {
                    orderquery = "order by #_CD.[CustomerNo] desc";
                    orderquery1 = "order by [CustomerNo] desc";
                }
                else if (order == "ascending/receiveddate")
                {
                    orderquery = "order by #_CD.ReceivedDate asc";
                    orderquery1 = "order by ReceivedDate asc";
                }
                else if (order == "descending/receiveddate")
                {
                    orderquery = "order by #_CD.ReceivedDate desc";
                    orderquery1 = "order by ReceivedDate desc";
                }
                else if (order == "ascending/fundingdate")
                {
                    orderquery = "order by #_CD.[BrinksFundingDate] asc";
                    orderquery1 = "order by [BrinksFundingDate] asc";
                }
                else if (order == "descending/fundingdate")
                {
                    orderquery = "order by #_CD.[BrinksFundingDate] desc";
                    orderquery1 = "order by [BrinksFundingDate] desc";
                }
                else if (order == "ascending/amount")
                {
                    orderquery = "order by #_CD.[GrossFundedAmount] asc";
                    orderquery1 = "order by [GrossFundedAmount] asc";
                }
                else if (order == "descending/amount")
                {
                    orderquery = "order by #_CD.[GrossFundedAmount] desc";
                    orderquery1 = "order by [GrossFundedAmount] desc";
                }
          

                else
                {
                    orderquery = "order by #_CD.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #_CD.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusExt.BrinksFundingDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo like '%{0}%'", searchtext);
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select 
                                                 cus.Id
                                                ,cus.CustomerNo
                                                ,cusExt.BrinksFundingDate
	                                            ,cusExt.ReceivedDate
                                                ,cusExt.GrossFundedAmount
	                               
                                into #CustomerListData

                                from Customer cus
                                left join CustomerExtended cusExt on cusExt.CustomerId =  cus.CustomerId
                              where cus.CustomerNo != '' and cus.CustomerNo is not null 
                                {2}
                                {3}
								SELECT TOP (@pagesize) *
	                                                FROM #CustomerListData #CD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #CustomerListData #_CD {4})
	                                                {5}
	                                                select count(Id) as [TotalCount] from #CustomerListData
	                                                DROP TABLE #CustomerListData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
                                        );
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
        public DataSet FundingVerificationReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            string DateQuery = "";
            string SearchText = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/fundingdate")
                {
                    orderquery = "order by #_CD.[FinanceFundingDate] asc";
                    orderquery1 = "order by [FinanceFundingDate] asc";
                }
                else if (order == "descending/fundingdate")
                {
                    orderquery = "order by #_CD.[FinanceFundingDate] desc";
                    orderquery1 = "order by [FinanceFundingDate] desc";
                }
                else if (order == "ascending/cs")
                {
                    orderquery = "order by #_CD.CustomerNo asc";
                    orderquery1 = "order by CustomerNo asc";
                }
                else if (order == "descending/cs")
                {
                    orderquery = "order by #_CD.CustomerNo desc";
                    orderquery1 = "order by CustomerNo desc";
                }
                else if (order == "ascending/financecompany")
                {
                    orderquery = "order by #_CD.[FinanceCompany] asc";
                    orderquery1 = "order by [FinanceCompany] asc";
                }
                else if (order == "descending/financecompany")
                {
                    orderquery = "order by #_CD.[FinanceCompany] desc";
                    orderquery1 = "order by [FinanceCompany] desc";
                }
                else if (order == "ascending/plancode")
                {
                    orderquery = "order by #_CD.[Plancode] asc";
                    orderquery1 = "order by [Plancode] asc";
                }
                else if (order == "descending/plancode")
                {
                    orderquery = "order by #_CD.[Plancode] desc";
                    orderquery1 = "order by [Plancode] desc";
                }
                else if (order == "ascending/mmr")
                {
                    orderquery = "order by #_CD.NewMMR asc";
                    orderquery1 = "order by NewMMR asc";
                }
                else if (order == "descending/mmr")
                {
                    orderquery = "order by #_CD.NewMMR desc";
                    orderquery1 = "order by NewMMR desc";
                }
                else if (order == "ascending/loan")
                {
                    orderquery = "order by #_CD.[LoanAmount] asc";
                    orderquery1 = "order by [LoanAmount] asc";
                }
                else if (order == "descending/loan")
                {
                    orderquery = "order by #_CD.[LoanAmount] desc";
                    orderquery1 = "order by [LoanAmount] desc";
                }
                else if (order == "ascending/payout")
                {
                    orderquery = "order by #_CD.[PayOut] asc";
                    orderquery1 = "order by [PayOut] asc";
                }
                else if (order == "descending/payout")
                {
                    orderquery = "order by #_CD.[PayOut] desc";
                    orderquery1 = "order by [PayOut] desc";
                }
                else
                {
                    orderquery = "order by #_CD.[CustomerNo]  desc";
                    orderquery1 = "order by CustomerNo desc";
                }

            }
            else
            {
                orderquery = "order by #_CD.[CustomerNo] desc";
                orderquery1 = "order by CustomerNo desc";
            }
            #endregion
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusExt.FinanceFundingDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo like '%{0}%'", searchtext);
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select 
                                                 cus.Id
                                                ,cus.CustomerNo
                                                ,cusExt.FinanceCompany
                                                ,cusExt.FinanceFundingDate
	                                            ,cusExt.LoanAmount
                                                ,cusExt.PayOut
                                                ,cusExt.NewMMR
                                                ,cusExt.Plancode
	                           
                                into #FVData

                                from Customer cus
                                left join CustomerExtended cusExt on cusExt.CustomerId =  cus.CustomerId
                              where cus.CustomerNo != '' and cus.CustomerNo is not null 
                                {2}
                                {3}
								SELECT TOP (@pagesize) *
	                                                FROM #FVData #CD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #FVData #_CD {4})
	                                                {5}
	                                                select count(Id) as [TotalCount] from #FVData
	                                                DROP TABLE #FVData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
                                        );
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
        public DataSet GetPaidCommissionImportDataById(int Id)
        {
            string sqlQuery = @"select
                                                PD.Id
	                                            ,PD.RepName
	                                            ,PD.RepCommission
	                                            ,PD.RepHoldback
	                                            ,PD.OverrideRep1
	                                            ,PD.Override1
	                                            ,PD.OverrideRep2
	                                            ,PD.Override2
	                                            ,PD.RepPaidDate
	                                            ,PD.TechName
	                                            ,PD.TechPay
	                                            ,PD.TechHoldback
	                                            ,PD.TechPaidDate
	                                            ,PD.OpenerCommission
	                                            ,PD.MiscRep1
	                                            ,PD.MiscCommission1
	                                            ,PD.MiscRep2
	                                            ,PD.MiscCommission2
	                                            ,PD.MiscRep3
	                                            ,PD.MiscCommission3
	                                            ,PD.MiscRep4
	                                            ,PD.MiscCommission4
	                                            ,PD.MiscRep5
	                                            ,PD.MiscCommission5
                                                ,PD.MiscPaidDate

                                from PayrollDetail PD
                                where PD.Id =  '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        Id
                                        );
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
        public DataTable GetPaidCommissionReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                var StartDateQuery = Start.Value.SetClientZeroHourToUTC(); //ToString("yyyy-MM-dd 00:00:00.000");
                var EndDateQuery = End.Value.SetClientMaxHourToUTC(); //ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and PD.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), PD.RMAAccountNo) = '{0}' or  PD.RepName like '%{0}%')", searchtext);
            }
            string sqlQuery = @"select
                                                 PD.RMAAccountNo as [RMA Account No]
                                                ,PD.RepName as [Rep Name]
	                                            ,PD.RepCommission as [Rep Commission]
	                                            ,PD.RepHoldback as [Rep Holdback]
	                                            ,PD.OverrideRep1 as [Override Rep1]
	                                            ,PD.Override1 as [Override 1]
	                                            ,PD.OverrideRep2 as [Override Rep2]
	                                            ,PD.Override2 as [Override 2]
	                                            ,PD.RepPaidDate as [Rep Paid Date]
	                                            ,PD.TechName as [Tech Name]
	                                            ,PD.TechPay as [Tech Pay]
	                                            ,PD.TechHoldback as [Tech Holdback]
	                                            ,PD.TechPaidDate as [Tech Paid Date]
	                                            ,PD.OpenerCommission as [Opener Commission]
	                                            ,PD.MiscRep1 as [Misc Rep1]
	                                            ,PD.MiscCommission1 as [Misc Commission 1]
	                                            ,PD.MiscRep2 as [Misc Rep2]
	                                            ,PD.MiscCommission2 as [Misc Commission 2]
	                                            ,PD.MiscRep3 as [Misc Rep3]
	                                            ,PD.MiscCommission3 as [Misc Commission 3]
	                                            ,PD.MiscRep4 as [Misc Rep4]
	                                            ,PD.MiscCommission4 as [Misc Commission 4]
	                                            ,PD.MiscRep5 as [Misc Rep5]
	                                            ,PD.MiscCommission5 as [Misc Commission 5]
                                                ,PD.MiscPaidDate as [Misc Paid Date]
                                from PayrollDetail PD
                                where PD.RMAAccountNo is not null
                                {0}
                                {1}
                                Order by RMAAccountNo desc";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
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

        public DataTable CancelQueueReportListForDownload(DateTime? Start, DateTime? End,string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusExt.BrinksCancelDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo like '%{0}%'", searchtext);
            }
            string sqlQuery = @"

                                select 
                                                 cus.Id
                                                ,cus.CustomerNo
	                                            ,cusExt.BrinksCancelDate
	                               
                                into #CancelQueueData

                                from Customer cus
                                left join CustomerExtended cusExt on cusExt.CustomerId =  cus.CustomerId
                              where cus.CustomerNo != '' and cus.CustomerNo is not null and cusExt.BrinksCancelDate != ''
                                {0}
                                {1}
								SELECT  *
	                                                FROM #CancelQueueData #CD
	                                             
	                                                Order by CustomerNo desc
	                                            
	                                                DROP TABLE #CancelQueueData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
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
        public DataTable BrinksCustomerReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusExt.BrinksFundingDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo like '%{0}%'", searchtext);
            }
            string sqlQuery = @"

                                select 
                                                 cus.Id
                                                ,cus.CustomerNo as Account
	                                            ,cusExt.BrinksFundingDate
                                                ,cusExt.ReceivedDate
                                                ,cusExt.GrossFundedAmount
	                               
                                into #CustomerListData

                                from Customer cus
                                left join CustomerExtended cusExt on cusExt.CustomerId =  cus.CustomerId
                              where cus.CustomerNo != '' and cus.CustomerNo is not null 
                                {0}
                                {1}
								SELECT  *
	                                                FROM #CustomerListData #CD
	                                             
	                                                Order by Account desc
	                                            
	                                                DROP TABLE #CustomerListData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
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
        public DataTable BrinksFundingVerificationListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusExt.FinanceFundingDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo like '%{0}%'", searchtext);
            }
            string sqlQuery = @"

                                select 
                                                cus.Id
                                                ,cus.CustomerNo  [CS#]
                                                ,case when cusExt.FinanceCompany = '-1' then '' else cusExt.FinanceCompany END as FinanceCompany
                                                ,cusExt.FinanceFundingDate
	                                            ,cusExt.LoanAmount
                                                ,cusExt.PayOut
                                                ,cusExt.NewMMR
                                                ,cusExt.Plancode
	                               
                                into #FVData

                                from Customer cus
                                left join CustomerExtended cusExt on cusExt.CustomerId =  cus.CustomerId
                              where cus.CustomerNo != '' and cus.CustomerNo is not null 
                                {0}
                                {1}
								SELECT  *
	                                                FROM #FVData #CD
	                                             
	                                                Order by  [CS#] desc
	                                            
	                                                DROP TABLE #FVData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
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
        #endregion

        #region Global Ticket Search
        public DataTable GetTicketByKeyAndCompanyId(Guid companyId, string key, string emptag, Guid empid)
        {

            string sqlQuery = @"select
                                tk.Id as TicketId
                                ,cus.Id as CusIntId
                                ,tk.TicketType
                                ,tk.[Status]
                                ,cus.BusinessName
                                ,cus.PrimaryPhone
                                ,cus.FirstName +' '+ cus.LastName as CustomerName
                                ,tk.CreatedDate
                                ,tk.CompletionDate
                                ,emp.FirstName +' '+ emp.LastName as AssignTo
                                from Ticket tk
                                left join  Customer cus on cus.CustomerId = tk.CustomerId
                                left join Ticketuser tu on tu.TiketId = tk.TicketId
                                left join Employee emp on emp.UserId = tu.UserId
                                where (cus.FirstName +' ' +cus.LastName like @SearchText
				                            or cus.Title like @SearchText
                                            or cus.BusinessName like @SearchText
                                            or cus.DBA like @SearchText
                                            OR CONVERT(nvarchar(15), tk.Id) like @SearchText)
                                and tk.Id is not null
                                and tk.CompanyId = '{0}'
                                {2}
                                ";


            string subquery = "";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, key, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", key)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Ticket Tech
        public DataTable GetTicketTechByTicketId(Guid TicketId)
        {
            string sqlQuery = @"select emp.firstname +' '+ emp.lastname as TechName
                                from employee emp
                                left join TicketUser tu on tu.UserId = emp.UserId
                                where tu.tiketid = '{0}'
                                and NotificationOnly = 0 and IsPrimary = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, TicketId);
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
        #endregion
        public DataSet GetAllPublicTicketReplyByTicketId(Guid ticketId)
        {

            string sqlQuery = @"select tr.[Message],
                                emp.FirstName +' '+ emp.LastName as NoteBy,
                                tr.RepliedDate

                                from TicketReply tr
                                left join employee emp on tr.UserId = emp.UserId
                                where tr.ticketid = '{0}'
                                and IsPrivate = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
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
