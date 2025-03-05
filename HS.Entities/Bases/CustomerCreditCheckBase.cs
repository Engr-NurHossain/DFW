using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCreditCheckBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerCreditCheckBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			FirstName = 2,
			LastName = 3,
			CreditAddress = 4,
			CreditZipCode = 5,
			CreditCity = 6,
			CreditState = 7,
			DateOfBirth = 8,
			SocialSecurityNumber = 9,
			CreditBureau = 10,
			CreditCheckDate = 11,
			CustomerName = 12,
			Score = 13,
			ProviderCreditRating = 14,
			Hit = 15,
			RepontPdfName = 16,
			ReportPdfLink = 17,
			CreatedBy = 18,
			TransectionId = 19,
			CompanyId = 20,
			CreditCheckDesc = 21
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_CreditAddress = "CreditAddress";		            
		public const string Property_CreditZipCode = "CreditZipCode";		            
		public const string Property_CreditCity = "CreditCity";		            
		public const string Property_CreditState = "CreditState";		            
		public const string Property_DateOfBirth = "DateOfBirth";		            
		public const string Property_SocialSecurityNumber = "SocialSecurityNumber";		            
		public const string Property_CreditBureau = "CreditBureau";		            
		public const string Property_CreditCheckDate = "CreditCheckDate";		            
		public const string Property_CustomerName = "CustomerName";		            
		public const string Property_Score = "Score";		            
		public const string Property_ProviderCreditRating = "ProviderCreditRating";		            
		public const string Property_Hit = "Hit";		            
		public const string Property_RepontPdfName = "RepontPdfName";		            
		public const string Property_ReportPdfLink = "ReportPdfLink";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_TransectionId = "TransectionId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CreditCheckDesc = "CreditCheckDesc";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _CreditAddress;	            
		private String _CreditZipCode;	            
		private String _CreditCity;	            
		private String _CreditState;	            
		private Nullable<DateTime> _DateOfBirth;	            
		private String _SocialSecurityNumber;	            
		private String _CreditBureau;	            
		private DateTime _CreditCheckDate;	            
		private String _CustomerName;	            
		private String _Score;	            
		private String _ProviderCreditRating;	            
		private String _Hit;	            
		private String _RepontPdfName;	            
		private String _ReportPdfLink;	            
		private Guid _CreatedBy;	            
		private String _TransectionId;	            
		private Guid _CompanyId;	            
		private String _CreditCheckDesc;	            
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
		public String CreditAddress
		{	
			get{ return _CreditAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditAddress, value, _CreditAddress);
				if (PropertyChanging(args))
				{
					_CreditAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditZipCode
		{	
			get{ return _CreditZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditZipCode, value, _CreditZipCode);
				if (PropertyChanging(args))
				{
					_CreditZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditCity
		{	
			get{ return _CreditCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditCity, value, _CreditCity);
				if (PropertyChanging(args))
				{
					_CreditCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditState
		{	
			get{ return _CreditState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditState, value, _CreditState);
				if (PropertyChanging(args))
				{
					_CreditState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DateOfBirth
		{	
			get{ return _DateOfBirth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateOfBirth, value, _DateOfBirth);
				if (PropertyChanging(args))
				{
					_DateOfBirth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SocialSecurityNumber
		{	
			get{ return _SocialSecurityNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SocialSecurityNumber, value, _SocialSecurityNumber);
				if (PropertyChanging(args))
				{
					_SocialSecurityNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditBureau
		{	
			get{ return _CreditBureau; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditBureau, value, _CreditBureau);
				if (PropertyChanging(args))
				{
					_CreditBureau = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreditCheckDate
		{	
			get{ return _CreditCheckDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditCheckDate, value, _CreditCheckDate);
				if (PropertyChanging(args))
				{
					_CreditCheckDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerName
		{	
			get{ return _CustomerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerName, value, _CustomerName);
				if (PropertyChanging(args))
				{
					_CustomerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Score
		{	
			get{ return _Score; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Score, value, _Score);
				if (PropertyChanging(args))
				{
					_Score = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProviderCreditRating
		{	
			get{ return _ProviderCreditRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProviderCreditRating, value, _ProviderCreditRating);
				if (PropertyChanging(args))
				{
					_ProviderCreditRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Hit
		{	
			get{ return _Hit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Hit, value, _Hit);
				if (PropertyChanging(args))
				{
					_Hit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RepontPdfName
		{	
			get{ return _RepontPdfName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepontPdfName, value, _RepontPdfName);
				if (PropertyChanging(args))
				{
					_RepontPdfName = value;
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
		public String TransectionId
		{	
			get{ return _TransectionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransectionId, value, _TransectionId);
				if (PropertyChanging(args))
				{
					_TransectionId = value;
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
		public String CreditCheckDesc
		{	
			get{ return _CreditCheckDesc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditCheckDesc, value, _CreditCheckDesc);
				if (PropertyChanging(args))
				{
					_CreditCheckDesc = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCreditCheckBase Clone()
		{
			CustomerCreditCheckBase newObj = new  CustomerCreditCheckBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.CreditAddress = this.CreditAddress;						
			newObj.CreditZipCode = this.CreditZipCode;						
			newObj.CreditCity = this.CreditCity;						
			newObj.CreditState = this.CreditState;						
			newObj.DateOfBirth = this.DateOfBirth;						
			newObj.SocialSecurityNumber = this.SocialSecurityNumber;						
			newObj.CreditBureau = this.CreditBureau;						
			newObj.CreditCheckDate = this.CreditCheckDate;						
			newObj.CustomerName = this.CustomerName;						
			newObj.Score = this.Score;						
			newObj.ProviderCreditRating = this.ProviderCreditRating;						
			newObj.Hit = this.Hit;						
			newObj.RepontPdfName = this.RepontPdfName;						
			newObj.ReportPdfLink = this.ReportPdfLink;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.TransectionId = this.TransectionId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CreditCheckDesc = this.CreditCheckDesc;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCreditCheckBase.Property_Id, Id);				
			info.AddValue(CustomerCreditCheckBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCreditCheckBase.Property_FirstName, FirstName);				
			info.AddValue(CustomerCreditCheckBase.Property_LastName, LastName);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditAddress, CreditAddress);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditZipCode, CreditZipCode);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditCity, CreditCity);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditState, CreditState);				
			info.AddValue(CustomerCreditCheckBase.Property_DateOfBirth, DateOfBirth);				
			info.AddValue(CustomerCreditCheckBase.Property_SocialSecurityNumber, SocialSecurityNumber);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditBureau, CreditBureau);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditCheckDate, CreditCheckDate);				
			info.AddValue(CustomerCreditCheckBase.Property_CustomerName, CustomerName);				
			info.AddValue(CustomerCreditCheckBase.Property_Score, Score);				
			info.AddValue(CustomerCreditCheckBase.Property_ProviderCreditRating, ProviderCreditRating);				
			info.AddValue(CustomerCreditCheckBase.Property_Hit, Hit);				
			info.AddValue(CustomerCreditCheckBase.Property_RepontPdfName, RepontPdfName);				
			info.AddValue(CustomerCreditCheckBase.Property_ReportPdfLink, ReportPdfLink);				
			info.AddValue(CustomerCreditCheckBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerCreditCheckBase.Property_TransectionId, TransectionId);				
			info.AddValue(CustomerCreditCheckBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerCreditCheckBase.Property_CreditCheckDesc, CreditCheckDesc);				
		}
		#endregion

		
	}
}
