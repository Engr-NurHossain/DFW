using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class PayrollFacade : BaseFacade
    {
        public PayrollFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }
        SalesCommissionDataAccess _SalesCommissionDataAccess
        {
            get
            {
                return (SalesCommissionDataAccess)_ClientContext[typeof(SalesCommissionDataAccess)];
            }
        }
        PayrollTermSheetDataAccess _PayrollTermSheetDataAccess
        {
            get
            {
                return (PayrollTermSheetDataAccess)_ClientContext[typeof(PayrollTermSheetDataAccess)];
            }
        }
        PayrollSingleItemSettingsDataAccess _PayrollSingleItemSettingsDataAccess
        {
            get
            {
                return (PayrollSingleItemSettingsDataAccess)_ClientContext[typeof(PayrollSingleItemSettingsDataAccess)];
            }
        }
        PayrollBaseMultipleDataAccess _PayrollBaseMultipleDataAccess
        {
            get
            {
                return (PayrollBaseMultipleDataAccess)_ClientContext[typeof(PayrollBaseMultipleDataAccess)];
            }
        }
        PayrollCustomerBillingMethodDataAccess _PayrollCustomerBillingMethodDataAccess
        {
            get
            {
                return (PayrollCustomerBillingMethodDataAccess)_ClientContext[typeof(PayrollCustomerBillingMethodDataAccess)];
            }
        }
        PayrollMonthlyProductionBonusDataAccess _PayrollMonthlyProductionBonusDataAccess
        {
            get
            {
                return (PayrollMonthlyProductionBonusDataAccess)_ClientContext[typeof(PayrollMonthlyProductionBonusDataAccess)];
            }
        }
        PayrollCreditRatingDataAccess _PayrollCreditRatingDataAccess
        {
            get
            {
                return (PayrollCreditRatingDataAccess)_ClientContext[typeof(PayrollCreditRatingDataAccess)];
            }
        }
        PayrollCustomerTypeDataAccess _PayrollCustomerTypeDataAccess
        {
            get
            {
                return (PayrollCustomerTypeDataAccess)_ClientContext[typeof(PayrollCustomerTypeDataAccess)];
            }
        }
        PayrollAgreementLengthDataAccess _PayrollAgreementLengthDataAccess
        {
            get
            {
                return (PayrollAgreementLengthDataAccess)_ClientContext[typeof(PayrollAgreementLengthDataAccess)];
            }
        }
        PayrollPassThrusDataAccess _PayrollPassThrusDataAccess
        {
            get
            {
                return (PayrollPassThrusDataAccess)_ClientContext[typeof(PayrollPassThrusDataAccess)];
            }
        }
        PayrollInstallationFeeDataAccess _PayrollInstallationFeeDataAccess
        {
            get
            {
                return (PayrollInstallationFeeDataAccess)_ClientContext[typeof(PayrollInstallationFeeDataAccess)];
            }
        }
        PayrollHoldBackDataAccess _PayrollHoldBackDataAccess
        {
            get
            {
                return (PayrollHoldBackDataAccess)_ClientContext[typeof(PayrollHoldBackDataAccess)];
            }
        }
        PayrollAdminFeeDataAccess _PayrollAdminFeeDataAccess
        {
            get
            {
                return (PayrollAdminFeeDataAccess)_ClientContext[typeof(PayrollAdminFeeDataAccess)];
            }
        }
        AdjustmentFundingDataAccess _AdjustmentFundingDataAccess
        {
            get
            {
                return (AdjustmentFundingDataAccess)_ClientContext[typeof(AdjustmentFundingDataAccess)];
            }
        }
        PayrollTermSheetManagerDataAccess _PayrollTermSheetManagerDataAccess
        {
            get
            {
                return (PayrollTermSheetManagerDataAccess)_ClientContext[typeof(PayrollTermSheetManagerDataAccess)];
            }
        }
        PayrollBrinksDataAccess _PayrollBrinksDataAccess
        {
            get
            {
                return (PayrollBrinksDataAccess)_ClientContext[typeof(PayrollBrinksDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        public long InsertPayrollTermSheet(PayrollTermSheet payrollTermSheet)
        {
            return _PayrollTermSheetDataAccess.Insert(payrollTermSheet);
        }
        public bool UpdatePayrollTermSheet(PayrollTermSheet payrollTermSheet)
        {
            return _PayrollTermSheetDataAccess.Update(payrollTermSheet) > 0;
        }
        public List<PayrollTermSheet> GetPayrollTermSheetList()
        {
            return _PayrollTermSheetDataAccess.GetAll();
        }
        public PayrollTermSheet GetBasePayrollTermSheetBy()
        {
            string query = string.Format("IsBase=1");
            return _PayrollTermSheetDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public PayrollTermSheet GetPayrollTermSheetById(int id)
        {
            return _PayrollTermSheetDataAccess.Get(id);
        }
        public PayrollTermSheet GetPayrollTermSheetByTermSheetId(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollTermSheetDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool DeletePayrollTermSheet(int Id)
        {
            return _PayrollTermSheetDataAccess.Delete(Id) > 0;
        }
        public bool SetAllTermSheetIsBaseFalse()
        {
            return _PayrollTermSheetDataAccess.SetAllTermSheetIsBaseFalse();
        }
        public bool CloneBasePayrollTermSheet(Guid oldTermSheetId, Guid newTermSheetId, Guid createdbyuid, DateTime dateTime)
        {
            return _PayrollTermSheetDataAccess.CloneBasePayrollTermSheet(oldTermSheetId, newTermSheetId, createdbyuid, dateTime);
        }

        public long InsertPayrollBaseMultiple(PayrollBaseMultiple payrollBaseMultiple)
        {
            return _PayrollBaseMultipleDataAccess.Insert(payrollBaseMultiple);
        }
        public bool UpdatePayrollBaseMultiple(PayrollBaseMultiple payrollBaseMultiple)
        {
            return _PayrollBaseMultipleDataAccess.Update(payrollBaseMultiple) > 0;
        }
        public List<PayrollBaseMultiple> GetPayrollBaseMultipleList(Guid termSheetId)
        {
            DataTable dt = _PayrollBaseMultipleDataAccess.GetPayrollBaseMultipleList(termSheetId);
            List<PayrollBaseMultiple> payrollBaseMultiple = new List<PayrollBaseMultiple>();
            payrollBaseMultiple = (from DataRow dr in dt.Rows
                                   select new PayrollBaseMultiple()
                                   {
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       BaseMultiple = dr["BaseMultiple"].ToString(),
                                       Amount = dr["Amount"] != DBNull.Value ? Convert.ToInt32(dr["Amount"]) : 0
                                   }).ToList();

            return payrollBaseMultiple;
        }
        public PayrollBaseMultiple GetPayrollBaseMultipleById(int id)
        {
            return _PayrollBaseMultipleDataAccess.Get(id);
        }
        public bool DeletePayrollBaseMultiple(int Id)
        {
            return _PayrollBaseMultipleDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollCustomerBillingMethod(PayrollCustomerBillingMethod payrollCustomerBillingMethod)
        {
            return _PayrollCustomerBillingMethodDataAccess.Insert(payrollCustomerBillingMethod);
        }
        public bool UpdatePayrollCustomerBillingMethod(PayrollCustomerBillingMethod payrollCustomerBillingMethod)
        {
            return _PayrollCustomerBillingMethodDataAccess.Update(payrollCustomerBillingMethod) > 0;
        }
        public List<PayrollCustomerBillingMethod> GetPayrollCustomerBillingMethodList(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollCustomerBillingMethodDataAccess.GetByQuery(query);
        }
        public PayrollCustomerBillingMethod GetPayrollCustomerBillingMethodById(int id)
        {
            return _PayrollCustomerBillingMethodDataAccess.Get(id);
        }
        public bool DeletePayrollCustomerBillingMethod(int Id)
        {
            return _PayrollCustomerBillingMethodDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollSingleItemSettings(PayrollSingleItemSettings payrollSingleItemSettings)
        {
            return _PayrollSingleItemSettingsDataAccess.Insert(payrollSingleItemSettings);
        }
        public bool UpdatePayrollSingleItemSettings(PayrollSingleItemSettings payrollSingleItemSettings)
        {
            return _PayrollSingleItemSettingsDataAccess.Update(payrollSingleItemSettings) > 0;
        }
        public List<PayrollSingleItemSettings> GetPayrollSingleItemSettingsList(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollSingleItemSettingsDataAccess.GetByQuery(query);
        }
        public PayrollSingleItemSettings GetPayrollSingleItemSettingsById(int id)
        {
            return _PayrollSingleItemSettingsDataAccess.Get(id);
        }
        public bool DeletePayrollSingleItemSettings(int Id)
        {
            return _PayrollSingleItemSettingsDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollMonthlyProductionBonus(PayrollMonthlyProductionBonus payrollMonthlyProductionBonus)
        {
            return _PayrollMonthlyProductionBonusDataAccess.Insert(payrollMonthlyProductionBonus);
        }
        public bool UpdatePayrollMonthlyProductionBonus(PayrollMonthlyProductionBonus payrollMonthlyProductionBonus)
        {
            return _PayrollMonthlyProductionBonusDataAccess.Update(payrollMonthlyProductionBonus) > 0;
        }
        public List<PayrollMonthlyProductionBonus> GetPayrollMonthlyProductionBonusList(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollMonthlyProductionBonusDataAccess.GetByQuery(query);
        }
        public PayrollMonthlyProductionBonus GetPayrollMonthlyProductionBonusById(int id)
        {
            return _PayrollMonthlyProductionBonusDataAccess.Get(id);
        }
        public bool DeletePayrollMonthlyProductionBonus(int Id)
        {
            return _PayrollMonthlyProductionBonusDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollCreditRating(PayrollCreditRating payrollCreditRating)
        {
            return _PayrollCreditRatingDataAccess.Insert(payrollCreditRating);
        }
        public bool UpdatePayrollCreditRating(PayrollCreditRating payrollCreditRating)
        {
            return _PayrollCreditRatingDataAccess.Update(payrollCreditRating) > 0;
        }
        public List<PayrollCreditRating> GetPayrollCreditRatingList(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollCreditRatingDataAccess.GetByQuery(query);
        }
        public PayrollCreditRating GetPayrollCreditRatingById(int id)
        {
            return _PayrollCreditRatingDataAccess.Get(id);
        }
        public bool DeletePayrollCreditRating(int Id)
        {
            return _PayrollCreditRatingDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollCustomerType(PayrollCustomerType payrollCustomerType)
        {
            return _PayrollCustomerTypeDataAccess.Insert(payrollCustomerType);
        }
        public bool UpdatePayrollCustomerType(PayrollCustomerType payrollCustomerType)
        {
            return _PayrollCustomerTypeDataAccess.Update(payrollCustomerType) > 0;
        }
        public List<PayrollCustomerType> GetPayrollCustomerTypeList(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollCustomerTypeDataAccess.GetByQuery(query);
        }
        public PayrollCustomerType GetPayrollCustomerTypeById(int id)
        {
            return _PayrollCustomerTypeDataAccess.Get(id);
        }
        public bool DeletePayrollCustomerType(int Id)
        {
            return _PayrollCustomerTypeDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollAgreementLength(PayrollAgreementLength payrollAgreementLength)
        {
            return _PayrollAgreementLengthDataAccess.Insert(payrollAgreementLength);
        }
        public bool UpdatePayrollAgreementLength(PayrollAgreementLength payrollAgreementLength)
        {
            return _PayrollAgreementLengthDataAccess.Update(payrollAgreementLength) > 0;
        }
        public List<PayrollAgreementLength> GetPayrollAgreementLengthList(Guid termSheetId)
        {
            DataTable dt = _PayrollAgreementLengthDataAccess.GetPayrollAgreementLengthList(termSheetId);
            List<PayrollAgreementLength> payrollAgreementLength = new List<PayrollAgreementLength>();
            payrollAgreementLength = (from DataRow dr in dt.Rows
                                      select new PayrollAgreementLength()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          AgreementLength = dr["AgreementLength"].ToString(),
                                          Point = dr["Point"] != DBNull.Value ? Convert.ToInt32(dr["Point"]) : 0
                                      }).ToList();

            return payrollAgreementLength;
        }
        public PayrollAgreementLength GetPayrollAgreementLengthById(int id)
        {
            return _PayrollAgreementLengthDataAccess.Get(id);
        }
        public bool DeletePayrollAgreementLength(int Id)
        {
            return _PayrollAgreementLengthDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollPassThrus(PayrollPassThrus payrollPassThrus)
        {
            return _PayrollPassThrusDataAccess.Insert(payrollPassThrus);
        }
        public bool UpdatePayrollPassThrus(PayrollPassThrus payrollPassThrus)
        {
            return _PayrollPassThrusDataAccess.Update(payrollPassThrus) > 0;
        }
        public List<PayrollPassThrus> GetPayrollPassThrusList(Guid termSheetId)
        {
            DataTable dt = _PayrollPassThrusDataAccess.GetPayrollPassThrusList(termSheetId);
            List<PayrollPassThrus> payrollPassThrus = new List<PayrollPassThrus>();
            payrollPassThrus = (from DataRow dr in dt.Rows
                                select new PayrollPassThrus()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    PassThrus = dr["PassThrus"].ToString(),
                                    IsBase = dr["IsBase"] != DBNull.Value ? Convert.ToBoolean(dr["IsBase"]) : false,
                                    Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0
                                }).ToList();

            return payrollPassThrus;
        }
        public PayrollPassThrus GetPayrollPassThrusById(int id)
        {
            return _PayrollPassThrusDataAccess.Get(id);
        }
        public bool DeletePayrollPassThrus(int Id)
        {
            return _PayrollPassThrusDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollInstallationFee(PayrollInstallationFee payrollInstallationFee)
        {
            return _PayrollInstallationFeeDataAccess.Insert(payrollInstallationFee);
        }
        public bool UpdatePayrollInstallationFee(PayrollInstallationFee payrollInstallationFee)
        {
            return _PayrollInstallationFeeDataAccess.Update(payrollInstallationFee) > 0;
        }
        public List<PayrollInstallationFee> GetPayrollInstallationFeeList(Guid termSheetId)
        {
            string query = "TermSheetId='" + termSheetId + "'";
            return _PayrollInstallationFeeDataAccess.GetByQuery(query);
        }
        public PayrollInstallationFee GetPayrollInstallationFeeById(int id)
        {
            return _PayrollInstallationFeeDataAccess.Get(id);
        }
        public bool DeletePayrollInstallationFee(int Id)
        {
            return _PayrollInstallationFeeDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollHoldBack(PayrollHoldBack payrollHoldBack)
        {
            return _PayrollHoldBackDataAccess.Insert(payrollHoldBack);
        }
        public bool UpdatePayrollHoldBack(PayrollHoldBack payrollHoldBack)
        {
            return _PayrollHoldBackDataAccess.Update(payrollHoldBack) > 0;
        }
        public List<PayrollHoldBack> GetPayrollHoldBackList(Guid termSheetId)
        {
            DataTable dt = _PayrollHoldBackDataAccess.GetPayrollHoldBackList(termSheetId);
            List<PayrollHoldBack> payrollHoldBack = new List<PayrollHoldBack>();
            payrollHoldBack = (from DataRow dr in dt.Rows
                               select new PayrollHoldBack()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   HoldBack = dr["HoldBack"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   Percentage = dr["Percentage"] != DBNull.Value ? Convert.ToDouble(dr["Percentage"]) : 0.0
                               }).ToList();

            return payrollHoldBack;
        }
        public PayrollHoldBack GetPayrollHoldBackById(int id)
        {
            return _PayrollHoldBackDataAccess.Get(id);
        }
        public bool DeletePayrollHoldBack(int Id)
        {
            return _PayrollHoldBackDataAccess.Delete(Id) > 0;
        }


        public long InsertPayrollTermSheetManager(PayrollTermSheetManager payrollTermSheetManager)
        {
            return _PayrollTermSheetManagerDataAccess.Insert(payrollTermSheetManager);
        }
        public bool UpdatePayrollTermSheetManager(PayrollTermSheetManager payrollTermSheetManager)
        {
            return _PayrollTermSheetManagerDataAccess.Update(payrollTermSheetManager) > 0;
        }
        public List<PayrollTermSheetManager> GetPayrollTermSheetManagerList(Guid termSheetId)
        {
            DataTable dt = _PayrollTermSheetManagerDataAccess.GetPayrollTermSheetManagerList(termSheetId);
            List<PayrollTermSheetManager> payrollTermSheetManager = new List<PayrollTermSheetManager>();
            payrollTermSheetManager = (from DataRow dr in dt.Rows
                                       select new PayrollTermSheetManager()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           Name = dr["Name"].ToString(),
                                           Type = dr["Type"].ToString(),
                                           ManagerId = (Guid)dr["ManagerId"],
                                           Value = dr["Value"] != DBNull.Value ? Convert.ToDouble(dr["Value"]) : 0.0
                                       }).ToList();

            return payrollTermSheetManager;
        }
        public PayrollTermSheetManager GetPayrollTermSheetManagerById(int id)
        {
            return _PayrollTermSheetManagerDataAccess.Get(id);
        }
        public bool DeletePayrollTermSheetManager(int Id)
        {
            return _PayrollTermSheetManagerDataAccess.Delete(Id) > 0;
        }

        public long InsertPayrollAdminFee(PayrollAdminFee payrollAdminFee)
        {
            return _PayrollAdminFeeDataAccess.Insert(payrollAdminFee);
        }
        public bool UpdatePayrollAdminFee(PayrollAdminFee payrollAdminFee)
        {
            return _PayrollAdminFeeDataAccess.Update(payrollAdminFee) > 0;
        }
        public List<PayrollAdminFee> GetPayrollAdminFeeList(Guid termSheetId)
        {
            DataTable dt = _PayrollAdminFeeDataAccess.GetPayrollAdminFeeList(termSheetId);
            List<PayrollAdminFee> payrollAdminFee = new List<PayrollAdminFee>();
            payrollAdminFee = (from DataRow dr in dt.Rows
                               select new PayrollAdminFee()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   AdminFee = dr["AdminFee"].ToString(),
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0
                               }).ToList();

            return payrollAdminFee;
        }
        public PayrollAdminFee GetPayrollAdminFeeById(int id)
        {
            return _PayrollAdminFeeDataAccess.Get(id);
        }
        public bool DeletePayrollAdminFee(int Id)
        {
            return _PayrollAdminFeeDataAccess.Delete(Id) > 0;
        }

        public AdjustmentFunding GetAdjustmentFundingById(int id)
        {
            return _AdjustmentFundingDataAccess.Get(id);
        }
        public bool DeleteAdjustmentFunding(int Id)
        {
            return _AdjustmentFundingDataAccess.Delete(Id) > 0;
        }
        public EmpSalesPayReport GetAllPayrollBrinks(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, string SalesPersonList, string order, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            EmpSalesPayReport empSalesPay = new EmpSalesPayReport();

            DataSet ds = _EmployeeDataAccess.GetAllPayrollBrinks(FilterStartDate, FilterEndDate, pageno, pagesize, SearchText, SalesPersonList, order, PayrollBrinksStatus, PayrollBrinksFunding);
            List<SalesPay> salesPayList = new List<SalesPay>();
            salesPayList = (from DataRow dr in ds.Tables[0].Rows
                            select new SalesPay()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                SalesPerson = dr["SalesPerson"].ToString(),
                                SalesPersonId = (Guid)dr["SalesPersonId"],
                                TotalRMR = dr["MMR"] != DBNull.Value ? Convert.ToDouble(dr["MMR"]) : 0,
                                GrossPay = dr["GrossPay"] != DBNull.Value ? Convert.ToDouble(dr["GrossPay"]) : 0,
                                Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                Deductions = dr["Deductions"] != DBNull.Value ? Convert.ToDouble(dr["Deductions"]) : 0,
                                NetPay = dr["NetPay"] != DBNull.Value ? Convert.ToDouble(dr["NetPay"]) : 0,
                                TermSheetName = dr["TermSheetName"].ToString(),
                                TermSheetId = dr["TermSheetId"] != DBNull.Value ? (Guid)dr["TermSheetId"] : Guid.Empty,
                                HoldBack = dr["HoldBack"] != DBNull.Value ? Convert.ToDouble(dr["HoldBack"]) : 0,
                                PassThrus = dr["GrossPay"] != DBNull.Value ? Convert.ToDouble(dr["PassThrus"]) : 0,
                            }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();

            empSalesPay.TotalTotalRMR = ds.Tables[2].Rows[0]["TotalTotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTotalRMR"]) : 0;
            empSalesPay.TotalGrossPay = ds.Tables[2].Rows[0]["TotalGrossPay"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalGrossPay"]) : 0;
            empSalesPay.TotalDeductions = ds.Tables[2].Rows[0]["TotalDeductions"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalDeductions"]) : 0;
            empSalesPay.TotalAdjustments = ds.Tables[2].Rows[0]["TotalAdjustments"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdjustments"]) : 0;
            empSalesPay.TotalNetPay = ds.Tables[2].Rows[0]["TotalNetPay"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalNetPay"]) : 0;
            empSalesPay.TotalHoldBack = ds.Tables[2].Rows[0]["TotalHoldBack"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalHoldBack"]) : 0;
            empSalesPay.TotalPassThru = ds.Tables[2].Rows[0]["TotalPassThru"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalPassThru"]) : 0;

            empSalesPay.SalesPayList = salesPayList;
            empSalesPay.PayrollTotalCount = PayrollTotalCount;
            return empSalesPay;
        }
        public PayrollBrinks GetDedudctionDetailsByPayrollBrinksId(int PayrollBrinksId)
        {
            PayrollBrinks payrollBrinks = new PayrollBrinks();

            DataSet ds = _EmployeeDataAccess.GetDedudctionDetailsByPayrollBrinksId(PayrollBrinksId);

            payrollBrinks.CustomerIntId = ds.Tables[0].Rows[0]["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerIntId"]) : 0;
            payrollBrinks.Deductions = ds.Tables[0].Rows[0]["Deductions"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["Deductions"]) : 0;
            payrollBrinks.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            payrollBrinks.EquipmentRepCost = ds.Tables[0].Rows[0]["EquipmentRepCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["EquipmentRepCost"]) : 0;
            payrollBrinks.UpFrontCollect = ds.Tables[0].Rows[0]["UpFrontCollect"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["UpFrontCollect"]) : 0;
            payrollBrinks.EquipmentAdjustment = ds.Tables[0].Rows[0]["EquipmentAdjustment"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["EquipmentAdjustment"]) : 0;
            payrollBrinks.InstallationFee = ds.Tables[0].Rows[0]["InstallationFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["InstallationFee"]) : 0;
            payrollBrinks.HoldBack = ds.Tables[0].Rows[0]["HoldBack"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["HoldBack"]) : 0;
            payrollBrinks.PassThrus = ds.Tables[0].Rows[0]["PassThrus"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["PassThrus"]) : 0;
            payrollBrinks.MMR = ds.Tables[0].Rows[0]["MMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["MMR"]) : 0;
            payrollBrinks.Multiple = ds.Tables[0].Rows[0]["Multiple"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["Multiple"]) : 0;
            payrollBrinks.TotalPassThrus = ds.Tables[0].Rows[0]["TotalPassThrus"] != DBNull.Value ? Convert.ToDouble(ds.Tables[0].Rows[0]["TotalPassThrus"]) : 0;

            payrollBrinks.EquipmentAdjustList = (from DataRow dr in ds.Tables[1].Rows
                                                 select new EquipmentAdjust()
                                                 {
                                                     EquipmentName = dr["EquipmentName"].ToString(),
                                                     Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0
                                                 }).ToList();
            return payrollBrinks;
        }
        public DataTable DownLoadAllPayrollBrinks(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, string SalesPersonList, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            DataSet ds = _EmployeeDataAccess.DownLoadAllPayrollBrinks(FilterStartDate, FilterEndDate, pageno, pagesize, SearchText, SalesPersonList, PayrollBrinksStatus, PayrollBrinksFunding);
            DataTable buildList = ds.Tables[0];
            return buildList;
        }
        public DataTable DownLoadPayrollBrinksBySalesPersonId(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, Guid UserId, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            DataSet ds = _EmployeeDataAccess.DownLoadPayrollBrinksBySalesPersonId(FilterStartDate, FilterEndDate, pageno, pagesize, SearchText, UserId, PayrollBrinksStatus, PayrollBrinksFunding);
            DataTable buildList = ds.Tables[0];
            return buildList;
        }
        public EmpSalesPayReport GetPayrollBrinksBySalesPersonId(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, Guid UserId, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            EmpSalesPayReport empSalesPay = new EmpSalesPayReport();

            DataSet ds = _EmployeeDataAccess.GetPayrollBrinksBySalesPersonId(FilterStartDate, FilterEndDate, pageno, pagesize, SearchText, UserId, PayrollBrinksStatus, PayrollBrinksFunding);
            List<SalesPay> salesPayList = new List<SalesPay>();
            salesPayList = (from DataRow dr in ds.Tables[0].Rows
                            select new SalesPay()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                SalesPersonId = (Guid)dr["SalesPersonId"],
                                CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                AccountId = dr["AccountId"] != DBNull.Value ? Convert.ToInt32(dr["AccountId"]) : 0,
                                CustomerName = dr["CustomerName"].ToString(),
                                BrinksFundingStatus = dr["BrinksFundingStatus"].ToString(),
                                FinanceFundingStatus = dr["FinanceFundingStatus"].ToString(),
                                PaymentMethodForRMR = dr["PaymentMethodForRMR"].ToString(),
                                CreditScoreValue = dr["CreditScoreValue"] != DBNull.Value ? Convert.ToDouble(dr["CreditScoreValue"]) : 0,
                                CreditGrade = dr["CreditGrade"].ToString(),
                                Type = dr["Type"].ToString(),
                                ContractTerm = dr["ContractTerm"].ToString(),
                                PayType = dr["PayType"].ToString(),
                                TotalRMR = dr["MMR"] != DBNull.Value ? Convert.ToDouble(dr["MMR"]) : 0,
                                TotalMultiple = dr["Multiple"] != DBNull.Value ? Convert.ToDouble(dr["Multiple"]) : 0,
                                FinanceMonthlyPayment = dr["FinanceMonthlyPayment"] != DBNull.Value ? Convert.ToDouble(dr["FinanceMonthlyPayment"]) : 0,
                                TotalMonthly = dr["TotalMonthly"] != DBNull.Value ? Convert.ToDouble(dr["TotalMonthly"]) : 0,
                                GrossPay = dr["GrossPay"] != DBNull.Value ? Convert.ToDouble(dr["GrossPay"]) : 0,
                                Adjustment = dr["Adjustments"] != DBNull.Value ? Convert.ToDouble(dr["Adjustments"]) : 0,
                                Deductions = dr["Deductions"] != DBNull.Value ? Convert.ToDouble(dr["Deductions"]) : 0,
                                NetPay = dr["NetPay"] != DBNull.Value ? Convert.ToDouble(dr["NetPay"]) : 0,
                                FundingStatus = dr["FundingStatus"].ToString(),
                                PassThrus = dr["PassThrus"] != DBNull.Value ? Convert.ToDouble(dr["PassThrus"]) : 0,
                                EquipmentList = dr["EquipmentList"].ToString()
                            }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();

            empSalesPay.TotalTotalRMR = ds.Tables[2].Rows[0]["TotalTotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTotalRMR"]) : 0;
            empSalesPay.TotalGrossPay = ds.Tables[2].Rows[0]["TotalGrossPay"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalGrossPay"]) : 0;
            empSalesPay.TotalDeductions = ds.Tables[2].Rows[0]["TotalDeductions"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalDeductions"]) : 0;
            empSalesPay.TotalAdjustments = ds.Tables[2].Rows[0]["TotalAdjustments"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdjustments"]) : 0;
            empSalesPay.TotalNetPay = ds.Tables[2].Rows[0]["TotalNetPay"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalNetPay"]) : 0;

            empSalesPay.SalesPayList = salesPayList;
            empSalesPay.PayrollTotalCount = PayrollTotalCount;
            return empSalesPay;
        }
        public bool UpdatePayrollBrinksFund(DateTime FilterStartDate, DateTime FilterEndDate, string SearchText, string SalesPersonList, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            return _PayrollBrinksDataAccess.UpdatePayrollBrinksFund(FilterStartDate, FilterEndDate, SearchText, SalesPersonList, PayrollBrinksStatus, PayrollBrinksFunding);
        }
        public long InsertPayrollBrinks(PayrollBrinks payrollBrinks)
        {
            return _PayrollBrinksDataAccess.Insert(payrollBrinks);
        }
        public bool UpdatePayrollBrinks(PayrollBrinks payrollBrinks)
        {
            return _PayrollBrinksDataAccess.Update(payrollBrinks) > 0;
        }
        public PayrollBrinks GetPayrollBrinksById(int id)
        {
            return _PayrollBrinksDataAccess.Get(id);
        }
        public bool DeletePayrollBrinks(int Id)
        {
            return _PayrollBrinksDataAccess.Delete(Id) > 0;
        }
        public bool DeleteManagerPayrollBrinksByTicketId(Guid ticketId)
        {
            return _PayrollBrinksDataAccess.DeleteManagerPayrollBrinksByTicketId(ticketId);
        }
        public PayrollBrinks GetPayrollBrinksByTicketId(Guid ticketId)
        {
            string query = string.Format("TicketId='{0}'", ticketId);
            return _PayrollBrinksDataAccess.GetByQuery(query).FirstOrDefault();
        }

        public bool UpdatePayrollClusterFunding(string ticketIdJoin, int batch, DateTime PaidDate)
        {
            return _SalesCommissionDataAccess.UpdatePayrollClusterFunding(ticketIdJoin, batch, PaidDate);
        }
        public bool UpdatePayrollSingleFunding(string idSales, string idTech, string idAddMember, string idFinRep, string idServiceCall, string idFollowUp, string idReshcedule, string idAdjustmentFunding, int batch, DateTime PaidDate)
        {
            return _SalesCommissionDataAccess.UpdatePayrollSingleFunding(idSales, idTech, idAddMember, idFinRep, idServiceCall, idFollowUp, idReshcedule, idAdjustmentFunding, batch, PaidDate);
        }
        public DataTable GetInsideCommissionReportExportByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext, List<string> SalesRepList, List<string> FinRepList, DateTime? PayrollStartDate, DateTime? PayrollEndDate)
        {
            return _CustomerDataAccess.GetInsideCommissionReportExportByCompanyId(companyId, start, end, searchtext, SalesRepList, FinRepList, PayrollStartDate, PayrollEndDate);
        }
        public InsideCommissionModel GetInsideCommissionReportALLByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, List<string> SalesRepList, List<string> FinRepList, DateTime? PayrollStartDate, DateTime? PayrollEndDate)
        {
            InsideCommissionModel Model = new InsideCommissionModel();
            DataSet ds = _CustomerDataAccess.GetInsideCommissionReportALLByCompanyId(companyId, start, end, searchtext, pageno, pagesize, SalesRepList, FinRepList, PayrollStartDate, PayrollEndDate);
            Model.InsideCommission = new List<InsideCommission>();
            if (ds != null)
                Model.InsideCommission = (from DataRow dr in ds.Tables[0].Rows
                                          select new InsideCommission()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              Date = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime(),
                                              PayrollDate = dr["PayrollDate"] != DBNull.Value ? Convert.ToDateTime(dr["PayrollDate"]) : new DateTime(),
                                              Batch = dr["Batch"] != DBNull.Value ? Convert.ToInt32(dr["Batch"]) : 0,
                                              Rep = dr["SalesPerson"].ToString(),
                                              DisplayName = dr["DisplayName"].ToString(),
                                              RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                              Activation = dr["Activation"] != DBNull.Value ? Convert.ToDouble(dr["Activation"]) : 0,
                                              Equipment = dr["EquipmentFee"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentFee"]) : 0,
                                              Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                                              Comm = dr["Comm"] != DBNull.Value ? Convert.ToDouble(dr["Comm"]) : 0,
                                              FinanceRep = dr["FinanceRep"].ToString(),
                                              FinanceRepComm = dr["FinRepCommission"] != DBNull.Value ? Convert.ToDouble(dr["FinRepCommission"]) : 0,
                                          }).ToList();
            Model.Totalcount = ds.Tables[1].Rows[0]["Totalcount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Totalcount"]) : 0;
            Model.SumCustomer = ds.Tables[1].Rows[0]["SumCustomer"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["SumCustomer"]) : 0;
            Model.SumRMR = ds.Tables[1].Rows[0]["SumRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumRMR"]) : 0;
            Model.SumActivation = ds.Tables[1].Rows[0]["SumActivation"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumActivation"]) : 0;
            Model.SumEquipmentFee = ds.Tables[1].Rows[0]["SumEquipmentFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumEquipmentFee"]) : 0;
            Model.SumTotal = ds.Tables[1].Rows[0]["SumTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotal"]) : 0;
            Model.SumComm = ds.Tables[1].Rows[0]["SumComm"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumComm"]) : 0;
            Model.SumFinRepCommission = ds.Tables[1].Rows[0]["SumFinRepCommission"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumFinRepCommission"]) : 0;

            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.TotalActivation = ds.Tables[2].Rows[0]["TotalActivation"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalActivation"]) : 0.0;
            Model.TotalEquipment = ds.Tables[2].Rows[0]["TotalEquipmentFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentFee"]) : 0.0;
            Model.TotalTotal = ds.Tables[2].Rows[0]["TotalTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTotal"]) : 0.0;
            Model.TotalComm = ds.Tables[2].Rows[0]["TotalComm"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalComm"]) : 0.0;
            Model.TotalFinanceRepComm = ds.Tables[2].Rows[0]["TotalFinRepCommission"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalFinRepCommission"]) : 0.0;
            Model.pageno = pageno;
            Model.pagesize = pagesize;

            return Model;
        }
    }
}
