using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmergencyContactDraftBase", Namespace = "http://www.piistech.com//entities")]
	public class EmergencyContactDraftBase : BaseBusinessEntity
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
			PhoneType = 10
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

		#endregion
		
		#region Cloning Base Objects
		public  EmergencyContactDraftBase Clone()
		{
			EmergencyContactDraftBase newObj = new  EmergencyContactDraftBase();
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
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmergencyContactDraftBase.Property_Id, Id);				
			info.AddValue(EmergencyContactDraftBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmergencyContactDraftBase.Property_CustomerId, CustomerId);				
			info.AddValue(EmergencyContactDraftBase.Property_CrossSteet, CrossSteet);				
			info.AddValue(EmergencyContactDraftBase.Property_FirstName, FirstName);				
			info.AddValue(EmergencyContactDraftBase.Property_LastName, LastName);				
			info.AddValue(EmergencyContactDraftBase.Property_RelationShip, RelationShip);				
			info.AddValue(EmergencyContactDraftBase.Property_Email, Email);				
			info.AddValue(EmergencyContactDraftBase.Property_Phone, Phone);				
			info.AddValue(EmergencyContactDraftBase.Property_HasKey, HasKey);				
			info.AddValue(EmergencyContactDraftBase.Property_PhoneType, PhoneType);				
		}
		#endregion

		
	}
}
