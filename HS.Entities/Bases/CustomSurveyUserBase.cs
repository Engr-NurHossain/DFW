using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomSurveyUserBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomSurveyUserBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SurveyId = 1,
			UserId = 2,
			SurveyUserId = 3,
			AddedBy = 4,
			AddedDate = 5,
			Status = 6,
			ReferenceId = 7,
			ViewedDate = 8,
			UserIP = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SurveyId = "SurveyId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_SurveyUserId = "SurveyUserId";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_Status = "Status";		            
		public const string Property_ReferenceId = "ReferenceId";		            
		public const string Property_ViewedDate = "ViewedDate";		            
		public const string Property_UserIP = "UserIP";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SurveyId;	            
		private Guid _UserId;	            
		private Guid _SurveyUserId;	            
		private Guid _AddedBy;	            
		private DateTime _AddedDate;	            
		private String _Status;	            
		private String _ReferenceId;	            
		private Nullable<DateTime> _ViewedDate;	            
		private String _UserIP;	            
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
		public Guid SurveyId
		{	
			get{ return _SurveyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SurveyId, value, _SurveyId);
				if (PropertyChanging(args))
				{
					_SurveyId = value;
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
		public Guid SurveyUserId
		{	
			get{ return _SurveyUserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SurveyUserId, value, _SurveyUserId);
				if (PropertyChanging(args))
				{
					_SurveyUserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
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
		public String ReferenceId
		{	
			get{ return _ReferenceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceId, value, _ReferenceId);
				if (PropertyChanging(args))
				{
					_ReferenceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ViewedDate
		{	
			get{ return _ViewedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ViewedDate, value, _ViewedDate);
				if (PropertyChanging(args))
				{
					_ViewedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserIP
		{	
			get{ return _UserIP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserIP, value, _UserIP);
				if (PropertyChanging(args))
				{
					_UserIP = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomSurveyUserBase Clone()
		{
			CustomSurveyUserBase newObj = new  CustomSurveyUserBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SurveyId = this.SurveyId;						
			newObj.UserId = this.UserId;						
			newObj.SurveyUserId = this.SurveyUserId;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.Status = this.Status;						
			newObj.ReferenceId = this.ReferenceId;						
			newObj.ViewedDate = this.ViewedDate;						
			newObj.UserIP = this.UserIP;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomSurveyUserBase.Property_Id, Id);				
			info.AddValue(CustomSurveyUserBase.Property_SurveyId, SurveyId);				
			info.AddValue(CustomSurveyUserBase.Property_UserId, UserId);				
			info.AddValue(CustomSurveyUserBase.Property_SurveyUserId, SurveyUserId);				
			info.AddValue(CustomSurveyUserBase.Property_AddedBy, AddedBy);				
			info.AddValue(CustomSurveyUserBase.Property_AddedDate, AddedDate);				
			info.AddValue(CustomSurveyUserBase.Property_Status, Status);				
			info.AddValue(CustomSurveyUserBase.Property_ReferenceId, ReferenceId);				
			info.AddValue(CustomSurveyUserBase.Property_ViewedDate, ViewedDate);				
			info.AddValue(CustomSurveyUserBase.Property_UserIP, UserIP);				
		}
		#endregion

		
	}
}
