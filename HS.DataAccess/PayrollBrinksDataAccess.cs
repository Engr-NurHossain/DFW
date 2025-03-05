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
    public partial class PayrollBrinksDataAccess
    {
        public PayrollBrinksDataAccess(string ConnectionStr) : base(ConnectionStr) { }
        public bool UpdatePayrollBrinksFund(DateTime FilterStartDate, DateTime FilterEndDate, string SearchText, string SalesPersonList, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            string DateFilter1 = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string StatusFilterQuery = "";
            string FundingFilterQuery = "";

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName +' '+emp.LastName like '%{0}%')", SearchText);
            }
            if (!string.IsNullOrEmpty(SalesPersonList) && SalesPersonList != "'null'")
            {
                FilterQuery = string.Format(" and pb.SalesPersonId in ({0})", SalesPersonList);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksStatus) && PayrollBrinksStatus != "'null'")
            {
                StatusFilterQuery = string.Format(" and pb.FundingStatus in ({0})", PayrollBrinksStatus);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksFunding))
            {
                if (PayrollBrinksFunding == "Funded")
                {
                    FundingFilterQuery = " and pb.IsPaid=1";
                }
                else if (PayrollBrinksFunding == "NotFunded")
                {
                    FundingFilterQuery = " and pb.IsPaid!=1";
                }
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and cus.InstallDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            string sqlQuery = @"update PayrollBrinks set IsPaid=1
                                FROM PayrollBrinks pb
                                LEFT JOIN Customer cus on cus.CustomerId=pb.CustomerId
                                LEFT JOIN Employee emp on emp.UserId=pb.SalesPersonId
                                where pb.NetPay>0 {0} {1} {2} {3} {4}";

            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter1, SearchQuery, FilterQuery, StatusFilterQuery, FundingFilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    return true;
                }

            }
            catch (Exception ee)
            {
                return false;
            }

        }
        public bool DeleteManagerPayrollBrinksByTicketId(Guid ticketId)
        {
            string sqlQuery = @"Delete from PayrollBrinks where TicketId='{0}' and IsManagerPayroll=1";

            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    return true;
                }

            }
            catch (Exception ee)
            {
                return false;
            }

        }
    }
}
