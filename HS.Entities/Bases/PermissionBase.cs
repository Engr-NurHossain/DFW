using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PermissionBase", Namespace = "http://www.piistech.com//entities")]
	public class PermissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			ParentId = 2,
			Name = 3,
			DisplayText = 4,
			IsActive = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_ParentId = "ParentId";		            
		public const string Property_Name = "Name";		            
		public const string Property_DisplayText = "DisplayText";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Nullable<Int32> _ParentId;	            
		private String _Name;	            
		private String _DisplayText;	            
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
		public Nullable<Int32> ParentId
		{	
			get{ return _ParentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ParentId, value, _ParentId);
				if (PropertyChanging(args))
				{
					_ParentId = value;
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
		public String DisplayText
		{	
			get{ return _DisplayText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DisplayText, value, _DisplayText);
				if (PropertyChanging(args))
				{
					_DisplayText = value;
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
		public  PermissionBase Clone()
		{
			PermissionBase newObj = new  PermissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.ParentId = this.ParentId;						
			newObj.Name = this.Name;						
			newObj.DisplayText = this.DisplayText;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PermissionBase.Property_Id, Id);				
			info.AddValue(PermissionBase.Property_CompanyId, CompanyId);				
			info.AddValue(PermissionBase.Property_ParentId, ParentId);				
			info.AddValue(PermissionBase.Property_Name, Name);				
			info.AddValue(PermissionBase.Property_DisplayText, DisplayText);				
			info.AddValue(PermissionBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
