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
	public partial class InvoiceDetailDataAccess
    {
        public InvoiceDetailDataAccess() { }
        public InvoiceDetailDataAccess(string connStr) : base(connStr) { }

        public bool DeleteByInvoiceId(string invoiceId)
        {

            string SqlQuery = @"delete from InvoiceDetail where InvoiceId ='{0}' ";
            SqlQuery = string.Format(SqlQuery,  invoiceId);
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

        public DataTable GetInvoiceDetialsListByInvoiceId(string invoiceId)
        {

            string sqlQuery = @"select invd.*,eq.EquipmentClassId,eq.SupplierCost as VendorPrice, (eq.SupplierCost * invd.Quantity) as TotalRetail
                         ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = eq.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost

                                from InvoiceDetail invd 
                                left join Equipment eq on eq.EquipmentId = invd.EquipmentId 
                                where invd.InvoiceId ='{0}' 
                                order by invd.id asc";
            try
            {
                sqlQuery = string.Format(sqlQuery, invoiceId);
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

        public DataSet GetInvoiceDetailsByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select top(20) invdet.EquipName ,invdet.Quantity,invdet.InvoiceId,inv.Id,Equi.EquipmentClassId,EquiClass.Name
                                from InvoiceDetail invdet

                                left join Invoice inv
                                on inv.InvoiceId = invdet.InvoiceId 
                                left join Equipment Equi
								on Equi.EquipmentId = invdet.EquipmentId  
								left join EquipmentClass EquiClass
                                on Equi.EquipmentClassId = EquiClass.Id  
                                
                                where invdet.EquipmentId !='00000000-0000-0000-0000-000000000000'
                                and inv.CustomerId ='{0}'
                                and EquiClass.Name != 'Service'
                                group by invdet.EquipName,invdet.Quantity,invdet.InvoiceId,inv.Id,Equi.EquipmentClassId,EquiClass.Name";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
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

        public DataSet GetInvoiceDetailsServiceByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select top(20) invdet.EquipName ,invdet.Quantity,invdet.InvoiceId,inv.Id,Equi.EquipmentClassId,EquiClass.Name
                                from InvoiceDetail invdet

                                left join Invoice inv
                                on inv.InvoiceId = invdet.InvoiceId 
                                left join Equipment Equi
								on Equi.EquipmentId = invdet.EquipmentId  
								left join EquipmentClass EquiClass
                                on Equi.EquipmentClassId = EquiClass.Id  
                                
                                where invdet.EquipmentId !='00000000-0000-0000-0000-000000000000'
                                and inv.CustomerId ='{0}'
                                and EquiClass.Name = 'Service'
                                group by invdet.EquipName,invdet.Quantity,invdet.InvoiceId,inv.Id,Equi.EquipmentClassId,EquiClass.Name";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
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
        




        public List<InvoiceDetail> GetUnpaidInvoiceDetailListByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select * from InvoiceDetail where InvoiceId 
                                in( select InvoiceId from Invoice 
                                where 
                                --CreatedBy not in ('SystemGenerated','System','Automated') and 
                                DueDate < GETDATE()
                                and BalanceDue > 0
                                and Status not in('Cancelled','Rolled Over','Init')
                                and CustomerId = '{0}'
                                ) ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd,-1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<InvoiceDetail> GetUnpaidRecurringBillingInvoiceDetailsListByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select * from InvoiceDetail where InvoiceId 
                                in( select InvoiceId from Invoice 
                                where IsARBInvoice = 1
                                and DueDate < GETDATE()
                                and BalanceDue > 0
                                and IsEstimate = 0
                                and [Status] not in('Cancelled','Rolled Over','Init','Paid')
                                and CustomerId = '{0}'
                                )";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
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

        public List<InvoiceDetail> GetUnpaidOthersBillingInvoiceDetailsListByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select * from InvoiceDetail where InvoiceId 
                                in( select InvoiceId from Invoice 
                                where BalanceDue > 0
                                and DueDate < GETDATE()
                                and (IsARBInvoice != 1 or IsARBInvoice is null)
                                and IsEstimate = 0
                                and [Status] not in('Cancelled','Rolled Over','Init','Paid')
                                and CustomerId = '{0}'
                                )";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
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
    }	
}
