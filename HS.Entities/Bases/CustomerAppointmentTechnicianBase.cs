using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAppointmentTechnicianBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerAppointmentTechnicianBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EmployeeId = 1,
			CustomerAppointmentId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_CustomerAppointmentId = "CustomerAppointmentId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EmployeeId;	            
		private Int32 _CustomerAppointmentId;	            
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
		public Int32 CustomerAppointmentId
		{	
			get{ return _CustomerAppointmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerAppointmentId, value, _CustomerAppointmentId);
				if (PropertyChanging(args))
				{
					_CustomerAppointmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAppointmentTechnicianBase Clone()
		{
			CustomerAppointmentTechnicianBase newObj = new  CustomerAppointmentTechnicianBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.CustomerAppointmentId = this.CustomerAppointmentId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAppointmentTechnicianBase.Property_Id, Id);				
			info.AddValue(CustomerAppointmentTechnicianBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(CustomerAppointmentTechnicianBase.Property_CustomerAppointmentId, CustomerAppointmentId);				
		}
		#endregion

		
	}
}
