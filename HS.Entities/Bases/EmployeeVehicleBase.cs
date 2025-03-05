using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeVehicleBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeVehicleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			EmployeeId = 2,
			VehicleId = 3,
			AddedBy = 4,
			AddedDate = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_VehicleId = "VehicleId";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _EmployeeId;	            
		private Guid _VehicleId;	            
		private Guid _AddedBy;	            
		private DateTime _AddedDate;	            
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
		public Guid VehicleId
		{	
			get{ return _VehicleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VehicleId, value, _VehicleId);
				if (PropertyChanging(args))
				{
					_VehicleId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeVehicleBase Clone()
		{
			EmployeeVehicleBase newObj = new  EmployeeVehicleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.VehicleId = this.VehicleId;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeVehicleBase.Property_Id, Id);				
			info.AddValue(EmployeeVehicleBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeVehicleBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(EmployeeVehicleBase.Property_VehicleId, VehicleId);				
			info.AddValue(EmployeeVehicleBase.Property_AddedBy, AddedBy);				
			info.AddValue(EmployeeVehicleBase.Property_AddedDate, AddedDate);				
		}
		#endregion

		
	}
}
