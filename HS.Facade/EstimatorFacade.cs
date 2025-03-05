using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using HS.Framework.Utils;
using System.Collections;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class EstimatorFacade : BaseFacade
    {
        public EstimatorFacade(ClientContext clientContext)
         : base(clientContext)
        {

        }
        public EstimatorFacade()
        {
        }
        EstimatorDataAccess _EstimatorDataAccess
        {
            get
            {
                return (EstimatorDataAccess)_ClientContext[typeof(EstimatorDataAccess)];
            }
        }
        EstimatorFileDataAccess _EstimatorFileDataAccess
        {
            get
            {
                return (EstimatorFileDataAccess)_ClientContext[typeof(EstimatorFileDataAccess)];
            }
        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }
        EquipmentDataAccess _EquipmentDataAccess
        {
            get
            {
                return (EquipmentDataAccess)_ClientContext[typeof(EquipmentDataAccess)];
            }
        }
        EstimatorServiceDataAccess _EstimatorServiceDataAccess
        {
            get
            {
                return (EstimatorServiceDataAccess)_ClientContext[typeof(EstimatorServiceDataAccess)];
            }
        }


        EstimatorDetailDataAccess _EstimatorDetailDataAccess
        {
            get
            {
                return (EstimatorDetailDataAccess)_ClientContext[typeof(EstimatorDetailDataAccess)];
            }
        }
        EstimatorPDFFilterDataAccess _EstimatorPDFFilterDataAccess
        {
            get
            {
                return (EstimatorPDFFilterDataAccess)_ClientContext[typeof(EstimatorPDFFilterDataAccess)];
            }
        }
        EstimatorNoteDataAccess _EstimatorNoteDataAccess
        {
            get
            {
                return (EstimatorNoteDataAccess)_ClientContext[typeof(EstimatorNoteDataAccess)];
            }
        }
        EmailTemplateDataAccess _EmailTemplateDataAccess
        {
            get
            {
                return (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            }
        }
        public Estimator GetById(int value)
        {
            return _EstimatorDataAccess.Get(value);
        }

        public EstimatorDetail GetByIId(int value)
        {
            return _EstimatorDetailDataAccess.Get(value);
        }
        public Estimator GetByIIId(int value)
        {
            return _EstimatorDataAccess.Get(value);
        }

        public bool UpdateEstimatorDetail(EstimatorDetail invoice)
        {
            return _EstimatorDetailDataAccess.Update(invoice) > 0;
        }

        public Estimator GetByEstimatorId(string estimatorId)
        {
            return _EstimatorDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}'", estimatorId)).FirstOrDefault();
        }
        public EstimatorFile GetEstimatorFileByEstimatorId(string estimatorId)
        {
            return _EstimatorFileDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}'", estimatorId)).FirstOrDefault();
        }
        public EstimatorFile GetEstimatorFileById(int Id)
        {
            return _EstimatorFileDataAccess.GetByQuery(string.Format(" Id = {0}", Id)).FirstOrDefault();
        }
        public List<EstimatorFile> GetByEstimatorFileByEstimatorId(string estimatorId)
        {
            return _EstimatorFileDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}'", estimatorId)).ToList();
        }
        public List<EstimatorFile> GetByEstimatorFileByEstimatorIdAndEstimatorType(string estimatorId,string EstimatorType)
        {
            return _EstimatorFileDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}' and EstimatorType = '{1}'", estimatorId, EstimatorType)).ToList();
        }
        public EstimatorPDFFilter GetEstimatorPdfFilterById(int value)
        {
            return _EstimatorPDFFilterDataAccess.Get(value);
        }
        public EstimatorPDFFilter GetEstimatorPdfFilterByComIdCusIdUserId(Guid comid, Guid userid, Guid cusid)
        {
            return _EstimatorPDFFilterDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and CreatedBy = '{1}' and CustomerId = '{2}'", comid, userid, cusid)).FirstOrDefault();
        }
        public EstimatorPDFFilter NewGetEstimatorPdfFilterByComIdCusIdUserId(Guid comid, Guid cusid)
        {
            return _EstimatorPDFFilterDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and CustomerId = '{1}'", comid, cusid)).FirstOrDefault();
        }
        public int InsertEstimatorPDFFilter(EstimatorPDFFilter estfil)
        {
            return (int)_EstimatorPDFFilterDataAccess.Insert(estfil);
        }
        public bool UpdateEstimatorPDFFilter(EstimatorPDFFilter est)
        {
            return _EstimatorPDFFilterDataAccess.Update(est) > 0;
        }
        public Estimator GetByEstimatorID(int id)
        {
            return _EstimatorDataAccess.Get(id);
        }
        public List<EquipmentSearchModelEstimator> GetEqupmentListBySearchKeySupplierIdAndCategory(string key, int MaxLoad,Guid? SupplierId, int? CategoryId,double DefaultProfitRate, double DefaultOverHeadRate)
        {
            DataTable dt = _EquipmentDataAccess.GetEqupmentListBySearchKeySupplierIdAndCategory(key, MaxLoad, SupplierId, CategoryId);
            List<EquipmentSearchModelEstimator> NoteList = new List<EquipmentSearchModelEstimator>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModelEstimator()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(), 
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0, 
                            EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : -1,
                            SupplierId = (Guid)dr["SupplierId"],
                            ManufacturerId = (Guid)dr["ManufacturerId"],
                            Unit = dr["Unit"].ToString(),
                            ProfitRate = dr["ProfitRate"] != DBNull.Value ? Convert.ToDouble(dr["ProfitRate"]) : -1 ,
                            OverheadRate = dr["OverheadRate"] != DBNull.Value ? Convert.ToDouble(dr["OverheadRate"]) : DefaultOverHeadRate,
                            Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                            
                        }).ToList();
            return NoteList;
        }

        public EstimatorDashboard GetAllEstimatorCountByCustomerIdAndCompanyId(Guid EquipmentId, Guid CompanyId, EstimateFilter filter)
        {
            DataSet dsresult = _EstimatorDataAccess.GetAllEstimatorDetailCountByCustomerIdAndCompanyId(EquipmentId, CompanyId, filter);
            DataTable dt = dsresult.Tables[0];

            DataTable dt1 = dsresult.Tables[1];
            List<Estimator> InvoiceDetailList = new List<Estimator>();
            EstimatorDashboard estimator = new EstimatorDashboard();

            if (dt.Rows.Count > 0)
            {
                estimator.EstimatorList = (from DataRow dr in dt.Rows //DataRow dr in dt.Rows
                                           select new Estimator()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               TotalCost = dr["TotalCost"] != DBNull.Value ? Convert.ToInt32(dr["TotalCost"]) : 0,
                                               TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                               CreatedByName = dr["CreatedByName"].ToString(),
                                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                               CreatedBy = (Guid)dr["CreatedBy"],
                                               BillingAddress = dr["BillingAddress"].ToString(),
                                               CompanyId = (Guid)dr["CompanyId"],
                                               CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                               EstimatorId = dr["EstimatorId"].ToString(),
                                               Description = dr["Description"].ToString(),
                                               EmailAddress = dr["EmailAddress"].ToString(),
                                               Status = dr["Status"].ToString(),
                                               ProjectAddress = dr["ProjectAddress"].ToString(),
                                               CustomerId = (Guid)dr["CustomerId"],
                                               LastUpdatedBy = (Guid)dr["CustomerId"],
                                               StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                               TaxAmount = dr["TaxAmount"] != DBNull.Value ? Convert.ToDouble(dr["TaxAmount"]) : 0,
                                               TaxPercnetage = dr["TaxPercnetage"] != DBNull.Value ? Convert.ToDouble(dr["TaxPercnetage"]) : 0,
                                               PoriftPercentage = dr["PoriftPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PoriftPercentage"]) : 0,
                                               TotalOverheadCostAmount = dr["TotalOverheadCostAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalOverheadCostAmount"]) : 0,
                                               OverheadCostPercentage = dr["OverheadCostPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverheadCostPercentage"]) : 0,
                                               TotalProfitAmount = dr["TotalProfitAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalProfitAmount"]) : 0,
                                           }).ToList();

                estimator.OpenCount = dsresult.Tables[1].Rows[0]["OpenCount"] != DBNull.Value ? Convert.ToInt32(dsresult.Tables[1].Rows[0]["OpenCount"]) : 0;
                estimator.AcceptedCount = dsresult.Tables[2].Rows[0]["AcceptedCount"] != DBNull.Value ? Convert.ToInt32(dsresult.Tables[2].Rows[0]["AcceptedCount"]) : 0;
                estimator.PendingCount = dsresult.Tables[3].Rows[0]["DeclinedCount"] != DBNull.Value ? Convert.ToInt32(dsresult.Tables[3].Rows[0]["DeclinedCount"]) : 0;
            }
            else
            {
                estimator.EstimatorList = new List<Estimator>();
                estimator.TotalCount = 0;
            }

            return estimator;
        }

        public EstimatorDashboard GetAllEstimatorListByCustomerIdAndCompanyId(Guid EquipmentId, Guid CompanyId, EstimateFilter filter)
        {
            DataSet dsresult = _EstimatorDataAccess.GetAllEstimatorDetailListByCustomerIdAndCompanyId(EquipmentId, CompanyId, filter);
            DataTable dt = dsresult.Tables[0];

            DataTable dt1 = dsresult.Tables[1];
            List<Estimator> InvoiceDetailList = new List<Estimator>();
            EstimatorDashboard estimator = new EstimatorDashboard();

            if (dt.Rows.Count > 0)
            {
                estimator.EstimatorList = (from DataRow dr in dt.Rows //DataRow dr in dt.Rows
                                           select new Estimator()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               TotalCost = dr["TotalCost"] != DBNull.Value ? Convert.ToInt32(dr["TotalCost"]) : 0,
                                               TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                               CreatedByName = dr["CreatedByName"].ToString(),
                                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                               CreatedBy = (Guid)dr["CreatedBy"],
                                               BillingAddress = dr["BillingAddress"].ToString(),
                                               ParentEstimatorRef = dr["ParentEstimatorRef"].ToString(),
                                               IsApproved = dr["IsApproved"] != DBNull.Value ? Convert.ToBoolean(dr["IsApproved"]) : false,
                                               CompanyId = (Guid)dr["CompanyId"],
                                               CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                               EstimatorId = dr["EstimatorId"].ToString(),
                                               Description = dr["Description"].ToString(),
                                               EmailAddress = dr["EmailAddress"].ToString(),
                                               Status = dr["Status"].ToString(),
                                               ProjectAddress = dr["ProjectAddress"].ToString(),
                                               CustomerId = (Guid)dr["CustomerId"],
                                               LastUpdatedBy = (Guid)dr["CustomerId"],
                                               StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                               TaxAmount = dr["TaxAmount"] != DBNull.Value ? Convert.ToDouble(dr["TaxAmount"]) : 0,
                                               TaxPercnetage = dr["TaxPercnetage"] != DBNull.Value ? Convert.ToDouble(dr["TaxPercnetage"]) : 0,
                                               PoriftPercentage = dr["PoriftPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PoriftPercentage"]) : 0,
                                               TotalOverheadCostAmount = dr["TotalOverheadCostAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalOverheadCostAmount"]) : 0,
                                               OverheadCostPercentage = dr["OverheadCostPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverheadCostPercentage"]) : 0,
                                               TotalProfitAmount = dr["TotalProfitAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalProfitAmount"]) : 0,
                                           }).ToList();

                estimator.TotalCount = dsresult.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dsresult.Tables[1].Rows[0]["TotalCount"]) : 0;
            }
            else
            {
                estimator.EstimatorList = new List<Estimator>();
                estimator.TotalCount = 0;
            }

            return estimator;
        }
        public EstimatorDashboard GetAllEstimatorListForDashboard(Guid CompanyId, EstimatorFilter filter, int pageno, int pagesize, string status, string overxprice, string startdate, string enddate)
        {
            DataSet dsresult = _EstimatorDataAccess.GetAllEstimatorListForDashboard(CompanyId, filter, pageno, pagesize, status, overxprice, startdate, enddate);
            DataTable dt = dsresult.Tables[0];

            DataTable dt1 = dsresult.Tables[1];
            DataTable dt2 = dsresult.Tables[2];
            Estimator totalcount = new Estimator();
            Estimator countestimator = new Estimator();
            List<Estimator> InvoiceDetailList = new List<Estimator>();
            InvoiceDetailList = (from DataRow dr in dt.Rows
                                 select new Estimator()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     TotalCost = dr["TotalCost"] != DBNull.Value ? Convert.ToInt32(dr["TotalCost"]) : 0,
                                     TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                     CreatedByName = dr["CreatedByName"].ToString(),
                                     LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                     CreatedBy = (Guid)dr["CreatedBy"],
                                     BillingAddress = dr["BillingAddress"].ToString(),
                                     CompanyId = (Guid)dr["CompanyId"],
                                     CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                     EstimateDate = dr["EstimateDate"] != DBNull.Value ? Convert.ToDateTime(dr["EstimateDate"]) : new DateTime(),

                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     EstimatorId = dr["EstimatorId"].ToString(),
                                     Description = dr["Description"].ToString(),
                                     EmailAddress = dr["EmailAddress"].ToString(),
                                     Status = dr["Status"].ToString(),
                                     CustomerName = dr["CustomerName"].ToString(),
                                     ProjectAddress = dr["ProjectAddress"].ToString(),
                                     CustomerId = (Guid)dr["CustomerId"],
                                     LastUpdatedBy = (Guid)dr["CustomerId"],
                                     StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                     TaxAmount = dr["TaxAmount"] != DBNull.Value ? Convert.ToDouble(dr["TaxAmount"]) : 0,
                                     TaxPercnetage = dr["TaxPercnetage"] != DBNull.Value ? Convert.ToDouble(dr["TaxPercnetage"]) : 0,
                                     PoriftPercentage = dr["PoriftPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PoriftPercentage"]) : 0,
                                     TotalOverheadCostAmount = dr["TotalOverheadCostAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalOverheadCostAmount"]) : 0,
                                     OverheadCostPercentage = dr["OverheadCostPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverheadCostPercentage"]) : 0,
                                     TotalProfitAmount = dr["TotalProfitAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalProfitAmount"]) : 0,
                                 }).ToList();

            //totalcount = (from DataRow dr in dt1.Rows
            //                 select new Estimator()
            //                 {
            //                     TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0

            //                 }).FirstOrDefault();
            //countestimator = (from DataRow dr in dt2.Rows
            //       select new Estimator()
            //       {
            //           CountEstimator = dr["CountEstimator"] != DBNull.Value ? Convert.ToInt32(dr["CountEstimator"]) : 0
            //       }).FirstOrDefault();

            EstimatorDashboard estimator = new EstimatorDashboard();
            estimator.TotalCount = dsresult.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dsresult.Tables[1].Rows[0]["TotalCount"]) : 0;
            estimator.CountEstimator = dsresult.Tables[2].Rows[0]["CountEstimator"] != DBNull.Value ? Convert.ToInt32(dsresult.Tables[2].Rows[0]["CountEstimator"]) : 0;

            estimator.EstimatorList = InvoiceDetailList;
            return estimator;
        }
        public List<EstimatorService> GetEstimatorServicesByEstimatorId(string estimatorId)
        {
            return _EstimatorServiceDataAccess.GetByQuery(string.Format(" EstimatorId='{0}'",estimatorId));
        }
        public List<EstimatorService> GetEstimatorOneTimeServicesByEstimatorId(string estimatorId)
        {
            return _EstimatorServiceDataAccess.GetByQuery(string.Format(" EstimatorId='{0}' and IsOneTimeService = 1", estimatorId));
        }
        public List<EstimatorDetail> GetEstimatorDetailListByEstimatorIdForChild(string EstimatorId)
        {
            DataSet dsresult = _EstimatorDataAccess.GetAllChildEstimatorDetailByEstimatorId(EstimatorId);
            DataTable dt = dsresult.Tables[0];
            List<EstimatorDetail> NoteList = new List<EstimatorDetail>();
            if (dt != null)
            {
                NoteList = (from DataRow dr in dt.Rows
                            select new EstimatorDetail()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                PartDescription = dr["PartDescription"].ToString(),
                                PartNumber = dr["PartNumber"].ToString(),
                                CategoryId = dr["CategoryId"] != DBNull.Value ? Convert.ToInt32(dr["CategoryId"]) : 0,
                                Unit = dr["Unit"].ToString(),
                                Qunatity = dr["Qunatity"] != DBNull.Value ? Convert.ToInt32(dr["Qunatity"]) : 0,
                                Overhead = dr["Overhead"] != DBNull.Value ? Convert.ToDouble(dr["Overhead"]) : 0,
                                EstimatorId = dr["EstimatorId"].ToString(),
                                UnitCost = dr["UnitCost"] != DBNull.Value ? Convert.ToDouble(dr["UnitCost"]) : 0,
                                TotalCost = dr["TotalCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalCost"]) : 0,
                                Profit = dr["Profit"] != DBNull.Value ? Convert.ToDouble(dr["Profit"]) : 0,
                                TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                EquipmentId = (Guid)dr["EquipmentId"],
                                SupplierId = (Guid)dr["SupplierId"],
                                CreatedBy = (Guid)dr["CreatedBy"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : false,
                                OverheadRate = dr["OverheadRate"] != DBNull.Value ? Convert.ToDouble(dr["OverheadRate"]) : 0,
                                ProfitRate = dr["ProfitRate"] != DBNull.Value ? Convert.ToDouble(dr["ProfitRate"]) : 0,
                                CategoryVal = dr["CategoryVal"].ToString(),
                                SupplierVal = dr["SupplierVal"].ToString(),

                            }).ToList();
            }
            return NoteList;
        }
        public DataTable GetEstimatorList(Guid CustomerId, Guid CompanyId, int[] IdList, EstimatorFilter filter)
        {
            return _EstimatorDataAccess.GetEstimatorList(CustomerId, CompanyId, IdList, filter);
        }
        //public DataTable ExportEstimatorByEstimatorId(string EstimatorId)
        //{
        //    return _EstimatorDataAccess.ExportEstimatorByEstimatorId(EstimatorId);
        //}
        public DataSet GetExportEstimatorByEstimatorId(string EstimatorId)
        {
            //DataTable dt = new DataTable();
            return _EstimatorDataAccess.ExportEstimatorByEstimatorId(EstimatorId);
            
            //Estimator model = new Estimator();
            //if (dt != null)
            //{
            //    model.EstimatorDetailList = (from DataRow dr in dt.Rows
            //                select new EstimatorDetail()
            //                {

            //                    PartDescription = dr["PartDescription"].ToString(), 
            //                    Qunatity = dr["Qunatity"] != DBNull.Value ? Convert.ToDouble(dr["Qunatity"]) : 0,
            //                    UnitPrice = dr["Unit Price"] != DBNull.Value ? Convert.ToDouble(dr["Unit Price"]) : 0,
            //                    TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
            //                    Qunatity2 = dr["Qunatity 2"] != DBNull.Value ? Convert.ToInt32(dr["Qunatity 2"]) : 0,
            //                    TotalCost = dr["TotalCost"] != DBNull.Value ? Convert.ToInt32(dr["TotalCost"]) : 0,
            //                    Profit = dr["Profit"] != DBNull.Value ? Convert.ToInt32(dr["Profit"]) : 0,
 
            //                }).ToList();
            //      model.CustomerBusinessName = dsresult.Tables[2].Rows[0]["Business Name"] != DBNull.Value ? Convert.ToString(dsresult.Tables[2].Rows[0]["Business Name"]) : " ";
            //      model.AccountNo = dsresult.Tables[2].Rows[0]["Account Number"] != DBNull.Value ? Convert.ToString(dsresult.Tables[2].Rows[0]["Account Number"]) : " ";
            //      model.EstimatorId = dsresult.Tables[2].Rows[0]["Estimator Number"] != DBNull.Value ? Convert.ToString(dsresult.Tables[2].Rows[0]["Estimator Number"]) : " ";
            //}
            //return dt;
        }

        public List<EstimatorDetail> GetEstimatorDetailListByEstimatorId(string estimatorId)
        {
            return _EstimatorDetailDataAccess.GetByQuery(String.Format(" EstimatorId = '{0}'", estimatorId));
        }
        public List<EstimatorDetail> NewGetEstimatorDetailListByEstimatorId(string EstimatorId)
        {
            DataTable dt = _EstimatorNoteDataAccess.NewGetAllEstimatorNoteByEstimatorIdAndCompanyId(EstimatorId);
            List<EstimatorDetail> NoteList = new List<EstimatorDetail>();
            if (dt != null)
            {
                NoteList = (from DataRow dr in dt.Rows
                            select new EstimatorDetail()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                PartDescription = dr["PartDescription"].ToString(),
                                SKU = dr["SKU"].ToString(),
                                PartNumber = dr["PartNumber"].ToString(),
                                CategoryId = dr["CategoryId"] != DBNull.Value ? Convert.ToInt32(dr["CategoryId"]) : 0,
                                Unit = dr["Unit"].ToString(),
                                Qunatity = dr["Qunatity"] != DBNull.Value ? Convert.ToInt32(dr["Qunatity"]) : 0,
                                Overhead = dr["Overhead"] != DBNull.Value ? Convert.ToDouble(dr["Overhead"]) : 0,
                                EstimatorId = dr["EstimatorId"].ToString(),
                                UnitCost = dr["UnitCost"] != DBNull.Value ? Convert.ToDouble(dr["UnitCost"]) : 0,
                                TotalCost = dr["TotalCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalCost"]) : 0,
                                Profit = dr["Profit"] != DBNull.Value ? Convert.ToDouble(dr["Profit"]) : 0,
                                TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                EquipmentId = (Guid)dr["EquipmentId"],
                                SupplierId = (Guid)dr["SupplierId"],
                                CreatedBy = (Guid)dr["CreatedBy"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : false,
                                OverheadRate = dr["OverheadRate"] != DBNull.Value ? Convert.ToDouble(dr["OverheadRate"]) : 0,
                                ProfitRate = dr["ProfitRate"] != DBNull.Value ? Convert.ToDouble(dr["ProfitRate"]) : 0,
                                //CategoryVal = dr["CategoryVal"].ToString(),
                                //SupplierVal = dr["SupplierVal"].ToString(),

                            }).ToList();
            }
            return NoteList;
        }
        public int InsertEstimator(Estimator estimator)
        {
            return (int)_EstimatorDataAccess.Insert(estimator);
        }

        public bool UpdateEstimator(Estimator estimator)
        {
            return _EstimatorDataAccess.Update(estimator)>0;
        }

        public List<EstimatorNote> GetAllEstimatorNoteByEstimatorIdAndCompanyId(int EstimatorId, Guid CompanyId)
        {
            DataTable dt = _EstimatorNoteDataAccess.GetAllEstimatorNoteByEstimatorIdAndCompanyId(EstimatorId, CompanyId);
            List<EstimatorNote> NoteList = new List<EstimatorNote>();
            if (dt != null)
            {
                NoteList = (from DataRow dr in dt.Rows
                            select new EstimatorNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                AddedBy = (Guid)dr["AddedBy"],
                                AddedByText = dr["AddedByText"].ToString(),
                                AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                CompanyId = (Guid)dr["CompanyId"],
                                EstimatorId = dr["EstimatorId"] != DBNull.Value ? Convert.ToInt32(dr["EstimatorId"]) : 0,
                                Note = dr["Note"].ToString(),

                            }).ToList();
            }
            return NoteList;
        }
        public List<SupplierIdList> GetAllSuplierIdByEstimatorId(string EstimatorId)
        {
            DataTable dt = _EstimatorNoteDataAccess.GetAllSuplierIdByEstimatorId(EstimatorId);
            List<SupplierIdList> IdList = new List<SupplierIdList>();
            if (dt != null)
            {
                IdList = (from DataRow dr in dt.Rows
                            select new SupplierIdList()
                            {
                                SupplierId = (Guid)dr["SupplierId"]

                            }).ToList();
            }
            return IdList;
        }

        public Estimator GetEstimatorByEstimatorId(string estimatorId)
        {
            return _EstimatorDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}' ",estimatorId)).FirstOrDefault() ;
        }
        public Estimator GetEstimatorByCustomerId(Guid CustomerId)
        {
            return _EstimatorDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' ", CustomerId)).FirstOrDefault();
        }
        public bool DeleteEstimatorDetailsByEstimatorId(string estimatorId)
        {
            return _EstimatorDetailDataAccess.DeleteEstimatorDetailsByEstimatorId(estimatorId);
        }

        public int InsertEstimatorDetails(EstimatorDetail item)
        {
            return (int)_EstimatorDetailDataAccess.Insert(item);
        }

        public bool DeleteEstimatorServiceByEstimatorId(string estimatorId)
        {
            return _EstimatorServiceDataAccess.DeleteEstimatorServiceByEstimatorId(estimatorId);
        }
        public bool DeleteEstimatorOneTimeServiceByEstimatorId(string estimatorId)
        {
            return _EstimatorServiceDataAccess.DeleteEstimatorOneTimeServiceByEstimatorId(estimatorId);
        }
        public bool DeleteEstimatorByEstimatorId(string estimatorId)
        {
            return _EstimatorDataAccess.DeleteEstimatorByEstimatorId(estimatorId);
        }

        
        public int InsertEstimatorService(EstimatorService item)
        {
            return (int)_EstimatorServiceDataAccess.Insert(item);
        }
        public Estimator GetEstimatorById(int value)
        {
            return _EstimatorDataAccess.Get(value);
        }
    }
}
