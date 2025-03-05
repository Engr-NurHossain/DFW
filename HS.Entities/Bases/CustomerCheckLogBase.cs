using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCheckLogBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerCheckLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			RouteId = 2,
			CheckInTime = 3,
			CheckOutTime = 4,
			CheckType = 5,
			ToatlTime = 6,
			CreadtedBy = 7,
			CreatedDate = 8,
			GeeseCount = 9,
			UserId = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_RouteId = "RouteId";		            
		public const string Property_CheckInTime = "CheckInTime";		            
		public const string Property_CheckOutTime = "CheckOutTime";		            
		public const string Property_CheckType = "CheckType";		            
		public const string Property_ToatlTime = "ToatlTime";		            
		public const string Property_CreadtedBy = "CreadtedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_GeeseCount = "GeeseCount";		            
		public const string Property_UserId = "UserId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _RouteId;	            
		private Nullable<DateTime> _CheckInTime;	            
		private Nullable<DateTime> _CheckOutTime;	            
		private String _CheckType;	            
		private Nullable<Double> _ToatlTime;	            
		private Guid _CreadtedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Int32> _GeeseCount;	            
		private Guid _UserId;	            
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
		public Guid RouteId
		{	
			get{ return _RouteId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RouteId, value, _RouteId);
				if (PropertyChanging(args))
				{
					_RouteId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CheckInTime
		{	
			get{ return _CheckInTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CheckInTime, value, _CheckInTime);
				if (PropertyChanging(args))
				{
					_CheckInTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CheckOutTime
		{	
			get{ return _CheckOutTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CheckOutTime, value, _CheckOutTime);
				if (PropertyChanging(args))
				{
					_CheckOutTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CheckType
		{	
			get{ return _CheckType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CheckType, value, _CheckType);
				if (PropertyChanging(args))
				{
					_CheckType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ToatlTime
		{	
			get{ return _ToatlTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToatlTime, value, _ToatlTime);
				if (PropertyChanging(args))
				{
					_ToatlTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreadtedBy
		{	
			get{ return _CreadtedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreadtedBy, value, _CreadtedBy);
				if (PropertyChanging(args))
				{
					_CreadtedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> GeeseCount
		{	
			get{ return _GeeseCount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GeeseCount, value, _GeeseCount);
				if (PropertyChanging(args))
				{
					_GeeseCount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCheckLogBase Clone()
		{
			CustomerCheckLogBase newObj = new  CustomerCheckLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.RouteId = this.RouteId;						
			newObj.CheckInTime = this.CheckInTime;						
			newObj.CheckOutTime = this.CheckOutTime;						
			newObj.CheckType = this.CheckType;						
			newObj.ToatlTime = this.ToatlTime;						
			newObj.CreadtedBy = this.CreadtedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.GeeseCount = this.GeeseCount;						
			newObj.UserId = this.UserId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCheckLogBase.Property_Id, Id);				
			info.AddValue(CustomerCheckLogBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCheckLogBase.Property_RouteId, RouteId);				
			info.AddValue(CustomerCheckLogBase.Property_CheckInTime, CheckInTime);				
			info.AddValue(CustomerCheckLogBase.Property_CheckOutTime, CheckOutTime);				
			info.AddValue(CustomerCheckLogBase.Property_CheckType, CheckType);				
			info.AddValue(CustomerCheckLogBase.Property_ToatlTime, ToatlTime);				
			info.AddValue(CustomerCheckLogBase.Property_CreadtedBy, CreadtedBy);				
			info.AddValue(CustomerCheckLogBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerCheckLogBase.Property_GeeseCount, GeeseCount);				
			info.AddValue(CustomerCheckLogBase.Property_UserId, UserId);				
		}
		#endregion

		
	}
}
