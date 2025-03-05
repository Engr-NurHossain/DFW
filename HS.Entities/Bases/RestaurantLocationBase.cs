using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestaurantLocationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestaurantLocationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			LocationName = 2,
			StreetAddress = 3,
			Address2 = 4,
			City = 5,
			State = 6,
			Zip = 7,
			ContactName = 8,
			ContactPhone = 9,
			ContactEmail = 10,
			CreatedBy = 11,
			CreatedDate = 12,
			LastUpdatedBy = 13,
			LastUpdatedDate = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_LocationName = "LocationName";		            
		public const string Property_StreetAddress = "StreetAddress";		            
		public const string Property_Address2 = "Address2";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zip = "Zip";		            
		public const string Property_ContactName = "ContactName";		            
		public const string Property_ContactPhone = "ContactPhone";		            
		public const string Property_ContactEmail = "ContactEmail";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _LocationName;	            
		private String _StreetAddress;	            
		private String _Address2;	            
		private String _City;	            
		private String _State;	            
		private String _Zip;	            
		private String _ContactName;	            
		private String _ContactPhone;	            
		private String _ContactEmail;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public String LocationName
		{	
			get{ return _LocationName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LocationName, value, _LocationName);
				if (PropertyChanging(args))
				{
					_LocationName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StreetAddress
		{	
			get{ return _StreetAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StreetAddress, value, _StreetAddress);
				if (PropertyChanging(args))
				{
					_StreetAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Address2
		{	
			get{ return _Address2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address2, value, _Address2);
				if (PropertyChanging(args))
				{
					_Address2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String City
		{	
			get{ return _City; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_City, value, _City);
				if (PropertyChanging(args))
				{
					_City = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String State
		{	
			get{ return _State; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_State, value, _State);
				if (PropertyChanging(args))
				{
					_State = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zip
		{	
			get{ return _Zip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zip, value, _Zip);
				if (PropertyChanging(args))
				{
					_Zip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactName
		{	
			get{ return _ContactName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactName, value, _ContactName);
				if (PropertyChanging(args))
				{
					_ContactName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactPhone
		{	
			get{ return _ContactPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactPhone, value, _ContactPhone);
				if (PropertyChanging(args))
				{
					_ContactPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactEmail
		{	
			get{ return _ContactEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactEmail, value, _ContactEmail);
				if (PropertyChanging(args))
				{
					_ContactEmail = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RestaurantLocationBase Clone()
		{
			RestaurantLocationBase newObj = new  RestaurantLocationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.LocationName = this.LocationName;						
			newObj.StreetAddress = this.StreetAddress;						
			newObj.Address2 = this.Address2;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zip = this.Zip;						
			newObj.ContactName = this.ContactName;						
			newObj.ContactPhone = this.ContactPhone;						
			newObj.ContactEmail = this.ContactEmail;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestaurantLocationBase.Property_Id, Id);				
			info.AddValue(RestaurantLocationBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestaurantLocationBase.Property_LocationName, LocationName);				
			info.AddValue(RestaurantLocationBase.Property_StreetAddress, StreetAddress);				
			info.AddValue(RestaurantLocationBase.Property_Address2, Address2);				
			info.AddValue(RestaurantLocationBase.Property_City, City);				
			info.AddValue(RestaurantLocationBase.Property_State, State);				
			info.AddValue(RestaurantLocationBase.Property_Zip, Zip);				
			info.AddValue(RestaurantLocationBase.Property_ContactName, ContactName);				
			info.AddValue(RestaurantLocationBase.Property_ContactPhone, ContactPhone);				
			info.AddValue(RestaurantLocationBase.Property_ContactEmail, ContactEmail);				
			info.AddValue(RestaurantLocationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestaurantLocationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestaurantLocationBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestaurantLocationBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
