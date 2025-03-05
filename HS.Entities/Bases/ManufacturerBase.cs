using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ManufacturerBase", Namespace = "http://www.piistech.com//entities")]
	public class ManufacturerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			OrderBy = 3,
			ContactPerson = 4,
			Phone = 5,
			EmailAddress = 6,
			Street = 7,
			City = 8,
			State = 9,
			Zipcode = 10,
			Country = 11,
			IsActive = 12,
			ManufacturerId = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_ContactPerson = "ContactPerson";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zipcode = "Zipcode";		            
		public const string Property_Country = "Country";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private Nullable<Int32> _OrderBy;	            
		private String _ContactPerson;	            
		private String _Phone;	            
		private String _EmailAddress;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _Zipcode;	            
		private String _Country;	            
		private Nullable<Boolean> _IsActive;	            
		private Guid _ManufacturerId;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactPerson
		{	
			get{ return _ContactPerson; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactPerson, value, _ContactPerson);
				if (PropertyChanging(args))
				{
					_ContactPerson = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone
		{	
			get{ return _Phone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone, value, _Phone);
				if (PropertyChanging(args))
				{
					_Phone = value;
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
		public String Street
		{	
			get{ return _Street; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Street, value, _Street);
				if (PropertyChanging(args))
				{
					_Street = value;
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
		public String Zipcode
		{	
			get{ return _Zipcode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zipcode, value, _Zipcode);
				if (PropertyChanging(args))
				{
					_Zipcode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Country
		{	
			get{ return _Country; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Country, value, _Country);
				if (PropertyChanging(args))
				{
					_Country = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ManufacturerId
		{	
			get{ return _ManufacturerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManufacturerId, value, _ManufacturerId);
				if (PropertyChanging(args))
				{
					_ManufacturerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ManufacturerBase Clone()
		{
			ManufacturerBase newObj = new  ManufacturerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.OrderBy = this.OrderBy;						
			newObj.ContactPerson = this.ContactPerson;						
			newObj.Phone = this.Phone;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zipcode = this.Zipcode;						
			newObj.Country = this.Country;						
			newObj.IsActive = this.IsActive;						
			newObj.ManufacturerId = this.ManufacturerId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ManufacturerBase.Property_Id, Id);				
			info.AddValue(ManufacturerBase.Property_CompanyId, CompanyId);				
			info.AddValue(ManufacturerBase.Property_Name, Name);				
			info.AddValue(ManufacturerBase.Property_OrderBy, OrderBy);				
			info.AddValue(ManufacturerBase.Property_ContactPerson, ContactPerson);				
			info.AddValue(ManufacturerBase.Property_Phone, Phone);				
			info.AddValue(ManufacturerBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(ManufacturerBase.Property_Street, Street);				
			info.AddValue(ManufacturerBase.Property_City, City);				
			info.AddValue(ManufacturerBase.Property_State, State);				
			info.AddValue(ManufacturerBase.Property_Zipcode, Zipcode);				
			info.AddValue(ManufacturerBase.Property_Country, Country);				
			info.AddValue(ManufacturerBase.Property_IsActive, IsActive);				
			info.AddValue(ManufacturerBase.Property_ManufacturerId, ManufacturerId);				
		}
		#endregion

		
	}
}
