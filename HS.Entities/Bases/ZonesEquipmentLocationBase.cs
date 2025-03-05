using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ZonesEquipmentLocationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ZonesEquipmentLocationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			EquipmentLocationId = 1,
			EquipmentLocation = 2,
			Platform = 3
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_EquipmentLocationId = "EquipmentLocationId";		            
		public const string Property_EquipmentLocation = "EquipmentLocation";		            
		public const string Property_Platform = "Platform";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private String _EquipmentLocationId;	            
		private String _EquipmentLocation;	            
		private String _Platform;	            
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
		public String EquipmentLocationId
		{	
			get{ return _EquipmentLocationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentLocationId, value, _EquipmentLocationId);
				if (PropertyChanging(args))
				{
					_EquipmentLocationId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipmentLocation
		{	
			get{ return _EquipmentLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentLocation, value, _EquipmentLocation);
				if (PropertyChanging(args))
				{
					_EquipmentLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Platform
		{	
			get{ return _Platform; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Platform, value, _Platform);
				if (PropertyChanging(args))
				{
					_Platform = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ZonesEquipmentLocationBase Clone()
		{
			ZonesEquipmentLocationBase newObj = new  ZonesEquipmentLocationBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.EquipmentLocationId = this.EquipmentLocationId;						
			newObj.EquipmentLocation = this.EquipmentLocation;						
			newObj.Platform = this.Platform;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ZonesEquipmentLocationBase.Property_ID, ID);				
			info.AddValue(ZonesEquipmentLocationBase.Property_EquipmentLocationId, EquipmentLocationId);				
			info.AddValue(ZonesEquipmentLocationBase.Property_EquipmentLocation, EquipmentLocation);				
			info.AddValue(ZonesEquipmentLocationBase.Property_Platform, Platform);				
		}
		#endregion

		
	}
}
