using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AlarmCustomerTerminationBase", Namespace = "http://www.piistech.com//entities")]
	public class AlarmCustomerTerminationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			AlarmId = 2,
			TerminationDate = 3,
			TerminationReason = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			IsMsgSend = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_AlarmId = "AlarmId";		            
		public const string Property_TerminationDate = "TerminationDate";		            
		public const string Property_TerminationReason = "TerminationReason";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsMsgSend = "IsMsgSend";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Int32 _AlarmId;	            
		private DateTime _TerminationDate;	            
		private String _TerminationReason;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsMsgSend;	            
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
		public Int32 AlarmId
		{	
			get{ return _AlarmId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlarmId, value, _AlarmId);
				if (PropertyChanging(args))
				{
					_AlarmId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime TerminationDate
		{	
			get{ return _TerminationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TerminationDate, value, _TerminationDate);
				if (PropertyChanging(args))
				{
					_TerminationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TerminationReason
		{	
			get{ return _TerminationReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TerminationReason, value, _TerminationReason);
				if (PropertyChanging(args))
				{
					_TerminationReason = value;
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
		public Nullable<Boolean> IsMsgSend
		{	
			get{ return _IsMsgSend; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsMsgSend, value, _IsMsgSend);
				if (PropertyChanging(args))
				{
					_IsMsgSend = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AlarmCustomerTerminationBase Clone()
		{
			AlarmCustomerTerminationBase newObj = new  AlarmCustomerTerminationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.AlarmId = this.AlarmId;						
			newObj.TerminationDate = this.TerminationDate;						
			newObj.TerminationReason = this.TerminationReason;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsMsgSend = this.IsMsgSend;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AlarmCustomerTerminationBase.Property_Id, Id);				
			info.AddValue(AlarmCustomerTerminationBase.Property_CustomerId, CustomerId);				
			info.AddValue(AlarmCustomerTerminationBase.Property_AlarmId, AlarmId);				
			info.AddValue(AlarmCustomerTerminationBase.Property_TerminationDate, TerminationDate);				
			info.AddValue(AlarmCustomerTerminationBase.Property_TerminationReason, TerminationReason);				
			info.AddValue(AlarmCustomerTerminationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(AlarmCustomerTerminationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(AlarmCustomerTerminationBase.Property_IsMsgSend, IsMsgSend);				
		}
		#endregion

		
	}
}
