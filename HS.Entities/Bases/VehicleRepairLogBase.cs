using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "VehicleRepairLogBase", Namespace = "http://www.piistech.com//entities")]
	public class VehicleRepairLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			VehicleId = 1,
			Driver = 2,
			RepairDate = 3,
			Spent = 4,
			TireRotation = 5,
			OilChange = 6,
			Note = 7,
			CreatedDate = 8,
			CreatedByUid = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_VehicleId = "VehicleId";		            
		public const string Property_Driver = "Driver";		            
		public const string Property_RepairDate = "RepairDate";		            
		public const string Property_Spent = "Spent";		            
		public const string Property_TireRotation = "TireRotation";		            
		public const string Property_OilChange = "OilChange";		            
		public const string Property_Note = "Note";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _VehicleId;	            
		private Guid _Driver;	            
		private DateTime _RepairDate;	            
		private Double _Spent;	            
		private Nullable<DateTime> _TireRotation;	            
		private Nullable<DateTime> _OilChange;	            
		private String _Note;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedByUid;	            
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
		public Guid Driver
		{	
			get{ return _Driver; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Driver, value, _Driver);
				if (PropertyChanging(args))
				{
					_Driver = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime RepairDate
		{	
			get{ return _RepairDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepairDate, value, _RepairDate);
				if (PropertyChanging(args))
				{
					_RepairDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Spent
		{	
			get{ return _Spent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Spent, value, _Spent);
				if (PropertyChanging(args))
				{
					_Spent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> TireRotation
		{	
			get{ return _TireRotation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TireRotation, value, _TireRotation);
				if (PropertyChanging(args))
				{
					_TireRotation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> OilChange
		{	
			get{ return _OilChange; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OilChange, value, _OilChange);
				if (PropertyChanging(args))
				{
					_OilChange = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
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
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  VehicleRepairLogBase Clone()
		{
			VehicleRepairLogBase newObj = new  VehicleRepairLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.VehicleId = this.VehicleId;						
			newObj.Driver = this.Driver;						
			newObj.RepairDate = this.RepairDate;						
			newObj.Spent = this.Spent;						
			newObj.TireRotation = this.TireRotation;						
			newObj.OilChange = this.OilChange;						
			newObj.Note = this.Note;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedByUid = this.CreatedByUid;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(VehicleRepairLogBase.Property_Id, Id);				
			info.AddValue(VehicleRepairLogBase.Property_VehicleId, VehicleId);				
			info.AddValue(VehicleRepairLogBase.Property_Driver, Driver);				
			info.AddValue(VehicleRepairLogBase.Property_RepairDate, RepairDate);				
			info.AddValue(VehicleRepairLogBase.Property_Spent, Spent);				
			info.AddValue(VehicleRepairLogBase.Property_TireRotation, TireRotation);				
			info.AddValue(VehicleRepairLogBase.Property_OilChange, OilChange);				
			info.AddValue(VehicleRepairLogBase.Property_Note, Note);				
			info.AddValue(VehicleRepairLogBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(VehicleRepairLogBase.Property_CreatedByUid, CreatedByUid);				
		}
		#endregion

		
	}
}
