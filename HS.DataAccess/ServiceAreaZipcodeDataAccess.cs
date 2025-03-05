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
    public partial class ServiceAreaZipcodeDataAccess
    {
        public List<ServiceAreaZipcode> GetAllZipcode(int PageNumber, int UnitPerPage, string SearchText)
        {
            string searchTextQuery = "";

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                searchTextQuery = " Name like '%" + SearchText + "%'";
            }

            List<ServiceAreaZipcode> content = new List<ServiceAreaZipcode>();
            string rawQuery = @"  
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + UnitPerPage + @"
                                set @pageno = " + PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                select  TOP (@pagesize) * FROM ServiceAreaZipcode
                                where ZipCode like'%" + SearchText + @"%'AND Id NOT IN(Select TOP (@pagestart) Id from ServiceAreaZipcode)
                                select Count(Id) As TotalCount from ServiceAreaZipcode";




            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
                try
                {
                    content = (from DataRow dr in dt.Rows
                               select new ServiceAreaZipcode()
                               {

                                   Zipcode = dr["Zipcode"].ToString(),
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               }).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return content;
        }
    }
}
