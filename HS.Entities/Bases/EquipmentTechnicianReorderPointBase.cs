using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentTechnicianReorderPointBase", Namespace = "http://www.piistech.com//entities")]
	public class EquipmentTechnicianReorderPointBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			EquipmentId = 2,
			TechnicianId = 3,
			ReorderPoint = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_ReorderPoint = "ReorderPoint";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _EquipmentId;	            
		private Guid _TechnicianId;	            
		private Int32 _ReorderPoint;	            
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
		public Guid EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TechnicianId
		{	
			get{ return _TechnicianId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechnicianId, value, _TechnicianId);
				if (PropertyChanging(args))
				{
					_TechnicianId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ReorderPoint
		{	
			get{ return _ReorderPoint; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReorderPoint, value, _ReorderPoint);
				if (PropertyChanging(args))
				{
					_ReorderPoint = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EquipmentTechnicianReorderPointBase Clone()
		{
			EquipmentTechnicianReorderPointBase newObj = new  EquipmentTechnicianReorderPointBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.ReorderPoint = this.ReorderPoint;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentTechnicianReorderPointBase.Property_Id, Id);				
			info.AddValue(EquipmentTechnicianReorderPointBase.Property_CompanyId, CompanyId);				
			info.AddValue(EquipmentTechnicianReorderPointBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentTechnicianReorderPointBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(EquipmentTechnicianReorderPointBase.Property_ReorderPoint, ReorderPoint);				
		}
		#endregion

		
	}
}
