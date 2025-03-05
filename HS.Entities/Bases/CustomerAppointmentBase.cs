using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAppointmentBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerAppointmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AppointmentId = 1,
			CompanyId = 2,
			CustomerId = 3,
			EmployeeId = 4,
			AppointmentType = 5,
			AppointmentDate = 6,
			AppointmentStartTime = 7,
			AppointmentEndTime = 8,
			IsAllDay = 9,
			Notes = 10,
			Status = 11,
			TaxType = 12,
			TaxPercent = 13,
			TaxTotal = 14,
			TotalAmount = 15,
			TotalAmountTax = 16,
			CreatedBy = 17,
			LastUpdatedBy = 18,
			LastUpdatedDate = 19,
			Address = 20
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AppointmentId = "AppointmentId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_AppointmentType = "AppointmentType";		            
		public const string Property_AppointmentDate = "AppointmentDate";		            
		public const string Property_AppointmentStartTime = "AppointmentStartTime";		            
		public const string Property_AppointmentEndTime = "AppointmentEndTime";		            
		public const string Property_IsAllDay = "IsAllDay";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_Status = "Status";		            
		public const string Property_TaxType = "TaxType";		            
		public const string Property_TaxPercent = "TaxPercent";		            
		public const string Property_TaxTotal = "TaxTotal";		            
		public const string Property_TotalAmount = "TotalAmount";		            
		public const string Property_TotalAmountTax = "TotalAmountTax";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Address = "Address";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _AppointmentId;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Guid _EmployeeId;	            
		private String _AppointmentType;	            
		private Nullable<DateTime> _AppointmentDate;	            
		private String _AppointmentStartTime;	            
		private String _AppointmentEndTime;	            
		private Boolean _IsAllDay;	            
		private String _Notes;	            
		private Nullable<Boolean> _Status;	            
		private String _TaxType;	            
		private Nullable<Double> _TaxPercent;	            
		private Nullable<Double> _TaxTotal;	            
		private Nullable<Double> _TotalAmount;	            
		private Nullable<Double> _TotalAmountTax;	            
		private String _CreatedBy;	            
		private String _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _Address;	            
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
		public Guid AppointmentId
		{	
			get{ return _AppointmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentId, value, _AppointmentId);
				if (PropertyChanging(args))
				{
					_AppointmentId = value;
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
		public String AppointmentType
		{	
			get{ return _AppointmentType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentType, value, _AppointmentType);
				if (PropertyChanging(args))
				{
					_AppointmentType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AppointmentDate
		{	
			get{ return _AppointmentDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentDate, value, _AppointmentDate);
				if (PropertyChanging(args))
				{
					_AppointmentDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AppointmentStartTime
		{	
			get{ return _AppointmentStartTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentStartTime, value, _AppointmentStartTime);
				if (PropertyChanging(args))
				{
					_AppointmentStartTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AppointmentEndTime
		{	
			get{ return _AppointmentEndTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentEndTime, value, _AppointmentEndTime);
				if (PropertyChanging(args))
				{
					_AppointmentEndTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsAllDay
		{	
			get{ return _IsAllDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAllDay, value, _IsAllDay);
				if (PropertyChanging(args))
				{
					_IsAllDay = value;
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
		public Nullable<Boolean> Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TaxType
		{	
			get{ return _TaxType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxType, value, _TaxType);
				if (PropertyChanging(args))
				{
					_TaxType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxPercent
		{	
			get{ return _TaxPercent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxPercent, value, _TaxPercent);
				if (PropertyChanging(args))
				{
					_TaxPercent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxTotal
		{	
			get{ return _TaxTotal; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxTotal, value, _TaxTotal);
				if (PropertyChanging(args))
				{
					_TaxTotal = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalAmount
		{	
			get{ return _TotalAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalAmount, value, _TotalAmount);
				if (PropertyChanging(args))
				{
					_TotalAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalAmountTax
		{	
			get{ return _TotalAmountTax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalAmountTax, value, _TotalAmountTax);
				if (PropertyChanging(args))
				{
					_TotalAmountTax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreatedBy
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
		public String LastUpdatedBy
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

		[DataMember]
		public String Address
		{	
			get{ return _Address; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address, value, _Address);
				if (PropertyChanging(args))
				{
					_Address = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAppointmentBase Clone()
		{
			CustomerAppointmentBase newObj = new  CustomerAppointmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AppointmentId = this.AppointmentId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.AppointmentType = this.AppointmentType;						
			newObj.AppointmentDate = this.AppointmentDate;						
			newObj.AppointmentStartTime = this.AppointmentStartTime;						
			newObj.AppointmentEndTime = this.AppointmentEndTime;						
			newObj.IsAllDay = this.IsAllDay;						
			newObj.Notes = this.Notes;						
			newObj.Status = this.Status;						
			newObj.TaxType = this.TaxType;						
			newObj.TaxPercent = this.TaxPercent;						
			newObj.TaxTotal = this.TaxTotal;						
			newObj.TotalAmount = this.TotalAmount;						
			newObj.TotalAmountTax = this.TotalAmountTax;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Address = this.Address;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAppointmentBase.Property_Id, Id);				
			info.AddValue(CustomerAppointmentBase.Property_AppointmentId, AppointmentId);				
			info.AddValue(CustomerAppointmentBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerAppointmentBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerAppointmentBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(CustomerAppointmentBase.Property_AppointmentType, AppointmentType);				
			info.AddValue(CustomerAppointmentBase.Property_AppointmentDate, AppointmentDate);				
			info.AddValue(CustomerAppointmentBase.Property_AppointmentStartTime, AppointmentStartTime);				
			info.AddValue(CustomerAppointmentBase.Property_AppointmentEndTime, AppointmentEndTime);				
			info.AddValue(CustomerAppointmentBase.Property_IsAllDay, IsAllDay);				
			info.AddValue(CustomerAppointmentBase.Property_Notes, Notes);				
			info.AddValue(CustomerAppointmentBase.Property_Status, Status);				
			info.AddValue(CustomerAppointmentBase.Property_TaxType, TaxType);				
			info.AddValue(CustomerAppointmentBase.Property_TaxPercent, TaxPercent);				
			info.AddValue(CustomerAppointmentBase.Property_TaxTotal, TaxTotal);				
			info.AddValue(CustomerAppointmentBase.Property_TotalAmount, TotalAmount);				
			info.AddValue(CustomerAppointmentBase.Property_TotalAmountTax, TotalAmountTax);				
			info.AddValue(CustomerAppointmentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerAppointmentBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(CustomerAppointmentBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(CustomerAppointmentBase.Property_Address, Address);				
		}
		#endregion

		
	}
}
