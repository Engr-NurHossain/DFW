using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ServiceDetailInfoViewBase", Namespace = "http://www.piistech.com//entities")]
	public class ServiceDetailInfoViewBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ServiceId = 1,
			ShowManufacturer = 2,
			ShowLocation = 3,
			ShowType = 4,
			ShowModel = 5,
			ShowFinish = 6,
			ShowCapacity = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ServiceId = "ServiceId";		            
		public const string Property_ShowManufacturer = "ShowManufacturer";		            
		public const string Property_ShowLocation = "ShowLocation";		            
		public const string Property_ShowType = "ShowType";		            
		public const string Property_ShowModel = "ShowModel";		            
		public const string Property_ShowFinish = "ShowFinish";		            
		public const string Property_ShowCapacity = "ShowCapacity";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ServiceId;	            
		private Boolean _ShowManufacturer;	            
		private Boolean _ShowLocation;	            
		private Boolean _ShowType;	            
		private Boolean _ShowModel;	            
		private Boolean _ShowFinish;	            
		private Boolean _ShowCapacity;	            
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
		public Guid ServiceId
		{	
			get{ return _ServiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceId, value, _ServiceId);
				if (PropertyChanging(args))
				{
					_ServiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean ShowManufacturer
		{	
			get{ return _ShowManufacturer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowManufacturer, value, _ShowManufacturer);
				if (PropertyChanging(args))
				{
					_ShowManufacturer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean ShowLocation
		{	
			get{ return _ShowLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowLocation, value, _ShowLocation);
				if (PropertyChanging(args))
				{
					_ShowLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean ShowType
		{	
			get{ return _ShowType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowType, value, _ShowType);
				if (PropertyChanging(args))
				{
					_ShowType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean ShowModel
		{	
			get{ return _ShowModel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowModel, value, _ShowModel);
				if (PropertyChanging(args))
				{
					_ShowModel = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean ShowFinish
		{	
			get{ return _ShowFinish; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowFinish, value, _ShowFinish);
				if (PropertyChanging(args))
				{
					_ShowFinish = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean ShowCapacity
		{	
			get{ return _ShowCapacity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShowCapacity, value, _ShowCapacity);
				if (PropertyChanging(args))
				{
					_ShowCapacity = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ServiceDetailInfoViewBase Clone()
		{
			ServiceDetailInfoViewBase newObj = new  ServiceDetailInfoViewBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ServiceId = this.ServiceId;						
			newObj.ShowManufacturer = this.ShowManufacturer;						
			newObj.ShowLocation = this.ShowLocation;						
			newObj.ShowType = this.ShowType;						
			newObj.ShowModel = this.ShowModel;						
			newObj.ShowFinish = this.ShowFinish;						
			newObj.ShowCapacity = this.ShowCapacity;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ServiceDetailInfoViewBase.Property_Id, Id);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ServiceId, ServiceId);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ShowManufacturer, ShowManufacturer);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ShowLocation, ShowLocation);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ShowType, ShowType);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ShowModel, ShowModel);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ShowFinish, ShowFinish);				
			info.AddValue(ServiceDetailInfoViewBase.Property_ShowCapacity, ShowCapacity);				
		}
		#endregion

		
	}
}
