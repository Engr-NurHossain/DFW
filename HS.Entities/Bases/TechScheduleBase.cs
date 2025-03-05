using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TechScheduleBase", Namespace = "http://www.piistech.com//entities")]
	public class TechScheduleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EmployeeId = 1,
			CustomerId = 2,
			CompanyId = 3,
			InstallDate = 4,
			ArrivalTime = 5,
			DepartureTime = 6,
			EstimatedArrival = 7,
			CheckScheduleConflict = 8,
			IsSendNotification = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_InstallDate = "InstallDate";		            
		public const string Property_ArrivalTime = "ArrivalTime";		            
		public const string Property_DepartureTime = "DepartureTime";		            
		public const string Property_EstimatedArrival = "EstimatedArrival";		            
		public const string Property_CheckScheduleConflict = "CheckScheduleConflict";		            
		public const string Property_IsSendNotification = "IsSendNotification";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EmployeeId;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private Nullable<DateTime> _InstallDate;	            
		private String _ArrivalTime;	            
		private String _DepartureTime;	            
		private String _EstimatedArrival;	            
		private Boolean _CheckScheduleConflict;	            
		private Boolean _IsSendNotification;	            
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
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
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
		public Nullable<DateTime> InstallDate
		{	
			get{ return _InstallDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallDate, value, _InstallDate);
				if (PropertyChanging(args))
				{
					_InstallDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ArrivalTime
		{	
			get{ return _ArrivalTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ArrivalTime, value, _ArrivalTime);
				if (PropertyChanging(args))
				{
					_ArrivalTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DepartureTime
		{	
			get{ return _DepartureTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DepartureTime, value, _DepartureTime);
				if (PropertyChanging(args))
				{
					_DepartureTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EstimatedArrival
		{	
			get{ return _EstimatedArrival; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatedArrival, value, _EstimatedArrival);
				if (PropertyChanging(args))
				{
					_EstimatedArrival = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean CheckScheduleConflict
		{	
			get{ return _CheckScheduleConflict; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CheckScheduleConflict, value, _CheckScheduleConflict);
				if (PropertyChanging(args))
				{
					_CheckScheduleConflict = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsSendNotification
		{	
			get{ return _IsSendNotification; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSendNotification, value, _IsSendNotification);
				if (PropertyChanging(args))
				{
					_IsSendNotification = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TechScheduleBase Clone()
		{
			TechScheduleBase newObj = new  TechScheduleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.InstallDate = this.InstallDate;						
			newObj.ArrivalTime = this.ArrivalTime;						
			newObj.DepartureTime = this.DepartureTime;						
			newObj.EstimatedArrival = this.EstimatedArrival;						
			newObj.CheckScheduleConflict = this.CheckScheduleConflict;						
			newObj.IsSendNotification = this.IsSendNotification;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TechScheduleBase.Property_Id, Id);				
			info.AddValue(TechScheduleBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(TechScheduleBase.Property_CustomerId, CustomerId);				
			info.AddValue(TechScheduleBase.Property_CompanyId, CompanyId);				
			info.AddValue(TechScheduleBase.Property_InstallDate, InstallDate);				
			info.AddValue(TechScheduleBase.Property_ArrivalTime, ArrivalTime);				
			info.AddValue(TechScheduleBase.Property_DepartureTime, DepartureTime);				
			info.AddValue(TechScheduleBase.Property_EstimatedArrival, EstimatedArrival);				
			info.AddValue(TechScheduleBase.Property_CheckScheduleConflict, CheckScheduleConflict);				
			info.AddValue(TechScheduleBase.Property_IsSendNotification, IsSendNotification);				
		}
		#endregion

		
	}
}
