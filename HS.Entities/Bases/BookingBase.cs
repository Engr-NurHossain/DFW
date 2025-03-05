using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BookingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class BookingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			BookingId = 1,
			CustomerId = 2,
			CompanyId = 3,
			Amount = 4,
			TaxType = 5,
			Tax = 6,
			DiscountType = 7,
			DiscountCode = 8,
			DiscountAmount = 9,
			TotalAmount = 10,
			Status = 11,
			Description = 12,
			Message = 13,
			BillingAddress = 14,
			PickUpDate = 15,
			Signature = 16,
			CancelReason = 17,
			CreatedDate = 18,
			CreatedBy = 19,
			LastUpdatedDate = 20,
			LastUpdatedBy = 21,
			EmailAddress = 22,
			PickUpLocation = 23,
			DropOffLocation = 24,
			DropOffDate = 25,
			BookingSource = 26
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_BookingId = "BookingId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_TaxType = "TaxType";		            
		public const string Property_Tax = "Tax";		            
		public const string Property_DiscountType = "DiscountType";		            
		public const string Property_DiscountCode = "DiscountCode";		            
		public const string Property_DiscountAmount = "DiscountAmount";		            
		public const string Property_TotalAmount = "TotalAmount";		            
		public const string Property_Status = "Status";		            
		public const string Property_Description = "Description";		            
		public const string Property_Message = "Message";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_PickUpDate = "PickUpDate";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_CancelReason = "CancelReason";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_PickUpLocation = "PickUpLocation";		            
		public const string Property_DropOffLocation = "DropOffLocation";		            
		public const string Property_DropOffDate = "DropOffDate";		            
		public const string Property_BookingSource = "BookingSource";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _BookingId;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private Nullable<Double> _Amount;	            
		private String _TaxType;	            
		private Nullable<Double> _Tax;	            
		private String _DiscountType;	            
		private String _DiscountCode;	            
		private Nullable<Double> _DiscountAmount;	            
		private Nullable<Double> _TotalAmount;	            
		private String _Status;	            
		private String _Description;	            
		private String _Message;	            
		private String _BillingAddress;	            
		private Nullable<DateTime> _PickUpDate;	            
		private String _Signature;	            
		private String _CancelReason;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedBy;	            
		private String _EmailAddress;	            
		private String _PickUpLocation;	            
		private String _DropOffLocation;	            
		private Nullable<DateTime> _DropOffDate;	            
		private String _BookingSource;	            
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
		public Nullable<Double> Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
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
		public Nullable<Double> Tax
		{	
			get{ return _Tax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tax, value, _Tax);
				if (PropertyChanging(args))
				{
					_Tax = value;
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
		public String DiscountCode
		{	
			get{ return _DiscountCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountCode, value, _DiscountCode);
				if (PropertyChanging(args))
				{
					_DiscountCode = value;
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
		public Nullable<Double> TotalAmount
		{	
			get{ return _TotalAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalAmount, value, _TotalAmount);
				if (PropertyChanging(args))
				{
					_TotalAmount = value;
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
		public String Message
		{	
			get{ return _Message; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Message, value, _Message);
				if (PropertyChanging(args))
				{
					_Message = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingAddress
		{	
			get{ return _BillingAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingAddress, value, _BillingAddress);
				if (PropertyChanging(args))
				{
					_BillingAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PickUpDate
		{	
			get{ return _PickUpDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PickUpDate, value, _PickUpDate);
				if (PropertyChanging(args))
				{
					_PickUpDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Signature
		{	
			get{ return _Signature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Signature, value, _Signature);
				if (PropertyChanging(args))
				{
					_Signature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CancelReason
		{	
			get{ return _CancelReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancelReason, value, _CancelReason);
				if (PropertyChanging(args))
				{
					_CancelReason = value;
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
		public String EmailAddress
		{	
			get{ return _EmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailAddress, value, _EmailAddress);
				if (PropertyChanging(args))
				{
					_EmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PickUpLocation
		{	
			get{ return _PickUpLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PickUpLocation, value, _PickUpLocation);
				if (PropertyChanging(args))
				{
					_PickUpLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DropOffLocation
		{	
			get{ return _DropOffLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DropOffLocation, value, _DropOffLocation);
				if (PropertyChanging(args))
				{
					_DropOffLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DropOffDate
		{	
			get{ return _DropOffDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DropOffDate, value, _DropOffDate);
				if (PropertyChanging(args))
				{
					_DropOffDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BookingSource
		{	
			get{ return _BookingSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BookingSource, value, _BookingSource);
				if (PropertyChanging(args))
				{
					_BookingSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BookingBase Clone()
		{
			BookingBase newObj = new  BookingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.BookingId = this.BookingId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Amount = this.Amount;						
			newObj.TaxType = this.TaxType;						
			newObj.Tax = this.Tax;						
			newObj.DiscountType = this.DiscountType;						
			newObj.DiscountCode = this.DiscountCode;						
			newObj.DiscountAmount = this.DiscountAmount;						
			newObj.TotalAmount = this.TotalAmount;						
			newObj.Status = this.Status;						
			newObj.Description = this.Description;						
			newObj.Message = this.Message;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.PickUpDate = this.PickUpDate;						
			newObj.Signature = this.Signature;						
			newObj.CancelReason = this.CancelReason;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.PickUpLocation = this.PickUpLocation;						
			newObj.DropOffLocation = this.DropOffLocation;						
			newObj.DropOffDate = this.DropOffDate;						
			newObj.BookingSource = this.BookingSource;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BookingBase.Property_Id, Id);				
			info.AddValue(BookingBase.Property_BookingId, BookingId);				
			info.AddValue(BookingBase.Property_CustomerId, CustomerId);				
			info.AddValue(BookingBase.Property_CompanyId, CompanyId);				
			info.AddValue(BookingBase.Property_Amount, Amount);				
			info.AddValue(BookingBase.Property_TaxType, TaxType);				
			info.AddValue(BookingBase.Property_Tax, Tax);				
			info.AddValue(BookingBase.Property_DiscountType, DiscountType);				
			info.AddValue(BookingBase.Property_DiscountCode, DiscountCode);				
			info.AddValue(BookingBase.Property_DiscountAmount, DiscountAmount);				
			info.AddValue(BookingBase.Property_TotalAmount, TotalAmount);				
			info.AddValue(BookingBase.Property_Status, Status);				
			info.AddValue(BookingBase.Property_Description, Description);				
			info.AddValue(BookingBase.Property_Message, Message);				
			info.AddValue(BookingBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(BookingBase.Property_PickUpDate, PickUpDate);				
			info.AddValue(BookingBase.Property_Signature, Signature);				
			info.AddValue(BookingBase.Property_CancelReason, CancelReason);				
			info.AddValue(BookingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(BookingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(BookingBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(BookingBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(BookingBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(BookingBase.Property_PickUpLocation, PickUpLocation);				
			info.AddValue(BookingBase.Property_DropOffLocation, DropOffLocation);				
			info.AddValue(BookingBase.Property_DropOffDate, DropOffDate);				
			info.AddValue(BookingBase.Property_BookingSource, BookingSource);				
		}
		#endregion

		
	}
}
