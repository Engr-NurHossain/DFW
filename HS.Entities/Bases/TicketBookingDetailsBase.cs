using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketBookingDetailsBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TicketBookingDetailsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			BookingId = 2,
			RugType = 3,
			Length = 4,
			LengthInch = 5,
			Width = 6,
			WidthInch = 7,
			Radius = 8,
			RadiusInch = 9,
			TotalSize = 10,
			Package = 11,
			Included = 12,
			Extras = 13,
			UnitPrice = 14,
			DiscountType = 15,
			TaxType = 16,
			Quantity = 17,
			Discount = 18,
			TotalPrice = 19,
			AddedDate = 20,
			AddedBy = 21,
			RugConditions = 22,
			Comments = 23
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BookingId = "BookingId";		            
		public const string Property_RugType = "RugType";		            
		public const string Property_Length = "Length";		            
		public const string Property_LengthInch = "LengthInch";		            
		public const string Property_Width = "Width";		            
		public const string Property_WidthInch = "WidthInch";		            
		public const string Property_Radius = "Radius";		            
		public const string Property_RadiusInch = "RadiusInch";		            
		public const string Property_TotalSize = "TotalSize";		            
		public const string Property_Package = "Package";		            
		public const string Property_Included = "Included";		            
		public const string Property_Extras = "Extras";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_DiscountType = "DiscountType";		            
		public const string Property_TaxType = "TaxType";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_Discount = "Discount";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_RugConditions = "RugConditions";		            
		public const string Property_Comments = "Comments";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _BookingId;	            
		private String _RugType;	            
		private Nullable<Double> _Length;	            
		private Nullable<Double> _LengthInch;	            
		private Nullable<Double> _Width;	            
		private Nullable<Double> _WidthInch;	            
		private Nullable<Double> _Radius;	            
		private Nullable<Double> _RadiusInch;	            
		private Nullable<Double> _TotalSize;	            
		private String _Package;	            
		private String _Included;	            
		private String _Extras;	            
		private Nullable<Double> _UnitPrice;	            
		private String _DiscountType;	            
		private String _TaxType;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _Discount;	            
		private Nullable<Double> _TotalPrice;	            
		private DateTime _AddedDate;	            
		private Guid _AddedBy;	            
		private String _RugConditions;	            
		private String _Comments;	            
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
		public String BookingId
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
		public String RugType
		{	
			get{ return _RugType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RugType, value, _RugType);
				if (PropertyChanging(args))
				{
					_RugType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Length
		{	
			get{ return _Length; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Length, value, _Length);
				if (PropertyChanging(args))
				{
					_Length = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> LengthInch
		{	
			get{ return _LengthInch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LengthInch, value, _LengthInch);
				if (PropertyChanging(args))
				{
					_LengthInch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Width
		{	
			get{ return _Width; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Width, value, _Width);
				if (PropertyChanging(args))
				{
					_Width = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> WidthInch
		{	
			get{ return _WidthInch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WidthInch, value, _WidthInch);
				if (PropertyChanging(args))
				{
					_WidthInch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Radius
		{	
			get{ return _Radius; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Radius, value, _Radius);
				if (PropertyChanging(args))
				{
					_Radius = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RadiusInch
		{	
			get{ return _RadiusInch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RadiusInch, value, _RadiusInch);
				if (PropertyChanging(args))
				{
					_RadiusInch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalSize
		{	
			get{ return _TotalSize; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalSize, value, _TotalSize);
				if (PropertyChanging(args))
				{
					_TotalSize = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Package
		{	
			get{ return _Package; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Package, value, _Package);
				if (PropertyChanging(args))
				{
					_Package = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Included
		{	
			get{ return _Included; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Included, value, _Included);
				if (PropertyChanging(args))
				{
					_Included = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Extras
		{	
			get{ return _Extras; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Extras, value, _Extras);
				if (PropertyChanging(args))
				{
					_Extras = value;
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
		public String TaxType
		{	
			get{ return _TaxType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxType, value, _TaxType);
				if (PropertyChanging(args))
				{
					_TaxType = value;
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
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RugConditions
		{	
			get{ return _RugConditions; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RugConditions, value, _RugConditions);
				if (PropertyChanging(args))
				{
					_RugConditions = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Comments
		{	
			get{ return _Comments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Comments, value, _Comments);
				if (PropertyChanging(args))
				{
					_Comments = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketBookingDetailsBase Clone()
		{
			TicketBookingDetailsBase newObj = new  TicketBookingDetailsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BookingId = this.BookingId;						
			newObj.RugType = this.RugType;						
			newObj.Length = this.Length;						
			newObj.LengthInch = this.LengthInch;						
			newObj.Width = this.Width;						
			newObj.WidthInch = this.WidthInch;						
			newObj.Radius = this.Radius;						
			newObj.RadiusInch = this.RadiusInch;						
			newObj.TotalSize = this.TotalSize;						
			newObj.Package = this.Package;						
			newObj.Included = this.Included;						
			newObj.Extras = this.Extras;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.DiscountType = this.DiscountType;						
			newObj.TaxType = this.TaxType;						
			newObj.Quantity = this.Quantity;						
			newObj.Discount = this.Discount;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.AddedDate = this.AddedDate;						
			newObj.AddedBy = this.AddedBy;						
			newObj.RugConditions = this.RugConditions;						
			newObj.Comments = this.Comments;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketBookingDetailsBase.Property_Id, Id);				
			info.AddValue(TicketBookingDetailsBase.Property_CompanyId, CompanyId);				
			info.AddValue(TicketBookingDetailsBase.Property_BookingId, BookingId);				
			info.AddValue(TicketBookingDetailsBase.Property_RugType, RugType);				
			info.AddValue(TicketBookingDetailsBase.Property_Length, Length);				
			info.AddValue(TicketBookingDetailsBase.Property_LengthInch, LengthInch);				
			info.AddValue(TicketBookingDetailsBase.Property_Width, Width);				
			info.AddValue(TicketBookingDetailsBase.Property_WidthInch, WidthInch);				
			info.AddValue(TicketBookingDetailsBase.Property_Radius, Radius);				
			info.AddValue(TicketBookingDetailsBase.Property_RadiusInch, RadiusInch);				
			info.AddValue(TicketBookingDetailsBase.Property_TotalSize, TotalSize);				
			info.AddValue(TicketBookingDetailsBase.Property_Package, Package);				
			info.AddValue(TicketBookingDetailsBase.Property_Included, Included);				
			info.AddValue(TicketBookingDetailsBase.Property_Extras, Extras);				
			info.AddValue(TicketBookingDetailsBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(TicketBookingDetailsBase.Property_DiscountType, DiscountType);				
			info.AddValue(TicketBookingDetailsBase.Property_TaxType, TaxType);				
			info.AddValue(TicketBookingDetailsBase.Property_Quantity, Quantity);				
			info.AddValue(TicketBookingDetailsBase.Property_Discount, Discount);				
			info.AddValue(TicketBookingDetailsBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(TicketBookingDetailsBase.Property_AddedDate, AddedDate);				
			info.AddValue(TicketBookingDetailsBase.Property_AddedBy, AddedBy);				
			info.AddValue(TicketBookingDetailsBase.Property_RugConditions, RugConditions);				
			info.AddValue(TicketBookingDetailsBase.Property_Comments, Comments);				
		}
		#endregion

		
	}
}
