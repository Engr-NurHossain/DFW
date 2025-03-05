using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RecurringBillingScheduleItemsBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RecurringBillingScheduleItemsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ScheduleId = 1,
			ProductName = 2,
			Description = 3,
			Qty = 4,
			Rate = 5,
			Amount = 6,
			IsTaxable = 7,
			AddedBy = 8,
			AddedDate = 9,
			EffectiveDate = 10,
			CycleStartDate = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ScheduleId = "ScheduleId";		            
		public const string Property_ProductName = "ProductName";		            
		public const string Property_Description = "Description";		            
		public const string Property_Qty = "Qty";		            
		public const string Property_Rate = "Rate";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_IsTaxable = "IsTaxable";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_EffectiveDate = "EffectiveDate";		            
		public const string Property_CycleStartDate = "CycleStartDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ScheduleId;	            
		private String _ProductName;	            
		private String _Description;	            
		private Int32 _Qty;	            
		private Double _Rate;	            
		private Double _Amount;	            
		private Boolean _IsTaxable;	            
		private Guid _AddedBy;	            
		private DateTime _AddedDate;	            
		private Nullable<DateTime> _EffectiveDate;	            
		private Nullable<DateTime> _CycleStartDate;	            
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
		public Guid ScheduleId
		{	
			get{ return _ScheduleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ScheduleId, value, _ScheduleId);
				if (PropertyChanging(args))
				{
					_ScheduleId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProductName
		{	
			get{ return _ProductName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProductName, value, _ProductName);
				if (PropertyChanging(args))
				{
					_ProductName = value;
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
		public Int32 Qty
		{	
			get{ return _Qty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Qty, value, _Qty);
				if (PropertyChanging(args))
				{
					_Qty = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Rate
		{	
			get{ return _Rate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Rate, value, _Rate);
				if (PropertyChanging(args))
				{
					_Rate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsTaxable
		{	
			get{ return _IsTaxable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTaxable, value, _IsTaxable);
				if (PropertyChanging(args))
				{
					_IsTaxable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EffectiveDate
		{	
			get{ return _EffectiveDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EffectiveDate, value, _EffectiveDate);
				if (PropertyChanging(args))
				{
					_EffectiveDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CycleStartDate
		{	
			get{ return _CycleStartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CycleStartDate, value, _CycleStartDate);
				if (PropertyChanging(args))
				{
					_CycleStartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RecurringBillingScheduleItemsBase Clone()
		{
			RecurringBillingScheduleItemsBase newObj = new  RecurringBillingScheduleItemsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ScheduleId = this.ScheduleId;						
			newObj.ProductName = this.ProductName;						
			newObj.Description = this.Description;						
			newObj.Qty = this.Qty;						
			newObj.Rate = this.Rate;						
			newObj.Amount = this.Amount;						
			newObj.IsTaxable = this.IsTaxable;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.EffectiveDate = this.EffectiveDate;						
			newObj.CycleStartDate = this.CycleStartDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RecurringBillingScheduleItemsBase.Property_Id, Id);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_ScheduleId, ScheduleId);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_ProductName, ProductName);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_Description, Description);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_Qty, Qty);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_Rate, Rate);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_Amount, Amount);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_IsTaxable, IsTaxable);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_AddedBy, AddedBy);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_AddedDate, AddedDate);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_EffectiveDate, EffectiveDate);				
			info.AddValue(RecurringBillingScheduleItemsBase.Property_CycleStartDate, CycleStartDate);				
		}
		#endregion

		
	}
}
