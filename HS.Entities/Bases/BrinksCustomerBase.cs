using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BrinksCustomerBase", Namespace = "http://www.hims-tech.com//entities")]
	public class BrinksCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			CustomerId = 1,
			CustomerNumber = 2,
			FirstName = 3,
			LastName = 4,
			TransectionID = 5,
			DateSubmitted = 6,
			Contact = 7,
			DealerNumber = 8,
			AccountOnlineDate = 9
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CustomerNumber = "CustomerNumber";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_TransectionID = "TransectionID";		            
		public const string Property_DateSubmitted = "DateSubmitted";		            
		public const string Property_Contact = "Contact";		            
		public const string Property_DealerNumber = "DealerNumber";		            
		public const string Property_AccountOnlineDate = "AccountOnlineDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private Nullable<Guid> _CustomerId;	            
		private Nullable<Int32> _CustomerNumber;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _TransectionID;	            
		private Nullable<DateTime> _DateSubmitted;	            
		private String _Contact;	            
		private String _DealerNumber;	            
		private Nullable<DateTime> _AccountOnlineDate;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int32 ID
		{	
			get{ return _ID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ID, value, _ID);
				if (PropertyChanging(args))
				{
					_ID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Guid> CustomerId
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
		public Nullable<Int32> CustomerNumber
		{	
			get{ return _CustomerNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerNumber, value, _CustomerNumber);
				if (PropertyChanging(args))
				{
					_CustomerNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FirstName
		{	
			get{ return _FirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstName, value, _FirstName);
				if (PropertyChanging(args))
				{
					_FirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastName
		{	
			get{ return _LastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastName, value, _LastName);
				if (PropertyChanging(args))
				{
					_LastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransectionID
		{	
			get{ return _TransectionID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransectionID, value, _TransectionID);
				if (PropertyChanging(args))
				{
					_TransectionID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DateSubmitted
		{	
			get{ return _DateSubmitted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateSubmitted, value, _DateSubmitted);
				if (PropertyChanging(args))
				{
					_DateSubmitted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Contact
		{	
			get{ return _Contact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Contact, value, _Contact);
				if (PropertyChanging(args))
				{
					_Contact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DealerNumber
		{	
			get{ return _DealerNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DealerNumber, value, _DealerNumber);
				if (PropertyChanging(args))
				{
					_DealerNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AccountOnlineDate
		{	
			get{ return _AccountOnlineDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountOnlineDate, value, _AccountOnlineDate);
				if (PropertyChanging(args))
				{
					_AccountOnlineDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BrinksCustomerBase Clone()
		{
			BrinksCustomerBase newObj = new  BrinksCustomerBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CustomerNumber = this.CustomerNumber;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.TransectionID = this.TransectionID;						
			newObj.DateSubmitted = this.DateSubmitted;						
			newObj.Contact = this.Contact;						
			newObj.DealerNumber = this.DealerNumber;						
			newObj.AccountOnlineDate = this.AccountOnlineDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BrinksCustomerBase.Property_ID, ID);				
			info.AddValue(BrinksCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(BrinksCustomerBase.Property_CustomerNumber, CustomerNumber);				
			info.AddValue(BrinksCustomerBase.Property_FirstName, FirstName);				
			info.AddValue(BrinksCustomerBase.Property_LastName, LastName);				
			info.AddValue(BrinksCustomerBase.Property_TransectionID, TransectionID);				
			info.AddValue(BrinksCustomerBase.Property_DateSubmitted, DateSubmitted);				
			info.AddValue(BrinksCustomerBase.Property_Contact, Contact);				
			info.AddValue(BrinksCustomerBase.Property_DealerNumber, DealerNumber);				
			info.AddValue(BrinksCustomerBase.Property_AccountOnlineDate, AccountOnlineDate);				
		}
		#endregion

		
	}
}
