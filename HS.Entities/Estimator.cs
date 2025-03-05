using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.EnterpriseServices.Internal;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Estimator 
	{
        public bool IsEstimate { get; set; }
        public string CustomerName { set; get; }
        public string EstimatorDescription { set; get; }
        public string CustomerBusinessName { set; get; }
        public string AccountNo { set; get; } 
        public double ServiceSubTotal { get; set; }
        public double TotalEstimateServiceAmount { get; set; }
        public string CreatedByName { set; get; } 
        public string ParentBillingAddress { set; get; }
        public List<EstimatorService> EstimatorService { set; get; }
        public List<EstimatorDetail> EstimatorDetailList { set; get; }
        public List<EstimatorFile> EstimatorFileList { set; get; }
        public string ServicePlanTypeName { set; get; }
        public string CustomerBussinessName { get; set; }
        public Nullable<Boolean> IsSigned { get; set; }
    }

    public class EstimatorDashboard
    {
        public List<Estimator> EstimatorList { get; set; }
        public int TotalCount { get; set; }
        public int OpenCount { get; set; }
        public int PendingCount { get; set; }
        public int AcceptedCount { get; set; } 
        public int CountEstimator { get; set; }
    }
}
