using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RescheduleTicketBase", Namespace = "http://www.piistech.com//entities")]
	public class RescheduleTicketBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			RescheduleId = 1,
			TicketId = 2,
			Reason = 3,
			IsPay = 4,
			CreatedBy = 5,
			CreatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_RescheduleId = "RescheduleId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_Reason = "Reason";		            
		public const string Property_IsPay = "IsPay";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _RescheduleId;	            
		private Guid _TicketId;	            
		private String _Reason;	            
		private Nullable<Boolean> _IsPay;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
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
		public Guid RescheduleId
		{	
			get{ return _RescheduleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RescheduleId, value, _RescheduleId);
				if (PropertyChanging(args))
				{
					_RescheduleId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Reason
		{	
			get{ return _Reason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reason, value, _Reason);
				if (PropertyChanging(args))
				{
					_Reason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPay
		{	
			get{ return _IsPay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPay, value, _IsPay);
				if (PropertyChanging(args))
				{
					_IsPay = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RescheduleTicketBase Clone()
		{
			RescheduleTicketBase newObj = new  RescheduleTicketBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.RescheduleId = this.RescheduleId;						
			newObj.TicketId = this.TicketId;						
			newObj.Reason = this.Reason;						
			newObj.IsPay = this.IsPay;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RescheduleTicketBase.Property_Id, Id);				
			info.AddValue(RescheduleTicketBase.Property_RescheduleId, RescheduleId);				
			info.AddValue(RescheduleTicketBase.Property_TicketId, TicketId);				
			info.AddValue(RescheduleTicketBase.Property_Reason, Reason);				
			info.AddValue(RescheduleTicketBase.Property_IsPay, IsPay);				
			info.AddValue(RescheduleTicketBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RescheduleTicketBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
