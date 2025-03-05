using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeCommissionBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeCommissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			EmployeeCommissionId = 2,
			UserId = 3,
			CustomerId = 4,
			Amount = 5,
			CreatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EmployeeCommissionId = "EmployeeCommissionId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _EmployeeCommissionId;	            
		private Guid _UserId;	            
		private Guid _CustomerId;	            
		private Double _Amount;	            
		private DateTime _CreatedDate;	            
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
		public Guid EmployeeCommissionId
		{	
			get{ return _EmployeeCommissionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeCommissionId, value, _EmployeeCommissionId);
				if (PropertyChanging(args))
				{
					_EmployeeCommissionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
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
		public Double Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
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

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeCommissionBase Clone()
		{
			EmployeeCommissionBase newObj = new  EmployeeCommissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EmployeeCommissionId = this.EmployeeCommissionId;						
			newObj.UserId = this.UserId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Amount = this.Amount;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeCommissionBase.Property_Id, Id);				
			info.AddValue(EmployeeCommissionBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeCommissionBase.Property_EmployeeCommissionId, EmployeeCommissionId);				
			info.AddValue(EmployeeCommissionBase.Property_UserId, UserId);				
			info.AddValue(EmployeeCommissionBase.Property_CustomerId, CustomerId);				
			info.AddValue(EmployeeCommissionBase.Property_Amount, Amount);				
			info.AddValue(EmployeeCommissionBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
