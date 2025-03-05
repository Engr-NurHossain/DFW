using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SystemTypeManufacturerMapBase", Namespace = "http://www.piistech.com//entities")]
	public class SystemTypeManufacturerMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SystemId = 1,
			ManufacturerId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SystemId = "SystemId";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _SystemId;	            
		private Guid _ManufacturerId;	            
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
		public Int32 SystemId
		{	
			get{ return _SystemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SystemId, value, _SystemId);
				if (PropertyChanging(args))
				{
					_SystemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ManufacturerId
		{	
			get{ return _ManufacturerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManufacturerId, value, _ManufacturerId);
				if (PropertyChanging(args))
				{
					_ManufacturerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SystemTypeManufacturerMapBase Clone()
		{
			SystemTypeManufacturerMapBase newObj = new  SystemTypeManufacturerMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SystemId = this.SystemId;						
			newObj.ManufacturerId = this.ManufacturerId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SystemTypeManufacturerMapBase.Property_Id, Id);				
			info.AddValue(SystemTypeManufacturerMapBase.Property_SystemId, SystemId);				
			info.AddValue(SystemTypeManufacturerMapBase.Property_ManufacturerId, ManufacturerId);				
		}
		#endregion

		
	}
}
