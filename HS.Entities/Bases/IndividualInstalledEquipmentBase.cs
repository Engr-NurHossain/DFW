using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "IndividualInstalledEquipmentBase", Namespace = "http://www.hims-tech.com//entities")]
	public class IndividualInstalledEquipmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AppointmentEquipmentId = 1,
			Category = 2,
			Manufacturer = 3,
			Description = 4,
			TicketType = 5,
			EmpUser = 6,
			TicketId = 7,
			RepliesCount = 8,
			AttachmentsCount = 9,
			CusIdInt = 10,
			CustomerName = 11,
			CompletionDate = 12,
			SKU = 13,
			TotalPoint = 14,
			IsClosed = 15,
			CompanyCost = 16,
			CustomerCost = 17,
			Quantity = 18,
			InstalledEquipment = 19,
			Qty = 20,
			Status = 21,
			CreatedBy = 22,
			CreatedDate = 23,
			InstalledByUid = 24,
			EquipmentId = 25
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AppointmentEquipmentId = "AppointmentEquipmentId";		            
		public const string Property_Category = "Category";		            
		public const string Property_Manufacturer = "Manufacturer";		            
		public const string Property_Description = "Description";		            
		public const string Property_TicketType = "TicketType";		            
		public const string Property_EmpUser = "EmpUser";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_RepliesCount = "RepliesCount";		            
		public const string Property_AttachmentsCount = "AttachmentsCount";		            
		public const string Property_CusIdInt = "CusIdInt";		            
		public const string Property_CustomerName = "CustomerName";		            
		public const string Property_CompletionDate = "CompletionDate";		            
		public const string Property_SKU = "SKU";		            
		public const string Property_TotalPoint = "TotalPoint";		            
		public const string Property_IsClosed = "IsClosed";		            
		public const string Property_CompanyCost = "CompanyCost";		            
		public const string Property_CustomerCost = "CustomerCost";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_InstalledEquipment = "InstalledEquipment";		            
		public const string Property_Qty = "Qty";		            
		public const string Property_Status = "Status";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_InstalledByUid = "InstalledByUid";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _AppointmentEquipmentId;	            
		private String _Category;	            
		private String _Manufacturer;	            
		private String _Description;	            
		private String _TicketType;	            
		private String _EmpUser;	            
		private Int32 _TicketId;	            
		private Nullable<Int32> _RepliesCount;	            
		private Nullable<Int32> _AttachmentsCount;	            
		private Int32 _CusIdInt;	            
		private String _CustomerName;	            
		private DateTime _CompletionDate;	            
		private String _SKU;	            
		private Double _TotalPoint;	            
		private Boolean _IsClosed;	            
		private Nullable<Double> _CompanyCost;	            
		private Nullable<Double> _CustomerCost;	            
		private Int32 _Quantity;	            
		private Int32 _InstalledEquipment;	            
		private Int32 _Qty;	            
		private String _Status;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _InstalledByUid;	            
		private Guid _EquipmentId;	            
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
		public Int32 AppointmentEquipmentId
		{	
			get{ return _AppointmentEquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentEquipmentId, value, _AppointmentEquipmentId);
				if (PropertyChanging(args))
				{
					_AppointmentEquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Category
		{	
			get{ return _Category; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Category, value, _Category);
				if (PropertyChanging(args))
				{
					_Category = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Manufacturer
		{	
			get{ return _Manufacturer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Manufacturer, value, _Manufacturer);
				if (PropertyChanging(args))
				{
					_Manufacturer = value;
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
		public String TicketType
		{	
			get{ return _TicketType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketType, value, _TicketType);
				if (PropertyChanging(args))
				{
					_TicketType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmpUser
		{	
			get{ return _EmpUser; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmpUser, value, _EmpUser);
				if (PropertyChanging(args))
				{
					_EmpUser = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 TicketId
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

		[DataMember]
		public Nullable<Int32> RepliesCount
		{	
			get{ return _RepliesCount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepliesCount, value, _RepliesCount);
				if (PropertyChanging(args))
				{
					_RepliesCount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AttachmentsCount
		{	
			get{ return _AttachmentsCount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AttachmentsCount, value, _AttachmentsCount);
				if (PropertyChanging(args))
				{
					_AttachmentsCount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 CusIdInt
		{	
			get{ return _CusIdInt; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CusIdInt, value, _CusIdInt);
				if (PropertyChanging(args))
				{
					_CusIdInt = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerName
		{	
			get{ return _CustomerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerName, value, _CustomerName);
				if (PropertyChanging(args))
				{
					_CustomerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CompletionDate
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
		public String SKU
		{	
			get{ return _SKU; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SKU, value, _SKU);
				if (PropertyChanging(args))
				{
					_SKU = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double TotalPoint
		{	
			get{ return _TotalPoint; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalPoint, value, _TotalPoint);
				if (PropertyChanging(args))
				{
					_TotalPoint = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsClosed
		{	
			get{ return _IsClosed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsClosed, value, _IsClosed);
				if (PropertyChanging(args))
				{
					_IsClosed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CompanyCost
		{	
			get{ return _CompanyCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyCost, value, _CompanyCost);
				if (PropertyChanging(args))
				{
					_CompanyCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CustomerCost
		{	
			get{ return _CustomerCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerCost, value, _CustomerCost);
				if (PropertyChanging(args))
				{
					_CustomerCost = value;
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
		public Int32 InstalledEquipment
		{	
			get{ return _InstalledEquipment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstalledEquipment, value, _InstalledEquipment);
				if (PropertyChanging(args))
				{
					_InstalledEquipment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Qty
		{	
			get{ return _Qty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Qty, value, _Qty);
				if (PropertyChanging(args))
				{
					_Qty = value;
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
		public Guid InstalledByUid
		{	
			get{ return _InstalledByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstalledByUid, value, _InstalledByUid);
				if (PropertyChanging(args))
				{
					_InstalledByUid = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  IndividualInstalledEquipmentBase Clone()
		{
			IndividualInstalledEquipmentBase newObj = new  IndividualInstalledEquipmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AppointmentEquipmentId = this.AppointmentEquipmentId;						
			newObj.Category = this.Category;						
			newObj.Manufacturer = this.Manufacturer;						
			newObj.Description = this.Description;						
			newObj.TicketType = this.TicketType;						
			newObj.EmpUser = this.EmpUser;						
			newObj.TicketId = this.TicketId;						
			newObj.RepliesCount = this.RepliesCount;						
			newObj.AttachmentsCount = this.AttachmentsCount;						
			newObj.CusIdInt = this.CusIdInt;						
			newObj.CustomerName = this.CustomerName;						
			newObj.CompletionDate = this.CompletionDate;						
			newObj.SKU = this.SKU;						
			newObj.TotalPoint = this.TotalPoint;						
			newObj.IsClosed = this.IsClosed;						
			newObj.CompanyCost = this.CompanyCost;						
			newObj.CustomerCost = this.CustomerCost;						
			newObj.Quantity = this.Quantity;						
			newObj.InstalledEquipment = this.InstalledEquipment;						
			newObj.Qty = this.Qty;						
			newObj.Status = this.Status;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.InstalledByUid = this.InstalledByUid;						
			newObj.EquipmentId = this.EquipmentId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(IndividualInstalledEquipmentBase.Property_Id, Id);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_AppointmentEquipmentId, AppointmentEquipmentId);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_Category, Category);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_Manufacturer, Manufacturer);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_Description, Description);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_TicketType, TicketType);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_EmpUser, EmpUser);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_TicketId, TicketId);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_RepliesCount, RepliesCount);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_AttachmentsCount, AttachmentsCount);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CusIdInt, CusIdInt);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CustomerName, CustomerName);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_SKU, SKU);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_TotalPoint, TotalPoint);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_IsClosed, IsClosed);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CompanyCost, CompanyCost);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CustomerCost, CustomerCost);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_Quantity, Quantity);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_InstalledEquipment, InstalledEquipment);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_Qty, Qty);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_Status, Status);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_InstalledByUid, InstalledByUid);				
			info.AddValue(IndividualInstalledEquipmentBase.Property_EquipmentId, EquipmentId);				
		}
		#endregion

		
	}
}
