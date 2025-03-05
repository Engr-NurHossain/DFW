using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SecondaryCreditCheckContactBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SecondaryCreditCheckContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SecondaryContactId = 1,
			CustomerId = 2,
			FirstName = 3,
			LastName = 4,
			City = 5,
			State = 6,
			Zip = 7,
			Street = 8,
			SSN = 9,
			DateOfBirth = 10,
			IsUsed = 11,
			CreatedDate = 12,
			CreatedBy = 13,
			CreditScore = 14,
			Email = 15,
			Phone = 16,
			IsForSecondarySign = 17,
			ReportPdfLink = 18
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SecondaryContactId = "SecondaryContactId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zip = "Zip";		            
		public const string Property_Street = "Street";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_DateOfBirth = "DateOfBirth";		            
		public const string Property_IsUsed = "IsUsed";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreditScore = "CreditScore";		            
		public const string Property_Email = "Email";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_IsForSecondarySign = "IsForSecondarySign";		            
		public const string Property_ReportPdfLink = "ReportPdfLink";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SecondaryContactId;	            
		private Guid _CustomerId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _City;	            
		private String _State;	            
		private String _Zip;	            
		private String _Street;	            
		private String _SSN;	            
		private Nullable<DateTime> _DateOfBirth;	            
		private Nullable<Boolean> _IsUsed;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Nullable<Int32> _CreditScore;	            
		private String _Email;	            
		private String _Phone;	            
		private Nullable<Boolean> _IsForSecondarySign;	            
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
		public Guid SecondaryContactId
		{	
			get{ return _SecondaryContactId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondaryContactId, value, _SecondaryContactId);
				if (PropertyChanging(args))
				{
					_SecondaryContactId = value;
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
		public Nullable<Boolean> IsUsed
		{	
			get{ return _IsUsed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUsed, value, _IsUsed);
				if (PropertyChanging(args))
				{
					_IsUsed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
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
		public Nullable<Int32> CreditScore
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
		public Nullable<Boolean> IsForSecondarySign
		{	
			get{ return _IsForSecondarySign; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsForSecondarySign, value, _IsForSecondarySign);
				if (PropertyChanging(args))
				{
					_IsForSecondarySign = value;
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
		public  SecondaryCreditCheckContactBase Clone()
		{
			SecondaryCreditCheckContactBase newObj = new  SecondaryCreditCheckContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SecondaryContactId = this.SecondaryContactId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zip = this.Zip;						
			newObj.Street = this.Street;						
			newObj.SSN = this.SSN;						
			newObj.DateOfBirth = this.DateOfBirth;						
			newObj.IsUsed = this.IsUsed;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreditScore = this.CreditScore;						
			newObj.Email = this.Email;						
			newObj.Phone = this.Phone;						
			newObj.IsForSecondarySign = this.IsForSecondarySign;						
			newObj.ReportPdfLink = this.ReportPdfLink;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SecondaryCreditCheckContactBase.Property_Id, Id);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_SecondaryContactId, SecondaryContactId);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_CustomerId, CustomerId);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_FirstName, FirstName);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_LastName, LastName);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_City, City);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_State, State);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_Zip, Zip);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_Street, Street);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_SSN, SSN);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_DateOfBirth, DateOfBirth);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_IsUsed, IsUsed);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_CreditScore, CreditScore);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_Email, Email);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_Phone, Phone);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_IsForSecondarySign, IsForSecondarySign);				
			info.AddValue(SecondaryCreditCheckContactBase.Property_ReportPdfLink, ReportPdfLink);				
		}
		#endregion

		
	}
}
