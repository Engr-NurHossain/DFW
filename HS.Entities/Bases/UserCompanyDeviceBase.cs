using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "UserCompanyDeviceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class UserCompanyDeviceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			UserId = 2,
			DeviceId = 3,
			IsActive = 4,
			CreatedDate = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_DeviceId = "DeviceId";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _UserId;	            
		private String _DeviceId;	            
		private Boolean _IsActive;	            
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
		public String DeviceId
		{	
			get{ return _DeviceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DeviceId, value, _DeviceId);
				if (PropertyChanging(args))
				{
					_DeviceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsActive
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
		public  UserCompanyDeviceBase Clone()
		{
			UserCompanyDeviceBase newObj = new  UserCompanyDeviceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UserId = this.UserId;						
			newObj.DeviceId = this.DeviceId;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(UserCompanyDeviceBase.Property_Id, Id);				
			info.AddValue(UserCompanyDeviceBase.Property_CompanyId, CompanyId);				
			info.AddValue(UserCompanyDeviceBase.Property_UserId, UserId);				
			info.AddValue(UserCompanyDeviceBase.Property_DeviceId, DeviceId);				
			info.AddValue(UserCompanyDeviceBase.Property_IsActive, IsActive);				
			info.AddValue(UserCompanyDeviceBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
