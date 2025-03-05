using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "LookupBase", Namespace = "http://www.hims-tech.com//entities")]
	public class LookupBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ParentDataKey = 1,
			DataKey = 2,
			DisplayText = 3,
			DataValue = 4,
			DataOrder = 5,
			IsActive = 6,
			AlterDisplayText = 7,
			AlterDisplayText1 = 8,
			IsDefaultItem = 9,
			CompanyId = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ParentDataKey = "ParentDataKey";		            
		public const string Property_DataKey = "DataKey";		            
		public const string Property_DisplayText = "DisplayText";		            
		public const string Property_DataValue = "DataValue";		            
		public const string Property_DataOrder = "DataOrder";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_AlterDisplayText = "AlterDisplayText";		            
		public const string Property_AlterDisplayText1 = "AlterDisplayText1";		            
		public const string Property_IsDefaultItem = "IsDefaultItem";		            
		public const string Property_CompanyId = "CompanyId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _ParentDataKey;	            
		private String _DataKey;	            
		private String _DisplayText;	            
		private String _DataValue;	            
		private Nullable<Int32> _DataOrder;	            
		private Nullable<Boolean> _IsActive;	            
		private String _AlterDisplayText;	            
		private String _AlterDisplayText1;	            
		private Nullable<Boolean> _IsDefaultItem;	            
		private Guid _CompanyId;	            
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
		public String ParentDataKey
		{	
			get{ return _ParentDataKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ParentDataKey, value, _ParentDataKey);
				if (PropertyChanging(args))
				{
					_ParentDataKey = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DataKey
		{	
			get{ return _DataKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DataKey, value, _DataKey);
				if (PropertyChanging(args))
				{
					_DataKey = value;
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
		public String DataValue
		{	
			get{ return _DataValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DataValue, value, _DataValue);
				if (PropertyChanging(args))
				{
					_DataValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> DataOrder
		{	
			get{ return _DataOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DataOrder, value, _DataOrder);
				if (PropertyChanging(args))
				{
					_DataOrder = value;
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
		public String AlterDisplayText
		{	
			get{ return _AlterDisplayText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlterDisplayText, value, _AlterDisplayText);
				if (PropertyChanging(args))
				{
					_AlterDisplayText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AlterDisplayText1
		{	
			get{ return _AlterDisplayText1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlterDisplayText1, value, _AlterDisplayText1);
				if (PropertyChanging(args))
				{
					_AlterDisplayText1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDefaultItem
		{	
			get{ return _IsDefaultItem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefaultItem, value, _IsDefaultItem);
				if (PropertyChanging(args))
				{
					_IsDefaultItem = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  LookupBase Clone()
		{
			LookupBase newObj = new  LookupBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ParentDataKey = this.ParentDataKey;						
			newObj.DataKey = this.DataKey;						
			newObj.DisplayText = this.DisplayText;						
			newObj.DataValue = this.DataValue;						
			newObj.DataOrder = this.DataOrder;						
			newObj.IsActive = this.IsActive;						
			newObj.AlterDisplayText = this.AlterDisplayText;						
			newObj.AlterDisplayText1 = this.AlterDisplayText1;						
			newObj.IsDefaultItem = this.IsDefaultItem;						
			newObj.CompanyId = this.CompanyId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(LookupBase.Property_Id, Id);				
			info.AddValue(LookupBase.Property_ParentDataKey, ParentDataKey);				
			info.AddValue(LookupBase.Property_DataKey, DataKey);				
			info.AddValue(LookupBase.Property_DisplayText, DisplayText);				
			info.AddValue(LookupBase.Property_DataValue, DataValue);				
			info.AddValue(LookupBase.Property_DataOrder, DataOrder);				
			info.AddValue(LookupBase.Property_IsActive, IsActive);				
			info.AddValue(LookupBase.Property_AlterDisplayText, AlterDisplayText);				
			info.AddValue(LookupBase.Property_AlterDisplayText1, AlterDisplayText1);				
			info.AddValue(LookupBase.Property_IsDefaultItem, IsDefaultItem);				
			info.AddValue(LookupBase.Property_CompanyId, CompanyId);				
		}
		#endregion

		
	}
}
