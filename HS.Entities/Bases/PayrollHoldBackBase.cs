using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollHoldBackBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollHoldBackBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollHoldBackId = 1,
			CompanyId = 2,
			HoldBack = 3,
			Percentage = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			LastUpdateBy = 7,
			LastUpdateDate = 8,
			TermSheetId = 9,
			Type = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollHoldBackId = "PayrollHoldBackId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_HoldBack = "HoldBack";		            
		public const string Property_Percentage = "Percentage";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		public const string Property_Type = "Type";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollHoldBackId;	            
		private Guid _CompanyId;	            
		private String _HoldBack;	            
		private Double _Percentage;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _LastUpdateBy;	            
		private Nullable<DateTime> _LastUpdateDate;	            
		private Guid _TermSheetId;	            
		private String _Type;	            
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
		public Guid PayrollHoldBackId
		{	
			get{ return _PayrollHoldBackId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollHoldBackId, value, _PayrollHoldBackId);
				if (PropertyChanging(args))
				{
					_PayrollHoldBackId = value;
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
		public String HoldBack
		{	
			get{ return _HoldBack; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HoldBack, value, _HoldBack);
				if (PropertyChanging(args))
				{
					_HoldBack = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Percentage
		{	
			get{ return _Percentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Percentage, value, _Percentage);
				if (PropertyChanging(args))
				{
					_Percentage = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  PayrollHoldBackBase Clone()
		{
			PayrollHoldBackBase newObj = new  PayrollHoldBackBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollHoldBackId = this.PayrollHoldBackId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.HoldBack = this.HoldBack;						
			newObj.Percentage = this.Percentage;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdateDate = this.LastUpdateDate;						
			newObj.TermSheetId = this.TermSheetId;						
			newObj.Type = this.Type;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollHoldBackBase.Property_Id, Id);				
			info.AddValue(PayrollHoldBackBase.Property_PayrollHoldBackId, PayrollHoldBackId);				
			info.AddValue(PayrollHoldBackBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollHoldBackBase.Property_HoldBack, HoldBack);				
			info.AddValue(PayrollHoldBackBase.Property_Percentage, Percentage);				
			info.AddValue(PayrollHoldBackBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollHoldBackBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollHoldBackBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollHoldBackBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollHoldBackBase.Property_TermSheetId, TermSheetId);				
			info.AddValue(PayrollHoldBackBase.Property_Type, Type);				
		}
		#endregion

		
	}
}
