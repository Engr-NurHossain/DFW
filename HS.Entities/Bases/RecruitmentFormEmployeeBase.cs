using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RecruitmentFormEmployeeBase", Namespace = "http://www.piistech.com//entities")]
	public class RecruitmentFormEmployeeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			RecruitmentFormId = 1,
			EmployeeId = 2,
			FormId = 3,
			IsFillUp = 4,
			IsSubmitted = 5,
			FillDate = 6,
			SubmitDate = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_RecruitmentFormId = "RecruitmentFormId";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_FormId = "FormId";		            
		public const string Property_IsFillUp = "IsFillUp";		            
		public const string Property_IsSubmitted = "IsSubmitted";		            
		public const string Property_FillDate = "FillDate";		            
		public const string Property_SubmitDate = "SubmitDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _RecruitmentFormId;	            
		private Guid _EmployeeId;	            
		private Guid _FormId;	            
		private Boolean _IsFillUp;	            
		private Boolean _IsSubmitted;	            
		private Nullable<DateTime> _FillDate;	            
		private Nullable<DateTime> _SubmitDate;	            
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
		public Int32 RecruitmentFormId
		{	
			get{ return _RecruitmentFormId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecruitmentFormId, value, _RecruitmentFormId);
				if (PropertyChanging(args))
				{
					_RecruitmentFormId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid EmployeeId
		{	
			get{ return _EmployeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeId, value, _EmployeeId);
				if (PropertyChanging(args))
				{
					_EmployeeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid FormId
		{	
			get{ return _FormId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FormId, value, _FormId);
				if (PropertyChanging(args))
				{
					_FormId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsFillUp
		{	
			get{ return _IsFillUp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFillUp, value, _IsFillUp);
				if (PropertyChanging(args))
				{
					_IsFillUp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsSubmitted
		{	
			get{ return _IsSubmitted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSubmitted, value, _IsSubmitted);
				if (PropertyChanging(args))
				{
					_IsSubmitted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FillDate
		{	
			get{ return _FillDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FillDate, value, _FillDate);
				if (PropertyChanging(args))
				{
					_FillDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SubmitDate
		{	
			get{ return _SubmitDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SubmitDate, value, _SubmitDate);
				if (PropertyChanging(args))
				{
					_SubmitDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RecruitmentFormEmployeeBase Clone()
		{
			RecruitmentFormEmployeeBase newObj = new  RecruitmentFormEmployeeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.RecruitmentFormId = this.RecruitmentFormId;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.FormId = this.FormId;						
			newObj.IsFillUp = this.IsFillUp;						
			newObj.IsSubmitted = this.IsSubmitted;						
			newObj.FillDate = this.FillDate;						
			newObj.SubmitDate = this.SubmitDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RecruitmentFormEmployeeBase.Property_Id, Id);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_RecruitmentFormId, RecruitmentFormId);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_FormId, FormId);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_IsFillUp, IsFillUp);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_IsSubmitted, IsSubmitted);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_FillDate, FillDate);				
			info.AddValue(RecruitmentFormEmployeeBase.Property_SubmitDate, SubmitDate);				
		}
		#endregion

		
	}
}
