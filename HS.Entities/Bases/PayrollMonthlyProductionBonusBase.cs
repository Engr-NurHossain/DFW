using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollMonthlyProductionBonusBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollMonthlyProductionBonusBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollMonthlyProductionBonusId = 1,
			CompanyId = 2,
			MonthlyProductionBonusMin = 3,
			MonthlyProductionBonusMax = 4,
			Point = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			LastUpdateBy = 8,
			LastUpdateDate = 9,
			TermSheetId = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollMonthlyProductionBonusId = "PayrollMonthlyProductionBonusId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_MonthlyProductionBonusMin = "MonthlyProductionBonusMin";		            
		public const string Property_MonthlyProductionBonusMax = "MonthlyProductionBonusMax";		            
		public const string Property_Point = "Point";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollMonthlyProductionBonusId;	            
		private Guid _CompanyId;	            
		private Int32 _MonthlyProductionBonusMin;	            
		private Int32 _MonthlyProductionBonusMax;	            
		private Int32 _Point;	            
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
		public Guid PayrollMonthlyProductionBonusId
		{	
			get{ return _PayrollMonthlyProductionBonusId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollMonthlyProductionBonusId, value, _PayrollMonthlyProductionBonusId);
				if (PropertyChanging(args))
				{
					_PayrollMonthlyProductionBonusId = value;
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
		public Int32 MonthlyProductionBonusMin
		{	
			get{ return _MonthlyProductionBonusMin; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyProductionBonusMin, value, _MonthlyProductionBonusMin);
				if (PropertyChanging(args))
				{
					_MonthlyProductionBonusMin = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 MonthlyProductionBonusMax
		{	
			get{ return _MonthlyProductionBonusMax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyProductionBonusMax, value, _MonthlyProductionBonusMax);
				if (PropertyChanging(args))
				{
					_MonthlyProductionBonusMax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Point
		{	
			get{ return _Point; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Point, value, _Point);
				if (PropertyChanging(args))
				{
					_Point = value;
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
		public  PayrollMonthlyProductionBonusBase Clone()
		{
			PayrollMonthlyProductionBonusBase newObj = new  PayrollMonthlyProductionBonusBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollMonthlyProductionBonusId = this.PayrollMonthlyProductionBonusId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.MonthlyProductionBonusMin = this.MonthlyProductionBonusMin;						
			newObj.MonthlyProductionBonusMax = this.MonthlyProductionBonusMax;						
			newObj.Point = this.Point;						
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
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_Id, Id);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_PayrollMonthlyProductionBonusId, PayrollMonthlyProductionBonusId);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_MonthlyProductionBonusMin, MonthlyProductionBonusMin);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_MonthlyProductionBonusMax, MonthlyProductionBonusMax);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_Point, Point);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollMonthlyProductionBonusBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
