using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "OpportunityBase", Namespace = "http://www.hims-tech.com//entities")]
	public class OpportunityBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			OpportunityId = 1,
			CustomerId = 2,
			OpportunityName = 3,
			Type = 4,
			LeadSource = 5,
			Revenue = 6,
			ProjectedGP = 7,
			Points = 8,
			TotalProjectedGP = 9,
			CloseDate = 10,
			Status = 11,
			Probability = 12,
			DealReason = 13,
			IsForecast = 14,
			DeliveryDays = 15,
			Competitors = 16,
			CampaignSource = 17,
			AccountOwner = 18,
			CreatedBy = 19,
			CreatedDate = 20,
			LastUpdatedDate = 21,
			Market = 22,
			Used = 23,
			YearModel = 24,
			Make = 25,
			Model = 26,
			Capacity = 27,
			Note = 28,
			AccessGivenTo = 29,
			VehicleCondition = 30,
			CloseDateSetDate = 31
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_OpportunityId = "OpportunityId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_OpportunityName = "OpportunityName";		            
		public const string Property_Type = "Type";		            
		public const string Property_LeadSource = "LeadSource";		            
		public const string Property_Revenue = "Revenue";		            
		public const string Property_ProjectedGP = "ProjectedGP";		            
		public const string Property_Points = "Points";		            
		public const string Property_TotalProjectedGP = "TotalProjectedGP";		            
		public const string Property_CloseDate = "CloseDate";		            
		public const string Property_Status = "Status";		            
		public const string Property_Probability = "Probability";		            
		public const string Property_DealReason = "DealReason";		            
		public const string Property_IsForecast = "IsForecast";		            
		public const string Property_DeliveryDays = "DeliveryDays";		            
		public const string Property_Competitors = "Competitors";		            
		public const string Property_CampaignSource = "CampaignSource";		            
		public const string Property_AccountOwner = "AccountOwner";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Market = "Market";		            
		public const string Property_Used = "Used";		            
		public const string Property_YearModel = "YearModel";		            
		public const string Property_Make = "Make";		            
		public const string Property_Model = "Model";		            
		public const string Property_Capacity = "Capacity";		            
		public const string Property_Note = "Note";		            
		public const string Property_AccessGivenTo = "AccessGivenTo";		            
		public const string Property_VehicleCondition = "VehicleCondition";		            
		public const string Property_CloseDateSetDate = "CloseDateSetDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _OpportunityId;	            
		private Guid _CustomerId;	            
		private String _OpportunityName;	            
		private String _Type;	            
		private String _LeadSource;	            
		private String _Revenue;	            
		private String _ProjectedGP;	            
		private String _Points;	            
		private String _TotalProjectedGP;	            
		private Nullable<DateTime> _CloseDate;	            
		private String _Status;	            
		private Int32 _Probability;	            
		private String _DealReason;	            
		private Nullable<Boolean> _IsForecast;	            
		private String _DeliveryDays;	            
		private String _Competitors;	            
		private String _CampaignSource;	            
		private Guid _AccountOwner;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private DateTime _LastUpdatedDate;	            
		private String _Market;	            
		private String _Used;	            
		private String _YearModel;	            
		private String _Make;	            
		private String _Model;	            
		private Nullable<Int32> _Capacity;	            
		private String _Note;	            
		private Guid _AccessGivenTo;	            
		private String _VehicleCondition;	            
		private Nullable<DateTime> _CloseDateSetDate;	            
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
		public Guid OpportunityId
		{	
			get{ return _OpportunityId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OpportunityId, value, _OpportunityId);
				if (PropertyChanging(args))
				{
					_OpportunityId = value;
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
		public String OpportunityName
		{	
			get{ return _OpportunityName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OpportunityName, value, _OpportunityName);
				if (PropertyChanging(args))
				{
					_OpportunityName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeadSource
		{	
			get{ return _LeadSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadSource, value, _LeadSource);
				if (PropertyChanging(args))
				{
					_LeadSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Revenue
		{	
			get{ return _Revenue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Revenue, value, _Revenue);
				if (PropertyChanging(args))
				{
					_Revenue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProjectedGP
		{	
			get{ return _ProjectedGP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProjectedGP, value, _ProjectedGP);
				if (PropertyChanging(args))
				{
					_ProjectedGP = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Points
		{	
			get{ return _Points; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Points, value, _Points);
				if (PropertyChanging(args))
				{
					_Points = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TotalProjectedGP
		{	
			get{ return _TotalProjectedGP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalProjectedGP, value, _TotalProjectedGP);
				if (PropertyChanging(args))
				{
					_TotalProjectedGP = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CloseDate
		{	
			get{ return _CloseDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CloseDate, value, _CloseDate);
				if (PropertyChanging(args))
				{
					_CloseDate = value;
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
		public Int32 Probability
		{	
			get{ return _Probability; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Probability, value, _Probability);
				if (PropertyChanging(args))
				{
					_Probability = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DealReason
		{	
			get{ return _DealReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DealReason, value, _DealReason);
				if (PropertyChanging(args))
				{
					_DealReason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsForecast
		{	
			get{ return _IsForecast; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsForecast, value, _IsForecast);
				if (PropertyChanging(args))
				{
					_IsForecast = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DeliveryDays
		{	
			get{ return _DeliveryDays; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DeliveryDays, value, _DeliveryDays);
				if (PropertyChanging(args))
				{
					_DeliveryDays = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Competitors
		{	
			get{ return _Competitors; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Competitors, value, _Competitors);
				if (PropertyChanging(args))
				{
					_Competitors = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CampaignSource
		{	
			get{ return _CampaignSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CampaignSource, value, _CampaignSource);
				if (PropertyChanging(args))
				{
					_CampaignSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AccountOwner
		{	
			get{ return _AccountOwner; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountOwner, value, _AccountOwner);
				if (PropertyChanging(args))
				{
					_AccountOwner = value;
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
		public String Market
		{	
			get{ return _Market; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Market, value, _Market);
				if (PropertyChanging(args))
				{
					_Market = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Used
		{	
			get{ return _Used; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Used, value, _Used);
				if (PropertyChanging(args))
				{
					_Used = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String YearModel
		{	
			get{ return _YearModel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_YearModel, value, _YearModel);
				if (PropertyChanging(args))
				{
					_YearModel = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Make
		{	
			get{ return _Make; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Make, value, _Make);
				if (PropertyChanging(args))
				{
					_Make = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Model
		{	
			get{ return _Model; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Model, value, _Model);
				if (PropertyChanging(args))
				{
					_Model = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Capacity
		{	
			get{ return _Capacity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Capacity, value, _Capacity);
				if (PropertyChanging(args))
				{
					_Capacity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AccessGivenTo
		{	
			get{ return _AccessGivenTo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccessGivenTo, value, _AccessGivenTo);
				if (PropertyChanging(args))
				{
					_AccessGivenTo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String VehicleCondition
		{	
			get{ return _VehicleCondition; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VehicleCondition, value, _VehicleCondition);
				if (PropertyChanging(args))
				{
					_VehicleCondition = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CloseDateSetDate
		{	
			get{ return _CloseDateSetDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CloseDateSetDate, value, _CloseDateSetDate);
				if (PropertyChanging(args))
				{
					_CloseDateSetDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  OpportunityBase Clone()
		{
			OpportunityBase newObj = new  OpportunityBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.OpportunityId = this.OpportunityId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.OpportunityName = this.OpportunityName;						
			newObj.Type = this.Type;						
			newObj.LeadSource = this.LeadSource;						
			newObj.Revenue = this.Revenue;						
			newObj.ProjectedGP = this.ProjectedGP;						
			newObj.Points = this.Points;						
			newObj.TotalProjectedGP = this.TotalProjectedGP;						
			newObj.CloseDate = this.CloseDate;						
			newObj.Status = this.Status;						
			newObj.Probability = this.Probability;						
			newObj.DealReason = this.DealReason;						
			newObj.IsForecast = this.IsForecast;						
			newObj.DeliveryDays = this.DeliveryDays;						
			newObj.Competitors = this.Competitors;						
			newObj.CampaignSource = this.CampaignSource;						
			newObj.AccountOwner = this.AccountOwner;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Market = this.Market;						
			newObj.Used = this.Used;						
			newObj.YearModel = this.YearModel;						
			newObj.Make = this.Make;						
			newObj.Model = this.Model;						
			newObj.Capacity = this.Capacity;						
			newObj.Note = this.Note;						
			newObj.AccessGivenTo = this.AccessGivenTo;						
			newObj.VehicleCondition = this.VehicleCondition;						
			newObj.CloseDateSetDate = this.CloseDateSetDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(OpportunityBase.Property_Id, Id);				
			info.AddValue(OpportunityBase.Property_OpportunityId, OpportunityId);				
			info.AddValue(OpportunityBase.Property_CustomerId, CustomerId);				
			info.AddValue(OpportunityBase.Property_OpportunityName, OpportunityName);				
			info.AddValue(OpportunityBase.Property_Type, Type);				
			info.AddValue(OpportunityBase.Property_LeadSource, LeadSource);				
			info.AddValue(OpportunityBase.Property_Revenue, Revenue);				
			info.AddValue(OpportunityBase.Property_ProjectedGP, ProjectedGP);				
			info.AddValue(OpportunityBase.Property_Points, Points);				
			info.AddValue(OpportunityBase.Property_TotalProjectedGP, TotalProjectedGP);				
			info.AddValue(OpportunityBase.Property_CloseDate, CloseDate);				
			info.AddValue(OpportunityBase.Property_Status, Status);				
			info.AddValue(OpportunityBase.Property_Probability, Probability);				
			info.AddValue(OpportunityBase.Property_DealReason, DealReason);				
			info.AddValue(OpportunityBase.Property_IsForecast, IsForecast);				
			info.AddValue(OpportunityBase.Property_DeliveryDays, DeliveryDays);				
			info.AddValue(OpportunityBase.Property_Competitors, Competitors);				
			info.AddValue(OpportunityBase.Property_CampaignSource, CampaignSource);				
			info.AddValue(OpportunityBase.Property_AccountOwner, AccountOwner);				
			info.AddValue(OpportunityBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(OpportunityBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(OpportunityBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(OpportunityBase.Property_Market, Market);				
			info.AddValue(OpportunityBase.Property_Used, Used);				
			info.AddValue(OpportunityBase.Property_YearModel, YearModel);				
			info.AddValue(OpportunityBase.Property_Make, Make);				
			info.AddValue(OpportunityBase.Property_Model, Model);				
			info.AddValue(OpportunityBase.Property_Capacity, Capacity);				
			info.AddValue(OpportunityBase.Property_Note, Note);				
			info.AddValue(OpportunityBase.Property_AccessGivenTo, AccessGivenTo);				
			info.AddValue(OpportunityBase.Property_VehicleCondition, VehicleCondition);				
			info.AddValue(OpportunityBase.Property_CloseDateSetDate, CloseDateSetDate);				
		}
		#endregion

		
	}
}
