using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAdditionalContactBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerAdditionalContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CrossSteet = 2,
			FirstName = 3,
			LastName = 4,
			RelationShip = 5,
			Email = 6,
			Phone = 7,
			PhoneType = 8,
			AltFirstName = 9,
			AltLastName = 10,
			DOB = 11,
			SSN = 12,
			ExternalID = 13,
			BillingContact = 14,
			BillingPhone = 15,
			BillingEmail = 16,
			BillingAddress = 17,
			BillingZipCode = 18,
			BillingCity = 19,
			BillingState = 20,
			Phone2 = 21,
			Phone2Type = 22,
			Phone3 = 23,
			Phone3Type = 24,
			CorpLegalEntityName = 25,
			CorpAddress = 26,
			CorpZipCode = 27,
			CorpCity = 28,
			CorpState = 29,
			PointContact = 30,
			AlternateContact = 31,
			AuthorizedUser = 32,
			IsEmergencyContact = 33,
			CreditScore = 34,
			IsCreditUsed = 35,
			IsSigningUsed = 36,
			ReportPdfLink = 37
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CrossSteet = "CrossSteet";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_RelationShip = "RelationShip";		            
		public const string Property_Email = "Email";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_PhoneType = "PhoneType";		            
		public const string Property_AltFirstName = "AltFirstName";		            
		public const string Property_AltLastName = "AltLastName";		            
		public const string Property_DOB = "DOB";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_ExternalID = "ExternalID";		            
		public const string Property_BillingContact = "BillingContact";		            
		public const string Property_BillingPhone = "BillingPhone";		            
		public const string Property_BillingEmail = "BillingEmail";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_BillingZipCode = "BillingZipCode";		            
		public const string Property_BillingCity = "BillingCity";		            
		public const string Property_BillingState = "BillingState";		            
		public const string Property_Phone2 = "Phone2";		            
		public const string Property_Phone2Type = "Phone2Type";		            
		public const string Property_Phone3 = "Phone3";		            
		public const string Property_Phone3Type = "Phone3Type";		            
		public const string Property_CorpLegalEntityName = "CorpLegalEntityName";		            
		public const string Property_CorpAddress = "CorpAddress";		            
		public const string Property_CorpZipCode = "CorpZipCode";		            
		public const string Property_CorpCity = "CorpCity";		            
		public const string Property_CorpState = "CorpState";		            
		public const string Property_PointContact = "PointContact";		            
		public const string Property_AlternateContact = "AlternateContact";		            
		public const string Property_AuthorizedUser = "AuthorizedUser";		            
		public const string Property_IsEmergencyContact = "IsEmergencyContact";		            
		public const string Property_CreditScore = "CreditScore";		            
		public const string Property_IsCreditUsed = "IsCreditUsed";		            
		public const string Property_IsSigningUsed = "IsSigningUsed";		            
		public const string Property_ReportPdfLink = "ReportPdfLink";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _CrossSteet;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _RelationShip;	            
		private String _Email;	            
		private String _Phone;	            
		private String _PhoneType;	            
		private String _AltFirstName;	            
		private String _AltLastName;	            
		private Nullable<DateTime> _DOB;	            
		private String _SSN;	            
		private String _ExternalID;	            
		private String _BillingContact;	            
		private String _BillingPhone;	            
		private String _BillingEmail;	            
		private String _BillingAddress;	            
		private String _BillingZipCode;	            
		private String _BillingCity;	            
		private String _BillingState;	            
		private String _Phone2;	            
		private String _Phone2Type;	            
		private String _Phone3;	            
		private String _Phone3Type;	            
		private String _CorpLegalEntityName;	            
		private String _CorpAddress;	            
		private String _CorpZipCode;	            
		private String _CorpCity;	            
		private String _CorpState;	            
		private Nullable<Boolean> _PointContact;	            
		private Nullable<Boolean> _AlternateContact;	            
		private Nullable<Boolean> _AuthorizedUser;	            
		private Nullable<Boolean> _IsEmergencyContact;	            
		private String _CreditScore;	            
		private Nullable<Boolean> _IsCreditUsed;	            
		private Nullable<Boolean> _IsSigningUsed;	            
		private String _ReportPdfLink;	            
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
		public String AltFirstName
		{	
			get{ return _AltFirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AltFirstName, value, _AltFirstName);
				if (PropertyChanging(args))
				{
					_AltFirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AltLastName
		{	
			get{ return _AltLastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AltLastName, value, _AltLastName);
				if (PropertyChanging(args))
				{
					_AltLastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DOB
		{	
			get{ return _DOB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DOB, value, _DOB);
				if (PropertyChanging(args))
				{
					_DOB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SSN
		{	
			get{ return _SSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SSN, value, _SSN);
				if (PropertyChanging(args))
				{
					_SSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExternalID
		{	
			get{ return _ExternalID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExternalID, value, _ExternalID);
				if (PropertyChanging(args))
				{
					_ExternalID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingContact
		{	
			get{ return _BillingContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingContact, value, _BillingContact);
				if (PropertyChanging(args))
				{
					_BillingContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingPhone
		{	
			get{ return _BillingPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingPhone, value, _BillingPhone);
				if (PropertyChanging(args))
				{
					_BillingPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingEmail
		{	
			get{ return _BillingEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingEmail, value, _BillingEmail);
				if (PropertyChanging(args))
				{
					_BillingEmail = value;
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
		public String BillingZipCode
		{	
			get{ return _BillingZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingZipCode, value, _BillingZipCode);
				if (PropertyChanging(args))
				{
					_BillingZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingCity
		{	
			get{ return _BillingCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingCity, value, _BillingCity);
				if (PropertyChanging(args))
				{
					_BillingCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingState
		{	
			get{ return _BillingState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingState, value, _BillingState);
				if (PropertyChanging(args))
				{
					_BillingState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone2
		{	
			get{ return _Phone2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone2, value, _Phone2);
				if (PropertyChanging(args))
				{
					_Phone2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone2Type
		{	
			get{ return _Phone2Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone2Type, value, _Phone2Type);
				if (PropertyChanging(args))
				{
					_Phone2Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone3
		{	
			get{ return _Phone3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone3, value, _Phone3);
				if (PropertyChanging(args))
				{
					_Phone3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone3Type
		{	
			get{ return _Phone3Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone3Type, value, _Phone3Type);
				if (PropertyChanging(args))
				{
					_Phone3Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CorpLegalEntityName
		{	
			get{ return _CorpLegalEntityName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CorpLegalEntityName, value, _CorpLegalEntityName);
				if (PropertyChanging(args))
				{
					_CorpLegalEntityName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CorpAddress
		{	
			get{ return _CorpAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CorpAddress, value, _CorpAddress);
				if (PropertyChanging(args))
				{
					_CorpAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CorpZipCode
		{	
			get{ return _CorpZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CorpZipCode, value, _CorpZipCode);
				if (PropertyChanging(args))
				{
					_CorpZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CorpCity
		{	
			get{ return _CorpCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CorpCity, value, _CorpCity);
				if (PropertyChanging(args))
				{
					_CorpCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CorpState
		{	
			get{ return _CorpState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CorpState, value, _CorpState);
				if (PropertyChanging(args))
				{
					_CorpState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> PointContact
		{	
			get{ return _PointContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PointContact, value, _PointContact);
				if (PropertyChanging(args))
				{
					_PointContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> AlternateContact
		{	
			get{ return _AlternateContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlternateContact, value, _AlternateContact);
				if (PropertyChanging(args))
				{
					_AlternateContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> AuthorizedUser
		{	
			get{ return _AuthorizedUser; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthorizedUser, value, _AuthorizedUser);
				if (PropertyChanging(args))
				{
					_AuthorizedUser = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEmergencyContact
		{	
			get{ return _IsEmergencyContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEmergencyContact, value, _IsEmergencyContact);
				if (PropertyChanging(args))
				{
					_IsEmergencyContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditScore
		{	
			get{ return _CreditScore; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditScore, value, _CreditScore);
				if (PropertyChanging(args))
				{
					_CreditScore = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCreditUsed
		{	
			get{ return _IsCreditUsed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCreditUsed, value, _IsCreditUsed);
				if (PropertyChanging(args))
				{
					_IsCreditUsed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSigningUsed
		{	
			get{ return _IsSigningUsed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSigningUsed, value, _IsSigningUsed);
				if (PropertyChanging(args))
				{
					_IsSigningUsed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReportPdfLink
		{	
			get{ return _ReportPdfLink; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReportPdfLink, value, _ReportPdfLink);
				if (PropertyChanging(args))
				{
					_ReportPdfLink = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAdditionalContactBase Clone()
		{
			CustomerAdditionalContactBase newObj = new  CustomerAdditionalContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CrossSteet = this.CrossSteet;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.RelationShip = this.RelationShip;						
			newObj.Email = this.Email;						
			newObj.Phone = this.Phone;						
			newObj.PhoneType = this.PhoneType;						
			newObj.AltFirstName = this.AltFirstName;						
			newObj.AltLastName = this.AltLastName;						
			newObj.DOB = this.DOB;						
			newObj.SSN = this.SSN;						
			newObj.ExternalID = this.ExternalID;						
			newObj.BillingContact = this.BillingContact;						
			newObj.BillingPhone = this.BillingPhone;						
			newObj.BillingEmail = this.BillingEmail;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.BillingZipCode = this.BillingZipCode;						
			newObj.BillingCity = this.BillingCity;						
			newObj.BillingState = this.BillingState;						
			newObj.Phone2 = this.Phone2;						
			newObj.Phone2Type = this.Phone2Type;						
			newObj.Phone3 = this.Phone3;						
			newObj.Phone3Type = this.Phone3Type;						
			newObj.CorpLegalEntityName = this.CorpLegalEntityName;						
			newObj.CorpAddress = this.CorpAddress;						
			newObj.CorpZipCode = this.CorpZipCode;						
			newObj.CorpCity = this.CorpCity;						
			newObj.CorpState = this.CorpState;						
			newObj.PointContact = this.PointContact;						
			newObj.AlternateContact = this.AlternateContact;						
			newObj.AuthorizedUser = this.AuthorizedUser;						
			newObj.IsEmergencyContact = this.IsEmergencyContact;						
			newObj.CreditScore = this.CreditScore;						
			newObj.IsCreditUsed = this.IsCreditUsed;						
			newObj.IsSigningUsed = this.IsSigningUsed;						
			newObj.ReportPdfLink = this.ReportPdfLink;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAdditionalContactBase.Property_Id, Id);				
			info.AddValue(CustomerAdditionalContactBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerAdditionalContactBase.Property_CrossSteet, CrossSteet);				
			info.AddValue(CustomerAdditionalContactBase.Property_FirstName, FirstName);				
			info.AddValue(CustomerAdditionalContactBase.Property_LastName, LastName);				
			info.AddValue(CustomerAdditionalContactBase.Property_RelationShip, RelationShip);				
			info.AddValue(CustomerAdditionalContactBase.Property_Email, Email);				
			info.AddValue(CustomerAdditionalContactBase.Property_Phone, Phone);				
			info.AddValue(CustomerAdditionalContactBase.Property_PhoneType, PhoneType);				
			info.AddValue(CustomerAdditionalContactBase.Property_AltFirstName, AltFirstName);				
			info.AddValue(CustomerAdditionalContactBase.Property_AltLastName, AltLastName);				
			info.AddValue(CustomerAdditionalContactBase.Property_DOB, DOB);				
			info.AddValue(CustomerAdditionalContactBase.Property_SSN, SSN);				
			info.AddValue(CustomerAdditionalContactBase.Property_ExternalID, ExternalID);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingContact, BillingContact);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingPhone, BillingPhone);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingEmail, BillingEmail);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingZipCode, BillingZipCode);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingCity, BillingCity);				
			info.AddValue(CustomerAdditionalContactBase.Property_BillingState, BillingState);				
			info.AddValue(CustomerAdditionalContactBase.Property_Phone2, Phone2);				
			info.AddValue(CustomerAdditionalContactBase.Property_Phone2Type, Phone2Type);				
			info.AddValue(CustomerAdditionalContactBase.Property_Phone3, Phone3);				
			info.AddValue(CustomerAdditionalContactBase.Property_Phone3Type, Phone3Type);				
			info.AddValue(CustomerAdditionalContactBase.Property_CorpLegalEntityName, CorpLegalEntityName);				
			info.AddValue(CustomerAdditionalContactBase.Property_CorpAddress, CorpAddress);				
			info.AddValue(CustomerAdditionalContactBase.Property_CorpZipCode, CorpZipCode);				
			info.AddValue(CustomerAdditionalContactBase.Property_CorpCity, CorpCity);				
			info.AddValue(CustomerAdditionalContactBase.Property_CorpState, CorpState);				
			info.AddValue(CustomerAdditionalContactBase.Property_PointContact, PointContact);				
			info.AddValue(CustomerAdditionalContactBase.Property_AlternateContact, AlternateContact);				
			info.AddValue(CustomerAdditionalContactBase.Property_AuthorizedUser, AuthorizedUser);				
			info.AddValue(CustomerAdditionalContactBase.Property_IsEmergencyContact, IsEmergencyContact);				
			info.AddValue(CustomerAdditionalContactBase.Property_CreditScore, CreditScore);				
			info.AddValue(CustomerAdditionalContactBase.Property_IsCreditUsed, IsCreditUsed);				
			info.AddValue(CustomerAdditionalContactBase.Property_IsSigningUsed, IsSigningUsed);				
			info.AddValue(CustomerAdditionalContactBase.Property_ReportPdfLink, ReportPdfLink);				
		}
		#endregion

		
	}
}
