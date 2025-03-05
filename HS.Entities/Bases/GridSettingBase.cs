using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "GridSettingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class GridSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			ListKeyName = 2,
			SelectedColumn = 3,
			ColumnGroup = 4,
			GroupOrder = 5,
			OrderBy = 6,
			IsActive = 7,
			GridActive = 8,
			FormActive = 9,
			InputType = 10,
			IsFilter = 11,
			IsCustomerRequired = 12,
			IsLeadRequired = 13,
			IsCustomerLabel = 14,
			IsLeadLabel = 15
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_ListKeyName = "ListKeyName";		            
		public const string Property_SelectedColumn = "SelectedColumn";		            
		public const string Property_ColumnGroup = "ColumnGroup";		            
		public const string Property_GroupOrder = "GroupOrder";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_GridActive = "GridActive";		            
		public const string Property_FormActive = "FormActive";		            
		public const string Property_InputType = "InputType";		            
		public const string Property_IsFilter = "IsFilter";		            
		public const string Property_IsCustomerRequired = "IsCustomerRequired";		            
		public const string Property_IsLeadRequired = "IsLeadRequired";		            
		public const string Property_IsCustomerLabel = "IsCustomerLabel";		            
		public const string Property_IsLeadLabel = "IsLeadLabel";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _ListKeyName;	            
		private String _SelectedColumn;	            
		private String _ColumnGroup;	            
		private Nullable<Int32> _GroupOrder;	            
		private Int32 _OrderBy;	            
		private Boolean _IsActive;	            
		private Nullable<Boolean> _GridActive;	            
		private Nullable<Boolean> _FormActive;	            
		private String _InputType;	            
		private Nullable<Boolean> _IsFilter;	            
		private Nullable<Boolean> _IsCustomerRequired;	            
		private Nullable<Boolean> _IsLeadRequired;	            
		private Nullable<Boolean> _IsCustomerLabel;	            
		private Nullable<Boolean> _IsLeadLabel;	            
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
		public String ListKeyName
		{	
			get{ return _ListKeyName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ListKeyName, value, _ListKeyName);
				if (PropertyChanging(args))
				{
					_ListKeyName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SelectedColumn
		{	
			get{ return _SelectedColumn; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SelectedColumn, value, _SelectedColumn);
				if (PropertyChanging(args))
				{
					_SelectedColumn = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ColumnGroup
		{	
			get{ return _ColumnGroup; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ColumnGroup, value, _ColumnGroup);
				if (PropertyChanging(args))
				{
					_ColumnGroup = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> GroupOrder
		{	
			get{ return _GroupOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupOrder, value, _GroupOrder);
				if (PropertyChanging(args))
				{
					_GroupOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 OrderBy
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
		public Nullable<Boolean> GridActive
		{	
			get{ return _GridActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GridActive, value, _GridActive);
				if (PropertyChanging(args))
				{
					_GridActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> FormActive
		{	
			get{ return _FormActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FormActive, value, _FormActive);
				if (PropertyChanging(args))
				{
					_FormActive = value;
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
		public Nullable<Boolean> IsFilter
		{	
			get{ return _IsFilter; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFilter, value, _IsFilter);
				if (PropertyChanging(args))
				{
					_IsFilter = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCustomerRequired
		{	
			get{ return _IsCustomerRequired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCustomerRequired, value, _IsCustomerRequired);
				if (PropertyChanging(args))
				{
					_IsCustomerRequired = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsLeadRequired
		{	
			get{ return _IsLeadRequired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsLeadRequired, value, _IsLeadRequired);
				if (PropertyChanging(args))
				{
					_IsLeadRequired = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCustomerLabel
		{	
			get{ return _IsCustomerLabel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCustomerLabel, value, _IsCustomerLabel);
				if (PropertyChanging(args))
				{
					_IsCustomerLabel = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsLeadLabel
		{	
			get{ return _IsLeadLabel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsLeadLabel, value, _IsLeadLabel);
				if (PropertyChanging(args))
				{
					_IsLeadLabel = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  GridSettingBase Clone()
		{
			GridSettingBase newObj = new  GridSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.ListKeyName = this.ListKeyName;						
			newObj.SelectedColumn = this.SelectedColumn;						
			newObj.ColumnGroup = this.ColumnGroup;						
			newObj.GroupOrder = this.GroupOrder;						
			newObj.OrderBy = this.OrderBy;						
			newObj.IsActive = this.IsActive;						
			newObj.GridActive = this.GridActive;						
			newObj.FormActive = this.FormActive;						
			newObj.InputType = this.InputType;						
			newObj.IsFilter = this.IsFilter;						
			newObj.IsCustomerRequired = this.IsCustomerRequired;						
			newObj.IsLeadRequired = this.IsLeadRequired;						
			newObj.IsCustomerLabel = this.IsCustomerLabel;						
			newObj.IsLeadLabel = this.IsLeadLabel;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(GridSettingBase.Property_Id, Id);				
			info.AddValue(GridSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(GridSettingBase.Property_ListKeyName, ListKeyName);				
			info.AddValue(GridSettingBase.Property_SelectedColumn, SelectedColumn);				
			info.AddValue(GridSettingBase.Property_ColumnGroup, ColumnGroup);				
			info.AddValue(GridSettingBase.Property_GroupOrder, GroupOrder);				
			info.AddValue(GridSettingBase.Property_OrderBy, OrderBy);				
			info.AddValue(GridSettingBase.Property_IsActive, IsActive);				
			info.AddValue(GridSettingBase.Property_GridActive, GridActive);				
			info.AddValue(GridSettingBase.Property_FormActive, FormActive);				
			info.AddValue(GridSettingBase.Property_InputType, InputType);				
			info.AddValue(GridSettingBase.Property_IsFilter, IsFilter);				
			info.AddValue(GridSettingBase.Property_IsCustomerRequired, IsCustomerRequired);				
			info.AddValue(GridSettingBase.Property_IsLeadRequired, IsLeadRequired);				
			info.AddValue(GridSettingBase.Property_IsCustomerLabel, IsCustomerLabel);				
			info.AddValue(GridSettingBase.Property_IsLeadLabel, IsLeadLabel);				
		}
		#endregion

		
	}
}
