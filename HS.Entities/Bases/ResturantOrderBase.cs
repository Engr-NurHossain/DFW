using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ResturantOrderBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ResturantOrderBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			OrderId = 1,
			CustomerId = 2,
			Location = 3,
			OrderType = 4,
			Amount = 5,
			Status = 6,
			Quantity = 7,
			PickUpTime = 8,
			CreatedDate = 9,
			LastUpdatedDate = 10,
			CreatedBy = 11,
			LastUpdatedBy = 12,
			CompanyId = 13,
			ContactNo = 14,
			SpecialInstruction = 15,
			OrderDate = 16,
			Notes = 17,
			AcceptDate = 18,
			RejectedReason = 19,
			RejectedDate = 20,
			PaymentMethod = 21,
			TaxAmount = 22,
			RestaurantLocation = 23,
			IsViewed = 24,
			AcceptTime = 25,
			DeliveryNotes = 26,
			CardProfile = 27,
			CardNumber = 28,
			IsDeleted = 29,
			DiscountValue = 30,
			DiscountCode = 31
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_OrderId = "OrderId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Location = "Location";		            
		public const string Property_OrderType = "OrderType";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Status = "Status";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_PickUpTime = "PickUpTime";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_ContactNo = "ContactNo";		            
		public const string Property_SpecialInstruction = "SpecialInstruction";		            
		public const string Property_OrderDate = "OrderDate";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_AcceptDate = "AcceptDate";		            
		public const string Property_RejectedReason = "RejectedReason";		            
		public const string Property_RejectedDate = "RejectedDate";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_TaxAmount = "TaxAmount";		            
		public const string Property_RestaurantLocation = "RestaurantLocation";		            
		public const string Property_IsViewed = "IsViewed";		            
		public const string Property_AcceptTime = "AcceptTime";		            
		public const string Property_DeliveryNotes = "DeliveryNotes";		            
		public const string Property_CardProfile = "CardProfile";		            
		public const string Property_CardNumber = "CardNumber";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		public const string Property_DiscountValue = "DiscountValue";		            
		public const string Property_DiscountCode = "DiscountCode";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _OrderId;	            
		private Guid _CustomerId;	            
		private String _Location;	            
		private String _OrderType;	            
		private Double _Amount;	            
		private String _Status;	            
		private Int32 _Quantity;	            
		private String _PickUpTime;	            
		private DateTime _CreatedDate;	            
		private DateTime _LastUpdatedDate;	            
		private String _CreatedBy;	            
		private String _LastUpdatedBy;	            
		private Guid _CompanyId;	            
		private String _ContactNo;	            
		private String _SpecialInstruction;	            
		private Nullable<DateTime> _OrderDate;	            
		private String _Notes;	            
		private Nullable<DateTime> _AcceptDate;	            
		private String _RejectedReason;	            
		private Nullable<DateTime> _RejectedDate;	            
		private String _PaymentMethod;	            
		private Nullable<Double> _TaxAmount;	            
		private String _RestaurantLocation;	            
		private Nullable<Boolean> _IsViewed;	            
		private String _AcceptTime;	            
		private String _DeliveryNotes;	            
		private String _CardProfile;	            
		private String _CardNumber;	            
		private Nullable<Boolean> _IsDeleted;	            
		private Nullable<Double> _DiscountValue;	            
		private String _DiscountCode;	            
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
		public String OrderType
		{	
			get{ return _OrderType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderType, value, _OrderType);
				if (PropertyChanging(args))
				{
					_OrderType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Amount
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
		public String PickUpTime
		{	
			get{ return _PickUpTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PickUpTime, value, _PickUpTime);
				if (PropertyChanging(args))
				{
					_PickUpTime = value;
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
		public String ContactNo
		{	
			get{ return _ContactNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactNo, value, _ContactNo);
				if (PropertyChanging(args))
				{
					_ContactNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SpecialInstruction
		{	
			get{ return _SpecialInstruction; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SpecialInstruction, value, _SpecialInstruction);
				if (PropertyChanging(args))
				{
					_SpecialInstruction = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> OrderDate
		{	
			get{ return _OrderDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderDate, value, _OrderDate);
				if (PropertyChanging(args))
				{
					_OrderDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AcceptDate
		{	
			get{ return _AcceptDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AcceptDate, value, _AcceptDate);
				if (PropertyChanging(args))
				{
					_AcceptDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RejectedReason
		{	
			get{ return _RejectedReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RejectedReason, value, _RejectedReason);
				if (PropertyChanging(args))
				{
					_RejectedReason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> RejectedDate
		{	
			get{ return _RejectedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RejectedDate, value, _RejectedDate);
				if (PropertyChanging(args))
				{
					_RejectedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentMethod
		{	
			get{ return _PaymentMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentMethod, value, _PaymentMethod);
				if (PropertyChanging(args))
				{
					_PaymentMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxAmount
		{	
			get{ return _TaxAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxAmount, value, _TaxAmount);
				if (PropertyChanging(args))
				{
					_TaxAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RestaurantLocation
		{	
			get{ return _RestaurantLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RestaurantLocation, value, _RestaurantLocation);
				if (PropertyChanging(args))
				{
					_RestaurantLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsViewed
		{	
			get{ return _IsViewed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsViewed, value, _IsViewed);
				if (PropertyChanging(args))
				{
					_IsViewed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AcceptTime
		{	
			get{ return _AcceptTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AcceptTime, value, _AcceptTime);
				if (PropertyChanging(args))
				{
					_AcceptTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DeliveryNotes
		{	
			get{ return _DeliveryNotes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DeliveryNotes, value, _DeliveryNotes);
				if (PropertyChanging(args))
				{
					_DeliveryNotes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardProfile
		{	
			get{ return _CardProfile; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardProfile, value, _CardProfile);
				if (PropertyChanging(args))
				{
					_CardProfile = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardNumber
		{	
			get{ return _CardNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardNumber, value, _CardNumber);
				if (PropertyChanging(args))
				{
					_CardNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDeleted
		{	
			get{ return _IsDeleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDeleted, value, _IsDeleted);
				if (PropertyChanging(args))
				{
					_IsDeleted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountValue
		{	
			get{ return _DiscountValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountValue, value, _DiscountValue);
				if (PropertyChanging(args))
				{
					_DiscountValue = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  ResturantOrderBase Clone()
		{
			ResturantOrderBase newObj = new  ResturantOrderBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.OrderId = this.OrderId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Location = this.Location;						
			newObj.OrderType = this.OrderType;						
			newObj.Amount = this.Amount;						
			newObj.Status = this.Status;						
			newObj.Quantity = this.Quantity;						
			newObj.PickUpTime = this.PickUpTime;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.CompanyId = this.CompanyId;						
			newObj.ContactNo = this.ContactNo;						
			newObj.SpecialInstruction = this.SpecialInstruction;						
			newObj.OrderDate = this.OrderDate;						
			newObj.Notes = this.Notes;						
			newObj.AcceptDate = this.AcceptDate;						
			newObj.RejectedReason = this.RejectedReason;						
			newObj.RejectedDate = this.RejectedDate;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.TaxAmount = this.TaxAmount;						
			newObj.RestaurantLocation = this.RestaurantLocation;						
			newObj.IsViewed = this.IsViewed;						
			newObj.AcceptTime = this.AcceptTime;						
			newObj.DeliveryNotes = this.DeliveryNotes;						
			newObj.CardProfile = this.CardProfile;						
			newObj.CardNumber = this.CardNumber;						
			newObj.IsDeleted = this.IsDeleted;						
			newObj.DiscountValue = this.DiscountValue;						
			newObj.DiscountCode = this.DiscountCode;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ResturantOrderBase.Property_Id, Id);				
			info.AddValue(ResturantOrderBase.Property_OrderId, OrderId);				
			info.AddValue(ResturantOrderBase.Property_CustomerId, CustomerId);				
			info.AddValue(ResturantOrderBase.Property_Location, Location);				
			info.AddValue(ResturantOrderBase.Property_OrderType, OrderType);				
			info.AddValue(ResturantOrderBase.Property_Amount, Amount);				
			info.AddValue(ResturantOrderBase.Property_Status, Status);				
			info.AddValue(ResturantOrderBase.Property_Quantity, Quantity);				
			info.AddValue(ResturantOrderBase.Property_PickUpTime, PickUpTime);				
			info.AddValue(ResturantOrderBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ResturantOrderBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(ResturantOrderBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ResturantOrderBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(ResturantOrderBase.Property_CompanyId, CompanyId);				
			info.AddValue(ResturantOrderBase.Property_ContactNo, ContactNo);				
			info.AddValue(ResturantOrderBase.Property_SpecialInstruction, SpecialInstruction);				
			info.AddValue(ResturantOrderBase.Property_OrderDate, OrderDate);				
			info.AddValue(ResturantOrderBase.Property_Notes, Notes);				
			info.AddValue(ResturantOrderBase.Property_AcceptDate, AcceptDate);				
			info.AddValue(ResturantOrderBase.Property_RejectedReason, RejectedReason);				
			info.AddValue(ResturantOrderBase.Property_RejectedDate, RejectedDate);				
			info.AddValue(ResturantOrderBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(ResturantOrderBase.Property_TaxAmount, TaxAmount);				
			info.AddValue(ResturantOrderBase.Property_RestaurantLocation, RestaurantLocation);				
			info.AddValue(ResturantOrderBase.Property_IsViewed, IsViewed);				
			info.AddValue(ResturantOrderBase.Property_AcceptTime, AcceptTime);				
			info.AddValue(ResturantOrderBase.Property_DeliveryNotes, DeliveryNotes);				
			info.AddValue(ResturantOrderBase.Property_CardProfile, CardProfile);				
			info.AddValue(ResturantOrderBase.Property_CardNumber, CardNumber);				
			info.AddValue(ResturantOrderBase.Property_IsDeleted, IsDeleted);				
			info.AddValue(ResturantOrderBase.Property_DiscountValue, DiscountValue);				
			info.AddValue(ResturantOrderBase.Property_DiscountCode, DiscountCode);				
		}
		#endregion

		
	}
}
