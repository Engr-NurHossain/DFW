using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ScheduleBase", Namespace = "http://www.piistech.com//entities")]
	public class ScheduleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Type = 2,
			StartDate = 3,
			EndDate = 4,
			Title = 5,
			IsCompleted = 6,
			LeadId = 7,
			IsFullDay = 8,
			Identifier = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Type = "Type";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_Title = "Title";		            
		public const string Property_IsCompleted = "IsCompleted";		            
		public const string Property_LeadId = "LeadId";		            
		public const string Property_IsFullDay = "IsFullDay";		            
		public const string Property_Identifier = "Identifier";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Type;	            
		private Nullable<DateTime> _StartDate;	            
		private Nullable<DateTime> _EndDate;	            
		private String _Title;	            
		private Nullable<Boolean> _IsCompleted;	            
		private Nullable<Int32> _LeadId;	            
		private Nullable<Boolean> _IsFullDay;	            
		private String _Identifier;	            
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
		public Nullable<DateTime> StartDate
		{	
			get{ return _StartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDate, value, _StartDate);
				if (PropertyChanging(args))
				{
					_StartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
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
		public Nullable<Boolean> IsCompleted
		{	
			get{ return _IsCompleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCompleted, value, _IsCompleted);
				if (PropertyChanging(args))
				{
					_IsCompleted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> LeadId
		{	
			get{ return _LeadId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadId, value, _LeadId);
				if (PropertyChanging(args))
				{
					_LeadId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsFullDay
		{	
			get{ return _IsFullDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFullDay, value, _IsFullDay);
				if (PropertyChanging(args))
				{
					_IsFullDay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Identifier
		{	
			get{ return _Identifier; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Identifier, value, _Identifier);
				if (PropertyChanging(args))
				{
					_Identifier = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ScheduleBase Clone()
		{
			ScheduleBase newObj = new  ScheduleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Type = this.Type;						
			newObj.StartDate = this.StartDate;						
			newObj.EndDate = this.EndDate;						
			newObj.Title = this.Title;						
			newObj.IsCompleted = this.IsCompleted;						
			newObj.LeadId = this.LeadId;						
			newObj.IsFullDay = this.IsFullDay;						
			newObj.Identifier = this.Identifier;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ScheduleBase.Property_Id, Id);				
			info.AddValue(ScheduleBase.Property_CompanyId, CompanyId);				
			info.AddValue(ScheduleBase.Property_Type, Type);				
			info.AddValue(ScheduleBase.Property_StartDate, StartDate);				
			info.AddValue(ScheduleBase.Property_EndDate, EndDate);				
			info.AddValue(ScheduleBase.Property_Title, Title);				
			info.AddValue(ScheduleBase.Property_IsCompleted, IsCompleted);				
			info.AddValue(ScheduleBase.Property_LeadId, LeadId);				
			info.AddValue(ScheduleBase.Property_IsFullDay, IsFullDay);				
			info.AddValue(ScheduleBase.Property_Identifier, Identifier);				
		}
		#endregion

		
	}
}
