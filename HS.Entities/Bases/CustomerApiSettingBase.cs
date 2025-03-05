using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerApiSettingBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerApiSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			AccountName = 3,
			Url = 4,
			UserName = 5,
			Password = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_AccountName = "AccountName";		            
		public const string Property_Url = "Url";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_Password = "Password";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _AccountName;	            
		private String _Url;	            
		private String _UserName;	            
		private String _Password;	            
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
		public String AccountName
		{	
			get{ return _AccountName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountName, value, _AccountName);
				if (PropertyChanging(args))
				{
					_AccountName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Url
		{	
			get{ return _Url; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Url, value, _Url);
				if (PropertyChanging(args))
				{
					_Url = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerApiSettingBase Clone()
		{
			CustomerApiSettingBase newObj = new  CustomerApiSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.AccountName = this.AccountName;						
			newObj.Url = this.Url;						
			newObj.UserName = this.UserName;						
			newObj.Password = this.Password;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerApiSettingBase.Property_Id, Id);				
			info.AddValue(CustomerApiSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerApiSettingBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerApiSettingBase.Property_AccountName, AccountName);				
			info.AddValue(CustomerApiSettingBase.Property_Url, Url);				
			info.AddValue(CustomerApiSettingBase.Property_UserName, UserName);				
			info.AddValue(CustomerApiSettingBase.Property_Password, Password);				
		}
		#endregion

		
	}
}
