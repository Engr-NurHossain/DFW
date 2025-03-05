using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ThirdPartyEmergencyContactBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ThirdPartyEmergencyContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			FirstName = 2,
			LastName = 3,
			Relation = 4,
			Phone = 5,
			Email = 6,
			ContactNo = 7,
			CtacLink = 8,
			Platform = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_Relation = "Relation";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_Email = "Email";		            
		public const string Property_ContactNo = "ContactNo";		            
		public const string Property_CtacLink = "CtacLink";		            
		public const string Property_Platform = "Platform";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _Relation;	            
		private String _Phone;	            
		private String _Email;	            
		private String _ContactNo;	            
		private String _CtacLink;	            
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
		public String Relation
		{	
			get{ return _Relation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Relation, value, _Relation);
				if (PropertyChanging(args))
				{
					_Relation = value;
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
		public String CtacLink
		{	
			get{ return _CtacLink; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CtacLink, value, _CtacLink);
				if (PropertyChanging(args))
				{
					_CtacLink = value;
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
		public  ThirdPartyEmergencyContactBase Clone()
		{
			ThirdPartyEmergencyContactBase newObj = new  ThirdPartyEmergencyContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.Relation = this.Relation;						
			newObj.Phone = this.Phone;						
			newObj.Email = this.Email;						
			newObj.ContactNo = this.ContactNo;						
			newObj.CtacLink = this.CtacLink;						
			newObj.Platform = this.Platform;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ThirdPartyEmergencyContactBase.Property_Id, Id);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_CustomerId, CustomerId);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_FirstName, FirstName);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_LastName, LastName);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_Relation, Relation);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_Phone, Phone);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_Email, Email);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_ContactNo, ContactNo);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_CtacLink, CtacLink);				
			info.AddValue(ThirdPartyEmergencyContactBase.Property_Platform, Platform);				
		}
		#endregion

		
	}
}
