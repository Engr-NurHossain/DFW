using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PurchaseOrderTechBase", Namespace = "http://www.piistech.com//entities")]
	public class PurchaseOrderTechBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			TechnicianId = 2,
			DemandOrderId = 3,
			Status = 4,
			Location = 5,
			IsReceived = 6,
			CreatedByUid = 7,
			LastUpdatedDate = 8,
			LastUpdatedByUid = 9,
			Description = 10,
			CreatedDate = 11,
			IsBulkPO = 12,
			TicketId = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_DemandOrderId = "DemandOrderId";		            
		public const string Property_Status = "Status";		            
		public const string Property_Location = "Location";		            
		public const string Property_IsReceived = "IsReceived";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_Description = "Description";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsBulkPO = "IsBulkPO";		            
		public const string Property_TicketId = "TicketId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _TechnicianId;	            
		private String _DemandOrderId;	            
		private String _Status;	            
		private String _Location;	            
		private Boolean _IsReceived;	            
		private Guid _CreatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private String _Description;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsBulkPO;	            
		private Nullable<Int32> _TicketId;	            
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
		public Guid TechnicianId
		{	
			get{ return _TechnicianId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechnicianId, value, _TechnicianId);
				if (PropertyChanging(args))
				{
					_TechnicianId = value;
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

		[DataMember]
		public Nullable<Int32> TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PurchaseOrderTechBase Clone()
		{
			PurchaseOrderTechBase newObj = new  PurchaseOrderTechBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.DemandOrderId = this.DemandOrderId;						
			newObj.Status = this.Status;						
			newObj.Location = this.Location;						
			newObj.IsReceived = this.IsReceived;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.Description = this.Description;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsBulkPO = this.IsBulkPO;						
			newObj.TicketId = this.TicketId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PurchaseOrderTechBase.Property_Id, Id);				
			info.AddValue(PurchaseOrderTechBase.Property_CompanyId, CompanyId);				
			info.AddValue(PurchaseOrderTechBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(PurchaseOrderTechBase.Property_DemandOrderId, DemandOrderId);				
			info.AddValue(PurchaseOrderTechBase.Property_Status, Status);				
			info.AddValue(PurchaseOrderTechBase.Property_Location, Location);				
			info.AddValue(PurchaseOrderTechBase.Property_IsReceived, IsReceived);				
			info.AddValue(PurchaseOrderTechBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(PurchaseOrderTechBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(PurchaseOrderTechBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(PurchaseOrderTechBase.Property_Description, Description);				
			info.AddValue(PurchaseOrderTechBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PurchaseOrderTechBase.Property_IsBulkPO, IsBulkPO);				
			info.AddValue(PurchaseOrderTechBase.Property_TicketId, TicketId);				
		}
		#endregion

		
	}
}
