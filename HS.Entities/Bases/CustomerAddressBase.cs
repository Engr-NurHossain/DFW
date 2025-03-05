using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAddressBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerAddressBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			RefId = 2,
			AddressType = 3,
			Street = 4,
			City = 5,
			State = 6,
			ZipCode = 7,
			County = 8,
			FirstName = 9,
			LastName = 10,
			BusinessName = 11,
			IsDefault = 12,
			Notes = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_RefId = "RefId";		            
		public const string Property_AddressType = "AddressType";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_County = "County";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_BusinessName = "BusinessName";		            
		public const string Property_IsDefault = "IsDefault";		            
		public const string Property_Notes = "Notes";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _RefId;	            
		private String _AddressType;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _County;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _BusinessName;	            
		private Nullable<Boolean> _IsDefault;	            
		private String _Notes;	            
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
		public String RefId
		{	
			get{ return _RefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RefId, value, _RefId);
				if (PropertyChanging(args))
				{
					_RefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressType
		{	
			get{ return _AddressType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressType, value, _AddressType);
				if (PropertyChanging(args))
				{
					_AddressType = value;
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
		public String ZipCode
		{	
			get{ return _ZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZipCode, value, _ZipCode);
				if (PropertyChanging(args))
				{
					_ZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String County
		{	
			get{ return _County; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_County, value, _County);
				if (PropertyChanging(args))
				{
					_County = value;
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
		public String BusinessName
		{	
			get{ return _BusinessName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BusinessName, value, _BusinessName);
				if (PropertyChanging(args))
				{
					_BusinessName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDefault
		{	
			get{ return _IsDefault; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefault, value, _IsDefault);
				if (PropertyChanging(args))
				{
					_IsDefault = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAddressBase Clone()
		{
			CustomerAddressBase newObj = new  CustomerAddressBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.RefId = this.RefId;						
			newObj.AddressType = this.AddressType;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.County = this.County;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.BusinessName = this.BusinessName;						
			newObj.IsDefault = this.IsDefault;						
			newObj.Notes = this.Notes;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAddressBase.Property_Id, Id);				
			info.AddValue(CustomerAddressBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerAddressBase.Property_RefId, RefId);				
			info.AddValue(CustomerAddressBase.Property_AddressType, AddressType);				
			info.AddValue(CustomerAddressBase.Property_Street, Street);				
			info.AddValue(CustomerAddressBase.Property_City, City);				
			info.AddValue(CustomerAddressBase.Property_State, State);				
			info.AddValue(CustomerAddressBase.Property_ZipCode, ZipCode);				
			info.AddValue(CustomerAddressBase.Property_County, County);				
			info.AddValue(CustomerAddressBase.Property_FirstName, FirstName);				
			info.AddValue(CustomerAddressBase.Property_LastName, LastName);				
			info.AddValue(CustomerAddressBase.Property_BusinessName, BusinessName);				
			info.AddValue(CustomerAddressBase.Property_IsDefault, IsDefault);				
			info.AddValue(CustomerAddressBase.Property_Notes, Notes);				
		}
		#endregion

		
	}
}
