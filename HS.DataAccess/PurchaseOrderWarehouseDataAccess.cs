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
	public partial class PurchaseOrderWarehouseDataAccess
	{
        public PurchaseOrderWarehouse GetPurchaseOrderByPurchaseOrderId(string purchaseOrderId)
        {
            string SqlQuery = @"select PO.* 
                                , lk.DisplayText as ShippingViaVal
                                , emp.FirstName + ' ' +emp.LastName as SoldByVal
                                from PurchaseOrderWarehouse PO
                                left join lookup lk on lk.DataValue = PO.ShippingVia 
                                and po.ShippingVia != '-1' and po.ShippingVia != '' 
                                and PO.ShippingVia is not null
                                left join Employee emp on PO.SoldBy = emp.UserId
                                where PO.PurchaseOrderId = '{0}'
                                ";
            try
            {
                SqlQuery = string.Format(SqlQuery, purchaseOrderId);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            PurchaseOrderWarehouse purchaseOrderWarehouseObject = new PurchaseOrderWarehouse();
                            FillObject(purchaseOrderWarehouseObject, reader);
                            purchaseOrderWarehouseObject.ShippingViaVal = reader["ShippingViaVal"].ToString();
                            purchaseOrderWarehouseObject.SoldByVal = reader["SoldByVal"].ToString();
                            return purchaseOrderWarehouseObject;
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
    }	
}
