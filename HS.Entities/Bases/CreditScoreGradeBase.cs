using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CreditScoreGradeBase", Namespace = "http://www.piistech.com//entities")]
	public class CreditScoreGradeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			ID = 0,
			CreditGradeId = 1,
			MinScore = 2,
			MaxScore = 3,
			Grade = 4,
			CreatedBy = 5,
			CreatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_ID = "ID";		            
		public const string Property_CreditGradeId = "CreditGradeId";		            
		public const string Property_MinScore = "MinScore";		            
		public const string Property_MaxScore = "MaxScore";		            
		public const string Property_Grade = "Grade";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _ID;	            
		private Guid _CreditGradeId;	            
		private Nullable<Int32> _MinScore;	            
		private Nullable<Int32> _MaxScore;	            
		private String _Grade;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public Guid CreditGradeId
		{	
			get{ return _CreditGradeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditGradeId, value, _CreditGradeId);
				if (PropertyChanging(args))
				{
					_CreditGradeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> MinScore
		{	
			get{ return _MinScore; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinScore, value, _MinScore);
				if (PropertyChanging(args))
				{
					_MinScore = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> MaxScore
		{	
			get{ return _MaxScore; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaxScore, value, _MaxScore);
				if (PropertyChanging(args))
				{
					_MaxScore = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Grade
		{	
			get{ return _Grade; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Grade, value, _Grade);
				if (PropertyChanging(args))
				{
					_Grade = value;
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
		public  CreditScoreGradeBase Clone()
		{
			CreditScoreGradeBase newObj = new  CreditScoreGradeBase();
			base.CloneBase(newObj);
			newObj.ID = this.ID;						
			newObj.CreditGradeId = this.CreditGradeId;						
			newObj.MinScore = this.MinScore;						
			newObj.MaxScore = this.MaxScore;						
			newObj.Grade = this.Grade;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CreditScoreGradeBase.Property_ID, ID);				
			info.AddValue(CreditScoreGradeBase.Property_CreditGradeId, CreditGradeId);				
			info.AddValue(CreditScoreGradeBase.Property_MinScore, MinScore);				
			info.AddValue(CreditScoreGradeBase.Property_MaxScore, MaxScore);				
			info.AddValue(CreditScoreGradeBase.Property_Grade, Grade);				
			info.AddValue(CreditScoreGradeBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CreditScoreGradeBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
