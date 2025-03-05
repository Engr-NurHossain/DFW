using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCompanyBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerCompanyBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			IsLead = 3,
			ConvertionDate = 4,
			ConvertionType = 5,
			IsActive = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsLead = "IsLead";		            
		public const string Property_ConvertionDate = "ConvertionDate";		            
		public const string Property_ConvertionType = "ConvertionType";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private Boolean _IsLead;	            
		private Nullable<DateTime> _ConvertionDate;	            
		private String _ConvertionType;	            
		private Nullable<Boolean> _IsActive;	            
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
		public Boolean IsLead
		{	
			get{ return _IsLead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsLead, value, _IsLead);
				if (PropertyChanging(args))
				{
					_IsLead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ConvertionDate
		{	
			get{ return _ConvertionDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ConvertionDate, value, _ConvertionDate);
				if (PropertyChanging(args))
				{
					_ConvertionDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ConvertionType
		{	
			get{ return _ConvertionType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ConvertionType, value, _ConvertionType);
				if (PropertyChanging(args))
				{
					_ConvertionType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCompanyBase Clone()
		{
			CustomerCompanyBase newObj = new  CustomerCompanyBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsLead = this.IsLead;						
			newObj.ConvertionDate = this.ConvertionDate;						
			newObj.ConvertionType = this.ConvertionType;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCompanyBase.Property_Id, Id);				
			info.AddValue(CustomerCompanyBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCompanyBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerCompanyBase.Property_IsLead, IsLead);				
			info.AddValue(CustomerCompanyBase.Property_ConvertionDate, ConvertionDate);				
			info.AddValue(CustomerCompanyBase.Property_ConvertionType, ConvertionType);				
			info.AddValue(CustomerCompanyBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
