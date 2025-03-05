using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketBookingExtraItemBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketBookingExtraItemBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			BookingId = 1,
			EquipmentId = 2,
			EquipName = 3,
			EquipDetail = 4,
			Quantity = 5,
			UnitPrice = 6,
			Discount = 7,
			TotalPrice = 8,
			CreatedDate = 9,
			CreatedBy = 10,
			Taxable = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_BookingId = "BookingId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_EquipName = "EquipName";		            
		public const string Property_EquipDetail = "EquipDetail";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_Discount = "Discount";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_Taxable = "Taxable";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _BookingId;	            
		private Guid _EquipmentId;	            
		private String _EquipName;	            
		private String _EquipDetail;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _UnitPrice;	            
		private Nullable<Double> _Discount;	            
		private Nullable<Double> _TotalPrice;	            
		private Nullable<DateTime> _CreatedDate;	            
		private String _CreatedBy;	            
		private Nullable<Boolean> _Taxable;	            
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
		public Int32 BookingId
		{	
			get{ return _BookingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BookingId, value, _BookingId);
				if (PropertyChanging(args))
				{
					_BookingId = value;
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
		public Nullable<Double> Discount
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

		#endregion
		
		#region Cloning Base Objects
		public  TicketBookingExtraItemBase Clone()
		{
			TicketBookingExtraItemBase newObj = new  TicketBookingExtraItemBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.BookingId = this.BookingId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.EquipName = this.EquipName;						
			newObj.EquipDetail = this.EquipDetail;						
			newObj.Quantity = this.Quantity;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.Discount = this.Discount;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.Taxable = this.Taxable;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketBookingExtraItemBase.Property_Id, Id);				
			info.AddValue(TicketBookingExtraItemBase.Property_BookingId, BookingId);				
			info.AddValue(TicketBookingExtraItemBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(TicketBookingExtraItemBase.Property_EquipName, EquipName);				
			info.AddValue(TicketBookingExtraItemBase.Property_EquipDetail, EquipDetail);				
			info.AddValue(TicketBookingExtraItemBase.Property_Quantity, Quantity);				
			info.AddValue(TicketBookingExtraItemBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(TicketBookingExtraItemBase.Property_Discount, Discount);				
			info.AddValue(TicketBookingExtraItemBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(TicketBookingExtraItemBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TicketBookingExtraItemBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TicketBookingExtraItemBase.Property_Taxable, Taxable);				
		}
		#endregion

		
	}
}
