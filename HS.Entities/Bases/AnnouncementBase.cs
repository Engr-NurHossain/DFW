using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AnnouncementBase", Namespace = "http://www.piistech.com//entities")]
	public class AnnouncementBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Title = 2,
			Message = 3,
			StartTime = 4,
			EndTime = 5,
			CreatedBy = 6,
			CreatedDate = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Title = "Title";		            
		public const string Property_Message = "Message";		            
		public const string Property_StartTime = "StartTime";		            
		public const string Property_EndTime = "EndTime";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Title;	            
		private String _Message;	            
		private DateTime _StartTime;	            
		private DateTime _EndTime;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public String Title
		{	
			get{ return _Title; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Title, value, _Title);
				if (PropertyChanging(args))
				{
					_Title = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Message
		{	
			get{ return _Message; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Message, value, _Message);
				if (PropertyChanging(args))
				{
					_Message = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime StartTime
		{	
			get{ return _StartTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartTime, value, _StartTime);
				if (PropertyChanging(args))
				{
					_StartTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime EndTime
		{	
			get{ return _EndTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndTime, value, _EndTime);
				if (PropertyChanging(args))
				{
					_EndTime = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  AnnouncementBase Clone()
		{
			AnnouncementBase newObj = new  AnnouncementBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Title = this.Title;						
			newObj.Message = this.Message;						
			newObj.StartTime = this.StartTime;						
			newObj.EndTime = this.EndTime;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AnnouncementBase.Property_Id, Id);				
			info.AddValue(AnnouncementBase.Property_CompanyId, CompanyId);				
			info.AddValue(AnnouncementBase.Property_Title, Title);				
			info.AddValue(AnnouncementBase.Property_Message, Message);				
			info.AddValue(AnnouncementBase.Property_StartTime, StartTime);				
			info.AddValue(AnnouncementBase.Property_EndTime, EndTime);				
			info.AddValue(AnnouncementBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(AnnouncementBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
