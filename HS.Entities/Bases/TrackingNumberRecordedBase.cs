using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TrackingNumberRecordedBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TrackingNumberRecordedBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TrackingId = 1,
			CallerId = 2,
			CompanyId = 3,
			TrackingNumber = 4,
			CallerNumber = 5,
			Status = 6,
			RecordDate = 7,
			CreatedBy = 8,
			LastUpdatedBy = 9,
			CreatedDate = 10,
			LastUpdatedDate = 11,
			CustomerId = 12,
			RecordFile = 13,
			TalkTimeSeconds = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TrackingId = "TrackingId";		            
		public const string Property_CallerId = "CallerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TrackingNumber = "TrackingNumber";		            
		public const string Property_CallerNumber = "CallerNumber";		            
		public const string Property_Status = "Status";		            
		public const string Property_RecordDate = "RecordDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_RecordFile = "RecordFile";		            
		public const string Property_TalkTimeSeconds = "TalkTimeSeconds";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TrackingId;	            
		private Guid _CallerId;	            
		private Guid _CompanyId;	            
		private String _TrackingNumber;	            
		private String _CallerNumber;	            
		private String _Status;	            
		private Nullable<DateTime> _RecordDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _CreatedDate;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _CustomerId;	            
		private String _RecordFile;	            
		private String _TalkTimeSeconds;	            
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
		public Guid TrackingId
		{	
			get{ return _TrackingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TrackingId, value, _TrackingId);
				if (PropertyChanging(args))
				{
					_TrackingId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CallerId
		{	
			get{ return _CallerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CallerId, value, _CallerId);
				if (PropertyChanging(args))
				{
					_CallerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CompanyId
		{	
			get{ return _CompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyId, value, _CompanyId);
				if (PropertyChanging(args))
				{
					_CompanyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TrackingNumber
		{	
			get{ return _TrackingNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TrackingNumber, value, _TrackingNumber);
				if (PropertyChanging(args))
				{
					_TrackingNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CallerNumber
		{	
			get{ return _CallerNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CallerNumber, value, _CallerNumber);
				if (PropertyChanging(args))
				{
					_CallerNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> RecordDate
		{	
			get{ return _RecordDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecordDate, value, _RecordDate);
				if (PropertyChanging(args))
				{
					_RecordDate = value;
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
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
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
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
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
		public String RecordFile
		{	
			get{ return _RecordFile; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecordFile, value, _RecordFile);
				if (PropertyChanging(args))
				{
					_RecordFile = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TalkTimeSeconds
		{	
			get{ return _TalkTimeSeconds; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TalkTimeSeconds, value, _TalkTimeSeconds);
				if (PropertyChanging(args))
				{
					_TalkTimeSeconds = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TrackingNumberRecordedBase Clone()
		{
			TrackingNumberRecordedBase newObj = new  TrackingNumberRecordedBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TrackingId = this.TrackingId;						
			newObj.CallerId = this.CallerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TrackingNumber = this.TrackingNumber;						
			newObj.CallerNumber = this.CallerNumber;						
			newObj.Status = this.Status;						
			newObj.RecordDate = this.RecordDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CustomerId = this.CustomerId;						
			newObj.RecordFile = this.RecordFile;						
			newObj.TalkTimeSeconds = this.TalkTimeSeconds;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TrackingNumberRecordedBase.Property_Id, Id);				
			info.AddValue(TrackingNumberRecordedBase.Property_TrackingId, TrackingId);				
			info.AddValue(TrackingNumberRecordedBase.Property_CallerId, CallerId);				
			info.AddValue(TrackingNumberRecordedBase.Property_CompanyId, CompanyId);				
			info.AddValue(TrackingNumberRecordedBase.Property_TrackingNumber, TrackingNumber);				
			info.AddValue(TrackingNumberRecordedBase.Property_CallerNumber, CallerNumber);				
			info.AddValue(TrackingNumberRecordedBase.Property_Status, Status);				
			info.AddValue(TrackingNumberRecordedBase.Property_RecordDate, RecordDate);				
			info.AddValue(TrackingNumberRecordedBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TrackingNumberRecordedBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(TrackingNumberRecordedBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TrackingNumberRecordedBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(TrackingNumberRecordedBase.Property_CustomerId, CustomerId);				
			info.AddValue(TrackingNumberRecordedBase.Property_RecordFile, RecordFile);				
			info.AddValue(TrackingNumberRecordedBase.Property_TalkTimeSeconds, TalkTimeSeconds);				
		}
		#endregion

		
	}
}
