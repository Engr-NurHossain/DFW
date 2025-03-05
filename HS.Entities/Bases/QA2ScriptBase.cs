using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "QA2ScriptBase", Namespace = "http://www.hims-tech.com//entities")]
	public class QA2ScriptBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			Street = 3,
			City = 4,
			State = 5,
			ZipCode = 6,
			AddressIsCorrect = 7,
			AddressUpdateNote = 8,
			PrimaryPhone = 9,
			PrimaryPhoneIsCorrect = 10,
			PrimaryPhoneUpdateNote = 11,
			Passcode = 12,
			PasscodeIsCorrect = 13,
			PassCodeUpdateNote = 14,
			ContractTerm = 15,
			MonitoringFee = 16,
			TermAndFeeIsCorrect = 17,
			TermAndFeeUpdateNote = 18,
			IsReceiveCallOrText = 19,
			IsInstallThatPromised = 20,
			IsShowSystem = 21,
			IsCleanUpAfterInstallation = 22,
			ExperienceRate = 23,
			IsCompletelySatisfied = 24,
			DiscussionIsOkay = 25,
			CreatedBy = 26,
			CreatedByUid = 27,
			CreatedDate = 28,
			LastUpdatedByUid = 29,
			LastUpdatedDate = 30,
			IsCompleted = 31,
			ManualNote = 32,
			FinanceCompletelySatisfied = 33,
			GallantFew = 34,
			TextLinkForTremendously = 35,
			AccountOnline = 36,
			AgreementSigned = 37,
			FirstMonthSetup = 38,
			CompletedBy = 39
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_AddressIsCorrect = "AddressIsCorrect";		            
		public const string Property_AddressUpdateNote = "AddressUpdateNote";		            
		public const string Property_PrimaryPhone = "PrimaryPhone";		            
		public const string Property_PrimaryPhoneIsCorrect = "PrimaryPhoneIsCorrect";		            
		public const string Property_PrimaryPhoneUpdateNote = "PrimaryPhoneUpdateNote";		            
		public const string Property_Passcode = "Passcode";		            
		public const string Property_PasscodeIsCorrect = "PasscodeIsCorrect";		            
		public const string Property_PassCodeUpdateNote = "PassCodeUpdateNote";		            
		public const string Property_ContractTerm = "ContractTerm";		            
		public const string Property_MonitoringFee = "MonitoringFee";		            
		public const string Property_TermAndFeeIsCorrect = "TermAndFeeIsCorrect";		            
		public const string Property_TermAndFeeUpdateNote = "TermAndFeeUpdateNote";		            
		public const string Property_IsReceiveCallOrText = "IsReceiveCallOrText";		            
		public const string Property_IsInstallThatPromised = "IsInstallThatPromised";		            
		public const string Property_IsShowSystem = "IsShowSystem";		            
		public const string Property_IsCleanUpAfterInstallation = "IsCleanUpAfterInstallation";		            
		public const string Property_ExperienceRate = "ExperienceRate";		            
		public const string Property_IsCompletelySatisfied = "IsCompletelySatisfied";		            
		public const string Property_DiscussionIsOkay = "DiscussionIsOkay";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_IsCompleted = "IsCompleted";		            
		public const string Property_ManualNote = "ManualNote";		            
		public const string Property_FinanceCompletelySatisfied = "FinanceCompletelySatisfied";		            
		public const string Property_GallantFew = "GallantFew";		            
		public const string Property_TextLinkForTremendously = "TextLinkForTremendously";		            
		public const string Property_AccountOnline = "AccountOnline";		            
		public const string Property_AgreementSigned = "AgreementSigned";		            
		public const string Property_FirstMonthSetup = "FirstMonthSetup";		            
		public const string Property_CompletedBy = "CompletedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _AddressIsCorrect;	            
		private String _AddressUpdateNote;	            
		private String _PrimaryPhone;	            
		private String _PrimaryPhoneIsCorrect;	            
		private String _PrimaryPhoneUpdateNote;	            
		private String _Passcode;	            
		private String _PasscodeIsCorrect;	            
		private String _PassCodeUpdateNote;	            
		private String _ContractTerm;	            
		private String _MonitoringFee;	            
		private String _TermAndFeeIsCorrect;	            
		private String _TermAndFeeUpdateNote;	            
		private String _IsReceiveCallOrText;	            
		private String _IsInstallThatPromised;	            
		private String _IsShowSystem;	            
		private String _IsCleanUpAfterInstallation;	            
		private String _ExperienceRate;	            
		private String _IsCompletelySatisfied;	            
		private String _DiscussionIsOkay;	            
		private String _CreatedBy;	            
		private Guid _CreatedByUid;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Boolean _IsCompleted;	            
		private String _ManualNote;	            
		private String _FinanceCompletelySatisfied;	            
		private String _GallantFew;	            
		private String _TextLinkForTremendously;	            
		private String _AccountOnline;	            
		private String _AgreementSigned;	            
		private String _FirstMonthSetup;	            
		private Guid _CompletedBy;	            
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
		public String Street
		{	
			get{ return _Street; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Street, value, _Street);
				if (PropertyChanging(args))
				{
					_Street = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String City
		{	
			get{ return _City; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_City, value, _City);
				if (PropertyChanging(args))
				{
					_City = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String State
		{	
			get{ return _State; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_State, value, _State);
				if (PropertyChanging(args))
				{
					_State = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ZipCode
		{	
			get{ return _ZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZipCode, value, _ZipCode);
				if (PropertyChanging(args))
				{
					_ZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressIsCorrect
		{	
			get{ return _AddressIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressIsCorrect, value, _AddressIsCorrect);
				if (PropertyChanging(args))
				{
					_AddressIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressUpdateNote
		{	
			get{ return _AddressUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressUpdateNote, value, _AddressUpdateNote);
				if (PropertyChanging(args))
				{
					_AddressUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrimaryPhone
		{	
			get{ return _PrimaryPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryPhone, value, _PrimaryPhone);
				if (PropertyChanging(args))
				{
					_PrimaryPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrimaryPhoneIsCorrect
		{	
			get{ return _PrimaryPhoneIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryPhoneIsCorrect, value, _PrimaryPhoneIsCorrect);
				if (PropertyChanging(args))
				{
					_PrimaryPhoneIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrimaryPhoneUpdateNote
		{	
			get{ return _PrimaryPhoneUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryPhoneUpdateNote, value, _PrimaryPhoneUpdateNote);
				if (PropertyChanging(args))
				{
					_PrimaryPhoneUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Passcode
		{	
			get{ return _Passcode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Passcode, value, _Passcode);
				if (PropertyChanging(args))
				{
					_Passcode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PasscodeIsCorrect
		{	
			get{ return _PasscodeIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PasscodeIsCorrect, value, _PasscodeIsCorrect);
				if (PropertyChanging(args))
				{
					_PasscodeIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PassCodeUpdateNote
		{	
			get{ return _PassCodeUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PassCodeUpdateNote, value, _PassCodeUpdateNote);
				if (PropertyChanging(args))
				{
					_PassCodeUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractTerm
		{	
			get{ return _ContractTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractTerm, value, _ContractTerm);
				if (PropertyChanging(args))
				{
					_ContractTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonitoringFee
		{	
			get{ return _MonitoringFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitoringFee, value, _MonitoringFee);
				if (PropertyChanging(args))
				{
					_MonitoringFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TermAndFeeIsCorrect
		{	
			get{ return _TermAndFeeIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TermAndFeeIsCorrect, value, _TermAndFeeIsCorrect);
				if (PropertyChanging(args))
				{
					_TermAndFeeIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TermAndFeeUpdateNote
		{	
			get{ return _TermAndFeeUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TermAndFeeUpdateNote, value, _TermAndFeeUpdateNote);
				if (PropertyChanging(args))
				{
					_TermAndFeeUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsReceiveCallOrText
		{	
			get{ return _IsReceiveCallOrText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReceiveCallOrText, value, _IsReceiveCallOrText);
				if (PropertyChanging(args))
				{
					_IsReceiveCallOrText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsInstallThatPromised
		{	
			get{ return _IsInstallThatPromised; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInstallThatPromised, value, _IsInstallThatPromised);
				if (PropertyChanging(args))
				{
					_IsInstallThatPromised = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsShowSystem
		{	
			get{ return _IsShowSystem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsShowSystem, value, _IsShowSystem);
				if (PropertyChanging(args))
				{
					_IsShowSystem = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCleanUpAfterInstallation
		{	
			get{ return _IsCleanUpAfterInstallation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCleanUpAfterInstallation, value, _IsCleanUpAfterInstallation);
				if (PropertyChanging(args))
				{
					_IsCleanUpAfterInstallation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExperienceRate
		{	
			get{ return _ExperienceRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExperienceRate, value, _ExperienceRate);
				if (PropertyChanging(args))
				{
					_ExperienceRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCompletelySatisfied
		{	
			get{ return _IsCompletelySatisfied; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCompletelySatisfied, value, _IsCompletelySatisfied);
				if (PropertyChanging(args))
				{
					_IsCompletelySatisfied = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DiscussionIsOkay
		{	
			get{ return _DiscussionIsOkay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscussionIsOkay, value, _DiscussionIsOkay);
				if (PropertyChanging(args))
				{
					_DiscussionIsOkay = value;
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
		public String FinanceCompletelySatisfied
		{	
			get{ return _FinanceCompletelySatisfied; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceCompletelySatisfied, value, _FinanceCompletelySatisfied);
				if (PropertyChanging(args))
				{
					_FinanceCompletelySatisfied = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GallantFew
		{	
			get{ return _GallantFew; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GallantFew, value, _GallantFew);
				if (PropertyChanging(args))
				{
					_GallantFew = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TextLinkForTremendously
		{	
			get{ return _TextLinkForTremendously; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TextLinkForTremendously, value, _TextLinkForTremendously);
				if (PropertyChanging(args))
				{
					_TextLinkForTremendously = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AccountOnline
		{	
			get{ return _AccountOnline; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountOnline, value, _AccountOnline);
				if (PropertyChanging(args))
				{
					_AccountOnline = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AgreementSigned
		{	
			get{ return _AgreementSigned; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgreementSigned, value, _AgreementSigned);
				if (PropertyChanging(args))
				{
					_AgreementSigned = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FirstMonthSetup
		{	
			get{ return _FirstMonthSetup; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstMonthSetup, value, _FirstMonthSetup);
				if (PropertyChanging(args))
				{
					_FirstMonthSetup = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CompletedBy
		{	
			get{ return _CompletedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompletedBy, value, _CompletedBy);
				if (PropertyChanging(args))
				{
					_CompletedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  QA2ScriptBase Clone()
		{
			QA2ScriptBase newObj = new  QA2ScriptBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.AddressIsCorrect = this.AddressIsCorrect;						
			newObj.AddressUpdateNote = this.AddressUpdateNote;						
			newObj.PrimaryPhone = this.PrimaryPhone;						
			newObj.PrimaryPhoneIsCorrect = this.PrimaryPhoneIsCorrect;						
			newObj.PrimaryPhoneUpdateNote = this.PrimaryPhoneUpdateNote;						
			newObj.Passcode = this.Passcode;						
			newObj.PasscodeIsCorrect = this.PasscodeIsCorrect;						
			newObj.PassCodeUpdateNote = this.PassCodeUpdateNote;						
			newObj.ContractTerm = this.ContractTerm;						
			newObj.MonitoringFee = this.MonitoringFee;						
			newObj.TermAndFeeIsCorrect = this.TermAndFeeIsCorrect;						
			newObj.TermAndFeeUpdateNote = this.TermAndFeeUpdateNote;						
			newObj.IsReceiveCallOrText = this.IsReceiveCallOrText;						
			newObj.IsInstallThatPromised = this.IsInstallThatPromised;						
			newObj.IsShowSystem = this.IsShowSystem;						
			newObj.IsCleanUpAfterInstallation = this.IsCleanUpAfterInstallation;						
			newObj.ExperienceRate = this.ExperienceRate;						
			newObj.IsCompletelySatisfied = this.IsCompletelySatisfied;						
			newObj.DiscussionIsOkay = this.DiscussionIsOkay;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.IsCompleted = this.IsCompleted;						
			newObj.ManualNote = this.ManualNote;						
			newObj.FinanceCompletelySatisfied = this.FinanceCompletelySatisfied;						
			newObj.GallantFew = this.GallantFew;						
			newObj.TextLinkForTremendously = this.TextLinkForTremendously;						
			newObj.AccountOnline = this.AccountOnline;						
			newObj.AgreementSigned = this.AgreementSigned;						
			newObj.FirstMonthSetup = this.FirstMonthSetup;						
			newObj.CompletedBy = this.CompletedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(QA2ScriptBase.Property_Id, Id);				
			info.AddValue(QA2ScriptBase.Property_CustomerId, CustomerId);				
			info.AddValue(QA2ScriptBase.Property_CompanyId, CompanyId);				
			info.AddValue(QA2ScriptBase.Property_Street, Street);				
			info.AddValue(QA2ScriptBase.Property_City, City);				
			info.AddValue(QA2ScriptBase.Property_State, State);				
			info.AddValue(QA2ScriptBase.Property_ZipCode, ZipCode);				
			info.AddValue(QA2ScriptBase.Property_AddressIsCorrect, AddressIsCorrect);				
			info.AddValue(QA2ScriptBase.Property_AddressUpdateNote, AddressUpdateNote);				
			info.AddValue(QA2ScriptBase.Property_PrimaryPhone, PrimaryPhone);				
			info.AddValue(QA2ScriptBase.Property_PrimaryPhoneIsCorrect, PrimaryPhoneIsCorrect);				
			info.AddValue(QA2ScriptBase.Property_PrimaryPhoneUpdateNote, PrimaryPhoneUpdateNote);				
			info.AddValue(QA2ScriptBase.Property_Passcode, Passcode);				
			info.AddValue(QA2ScriptBase.Property_PasscodeIsCorrect, PasscodeIsCorrect);				
			info.AddValue(QA2ScriptBase.Property_PassCodeUpdateNote, PassCodeUpdateNote);				
			info.AddValue(QA2ScriptBase.Property_ContractTerm, ContractTerm);				
			info.AddValue(QA2ScriptBase.Property_MonitoringFee, MonitoringFee);				
			info.AddValue(QA2ScriptBase.Property_TermAndFeeIsCorrect, TermAndFeeIsCorrect);				
			info.AddValue(QA2ScriptBase.Property_TermAndFeeUpdateNote, TermAndFeeUpdateNote);				
			info.AddValue(QA2ScriptBase.Property_IsReceiveCallOrText, IsReceiveCallOrText);				
			info.AddValue(QA2ScriptBase.Property_IsInstallThatPromised, IsInstallThatPromised);				
			info.AddValue(QA2ScriptBase.Property_IsShowSystem, IsShowSystem);				
			info.AddValue(QA2ScriptBase.Property_IsCleanUpAfterInstallation, IsCleanUpAfterInstallation);				
			info.AddValue(QA2ScriptBase.Property_ExperienceRate, ExperienceRate);				
			info.AddValue(QA2ScriptBase.Property_IsCompletelySatisfied, IsCompletelySatisfied);				
			info.AddValue(QA2ScriptBase.Property_DiscussionIsOkay, DiscussionIsOkay);				
			info.AddValue(QA2ScriptBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(QA2ScriptBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(QA2ScriptBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(QA2ScriptBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(QA2ScriptBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(QA2ScriptBase.Property_IsCompleted, IsCompleted);				
			info.AddValue(QA2ScriptBase.Property_ManualNote, ManualNote);				
			info.AddValue(QA2ScriptBase.Property_FinanceCompletelySatisfied, FinanceCompletelySatisfied);				
			info.AddValue(QA2ScriptBase.Property_GallantFew, GallantFew);				
			info.AddValue(QA2ScriptBase.Property_TextLinkForTremendously, TextLinkForTremendously);				
			info.AddValue(QA2ScriptBase.Property_AccountOnline, AccountOnline);				
			info.AddValue(QA2ScriptBase.Property_AgreementSigned, AgreementSigned);				
			info.AddValue(QA2ScriptBase.Property_FirstMonthSetup, FirstMonthSetup);				
			info.AddValue(QA2ScriptBase.Property_CompletedBy, CompletedBy);				
		}
		#endregion

		
	}
}
