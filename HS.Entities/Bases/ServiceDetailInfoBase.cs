using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ServiceDetailInfoBase", Namespace = "http://www.piistech.com//entities")]
	public class ServiceDetailInfoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ServiceInfoId = 1,
			Name = 2,
			Type = 3,
			ServiceId = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ServiceInfoId = "ServiceInfoId";		            
		public const string Property_Name = "Name";		            
		public const string Property_Type = "Type";		            
		public const string Property_ServiceId = "ServiceId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ServiceInfoId;	            
		private String _Name;	            
		private String _Type;	            
		private Guid _ServiceId;	            
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
		public Guid ServiceInfoId
		{	
			get{ return _ServiceInfoId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceInfoId, value, _ServiceInfoId);
				if (PropertyChanging(args))
				{
					_ServiceInfoId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  ServiceDetailInfoBase Clone()
		{
			ServiceDetailInfoBase newObj = new  ServiceDetailInfoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ServiceInfoId = this.ServiceInfoId;						
			newObj.Name = this.Name;						
			newObj.Type = this.Type;						
			newObj.ServiceId = this.ServiceId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ServiceDetailInfoBase.Property_Id, Id);				
			info.AddValue(ServiceDetailInfoBase.Property_ServiceInfoId, ServiceInfoId);				
			info.AddValue(ServiceDetailInfoBase.Property_Name, Name);				
			info.AddValue(ServiceDetailInfoBase.Property_Type, Type);				
			info.AddValue(ServiceDetailInfoBase.Property_ServiceId, ServiceId);				
		}
		#endregion

		
	}
}
