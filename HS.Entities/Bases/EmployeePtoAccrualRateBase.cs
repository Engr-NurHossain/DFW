using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeePtoAccrualRateBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeePtoAccrualRateBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PayType = 2,
			MinimumDay = 3,
			MaximumDay = 4,
			AccrualRate = 5,
			PtoHours = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			LastUpdatedBy = 9,
			LastUpdatedDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PayType = "PayType";		            
		public const string Property_MinimumDay = "MinimumDay";		            
		public const string Property_MaximumDay = "MaximumDay";		            
		public const string Property_AccrualRate = "AccrualRate";		            
		public const string Property_PtoHours = "PtoHours";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _PayType;	            
		private Nullable<Int32> _MinimumDay;	            
		private Nullable<Int32> _MaximumDay;	            
		private Nullable<Double> _AccrualRate;	            
		private Nullable<Double> _PtoHours;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public String PayType
		{	
			get{ return _PayType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayType, value, _PayType);
				if (PropertyChanging(args))
				{
					_PayType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> MinimumDay
		{	
			get{ return _MinimumDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinimumDay, value, _MinimumDay);
				if (PropertyChanging(args))
				{
					_MinimumDay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> MaximumDay
		{	
			get{ return _MaximumDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaximumDay, value, _MaximumDay);
				if (PropertyChanging(args))
				{
					_MaximumDay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AccrualRate
		{	
			get{ return _AccrualRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccrualRate, value, _AccrualRate);
				if (PropertyChanging(args))
				{
					_AccrualRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PtoHours
		{	
			get{ return _PtoHours; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PtoHours, value, _PtoHours);
				if (PropertyChanging(args))
				{
					_PtoHours = value;
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
		public DateTime CreatedDate
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
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeePtoAccrualRateBase Clone()
		{
			EmployeePtoAccrualRateBase newObj = new  EmployeePtoAccrualRateBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PayType = this.PayType;						
			newObj.MinimumDay = this.MinimumDay;						
			newObj.MaximumDay = this.MaximumDay;						
			newObj.AccrualRate = this.AccrualRate;						
			newObj.PtoHours = this.PtoHours;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeePtoAccrualRateBase.Property_Id, Id);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_PayType, PayType);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_MinimumDay, MinimumDay);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_MaximumDay, MaximumDay);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_AccrualRate, AccrualRate);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_PtoHours, PtoHours);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EmployeePtoAccrualRateBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}