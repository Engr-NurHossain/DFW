using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAppointmentEquipmentBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerAppointmentEquipmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AppointmentId = 1,
			EquipmentId = 2,
			Quantity = 3,
			UnitPrice = 4,
			TotalPrice = 5,
			CreatedDate = 6,
			CreatedBy = 7,
			EquipName = 8,
			EquipDetail = 9,
			IsEquipmentRelease = 10,
			IsService = 11,
			CreatedByUid = 12,
			IsAgreementItem = 13,
			IsBaseItem = 14,
			IsBadInventory = 15,
			IsDefaultService = 16,
			IsCheckedEquipment = 17,
			IsTransfered = 18,
			QuantityLeftEquipment = 19,
			IsEquipmentExist = 20,
			OriginalUnitPrice = 21,
			IsInvoiceCreate = 22,
			ReferenceInvoiceId = 23,
			ReferenceInvDetailId = 24,
			IsBilling = 25,
			IsCopied = 26,
			IsNonCommissionable = 27,
			InstalledByUid = 28,
			IsBillingProcess = 29
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AppointmentId = "AppointmentId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_EquipName = "EquipName";		            
		public const string Property_EquipDetail = "EquipDetail";		            
		public const string Property_IsEquipmentRelease = "IsEquipmentRelease";		            
		public const string Property_IsService = "IsService";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_IsAgreementItem = "IsAgreementItem";		            
		public const string Property_IsBaseItem = "IsBaseItem";		            
		public const string Property_IsBadInventory = "IsBadInventory";		            
		public const string Property_IsDefaultService = "IsDefaultService";		            
		public const string Property_IsCheckedEquipment = "IsCheckedEquipment";		            
		public const string Property_IsTransfered = "IsTransfered";		            
		public const string Property_QuantityLeftEquipment = "QuantityLeftEquipment";		            
		public const string Property_IsEquipmentExist = "IsEquipmentExist";		            
		public const string Property_OriginalUnitPrice = "OriginalUnitPrice";		            
		public const string Property_IsInvoiceCreate = "IsInvoiceCreate";		            
		public const string Property_ReferenceInvoiceId = "ReferenceInvoiceId";		            
		public const string Property_ReferenceInvDetailId = "ReferenceInvDetailId";		            
		public const string Property_IsBilling = "IsBilling";		            
		public const string Property_IsCopied = "IsCopied";		            
		public const string Property_IsNonCommissionable = "IsNonCommissionable";		            
		public const string Property_InstalledByUid = "InstalledByUid";		            
		public const string Property_IsBillingProcess = "IsBillingProcess";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _AppointmentId;	            
		private Guid _EquipmentId;	            
		private Int32 _Quantity;	            
		private Double _UnitPrice;	            
		private Double _TotalPrice;	            
		private DateTime _CreatedDate;	            
		private String _CreatedBy;	            
		private String _EquipName;	            
		private String _EquipDetail;	            
		private Nullable<Boolean> _IsEquipmentRelease;	            
		private Nullable<Boolean> _IsService;	            
		private Guid _CreatedByUid;	            
		private Nullable<Boolean> _IsAgreementItem;	            
		private Nullable<Boolean> _IsBaseItem;	            
		private Nullable<Boolean> _IsBadInventory;	            
		private Nullable<Boolean> _IsDefaultService;	            
		private Nullable<Boolean> _IsCheckedEquipment;	            
		private Nullable<Boolean> _IsTransfered;	            
		private Nullable<Int32> _QuantityLeftEquipment;	            
		private Nullable<Boolean> _IsEquipmentExist;	            
		private Nullable<Double> _OriginalUnitPrice;	            
		private Nullable<Boolean> _IsInvoiceCreate;	            
		private String _ReferenceInvoiceId;	            
		private Nullable<Int32> _ReferenceInvDetailId;	            
		private Nullable<Boolean> _IsBilling;	            
		private Nullable<Boolean> _IsCopied;	            
		private Nullable<Boolean> _IsNonCommissionable;	            
		private Guid _InstalledByUid;	            
		private Nullable<Boolean> _IsBillingProcess;	            
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
		public Guid AppointmentId
		{	
			get{ return _AppointmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentId, value, _AppointmentId);
				if (PropertyChanging(args))
				{
					_AppointmentId = value;
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
		public Double UnitPrice
		{	
			get{ return _UnitPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnitPrice, value, _UnitPrice);
				if (PropertyChanging(args))
				{
					_UnitPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double TotalPrice
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
		public String CreatedBy
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
		public String EquipName
		{	
			get{ return _EquipName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipName, value, _EquipName);
				if (PropertyChanging(args))
				{
					_EquipName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipDetail
		{	
			get{ return _EquipDetail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipDetail, value, _EquipDetail);
				if (PropertyChanging(args))
				{
					_EquipDetail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEquipmentRelease
		{	
			get{ return _IsEquipmentRelease; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEquipmentRelease, value, _IsEquipmentRelease);
				if (PropertyChanging(args))
				{
					_IsEquipmentRelease = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsService
		{	
			get{ return _IsService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsService, value, _IsService);
				if (PropertyChanging(args))
				{
					_IsService = value;
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
		public Nullable<Boolean> IsAgreementItem
		{	
			get{ return _IsAgreementItem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAgreementItem, value, _IsAgreementItem);
				if (PropertyChanging(args))
				{
					_IsAgreementItem = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsBaseItem
		{	
			get{ return _IsBaseItem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBaseItem, value, _IsBaseItem);
				if (PropertyChanging(args))
				{
					_IsBaseItem = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsBadInventory
		{	
			get{ return _IsBadInventory; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBadInventory, value, _IsBadInventory);
				if (PropertyChanging(args))
				{
					_IsBadInventory = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDefaultService
		{	
			get{ return _IsDefaultService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefaultService, value, _IsDefaultService);
				if (PropertyChanging(args))
				{
					_IsDefaultService = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCheckedEquipment
		{	
			get{ return _IsCheckedEquipment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCheckedEquipment, value, _IsCheckedEquipment);
				if (PropertyChanging(args))
				{
					_IsCheckedEquipment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTransfered
		{	
			get{ return _IsTransfered; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTransfered, value, _IsTransfered);
				if (PropertyChanging(args))
				{
					_IsTransfered = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> QuantityLeftEquipment
		{	
			get{ return _QuantityLeftEquipment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QuantityLeftEquipment, value, _QuantityLeftEquipment);
				if (PropertyChanging(args))
				{
					_QuantityLeftEquipment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEquipmentExist
		{	
			get{ return _IsEquipmentExist; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEquipmentExist, value, _IsEquipmentExist);
				if (PropertyChanging(args))
				{
					_IsEquipmentExist = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> OriginalUnitPrice
		{	
			get{ return _OriginalUnitPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OriginalUnitPrice, value, _OriginalUnitPrice);
				if (PropertyChanging(args))
				{
					_OriginalUnitPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsInvoiceCreate
		{	
			get{ return _IsInvoiceCreate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInvoiceCreate, value, _IsInvoiceCreate);
				if (PropertyChanging(args))
				{
					_IsInvoiceCreate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReferenceInvoiceId
		{	
			get{ return _ReferenceInvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceInvoiceId, value, _ReferenceInvoiceId);
				if (PropertyChanging(args))
				{
					_ReferenceInvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ReferenceInvDetailId
		{	
			get{ return _ReferenceInvDetailId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceInvDetailId, value, _ReferenceInvDetailId);
				if (PropertyChanging(args))
				{
					_ReferenceInvDetailId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsBilling
		{	
			get{ return _IsBilling; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBilling, value, _IsBilling);
				if (PropertyChanging(args))
				{
					_IsBilling = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCopied
		{	
			get{ return _IsCopied; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCopied, value, _IsCopied);
				if (PropertyChanging(args))
				{
					_IsCopied = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsNonCommissionable
		{	
			get{ return _IsNonCommissionable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsNonCommissionable, value, _IsNonCommissionable);
				if (PropertyChanging(args))
				{
					_IsNonCommissionable = value;
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
		public Nullable<Boolean> IsBillingProcess
		{	
			get{ return _IsBillingProcess; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBillingProcess, value, _IsBillingProcess);
				if (PropertyChanging(args))
				{
					_IsBillingProcess = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAppointmentEquipmentBase Clone()
		{
			CustomerAppointmentEquipmentBase newObj = new  CustomerAppointmentEquipmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AppointmentId = this.AppointmentId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Quantity = this.Quantity;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.EquipName = this.EquipName;						
			newObj.EquipDetail = this.EquipDetail;						
			newObj.IsEquipmentRelease = this.IsEquipmentRelease;						
			newObj.IsService = this.IsService;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.IsAgreementItem = this.IsAgreementItem;						
			newObj.IsBaseItem = this.IsBaseItem;						
			newObj.IsBadInventory = this.IsBadInventory;						
			newObj.IsDefaultService = this.IsDefaultService;						
			newObj.IsCheckedEquipment = this.IsCheckedEquipment;						
			newObj.IsTransfered = this.IsTransfered;						
			newObj.QuantityLeftEquipment = this.QuantityLeftEquipment;						
			newObj.IsEquipmentExist = this.IsEquipmentExist;						
			newObj.OriginalUnitPrice = this.OriginalUnitPrice;						
			newObj.IsInvoiceCreate = this.IsInvoiceCreate;						
			newObj.ReferenceInvoiceId = this.ReferenceInvoiceId;						
			newObj.ReferenceInvDetailId = this.ReferenceInvDetailId;						
			newObj.IsBilling = this.IsBilling;						
			newObj.IsCopied = this.IsCopied;						
			newObj.IsNonCommissionable = this.IsNonCommissionable;						
			newObj.InstalledByUid = this.InstalledByUid;						
			newObj.IsBillingProcess = this.IsBillingProcess;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAppointmentEquipmentBase.Property_Id, Id);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_AppointmentId, AppointmentId);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_Quantity, Quantity);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_EquipName, EquipName);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_EquipDetail, EquipDetail);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsEquipmentRelease, IsEquipmentRelease);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsService, IsService);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsAgreementItem, IsAgreementItem);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsBaseItem, IsBaseItem);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsBadInventory, IsBadInventory);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsDefaultService, IsDefaultService);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsCheckedEquipment, IsCheckedEquipment);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsTransfered, IsTransfered);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_QuantityLeftEquipment, QuantityLeftEquipment);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsEquipmentExist, IsEquipmentExist);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_OriginalUnitPrice, OriginalUnitPrice);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsInvoiceCreate, IsInvoiceCreate);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_ReferenceInvoiceId, ReferenceInvoiceId);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_ReferenceInvDetailId, ReferenceInvDetailId);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsBilling, IsBilling);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsCopied, IsCopied);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsNonCommissionable, IsNonCommissionable);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_InstalledByUid, InstalledByUid);				
			info.AddValue(CustomerAppointmentEquipmentBase.Property_IsBillingProcess, IsBillingProcess);				
		}
		#endregion

		
	}
}
