using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAgreementBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerAgreementBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			InvoiceId = 3,
			IP = 4,
			UserAgent = 5,
			Type = 6,
			AddedDate = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_IP = "IP";		            
		public const string Property_UserAgent = "UserAgent";		            
		public const string Property_Type = "Type";		            
		public const string Property_AddedDate = "AddedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _InvoiceId;	            
		private String _IP;	            
		private String _UserAgent;	            
		private String _Type;	            
		private Nullable<DateTime> _AddedDate;	            
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
		public String InvoiceId
		{	
			get{ return _InvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceId, value, _InvoiceId);
				if (PropertyChanging(args))
				{
					_InvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IP
		{	
			get{ return _IP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IP, value, _IP);
				if (PropertyChanging(args))
				{
					_IP = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserAgent
		{	
			get{ return _UserAgent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserAgent, value, _UserAgent);
				if (PropertyChanging(args))
				{
					_UserAgent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AddedDate
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAgreementBase Clone()
		{
			CustomerAgreementBase newObj = new  CustomerAgreementBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.IP = this.IP;						
			newObj.UserAgent = this.UserAgent;						
			newObj.Type = this.Type;						
			newObj.AddedDate = this.AddedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAgreementBase.Property_Id, Id);				
			info.AddValue(CustomerAgreementBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerAgreementBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerAgreementBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(CustomerAgreementBase.Property_IP, IP);				
			info.AddValue(CustomerAgreementBase.Property_UserAgent, UserAgent);				
			info.AddValue(CustomerAgreementBase.Property_Type, Type);				
			info.AddValue(CustomerAgreementBase.Property_AddedDate, AddedDate);				
		}
		#endregion

		
	}
}
