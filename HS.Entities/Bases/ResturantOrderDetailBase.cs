using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ResturantOrderDetailBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ResturantOrderDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			OrderId = 1,
			ItemId = 2,
			ItemName = 3,
			ItemPrice = 4,
			ItemQty = 5,
			CreatedDate = 6,
			CreatedBy = 7,
			LastUpdatedDate = 8,
			LastUpdatedBy = 9,
			CompanyId = 10,
			IsStock = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_OrderId = "OrderId";		            
		public const string Property_ItemId = "ItemId";		            
		public const string Property_ItemName = "ItemName";		            
		public const string Property_ItemPrice = "ItemPrice";		            
		public const string Property_ItemQty = "ItemQty";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsStock = "IsStock";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _OrderId;	            
		private Int32 _ItemId;	            
		private String _ItemName;	            
		private Double _ItemPrice;	            
		private Int32 _ItemQty;	            
		private DateTime _CreatedDate;	            
		private String _CreatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _LastUpdatedBy;	            
		private Guid _CompanyId;	            
		private Nullable<Boolean> _IsStock;	            
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
		public Guid OrderId
		{	
			get{ return _OrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderId, value, _OrderId);
				if (PropertyChanging(args))
				{
					_OrderId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ItemId
		{	
			get{ return _ItemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemId, value, _ItemId);
				if (PropertyChanging(args))
				{
					_ItemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemName
		{	
			get{ return _ItemName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemName, value, _ItemName);
				if (PropertyChanging(args))
				{
					_ItemName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double ItemPrice
		{	
			get{ return _ItemPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemPrice, value, _ItemPrice);
				if (PropertyChanging(args))
				{
					_ItemPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ItemQty
		{	
			get{ return _ItemQty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemQty, value, _ItemQty);
				if (PropertyChanging(args))
				{
					_ItemQty = value;
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
		public String LastUpdatedBy
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
		public Nullable<Boolean> IsStock
		{	
			get{ return _IsStock; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsStock, value, _IsStock);
				if (PropertyChanging(args))
				{
					_IsStock = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ResturantOrderDetailBase Clone()
		{
			ResturantOrderDetailBase newObj = new  ResturantOrderDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.OrderId = this.OrderId;						
			newObj.ItemId = this.ItemId;						
			newObj.ItemName = this.ItemName;						
			newObj.ItemPrice = this.ItemPrice;						
			newObj.ItemQty = this.ItemQty;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsStock = this.IsStock;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ResturantOrderDetailBase.Property_Id, Id);				
			info.AddValue(ResturantOrderDetailBase.Property_OrderId, OrderId);				
			info.AddValue(ResturantOrderDetailBase.Property_ItemId, ItemId);				
			info.AddValue(ResturantOrderDetailBase.Property_ItemName, ItemName);				
			info.AddValue(ResturantOrderDetailBase.Property_ItemPrice, ItemPrice);				
			info.AddValue(ResturantOrderDetailBase.Property_ItemQty, ItemQty);				
			info.AddValue(ResturantOrderDetailBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ResturantOrderDetailBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ResturantOrderDetailBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(ResturantOrderDetailBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(ResturantOrderDetailBase.Property_CompanyId, CompanyId);				
			info.AddValue(ResturantOrderDetailBase.Property_IsStock, IsStock);				
		}
		#endregion

		
	}
}
