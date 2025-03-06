using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EstimatorBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EstimatorBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			EstimatorId = 2,
			CustomerId = 3,
			BillingAddress = 4,
			ProjectAddress = 5,
			Status = 6,
			StartDate = 7,
			CompletionDate = 8,
			EmailAddress = 9,
			Description = 10,
			TaxPercnetage = 11,
			TaxAmount = 12,
			TotalPrice = 13,
			TotalCost = 14,
			PoriftPercentage = 15,
			TotalProfitAmount = 16,
			OverheadCostPercentage = 17,
			TotalOverheadCostAmount = 18,
			CreatedBy = 19,
			CreatedDate = 20,
			LastUpdatedBy = 21,
			LastUpdatedDate = 22,
			DefaultOverheadRate = 23,
			DefaultProfitRate = 24,
			CoverLetter = 25,
			CoverLetterFile = 26,
			PaymentTerm = 27,
			ExpiresOn = 28,
			EstimateDate = 29,
			ShowServicePlan = 30,
			ServicePlanRate = 31,
			ShowService = 32,
			ServicePlanAmount = 33,
			ServiceTaxAmount = 34,
			ServiceTotalAmount = 35,
			ServicePlanType = 36,
			ContractTerm = 37,
			DefaultMaterialMarkupRate = 38,
			ActivationFee = 39,
			EstimatorSignature = 40,
			ParentEstimatorRef = 41,
			IsApproved = 42,
			OneTimeServiceTaxAmount = 43,
			OneTimeServiceTotalAmount = 44
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EstimatorId = "EstimatorId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_ProjectAddress = "ProjectAddress";		            
		public const string Property_Status = "Status";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_CompletionDate = "CompletionDate";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_Description = "Description";		            
		public const string Property_TaxPercnetage = "TaxPercnetage";		            
		public const string Property_TaxAmount = "TaxAmount";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_TotalCost = "TotalCost";		            
		public const string Property_PoriftPercentage = "PoriftPercentage";		            
		public const string Property_TotalProfitAmount = "TotalProfitAmount";		            
		public const string Property_OverheadCostPercentage = "OverheadCostPercentage";		            
		public const string Property_TotalOverheadCostAmount = "TotalOverheadCostAmount";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_DefaultOverheadRate = "DefaultOverheadRate";		            
		public const string Property_DefaultProfitRate = "DefaultProfitRate";		            
		public const string Property_CoverLetter = "CoverLetter";		            
		public const string Property_CoverLetterFile = "CoverLetterFile";		            
		public const string Property_PaymentTerm = "PaymentTerm";		            
		public const string Property_ExpiresOn = "ExpiresOn";		            
		public const string Property_EstimateDate = "EstimateDate";		            
		public const string Property_ShowServicePlan = "ShowServicePlan";		            
		public const string Property_ServicePlanRate = "ServicePlanRate";		            
		public const string Property_ShowService = "ShowService";		            
		public const string Property_ServicePlanAmount = "ServicePlanAmount";		            
		public const string Property_ServiceTaxAmount = "ServiceTaxAmount";		            
		public const string Property_ServiceTotalAmount = "ServiceTotalAmount";		            
		public const string Property_ServicePlanType = "ServicePlanType";		            
		public const string Property_ContractTerm = "ContractTerm";		            
		public const string Property_DefaultMaterialMarkupRate = "DefaultMaterialMarkupRate";		            
		public const string Property_ActivationFee = "ActivationFee";		            
		public const string Property_EstimatorSignature = "EstimatorSignature";		            
		public const string Property_ParentEstimatorRef = "ParentEstimatorRef";		            
		public const string Property_IsApproved = "IsApproved";		            
		public const string Property_OneTimeServiceTaxAmount = "OneTimeServiceTaxAmount";		            
		public const string Property_OneTimeServiceTotalAmount = "OneTimeServiceTotalAmount";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _EstimatorId;	            
		private Guid _CustomerId;	            
		private String _BillingAddress;	            
		private String _ProjectAddress;	            
		private String _Status;	            
		private Nullable<DateTime> _StartDate;	            
		private Nullable<DateTime> _CompletionDate;	            
		private String _EmailAddress;	            
		private String _Description;	            
		private Nullable<Double> _TaxPercnetage;	            
		private Nullable<Double> _TaxAmount;	            
		private Nullable<Double> _TotalPrice;	            
		private Nullable<Double> _TotalCost;	            
		private Nullable<Double> _PoriftPercentage;	            
		private Nullable<Double> _TotalProfitAmount;	            
		private Nullable<Double> _OverheadCostPercentage;	            
		private Nullable<Double> _TotalOverheadCostAmount;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Double> _DefaultOverheadRate;	            
		private Nullable<Double> _DefaultProfitRate;	            
		private String _CoverLetter;	            
		private String _CoverLetterFile;	            
		private String _PaymentTerm;	            
		private String _ExpiresOn;	            
		private Nullable<DateTime> _EstimateDate;	            
		private Nullable<Boolean> _ShowServicePlan;	            
		private Nullable<Double> _ServicePlanRate;	            
		private Nullable<Boolean> _ShowService;	            
		private Nullable<Double> _ServicePlanAmount;	            
		private Nullable<Double> _ServiceTaxAmount;	            
		private Nullable<Double> _ServiceTotalAmount;	            
		private String _ServicePlanType;	            
		private String _ContractTerm;	            
		private Nullable<Double> _DefaultMaterialMarkupRate;	            
		private Nullable<Double> _ActivationFee;	            
		private String _EstimatorSignature;	            
		private String _ParentEstimatorRef;	            
		private Boolean _IsApproved;	            
		private Nullable<Double> _OneTimeServiceTaxAmount;	            
		private Nullable<Double> _OneTimeServiceTotalAmount;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int32 Id
		{	
			get{ return _Id; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Id, value, _Id);
				if (PropertyChanging(args))
				{
					_Id = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CompanyId
		{	
			get{ return _CompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyId, value, _CompanyId);
				if (PropertyChanging(args))
				{
					_CompanyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EstimatorId
		{	
			get{ return _EstimatorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatorId, value, _EstimatorId);
				if (PropertyChanging(args))
				{
					_EstimatorId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingAddress
		{	
			get{ return _BillingAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingAddress, value, _BillingAddress);
				if (PropertyChanging(args))
				{
					_BillingAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProjectAddress
		{	
			get{ return _ProjectAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProjectAddress, value, _ProjectAddress);
				if (PropertyChanging(args))
				{
					_ProjectAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> StartDate
		{	
			get{ return _StartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDate, value, _StartDate);
				if (PropertyChanging(args))
				{
					_StartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CompletionDate
		{	
			get{ return _CompletionDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompletionDate, value, _CompletionDate);
				if (PropertyChanging(args))
				{
					_CompletionDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailAddress
		{	
			get{ return _EmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailAddress, value, _EmailAddress);
				if (PropertyChanging(args))
				{
					_EmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxPercnetage
		{	
			get{ return _TaxPercnetage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxPercnetage, value, _TaxPercnetage);
				if (PropertyChanging(args))
				{
					_TaxPercnetage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxAmount
		{	
			get{ return _TaxAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxAmount, value, _TaxAmount);
				if (PropertyChanging(args))
				{
					_TaxAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalPrice
		{	
			get{ return _TotalPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalPrice, value, _TotalPrice);
				if (PropertyChanging(args))
				{
					_TotalPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalCost
		{	
			get{ return _TotalCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalCost, value, _TotalCost);
				if (PropertyChanging(args))
				{
					_TotalCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PoriftPercentage
		{	
			get{ return _PoriftPercentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PoriftPercentage, value, _PoriftPercentage);
				if (PropertyChanging(args))
				{
					_PoriftPercentage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalProfitAmount
		{	
			get{ return _TotalProfitAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalProfitAmount, value, _TotalProfitAmount);
				if (PropertyChanging(args))
				{
					_TotalProfitAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> OverheadCostPercentage
		{	
			get{ return _OverheadCostPercentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OverheadCostPercentage, value, _OverheadCostPercentage);
				if (PropertyChanging(args))
				{
					_OverheadCostPercentage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalOverheadCostAmount
		{	
			get{ return _TotalOverheadCostAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalOverheadCostAmount, value, _TotalOverheadCostAmount);
				if (PropertyChanging(args))
				{
					_TotalOverheadCostAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DefaultOverheadRate
		{	
			get{ return _DefaultOverheadRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DefaultOverheadRate, value, _DefaultOverheadRate);
				if (PropertyChanging(args))
				{
					_DefaultOverheadRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DefaultProfitRate
		{	
			get{ return _DefaultProfitRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DefaultProfitRate, value, _DefaultProfitRate);
				if (PropertyChanging(args))
				{
					_DefaultProfitRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoverLetter
		{	
			get{ return _CoverLetter; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoverLetter, value, _CoverLetter);
				if (PropertyChanging(args))
				{
					_CoverLetter = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoverLetterFile
		{	
			get{ return _CoverLetterFile; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoverLetterFile, value, _CoverLetterFile);
				if (PropertyChanging(args))
				{
					_CoverLetterFile = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentTerm
		{	
			get{ return _PaymentTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentTerm, value, _PaymentTerm);
				if (PropertyChanging(args))
				{
					_PaymentTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExpiresOn
		{	
			get{ return _ExpiresOn; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpiresOn, value, _ExpiresOn);
				if (PropertyChanging(args))
				{
					_ExpiresOn = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EstimateDate
		{	
			get{ return _EstimateDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimateDate, value, _EstimateDate);
				if (PropertyChanging(args))
				{
					_EstimateDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> ShowServicePlan
		{	
			get{ return _ShowServicePlan; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowServicePlan, value, _ShowServicePlan);
				if (PropertyChanging(args))
				{
					_ShowServicePlan = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ServicePlanRate
		{	
			get{ return _ServicePlanRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServicePlanRate, value, _ServicePlanRate);
				if (PropertyChanging(args))
				{
					_ServicePlanRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> ShowService
		{	
			get{ return _ShowService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowService, value, _ShowService);
				if (PropertyChanging(args))
				{
					_ShowService = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ServicePlanAmount
		{	
			get{ return _ServicePlanAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServicePlanAmount, value, _ServicePlanAmount);
				if (PropertyChanging(args))
				{
					_ServicePlanAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ServiceTaxAmount
		{	
			get{ return _ServiceTaxAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceTaxAmount, value, _ServiceTaxAmount);
				if (PropertyChanging(args))
				{
					_ServiceTaxAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ServiceTotalAmount
		{	
			get{ return _ServiceTotalAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceTotalAmount, value, _ServiceTotalAmount);
				if (PropertyChanging(args))
				{
					_ServiceTotalAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ServicePlanType
		{	
			get{ return _ServicePlanType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServicePlanType, value, _ServicePlanType);
				if (PropertyChanging(args))
				{
					_ServicePlanType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractTerm
		{	
			get{ return _ContractTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractTerm, value, _ContractTerm);
				if (PropertyChanging(args))
				{
					_ContractTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DefaultMaterialMarkupRate
		{	
			get{ return _DefaultMaterialMarkupRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DefaultMaterialMarkupRate, value, _DefaultMaterialMarkupRate);
				if (PropertyChanging(args))
				{
					_DefaultMaterialMarkupRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ActivationFee
		{	
			get{ return _ActivationFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivationFee, value, _ActivationFee);
				if (PropertyChanging(args))
				{
					_ActivationFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EstimatorSignature
		{	
			get{ return _EstimatorSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatorSignature, value, _EstimatorSignature);
				if (PropertyChanging(args))
				{
					_EstimatorSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ParentEstimatorRef
		{	
			get{ return _ParentEstimatorRef; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ParentEstimatorRef, value, _ParentEstimatorRef);
				if (PropertyChanging(args))
				{
					_ParentEstimatorRef = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsApproved
		{	
			get{ return _IsApproved; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsApproved, value, _IsApproved);
				if (PropertyChanging(args))
				{
					_IsApproved = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> OneTimeServiceTaxAmount
		{	
			get{ return _OneTimeServiceTaxAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OneTimeServiceTaxAmount, value, _OneTimeServiceTaxAmount);
				if (PropertyChanging(args))
				{
					_OneTimeServiceTaxAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> OneTimeServiceTotalAmount
		{	
			get{ return _OneTimeServiceTotalAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OneTimeServiceTotalAmount, value, _OneTimeServiceTotalAmount);
				if (PropertyChanging(args))
				{
					_OneTimeServiceTotalAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EstimatorBase Clone()
		{
			EstimatorBase newObj = new  EstimatorBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EstimatorId = this.EstimatorId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.ProjectAddress = this.ProjectAddress;						
			newObj.Status = this.Status;						
			newObj.StartDate = this.StartDate;						
			newObj.CompletionDate = this.CompletionDate;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.Description = this.Description;						
			newObj.TaxPercnetage = this.TaxPercnetage;						
			newObj.TaxAmount = this.TaxAmount;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.TotalCost = this.TotalCost;						
			newObj.PoriftPercentage = this.PoriftPercentage;						
			newObj.TotalProfitAmount = this.TotalProfitAmount;						
			newObj.OverheadCostPercentage = this.OverheadCostPercentage;						
			newObj.TotalOverheadCostAmount = this.TotalOverheadCostAmount;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.DefaultOverheadRate = this.DefaultOverheadRate;						
			newObj.DefaultProfitRate = this.DefaultProfitRate;						
			newObj.CoverLetter = this.CoverLetter;						
			newObj.CoverLetterFile = this.CoverLetterFile;						
			newObj.PaymentTerm = this.PaymentTerm;						
			newObj.ExpiresOn = this.ExpiresOn;						
			newObj.EstimateDate = this.EstimateDate;						
			newObj.ShowServicePlan = this.ShowServicePlan;						
			newObj.ServicePlanRate = this.ServicePlanRate;						
			newObj.ShowService = this.ShowService;						
			newObj.ServicePlanAmount = this.ServicePlanAmount;						
			newObj.ServiceTaxAmount = this.ServiceTaxAmount;						
			newObj.ServiceTotalAmount = this.ServiceTotalAmount;						
			newObj.ServicePlanType = this.ServicePlanType;						
			newObj.ContractTerm = this.ContractTerm;						
			newObj.DefaultMaterialMarkupRate = this.DefaultMaterialMarkupRate;						
			newObj.ActivationFee = this.ActivationFee;						
			newObj.EstimatorSignature = this.EstimatorSignature;						
			newObj.ParentEstimatorRef = this.ParentEstimatorRef;						
			newObj.IsApproved = this.IsApproved;						
			newObj.OneTimeServiceTaxAmount = this.OneTimeServiceTaxAmount;						
			newObj.OneTimeServiceTotalAmount = this.OneTimeServiceTotalAmount;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EstimatorBase.Property_Id, Id);				
			info.AddValue(EstimatorBase.Property_CompanyId, CompanyId);				
			info.AddValue(EstimatorBase.Property_EstimatorId, EstimatorId);				
			info.AddValue(EstimatorBase.Property_CustomerId, CustomerId);				
			info.AddValue(EstimatorBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(EstimatorBase.Property_ProjectAddress, ProjectAddress);				
			info.AddValue(EstimatorBase.Property_Status, Status);				
			info.AddValue(EstimatorBase.Property_StartDate, StartDate);				
			info.AddValue(EstimatorBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(EstimatorBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(EstimatorBase.Property_Description, Description);				
			info.AddValue(EstimatorBase.Property_TaxPercnetage, TaxPercnetage);				
			info.AddValue(EstimatorBase.Property_TaxAmount, TaxAmount);				
			info.AddValue(EstimatorBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(EstimatorBase.Property_TotalCost, TotalCost);				
			info.AddValue(EstimatorBase.Property_PoriftPercentage, PoriftPercentage);				
			info.AddValue(EstimatorBase.Property_TotalProfitAmount, TotalProfitAmount);				
			info.AddValue(EstimatorBase.Property_OverheadCostPercentage, OverheadCostPercentage);				
			info.AddValue(EstimatorBase.Property_TotalOverheadCostAmount, TotalOverheadCostAmount);				
			info.AddValue(EstimatorBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EstimatorBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EstimatorBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EstimatorBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(EstimatorBase.Property_DefaultOverheadRate, DefaultOverheadRate);				
			info.AddValue(EstimatorBase.Property_DefaultProfitRate, DefaultProfitRate);				
			info.AddValue(EstimatorBase.Property_CoverLetter, CoverLetter);				
			info.AddValue(EstimatorBase.Property_CoverLetterFile, CoverLetterFile);				
			info.AddValue(EstimatorBase.Property_PaymentTerm, PaymentTerm);				
			info.AddValue(EstimatorBase.Property_ExpiresOn, ExpiresOn);				
			info.AddValue(EstimatorBase.Property_EstimateDate, EstimateDate);				
			info.AddValue(EstimatorBase.Property_ShowServicePlan, ShowServicePlan);				
			info.AddValue(EstimatorBase.Property_ServicePlanRate, ServicePlanRate);				
			info.AddValue(EstimatorBase.Property_ShowService, ShowService);				
			info.AddValue(EstimatorBase.Property_ServicePlanAmount, ServicePlanAmount);				
			info.AddValue(EstimatorBase.Property_ServiceTaxAmount, ServiceTaxAmount);				
			info.AddValue(EstimatorBase.Property_ServiceTotalAmount, ServiceTotalAmount);				
			info.AddValue(EstimatorBase.Property_ServicePlanType, ServicePlanType);				
			info.AddValue(EstimatorBase.Property_ContractTerm, ContractTerm);				
			info.AddValue(EstimatorBase.Property_DefaultMaterialMarkupRate, DefaultMaterialMarkupRate);				
			info.AddValue(EstimatorBase.Property_ActivationFee, ActivationFee);				
			info.AddValue(EstimatorBase.Property_EstimatorSignature, EstimatorSignature);				
			info.AddValue(EstimatorBase.Property_ParentEstimatorRef, ParentEstimatorRef);				
			info.AddValue(EstimatorBase.Property_IsApproved, IsApproved);				
			info.AddValue(EstimatorBase.Property_OneTimeServiceTaxAmount, OneTimeServiceTaxAmount);				
			info.AddValue(EstimatorBase.Property_OneTimeServiceTotalAmount, OneTimeServiceTotalAmount);				
		}
		#endregion

		
	}
}
