using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ZonesEquipmentTypeBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ZonesEquipmentTypeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			EqpmentTypeId = 1,
			EquipmentType = 2,
			Platform = 3
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_EqpmentTypeId = "EqpmentTypeId";		            
		public const string Property_EquipmentType = "EquipmentType";		            
		public const string Property_Platform = "Platform";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private String _EqpmentTypeId;	            
		private String _EquipmentType;	            
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
		public String EqpmentTypeId
		{	
			get{ return _EqpmentTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EqpmentTypeId, value, _EqpmentTypeId);
				if (PropertyChanging(args))
				{
					_EqpmentTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipmentType
		{	
			get{ return _EquipmentType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentType, value, _EquipmentType);
				if (PropertyChanging(args))
				{
					_EquipmentType = value;
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
		public  ZonesEquipmentTypeBase Clone()
		{
			ZonesEquipmentTypeBase newObj = new  ZonesEquipmentTypeBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.EqpmentTypeId = this.EqpmentTypeId;						
			newObj.EquipmentType = this.EquipmentType;						
			newObj.Platform = this.Platform;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ZonesEquipmentTypeBase.Property_ID, ID);				
			info.AddValue(ZonesEquipmentTypeBase.Property_EqpmentTypeId, EqpmentTypeId);				
			info.AddValue(ZonesEquipmentTypeBase.Property_EquipmentType, EquipmentType);				
			info.AddValue(ZonesEquipmentTypeBase.Property_Platform, Platform);				
		}
		#endregion

		
	}
}
