using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "FormGeneratorBase", Namespace = "http://www.piistech.com//entities")]
	public class FormGeneratorBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			FormName = 2,
			FieldLabel = 3,
			FieldType = 4,
			FieldName = 5,
			DataType = 6,
			DataKey = 7,
			Placeholder = 8,
			OrderBy = 9,
			IsActive = 10,
			IsRequired = 11,
			ErrorMessage = 12,
			ErrorMessage2 = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_FormName = "FormName";		            
		public const string Property_FieldLabel = "FieldLabel";		            
		public const string Property_FieldType = "FieldType";		            
		public const string Property_FieldName = "FieldName";		            
		public const string Property_DataType = "DataType";		            
		public const string Property_DataKey = "DataKey";		            
		public const string Property_Placeholder = "Placeholder";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_IsRequired = "IsRequired";		            
		public const string Property_ErrorMessage = "ErrorMessage";		            
		public const string Property_ErrorMessage2 = "ErrorMessage2";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _FormName;	            
		private String _FieldLabel;	            
		private String _FieldType;	            
		private String _FieldName;	            
		private String _DataType;	            
		private String _DataKey;	            
		private String _Placeholder;	            
		private Int32 _OrderBy;	            
		private Boolean _IsActive;	            
		private Boolean _IsRequired;	            
		private String _ErrorMessage;	            
		private String _ErrorMessage2;	            
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
		public String FormName
		{	
			get{ return _FormName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FormName, value, _FormName);
				if (PropertyChanging(args))
				{
					_FormName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FieldLabel
		{	
			get{ return _FieldLabel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FieldLabel, value, _FieldLabel);
				if (PropertyChanging(args))
				{
					_FieldLabel = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FieldType
		{	
			get{ return _FieldType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FieldType, value, _FieldType);
				if (PropertyChanging(args))
				{
					_FieldType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FieldName
		{	
			get{ return _FieldName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FieldName, value, _FieldName);
				if (PropertyChanging(args))
				{
					_FieldName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DataType
		{	
			get{ return _DataType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DataType, value, _DataType);
				if (PropertyChanging(args))
				{
					_DataType = value;
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
		public String Placeholder
		{	
			get{ return _Placeholder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Placeholder, value, _Placeholder);
				if (PropertyChanging(args))
				{
					_Placeholder = value;
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
		public Boolean IsRequired
		{	
			get{ return _IsRequired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRequired, value, _IsRequired);
				if (PropertyChanging(args))
				{
					_IsRequired = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ErrorMessage
		{	
			get{ return _ErrorMessage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ErrorMessage, value, _ErrorMessage);
				if (PropertyChanging(args))
				{
					_ErrorMessage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ErrorMessage2
		{	
			get{ return _ErrorMessage2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ErrorMessage2, value, _ErrorMessage2);
				if (PropertyChanging(args))
				{
					_ErrorMessage2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  FormGeneratorBase Clone()
		{
			FormGeneratorBase newObj = new  FormGeneratorBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.FormName = this.FormName;						
			newObj.FieldLabel = this.FieldLabel;						
			newObj.FieldType = this.FieldType;						
			newObj.FieldName = this.FieldName;						
			newObj.DataType = this.DataType;						
			newObj.DataKey = this.DataKey;						
			newObj.Placeholder = this.Placeholder;						
			newObj.OrderBy = this.OrderBy;						
			newObj.IsActive = this.IsActive;						
			newObj.IsRequired = this.IsRequired;						
			newObj.ErrorMessage = this.ErrorMessage;						
			newObj.ErrorMessage2 = this.ErrorMessage2;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(FormGeneratorBase.Property_Id, Id);				
			info.AddValue(FormGeneratorBase.Property_CompanyId, CompanyId);				
			info.AddValue(FormGeneratorBase.Property_FormName, FormName);				
			info.AddValue(FormGeneratorBase.Property_FieldLabel, FieldLabel);				
			info.AddValue(FormGeneratorBase.Property_FieldType, FieldType);				
			info.AddValue(FormGeneratorBase.Property_FieldName, FieldName);				
			info.AddValue(FormGeneratorBase.Property_DataType, DataType);				
			info.AddValue(FormGeneratorBase.Property_DataKey, DataKey);				
			info.AddValue(FormGeneratorBase.Property_Placeholder, Placeholder);				
			info.AddValue(FormGeneratorBase.Property_OrderBy, OrderBy);				
			info.AddValue(FormGeneratorBase.Property_IsActive, IsActive);				
			info.AddValue(FormGeneratorBase.Property_IsRequired, IsRequired);				
			info.AddValue(FormGeneratorBase.Property_ErrorMessage, ErrorMessage);				
			info.AddValue(FormGeneratorBase.Property_ErrorMessage2, ErrorMessage2);				
		}
		#endregion

		
	}
}
