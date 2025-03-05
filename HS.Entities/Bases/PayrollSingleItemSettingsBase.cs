using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollSingleItemSettingsBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollSingleItemSettingsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SingleItemSettingsId = 1,
			CompanyId = 2,
			SearchKey = 3,
			SearchValue = 4,
			IsActive = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			LastUpdateBy = 8,
			LastUpdateDate = 9,
			TermSheetId = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SingleItemSettingsId = "SingleItemSettingsId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SearchKey = "SearchKey";		            
		public const string Property_SearchValue = "SearchValue";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SingleItemSettingsId;	            
		private Guid _CompanyId;	            
		private String _SearchKey;	            
		private String _SearchValue;	            
		private Boolean _IsActive;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _LastUpdateBy;	            
		private Nullable<DateTime> _LastUpdateDate;	            
		private Guid _TermSheetId;	            
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
		public Guid SingleItemSettingsId
		{	
			get{ return _SingleItemSettingsId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SingleItemSettingsId, value, _SingleItemSettingsId);
				if (PropertyChanging(args))
				{
					_SingleItemSettingsId = value;
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
		public String SearchValue
		{	
			get{ return _SearchValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchValue, value, _SearchValue);
				if (PropertyChanging(args))
				{
					_SearchValue = value;
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
		public Guid CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdateBy
		{	
			get{ return _LastUpdateBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateBy, value, _LastUpdateBy);
				if (PropertyChanging(args))
				{
					_LastUpdateBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastUpdateDate
		{	
			get{ return _LastUpdateDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateDate, value, _LastUpdateDate);
				if (PropertyChanging(args))
				{
					_LastUpdateDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TermSheetId
		{	
			get{ return _TermSheetId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TermSheetId, value, _TermSheetId);
				if (PropertyChanging(args))
				{
					_TermSheetId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PayrollSingleItemSettingsBase Clone()
		{
			PayrollSingleItemSettingsBase newObj = new  PayrollSingleItemSettingsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SingleItemSettingsId = this.SingleItemSettingsId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SearchKey = this.SearchKey;						
			newObj.SearchValue = this.SearchValue;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdateDate = this.LastUpdateDate;						
			newObj.TermSheetId = this.TermSheetId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollSingleItemSettingsBase.Property_Id, Id);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_SingleItemSettingsId, SingleItemSettingsId);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_SearchKey, SearchKey);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_SearchValue, SearchValue);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_IsActive, IsActive);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollSingleItemSettingsBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
