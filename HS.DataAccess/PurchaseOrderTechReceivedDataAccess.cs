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
	public partial class PurchaseOrderTechReceivedDataAccess
	{
        public DataSet GetPurchaseOrderTechReceivedListByTech(PurchaseOrderFilter filters)
        {
            string SearchTextFilter = "";
            if (!string.IsNullOrWhiteSpace(filters.SearchtextRcv) && filters.SearchtextRcv != "undefined")
            {
                SearchTextFilter = @"and(isnull(ptech.[DemandOrderId], '') like @SearchText 
                                    or ptech.[Status] like @SearchText
                                    or prcv.[EquipName] like @SearchText
                                    or prcv.[Quantity] like @SearchText)";
            }
            string subquery = "order by prcv.IsReceived";
            if (!string.IsNullOrWhiteSpace(filters.orderrcv))
            {
                if (filters.orderrcv == "ascending/dono")
                {
                    subquery = "order by prcv.BranchDemandOrderId asc";
                }
                else if (filters.orderrcv == "descending/dono")
                {
                    subquery = "order by prcv.BranchDemandOrderId desc";
                }
                else if (filters.orderrcv == "ascending/equipname")
                {
                    subquery = "order by prcv.EquipName asc";
                }
                else if (filters.orderrcv == "descending/equipname")
                {
                    subquery = "order by prcv.EquipName desc";
                }
                else if (filters.orderrcv == "ascending/status")
                {
                    subquery = "order by prcv.IsReceived asc";
                }
                else if (filters.orderrcv == "descending/status")
                {
                    subquery = "order by prcv.IsReceived desc";
                }
                else if (filters.orderrcv == "ascending/quantity")
                {
                    subquery = "order by prcv.Quantity asc";
                }
                else if (filters.orderrcv == "descending/quantity")
                {
                    subquery = "order by prcv.Quantity desc";
                }
                else if (filters.orderrcv == "ascending/rcvdate")
                {
                    subquery = "order by prcv.ReceivedDate asc";
                }
                else if (filters.orderrcv == "descending/rcvdate")
                {
                    subquery = "order by prcv.ReceivedDate desc";
                }
            }
            string sqlQuery = @"select
                                prcv.*,
                                ptech.TechnicianId,
                                ptech.Id as PurchaseOrderTechId
                                from PurchaseOrderTechReceived prcv
                                LEFT JOIN PurchaseOrderTech ptech on ptech.DemandOrderId=prcv.BranchDemandOrderId
                                where ptech.TechnicianId='{0}' {1} {2}";

            try
            {
                sqlQuery = string.Format(sqlQuery, filters.EmployeeId,SearchTextFilter, subquery);
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
