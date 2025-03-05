using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeOperationsBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeOperationsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EmployeeId = 1,
			DayName = 2,
			OperationStartTime = 3,
			OperationEndTime = 4,
			LastUpdatedBy = 5,
			UpdatedDate = 6,
			CompanyId = 7,
			Notes = 8,
			IsDayOff = 9,
			SelectedDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_DayName = "DayName";		            
		public const string Property_OperationStartTime = "OperationStartTime";		            
		public const string Property_OperationEndTime = "OperationEndTime";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_UpdatedDate = "UpdatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_IsDayOff = "IsDayOff";		            
		public const string Property_SelectedDate = "SelectedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EmployeeId;	            
		private String _DayName;	            
		private String _OperationStartTime;	            
		private String _OperationEndTime;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _UpdatedDate;	            
		private Guid _CompanyId;	            
		private String _Notes;	            
		private Nullable<Boolean> _IsDayOff;	            
		private Nullable<DateTime> _SelectedDate;	            
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
		public Guid EmployeeId
		{	
			get{ return _EmployeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeId, value, _EmployeeId);
				if (PropertyChanging(args))
				{
					_EmployeeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DayName
		{	
			get{ return _DayName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DayName, value, _DayName);
				if (PropertyChanging(args))
				{
					_DayName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OperationStartTime
		{	
			get{ return _OperationStartTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OperationStartTime, value, _OperationStartTime);
				if (PropertyChanging(args))
				{
					_OperationStartTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OperationEndTime
		{	
			get{ return _OperationEndTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OperationEndTime, value, _OperationEndTime);
				if (PropertyChanging(args))
				{
					_OperationEndTime = value;
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
		public DateTime UpdatedDate
		{	
			get{ return _UpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedDate, value, _UpdatedDate);
				if (PropertyChanging(args))
				{
					_UpdatedDate = value;
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
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDayOff
		{	
			get{ return _IsDayOff; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDayOff, value, _IsDayOff);
				if (PropertyChanging(args))
				{
					_IsDayOff = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SelectedDate
		{	
			get{ return _SelectedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SelectedDate, value, _SelectedDate);
				if (PropertyChanging(args))
				{
					_SelectedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeOperationsBase Clone()
		{
			EmployeeOperationsBase newObj = new  EmployeeOperationsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.DayName = this.DayName;						
			newObj.OperationStartTime = this.OperationStartTime;						
			newObj.OperationEndTime = this.OperationEndTime;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.UpdatedDate = this.UpdatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Notes = this.Notes;						
			newObj.IsDayOff = this.IsDayOff;						
			newObj.SelectedDate = this.SelectedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeOperationsBase.Property_Id, Id);				
			info.AddValue(EmployeeOperationsBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(EmployeeOperationsBase.Property_DayName, DayName);				
			info.AddValue(EmployeeOperationsBase.Property_OperationStartTime, OperationStartTime);				
			info.AddValue(EmployeeOperationsBase.Property_OperationEndTime, OperationEndTime);				
			info.AddValue(EmployeeOperationsBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EmployeeOperationsBase.Property_UpdatedDate, UpdatedDate);				
			info.AddValue(EmployeeOperationsBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeOperationsBase.Property_Notes, Notes);				
			info.AddValue(EmployeeOperationsBase.Property_IsDayOff, IsDayOff);				
			info.AddValue(EmployeeOperationsBase.Property_SelectedDate, SelectedDate);				
		}
		#endregion

		
	}
}