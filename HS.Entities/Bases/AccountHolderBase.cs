using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AccountHolderBase", Namespace = "http://www.piistech.com//entities")]
	public class AccountHolderBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			IsActive = 3,
			InHouse = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_InHouse = "InHouse";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private Boolean _IsActive;	            
		private Boolean _InHouse;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
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
		public Boolean InHouse
		{	
			get{ return _InHouse; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InHouse, value, _InHouse);
				if (PropertyChanging(args))
				{
					_InHouse = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AccountHolderBase Clone()
		{
			AccountHolderBase newObj = new  AccountHolderBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.IsActive = this.IsActive;						
			newObj.InHouse = this.InHouse;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AccountHolderBase.Property_Id, Id);				
			info.AddValue(AccountHolderBase.Property_CompanyId, CompanyId);				
			info.AddValue(AccountHolderBase.Property_Name, Name);				
			info.AddValue(AccountHolderBase.Property_IsActive, IsActive);				
			info.AddValue(AccountHolderBase.Property_InHouse, InHouse);				
		}
		#endregion

		
	}
}
