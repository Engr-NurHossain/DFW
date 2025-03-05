using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "LocalizeResourceBase", Namespace = "http://www.piistech.com//entities")]
	public class LocalizeResourceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			LanguageId = 2,
			ResourceName = 3,
			ResourceValue = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_LanguageId = "LanguageId";		            
		public const string Property_ResourceName = "ResourceName";		            
		public const string Property_ResourceValue = "ResourceValue";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _LanguageId;	            
		private String _ResourceName;	            
		private String _ResourceValue;	            
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
		public Int32 LanguageId
		{	
			get{ return _LanguageId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LanguageId, value, _LanguageId);
				if (PropertyChanging(args))
				{
					_LanguageId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ResourceName
		{	
			get{ return _ResourceName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResourceName, value, _ResourceName);
				if (PropertyChanging(args))
				{
					_ResourceName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ResourceValue
		{	
			get{ return _ResourceValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResourceValue, value, _ResourceValue);
				if (PropertyChanging(args))
				{
					_ResourceValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  LocalizeResourceBase Clone()
		{
			LocalizeResourceBase newObj = new  LocalizeResourceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.LanguageId = this.LanguageId;						
			newObj.ResourceName = this.ResourceName;						
			newObj.ResourceValue = this.ResourceValue;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(LocalizeResourceBase.Property_Id, Id);				
			info.AddValue(LocalizeResourceBase.Property_CompanyId, CompanyId);				
			info.AddValue(LocalizeResourceBase.Property_LanguageId, LanguageId);				
			info.AddValue(LocalizeResourceBase.Property_ResourceName, ResourceName);				
			info.AddValue(LocalizeResourceBase.Property_ResourceValue, ResourceValue);				
		}
		#endregion

		
	}
}
