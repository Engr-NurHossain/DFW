using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeePTOHourLogBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeePTOHourLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			PTOHour = 2,
			FromDate = 3,
			EndDate = 4,
			CreatedDate = 5,
			LastUpdatedDate = 6,
			WorkingHours = 7,
			PtoRate = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_PTOHour = "PTOHour";		            
		public const string Property_FromDate = "FromDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_WorkingHours = "WorkingHours";		            
		public const string Property_PtoRate = "PtoRate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Nullable<Double> _PTOHour;	            
		private Nullable<DateTime> _FromDate;	            
		private Nullable<DateTime> _EndDate;	            
		private DateTime _CreatedDate;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Double> _WorkingHours;	            
		private Nullable<Double> _PtoRate;	            
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PTOHour
		{	
			get{ return _PTOHour; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PTOHour, value, _PTOHour);
				if (PropertyChanging(args))
				{
					_PTOHour = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FromDate
		{	
			get{ return _FromDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FromDate, value, _FromDate);
				if (PropertyChanging(args))
				{
					_FromDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
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

		[DataMember]
		public Nullable<Double> WorkingHours
		{	
			get{ return _WorkingHours; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WorkingHours, value, _WorkingHours);
				if (PropertyChanging(args))
				{
					_WorkingHours = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PtoRate
		{	
			get{ return _PtoRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PtoRate, value, _PtoRate);
				if (PropertyChanging(args))
				{
					_PtoRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeePTOHourLogBase Clone()
		{
			EmployeePTOHourLogBase newObj = new  EmployeePTOHourLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.PTOHour = this.PTOHour;						
			newObj.FromDate = this.FromDate;						
			newObj.EndDate = this.EndDate;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.WorkingHours = this.WorkingHours;						
			newObj.PtoRate = this.PtoRate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeePTOHourLogBase.Property_Id, Id);				
			info.AddValue(EmployeePTOHourLogBase.Property_UserId, UserId);				
			info.AddValue(EmployeePTOHourLogBase.Property_PTOHour, PTOHour);				
			info.AddValue(EmployeePTOHourLogBase.Property_FromDate, FromDate);				
			info.AddValue(EmployeePTOHourLogBase.Property_EndDate, EndDate);				
			info.AddValue(EmployeePTOHourLogBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeePTOHourLogBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(EmployeePTOHourLogBase.Property_WorkingHours, WorkingHours);				
			info.AddValue(EmployeePTOHourLogBase.Property_PtoRate, PtoRate);				
		}
		#endregion

		
	}
}