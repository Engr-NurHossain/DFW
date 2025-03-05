using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RecruitmentW9FormBase", Namespace = "http://www.piistech.com//entities")]
	public class RecruitmentW9FormBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FormId = 1,
			Name = 2,
			BusinessName = 3,
			Individual = 4,
			CCorporation = 5,
			SCorporation = 6,
			Partnership = 7,
			Trust = 8,
			ExemptPayeeCode = 9,
			FATCAReportingCode = 10,
			LimitedLiability = 11,
			TaxClassification = 12,
			Other = 13,
			Address = 14,
			City = 15,
			State = 16,
			Zipcode = 17,
			AccountNumber = 18,
			SSN = 19,
			EIN = 20
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FormId = "FormId";		            
		public const string Property_Name = "Name";		            
		public const string Property_BusinessName = "BusinessName";		            
		public const string Property_Individual = "Individual";		            
		public const string Property_CCorporation = "CCorporation";		            
		public const string Property_SCorporation = "SCorporation";		            
		public const string Property_Partnership = "Partnership";		            
		public const string Property_Trust = "Trust";		            
		public const string Property_ExemptPayeeCode = "ExemptPayeeCode";		            
		public const string Property_FATCAReportingCode = "FATCAReportingCode";		            
		public const string Property_LimitedLiability = "LimitedLiability";		            
		public const string Property_TaxClassification = "TaxClassification";		            
		public const string Property_Other = "Other";		            
		public const string Property_Address = "Address";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zipcode = "Zipcode";		            
		public const string Property_AccountNumber = "AccountNumber";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_EIN = "EIN";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _FormId;	            
		private String _Name;	            
		private String _BusinessName;	            
		private Nullable<Boolean> _Individual;	            
		private Nullable<Boolean> _CCorporation;	            
		private Nullable<Boolean> _SCorporation;	            
		private Nullable<Boolean> _Partnership;	            
		private Nullable<Boolean> _Trust;	            
		private String _ExemptPayeeCode;	            
		private String _FATCAReportingCode;	            
		private Nullable<Boolean> _LimitedLiability;	            
		private String _TaxClassification;	            
		private String _Other;	            
		private String _Address;	            
		private String _City;	            
		private String _State;	            
		private String _Zipcode;	            
		private String _AccountNumber;	            
		private String _SSN;	            
		private String _EIN;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BusinessName
		{	
			get{ return _BusinessName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BusinessName, value, _BusinessName);
				if (PropertyChanging(args))
				{
					_BusinessName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Individual
		{	
			get{ return _Individual; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Individual, value, _Individual);
				if (PropertyChanging(args))
				{
					_Individual = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> CCorporation
		{	
			get{ return _CCorporation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CCorporation, value, _CCorporation);
				if (PropertyChanging(args))
				{
					_CCorporation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> SCorporation
		{	
			get{ return _SCorporation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SCorporation, value, _SCorporation);
				if (PropertyChanging(args))
				{
					_SCorporation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Partnership
		{	
			get{ return _Partnership; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Partnership, value, _Partnership);
				if (PropertyChanging(args))
				{
					_Partnership = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Trust
		{	
			get{ return _Trust; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Trust, value, _Trust);
				if (PropertyChanging(args))
				{
					_Trust = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExemptPayeeCode
		{	
			get{ return _ExemptPayeeCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExemptPayeeCode, value, _ExemptPayeeCode);
				if (PropertyChanging(args))
				{
					_ExemptPayeeCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FATCAReportingCode
		{	
			get{ return _FATCAReportingCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FATCAReportingCode, value, _FATCAReportingCode);
				if (PropertyChanging(args))
				{
					_FATCAReportingCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> LimitedLiability
		{	
			get{ return _LimitedLiability; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LimitedLiability, value, _LimitedLiability);
				if (PropertyChanging(args))
				{
					_LimitedLiability = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TaxClassification
		{	
			get{ return _TaxClassification; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxClassification, value, _TaxClassification);
				if (PropertyChanging(args))
				{
					_TaxClassification = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Other
		{	
			get{ return _Other; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Other, value, _Other);
				if (PropertyChanging(args))
				{
					_Other = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Address
		{	
			get{ return _Address; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address, value, _Address);
				if (PropertyChanging(args))
				{
					_Address = value;
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
		public String Zipcode
		{	
			get{ return _Zipcode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zipcode, value, _Zipcode);
				if (PropertyChanging(args))
				{
					_Zipcode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AccountNumber
		{	
			get{ return _AccountNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountNumber, value, _AccountNumber);
				if (PropertyChanging(args))
				{
					_AccountNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SSN
		{	
			get{ return _SSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SSN, value, _SSN);
				if (PropertyChanging(args))
				{
					_SSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EIN
		{	
			get{ return _EIN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EIN, value, _EIN);
				if (PropertyChanging(args))
				{
					_EIN = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RecruitmentW9FormBase Clone()
		{
			RecruitmentW9FormBase newObj = new  RecruitmentW9FormBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FormId = this.FormId;						
			newObj.Name = this.Name;						
			newObj.BusinessName = this.BusinessName;						
			newObj.Individual = this.Individual;						
			newObj.CCorporation = this.CCorporation;						
			newObj.SCorporation = this.SCorporation;						
			newObj.Partnership = this.Partnership;						
			newObj.Trust = this.Trust;						
			newObj.ExemptPayeeCode = this.ExemptPayeeCode;						
			newObj.FATCAReportingCode = this.FATCAReportingCode;						
			newObj.LimitedLiability = this.LimitedLiability;						
			newObj.TaxClassification = this.TaxClassification;						
			newObj.Other = this.Other;						
			newObj.Address = this.Address;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zipcode = this.Zipcode;						
			newObj.AccountNumber = this.AccountNumber;						
			newObj.SSN = this.SSN;						
			newObj.EIN = this.EIN;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RecruitmentW9FormBase.Property_Id, Id);				
			info.AddValue(RecruitmentW9FormBase.Property_FormId, FormId);				
			info.AddValue(RecruitmentW9FormBase.Property_Name, Name);				
			info.AddValue(RecruitmentW9FormBase.Property_BusinessName, BusinessName);				
			info.AddValue(RecruitmentW9FormBase.Property_Individual, Individual);				
			info.AddValue(RecruitmentW9FormBase.Property_CCorporation, CCorporation);				
			info.AddValue(RecruitmentW9FormBase.Property_SCorporation, SCorporation);				
			info.AddValue(RecruitmentW9FormBase.Property_Partnership, Partnership);				
			info.AddValue(RecruitmentW9FormBase.Property_Trust, Trust);				
			info.AddValue(RecruitmentW9FormBase.Property_ExemptPayeeCode, ExemptPayeeCode);				
			info.AddValue(RecruitmentW9FormBase.Property_FATCAReportingCode, FATCAReportingCode);				
			info.AddValue(RecruitmentW9FormBase.Property_LimitedLiability, LimitedLiability);				
			info.AddValue(RecruitmentW9FormBase.Property_TaxClassification, TaxClassification);				
			info.AddValue(RecruitmentW9FormBase.Property_Other, Other);				
			info.AddValue(RecruitmentW9FormBase.Property_Address, Address);				
			info.AddValue(RecruitmentW9FormBase.Property_City, City);				
			info.AddValue(RecruitmentW9FormBase.Property_State, State);				
			info.AddValue(RecruitmentW9FormBase.Property_Zipcode, Zipcode);				
			info.AddValue(RecruitmentW9FormBase.Property_AccountNumber, AccountNumber);				
			info.AddValue(RecruitmentW9FormBase.Property_SSN, SSN);				
			info.AddValue(RecruitmentW9FormBase.Property_EIN, EIN);				
		}
		#endregion

		
	}
}
