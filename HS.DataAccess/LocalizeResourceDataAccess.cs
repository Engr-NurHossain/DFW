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
    public partial class LocalizeResourceDataAccess
    {
        public LocalizeResourceDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }
        public DataSet GetAllLocalizeResourceByFilter(LocalizeFilterModel filter)
        {
            string sqlQuery = @"declare @CompanyId uniqueidentifier
                                declare @LanguageId int
                                declare @SearchText nvarchar(100)
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int


                                set @CompanyId ='{0}'
                                set @LanguageId ='{1}'
                                set @SearchText = '%{2}%'
                                SET @pageno = {3}
                                SET @pagesize = {4}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize


                                select * into #InvoiceData 
                                from (select * from LocalizeResource 
                                where LanguageId = @LanguageId
                                and CompanyId = @CompanyId
                                and (ResourceName like @SearchText or ResourceValue like @SearchText) 
                                ) a 

                                SELECT TOP (@pagesize) * FROM #InvoiceData
                                    where   Id NOT IN(Select TOP (@pagestart) Id from #InvoiceData order by ResourceName asc)
	                                order by ResourceName asc

                                select COUNT(id) as TotalCount  from #InvoiceData 

                                DROP TABLE #InvoiceData";

            try
            {
                sqlQuery = string.Format(sqlQuery, filter.CompanyId, filter.LangId, filter.SearchText, filter.PageNo, filter.PageSize);
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
