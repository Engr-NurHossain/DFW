using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmergencyContactBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EmergencyContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			CrossSteet = 3,
			FirstName = 4,
			LastName = 5,
			RelationShip = 6,
			Email = 7,
			Phone = 8,
			HasKey = 9,
			PhoneType = 10,
			OrderBy = 11,
			ContactNo = 12,
			Platform = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CrossSteet = "CrossSteet";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_RelationShip = "RelationShip";		            
		public const string Property_Email = "Email";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_HasKey = "HasKey";		            
		public const string Property_PhoneType = "PhoneType";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_ContactNo = "ContactNo";		            
		public const string Property_Platform = "Platform";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _CrossSteet;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _RelationShip;	            
		private String _Email;	            
		private String _Phone;	            
		private String _HasKey;	            
		private String _PhoneType;	            
		private Nullable<Int32> _OrderBy;	            
		private String _ContactNo;	            
		private String _Platform;	            
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
		public String CrossSteet
		{	
			get{ return _CrossSteet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CrossSteet, value, _CrossSteet);
				if (PropertyChanging(args))
				{
					_CrossSteet = value;
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
		public String RelationShip
		{	
			get{ return _RelationShip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RelationShip, value, _RelationShip);
				if (PropertyChanging(args))
				{
					_RelationShip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Email
		{	
			get{ return _Email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Email, value, _Email);
				if (PropertyChanging(args))
				{
					_Email = value;
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
		public String HasKey
		{	
			get{ return _HasKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HasKey, value, _HasKey);
				if (PropertyChanging(args))
				{
					_HasKey = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PhoneType
		{	
			get{ return _PhoneType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PhoneType, value, _PhoneType);
				if (PropertyChanging(args))
				{
					_PhoneType = value;
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
		public String Platform
		{	
			get{ return _Platform; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Platform, value, _Platform);
				if (PropertyChanging(args))
				{
					_Platform = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmergencyContactBase Clone()
		{
			EmergencyContactBase newObj = new  EmergencyContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CrossSteet = this.CrossSteet;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.RelationShip = this.RelationShip;						
			newObj.Email = this.Email;						
			newObj.Phone = this.Phone;						
			newObj.HasKey = this.HasKey;						
			newObj.PhoneType = this.PhoneType;						
			newObj.OrderBy = this.OrderBy;						
			newObj.ContactNo = this.ContactNo;						
			newObj.Platform = this.Platform;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmergencyContactBase.Property_Id, Id);				
			info.AddValue(EmergencyContactBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmergencyContactBase.Property_CustomerId, CustomerId);				
			info.AddValue(EmergencyContactBase.Property_CrossSteet, CrossSteet);				
			info.AddValue(EmergencyContactBase.Property_FirstName, FirstName);				
			info.AddValue(EmergencyContactBase.Property_LastName, LastName);				
			info.AddValue(EmergencyContactBase.Property_RelationShip, RelationShip);				
			info.AddValue(EmergencyContactBase.Property_Email, Email);				
			info.AddValue(EmergencyContactBase.Property_Phone, Phone);				
			info.AddValue(EmergencyContactBase.Property_HasKey, HasKey);				
			info.AddValue(EmergencyContactBase.Property_PhoneType, PhoneType);				
			info.AddValue(EmergencyContactBase.Property_OrderBy, OrderBy);				
			info.AddValue(EmergencyContactBase.Property_ContactNo, ContactNo);				
			info.AddValue(EmergencyContactBase.Property_Platform, Platform);				
		}
		#endregion

		
	}
}
