using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SupplierBase", Namespace = "http://www.piistech.com//entities")]
	public class SupplierBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SupplierId = 1,
			CompanyName = 2,
			CompanyId = 3,
			Name = 4,
			OrderBy = 5,
			ContactPerson = 6,
			Phone = 7,
			EmailAddress = 8,
			Street = 9,
			City = 10,
			State = 11,
			Zipcode = 12,
			Country = 13,
			IsActive = 14,
			Note = 15,
			Website = 16,
			TaxId = 17,
			SalesRepName = 18
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SupplierId = "SupplierId";		            
		public const string Property_CompanyName = "CompanyName";		            
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
		public const string Property_Note = "Note";		            
		public const string Property_Website = "Website";		            
		public const string Property_TaxId = "TaxId";		            
		public const string Property_SalesRepName = "SalesRepName";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SupplierId;	            
		private String _CompanyName;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private Nullable<Int32> _OrderBy;	            
		private Guid _ContactPerson;	            
		private String _Phone;	            
		private String _EmailAddress;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _Zipcode;	            
		private String _Country;	            
		private Boolean _IsActive;	            
		private String _Note;	            
		private String _Website;	            
		private String _TaxId;	            
		private String _SalesRepName;	            
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
		public Guid SupplierId
		{	
			get{ return _SupplierId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierId, value, _SupplierId);
				if (PropertyChanging(args))
				{
					_SupplierId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CompanyName
		{	
			get{ return _CompanyName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyName, value, _CompanyName);
				if (PropertyChanging(args))
				{
					_CompanyName = value;
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
		public Guid ContactPerson
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
		public Boolean IsActive
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
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Website
		{	
			get{ return _Website; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Website, value, _Website);
				if (PropertyChanging(args))
				{
					_Website = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TaxId
		{	
			get{ return _TaxId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxId, value, _TaxId);
				if (PropertyChanging(args))
				{
					_TaxId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SalesRepName
		{	
			get{ return _SalesRepName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesRepName, value, _SalesRepName);
				if (PropertyChanging(args))
				{
					_SalesRepName = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SupplierBase Clone()
		{
			SupplierBase newObj = new  SupplierBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SupplierId = this.SupplierId;						
			newObj.CompanyName = this.CompanyName;						
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
			newObj.Note = this.Note;						
			newObj.Website = this.Website;						
			newObj.TaxId = this.TaxId;						
			newObj.SalesRepName = this.SalesRepName;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SupplierBase.Property_Id, Id);				
			info.AddValue(SupplierBase.Property_SupplierId, SupplierId);				
			info.AddValue(SupplierBase.Property_CompanyName, CompanyName);				
			info.AddValue(SupplierBase.Property_CompanyId, CompanyId);				
			info.AddValue(SupplierBase.Property_Name, Name);				
			info.AddValue(SupplierBase.Property_OrderBy, OrderBy);				
			info.AddValue(SupplierBase.Property_ContactPerson, ContactPerson);				
			info.AddValue(SupplierBase.Property_Phone, Phone);				
			info.AddValue(SupplierBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(SupplierBase.Property_Street, Street);				
			info.AddValue(SupplierBase.Property_City, City);				
			info.AddValue(SupplierBase.Property_State, State);				
			info.AddValue(SupplierBase.Property_Zipcode, Zipcode);				
			info.AddValue(SupplierBase.Property_Country, Country);				
			info.AddValue(SupplierBase.Property_IsActive, IsActive);				
			info.AddValue(SupplierBase.Property_Note, Note);				
			info.AddValue(SupplierBase.Property_Website, Website);				
			info.AddValue(SupplierBase.Property_TaxId, TaxId);				
			info.AddValue(SupplierBase.Property_SalesRepName, SalesRepName);				
		}
		#endregion

		
	}
}
