using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PurchaseOrderBranchBase", Namespace = "http://www.piistech.com//entities")]
	public class PurchaseOrderBranchBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			BranchId = 2,
			DemandOrderId = 3,
			Action = 4,
			Status = 5,
			Location = 6,
			IsReceived = 7,
			CreatedByUid = 8,
			LastUpdatedDate = 9,
			LastUpdatedByUid = 10,
			TechDemandOrderId = 11,
			Description = 12,
			CreatedDate = 13,
			IsBulkPO = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BranchId = "BranchId";		            
		public const string Property_DemandOrderId = "DemandOrderId";		            
		public const string Property_Action = "Action";		            
		public const string Property_Status = "Status";		            
		public const string Property_Location = "Location";		            
		public const string Property_IsReceived = "IsReceived";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_TechDemandOrderId = "TechDemandOrderId";		            
		public const string Property_Description = "Description";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsBulkPO = "IsBulkPO";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _BranchId;	            
		private String _DemandOrderId;	            
		private String _Action;	            
		private String _Status;	            
		private String _Location;	            
		private Boolean _IsReceived;	            
		private Guid _CreatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private String _TechDemandOrderId;	            
		private String _Description;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsBulkPO;	            
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
		public Int32 BranchId
		{	
			get{ return _BranchId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BranchId, value, _BranchId);
				if (PropertyChanging(args))
				{
					_BranchId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DemandOrderId
		{	
			get{ return _DemandOrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DemandOrderId, value, _DemandOrderId);
				if (PropertyChanging(args))
				{
					_DemandOrderId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Action
		{	
			get{ return _Action; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Action, value, _Action);
				if (PropertyChanging(args))
				{
					_Action = value;
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
		public String Location
		{	
			get{ return _Location; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Location, value, _Location);
				if (PropertyChanging(args))
				{
					_Location = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsReceived
		{	
			get{ return _IsReceived; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReceived, value, _IsReceived);
				if (PropertyChanging(args))
				{
					_IsReceived = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
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
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TechDemandOrderId
		{	
			get{ return _TechDemandOrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechDemandOrderId, value, _TechDemandOrderId);
				if (PropertyChanging(args))
				{
					_TechDemandOrderId = value;
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
		public Nullable<Boolean> IsBulkPO
		{	
			get{ return _IsBulkPO; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBulkPO, value, _IsBulkPO);
				if (PropertyChanging(args))
				{
					_IsBulkPO = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PurchaseOrderBranchBase Clone()
		{
			PurchaseOrderBranchBase newObj = new  PurchaseOrderBranchBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BranchId = this.BranchId;						
			newObj.DemandOrderId = this.DemandOrderId;						
			newObj.Action = this.Action;						
			newObj.Status = this.Status;						
			newObj.Location = this.Location;						
			newObj.IsReceived = this.IsReceived;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.TechDemandOrderId = this.TechDemandOrderId;						
			newObj.Description = this.Description;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsBulkPO = this.IsBulkPO;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PurchaseOrderBranchBase.Property_Id, Id);				
			info.AddValue(PurchaseOrderBranchBase.Property_CompanyId, CompanyId);				
			info.AddValue(PurchaseOrderBranchBase.Property_BranchId, BranchId);				
			info.AddValue(PurchaseOrderBranchBase.Property_DemandOrderId, DemandOrderId);				
			info.AddValue(PurchaseOrderBranchBase.Property_Action, Action);				
			info.AddValue(PurchaseOrderBranchBase.Property_Status, Status);				
			info.AddValue(PurchaseOrderBranchBase.Property_Location, Location);				
			info.AddValue(PurchaseOrderBranchBase.Property_IsReceived, IsReceived);				
			info.AddValue(PurchaseOrderBranchBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(PurchaseOrderBranchBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(PurchaseOrderBranchBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(PurchaseOrderBranchBase.Property_TechDemandOrderId, TechDemandOrderId);				
			info.AddValue(PurchaseOrderBranchBase.Property_Description, Description);				
			info.AddValue(PurchaseOrderBranchBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PurchaseOrderBranchBase.Property_IsBulkPO, IsBulkPO);				
		}
		#endregion

		
	}
}
