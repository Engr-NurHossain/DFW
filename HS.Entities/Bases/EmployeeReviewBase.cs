using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeReviewBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeReviewBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ReviewId = 1,
			CompanyId = 2,
			UserId = 3,
			EmpName = 4,
			JobTitle = 5,
			Department = 6,
			ReviewPeriod = 7,
			ReviewDate = 8,
			Manager = 9,
			NextReview = 10,
			JobKnowledge = 11,
			JobKnowledgeComments = 12,
			WorkQuality = 13,
			WorkQualityComments = 14,
			Attendance = 15,
			AttendanceComments = 16,
			Initiative = 17,
			InitiativeComments = 18,
			CommunicationSkills = 19,
			CommunicationSkillsComments = 20,
			Dependability = 21,
			DependabilityComments = 22,
			OverallRating = 23,
			AdditionalComments = 24,
			Goals = 25,
			EmpSignature = 26,
			EmpSignatureDate = 27,
			ManagerSignature = 28,
			ManagerSignatureDate = 29,
			CreatedBy = 30,
			CreatedDate = 31,
			ReviewedBy = 32,
			ReviewedDate = 33,
			LastUpdatedBy = 34,
			LastUpdatedDate = 35
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ReviewId = "ReviewId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_EmpName = "EmpName";		            
		public const string Property_JobTitle = "JobTitle";		            
		public const string Property_Department = "Department";		            
		public const string Property_ReviewPeriod = "ReviewPeriod";		            
		public const string Property_ReviewDate = "ReviewDate";		            
		public const string Property_Manager = "Manager";		            
		public const string Property_NextReview = "NextReview";		            
		public const string Property_JobKnowledge = "JobKnowledge";		            
		public const string Property_JobKnowledgeComments = "JobKnowledgeComments";		            
		public const string Property_WorkQuality = "WorkQuality";		            
		public const string Property_WorkQualityComments = "WorkQualityComments";		            
		public const string Property_Attendance = "Attendance";		            
		public const string Property_AttendanceComments = "AttendanceComments";		            
		public const string Property_Initiative = "Initiative";		            
		public const string Property_InitiativeComments = "InitiativeComments";		            
		public const string Property_CommunicationSkills = "CommunicationSkills";		            
		public const string Property_CommunicationSkillsComments = "CommunicationSkillsComments";		            
		public const string Property_Dependability = "Dependability";		            
		public const string Property_DependabilityComments = "DependabilityComments";		            
		public const string Property_OverallRating = "OverallRating";		            
		public const string Property_AdditionalComments = "AdditionalComments";		            
		public const string Property_Goals = "Goals";		            
		public const string Property_EmpSignature = "EmpSignature";		            
		public const string Property_EmpSignatureDate = "EmpSignatureDate";		            
		public const string Property_ManagerSignature = "ManagerSignature";		            
		public const string Property_ManagerSignatureDate = "ManagerSignatureDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_ReviewedBy = "ReviewedBy";		            
		public const string Property_ReviewedDate = "ReviewedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ReviewId;	            
		private Guid _CompanyId;	            
		private Guid _UserId;	            
		private String _EmpName;	            
		private String _JobTitle;	            
		private String _Department;	            
		private String _ReviewPeriod;	            
		private Nullable<DateTime> _ReviewDate;	            
		private String _Manager;	            
		private Nullable<DateTime> _NextReview;	            
		private Nullable<Int32> _JobKnowledge;	            
		private String _JobKnowledgeComments;	            
		private Nullable<Int32> _WorkQuality;	            
		private String _WorkQualityComments;	            
		private Nullable<Int32> _Attendance;	            
		private String _AttendanceComments;	            
		private Nullable<Int32> _Initiative;	            
		private String _InitiativeComments;	            
		private Nullable<Int32> _CommunicationSkills;	            
		private String _CommunicationSkillsComments;	            
		private Nullable<Int32> _Dependability;	            
		private String _DependabilityComments;	            
		private Nullable<Int32> _OverallRating;	            
		private String _AdditionalComments;	            
		private String _Goals;	            
		private String _EmpSignature;	            
		private Nullable<DateTime> _EmpSignatureDate;	            
		private String _ManagerSignature;	            
		private Nullable<DateTime> _ManagerSignatureDate;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _ReviewedBy;	            
		private Nullable<DateTime> _ReviewedDate;	            
		private Guid _LastUpdatedBy;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
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
		public Guid ReviewId
		{	
			get{ return _ReviewId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReviewId, value, _ReviewId);
				if (PropertyChanging(args))
				{
					_ReviewId = value;
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
		public String EmpName
		{	
			get{ return _EmpName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmpName, value, _EmpName);
				if (PropertyChanging(args))
				{
					_EmpName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobTitle
		{	
			get{ return _JobTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobTitle, value, _JobTitle);
				if (PropertyChanging(args))
				{
					_JobTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Department
		{	
			get{ return _Department; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Department, value, _Department);
				if (PropertyChanging(args))
				{
					_Department = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReviewPeriod
		{	
			get{ return _ReviewPeriod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReviewPeriod, value, _ReviewPeriod);
				if (PropertyChanging(args))
				{
					_ReviewPeriod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReviewDate
		{	
			get{ return _ReviewDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReviewDate, value, _ReviewDate);
				if (PropertyChanging(args))
				{
					_ReviewDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Manager
		{	
			get{ return _Manager; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Manager, value, _Manager);
				if (PropertyChanging(args))
				{
					_Manager = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> NextReview
		{	
			get{ return _NextReview; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NextReview, value, _NextReview);
				if (PropertyChanging(args))
				{
					_NextReview = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> JobKnowledge
		{	
			get{ return _JobKnowledge; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobKnowledge, value, _JobKnowledge);
				if (PropertyChanging(args))
				{
					_JobKnowledge = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobKnowledgeComments
		{	
			get{ return _JobKnowledgeComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobKnowledgeComments, value, _JobKnowledgeComments);
				if (PropertyChanging(args))
				{
					_JobKnowledgeComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> WorkQuality
		{	
			get{ return _WorkQuality; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WorkQuality, value, _WorkQuality);
				if (PropertyChanging(args))
				{
					_WorkQuality = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WorkQualityComments
		{	
			get{ return _WorkQualityComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WorkQualityComments, value, _WorkQualityComments);
				if (PropertyChanging(args))
				{
					_WorkQualityComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Attendance
		{	
			get{ return _Attendance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Attendance, value, _Attendance);
				if (PropertyChanging(args))
				{
					_Attendance = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AttendanceComments
		{	
			get{ return _AttendanceComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AttendanceComments, value, _AttendanceComments);
				if (PropertyChanging(args))
				{
					_AttendanceComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Initiative
		{	
			get{ return _Initiative; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Initiative, value, _Initiative);
				if (PropertyChanging(args))
				{
					_Initiative = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InitiativeComments
		{	
			get{ return _InitiativeComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InitiativeComments, value, _InitiativeComments);
				if (PropertyChanging(args))
				{
					_InitiativeComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CommunicationSkills
		{	
			get{ return _CommunicationSkills; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommunicationSkills, value, _CommunicationSkills);
				if (PropertyChanging(args))
				{
					_CommunicationSkills = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CommunicationSkillsComments
		{	
			get{ return _CommunicationSkillsComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommunicationSkillsComments, value, _CommunicationSkillsComments);
				if (PropertyChanging(args))
				{
					_CommunicationSkillsComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Dependability
		{	
			get{ return _Dependability; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Dependability, value, _Dependability);
				if (PropertyChanging(args))
				{
					_Dependability = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DependabilityComments
		{	
			get{ return _DependabilityComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DependabilityComments, value, _DependabilityComments);
				if (PropertyChanging(args))
				{
					_DependabilityComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> OverallRating
		{	
			get{ return _OverallRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OverallRating, value, _OverallRating);
				if (PropertyChanging(args))
				{
					_OverallRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdditionalComments
		{	
			get{ return _AdditionalComments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdditionalComments, value, _AdditionalComments);
				if (PropertyChanging(args))
				{
					_AdditionalComments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Goals
		{	
			get{ return _Goals; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Goals, value, _Goals);
				if (PropertyChanging(args))
				{
					_Goals = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmpSignature
		{	
			get{ return _EmpSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmpSignature, value, _EmpSignature);
				if (PropertyChanging(args))
				{
					_EmpSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EmpSignatureDate
		{	
			get{ return _EmpSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmpSignatureDate, value, _EmpSignatureDate);
				if (PropertyChanging(args))
				{
					_EmpSignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ManagerSignature
		{	
			get{ return _ManagerSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManagerSignature, value, _ManagerSignature);
				if (PropertyChanging(args))
				{
					_ManagerSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ManagerSignatureDate
		{	
			get{ return _ManagerSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManagerSignatureDate, value, _ManagerSignatureDate);
				if (PropertyChanging(args))
				{
					_ManagerSignatureDate = value;
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

		[DataMember]
		public Guid ReviewedBy
		{	
			get{ return _ReviewedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReviewedBy, value, _ReviewedBy);
				if (PropertyChanging(args))
				{
					_ReviewedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReviewedDate
		{	
			get{ return _ReviewedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReviewedDate, value, _ReviewedDate);
				if (PropertyChanging(args))
				{
					_ReviewedDate = value;
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
		public Nullable<DateTime> LastUpdatedDate
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
		public  EmployeeReviewBase Clone()
		{
			EmployeeReviewBase newObj = new  EmployeeReviewBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ReviewId = this.ReviewId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UserId = this.UserId;						
			newObj.EmpName = this.EmpName;						
			newObj.JobTitle = this.JobTitle;						
			newObj.Department = this.Department;						
			newObj.ReviewPeriod = this.ReviewPeriod;						
			newObj.ReviewDate = this.ReviewDate;						
			newObj.Manager = this.Manager;						
			newObj.NextReview = this.NextReview;						
			newObj.JobKnowledge = this.JobKnowledge;						
			newObj.JobKnowledgeComments = this.JobKnowledgeComments;						
			newObj.WorkQuality = this.WorkQuality;						
			newObj.WorkQualityComments = this.WorkQualityComments;						
			newObj.Attendance = this.Attendance;						
			newObj.AttendanceComments = this.AttendanceComments;						
			newObj.Initiative = this.Initiative;						
			newObj.InitiativeComments = this.InitiativeComments;						
			newObj.CommunicationSkills = this.CommunicationSkills;						
			newObj.CommunicationSkillsComments = this.CommunicationSkillsComments;						
			newObj.Dependability = this.Dependability;						
			newObj.DependabilityComments = this.DependabilityComments;						
			newObj.OverallRating = this.OverallRating;						
			newObj.AdditionalComments = this.AdditionalComments;						
			newObj.Goals = this.Goals;						
			newObj.EmpSignature = this.EmpSignature;						
			newObj.EmpSignatureDate = this.EmpSignatureDate;						
			newObj.ManagerSignature = this.ManagerSignature;						
			newObj.ManagerSignatureDate = this.ManagerSignatureDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.ReviewedBy = this.ReviewedBy;						
			newObj.ReviewedDate = this.ReviewedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeReviewBase.Property_Id, Id);				
			info.AddValue(EmployeeReviewBase.Property_ReviewId, ReviewId);				
			info.AddValue(EmployeeReviewBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeReviewBase.Property_UserId, UserId);				
			info.AddValue(EmployeeReviewBase.Property_EmpName, EmpName);				
			info.AddValue(EmployeeReviewBase.Property_JobTitle, JobTitle);				
			info.AddValue(EmployeeReviewBase.Property_Department, Department);				
			info.AddValue(EmployeeReviewBase.Property_ReviewPeriod, ReviewPeriod);				
			info.AddValue(EmployeeReviewBase.Property_ReviewDate, ReviewDate);				
			info.AddValue(EmployeeReviewBase.Property_Manager, Manager);				
			info.AddValue(EmployeeReviewBase.Property_NextReview, NextReview);				
			info.AddValue(EmployeeReviewBase.Property_JobKnowledge, JobKnowledge);				
			info.AddValue(EmployeeReviewBase.Property_JobKnowledgeComments, JobKnowledgeComments);				
			info.AddValue(EmployeeReviewBase.Property_WorkQuality, WorkQuality);				
			info.AddValue(EmployeeReviewBase.Property_WorkQualityComments, WorkQualityComments);				
			info.AddValue(EmployeeReviewBase.Property_Attendance, Attendance);				
			info.AddValue(EmployeeReviewBase.Property_AttendanceComments, AttendanceComments);				
			info.AddValue(EmployeeReviewBase.Property_Initiative, Initiative);				
			info.AddValue(EmployeeReviewBase.Property_InitiativeComments, InitiativeComments);				
			info.AddValue(EmployeeReviewBase.Property_CommunicationSkills, CommunicationSkills);				
			info.AddValue(EmployeeReviewBase.Property_CommunicationSkillsComments, CommunicationSkillsComments);				
			info.AddValue(EmployeeReviewBase.Property_Dependability, Dependability);				
			info.AddValue(EmployeeReviewBase.Property_DependabilityComments, DependabilityComments);				
			info.AddValue(EmployeeReviewBase.Property_OverallRating, OverallRating);				
			info.AddValue(EmployeeReviewBase.Property_AdditionalComments, AdditionalComments);				
			info.AddValue(EmployeeReviewBase.Property_Goals, Goals);				
			info.AddValue(EmployeeReviewBase.Property_EmpSignature, EmpSignature);				
			info.AddValue(EmployeeReviewBase.Property_EmpSignatureDate, EmpSignatureDate);				
			info.AddValue(EmployeeReviewBase.Property_ManagerSignature, ManagerSignature);				
			info.AddValue(EmployeeReviewBase.Property_ManagerSignatureDate, ManagerSignatureDate);				
			info.AddValue(EmployeeReviewBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeeReviewBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeReviewBase.Property_ReviewedBy, ReviewedBy);				
			info.AddValue(EmployeeReviewBase.Property_ReviewedDate, ReviewedDate);				
			info.AddValue(EmployeeReviewBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EmployeeReviewBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
