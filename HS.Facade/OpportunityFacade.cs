using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;

namespace HS.Facade
{
    public class OpportunityFacade:BaseFacade
    {
        public OpportunityFacade(ClientContext clientContext)
            : base(clientContext)
        {

        } 
        OpportunityDataAccess _OpportunityDataAccess
        {
            get
            {
                return (OpportunityDataAccess)_ClientContext[typeof(OpportunityDataAccess)];
            }
        }
        public bool InsertOpportunity(Opportunity opportunity)
        {
            return _OpportunityDataAccess.Insert(opportunity) > 0;
        }
        public bool UpdateOpportunity(Opportunity opportunity)
        {
            return _OpportunityDataAccess.Update(opportunity) > 0;
        }
        public List<Opportunity> GetAllOpportunity()
        {
            return _OpportunityDataAccess.GetAll();
        }
        public OpportunityModel GetOpportunities(OpportunityFilter filter)
        {
            return _OpportunityDataAccess.GetOpportunities(filter);
        }
        public List<Opportunity> GetFilteredOpportunities(OpportunityFilter filter)
        {
            return _OpportunityDataAccess.GetFilteredOpportunities(filter);
        }
        public DataTable GetAllOpportunityForExport(string customerid)
        {
            return _OpportunityDataAccess.GetAllOpportunityForExport(customerid);
        }
        public DataTable GetAllOpportunityDatabaseForExport()
        {
            return _OpportunityDataAccess.GetAllOpportunityDatabaseForExport();
        }
        public long DeleteOpportunity(int id)
        {
            return _OpportunityDataAccess.Delete(id);
        }
        public List<Opportunity> GetOpportunyByCustomerId(Guid CustomerId)
        {
            DataTable dt = _OpportunityDataAccess.GetOpportunityListByCompanyId(CustomerId);
            List<Opportunity> OpportunityList = new List<Opportunity>();

            OpportunityList = (from DataRow dr in dt.Rows
                               select new Opportunity()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   OpportunityName = dr["OpportunityName"].ToString(),
                                   TypeVal = dr["TypeVal"].ToString(),
                                   LeadSource = dr["LeadSource"].ToString(),
                                   AccessGivenToVal = dr["AccessGivenToVal"].ToString(),
                                   Revenue = dr["Revenue"].ToString(),
                                   ProjectedGP = dr["ProjectedGP"].ToString(),
                                   Points = dr["Points"].ToString(),
                                   TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                  
                                   StatusVal = dr["StatusVal"].ToString(),
                                   ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                   DealReasonVal = dr["DealReasonVal"].ToString(),
                                   DeliveryDaysVal = dr["DeliveryDaysVal"].ToString(),
                                   Competitors = dr["Competitors"].ToString(),
                                   AccountOwnerName = dr["AccountOwnerName"].ToString(),
                                   CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                   AccountOwner = (Guid)dr["CustomerId"],
                                   VehicleCondition = dr["VehicleCondition"].ToString(),
                                   VehicleConditionVal = dr["VehicleConditionVal"].ToString(),
                                   LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                   CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                               }).ToList();

            return OpportunityList;
        }
        public Opportunity GetOpportunyDetailById(int Id)
        {
            DataTable dt = _OpportunityDataAccess.GetOpportunityDetailById(Id);
            Opportunity  opportunity = new Opportunity();
            opportunity = (from DataRow dr in dt.Rows
                               select new Opportunity()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   OpportunityName = dr["OpportunityName"].ToString(),
                                   TypeVal = dr["TypeVal"].ToString(),
                                   LeadSource = dr["LeadSource"].ToString(),
                                   Revenue = dr["Revenue"].ToString(),
                                   ProjectedGP = dr["ProjectedGP"].ToString(),
                                   Points = dr["Points"].ToString(),
                                   TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                   AccessGivenToVal = dr["AccessGivenToVal"].ToString(),
                                   StatusVal = dr["StatusVal"].ToString(),
                                   ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                   DealReasonVal = dr["DealReasonVal"].ToString(),
                                   DeliveryDaysVal = dr["DeliveryDaysVal"].ToString(),
                                   Competitors = dr["Competitors"].ToString(),
                                   AccountOwnerName = dr["AccountOwnerName"].ToString(),
                                   CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                   AccountOwner = (Guid)dr["CustomerId"],

                                   LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                   CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                   CustomerName = dr["CustomerName"].ToString(),
                                   IdCustomer = dr["IdCustomer"] != DBNull.Value ? Convert.ToInt32(dr["IdCustomer"]) : 0,
                                   VehicleConditionVal = dr["VehicleConditionVal"].ToString(),
                                   VehicleCondition = dr["VehicleCondition"].ToString()
                               }).ToList().FirstOrDefault();

