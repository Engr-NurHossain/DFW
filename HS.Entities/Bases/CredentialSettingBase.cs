using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CredentialSettingBase", Namespace = "http://www.piistech.com//entities")]
	public class CredentialSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			AcountHolderId = 2,
			UserName = 3,
			Password = 4,
			Description = 5,
			IsActive = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_AcountHolderId = "AcountHolderId";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_Password = "Password";		            
		public const string Property_Description = "Description";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _AcountHolderId;	            
		private String _UserName;	            
		private String _Password;	            
		private String _Description;	            
		private Boolean _IsActive;	            
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
		public Int32 AcountHolderId
		{	
			get{ return _AcountHolderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AcountHolderId, value, _AcountHolderId);
				if (PropertyChanging(args))
				{
					_AcountHolderId = value;
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
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CredentialSettingBase Clone()
		{
			CredentialSettingBase newObj = new  CredentialSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.AcountHolderId = this.AcountHolderId;						
			newObj.UserName = this.UserName;						
			newObj.Password = this.Password;						
			newObj.Description = this.Description;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CredentialSettingBase.Property_Id, Id);				
			info.AddValue(CredentialSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(CredentialSettingBase.Property_AcountHolderId, AcountHolderId);				
			info.AddValue(CredentialSettingBase.Property_UserName, UserName);				
			info.AddValue(CredentialSettingBase.Property_Password, Password);				
			info.AddValue(CredentialSettingBase.Property_Description, Description);				
			info.AddValue(CredentialSettingBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
