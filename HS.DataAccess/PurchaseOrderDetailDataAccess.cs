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
    public partial class PurchaseOrderDetailDataAccess
    {
        public bool DeleteAllPurchaseOrderDetailByPurchaseOrderId(string purchaseOrderId)
        {
            string SqlQuery = @"DELETE FROM [PurchaseOrderDetail] Where [PurchaseOrderId] ='{0}'";
            try
            {
                SqlQuery = string.Format(SqlQuery, purchaseOrderId);
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
        public List<PurchaseOrderDetail> GetPurchaseOrderDetailListByPurchaseOrderId(string purchaseOrderId)
        {
            string SqlQuery = @"select POD.* 
                                ,IsNULL((select  ISNULL(sum(Quantity),0) from InventoryWarehouse where [Type] ='Add' and EquipmentId = POD.EquipmentId) -(select ISNULL(sum(Quantity),0) from InventoryWarehouse where [Type] ='Release' and EquipmentId = POD.EquipmentId),0 ) as QuantityAvailable
                                ,eqp.SKU,eqp.Barcode, eqp.Comments as eqDescription                                
                                from PurchaseOrderDetail POD
                                LEFT JOIN Equipment eqp on eqp.EquipmentId=POD.EquipmentId
                                where PurchaseOrderId ='{0}'";
            try
            {
                SqlQuery = string.Format(SqlQuery, purchaseOrderId);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader); 
                    PurchaseOrderDetailList list = new PurchaseOrderDetailList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PurchaseOrderDetail purchaseOrderDetailObject = new PurchaseOrderDetail();
                            FillObject(purchaseOrderDetailObject, reader);
                            purchaseOrderDetailObject.QuantityAvailable = int.Parse(reader["QuantityAvailable"].ToString());
                            purchaseOrderDetailObject.SKU = reader["SKU"].ToString();
                            purchaseOrderDetailObject.Barcode = reader["Barcode"].ToString();
                            purchaseOrderDetailObject.eqDescription = reader["eqDescription"].ToString();
                            list.Add(purchaseOrderDetailObject);
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
        public DataTable GetReceivePOHistoryByPurchaseOrderId(int Id)
        {
            string sqlQuery = @"select 
                                pod.EquipName,
                                pod.RecieveQty,
                                emprcvby.FirstName +' '+emprcvby.LastName as ReceiveBy,
                                emprcvfor.FirstName +' '+emprcvfor.LastName as ReceiveFor,
                                pow.RecieveDate
                                from PurchaseOrderDetail pod
                                left join PurchaseOrderWarehouse pow on pow.PurchaseOrderId=pod.PurchaseOrderId
                                left join Employee emprcvby on emprcvby.UserId=pow.RecieveByUid
                                left join Employee emprcvfor on emprcvfor.UserId=pow.RecieveForUid
                                where pow.Id='{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, Id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
