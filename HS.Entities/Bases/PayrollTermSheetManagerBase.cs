using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollTermSheetManagerBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollTermSheetManagerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollTermSheetManagerId = 1,
			CompanyId = 2,
			TermSheetId = 3,
			ManagerId = 4,
			Type = 5,
			Value = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			LastUpdateBy = 9,
			LastUpdateDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollTermSheetManagerId = "PayrollTermSheetManagerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		public const string Property_ManagerId = "ManagerId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Value = "Value";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollTermSheetManagerId;	            
		private Guid _CompanyId;	            
		private Guid _TermSheetId;	            
		private Guid _ManagerId;	            
		private String _Type;	            
		private Double _Value;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _LastUpdateBy;	            
		private Nullable<DateTime> _LastUpdateDate;	            
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
		public Guid PayrollTermSheetManagerId
		{	
			get{ return _PayrollTermSheetManagerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollTermSheetManagerId, value, _PayrollTermSheetManagerId);
				if (PropertyChanging(args))
				{
					_PayrollTermSheetManagerId = value;
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

		[DataMember]
		public Guid ManagerId
		{	
			get{ return _ManagerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManagerId, value, _ManagerId);
				if (PropertyChanging(args))
				{
					_ManagerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Value
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

		#endregion
		
		#region Cloning Base Objects
		public  PayrollTermSheetManagerBase Clone()
		{
			PayrollTermSheetManagerBase newObj = new  PayrollTermSheetManagerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollTermSheetManagerId = this.PayrollTermSheetManagerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TermSheetId = this.TermSheetId;						
			newObj.ManagerId = this.ManagerId;						
			newObj.Type = this.Type;						
			newObj.Value = this.Value;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdateDate = this.LastUpdateDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollTermSheetManagerBase.Property_Id, Id);				
			info.AddValue(PayrollTermSheetManagerBase.Property_PayrollTermSheetManagerId, PayrollTermSheetManagerId);				
			info.AddValue(PayrollTermSheetManagerBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollTermSheetManagerBase.Property_TermSheetId, TermSheetId);				
			info.AddValue(PayrollTermSheetManagerBase.Property_ManagerId, ManagerId);				
			info.AddValue(PayrollTermSheetManagerBase.Property_Type, Type);				
			info.AddValue(PayrollTermSheetManagerBase.Property_Value, Value);				
			info.AddValue(PayrollTermSheetManagerBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollTermSheetManagerBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollTermSheetManagerBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollTermSheetManagerBase.Property_LastUpdateDate, LastUpdateDate);				
		}
		#endregion

		
	}
}
