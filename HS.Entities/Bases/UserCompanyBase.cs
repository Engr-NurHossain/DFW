using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "UserCompanyBase", Namespace = "http://www.piistech.com//entities")]
	public class UserCompanyBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			UserId = 2,
			IsDefault = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_IsDefault = "IsDefault";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _UserId;	            
		private Boolean _IsDefault;	            
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
		public Boolean IsDefault
		{	
			get{ return _IsDefault; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefault, value, _IsDefault);
				if (PropertyChanging(args))
				{
					_IsDefault = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  UserCompanyBase Clone()
		{
			UserCompanyBase newObj = new  UserCompanyBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UserId = this.UserId;						
			newObj.IsDefault = this.IsDefault;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(UserCompanyBase.Property_Id, Id);				
			info.AddValue(UserCompanyBase.Property_CompanyId, CompanyId);				
			info.AddValue(UserCompanyBase.Property_UserId, UserId);				
			info.AddValue(UserCompanyBase.Property_IsDefault, IsDefault);				
		}
		#endregion

		
	}
}
