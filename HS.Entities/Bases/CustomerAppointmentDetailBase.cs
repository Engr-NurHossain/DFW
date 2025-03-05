using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAppointmentDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerAppointmentDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AppointmentId = 1,
			InstallType = 2,
			CollectedAmount = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AppointmentId = "AppointmentId";		            
		public const string Property_InstallType = "InstallType";		            
		public const string Property_CollectedAmount = "CollectedAmount";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _AppointmentId;	            
		private String _InstallType;	            
		private Nullable<Double> _CollectedAmount;	            
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
		public Guid AppointmentId
		{	
			get{ return _AppointmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentId, value, _AppointmentId);
				if (PropertyChanging(args))
				{
					_AppointmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstallType
		{	
			get{ return _InstallType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallType, value, _InstallType);
				if (PropertyChanging(args))
				{
					_InstallType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CollectedAmount
		{	
			get{ return _CollectedAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CollectedAmount, value, _CollectedAmount);
				if (PropertyChanging(args))
				{
					_CollectedAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAppointmentDetailBase Clone()
		{
			CustomerAppointmentDetailBase newObj = new  CustomerAppointmentDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AppointmentId = this.AppointmentId;						
			newObj.InstallType = this.InstallType;						
			newObj.CollectedAmount = this.CollectedAmount;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAppointmentDetailBase.Property_Id, Id);				
			info.AddValue(CustomerAppointmentDetailBase.Property_AppointmentId, AppointmentId);				
			info.AddValue(CustomerAppointmentDetailBase.Property_InstallType, InstallType);				
			info.AddValue(CustomerAppointmentDetailBase.Property_CollectedAmount, CollectedAmount);				
		}
		#endregion

		
	}
}
