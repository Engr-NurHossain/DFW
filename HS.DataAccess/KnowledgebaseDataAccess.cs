using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
	public partial class KnowledgebaseDataAccess
	{
        public DataSet GetKnowledgebasebyFilter(QtiFilter filter)
        {
            string searchQuery = "";
            string orderQuery = "";
            string orderQuery1 = "";
            string IsDocumentLibraryQuery = "";
            string tagquery = "";
            string IsFlagquery = "";
            string Favoritequery = "";
            string IsFavorite = "";
            string IsHiddenQuery = "";
            string IsDeletedQuery = "";
            string NavQuery = "";
            string ContentQuery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/title")
                {
                    orderQuery = "order by #tp.Title asc";
                    orderQuery1 = "order by Title asc";
                }
                else if (filter.Order == "descending/title")
                {
                    orderQuery = "order by #tp.Title desc";
                    orderQuery1 = "order by Title desc";
                }
                else if (filter.Order == "ascending/tag")
                {
                    orderQuery = "order by #tp.Tags asc";
                    orderQuery1 = "order by Tags asc";
                }
                else if (filter.Order == "descending/tag")
                {
                    orderQuery = "order by #tp.Tags desc";
                    orderQuery1 = "order by Tags desc";
                }
                else if (filter.Order == "ascending/date")
                {
                    orderQuery = "order by #tp.LastUpdatedDate asc";
                    orderQuery1 = "order by LastUpdatedDate asc";
                }
                else if (filter.Order == "descending/date")
                {
                    orderQuery = "order by #tp.LastUpdatedDate desc";
                    orderQuery1 = "order by LastUpdatedDate desc";
                }
                else
                {
                    orderQuery = "order by #tp.IdP asc";
                    orderQuery1 = "order by IdP asc";
                }

            }
            else
            {
                orderQuery = "order by #tp.IdP asc";
                orderQuery1 = "order by IdP asc";
            }
            #endregion
            if (filter.IsDocumentLibrary)
            {
                IsDocumentLibraryQuery = string.Format("and kn.IsDocumentLibrary = 1");
            }
            else
            {
                IsDocumentLibraryQuery = string.Format("and kn.IsDocumentLibrary = 0");
            }
            if (filter.IsFlaged && filter.UserRole.IndexOf("admin") > -1)
            {
                IsFlagquery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgeBaseFlagUser where IsFlag=1 and IsDocument = 0))");
            }
            //if (filter.IsFavorite && filter.UserRole.IndexOf("admin") > -1)
            //{
            //    Favoritequery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgebaseFavouriteUser where IsFavourite = 1))");
            //}
            //if (filter.IsFavorite)
            //{
            //    IsFavorite = string.Format(@" and  fav = 1");
            //}
            if (filter.IsFlaged && filter.UserRole.IndexOf("admin") == -1)
            {
                IsFlagquery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgeBaseFlagUser where IsFlag=1 AND CreatedBy='{0}' and IsDocument = 0))", filter.UserId);
            }
            if (!string.IsNullOrWhiteSpace(filter.Tag) && filter.Tag != "null")
            {
                var array = filter.Tag.Split(",");
                if (array != null)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrWhiteSpace(tagquery))
                        {
                            tagquery += string.Format("and (kn.Tags like '%{0}%'", item);
                        }
                        else
                        {
                            tagquery += string.Format(" or kn.Tags like '%{0}%'", item);
                        }
                    }
                    tagquery = tagquery + ")";
                }
            }
            if (filter.NavList != null && filter.NavList.Count > 0 && !string.IsNullOrWhiteSpace(filter.NavList[0]) && filter.IsContact.ToLower() == "true")
            {
                var array = filter.NavList[0].Split(",");
                if (array != null)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrWhiteSpace(NavQuery))
                        {
                            NavQuery += string.Format("and (kn.Tags like '%{0}%'", item);
                        }
                        else
                        {
                            NavQuery += string.Format(" or kn.Tags like '%{0}%'", item);
                        }
                    }
                    NavQuery = NavQuery + ")";
                }
            }
            if (!filter.UserRole.ToLower().Contains("admin"))
            {
                IsHiddenQuery = string.Format("and kn.IsHidden = 0");
            }
            if (filter.IsDeleted)
            {
                IsDeletedQuery = string.Format("and kn.IsDeleted = 1");
            }
            else
            {
                IsDeletedQuery = string.Format("and kn.IsDeleted = 0");
            }
            string quuery = "";
            if (filter.UserRole.ToLower().IndexOf("admin") == -1)
            {
                quuery = string.Format(@" AND ku.UserId='{0}'", filter.UserId);
            }
            string mainQuery = String.Format(@"
                                select top(@pagesize)
								* into #tempKnowledge2 from #tempKnowledge1
								where IdP not in(select top(@pagestart) #tp.IdP from #tempKnowledge1 #tp {0})
                                {1}
                                
                                select * from #tempKnowledge2 {1}

                                select count(*) CountTotal from #tempKnowledge1
                                --select count(*) DeletedCount from Knowledgebase  where IsDeleted = 1 and IsDocumentLibrary = 0

                                select count(DISTINCT KnowledgebaseId) DeletedCount from KnowledgeBaseFlagUser ku
								left join Knowledgebase kn on kn.Id=ku.KnowledgebaseId
								where ku.IsFlag=1 and kn.IsDeleted = 1 and ku.IsDocument = 0 

                                select count(DISTINCT KnowledgebaseId) TotalKnFlagCount from KnowledgeBaseFlagUser ku
								left join Knowledgebase kn on kn.Id=ku.KnowledgebaseId
								where ku.IsFlag=1 {3} and ku.IsDocument = 0 {2}

                                

                                   select count(DISTINCT KnowledgebaseId) TotalKnFavoriteCount from KnowledgebaseFavouriteUser ku
									left join Knowledgebase kn on kn.Id=ku.KnowledgebaseId
									where ku.IsFavourite=1 and kn.IsDeleted = 0 and  UserId ='{4}'

                                select count(DISTINCT KnowledgebaseId) TotalKnDeleteFavoriteCount from KnowledgebaseFavouriteUser ku
								left join Knowledgebase kn on kn.Id=ku.KnowledgebaseId
								where ku.IsFavourite=1 and kn.IsDeleted = 1 and  UserId ='{4}'

                                ", orderQuery, orderQuery1, quuery, IsDeletedQuery, filter.UserId);
            int localmin = DateTime.UtcNow.UTCToClientTimeMin();
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchQuery = string.Format(@"and (kn.Title LIKE '%{0}%' or kn.Tags LIKE '%{0}%' or kn.Answer LIKE '%{0}%')", filter.SearchText);
            }
            if (filter.IsFavorite)
            {
                IsFavorite = string.Format(@"and fav = 1");
            }
            if (filter.IsContent)
            {
                ContentQuery = string.Format(@"t1.Answer as [Content],");
            }
            if (filter.IsDownload)
            {
                string LinkedArticlesQuery = "";
                if (filter.IsDeleted)
                {
                    LinkedArticlesQuery = String.Format(@",(select STUFF((
										SELECT ', '  +  kk.Title + ' '
										from Knowledgebase kk 
												where Id in (select KnowledgebaseId from KnowledgebaseWebLink where link like '%'+cast(t1.Id as nvarchar(50)))
											FOR XML PATH('')
												), 1, 2, '')
										) AS [Linked Articles]");
                }
                mainQuery = String.Format(@"
                                select 
								* into #tempKnowledge2 from #tempKnowledge1
								where Id not in(select top(@pagestart) #tp.Id from #tempKnowledge1 #tp {0})
                                {1}
                                
                                select 
                                t1.Id,
								t1.Title,
                                {3}
                                (select STUFF((
										SELECT ', '  +  cw.Title + ' '
										from KnowledgebaseWeblink cw 
												left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
												where cw.KnowledgebaseId = t1.Id
													and cw.IsRelated = 0
											FOR XML PATH('')
												), 1, 2, '')
										) AS [Hyperlinks],
								(select STUFF((
										SELECT ', '  +  k.Title + ' '
										from KnowledgebaseWeblink cw 
												left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
												where cw.KnowledgebaseId = t1.Id
													and cw.IsRelated = 1
											FOR XML PATH('')
												), 1, 2, '')
										) AS [Related Articals],
								TagName as [Tags],
								emp.FirstName +' '+ emp.LastName as [Created By],
								FORMAT(t1.CreatedDate,'M/d/yy') as [Created Date],
								t1.UpdatedBy as [Last Updated By],
								FORMAT(t1.LastUpdatedDate,'M/d/yy') as [Last Updated Date],
								iif((select count(*) From KnowledgeBaseFlagUser where knowledgebaseid = t1.Id and IsFlag = 1 and IsDocument = 0) > 0,'Yes','No') as Flaged,
								emp2.FirstName +' '+ emp2.LastName as [Flaged By],
								FORMAT(t1.FlagDate,'M/d/yy') as [Flaged Date],
								t1.AttachmentCount as [Attachment Count]
                                {2}
                                from #tempKnowledge1 t1
                                left join Employee emp on emp.UserId = t1.CreatedBy
								left join Employee emp2 on emp2.UserId = t1.FlagBy
                                {1}
                                ", orderQuery, orderQuery1, LinkedArticlesQuery, ContentQuery);
            }

            string sqlQuery = @"
                                declare @pagestart int
                                set @pagestart=(@pageno - 1) * @pagesize

                                select kn.*,
                                (STUFF((
								SELECT', '  +  TagName+ ' '
								FROM KnowledgebaseRMRTag tg
								where tg.Id in(select TagId from KnowledgebaseRMRTagMap where KnowledgebaseId = kn.Id and IsDeleted = 0) 
									and tg.IsDeleted = 0	
									FOR XML PATH('')
										), 1, 2, '')
								) AS TagName,
                                emp.FirstName +' '+ emp.LastName as [UpdatedBy]
								,(select count(*) from EstimateImage Where InvoiceId = kn.Id and IsDocument = 0) as AttachmentCount
                                ,(select IsFavourite from KnowledgebaseFavouriteUser where kn.Id = KnowledgebaseId and UserId ='{10}') as fav
                                into #tempKnowledge
								from Knowledgebase kn
								left join Employee emp on emp.UserId = kn.LastUpdatedBy
                                where kn.Id > 0 {7} {2} {3} {4} {5} {6} 
                                {0}

                                CREATE TABLE #tempKnowledge1
								(
								IDP INT IDENTITY(1, 1) primary key ,
								Id int,
								Title NVARCHAR(500),
								Answer NVARCHAR(max),
								Tags NVARCHAR(100),
								CreatedBy uniqueidentifier,
								CreatedDate datetime,
								LastUpdatedBy uniqueidentifier,
								LastUpdatedDate datetime,
								IsDeleted bit,
								IsDocumentLibrary bit,
								IsFlag bit,
								FlagBy uniqueidentifier,
								FlagDate datetime,
								IsHidden bit,
								TagName NVARCHAR(500),
								UpdatedBy NVARCHAR(100),
                                AttachmentCount int,
                                fav bit
								);

                                insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title LIKE '%{8}%' and Tags LIKE '%{8}%' and Answer LIKE '%{8}%'         {9}   order by Title asc 
								insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title LIKE '%{8}%' and Tags LIKE '%{8}%' and Answer not LIKE '%{8}%'     {9}   order by Title asc 
								insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title LIKE '%{8}%' and Tags not LIKE '%{8}%' and Answer LIKE '%{8}%'     {9}   order by Title asc 
								insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title LIKE '%{8}%' and Tags not LIKE '%{8}%' and Answer not LIKE '%{8}%' {9}   order by Title asc 
								insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title not LIKE '%{8}%' and Tags LIKE '%{8}%' and Answer LIKE '%{8}%'     {9}   order by Title asc 
								insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title not LIKE '%{8}%' and Tags LIKE '%{8}%' and Answer not LIKE '%{8}%' {9}   order by Title asc 
								insert into #tempKnowledge1 select Id,Title,Answer,Tags,CreatedBy,CreatedDate,LastUpdatedBy,LastUpdatedDate,IsDeleted,IsDocumentLibrary,IsFlag,FlagBy,FlagDate,IsHidden,TagName,UpdatedBy,AttachmentCount,fav from #tempKnowledge where Title not LIKE '%{8}%' and Tags not LIKE '%{8}%' and Answer LIKE '%{8}%' {9}   order by Title asc 
       
								{1}
								drop table #tempKnowledge
								drop table #tempKnowledge1
                                drop table #tempKnowledge2";
            try
            {
                sqlQuery = string.Format(sqlQuery, searchQuery, mainQuery, IsDocumentLibraryQuery, tagquery, IsFlagquery, IsHiddenQuery, NavQuery, IsDeletedQuery, filter.SearchText, IsFavorite, filter.UserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetKnowledgebaseWithTagName(int Id)
        {
            string sqlQuery = @"select kn.* 
                                ,(STUFF((
								        SELECT', '  +  TagName+ ' '
								        FROM KnowledgebaseRMRTag tg
								        where tg.Id in(select TagId from KnowledgebaseRMRTagMap where KnowledgebaseId = kn.Id and IsDeleted = 0) 
										and tg.IsDeleted = 0
									    FOR XML PATH('')
									), 1, 2, '')
								) AS TagName
                                from Knowledgebase kn
                                Where kn.Id ={0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, Id);
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
 
        public List<Knowledgebase> GetListOfDeletedKnowledgebase(int id)
        {
            string sqlQuery = @"select * from Knowledgebase
                                where Id in (select KnowledgebaseId from KnowledgebaseWebLink where link like '%{0}')";
            try
            {
                sqlQuery = string.Format(sqlQuery, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetRecentViewedKnowledgebaseList(Guid UserId)
        {
            DateTime Today = DateTime.UtcNow.SetMaxHour();
            DateTime PreviousDay = DateTime.UtcNow.SetZeroHour().AddDays(-30);
            string sqldate = string.Format("and VisitedDate between '{0}' and '{1}'", PreviousDay, Today);
            string searcheddate = string.Format("and SearchedDate between '{0}' and '{1}'", PreviousDay, Today);

            string sqlQuery = @"select * into #History from KnowledgebaseAccessedHistory where VisitedBy = '{0}' and IsDocumentLibrary = 0
                                    select top(10) max(Id) as Id, 
                                    max(VisitedDate) as VisitedDate, KnowledgebaseId 
                                    into #temptable
                                    from #History 
                                    group by KnowledgebaseId order by VisitedDate desc

                                    select tt.*, kn.Title from #temptable tt
                                    left join Knowledgebase kn on kn.Id = tt.KnowledgebaseId
                                    Where kn.IsDeleted = 0

                                    drop table #History
                                    drop table #temptable


                                    select distinct top(10) kn.id, kn.Title,
                                    (select count(*) from KnowledgebaseAccessedHistory where KnowledgebaseId = kn.Id {1}) as TotalCount
                                    from KnowledgebaseAccessedHistory knah
                                    left join Knowledgebase kn on kn.Id = knah.KnowledgebaseId
                                    where kn.id is not null and kn.IsDeleted = 0 {1}
                                    order by TotalCount desc, Title asc


                                    select distinct top(10) keyword,
                                    (select count(*) from KnowledgeSearchedKeyword where Keyword = kw.Keyword {2}) as TotalCount
                                    from KnowledgeSearchedKeyword kw
                                    where keyword is not null {2}
                                    order by TotalCount desc, keyword asc

                                    select * from KnowledgebaseRMRTag 
                                    where IsknowledgebaseNav = 1 and IsDeleted = 0 and IsFavourite = 1
                                    order by TagName asc

                                    select count(DISTINCT KnowledgebaseId) TotalKnFlagCount from KnowledgeBaseFlagUser ku
									left join Knowledgebase kn on kn.Id=ku.KnowledgebaseId
									where ku.IsFlag=1 and ku.IsDocument = 0 --and kn.IsDeleted=0

                                     select count(DISTINCT KnowledgebaseId) TotalKnFavoriteCount from KnowledgebaseFavouriteUser ku
									left join Knowledgebase kn on kn.Id=ku.KnowledgebaseId
									where ku.IsFavourite=1 and kn.IsDeleted=0 and ku.UserId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, sqldate, searcheddate);
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
        public DataSet GetKnowledgebaseListByTag(string Tag, string Order, string UserRole)
        {
            string searchQuery = "";
            string orderQuery = "";
            string IsHiddenQuery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/article")
                {
                    orderQuery = "order by Title asc";
                }
                else if (Order == "descending/article")
                {
                    orderQuery = "order by Title desc";
                }
                else if (Order == "ascending/tag")
                {
                    orderQuery = "order by Tags asc";
                }
                else if (Order == "descending/tag")
                {
                    orderQuery = "order by Tags desc";
                }
                else if (Order == "ascending/type")
                {
                    orderQuery = "order by IsDocumentLibrary desc";
                }
                else if (Order == "descending/type")
                {
                    orderQuery = "order by IsDocumentLibrary asc";
                }
                else
                {
                    orderQuery = "order by Title asc";
                }
            }
            else
            {
                orderQuery = "order by Title asc";
            }
            #endregion

            if (!UserRole.ToLower().Contains("admin"))
            {
                IsHiddenQuery = string.Format("and kn.IsHidden = 0");
            }

            if (!string.IsNullOrWhiteSpace(Tag))
            {
                searchQuery = string.Format("and kn.Tags like '%{0}%'", Tag);
            }

            string sqlQuery = @"
                                select  CAST(kn.Id AS int) Id, kn.Title, kn.Answer, kn.Tags, kn.IsDocumentLibrary,
                                (STUFF((
								SELECT', '  +  TagName+ ' '
								FROM KnowledgebaseRMRTag tg
								where tg.Id in(select TagId from KnowledgebaseRMRTagMap where KnowledgebaseId = kn.Id and IsDeleted = 0) 
									and tg.IsDeleted = 0	
									FOR XML PATH('')
										), 1, 2, '')
								) AS TagName,
                                kn.LastUpdatedBy, kn.LastUpdatedDate, emp.FirstName +' '+ emp.LastName as [UpdatedBy]
								,(select count(*) from EstimateImage Where InvoiceId = kn.Id) as AttachmentCount

								from Knowledgebase kn
								left join Employee emp on emp.UserId = kn.LastUpdatedBy
                                where kn.IsDeleted = 0 
                                {0} {1} {2}";
            try
            {
                sqlQuery = string.Format(sqlQuery, searchQuery, IsHiddenQuery, orderQuery);
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
        public DataSet GetKnowledgebasebyFilterForClassroom(QtiFilter filter)
        {
            string searchQuery = "";
            string orderQuery = "";
            string orderQuery1 = "";
            string IsDocumentLibraryQuery = "";
            string tagquery = "";
            string IsFlagquery = "";
            string IsHiddenQuery = "";
            string IsDeletedQuery = "";
            string NavQuery = "";
            string ContentQuery = "";
            string IsCompleted = "";
            string dateQuery = "";
            var assginedToQuery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/title")
                {
                    orderQuery = "order by #tp.Title asc";
                    orderQuery1 = "order by Title asc";
                }
                else if (filter.Order == "descending/title")
                {
                    orderQuery = "order by #tp.Title desc";
                    orderQuery1 = "order by Title desc";
                }
                else if (filter.Order == "ascending/tag")
                {
                    orderQuery = "order by #tp.TagName asc";
                    orderQuery1 = "order by TagName asc";
                }
                else if (filter.Order == "descending/tag")
                {
                    orderQuery = "order by #tp.TagName desc";
                    orderQuery1 = "order by TagName desc";
                }
                else if (filter.Order == "ascending/assignedby")
                {
                    orderQuery = "order by #tp.AssignedDate asc";
                    orderQuery1 = "order by AssignedDate asc";
                }
                else if (filter.Order == "descending/assignedby")
                {
                    orderQuery = "order by #tp.AssignedDate desc";
                    orderQuery1 = "order by AssignedDate desc";
                }
                else if (filter.Order == "ascending/assignto")
                {
                    orderQuery = "order by #tp.AssignedTo asc";
                    orderQuery1 = "order by AssignedTo asc";
                }
                else if (filter.Order == "descending/assignto")
                {
                    orderQuery = "order by #tp.AssignedTo desc";
                    orderQuery1 = "order by AssignedTo desc";
                }
                else if (filter.Order == "ascending/duedate")
                {
                    orderQuery = "order by #tp.DueDate asc";
                    orderQuery1 = "order by DueDate asc";
                }
                else if (filter.Order == "descending/duedate")
                {
                    orderQuery = "order by #tp.DueDate desc";
                    orderQuery1 = "order by DueDate desc";
                }
                else if (filter.Order == "ascending/completedate")
                {
                    orderQuery = "order by #tp.EndDate asc";
                    orderQuery1 = "order by EndDate asc";
                }
                else if (filter.Order == "descending/completedate")
                {
                    orderQuery = "order by #tp.EndDate desc";
                    orderQuery1 = "order by EndDate desc";
                }
                else
                {
                    orderQuery = "order by #tp.AssignedDate asc, #tp.Title asc";
                    orderQuery1 = "order by AssignedDate asc, Title asc";
                }

            }
            else
            {
                orderQuery = "order by #tp.AssignedDate asc, #tp.Title asc";
                orderQuery1 = "order by AssignedDate asc, Title asc";
            }
            #endregion
            if (filter.IsDocumentLibrary)
            {
                IsDocumentLibraryQuery = string.Format("and kn.IsDocumentLibrary = 1");
            }
            else
            {
                IsDocumentLibraryQuery = string.Format("and kn.IsDocumentLibrary = 0");
            }
            if (filter.IsFlaged && filter.UserRole.IndexOf("admin") > -1)
            {
                IsFlagquery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgeBaseFlagUser where IsFlag=1 and IsDocument = 0))");
            }
            if (filter.IsFlaged && filter.UserRole.IndexOf("admin") == -1)
            {
                IsFlagquery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgeBaseFlagUser where IsFlag=1 AND CreatedBy='{0}' and IsDocument = 0))", filter.UserId);
            }
            if (!string.IsNullOrWhiteSpace(filter.assignTo) && filter.assignTo != "null")
            {
                var assignedArray = filter.assignTo.Split(",");
                var formattedAssignTo = string.Join("','", assignedArray.Select(x => x.Trim()));
                assginedToQuery = string.Format("and emp3.UserId in ('{0}')", formattedAssignTo);

            } 
            if (filter.StartTime != new DateTime() && filter.EndTime != new DateTime())
            {
                dateQuery = string.Format(" AND ka.AssignedDate BETWEEN '{0}' AND '{1}' ", filter.StartTime.SetZeroHour().ToString("yyyy/MM/dd HH:mm"), filter.EndTime.SetMaxHour().ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(filter.Tag) && filter.Tag != "null")
            {
                var array = filter.Tag.Split(",");
                if (array != null)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrWhiteSpace(tagquery))
                        {
                            tagquery += string.Format("and (kn.Tags like '%{0}%'", item);
                        }
                        else
                        {
                            tagquery += string.Format(" or kn.Tags like '%{0}%'", item);
                        }
                    }
                    tagquery = tagquery + ")";
                }
            }
            if (filter.NavList != null && filter.NavList.Count > 0 && !string.IsNullOrWhiteSpace(filter.NavList[0]) && filter.NavList[0] != "null")
            {
                var array = filter.NavList[0].Split(",");
                if (array != null)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrWhiteSpace(NavQuery))
                        {
                            NavQuery += string.Format("and (kn.Tags like '%{0}%'", item);
                        }
                        else
                        {
                            NavQuery += string.Format(" or kn.Tags like '%{0}%'", item);
                        }
                    }
                    NavQuery = NavQuery + ")";
                }
            }
            if (!filter.UserRole.ToLower().Contains("admin"))
            {
                IsHiddenQuery = string.Format("and kn.IsHidden = 0");
            }
            if (filter.IsDeleted)
            {
                IsDeletedQuery = string.Format("kn.IsDeleted = 1");
            }
            else
            {
                IsDeletedQuery = string.Format("kn.IsDeleted = 0");
            }
            if (filter.IsComplete.HasValue && filter.IsComplete.Value)
            {
                IsCompleted = string.Format("and ka.IsRead = 1");
            }
            else if (filter.IsComplete.HasValue && !filter.IsComplete.Value)
            {
                IsCompleted = string.Format("and ka.IsRead = 0");
            }
            string quuery = "";
            string favoritequery = "";
            //if (filter.UserRole.ToLower().IndexOf("admin") == -1)
            if (!filter.IsAdmin)
            {
                quuery = string.Format(@" and ka.AssignedUser='{0}'", filter.UserId);
            }
            if (filter.UserId != null)
            {
                favoritequery = string.Format(@"  and UserId ='{0}'", filter.UserId);
            }
            string mainQuery = String.Format(@"
                                select top(@pagesize)
        * into #tempKnowledge2 from #tempKnowledge
        where paginationid not in(select top(@pagestart) #tp.paginationid from #tempKnowledge #tp {0})
                                {1}

                                select * from #tempKnowledge2 {1}

                                select count(*) CountTotal from #tempKnowledge
                                ", orderQuery, orderQuery1);
            int localmin = DateTime.UtcNow.UTCToClientTimeMin();
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchQuery = string.Format(@"and (kn.Title LIKE '%{0}%' or kn.Tags LIKE '%{0}%' or kn.Answer LIKE '%{0}%')", filter.SearchText);
            }
            if (filter.IsDownload)
            {
                string LinkedArticlesQuery = "";
                if (filter.IsDeleted)
                {
                    LinkedArticlesQuery = String.Format(@",(select STUFF((
        		SELECT ', '  +  kk.Title + ' '
        		from Knowledgebase kk 
        				where Id in (select KnowledgebaseId from KnowledgebaseWebLink where link like '%'+cast(t1.Id as nvarchar(50)))
        			FOR XML PATH('')
        				), 1, 2, '')
        		) AS [Linked Articles]");
                }
                mainQuery = String.Format(@"
                                select 
        * into #tempKnowledge2 from #tempKnowledge1
        where Id not in(select top(@pagestart) #tp.Id from #tempKnowledge1 #tp {0})
                                {1}

                                select 
                                t1.Id,
        t1.Title,
                                {3}
                                (select STUFF((
        		SELECT ', '  +  cw.Title + ' '
        		from KnowledgebaseWeblink cw 
        				left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
        				where cw.KnowledgebaseId = t1.Id
        					and cw.IsRelated = 0
        			FOR XML PATH('')
        				), 1, 2, '')
        		) AS [Hyperlinks],
        (select STUFF((
        		SELECT ', '  +  k.Title + ' '
        		from KnowledgebaseWeblink cw 
        				left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
        				where cw.KnowledgebaseId = t1.Id
        					and cw.IsRelated = 1
        			FOR XML PATH('')
        				), 1, 2, '')
        		) AS [Related Articals],
        TagName as [Tags],
        emp.FirstName +' '+ emp.LastName as [Created By],
        FORMAT(t1.CreatedDate,'M/d/yy') as [Created Date],
        t1.UpdatedBy as [Last Updated By],
        FORMAT(t1.LastUpdatedDate,'M/d/yy') as [Last Updated Date],
        iif((select count(*) From KnowledgeBaseFlagUser where knowledgebaseid = t1.Id and IsFlag = 1 and IsDocument = 0) > 0,'Yes','No') as Flaged,
        emp2.FirstName +' '+ emp2.LastName as [Flaged By],
        FORMAT(t1.FlagDate,'M/d/yy') as [Flaged Date],
        t1.AttachmentCount as [Attachment Count]
                                {2}
                                from #tempKnowledge1 t1
                                left join Employee emp on emp.UserId = t1.CreatedBy
        left join Employee emp2 on emp2.UserId = t1.FlagBy
                                {1}
                                ", orderQuery, orderQuery1, LinkedArticlesQuery, ContentQuery);
            }

            string sqlQuery = @"
                                declare @pagestart int
                                set @pagestart=(@pageno - 1) * @pagesize

                                select IDENTITY(INT, 1, 1) AS paginationid,kn.*,
                                (STUFF((
        SELECT', '  +  TagName+ ' '
        FROM KnowledgebaseRMRTag tg
        where tg.Id in(select TagId from KnowledgebaseRMRTagMap where KnowledgebaseId = kn.Id and IsDeleted = 0) 
        	and tg.IsDeleted = 0	
        	FOR XML PATH('')
        		), 1, 2, '')
        ) AS TagName,
                                emp.FirstName +' '+ emp.LastName as [UpdatedBy]
        ,(select count(*) from EstimateImage Where InvoiceId = kn.Id and IsDocument = 0) as AttachmentCount
                                ,(select IsFavourite from KnowledgebaseFavouriteUser where kn.Id = KnowledgebaseId {11} ) as fav
                                ,ka.AssignedDate
                                ,DATEADD(day, cast((select [Value] from GlobalSetting where searchkey ='AssignedArticlesDefaultDueDate') as int), ka.AssignedDate) as DueDate
        ,ka.EndDate
                                ,emp2.FirstName+' '+ emp2.LastName as [AssignedBy]
        ,emp3.FirstName+' '+ emp3.LastName as [AssignedTo]
                                ,ka.IsRead
                                into #tempKnowledge
        from Knowledgebase kn
        left join Employee emp on emp.UserId = kn.LastUpdatedBy
                                left join KnowledgebaseAccountability ka on ka.KnowledgebaseId = kn.Id
        left join Employee emp2 on emp2.UserId = ka.AssignedBy
        left join Employee emp3 on emp3.UserId = ka.AssignedUser
                                where {7} {2} {3} {4} {5} {6} and ka.AssignedUser is not null {9} {10} {12} {13}
                                {0}

        {1}
        drop table #tempKnowledge
                                drop table #tempKnowledge2";
            try
            {
                sqlQuery = string.Format(sqlQuery, searchQuery, mainQuery, IsDocumentLibraryQuery, tagquery, IsFlagquery, IsHiddenQuery, NavQuery, IsDeletedQuery, filter.SearchText, quuery, IsCompleted, favoritequery,dateQuery,assginedToQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public DataSet GetKnowledgebasebyFilterForClassroom(QtiFilter filter)
        //{
        //    string searchQuery = "";
        //    string orderQuery = "";
        //    string orderQuery1 = "";
        //    string IsDocumentLibraryQuery = "";
        //    string tagquery = "";
        //    string IsFlagquery = "";
        //    string IsHiddenQuery = "";
        //    string IsDeletedQuery = "";
        //    string NavQuery = "";
        //    string ContentQuery = "";
        //    string IsCompleted = "";
        //    string assginedToQuery = "";
        //    string dateQuery = "";
        //    #region Order
        //    if (!string.IsNullOrWhiteSpace(filter.Order))
        //    {
        //        if (filter.Order == "ascending/title")
        //        {
        //            orderQuery = "order by #tp.Title asc";
        //            orderQuery1 = "order by Title asc";
        //        }
        //        else if (filter.Order == "descending/title")
        //        {
        //            orderQuery = "order by #tp.Title desc";
        //            orderQuery1 = "order by Title desc";
        //        }
        //        else if (filter.Order == "ascending/tag")
        //        {
        //            orderQuery = "order by #tp.TagName asc";
        //            orderQuery1 = "order by TagName asc";
        //        }
        //        else if (filter.Order == "descending/tag")
        //        {
        //            orderQuery = "order by #tp.TagName desc";
        //            orderQuery1 = "order by TagName desc";
        //        }
        //        else if (filter.Order == "ascending/assignedby")
        //        {
        //            orderQuery = "order by #tp.AssignedDate asc";
        //            orderQuery1 = "order by AssignedDate asc";
        //        }
        //        else if (filter.Order == "descending/assignedby")
        //        {
        //            orderQuery = "order by #tp.AssignedDate desc";
        //            orderQuery1 = "order by AssignedDate desc";
        //        }
        //        else if (filter.Order == "ascending/assignto")
        //        {
        //            orderQuery = "order by #tp.AssignedTo asc";
        //            orderQuery1 = "order by AssignedTo asc";
        //        }
        //        else if (filter.Order == "descending/assignto")
        //        {
        //            orderQuery = "order by #tp.AssignedTo desc";
        //            orderQuery1 = "order by AssignedTo desc";
        //        }
        //        else if (filter.Order == "ascending/duedate")
        //        {
        //            orderQuery = "order by #tp.DueDate asc";
        //            orderQuery1 = "order by DueDate asc";
        //        }
        //        else if (filter.Order == "descending/duedate")
        //        {
        //            orderQuery = "order by #tp.DueDate desc";
        //            orderQuery1 = "order by DueDate desc";
        //        }
        //        else if (filter.Order == "ascending/completedate")
        //        {
        //            orderQuery = "order by #tp.EndDate asc";
        //            orderQuery1 = "order by EndDate asc";
        //        }
        //        else if (filter.Order == "descending/completedate")
        //        {
        //            orderQuery = "order by #tp.EndDate desc";
        //            orderQuery1 = "order by EndDate desc";
        //        }
        //        else
        //        {
        //            orderQuery = "order by #tp.AssignedDate asc, #tp.Title asc";
        //            orderQuery1 = "order by AssignedDate asc, Title asc";
        //        }

        //    }
        //    else
        //    {
        //        orderQuery = "order by #tp.AssignedDate asc, #tp.Title asc";
        //        orderQuery1 = "order by AssignedDate asc, Title asc";
        //    }
        //    #endregion
        //    if (filter.IsDocumentLibrary)
        //    {
        //        IsDocumentLibraryQuery = string.Format("and kn.IsDocumentLibrary = 1");
        //    }
        //    else
        //    {
        //        IsDocumentLibraryQuery = string.Format("and kn.IsDocumentLibrary = 0");
        //    }
        //    if (filter.IsFlaged && filter.UserRole.IndexOf("admin") > -1)
        //    {
        //        IsFlagquery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgeBaseFlagUser where IsFlag=1 and IsDocument = 0))");
        //    }
        //    if (filter.IsFlaged && filter.UserRole.IndexOf("admin") == -1)
        //    {
        //        IsFlagquery = string.Format(@" and (kn.Id in (select KnowledgebaseId from KnowledgeBaseFlagUser where IsFlag=1 AND CreatedBy='{0}' and IsDocument = 0))", filter.UserId);
        //    }
        //    if (!string.IsNullOrWhiteSpace(filter.assignTo) && filter.assignTo != "null")
        //    {
        //        assginedToQuery = string.Format("and emp3.UserId in ('{0}')", filter.assignTo);
        //    }
        //    if (filter.StartTime != new DateTime() && filter.EndTime != new DateTime())
        //    {
        //        dateQuery = string.Format("AND ka.AssignedDate BETWEEN '{0}' AND '{1}' ", filter.StartTime.SetZeroHour().ToString("yyyy/MM/dd HH:mm"), filter.EndTime.SetMaxHour().ToString("yyyy/MM/dd HH:mm"));
        //    }

        //    if (!string.IsNullOrWhiteSpace(filter.Tag) && filter.Tag != "null")
        //    {
        //        var array = filter.Tag.Split(",");
        //        if (array != null)
        //        {
        //            foreach (var item in array)
        //            {
        //                if (string.IsNullOrWhiteSpace(tagquery))
        //                {
        //                    tagquery += string.Format("and (kn.Tags like '%{0}%'", item);
        //                }
        //                else
        //                {
        //                    tagquery += string.Format(" or kn.Tags like '%{0}%'", item);
        //                }
        //            }
        //            tagquery = tagquery + ")";
        //        }
        //    }
        //    if (filter.NavList != null && filter.NavList.Count > 0 && !string.IsNullOrWhiteSpace(filter.NavList[0]) && filter.NavList[0] != "null")
        //    {
        //        var array = filter.NavList[0].Split(",");
        //        if (array != null)
        //        {
        //            foreach (var item in array)
        //            {
        //                if (string.IsNullOrWhiteSpace(NavQuery))
        //                {
        //                    NavQuery += string.Format("and (kn.Tags like '%{0}%'", item);
        //                }
        //                else
        //                {
        //                    NavQuery += string.Format(" or kn.Tags like '%{0}%'", item);
        //                }
        //            }
        //            NavQuery = NavQuery + ")";
        //        }
        //    }
        //    if (!filter.UserRole.ToLower().Contains("admin"))
        //    {
        //        IsHiddenQuery = string.Format("and kn.IsHidden = 0");
        //    }
        //    if (filter.IsDeleted)
        //    {
        //        IsDeletedQuery = string.Format("kn.IsDeleted = 1");
        //    }
        //    else
        //    {
        //        IsDeletedQuery = string.Format("kn.IsDeleted = 0");
        //    }
        //    if (filter.IsComplete.HasValue && filter.IsComplete.Value)
        //    {
        //        IsCompleted = string.Format("and ka.IsRead = 1");
        //    }
        //    else if (filter.IsComplete.HasValue && !filter.IsComplete.Value)
        //    {
        //        IsCompleted = string.Format("and ka.IsRead = 0");
        //    }
        //    string quuery = "";
        //    string favoritequery = "";
        //    //if (filter.UserRole.ToLower().IndexOf("admin") == -1)
        //    if (!filter.IsAdmin)
        //    {
        //        quuery = string.Format(@" and ka.AssignedUser='{0}'", filter.UserId);
        //    }
        //    if (filter.UserId != null)
        //    {
        //        favoritequery = string.Format(@"  and UserId ='{0}'", filter.UserId);
        //    }
        //    string mainQuery = String.Format(@"
        //                        select top(@pagesize)
        //* into #tempKnowledge2 from #tempKnowledge
        //where paginationid not in(select top(@pagestart) #tp.paginationid from #tempKnowledge #tp {0})
        //                        {1}

        //                        select * from #tempKnowledge2 {1}

        //                        select count(*) CountTotal from #tempKnowledge
        //                        ", orderQuery, orderQuery1);
        //    int localmin = DateTime.UtcNow.UTCToClientTimeMin();
        //    if (!string.IsNullOrWhiteSpace(filter.SearchText))
        //    {
        //        searchQuery = string.Format(@"and (kn.Title LIKE '%{0}%' or kn.Tags LIKE '%{0}%')", filter.SearchText);
        //    }
        //    if (filter.IsDownload)
        //    {
        //        string LinkedArticlesQuery = "";
        //        if (filter.IsDeleted)
        //        {
        //            LinkedArticlesQuery = String.Format(@",(select STUFF((
        //		SELECT ', '  +  kk.Title + ' '
        //		from Knowledgebase kk 
        //				where Id in (select KnowledgebaseId from KnowledgebaseWebLink where link like '%'+cast(t1.Id as nvarchar(50)))
        //			FOR XML PATH('')
        //				), 1, 2, '')
        //		) AS [Linked Articles]");
        //        }
        //        mainQuery = String.Format(@"
        //                        select 
        //* into #tempKnowledge2 from #tempKnowledge1
        //where Id not in(select top(@pagestart) #tp.Id from #tempKnowledge1 #tp {0})
        //                        {1}

        //                        select 
        //                        t1.Id,
        //t1.Title,
        //                        {3}
        //                        (select STUFF((
        //		SELECT ', '  +  cw.Title + ' '
        //		from KnowledgebaseWeblink cw 
        //				left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
        //				where cw.KnowledgebaseId = t1.Id
        //					and cw.IsRelated = 0
        //			FOR XML PATH('')
        //				), 1, 2, '')
        //		) AS [Hyperlinks],
        //(select STUFF((
        //		SELECT ', '  +  k.Title + ' '
        //		from KnowledgebaseWeblink cw 
        //				left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
        //				where cw.KnowledgebaseId = t1.Id
        //					and cw.IsRelated = 1
        //			FOR XML PATH('')
        //				), 1, 2, '')
        //		) AS [Related Articals],
        //TagName as [Tags],
        //emp.FirstName +' '+ emp.LastName as [Created By],
        //FORMAT(t1.CreatedDate,'M/d/yy') as [Created Date],
        //t1.UpdatedBy as [Last Updated By],
        //FORMAT(t1.LastUpdatedDate,'M/d/yy') as [Last Updated Date],
        //iif((select count(*) From KnowledgeBaseFlagUser where knowledgebaseid = t1.Id and IsFlag = 1 and IsDocument = 0) > 0,'Yes','No') as Flaged,
        //emp2.FirstName +' '+ emp2.LastName as [Flaged By],
        //FORMAT(t1.FlagDate,'M/d/yy') as [Flaged Date],
        //t1.AttachmentCount as [Attachment Count]
        //                        {2}
        //                        from #tempKnowledge1 t1
        //                        left join Employee emp on emp.UserId = t1.CreatedBy
        //left join Employee emp2 on emp2.UserId = t1.FlagBy
        //                        {1}
        //                        ", orderQuery, orderQuery1, LinkedArticlesQuery, ContentQuery);
        //    }

        //    string sqlQuery = @"
        //                        declare @pagestart int
        //                        set @pagestart=(@pageno - 1) * @pagesize

        //                        select IDENTITY(INT, 1, 1) AS paginationid,kn.*,
        //                        (STUFF((
        //SELECT', '  +  TagName+ ' '
        //FROM KnowledgebaseRMRTag tg
        //where tg.Id in(select TagId from KnowledgebaseRMRTagMap where KnowledgebaseId = kn.Id and IsDeleted = 0) 
        //	and tg.IsDeleted = 0	
        //	FOR XML PATH('')
        //		), 1, 2, '')
        //) AS TagName,
        //                        emp.FirstName +' '+ emp.LastName as [UpdatedBy]
        //,(select count(*) from EstimateImage Where InvoiceId = kn.Id and IsDocument = 0) as AttachmentCount
        //                         ,(select IsFavourite from KnowledgebaseFavouriteUser where kn.Id = KnowledgebaseId {13} ) as fav
        //                        ,ka.AssignedDate
        //                        ,DATEADD(day, cast((select [Value] from GlobalSetting where searchkey ='AssignedArticlesDefaultDueDate') as int), ka.AssignedDate) as DueDate
        //,ka.EndDate
        //                        ,emp2.FirstName+' '+ emp2.LastName as [AssignedBy]
        //,emp3.FirstName+' '+ emp3.LastName as [AssignedTo]
        //                        ,ka.IsRead
        //                        into #tempKnowledge
        //from Knowledgebase kn
        //left join Employee emp on emp.UserId = kn.LastUpdatedBy
        //                        left join KnowledgebaseAccountability ka on ka.KnowledgebaseId = kn.Id
        //left join Employee emp2 on emp2.UserId = ka.AssignedBy
        //left join Employee emp3 on emp3.UserId = ka.AssignedUser
        //                        where {7} {2} {3} {4} {5} {6} and ka.AssignedUser is not null {9} {10}
        //                        {0}
        //                        {11} {12}
        //{1}
        //drop table #tempKnowledge
        //                        drop table #tempKnowledge2";
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, searchQuery, mainQuery, IsDocumentLibraryQuery, tagquery, IsFlagquery, IsHiddenQuery, NavQuery, IsDeletedQuery, filter.SearchText, quuery, IsCompleted, assginedToQuery, dateQuery, favoritequery);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            AddParameter(cmd, pInt32("pageno", filter.PageNo));
        //            AddParameter(cmd, pInt32("pagesize", filter.PageSize));
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }	
}