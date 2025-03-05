using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "GlobalSettingBase", Namespace = "http://www.piistech.com//entities")]
	public class GlobalSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			SearchKey = 2,
			Value = 3,
			IsActive = 4,
			Tag = 5,
			InputType = 6,
			Description = 7,
			OrderBy = 8,
			OptionalValue = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SearchKey = "SearchKey";		            
		public const string Property_Value = "Value";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_Tag = "Tag";		            
		public const string Property_InputType = "InputType";		            
		public const string Property_Description = "Description";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_OptionalValue = "OptionalValue";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _SearchKey;	            
		private String _Value;	            
		private Nullable<Boolean> _IsActive;	            
		private String _Tag;	            
		private String _InputType;	            
		private String _Description;	            
		private Nullable<Int32> _OrderBy;	            
		private String _OptionalValue;	            
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
		public String SearchKey
		{	
			get{ return _SearchKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchKey, value, _SearchKey);
				if (PropertyChanging(args))
				{
					_SearchKey = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Value
		{	
			get{ return _Value; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Value, value, _Value);
				if (PropertyChanging(args))
				{
					_Value = value;
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

		[DataMember]
		public String Tag
		{	
			get{ return _Tag; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tag, value, _Tag);
				if (PropertyChanging(args))
				{
					_Tag = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InputType
		{	
			get{ return _InputType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InputType, value, _InputType);
				if (PropertyChanging(args))
				{
					_InputType = value;
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
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OptionalValue
		{	
			get{ return _OptionalValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OptionalValue, value, _OptionalValue);
				if (PropertyChanging(args))
				{
					_OptionalValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  GlobalSettingBase Clone()
		{
			GlobalSettingBase newObj = new  GlobalSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SearchKey = this.SearchKey;						
			newObj.Value = this.Value;						
			newObj.IsActive = this.IsActive;						
			newObj.Tag = this.Tag;						
			newObj.InputType = this.InputType;						
			newObj.Description = this.Description;						
			newObj.OrderBy = this.OrderBy;						
			newObj.OptionalValue = this.OptionalValue;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(GlobalSettingBase.Property_Id, Id);				
			info.AddValue(GlobalSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(GlobalSettingBase.Property_SearchKey, SearchKey);				
			info.AddValue(GlobalSettingBase.Property_Value, Value);				
			info.AddValue(GlobalSettingBase.Property_IsActive, IsActive);				
			info.AddValue(GlobalSettingBase.Property_Tag, Tag);				
			info.AddValue(GlobalSettingBase.Property_InputType, InputType);				
			info.AddValue(GlobalSettingBase.Property_Description, Description);				
			info.AddValue(GlobalSettingBase.Property_OrderBy, OrderBy);				
			info.AddValue(GlobalSettingBase.Property_OptionalValue, OptionalValue);				
		}
		#endregion

		
	}
}
