using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerSpouseBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerSpouseBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			FirstName = 3,
			LastName = 4,
			DateofBirth = 5,
			SSN = 6,
			AddedDate = 7,
			CreditCheckDate = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_DateofBirth = "DateofBirth";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_CreditCheckDate = "CreditCheckDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private DateTime _DateofBirth;	            
		private String _SSN;	            
		private DateTime _AddedDate;	            
		private DateTime _CreditCheckDate;	            
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
		public DateTime DateofBirth
		{	
			get{ return _DateofBirth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateofBirth, value, _DateofBirth);
				if (PropertyChanging(args))
				{
					_DateofBirth = value;
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
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerSpouseBase Clone()
		{
			CustomerSpouseBase newObj = new  CustomerSpouseBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.DateofBirth = this.DateofBirth;						
			newObj.SSN = this.SSN;						
			newObj.AddedDate = this.AddedDate;						
			newObj.CreditCheckDate = this.CreditCheckDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerSpouseBase.Property_Id, Id);				
			info.AddValue(CustomerSpouseBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerSpouseBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerSpouseBase.Property_FirstName, FirstName);				
			info.AddValue(CustomerSpouseBase.Property_LastName, LastName);				
			info.AddValue(CustomerSpouseBase.Property_DateofBirth, DateofBirth);				
			info.AddValue(CustomerSpouseBase.Property_SSN, SSN);				
			info.AddValue(CustomerSpouseBase.Property_AddedDate, AddedDate);				
			info.AddValue(CustomerSpouseBase.Property_CreditCheckDate, CreditCheckDate);				
		}
		#endregion

		
	}
}
