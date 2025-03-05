using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SystemTypeServiceMapBase", Namespace = "http://www.piistech.com//entities")]
	public class SystemTypeServiceMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SystemTypeId = 1,
			EquipmentId = 2,
			PackageId = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SystemTypeId = "SystemTypeId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_PackageId = "PackageId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _SystemTypeId;	            
		private Guid _EquipmentId;	            
		private Guid _PackageId;	            
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
		public Int32 SystemTypeId
		{	
			get{ return _SystemTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SystemTypeId, value, _SystemTypeId);
				if (PropertyChanging(args))
				{
					_SystemTypeId = value;
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
		public Guid PackageId
		{	
			get{ return _PackageId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageId, value, _PackageId);
				if (PropertyChanging(args))
				{
					_PackageId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SystemTypeServiceMapBase Clone()
		{
			SystemTypeServiceMapBase newObj = new  SystemTypeServiceMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SystemTypeId = this.SystemTypeId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.PackageId = this.PackageId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SystemTypeServiceMapBase.Property_Id, Id);				
			info.AddValue(SystemTypeServiceMapBase.Property_SystemTypeId, SystemTypeId);				
			info.AddValue(SystemTypeServiceMapBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(SystemTypeServiceMapBase.Property_PackageId, PackageId);				
		}
		#endregion

		
	}
}
