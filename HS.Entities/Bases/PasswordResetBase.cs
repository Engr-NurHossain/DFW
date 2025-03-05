using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PasswordResetBase", Namespace = "http://www.piistech.com//entities")]
	public class PasswordResetBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			UserName = 2,
			PasswordSalt = 3,
			IsUsed = 4,
			ResetCounter = 5,
			UserIP = 6,
			UserAgent = 7,
			PageUrl = 8,
			IsActive = 9,
			LastUpdatedDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_PasswordSalt = "PasswordSalt";		            
		public const string Property_IsUsed = "IsUsed";		            
		public const string Property_ResetCounter = "ResetCounter";		            
		public const string Property_UserIP = "UserIP";		            
		public const string Property_UserAgent = "UserAgent";		            
		public const string Property_PageUrl = "PageUrl";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _UserName;	            
		private String _PasswordSalt;	            
		private Nullable<Boolean> _IsUsed;	            
		private Nullable<Int32> _ResetCounter;	            
		private String _UserIP;	            
		private String _UserAgent;	            
		private String _PageUrl;	            
		private Boolean _IsActive;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
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
		public String PasswordSalt
		{	
			get{ return _PasswordSalt; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PasswordSalt, value, _PasswordSalt);
				if (PropertyChanging(args))
				{
					_PasswordSalt = value;
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
		public Nullable<Int32> ResetCounter
		{	
			get{ return _ResetCounter; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResetCounter, value, _ResetCounter);
				if (PropertyChanging(args))
				{
					_ResetCounter = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserIP
		{	
			get{ return _UserIP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserIP, value, _UserIP);
				if (PropertyChanging(args))
				{
					_UserIP = value;
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
		public String PageUrl
		{	
			get{ return _PageUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PageUrl, value, _PageUrl);
				if (PropertyChanging(args))
				{
					_PageUrl = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  PasswordResetBase Clone()
		{
			PasswordResetBase newObj = new  PasswordResetBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UserName = this.UserName;						
			newObj.PasswordSalt = this.PasswordSalt;						
			newObj.IsUsed = this.IsUsed;						
			newObj.ResetCounter = this.ResetCounter;						
			newObj.UserIP = this.UserIP;						
			newObj.UserAgent = this.UserAgent;						
			newObj.PageUrl = this.PageUrl;						
			newObj.IsActive = this.IsActive;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PasswordResetBase.Property_Id, Id);				
			info.AddValue(PasswordResetBase.Property_CompanyId, CompanyId);				
			info.AddValue(PasswordResetBase.Property_UserName, UserName);				
			info.AddValue(PasswordResetBase.Property_PasswordSalt, PasswordSalt);				
			info.AddValue(PasswordResetBase.Property_IsUsed, IsUsed);				
			info.AddValue(PasswordResetBase.Property_ResetCounter, ResetCounter);				
			info.AddValue(PasswordResetBase.Property_UserIP, UserIP);				
			info.AddValue(PasswordResetBase.Property_UserAgent, UserAgent);				
			info.AddValue(PasswordResetBase.Property_PageUrl, PageUrl);				
			info.AddValue(PasswordResetBase.Property_IsActive, IsActive);				
			info.AddValue(PasswordResetBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
