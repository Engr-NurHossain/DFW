using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "UserLoginBase", Namespace = "http://www.hims-tech.com//entities")]
	public class UserLoginBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			UserName = 2,
			Password = 3,
			EmailAddress = 4,
			IsActive = 5,
			IsDeleted = 6,
			LastUpdatedBy = 7,
			LastUpdatedDate = 8,
			CompanyId = 9,
			PasswordResetDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_Password = "Password";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PasswordResetDate = "PasswordResetDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private String _UserName;	            
		private String _Password;	            
		private String _EmailAddress;	            
		private Boolean _IsActive;	            
		private Boolean _IsDeleted;	            
		private String _LastUpdatedBy;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private Guid _CompanyId;	            
		private Nullable<DateTime> _PasswordResetDate;	            
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
		public String UserName
		{	
			get{ return _UserName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserName, value, _UserName);
				if (PropertyChanging(args))
				{
					_UserName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Password
		{	
			get{ return _Password; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Password, value, _Password);
				if (PropertyChanging(args))
				{
					_Password = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailAddress
		{	
			get{ return _EmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailAddress, value, _EmailAddress);
				if (PropertyChanging(args))
				{
					_EmailAddress = value;
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
		public Boolean IsDeleted
		{	
			get{ return _IsDeleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDeleted, value, _IsDeleted);
				if (PropertyChanging(args))
				{
					_IsDeleted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
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
		public Nullable<DateTime> PasswordResetDate
		{	
			get{ return _PasswordResetDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PasswordResetDate, value, _PasswordResetDate);
				if (PropertyChanging(args))
				{
					_PasswordResetDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  UserLoginBase Clone()
		{
			UserLoginBase newObj = new  UserLoginBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.UserName = this.UserName;						
			newObj.Password = this.Password;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.IsActive = this.IsActive;						
			newObj.IsDeleted = this.IsDeleted;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PasswordResetDate = this.PasswordResetDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(UserLoginBase.Property_Id, Id);				
			info.AddValue(UserLoginBase.Property_UserId, UserId);				
			info.AddValue(UserLoginBase.Property_UserName, UserName);				
			info.AddValue(UserLoginBase.Property_Password, Password);				
			info.AddValue(UserLoginBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(UserLoginBase.Property_IsActive, IsActive);				
			info.AddValue(UserLoginBase.Property_IsDeleted, IsDeleted);				
			info.AddValue(UserLoginBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(UserLoginBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(UserLoginBase.Property_CompanyId, CompanyId);				
			info.AddValue(UserLoginBase.Property_PasswordResetDate, PasswordResetDate);				
		}
		#endregion

		
	}
}
