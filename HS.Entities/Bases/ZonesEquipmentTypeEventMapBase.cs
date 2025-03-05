using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ZonesEquipmentTypeEventMapBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ZonesEquipmentTypeEventMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			EquipmentTypeId = 1,
			EventId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_EquipmentTypeId = "EquipmentTypeId";		            
		public const string Property_EventId = "EventId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private String _EquipmentTypeId;	            
		private String _EventId;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int32 ID
		{	
			get{ return _ID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ID, value, _ID);
				if (PropertyChanging(args))
				{
					_ID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipmentTypeId
		{	
			get{ return _EquipmentTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentTypeId, value, _EquipmentTypeId);
				if (PropertyChanging(args))
				{
					_EquipmentTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EventId
		{	
			get{ return _EventId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EventId, value, _EventId);
				if (PropertyChanging(args))
				{
					_EventId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ZonesEquipmentTypeEventMapBase Clone()
		{
			ZonesEquipmentTypeEventMapBase newObj = new  ZonesEquipmentTypeEventMapBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.EquipmentTypeId = this.EquipmentTypeId;						
			newObj.EventId = this.EventId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ZonesEquipmentTypeEventMapBase.Property_ID, ID);				
			info.AddValue(ZonesEquipmentTypeEventMapBase.Property_EquipmentTypeId, EquipmentTypeId);				
			info.AddValue(ZonesEquipmentTypeEventMapBase.Property_EventId, EventId);				
		}
		#endregion

		
	}
}
