using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketTimeClockBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketTimeClockBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TicketId = 1,
			UserId = 2,
			Time = 3,
			Type = 4,
			Lat = 5,
			Lng = 6,
			Note = 7,
			CreatedBy = 8,
			ClockedInMinutes = 9,
			LastUpdateBy = 10,
			LastUpdatedDate = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_Time = "Time";		            
		public const string Property_Type = "Type";		            
		public const string Property_Lat = "Lat";		            
		public const string Property_Lng = "Lng";		            
		public const string Property_Note = "Note";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_ClockedInMinutes = "ClockedInMinutes";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TicketId;	            
		private Guid _UserId;	            
		private DateTime _Time;	            
		private String _Type;	            
		private String _Lat;	            
		private String _Lng;	            
		private String _Note;	            
		private Guid _CreatedBy;	            
		private Nullable<Int32> _ClockedInMinutes;	            
		private Guid _LastUpdateBy;	            
		private DateTime _LastUpdatedDate;	            
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

		[DataMember]
		public DateTime Time
		{	
			get{ return _Time; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Time, value, _Time);
				if (PropertyChanging(args))
				{
					_Time = value;
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
		public String Lat
		{	
			get{ return _Lat; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Lat, value, _Lat);
				if (PropertyChanging(args))
				{
					_Lat = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Lng
		{	
			get{ return _Lng; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Lng, value, _Lng);
				if (PropertyChanging(args))
				{
					_Lng = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
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
		public Nullable<Int32> ClockedInMinutes
		{	
			get{ return _ClockedInMinutes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockedInMinutes, value, _ClockedInMinutes);
				if (PropertyChanging(args))
				{
					_ClockedInMinutes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdateBy
		{	
			get{ return _LastUpdateBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateBy, value, _LastUpdateBy);
				if (PropertyChanging(args))
				{
					_LastUpdateBy = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  TicketTimeClockBase Clone()
		{
			TicketTimeClockBase newObj = new  TicketTimeClockBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TicketId = this.TicketId;						
			newObj.UserId = this.UserId;						
			newObj.Time = this.Time;						
			newObj.Type = this.Type;						
			newObj.Lat = this.Lat;						
			newObj.Lng = this.Lng;						
			newObj.Note = this.Note;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.ClockedInMinutes = this.ClockedInMinutes;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketTimeClockBase.Property_Id, Id);				
			info.AddValue(TicketTimeClockBase.Property_TicketId, TicketId);				
			info.AddValue(TicketTimeClockBase.Property_UserId, UserId);				
			info.AddValue(TicketTimeClockBase.Property_Time, Time);				
			info.AddValue(TicketTimeClockBase.Property_Type, Type);				
			info.AddValue(TicketTimeClockBase.Property_Lat, Lat);				
			info.AddValue(TicketTimeClockBase.Property_Lng, Lng);				
			info.AddValue(TicketTimeClockBase.Property_Note, Note);				
			info.AddValue(TicketTimeClockBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TicketTimeClockBase.Property_ClockedInMinutes, ClockedInMinutes);				
			info.AddValue(TicketTimeClockBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(TicketTimeClockBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
