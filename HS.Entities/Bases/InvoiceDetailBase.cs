using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InvoiceDetailBase", Namespace = "http://www.hims-tech.com//entities")]
	public class InvoiceDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			InvoiceId = 1,
			InventoryId = 2,
			EquipmentId = 3,
			EquipName = 4,
			EquipDetail = 5,
			CompanyId = 6,
			BundleId = 7,
			Quantity = 8,
			UnitPrice = 9,
			TotalPrice = 10,
			CreatedDate = 11,
			CreatedBy = 12,
			Taxable = 13,
			DiscountAmount = 14,
			DiscountPercent = 15,
			DiscountType = 16,
			EquipCategory = 17
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_InventoryId = "InventoryId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_EquipName = "EquipName";		            
		public const string Property_EquipDetail = "EquipDetail";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BundleId = "BundleId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_Taxable = "Taxable";		            
		public const string Property_DiscountAmount = "DiscountAmount";		            
		public const string Property_DiscountPercent = "DiscountPercent";		            
		public const string Property_DiscountType = "DiscountType";		            
		public const string Property_EquipCategory = "EquipCategory";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _InvoiceId;	            
		private Guid _InventoryId;	            
		private Guid _EquipmentId;	            
		private String _EquipName;	            
		private String _EquipDetail;	            
		private Guid _CompanyId;	            
		private Nullable<Int32> _BundleId;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _UnitPrice;	            
		private Nullable<Double> _TotalPrice;	            
		private Nullable<DateTime> _CreatedDate;	            
		private String _CreatedBy;	            
		private Nullable<Boolean> _Taxable;	            
		private Nullable<Double> _DiscountAmount;	            
		private Nullable<Double> _DiscountPercent;	            
		private String _DiscountType;	            
		private String _EquipCategory;	            
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
		public String InvoiceId
		{	
			get{ return _InvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceId, value, _InvoiceId);
				if (PropertyChanging(args))
				{
					_InvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid InventoryId
		{	
			get{ return _InventoryId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InventoryId, value, _InventoryId);
				if (PropertyChanging(args))
				{
					_InventoryId = value;
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
		public Nullable<Int32> BundleId
		{	
			get{ return _BundleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BundleId, value, _BundleId);
				if (PropertyChanging(args))
				{
					_BundleId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Quantity
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
		public Nullable<Double> UnitPrice
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
		public Nullable<DateTime> CreatedDate
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
		public Nullable<Boolean> Taxable
		{	
			get{ return _Taxable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Taxable, value, _Taxable);
				if (PropertyChanging(args))
				{
					_Taxable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountAmount
		{	
			get{ return _DiscountAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountAmount, value, _DiscountAmount);
				if (PropertyChanging(args))
				{
					_DiscountAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountPercent
		{	
			get{ return _DiscountPercent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountPercent, value, _DiscountPercent);
				if (PropertyChanging(args))
				{
					_DiscountPercent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DiscountType
		{	
			get{ return _DiscountType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountType, value, _DiscountType);
				if (PropertyChanging(args))
				{
					_DiscountType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipCategory
		{	
			get{ return _EquipCategory; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipCategory, value, _EquipCategory);
				if (PropertyChanging(args))
				{
					_EquipCategory = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  InvoiceDetailBase Clone()
		{
			InvoiceDetailBase newObj = new  InvoiceDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.InventoryId = this.InventoryId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.EquipName = this.EquipName;						
			newObj.EquipDetail = this.EquipDetail;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BundleId = this.BundleId;						
			newObj.Quantity = this.Quantity;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.Taxable = this.Taxable;						
			newObj.DiscountAmount = this.DiscountAmount;						
			newObj.DiscountPercent = this.DiscountPercent;						
			newObj.DiscountType = this.DiscountType;						
			newObj.EquipCategory = this.EquipCategory;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InvoiceDetailBase.Property_Id, Id);				
			info.AddValue(InvoiceDetailBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(InvoiceDetailBase.Property_InventoryId, InventoryId);				
			info.AddValue(InvoiceDetailBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(InvoiceDetailBase.Property_EquipName, EquipName);				
			info.AddValue(InvoiceDetailBase.Property_EquipDetail, EquipDetail);				
			info.AddValue(InvoiceDetailBase.Property_CompanyId, CompanyId);				
			info.AddValue(InvoiceDetailBase.Property_BundleId, BundleId);				
			info.AddValue(InvoiceDetailBase.Property_Quantity, Quantity);				
			info.AddValue(InvoiceDetailBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(InvoiceDetailBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(InvoiceDetailBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(InvoiceDetailBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(InvoiceDetailBase.Property_Taxable, Taxable);				
			info.AddValue(InvoiceDetailBase.Property_DiscountAmount, DiscountAmount);				
			info.AddValue(InvoiceDetailBase.Property_DiscountPercent, DiscountPercent);				
			info.AddValue(InvoiceDetailBase.Property_DiscountType, DiscountType);				
			info.AddValue(InvoiceDetailBase.Property_EquipCategory, EquipCategory);				
		}
		#endregion

		
	}
}
