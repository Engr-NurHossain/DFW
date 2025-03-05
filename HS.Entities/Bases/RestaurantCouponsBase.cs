using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestaurantCouponsBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestaurantCouponsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CouponCode = 2,
			StartDate = 3,
			EndDate = 4,
			Discount = 5,
			MinimumOrder = 6,
			ReedemRequired = 7,
			CreatedDate = 8,
			LastUpdatedDate = 9,
			CreatedBy = 10,
			LastUpdatedBy = 11,
			DiscountType = 12,
			Status = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CouponCode = "CouponCode";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_Discount = "Discount";		            
		public const string Property_MinimumOrder = "MinimumOrder";		            
		public const string Property_ReedemRequired = "ReedemRequired";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_DiscountType = "DiscountType";		            
		public const string Property_Status = "Status";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _CouponCode;	            
		private DateTime _StartDate;	            
		private DateTime _EndDate;	            
		private String _Discount;	            
		private String _MinimumOrder;	            
		private String _ReedemRequired;	            
		private DateTime _CreatedDate;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private String _DiscountType;	            
		private Nullable<Boolean> _Status;	            
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
		public String CouponCode
		{	
			get{ return _CouponCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CouponCode, value, _CouponCode);
				if (PropertyChanging(args))
				{
					_CouponCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime StartDate
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
		public DateTime EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Discount
		{	
			get{ return _Discount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Discount, value, _Discount);
				if (PropertyChanging(args))
				{
					_Discount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MinimumOrder
		{	
			get{ return _MinimumOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinimumOrder, value, _MinimumOrder);
				if (PropertyChanging(args))
				{
					_MinimumOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReedemRequired
		{	
			get{ return _ReedemRequired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReedemRequired, value, _ReedemRequired);
				if (PropertyChanging(args))
				{
					_ReedemRequired = value;
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
		public Nullable<Boolean> Status
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

		#endregion
		
		#region Cloning Base Objects
		public  RestaurantCouponsBase Clone()
		{
			RestaurantCouponsBase newObj = new  RestaurantCouponsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CouponCode = this.CouponCode;						
			newObj.StartDate = this.StartDate;						
			newObj.EndDate = this.EndDate;						
			newObj.Discount = this.Discount;						
			newObj.MinimumOrder = this.MinimumOrder;						
			newObj.ReedemRequired = this.ReedemRequired;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.DiscountType = this.DiscountType;						
			newObj.Status = this.Status;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestaurantCouponsBase.Property_Id, Id);				
			info.AddValue(RestaurantCouponsBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestaurantCouponsBase.Property_CouponCode, CouponCode);				
			info.AddValue(RestaurantCouponsBase.Property_StartDate, StartDate);				
			info.AddValue(RestaurantCouponsBase.Property_EndDate, EndDate);				
			info.AddValue(RestaurantCouponsBase.Property_Discount, Discount);				
			info.AddValue(RestaurantCouponsBase.Property_MinimumOrder, MinimumOrder);				
			info.AddValue(RestaurantCouponsBase.Property_ReedemRequired, ReedemRequired);				
			info.AddValue(RestaurantCouponsBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestaurantCouponsBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(RestaurantCouponsBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestaurantCouponsBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestaurantCouponsBase.Property_DiscountType, DiscountType);				
			info.AddValue(RestaurantCouponsBase.Property_Status, Status);				
		}
		#endregion

		
	}
}
