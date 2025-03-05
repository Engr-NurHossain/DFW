using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "Recruitmenti9FormBase", Namespace = "http://www.piistech.com//entities")]
	public class Recruitmenti9FormBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FormId = 1,
			FirstName = 2,
			LastName = 3,
			MiddleInitial = 4,
			MaidenName = 5,
			DOB = 6,
			SSN = 7,
			Address = 8,
			Apartment = 9,
			City = 10,
			State = 11,
			ZipCode = 12,
			USCitizen = 13,
			NoncitizenNational = 14,
			LawfulPermanentResident = 15,
			AlienAuthorizedToWork = 16,
			UntilExp = 17,
			Signature = 18,
			SignatureDate = 19,
			TransSignature = 20,
			TransPrintName = 21,
			TransAddress = 22,
			TransSignaturedate = 23,
			DocTitleListA = 24,
			DoctTitleListB = 25,
			DoctTitleListC = 26,
			IssuingAuthorityListA = 27,
			IssuingAuthorityListB = 28,
			IssuingAuthorityListC = 29,
			Doc1ListA = 30,
			Doc1ListB = 31,
			Doc1ListC = 32,
			Exp1ListA = 33,
			Exp1ListB = 34,
			Exp1ListC = 35,
			Doc2ListA = 36,
			Doc2ListB = 37,
			Doc2ListC = 38,
			Exp2ListA = 39,
			Exp2ListB = 40,
			Exp2ListC = 41,
			BeganEmploymentOn = 42,
			AuthRepresentativeSignature = 43,
			AuthRepresentativeName = 44,
			AuthRepresentativeTitle = 45,
			AuthRepSignatureDate = 46,
			OrgNameAndAddress = 47,
			NewName = 48,
			DateOfRehire = 49,
			PrevDocTitle = 50,
			PrevDocNo = 51,
			PrevDocExp = 52,
			AuthRepSignature2 = 53,
			AuthRepSignatureDate2 = 54
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FormId = "FormId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_MiddleInitial = "MiddleInitial";		            
		public const string Property_MaidenName = "MaidenName";		            
		public const string Property_DOB = "DOB";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_Address = "Address";		            
		public const string Property_Apartment = "Apartment";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_USCitizen = "USCitizen";		            
		public const string Property_NoncitizenNational = "NoncitizenNational";		            
		public const string Property_LawfulPermanentResident = "LawfulPermanentResident";		            
		public const string Property_AlienAuthorizedToWork = "AlienAuthorizedToWork";		            
		public const string Property_UntilExp = "UntilExp";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_SignatureDate = "SignatureDate";		            
		public const string Property_TransSignature = "TransSignature";		            
		public const string Property_TransPrintName = "TransPrintName";		            
		public const string Property_TransAddress = "TransAddress";		            
		public const string Property_TransSignaturedate = "TransSignaturedate";		            
		public const string Property_DocTitleListA = "DocTitleListA";		            
		public const string Property_DoctTitleListB = "DoctTitleListB";		            
		public const string Property_DoctTitleListC = "DoctTitleListC";		            
		public const string Property_IssuingAuthorityListA = "IssuingAuthorityListA";		            
		public const string Property_IssuingAuthorityListB = "IssuingAuthorityListB";		            
		public const string Property_IssuingAuthorityListC = "IssuingAuthorityListC";		            
		public const string Property_Doc1ListA = "Doc1ListA";		            
		public const string Property_Doc1ListB = "Doc1ListB";		            
		public const string Property_Doc1ListC = "Doc1ListC";		            
		public const string Property_Exp1ListA = "Exp1ListA";		            
		public const string Property_Exp1ListB = "Exp1ListB";		            
		public const string Property_Exp1ListC = "Exp1ListC";		            
		public const string Property_Doc2ListA = "Doc2ListA";		            
		public const string Property_Doc2ListB = "Doc2ListB";		            
		public const string Property_Doc2ListC = "Doc2ListC";		            
		public const string Property_Exp2ListA = "Exp2ListA";		            
		public const string Property_Exp2ListB = "Exp2ListB";		            
		public const string Property_Exp2ListC = "Exp2ListC";		            
		public const string Property_BeganEmploymentOn = "BeganEmploymentOn";		            
		public const string Property_AuthRepresentativeSignature = "AuthRepresentativeSignature";		            
		public const string Property_AuthRepresentativeName = "AuthRepresentativeName";		            
		public const string Property_AuthRepresentativeTitle = "AuthRepresentativeTitle";		            
		public const string Property_AuthRepSignatureDate = "AuthRepSignatureDate";		            
		public const string Property_OrgNameAndAddress = "OrgNameAndAddress";		            
		public const string Property_NewName = "NewName";		            
		public const string Property_DateOfRehire = "DateOfRehire";		            
		public const string Property_PrevDocTitle = "PrevDocTitle";		            
		public const string Property_PrevDocNo = "PrevDocNo";		            
		public const string Property_PrevDocExp = "PrevDocExp";		            
		public const string Property_AuthRepSignature2 = "AuthRepSignature2";		            
		public const string Property_AuthRepSignatureDate2 = "AuthRepSignatureDate2";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _FormId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _MiddleInitial;	            
		private String _MaidenName;	            
		private Nullable<DateTime> _DOB;	            
		private String _SSN;	            
		private String _Address;	            
		private String _Apartment;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private Nullable<Boolean> _USCitizen;	            
		private Nullable<Boolean> _NoncitizenNational;	            
		private Nullable<Boolean> _LawfulPermanentResident;	            
		private Nullable<Boolean> _AlienAuthorizedToWork;	            
		private Nullable<Boolean> _UntilExp;	            
		private String _Signature;	            
		private Nullable<DateTime> _SignatureDate;	            
		private String _TransSignature;	            
		private String _TransPrintName;	            
		private String _TransAddress;	            
		private Nullable<DateTime> _TransSignaturedate;	            
		private String _DocTitleListA;	            
		private String _DoctTitleListB;	            
		private String _DoctTitleListC;	            
		private String _IssuingAuthorityListA;	            
		private String _IssuingAuthorityListB;	            
		private String _IssuingAuthorityListC;	            
		private String _Doc1ListA;	            
		private String _Doc1ListB;	            
		private String _Doc1ListC;	            
		private Nullable<DateTime> _Exp1ListA;	            
		private Nullable<DateTime> _Exp1ListB;	            
		private Nullable<DateTime> _Exp1ListC;	            
		private String _Doc2ListA;	            
		private String _Doc2ListB;	            
		private String _Doc2ListC;	            
		private Nullable<DateTime> _Exp2ListA;	            
		private Nullable<DateTime> _Exp2ListB;	            
		private Nullable<DateTime> _Exp2ListC;	            
		private Nullable<DateTime> _BeganEmploymentOn;	            
		private String _AuthRepresentativeSignature;	            
		private String _AuthRepresentativeName;	            
		private String _AuthRepresentativeTitle;	            
		private Nullable<DateTime> _AuthRepSignatureDate;	            
		private String _OrgNameAndAddress;	            
		private String _NewName;	            
		private Nullable<DateTime> _DateOfRehire;	            
		private String _PrevDocTitle;	            
		private String _PrevDocNo;	            
		private Nullable<DateTime> _PrevDocExp;	            
		private String _AuthRepSignature2;	            
		private Nullable<DateTime> _AuthRepSignatureDate2;	            
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
		public String FirstName
		{	
			get{ return _FirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstName, value, _FirstName);
				if (PropertyChanging(args))
				{
					_FirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastName
		{	
			get{ return _LastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastName, value, _LastName);
				if (PropertyChanging(args))
				{
					_LastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiddleInitial
		{	
			get{ return _MiddleInitial; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiddleInitial, value, _MiddleInitial);
				if (PropertyChanging(args))
				{
					_MiddleInitial = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MaidenName
		{	
			get{ return _MaidenName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaidenName, value, _MaidenName);
				if (PropertyChanging(args))
				{
					_MaidenName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DOB
		{	
			get{ return _DOB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DOB, value, _DOB);
				if (PropertyChanging(args))
				{
					_DOB = value;
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
		public String Apartment
		{	
			get{ return _Apartment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Apartment, value, _Apartment);
				if (PropertyChanging(args))
				{
					_Apartment = value;
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
		public Nullable<Boolean> USCitizen
		{	
			get{ return _USCitizen; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_USCitizen, value, _USCitizen);
				if (PropertyChanging(args))
				{
					_USCitizen = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> NoncitizenNational
		{	
			get{ return _NoncitizenNational; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoncitizenNational, value, _NoncitizenNational);
				if (PropertyChanging(args))
				{
					_NoncitizenNational = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> LawfulPermanentResident
		{	
			get{ return _LawfulPermanentResident; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LawfulPermanentResident, value, _LawfulPermanentResident);
				if (PropertyChanging(args))
				{
					_LawfulPermanentResident = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> AlienAuthorizedToWork
		{	
			get{ return _AlienAuthorizedToWork; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlienAuthorizedToWork, value, _AlienAuthorizedToWork);
				if (PropertyChanging(args))
				{
					_AlienAuthorizedToWork = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> UntilExp
		{	
			get{ return _UntilExp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UntilExp, value, _UntilExp);
				if (PropertyChanging(args))
				{
					_UntilExp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Signature
		{	
			get{ return _Signature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Signature, value, _Signature);
				if (PropertyChanging(args))
				{
					_Signature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SignatureDate
		{	
			get{ return _SignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SignatureDate, value, _SignatureDate);
				if (PropertyChanging(args))
				{
					_SignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransSignature
		{	
			get{ return _TransSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransSignature, value, _TransSignature);
				if (PropertyChanging(args))
				{
					_TransSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransPrintName
		{	
			get{ return _TransPrintName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransPrintName, value, _TransPrintName);
				if (PropertyChanging(args))
				{
					_TransPrintName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransAddress
		{	
			get{ return _TransAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransAddress, value, _TransAddress);
				if (PropertyChanging(args))
				{
					_TransAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> TransSignaturedate
		{	
			get{ return _TransSignaturedate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransSignaturedate, value, _TransSignaturedate);
				if (PropertyChanging(args))
				{
					_TransSignaturedate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DocTitleListA
		{	
			get{ return _DocTitleListA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DocTitleListA, value, _DocTitleListA);
				if (PropertyChanging(args))
				{
					_DocTitleListA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DoctTitleListB
		{	
			get{ return _DoctTitleListB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DoctTitleListB, value, _DoctTitleListB);
				if (PropertyChanging(args))
				{
					_DoctTitleListB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DoctTitleListC
		{	
			get{ return _DoctTitleListC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DoctTitleListC, value, _DoctTitleListC);
				if (PropertyChanging(args))
				{
					_DoctTitleListC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IssuingAuthorityListA
		{	
			get{ return _IssuingAuthorityListA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IssuingAuthorityListA, value, _IssuingAuthorityListA);
				if (PropertyChanging(args))
				{
					_IssuingAuthorityListA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IssuingAuthorityListB
		{	
			get{ return _IssuingAuthorityListB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IssuingAuthorityListB, value, _IssuingAuthorityListB);
				if (PropertyChanging(args))
				{
					_IssuingAuthorityListB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IssuingAuthorityListC
		{	
			get{ return _IssuingAuthorityListC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IssuingAuthorityListC, value, _IssuingAuthorityListC);
				if (PropertyChanging(args))
				{
					_IssuingAuthorityListC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Doc1ListA
		{	
			get{ return _Doc1ListA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Doc1ListA, value, _Doc1ListA);
				if (PropertyChanging(args))
				{
					_Doc1ListA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Doc1ListB
		{	
			get{ return _Doc1ListB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Doc1ListB, value, _Doc1ListB);
				if (PropertyChanging(args))
				{
					_Doc1ListB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Doc1ListC
		{	
			get{ return _Doc1ListC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Doc1ListC, value, _Doc1ListC);
				if (PropertyChanging(args))
				{
					_Doc1ListC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Exp1ListA
		{	
			get{ return _Exp1ListA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Exp1ListA, value, _Exp1ListA);
				if (PropertyChanging(args))
				{
					_Exp1ListA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Exp1ListB
		{	
			get{ return _Exp1ListB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Exp1ListB, value, _Exp1ListB);
				if (PropertyChanging(args))
				{
					_Exp1ListB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Exp1ListC
		{	
			get{ return _Exp1ListC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Exp1ListC, value, _Exp1ListC);
				if (PropertyChanging(args))
				{
					_Exp1ListC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Doc2ListA
		{	
			get{ return _Doc2ListA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Doc2ListA, value, _Doc2ListA);
				if (PropertyChanging(args))
				{
					_Doc2ListA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Doc2ListB
		{	
			get{ return _Doc2ListB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Doc2ListB, value, _Doc2ListB);
				if (PropertyChanging(args))
				{
					_Doc2ListB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Doc2ListC
		{	
			get{ return _Doc2ListC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Doc2ListC, value, _Doc2ListC);
				if (PropertyChanging(args))
				{
					_Doc2ListC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Exp2ListA
		{	
			get{ return _Exp2ListA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Exp2ListA, value, _Exp2ListA);
				if (PropertyChanging(args))
				{
					_Exp2ListA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Exp2ListB
		{	
			get{ return _Exp2ListB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Exp2ListB, value, _Exp2ListB);
				if (PropertyChanging(args))
				{
					_Exp2ListB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Exp2ListC
		{	
			get{ return _Exp2ListC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Exp2ListC, value, _Exp2ListC);
				if (PropertyChanging(args))
				{
					_Exp2ListC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> BeganEmploymentOn
		{	
			get{ return _BeganEmploymentOn; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BeganEmploymentOn, value, _BeganEmploymentOn);
				if (PropertyChanging(args))
				{
					_BeganEmploymentOn = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthRepresentativeSignature
		{	
			get{ return _AuthRepresentativeSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRepresentativeSignature, value, _AuthRepresentativeSignature);
				if (PropertyChanging(args))
				{
					_AuthRepresentativeSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthRepresentativeName
		{	
			get{ return _AuthRepresentativeName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRepresentativeName, value, _AuthRepresentativeName);
				if (PropertyChanging(args))
				{
					_AuthRepresentativeName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthRepresentativeTitle
		{	
			get{ return _AuthRepresentativeTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRepresentativeTitle, value, _AuthRepresentativeTitle);
				if (PropertyChanging(args))
				{
					_AuthRepresentativeTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AuthRepSignatureDate
		{	
			get{ return _AuthRepSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRepSignatureDate, value, _AuthRepSignatureDate);
				if (PropertyChanging(args))
				{
					_AuthRepSignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OrgNameAndAddress
		{	
			get{ return _OrgNameAndAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrgNameAndAddress, value, _OrgNameAndAddress);
				if (PropertyChanging(args))
				{
					_OrgNameAndAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NewName
		{	
			get{ return _NewName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NewName, value, _NewName);
				if (PropertyChanging(args))
				{
					_NewName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DateOfRehire
		{	
			get{ return _DateOfRehire; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateOfRehire, value, _DateOfRehire);
				if (PropertyChanging(args))
				{
					_DateOfRehire = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrevDocTitle
		{	
			get{ return _PrevDocTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrevDocTitle, value, _PrevDocTitle);
				if (PropertyChanging(args))
				{
					_PrevDocTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrevDocNo
		{	
			get{ return _PrevDocNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrevDocNo, value, _PrevDocNo);
				if (PropertyChanging(args))
				{
					_PrevDocNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PrevDocExp
		{	
			get{ return _PrevDocExp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrevDocExp, value, _PrevDocExp);
				if (PropertyChanging(args))
				{
					_PrevDocExp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthRepSignature2
		{	
			get{ return _AuthRepSignature2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRepSignature2, value, _AuthRepSignature2);
				if (PropertyChanging(args))
				{
					_AuthRepSignature2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AuthRepSignatureDate2
		{	
			get{ return _AuthRepSignatureDate2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRepSignatureDate2, value, _AuthRepSignatureDate2);
				if (PropertyChanging(args))
				{
					_AuthRepSignatureDate2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  Recruitmenti9FormBase Clone()
		{
			Recruitmenti9FormBase newObj = new  Recruitmenti9FormBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FormId = this.FormId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.MiddleInitial = this.MiddleInitial;						
			newObj.MaidenName = this.MaidenName;						
			newObj.DOB = this.DOB;						
			newObj.SSN = this.SSN;						
			newObj.Address = this.Address;						
			newObj.Apartment = this.Apartment;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.USCitizen = this.USCitizen;						
			newObj.NoncitizenNational = this.NoncitizenNational;						
			newObj.LawfulPermanentResident = this.LawfulPermanentResident;						
			newObj.AlienAuthorizedToWork = this.AlienAuthorizedToWork;						
			newObj.UntilExp = this.UntilExp;						
			newObj.Signature = this.Signature;						
			newObj.SignatureDate = this.SignatureDate;						
			newObj.TransSignature = this.TransSignature;						
			newObj.TransPrintName = this.TransPrintName;						
			newObj.TransAddress = this.TransAddress;						
			newObj.TransSignaturedate = this.TransSignaturedate;						
			newObj.DocTitleListA = this.DocTitleListA;						
			newObj.DoctTitleListB = this.DoctTitleListB;						
			newObj.DoctTitleListC = this.DoctTitleListC;						
			newObj.IssuingAuthorityListA = this.IssuingAuthorityListA;						
			newObj.IssuingAuthorityListB = this.IssuingAuthorityListB;						
			newObj.IssuingAuthorityListC = this.IssuingAuthorityListC;						
			newObj.Doc1ListA = this.Doc1ListA;						
			newObj.Doc1ListB = this.Doc1ListB;						
			newObj.Doc1ListC = this.Doc1ListC;						
			newObj.Exp1ListA = this.Exp1ListA;						
			newObj.Exp1ListB = this.Exp1ListB;						
			newObj.Exp1ListC = this.Exp1ListC;						
			newObj.Doc2ListA = this.Doc2ListA;						
			newObj.Doc2ListB = this.Doc2ListB;						
			newObj.Doc2ListC = this.Doc2ListC;						
			newObj.Exp2ListA = this.Exp2ListA;						
			newObj.Exp2ListB = this.Exp2ListB;						
			newObj.Exp2ListC = this.Exp2ListC;						
			newObj.BeganEmploymentOn = this.BeganEmploymentOn;						
			newObj.AuthRepresentativeSignature = this.AuthRepresentativeSignature;						
			newObj.AuthRepresentativeName = this.AuthRepresentativeName;						
			newObj.AuthRepresentativeTitle = this.AuthRepresentativeTitle;						
			newObj.AuthRepSignatureDate = this.AuthRepSignatureDate;						
			newObj.OrgNameAndAddress = this.OrgNameAndAddress;						
			newObj.NewName = this.NewName;						
			newObj.DateOfRehire = this.DateOfRehire;						
			newObj.PrevDocTitle = this.PrevDocTitle;						
			newObj.PrevDocNo = this.PrevDocNo;						
			newObj.PrevDocExp = this.PrevDocExp;						
			newObj.AuthRepSignature2 = this.AuthRepSignature2;						
			newObj.AuthRepSignatureDate2 = this.AuthRepSignatureDate2;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(Recruitmenti9FormBase.Property_Id, Id);				
			info.AddValue(Recruitmenti9FormBase.Property_FormId, FormId);				
			info.AddValue(Recruitmenti9FormBase.Property_FirstName, FirstName);				
			info.AddValue(Recruitmenti9FormBase.Property_LastName, LastName);				
			info.AddValue(Recruitmenti9FormBase.Property_MiddleInitial, MiddleInitial);				
			info.AddValue(Recruitmenti9FormBase.Property_MaidenName, MaidenName);				
			info.AddValue(Recruitmenti9FormBase.Property_DOB, DOB);				
			info.AddValue(Recruitmenti9FormBase.Property_SSN, SSN);				
			info.AddValue(Recruitmenti9FormBase.Property_Address, Address);				
			info.AddValue(Recruitmenti9FormBase.Property_Apartment, Apartment);				
			info.AddValue(Recruitmenti9FormBase.Property_City, City);				
			info.AddValue(Recruitmenti9FormBase.Property_State, State);				
			info.AddValue(Recruitmenti9FormBase.Property_ZipCode, ZipCode);				
			info.AddValue(Recruitmenti9FormBase.Property_USCitizen, USCitizen);				
			info.AddValue(Recruitmenti9FormBase.Property_NoncitizenNational, NoncitizenNational);				
			info.AddValue(Recruitmenti9FormBase.Property_LawfulPermanentResident, LawfulPermanentResident);				
			info.AddValue(Recruitmenti9FormBase.Property_AlienAuthorizedToWork, AlienAuthorizedToWork);				
			info.AddValue(Recruitmenti9FormBase.Property_UntilExp, UntilExp);				
			info.AddValue(Recruitmenti9FormBase.Property_Signature, Signature);				
			info.AddValue(Recruitmenti9FormBase.Property_SignatureDate, SignatureDate);				
			info.AddValue(Recruitmenti9FormBase.Property_TransSignature, TransSignature);				
			info.AddValue(Recruitmenti9FormBase.Property_TransPrintName, TransPrintName);				
			info.AddValue(Recruitmenti9FormBase.Property_TransAddress, TransAddress);				
			info.AddValue(Recruitmenti9FormBase.Property_TransSignaturedate, TransSignaturedate);				
			info.AddValue(Recruitmenti9FormBase.Property_DocTitleListA, DocTitleListA);				
			info.AddValue(Recruitmenti9FormBase.Property_DoctTitleListB, DoctTitleListB);				
			info.AddValue(Recruitmenti9FormBase.Property_DoctTitleListC, DoctTitleListC);				
			info.AddValue(Recruitmenti9FormBase.Property_IssuingAuthorityListA, IssuingAuthorityListA);				
			info.AddValue(Recruitmenti9FormBase.Property_IssuingAuthorityListB, IssuingAuthorityListB);				
			info.AddValue(Recruitmenti9FormBase.Property_IssuingAuthorityListC, IssuingAuthorityListC);				
			info.AddValue(Recruitmenti9FormBase.Property_Doc1ListA, Doc1ListA);				
			info.AddValue(Recruitmenti9FormBase.Property_Doc1ListB, Doc1ListB);				
			info.AddValue(Recruitmenti9FormBase.Property_Doc1ListC, Doc1ListC);				
			info.AddValue(Recruitmenti9FormBase.Property_Exp1ListA, Exp1ListA);				
			info.AddValue(Recruitmenti9FormBase.Property_Exp1ListB, Exp1ListB);				
			info.AddValue(Recruitmenti9FormBase.Property_Exp1ListC, Exp1ListC);				
			info.AddValue(Recruitmenti9FormBase.Property_Doc2ListA, Doc2ListA);				
			info.AddValue(Recruitmenti9FormBase.Property_Doc2ListB, Doc2ListB);				
			info.AddValue(Recruitmenti9FormBase.Property_Doc2ListC, Doc2ListC);				
			info.AddValue(Recruitmenti9FormBase.Property_Exp2ListA, Exp2ListA);				
			info.AddValue(Recruitmenti9FormBase.Property_Exp2ListB, Exp2ListB);				
			info.AddValue(Recruitmenti9FormBase.Property_Exp2ListC, Exp2ListC);				
			info.AddValue(Recruitmenti9FormBase.Property_BeganEmploymentOn, BeganEmploymentOn);				
			info.AddValue(Recruitmenti9FormBase.Property_AuthRepresentativeSignature, AuthRepresentativeSignature);				
			info.AddValue(Recruitmenti9FormBase.Property_AuthRepresentativeName, AuthRepresentativeName);				
			info.AddValue(Recruitmenti9FormBase.Property_AuthRepresentativeTitle, AuthRepresentativeTitle);				
			info.AddValue(Recruitmenti9FormBase.Property_AuthRepSignatureDate, AuthRepSignatureDate);				
			info.AddValue(Recruitmenti9FormBase.Property_OrgNameAndAddress, OrgNameAndAddress);				
			info.AddValue(Recruitmenti9FormBase.Property_NewName, NewName);				
			info.AddValue(Recruitmenti9FormBase.Property_DateOfRehire, DateOfRehire);				
			info.AddValue(Recruitmenti9FormBase.Property_PrevDocTitle, PrevDocTitle);				
			info.AddValue(Recruitmenti9FormBase.Property_PrevDocNo, PrevDocNo);				
			info.AddValue(Recruitmenti9FormBase.Property_PrevDocExp, PrevDocExp);				
			info.AddValue(Recruitmenti9FormBase.Property_AuthRepSignature2, AuthRepSignature2);				
			info.AddValue(Recruitmenti9FormBase.Property_AuthRepSignatureDate2, AuthRepSignatureDate2);				
		}
		#endregion

		
	}
}