            return opportunity;
        }
        public List<Opportunity> GetOpportunyByIds(string IdList)
        {
            DataTable dt = _OpportunityDataAccess.GetOpportunityListByIds(IdList);
            List<Opportunity> OpportunityList = new List<Opportunity>();
            OpportunityList = (from DataRow dr in dt.Rows
                               select new Opportunity()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   OpportunityName = dr["OpportunityName"].ToString(),
                                   TypeVal = dr["TypeVal"].ToString(),
                                   LeadSource = dr["LeadSource"].ToString(),
                                   Revenue = dr["Revenue"].ToString(),
                                   ProjectedGP = dr["ProjectedGP"].ToString(),
                                   Points = dr["Points"].ToString(),
                                   TotalProjectedGP = dr["TotalProjectedGP"].ToString(),

                                   StatusVal = dr["StatusVal"].ToString(),
                                   ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                   DealReasonVal = dr["DealReasonVal"].ToString(),
                                   DeliveryDaysVal = dr["DeliveryDaysVal"].ToString(),
                                   Competitors = dr["Competitors"].ToString(),
                                   AccountOwnerName = dr["AccountOwnerName"].ToString(),
                                   CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                   AccountOwner = (Guid)dr["CustomerId"],

                                   LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                   CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                               }).ToList();

            return OpportunityList;
        }
        public List<Opportunity> GetAllOpportunityByOpportunityIdList(string OpportunityIdList)
        {
           return _OpportunityDataAccess.GetAllOpportunityByOpportunityIdList(OpportunityIdList);
        }
        public Opportunity GetOpportunityById(int id)
        {
            return _OpportunityDataAccess.Get(id);
        }
        public Opportunity GetOpportunityByOpportunityId(Guid OpportinityId)
        {
            return _OpportunityDataAccess.GetByQuery(string.Format("OpportunityId ='{0}'", OpportinityId)).FirstOrDefault(); ;
        }
        public List<Opportunity> GetAllOpportunitybyAccountOwner(Guid AccountOwner)
        {
            return _OpportunityDataAccess.GetByQuery(string.Format("AccountOwner ='{0}'", AccountOwner)).ToList(); ;
        }
        public DataTable GetAllOpportunityList(string query)
        {
            return _OpportunityDataAccess.GetAllOpportunityList(query);
        }

        public List<Opportunity> GetOpportunitiesBySearchKey(string key, string employeeTag, Guid empid)
        {
            return _OpportunityDataAccess.GetOpportunitiesBySearchKey(key, employeeTag, empid);
        }

        public OpportunityListFilterModel GetAllOpportunitiesReportHard(int pageno, int pagesize, string OppType, string year, string delivery, string month, string accOwner, string soldBy)
        {
            DataSet ds = _OpportunityDataAccess.GetAllOpportunitiesReportHard(pageno, pagesize, OppType, year, delivery, month, accOwner, soldBy);
            OpportunityListFilterModel model = new OpportunityListFilterModel();
            if (!string.IsNullOrWhiteSpace(month))
            {
                if(month == "01")
                {
                    model.ListOpportunityJan = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelJan = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "02")
                {
                    model.ListOpportunityFeb = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelFeb = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "03")
                {
                    model.ListOpportunityMar = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelMar = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "04")
                {
                    model.ListOpportunityApr = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelApr = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "05")
                {
                    model.ListOpportunityMay = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelMay = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "06")
                {
                    model.ListOpportunityJun = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelJun = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "07")
                {
                    model.ListOpportunityJul = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelJul = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "08")
                {
                    model.ListOpportunityAug = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelAug = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "09")
                {
                    model.ListOpportunitySep = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelSep = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "10")
                {
                    model.ListOpportunityOct = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelOct = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "11")
                {
                    model.ListOpportunityNov = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelNov = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
                if (month == "12")
                {
                    model.ListOpportunityDec = (from DataRow dr in ds.Tables[0].Rows
                                                select new Opportunity()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    OpportunityName = dr["OpportunityName"].ToString(),
                                                    ProjectedGP = dr["ProjectedGP"].ToString(),
                                                    Points = dr["Points"].ToString(),
                                                    TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                    StatusVal = dr["StatusVal"].ToString(),
                                                    CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                    TypeVal = dr["TypeVal"].ToString(),
                                                    SourceLead = dr["SourceLead"].ToString(),
                                                    Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                    ReasonDeal = dr["ReasonDeal"].ToString(),
                                                    DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                    CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                    CustomerName = dr["CustomerName"].ToString(),
                                                    EmpName = dr["EmpName"].ToString(),
                                                    ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                    Revenue = dr["Revenue"].ToString(),
                                                    CustomerStreet = dr["CustomerStreet"].ToString(),
                                                    CustomerCity = dr["CustomerCity"].ToString(),
                                                    CustomerState = dr["CustomerState"].ToString(),
                                                    CustomerZip = dr["CustomerZip"].ToString(),
                                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                                    MonthOpportunity = "Jan",
                                                    CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                    CustomerDBA = dr["CustomerDBA"].ToString(),
                                                    CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                                }).ToList();
                    model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                        select new TotalOpportunityCountModel()
                                                        {
                                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                        }).FirstOrDefault();
                    model.HardBacklogRevenueModelDec = (from DataRow dr in ds.Tables[2].Rows
                                                        select new HardBacklogRevenueModel()
                                                        {
                                                            TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                            TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                            TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                            ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                        }).FirstOrDefault();
                }
            }
            else
            {
                model.ListOpportunityJan = (from DataRow dr in ds.Tables[0].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Jan",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityFeb = (from DataRow dr in ds.Tables[1].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Feb",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityMar = (from DataRow dr in ds.Tables[2].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Mar",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityApr = (from DataRow dr in ds.Tables[3].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Apr",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityMay = (from DataRow dr in ds.Tables[4].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "May",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityJun = (from DataRow dr in ds.Tables[5].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Jun",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityJul = (from DataRow dr in ds.Tables[6].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Jul",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityAug = (from DataRow dr in ds.Tables[7].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Aug",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunitySep = (from DataRow dr in ds.Tables[8].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Sep",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityOct = (from DataRow dr in ds.Tables[9].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Oct",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityNov = (from DataRow dr in ds.Tables[10].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Nov",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.ListOpportunityDec = (from DataRow dr in ds.Tables[11].Rows
                                            select new Opportunity()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                OpportunityName = dr["OpportunityName"].ToString(),
                                                ProjectedGP = dr["ProjectedGP"].ToString(),
                                                Points = dr["Points"].ToString(),
                                                TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                                StatusVal = dr["StatusVal"].ToString(),
                                                CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                                TypeVal = dr["TypeVal"].ToString(),
                                                SourceLead = dr["SourceLead"].ToString(),
                                                Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                                ReasonDeal = dr["ReasonDeal"].ToString(),
                                                DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                                CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                                CustomerName = dr["CustomerName"].ToString(),
                                                EmpName = dr["EmpName"].ToString(),
                                                ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                                Revenue = dr["Revenue"].ToString(),
                                                CustomerStreet = dr["CustomerStreet"].ToString(),
                                                CustomerCity = dr["CustomerCity"].ToString(),
                                                CustomerState = dr["CustomerState"].ToString(),
                                                CustomerZip = dr["CustomerZip"].ToString(),
                                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                                MonthOpportunity = "Dec",
                                                CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                                CustomerDBA = dr["CustomerDBA"].ToString(),
                                                CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                            }).ToList();
                model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[12].Rows
                                                    select new TotalOpportunityCountModel()
                                                    {
                                                        TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelJan = (from DataRow dr in ds.Tables[13].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelFeb = (from DataRow dr in ds.Tables[14].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelMar = (from DataRow dr in ds.Tables[15].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelApr = (from DataRow dr in ds.Tables[16].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelMay = (from DataRow dr in ds.Tables[17].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelJun = (from DataRow dr in ds.Tables[18].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelJul = (from DataRow dr in ds.Tables[19].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelAug = (from DataRow dr in ds.Tables[20].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelSep = (from DataRow dr in ds.Tables[21].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelOct = (from DataRow dr in ds.Tables[22].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelNov = (from DataRow dr in ds.Tables[23].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
                model.HardBacklogRevenueModelDec = (from DataRow dr in ds.Tables[24].Rows
                                                    select new HardBacklogRevenueModel()
                                                    {
                                                        TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                                        TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                                        TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                                        ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0
                                                    }).FirstOrDefault();
            }
            return model;
        }

        public OpportunityListFilterModel GetAllOpportunitiesReport(int pageno, int pagesize, DateTime? start, DateTime? end, string OppType, string year, string delivery, string month, string acOwner, string soldBy,string statusType)
        {
            DataSet ds = _OpportunityDataAccess.GetAllOpportunitiesReport(pageno, pagesize, start, end, OppType, year, delivery, month, acOwner,soldBy, statusType);
            OpportunityListFilterModel model = new OpportunityListFilterModel();
            model.ListOpportunity = (from DataRow dr in ds.Tables[0].Rows
                                        select new Opportunity()
                                        {
                                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                            OpportunityName = dr["OpportunityName"].ToString(),
                                            ProjectedGP = dr["ProjectedGP"].ToString(),
                                            Points = dr["Points"].ToString(),
                                            TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                            StatusVal = dr["StatusVal"].ToString(),
                                            CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                            TypeVal = dr["TypeVal"].ToString(),
                                            SourceLead = dr["SourceLead"].ToString(),
                                            Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                            ReasonDeal = dr["ReasonDeal"].ToString(),
                                            DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                            DeliveryDayDiff = dr["DeliveryDayDiff"].ToString(),
                                            CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                            CustomerName = dr["CustomerName"].ToString(),
                                            EmpName = dr["EmpName"].ToString(),
                                            ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                            Revenue = dr["Revenue"].ToString(),
                                            CustomerStreet = dr["CustomerStreet"].ToString(),
                                            CustomerCity = dr["CustomerCity"].ToString(),
                                            CustomerState = dr["CustomerState"].ToString(),
                                            CustomerZip = dr["CustomerZip"].ToString(),
                                            CustomerEmail = dr["CustomerEmail"].ToString(),
                                            MonthOpportunity = "Jan",
                                            CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0
                                        }).ToList();
            
            model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                select new TotalOpportunityCountModel()
                                                {
                                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                }).FirstOrDefault();
            return model;
        }

        public OpportunityListFilterModel GetAllOpportunitiesReportSoft(int pageno, int pagesize, DateTime? start, DateTime? end, string OppType, string year, string deliveryday, string month, string accOwner, string soldBy,string statusType)
        {
            DataSet ds = _OpportunityDataAccess.GetAllOpportunitiesReport(pageno, pagesize, start, end, OppType, year, deliveryday, month, accOwner, soldBy, statusType);
            OpportunityListFilterModel model = new OpportunityListFilterModel();
            model.ListOpportunity = (from DataRow dr in ds.Tables[0].Rows
                                     select new Opportunity()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         OpportunityName = dr["OpportunityName"].ToString(),
                                         ProjectedGP = dr["ProjectedGP"].ToString(),
                                         Points = dr["Points"].ToString(),
                                         TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                         StatusVal = dr["StatusVal"].ToString(),
                                         CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                         TypeVal = dr["TypeVal"].ToString(),
                                         SourceLead = dr["SourceLead"].ToString(),
                                         Probability = dr["Probability"] != DBNull.Value ? Convert.ToInt32(dr["Probability"]) : 0,
                                         ReasonDeal = dr["ReasonDeal"].ToString(),
                                         DeliverydayVal = dr["DeliverydayVal"].ToString(),
                                         DeliveryDayDiff = dr["DeliveryDayDiff"].ToString(),
                                         CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                         CustomerName = dr["CustomerName"].ToString(),
                                         EmpName = dr["EmpName"].ToString(),
                                         ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                         Revenue = dr["Revenue"].ToString(),
                                         CustomerStreet = dr["CustomerStreet"].ToString(),
                                         CustomerCity = dr["CustomerCity"].ToString(),
                                         CustomerState = dr["CustomerState"].ToString(),
                                         CustomerZip = dr["CustomerZip"].ToString(),
                                         CustomerEmail = dr["CustomerEmail"].ToString(),
                                         MonthOpportunity = "Jan",
                                         CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                         CustomerDBA = dr["CustomerDBA"].ToString(),
                                         CustomerBusinessName = dr["CustomerBusinessName"].ToString()
                                     }).ToList();

            model.TotalOpportunityCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                select new TotalOpportunityCountModel()
                                                {
                                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                                }).FirstOrDefault();
            model.RevenueModel = (from DataRow dr in ds.Tables[2].Rows
                                  select new RevenueModel()
                                  {
                                      TotalRevenueVal = dr["TotalRevenueVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenueVal"]) : 0,
                                      TotalProjectedGPVal = dr["TotalProjectedGPVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalProjectedGPVal"]) : 0,
                                      TotalPointsVal = dr["TotalPointsVal"] != DBNull.Value ? Convert.ToDouble(dr["TotalPointsVal"]) : 0,
                                      ProjectedGPTotalVal = dr["ProjectedGPTotalVal"] != DBNull.Value ? Convert.ToDouble(dr["ProjectedGPTotalVal"]) : 0,
                                      DeliveryDayDiff = dr["DeliveryDayDiff"].ToString()
                                  }).ToList();
            return model;
        }

        public DataTable GetAllOpportunitiesReportExport(DateTime? start, DateTime? end, string OppType, string year, string delivery, string month, string accOwner, string soldBy,string statusType)
        {
            return _OpportunityDataAccess.GetAllOpportunitiesReportExport(start, end, OppType, year, delivery, month, accOwner, soldBy, statusType);
        }
        public DataTable GetAllOpportunitiesForHardBacklogReportExport(string OppType, string year, string delivery, string month, string accOwner, string soldBy)
        {
            return _OpportunityDataAccess.GetAllOpportunitiesForHadrBacklogReportExport(OppType, year, delivery, month, accOwner, soldBy);
        }
    }
}
