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
    public partial class EmployeeReviewDataAccess
    {
        public List<EmployeeReview> GetEmployeeReviewsByUserId(Guid userId)
        {
            string sqlQuery = @"select er.*,emp.FirstName+' '+emp.LastName as ReviewedByName 
                                from EmployeeReview er
                                left join Employee emp on er.ReviewedBy = emp.UserId
                                where er.UserId = '{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader); 
                    EmployeeReviewList list = new EmployeeReviewList();

                    using (reader)
                    { 
                        while (reader.Read() )
                        {
                            EmployeeReview employeeReviewObject = new EmployeeReview();
                            FillObject(employeeReviewObject, reader);
                            employeeReviewObject.ReviewedByName = reader["ReviewedByName"].ToString();

                            list.Add(employeeReviewObject);
                        } 
                        reader.Close();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
