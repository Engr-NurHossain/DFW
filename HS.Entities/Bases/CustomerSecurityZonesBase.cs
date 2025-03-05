using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerSecurityZonesBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerSecurityZonesBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			CustomerId = 1,
			ZoneNumber = 2,
			EventCode = 3,
			Location = 4,
			Platform = 5,
			CreatedDate = 6,
			CreatedBy = 7,
			EquipmentType = 8,
			ZoneComment = 9,
			SignalCode = 10,
			SignalStatus = 11,
			AreaNum = 12
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_ZoneNumber = "ZoneNumber";		            
		public const string Property_EventCode = "EventCode";		            
		public const string Property_Location = "Location";		            
		public const string Property_Platform = "Platform";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_EquipmentType = "EquipmentType";		            
		public const string Property_ZoneComment = "ZoneComment";		            
		public const string Property_SignalCode = "SignalCode";		            
		public const string Property_SignalStatus = "SignalStatus";		            
		public const string Property_AreaNum = "AreaNum";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private Guid _CustomerId;	            
		private String _ZoneNumber;	            
		private String _EventCode;	            
		private String _Location;	            
		private String _Platform;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _CreatedBy;	            
		private String _EquipmentType;	            
		private String _ZoneComment;	            
		private String _SignalCode;	            
		private String _SignalStatus;	            
		private Nullable<Int32> _AreaNum;	            
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
		public String ZoneNumber
		{	
			get{ return _ZoneNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZoneNumber, value, _ZoneNumber);
				if (PropertyChanging(args))
				{
					_ZoneNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EventCode
		{	
			get{ return _EventCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EventCode, value, _EventCode);
				if (PropertyChanging(args))
				{
					_EventCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Location
		{	
			get{ return _Location; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Location, value, _Location);
				if (PropertyChanging(args))
				{
					_Location = value;
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

		[DataMember]
		public Nullable<DateTime> CreatedDate
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
		public Guid CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
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
		public String ZoneComment
		{	
			get{ return _ZoneComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZoneComment, value, _ZoneComment);
				if (PropertyChanging(args))
				{
					_ZoneComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SignalCode
		{	
			get{ return _SignalCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SignalCode, value, _SignalCode);
				if (PropertyChanging(args))
				{
					_SignalCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SignalStatus
		{	
			get{ return _SignalStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SignalStatus, value, _SignalStatus);
				if (PropertyChanging(args))
				{
					_SignalStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AreaNum
		{	
			get{ return _AreaNum; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AreaNum, value, _AreaNum);
				if (PropertyChanging(args))
				{
					_AreaNum = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerSecurityZonesBase Clone()
		{
			CustomerSecurityZonesBase newObj = new  CustomerSecurityZonesBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.CustomerId = this.CustomerId;						
			newObj.ZoneNumber = this.ZoneNumber;						
			newObj.EventCode = this.EventCode;						
			newObj.Location = this.Location;						
			newObj.Platform = this.Platform;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.EquipmentType = this.EquipmentType;						
			newObj.ZoneComment = this.ZoneComment;						
			newObj.SignalCode = this.SignalCode;						
			newObj.SignalStatus = this.SignalStatus;						
			newObj.AreaNum = this.AreaNum;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerSecurityZonesBase.Property_ID, ID);				
			info.AddValue(CustomerSecurityZonesBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerSecurityZonesBase.Property_ZoneNumber, ZoneNumber);				
			info.AddValue(CustomerSecurityZonesBase.Property_EventCode, EventCode);				
			info.AddValue(CustomerSecurityZonesBase.Property_Location, Location);				
			info.AddValue(CustomerSecurityZonesBase.Property_Platform, Platform);				
			info.AddValue(CustomerSecurityZonesBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerSecurityZonesBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerSecurityZonesBase.Property_EquipmentType, EquipmentType);				
			info.AddValue(CustomerSecurityZonesBase.Property_ZoneComment, ZoneComment);				
			info.AddValue(CustomerSecurityZonesBase.Property_SignalCode, SignalCode);				
			info.AddValue(CustomerSecurityZonesBase.Property_SignalStatus, SignalStatus);				
			info.AddValue(CustomerSecurityZonesBase.Property_AreaNum, AreaNum);				
		}
		#endregion

		
	}
}
