using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "QAFinanceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class QAFinanceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			IsUnderstandTwoPayment = 3,
			UnderstandPaymentReasonNote = 4,
			IsUnderstandSmartHomeInstall = 5,
			FinancedAmount = 6,
			FinancedAmountIsCorrect = 7,
			FinancedAmountUpdateNote = 8,
			IsUnderstandServicePayment = 9,
			IsMinimumEquipmentMonthlyPayment = 10,
			IsUnderstandMinimumMonthlyPayment = 11,
			IsUnderstandSameAsCash = 12,
			IsUnderstandInterestAccrues = 13,
			IsAnyQuestion = 14,
			ManualNote = 15,
			CreatedBy = 16,
			CreatedByUid = 17,
			CreatedDate = 18,
			LastUpdatedByUid = 19,
			LastUpdatedDate = 20,
			IsCompleted = 21
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsUnderstandTwoPayment = "IsUnderstandTwoPayment";		            
		public const string Property_UnderstandPaymentReasonNote = "UnderstandPaymentReasonNote";		            
		public const string Property_IsUnderstandSmartHomeInstall = "IsUnderstandSmartHomeInstall";		            
		public const string Property_FinancedAmount = "FinancedAmount";		            
		public const string Property_FinancedAmountIsCorrect = "FinancedAmountIsCorrect";		            
		public const string Property_FinancedAmountUpdateNote = "FinancedAmountUpdateNote";		            
		public const string Property_IsUnderstandServicePayment = "IsUnderstandServicePayment";		            
		public const string Property_IsMinimumEquipmentMonthlyPayment = "IsMinimumEquipmentMonthlyPayment";		            
		public const string Property_IsUnderstandMinimumMonthlyPayment = "IsUnderstandMinimumMonthlyPayment";		            
		public const string Property_IsUnderstandSameAsCash = "IsUnderstandSameAsCash";		            
		public const string Property_IsUnderstandInterestAccrues = "IsUnderstandInterestAccrues";		            
		public const string Property_IsAnyQuestion = "IsAnyQuestion";		            
		public const string Property_ManualNote = "ManualNote";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_IsCompleted = "IsCompleted";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _IsUnderstandTwoPayment;	            
		private String _UnderstandPaymentReasonNote;	            
		private String _IsUnderstandSmartHomeInstall;	            
		private Nullable<Double> _FinancedAmount;	            
		private String _FinancedAmountIsCorrect;	            
		private String _FinancedAmountUpdateNote;	            
		private String _IsUnderstandServicePayment;	            
		private String _IsMinimumEquipmentMonthlyPayment;	            
		private String _IsUnderstandMinimumMonthlyPayment;	            
		private String _IsUnderstandSameAsCash;	            
		private String _IsUnderstandInterestAccrues;	            
		private String _IsAnyQuestion;	            
		private String _ManualNote;	            
		private String _CreatedBy;	            
		private Guid _CreatedByUid;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Boolean _IsCompleted;	            
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
		public String IsUnderstandTwoPayment
		{	
			get{ return _IsUnderstandTwoPayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandTwoPayment, value, _IsUnderstandTwoPayment);
				if (PropertyChanging(args))
				{
					_IsUnderstandTwoPayment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UnderstandPaymentReasonNote
		{	
			get{ return _UnderstandPaymentReasonNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnderstandPaymentReasonNote, value, _UnderstandPaymentReasonNote);
				if (PropertyChanging(args))
				{
					_UnderstandPaymentReasonNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsUnderstandSmartHomeInstall
		{	
			get{ return _IsUnderstandSmartHomeInstall; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandSmartHomeInstall, value, _IsUnderstandSmartHomeInstall);
				if (PropertyChanging(args))
				{
					_IsUnderstandSmartHomeInstall = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FinancedAmount
		{	
			get{ return _FinancedAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinancedAmount, value, _FinancedAmount);
				if (PropertyChanging(args))
				{
					_FinancedAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FinancedAmountIsCorrect
		{	
			get{ return _FinancedAmountIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinancedAmountIsCorrect, value, _FinancedAmountIsCorrect);
				if (PropertyChanging(args))
				{
					_FinancedAmountIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FinancedAmountUpdateNote
		{	
			get{ return _FinancedAmountUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinancedAmountUpdateNote, value, _FinancedAmountUpdateNote);
				if (PropertyChanging(args))
				{
					_FinancedAmountUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsUnderstandServicePayment
		{	
			get{ return _IsUnderstandServicePayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandServicePayment, value, _IsUnderstandServicePayment);
				if (PropertyChanging(args))
				{
					_IsUnderstandServicePayment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsMinimumEquipmentMonthlyPayment
		{	
			get{ return _IsMinimumEquipmentMonthlyPayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsMinimumEquipmentMonthlyPayment, value, _IsMinimumEquipmentMonthlyPayment);
				if (PropertyChanging(args))
				{
					_IsMinimumEquipmentMonthlyPayment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsUnderstandMinimumMonthlyPayment
		{	
			get{ return _IsUnderstandMinimumMonthlyPayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandMinimumMonthlyPayment, value, _IsUnderstandMinimumMonthlyPayment);
				if (PropertyChanging(args))
				{
					_IsUnderstandMinimumMonthlyPayment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsUnderstandSameAsCash
		{	
			get{ return _IsUnderstandSameAsCash; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandSameAsCash, value, _IsUnderstandSameAsCash);
				if (PropertyChanging(args))
				{
					_IsUnderstandSameAsCash = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsUnderstandInterestAccrues
		{	
			get{ return _IsUnderstandInterestAccrues; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandInterestAccrues, value, _IsUnderstandInterestAccrues);
				if (PropertyChanging(args))
				{
					_IsUnderstandInterestAccrues = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsAnyQuestion
		{	
			get{ return _IsAnyQuestion; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAnyQuestion, value, _IsAnyQuestion);
				if (PropertyChanging(args))
				{
					_IsAnyQuestion = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ManualNote
		{	
			get{ return _ManualNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManualNote, value, _ManualNote);
				if (PropertyChanging(args))
				{
					_ManualNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreatedBy
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
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
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
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
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
		public Boolean IsCompleted
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

		#endregion
		
		#region Cloning Base Objects
		public  QAFinanceBase Clone()
		{
			QAFinanceBase newObj = new  QAFinanceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsUnderstandTwoPayment = this.IsUnderstandTwoPayment;						
			newObj.UnderstandPaymentReasonNote = this.UnderstandPaymentReasonNote;						
			newObj.IsUnderstandSmartHomeInstall = this.IsUnderstandSmartHomeInstall;						
			newObj.FinancedAmount = this.FinancedAmount;						
			newObj.FinancedAmountIsCorrect = this.FinancedAmountIsCorrect;						
			newObj.FinancedAmountUpdateNote = this.FinancedAmountUpdateNote;						
			newObj.IsUnderstandServicePayment = this.IsUnderstandServicePayment;						
			newObj.IsMinimumEquipmentMonthlyPayment = this.IsMinimumEquipmentMonthlyPayment;						
			newObj.IsUnderstandMinimumMonthlyPayment = this.IsUnderstandMinimumMonthlyPayment;						
			newObj.IsUnderstandSameAsCash = this.IsUnderstandSameAsCash;						
			newObj.IsUnderstandInterestAccrues = this.IsUnderstandInterestAccrues;						
			newObj.IsAnyQuestion = this.IsAnyQuestion;						
			newObj.ManualNote = this.ManualNote;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.IsCompleted = this.IsCompleted;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(QAFinanceBase.Property_Id, Id);				
			info.AddValue(QAFinanceBase.Property_CustomerId, CustomerId);				
			info.AddValue(QAFinanceBase.Property_CompanyId, CompanyId);				
			info.AddValue(QAFinanceBase.Property_IsUnderstandTwoPayment, IsUnderstandTwoPayment);				
			info.AddValue(QAFinanceBase.Property_UnderstandPaymentReasonNote, UnderstandPaymentReasonNote);				
			info.AddValue(QAFinanceBase.Property_IsUnderstandSmartHomeInstall, IsUnderstandSmartHomeInstall);				
			info.AddValue(QAFinanceBase.Property_FinancedAmount, FinancedAmount);				
			info.AddValue(QAFinanceBase.Property_FinancedAmountIsCorrect, FinancedAmountIsCorrect);				
			info.AddValue(QAFinanceBase.Property_FinancedAmountUpdateNote, FinancedAmountUpdateNote);				
			info.AddValue(QAFinanceBase.Property_IsUnderstandServicePayment, IsUnderstandServicePayment);				
			info.AddValue(QAFinanceBase.Property_IsMinimumEquipmentMonthlyPayment, IsMinimumEquipmentMonthlyPayment);				
			info.AddValue(QAFinanceBase.Property_IsUnderstandMinimumMonthlyPayment, IsUnderstandMinimumMonthlyPayment);				
			info.AddValue(QAFinanceBase.Property_IsUnderstandSameAsCash, IsUnderstandSameAsCash);				
			info.AddValue(QAFinanceBase.Property_IsUnderstandInterestAccrues, IsUnderstandInterestAccrues);				
			info.AddValue(QAFinanceBase.Property_IsAnyQuestion, IsAnyQuestion);				
			info.AddValue(QAFinanceBase.Property_ManualNote, ManualNote);				
			info.AddValue(QAFinanceBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(QAFinanceBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(QAFinanceBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(QAFinanceBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(QAFinanceBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(QAFinanceBase.Property_IsCompleted, IsCompleted);				
		}
		#endregion

		
	}
}
