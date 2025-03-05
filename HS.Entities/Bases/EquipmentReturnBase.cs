using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentReturnBase", Namespace = "http://www.piistech.com//entities")]
	public class EquipmentReturnBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			ReturnId = 2,
			CustomerId = 3,
			TechnicianId = 4,
			EquipmentId = 5,
			Quantity = 6,
			InvoiceNo = 7,
			PurchaseDate = 8,
			WanrantyAvailable = 9,
			Description = 10,
			Status = 11,
			LastUpdatedBy = 12,
			LastUpdatedDate = 13,
			Reason = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_ReturnId = "ReturnId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_InvoiceNo = "InvoiceNo";		            
		public const string Property_PurchaseDate = "PurchaseDate";		            
		public const string Property_WanrantyAvailable = "WanrantyAvailable";		            
		public const string Property_Description = "Description";		            
		public const string Property_Status = "Status";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Reason = "Reason";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _ReturnId;	            
		private Guid _CustomerId;	            
		private Guid _TechnicianId;	            
		private Guid _EquipmentId;	            
		private Int32 _Quantity;	            
		private String _InvoiceNo;	            
		private Nullable<DateTime> _PurchaseDate;	            
		private Boolean _WanrantyAvailable;	            
		private String _Description;	            
		private String _Status;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _Reason;	            
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
		public Guid ReturnId
		{	
			get{ return _ReturnId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReturnId, value, _ReturnId);
				if (PropertyChanging(args))
				{
					_ReturnId = value;
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
		public Guid EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Quantity
		{	
			get{ return _Quantity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Quantity, value, _Quantity);
				if (PropertyChanging(args))
				{
					_Quantity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceNo
		{	
			get{ return _InvoiceNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceNo, value, _InvoiceNo);
				if (PropertyChanging(args))
				{
					_InvoiceNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PurchaseDate
		{	
			get{ return _PurchaseDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PurchaseDate, value, _PurchaseDate);
				if (PropertyChanging(args))
				{
					_PurchaseDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean WanrantyAvailable
		{	
			get{ return _WanrantyAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WanrantyAvailable, value, _WanrantyAvailable);
				if (PropertyChanging(args))
				{
					_WanrantyAvailable = value;
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
		public String Reason
		{	
			get{ return _Reason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reason, value, _Reason);
				if (PropertyChanging(args))
				{
					_Reason = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EquipmentReturnBase Clone()
		{
			EquipmentReturnBase newObj = new  EquipmentReturnBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.ReturnId = this.ReturnId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Quantity = this.Quantity;						
			newObj.InvoiceNo = this.InvoiceNo;						
			newObj.PurchaseDate = this.PurchaseDate;						
			newObj.WanrantyAvailable = this.WanrantyAvailable;						
			newObj.Description = this.Description;						
			newObj.Status = this.Status;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Reason = this.Reason;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentReturnBase.Property_Id, Id);				
			info.AddValue(EquipmentReturnBase.Property_CompanyId, CompanyId);				
			info.AddValue(EquipmentReturnBase.Property_ReturnId, ReturnId);				
			info.AddValue(EquipmentReturnBase.Property_CustomerId, CustomerId);				
			info.AddValue(EquipmentReturnBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(EquipmentReturnBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentReturnBase.Property_Quantity, Quantity);				
			info.AddValue(EquipmentReturnBase.Property_InvoiceNo, InvoiceNo);				
			info.AddValue(EquipmentReturnBase.Property_PurchaseDate, PurchaseDate);				
			info.AddValue(EquipmentReturnBase.Property_WanrantyAvailable, WanrantyAvailable);				
			info.AddValue(EquipmentReturnBase.Property_Description, Description);				
			info.AddValue(EquipmentReturnBase.Property_Status, Status);				
			info.AddValue(EquipmentReturnBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EquipmentReturnBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(EquipmentReturnBase.Property_Reason, Reason);				
		}
		#endregion

		
	}
}
