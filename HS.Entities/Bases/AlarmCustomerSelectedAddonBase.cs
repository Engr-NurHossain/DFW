using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AlarmCustomerSelectedAddonBase", Namespace = "http://www.hims-tech.com//entities")]
	public class AlarmCustomerSelectedAddonBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			AddonType = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_AddonType = "AddonType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _AddonType;	            
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
		public String AddonType
		{	
			get{ return _AddonType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddonType, value, _AddonType);
				if (PropertyChanging(args))
				{
					_AddonType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AlarmCustomerSelectedAddonBase Clone()
		{
			AlarmCustomerSelectedAddonBase newObj = new  AlarmCustomerSelectedAddonBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.AddonType = this.AddonType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AlarmCustomerSelectedAddonBase.Property_Id, Id);				
			info.AddValue(AlarmCustomerSelectedAddonBase.Property_CustomerId, CustomerId);				
			info.AddValue(AlarmCustomerSelectedAddonBase.Property_AddonType, AddonType);				
		}
		#endregion

		
	}
}
