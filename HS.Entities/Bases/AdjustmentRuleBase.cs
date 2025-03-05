using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AdjustmentRuleBase", Namespace = "http://www.piistech.com//entities")]
	public class AdjustmentRuleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ComissionSessionId = 1,
			AdjustSchemeId = 2,
			TableName = 3,
			ColumnName = 4,
			ColumnValue = 5,
			DataType = 6,
			CommandType = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ComissionSessionId = "ComissionSessionId";		            
		public const string Property_AdjustSchemeId = "AdjustSchemeId";		            
		public const string Property_TableName = "TableName";		            
		public const string Property_ColumnName = "ColumnName";		            
		public const string Property_ColumnValue = "ColumnValue";		            
		public const string Property_DataType = "DataType";		            
		public const string Property_CommandType = "CommandType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _ComissionSessionId;	            
		private Nullable<Int32> _AdjustSchemeId;	            
		private String _TableName;	            
		private String _ColumnName;	            
		private String _ColumnValue;	            
		private String _DataType;	            
		private String _CommandType;	            
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
		public Nullable<Int32> ComissionSessionId
		{	
			get{ return _ComissionSessionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ComissionSessionId, value, _ComissionSessionId);
				if (PropertyChanging(args))
				{
					_ComissionSessionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AdjustSchemeId
		{	
			get{ return _AdjustSchemeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustSchemeId, value, _AdjustSchemeId);
				if (PropertyChanging(args))
				{
					_AdjustSchemeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TableName
		{	
			get{ return _TableName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TableName, value, _TableName);
				if (PropertyChanging(args))
				{
					_TableName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ColumnName
		{	
			get{ return _ColumnName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ColumnName, value, _ColumnName);
				if (PropertyChanging(args))
				{
					_ColumnName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ColumnValue
		{	
			get{ return _ColumnValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ColumnValue, value, _ColumnValue);
				if (PropertyChanging(args))
				{
					_ColumnValue = value;
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
		public String CommandType
		{	
			get{ return _CommandType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommandType, value, _CommandType);
				if (PropertyChanging(args))
				{
					_CommandType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AdjustmentRuleBase Clone()
		{
			AdjustmentRuleBase newObj = new  AdjustmentRuleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ComissionSessionId = this.ComissionSessionId;						
			newObj.AdjustSchemeId = this.AdjustSchemeId;						
			newObj.TableName = this.TableName;						
			newObj.ColumnName = this.ColumnName;						
			newObj.ColumnValue = this.ColumnValue;						
			newObj.DataType = this.DataType;						
			newObj.CommandType = this.CommandType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AdjustmentRuleBase.Property_Id, Id);				
			info.AddValue(AdjustmentRuleBase.Property_ComissionSessionId, ComissionSessionId);				
			info.AddValue(AdjustmentRuleBase.Property_AdjustSchemeId, AdjustSchemeId);				
			info.AddValue(AdjustmentRuleBase.Property_TableName, TableName);				
			info.AddValue(AdjustmentRuleBase.Property_ColumnName, ColumnName);				
			info.AddValue(AdjustmentRuleBase.Property_ColumnValue, ColumnValue);				
			info.AddValue(AdjustmentRuleBase.Property_DataType, DataType);				
			info.AddValue(AdjustmentRuleBase.Property_CommandType, CommandType);				
		}
		#endregion

		
	}
}
