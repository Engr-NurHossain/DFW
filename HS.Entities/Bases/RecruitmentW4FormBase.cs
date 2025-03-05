using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RecruitmentW4FormBase", Namespace = "http://www.piistech.com//entities")]
	public class RecruitmentW4FormBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FormId = 1,
			AllowanceWorksheetA = 2,
			AllowanceWorksheetB = 3,
			AllowanceWorksheetC = 4,
			AllowanceWorksheetD = 5,
			AllowanceWorksheetE = 6,
			AllowanceWorksheetF = 7,
			AllowanceWorksheetG = 8,
			AllowanceWorksheetH = 9,
			FirstName = 10,
			MiddleInitial = 11,
			LastName = 12,
			SSN = 13,
			Address = 14,
			Single = 15,
			Married = 16,
			MarriadButSeparated = 17,
			City = 18,
			State = 19,
			Zipcode = 20,
			ReplaceSSNCard4 = 21,
			TotalAllowance5 = 22,
			AdditionalAmount6 = 23,
			NoTaxLiability7 = 24,
			Signature = 25,
			SingatureDate = 26,
			EmployernameAndAddress = 27,
			OfficeCode = 28,
			EmployerEIN = 29,
			AdjustWorkSheet1 = 30,
			AdjustWorkSheet2 = 31,
			AdjustWorkSheet3 = 32,
			AdjustWorkSheet4 = 33,
			AdjustWorkSheet5 = 34,
			AdjustWorkSheet6 = 35,
			AdjustWorkSheet7 = 36,
			AdjustWorkSheet8 = 37,
			AdjustWorkSheet9 = 38,
			AdjustWorkSheet10 = 39,
			JobWroksheet1 = 40,
			JobWroksheet2 = 41,
			JobWroksheet3 = 42,
			JobWroksheet4 = 43,
			JobWroksheet5 = 44,
			JobWroksheet6 = 45,
			JobWroksheet7 = 46,
			JobWroksheet8 = 47,
			JobWroksheet9 = 48
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FormId = "FormId";		            
		public const string Property_AllowanceWorksheetA = "AllowanceWorksheetA";		            
		public const string Property_AllowanceWorksheetB = "AllowanceWorksheetB";		            
		public const string Property_AllowanceWorksheetC = "AllowanceWorksheetC";		            
		public const string Property_AllowanceWorksheetD = "AllowanceWorksheetD";		            
		public const string Property_AllowanceWorksheetE = "AllowanceWorksheetE";		            
		public const string Property_AllowanceWorksheetF = "AllowanceWorksheetF";		            
		public const string Property_AllowanceWorksheetG = "AllowanceWorksheetG";		            
		public const string Property_AllowanceWorksheetH = "AllowanceWorksheetH";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_MiddleInitial = "MiddleInitial";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_Address = "Address";		            
		public const string Property_Single = "Single";		            
		public const string Property_Married = "Married";		            
		public const string Property_MarriadButSeparated = "MarriadButSeparated";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zipcode = "Zipcode";		            
		public const string Property_ReplaceSSNCard4 = "ReplaceSSNCard4";		            
		public const string Property_TotalAllowance5 = "TotalAllowance5";		            
		public const string Property_AdditionalAmount6 = "AdditionalAmount6";		            
		public const string Property_NoTaxLiability7 = "NoTaxLiability7";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_SingatureDate = "SingatureDate";		            
		public const string Property_EmployernameAndAddress = "EmployernameAndAddress";		            
		public const string Property_OfficeCode = "OfficeCode";		            
		public const string Property_EmployerEIN = "EmployerEIN";		            
		public const string Property_AdjustWorkSheet1 = "AdjustWorkSheet1";		            
		public const string Property_AdjustWorkSheet2 = "AdjustWorkSheet2";		            
		public const string Property_AdjustWorkSheet3 = "AdjustWorkSheet3";		            
		public const string Property_AdjustWorkSheet4 = "AdjustWorkSheet4";		            
		public const string Property_AdjustWorkSheet5 = "AdjustWorkSheet5";		            
		public const string Property_AdjustWorkSheet6 = "AdjustWorkSheet6";		            
		public const string Property_AdjustWorkSheet7 = "AdjustWorkSheet7";		            
		public const string Property_AdjustWorkSheet8 = "AdjustWorkSheet8";		            
		public const string Property_AdjustWorkSheet9 = "AdjustWorkSheet9";		            
		public const string Property_AdjustWorkSheet10 = "AdjustWorkSheet10";		            
		public const string Property_JobWroksheet1 = "JobWroksheet1";		            
		public const string Property_JobWroksheet2 = "JobWroksheet2";		            
		public const string Property_JobWroksheet3 = "JobWroksheet3";		            
		public const string Property_JobWroksheet4 = "JobWroksheet4";		            
		public const string Property_JobWroksheet5 = "JobWroksheet5";		            
		public const string Property_JobWroksheet6 = "JobWroksheet6";		            
		public const string Property_JobWroksheet7 = "JobWroksheet7";		            
		public const string Property_JobWroksheet8 = "JobWroksheet8";		            
		public const string Property_JobWroksheet9 = "JobWroksheet9";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _FormId;	            
		private String _AllowanceWorksheetA;	            
		private String _AllowanceWorksheetB;	            
		private String _AllowanceWorksheetC;	            
		private String _AllowanceWorksheetD;	            
		private String _AllowanceWorksheetE;	            
		private String _AllowanceWorksheetF;	            
		private String _AllowanceWorksheetG;	            
		private String _AllowanceWorksheetH;	            
		private String _FirstName;	            
		private String _MiddleInitial;	            
		private String _LastName;	            
		private String _SSN;	            
		private String _Address;	            
		private Nullable<Boolean> _Single;	            
		private Nullable<Boolean> _Married;	            
		private Nullable<Boolean> _MarriadButSeparated;	            
		private String _City;	            
		private String _State;	            
		private String _Zipcode;	            
		private Nullable<Boolean> _ReplaceSSNCard4;	            
		private Nullable<Int32> _TotalAllowance5;	            
		private Nullable<Double> _AdditionalAmount6;	            
		private String _NoTaxLiability7;	            
		private String _Signature;	            
		private Nullable<DateTime> _SingatureDate;	            
		private String _EmployernameAndAddress;	            
		private String _OfficeCode;	            
		private String _EmployerEIN;	            
		private String _AdjustWorkSheet1;	            
		private String _AdjustWorkSheet2;	            
		private String _AdjustWorkSheet3;	            
		private String _AdjustWorkSheet4;	            
		private String _AdjustWorkSheet5;	            
		private String _AdjustWorkSheet6;	            
		private String _AdjustWorkSheet7;	            
		private String _AdjustWorkSheet8;	            
		private String _AdjustWorkSheet9;	            
		private String _AdjustWorkSheet10;	            
		private String _JobWroksheet1;	            
		private String _JobWroksheet2;	            
		private String _JobWroksheet3;	            
		private String _JobWroksheet4;	            
		private String _JobWroksheet5;	            
		private String _JobWroksheet6;	            
		private String _JobWroksheet7;	            
		private String _JobWroksheet8;	            
		private String _JobWroksheet9;	            
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
		public String AllowanceWorksheetA
		{	
			get{ return _AllowanceWorksheetA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetA, value, _AllowanceWorksheetA);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetB
		{	
			get{ return _AllowanceWorksheetB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetB, value, _AllowanceWorksheetB);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetC
		{	
			get{ return _AllowanceWorksheetC; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetC, value, _AllowanceWorksheetC);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetC = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetD
		{	
			get{ return _AllowanceWorksheetD; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetD, value, _AllowanceWorksheetD);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetD = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetE
		{	
			get{ return _AllowanceWorksheetE; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetE, value, _AllowanceWorksheetE);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetE = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetF
		{	
			get{ return _AllowanceWorksheetF; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetF, value, _AllowanceWorksheetF);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetF = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetG
		{	
			get{ return _AllowanceWorksheetG; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetG, value, _AllowanceWorksheetG);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetG = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AllowanceWorksheetH
		{	
			get{ return _AllowanceWorksheetH; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AllowanceWorksheetH, value, _AllowanceWorksheetH);
				if (PropertyChanging(args))
				{
					_AllowanceWorksheetH = value;
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
		public Nullable<Boolean> Single
		{	
			get{ return _Single; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Single, value, _Single);
				if (PropertyChanging(args))
				{
					_Single = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Married
		{	
			get{ return _Married; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Married, value, _Married);
				if (PropertyChanging(args))
				{
					_Married = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> MarriadButSeparated
		{	
			get{ return _MarriadButSeparated; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MarriadButSeparated, value, _MarriadButSeparated);
				if (PropertyChanging(args))
				{
					_MarriadButSeparated = value;
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
		public Nullable<Boolean> ReplaceSSNCard4
		{	
			get{ return _ReplaceSSNCard4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReplaceSSNCard4, value, _ReplaceSSNCard4);
				if (PropertyChanging(args))
				{
					_ReplaceSSNCard4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> TotalAllowance5
		{	
			get{ return _TotalAllowance5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalAllowance5, value, _TotalAllowance5);
				if (PropertyChanging(args))
				{
					_TotalAllowance5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AdditionalAmount6
		{	
			get{ return _AdditionalAmount6; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdditionalAmount6, value, _AdditionalAmount6);
				if (PropertyChanging(args))
				{
					_AdditionalAmount6 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoTaxLiability7
		{	
			get{ return _NoTaxLiability7; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoTaxLiability7, value, _NoTaxLiability7);
				if (PropertyChanging(args))
				{
					_NoTaxLiability7 = value;
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
		public Nullable<DateTime> SingatureDate
		{	
			get{ return _SingatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SingatureDate, value, _SingatureDate);
				if (PropertyChanging(args))
				{
					_SingatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmployernameAndAddress
		{	
			get{ return _EmployernameAndAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployernameAndAddress, value, _EmployernameAndAddress);
				if (PropertyChanging(args))
				{
					_EmployernameAndAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OfficeCode
		{	
			get{ return _OfficeCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OfficeCode, value, _OfficeCode);
				if (PropertyChanging(args))
				{
					_OfficeCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmployerEIN
		{	
			get{ return _EmployerEIN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployerEIN, value, _EmployerEIN);
				if (PropertyChanging(args))
				{
					_EmployerEIN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet1
		{	
			get{ return _AdjustWorkSheet1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet1, value, _AdjustWorkSheet1);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet2
		{	
			get{ return _AdjustWorkSheet2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet2, value, _AdjustWorkSheet2);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet3
		{	
			get{ return _AdjustWorkSheet3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet3, value, _AdjustWorkSheet3);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet4
		{	
			get{ return _AdjustWorkSheet4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet4, value, _AdjustWorkSheet4);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet5
		{	
			get{ return _AdjustWorkSheet5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet5, value, _AdjustWorkSheet5);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet6
		{	
			get{ return _AdjustWorkSheet6; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet6, value, _AdjustWorkSheet6);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet6 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet7
		{	
			get{ return _AdjustWorkSheet7; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet7, value, _AdjustWorkSheet7);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet7 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet8
		{	
			get{ return _AdjustWorkSheet8; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet8, value, _AdjustWorkSheet8);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet8 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet9
		{	
			get{ return _AdjustWorkSheet9; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet9, value, _AdjustWorkSheet9);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet9 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdjustWorkSheet10
		{	
			get{ return _AdjustWorkSheet10; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustWorkSheet10, value, _AdjustWorkSheet10);
				if (PropertyChanging(args))
				{
					_AdjustWorkSheet10 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet1
		{	
			get{ return _JobWroksheet1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet1, value, _JobWroksheet1);
				if (PropertyChanging(args))
				{
					_JobWroksheet1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet2
		{	
			get{ return _JobWroksheet2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet2, value, _JobWroksheet2);
				if (PropertyChanging(args))
				{
					_JobWroksheet2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet3
		{	
			get{ return _JobWroksheet3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet3, value, _JobWroksheet3);
				if (PropertyChanging(args))
				{
					_JobWroksheet3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet4
		{	
			get{ return _JobWroksheet4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet4, value, _JobWroksheet4);
				if (PropertyChanging(args))
				{
					_JobWroksheet4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet5
		{	
			get{ return _JobWroksheet5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet5, value, _JobWroksheet5);
				if (PropertyChanging(args))
				{
					_JobWroksheet5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet6
		{	
			get{ return _JobWroksheet6; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet6, value, _JobWroksheet6);
				if (PropertyChanging(args))
				{
					_JobWroksheet6 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet7
		{	
			get{ return _JobWroksheet7; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet7, value, _JobWroksheet7);
				if (PropertyChanging(args))
				{
					_JobWroksheet7 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet8
		{	
			get{ return _JobWroksheet8; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet8, value, _JobWroksheet8);
				if (PropertyChanging(args))
				{
					_JobWroksheet8 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobWroksheet9
		{	
			get{ return _JobWroksheet9; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobWroksheet9, value, _JobWroksheet9);
				if (PropertyChanging(args))
				{
					_JobWroksheet9 = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RecruitmentW4FormBase Clone()
		{
			RecruitmentW4FormBase newObj = new  RecruitmentW4FormBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FormId = this.FormId;						
			newObj.AllowanceWorksheetA = this.AllowanceWorksheetA;						
			newObj.AllowanceWorksheetB = this.AllowanceWorksheetB;						
			newObj.AllowanceWorksheetC = this.AllowanceWorksheetC;						
			newObj.AllowanceWorksheetD = this.AllowanceWorksheetD;						
			newObj.AllowanceWorksheetE = this.AllowanceWorksheetE;						
			newObj.AllowanceWorksheetF = this.AllowanceWorksheetF;						
			newObj.AllowanceWorksheetG = this.AllowanceWorksheetG;						
			newObj.AllowanceWorksheetH = this.AllowanceWorksheetH;						
			newObj.FirstName = this.FirstName;						
			newObj.MiddleInitial = this.MiddleInitial;						
			newObj.LastName = this.LastName;						
			newObj.SSN = this.SSN;						
			newObj.Address = this.Address;						
			newObj.Single = this.Single;						
			newObj.Married = this.Married;						
			newObj.MarriadButSeparated = this.MarriadButSeparated;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zipcode = this.Zipcode;						
			newObj.ReplaceSSNCard4 = this.ReplaceSSNCard4;						
			newObj.TotalAllowance5 = this.TotalAllowance5;						
			newObj.AdditionalAmount6 = this.AdditionalAmount6;						
			newObj.NoTaxLiability7 = this.NoTaxLiability7;						
			newObj.Signature = this.Signature;						
			newObj.SingatureDate = this.SingatureDate;						
			newObj.EmployernameAndAddress = this.EmployernameAndAddress;						
			newObj.OfficeCode = this.OfficeCode;						
			newObj.EmployerEIN = this.EmployerEIN;						
			newObj.AdjustWorkSheet1 = this.AdjustWorkSheet1;						
			newObj.AdjustWorkSheet2 = this.AdjustWorkSheet2;						
			newObj.AdjustWorkSheet3 = this.AdjustWorkSheet3;						
			newObj.AdjustWorkSheet4 = this.AdjustWorkSheet4;						
			newObj.AdjustWorkSheet5 = this.AdjustWorkSheet5;						
			newObj.AdjustWorkSheet6 = this.AdjustWorkSheet6;						
			newObj.AdjustWorkSheet7 = this.AdjustWorkSheet7;						
			newObj.AdjustWorkSheet8 = this.AdjustWorkSheet8;						
			newObj.AdjustWorkSheet9 = this.AdjustWorkSheet9;						
			newObj.AdjustWorkSheet10 = this.AdjustWorkSheet10;						
			newObj.JobWroksheet1 = this.JobWroksheet1;						
			newObj.JobWroksheet2 = this.JobWroksheet2;						
			newObj.JobWroksheet3 = this.JobWroksheet3;						
			newObj.JobWroksheet4 = this.JobWroksheet4;						
			newObj.JobWroksheet5 = this.JobWroksheet5;						
			newObj.JobWroksheet6 = this.JobWroksheet6;						
			newObj.JobWroksheet7 = this.JobWroksheet7;						
			newObj.JobWroksheet8 = this.JobWroksheet8;						
			newObj.JobWroksheet9 = this.JobWroksheet9;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RecruitmentW4FormBase.Property_Id, Id);				
			info.AddValue(RecruitmentW4FormBase.Property_FormId, FormId);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetA, AllowanceWorksheetA);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetB, AllowanceWorksheetB);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetC, AllowanceWorksheetC);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetD, AllowanceWorksheetD);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetE, AllowanceWorksheetE);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetF, AllowanceWorksheetF);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetG, AllowanceWorksheetG);				
			info.AddValue(RecruitmentW4FormBase.Property_AllowanceWorksheetH, AllowanceWorksheetH);				
			info.AddValue(RecruitmentW4FormBase.Property_FirstName, FirstName);				
			info.AddValue(RecruitmentW4FormBase.Property_MiddleInitial, MiddleInitial);				
			info.AddValue(RecruitmentW4FormBase.Property_LastName, LastName);				
			info.AddValue(RecruitmentW4FormBase.Property_SSN, SSN);				
			info.AddValue(RecruitmentW4FormBase.Property_Address, Address);				
			info.AddValue(RecruitmentW4FormBase.Property_Single, Single);				
			info.AddValue(RecruitmentW4FormBase.Property_Married, Married);				
			info.AddValue(RecruitmentW4FormBase.Property_MarriadButSeparated, MarriadButSeparated);				
			info.AddValue(RecruitmentW4FormBase.Property_City, City);				
			info.AddValue(RecruitmentW4FormBase.Property_State, State);				
			info.AddValue(RecruitmentW4FormBase.Property_Zipcode, Zipcode);				
			info.AddValue(RecruitmentW4FormBase.Property_ReplaceSSNCard4, ReplaceSSNCard4);				
			info.AddValue(RecruitmentW4FormBase.Property_TotalAllowance5, TotalAllowance5);				
			info.AddValue(RecruitmentW4FormBase.Property_AdditionalAmount6, AdditionalAmount6);				
			info.AddValue(RecruitmentW4FormBase.Property_NoTaxLiability7, NoTaxLiability7);				
			info.AddValue(RecruitmentW4FormBase.Property_Signature, Signature);				
			info.AddValue(RecruitmentW4FormBase.Property_SingatureDate, SingatureDate);				
			info.AddValue(RecruitmentW4FormBase.Property_EmployernameAndAddress, EmployernameAndAddress);				
			info.AddValue(RecruitmentW4FormBase.Property_OfficeCode, OfficeCode);				
			info.AddValue(RecruitmentW4FormBase.Property_EmployerEIN, EmployerEIN);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet1, AdjustWorkSheet1);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet2, AdjustWorkSheet2);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet3, AdjustWorkSheet3);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet4, AdjustWorkSheet4);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet5, AdjustWorkSheet5);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet6, AdjustWorkSheet6);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet7, AdjustWorkSheet7);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet8, AdjustWorkSheet8);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet9, AdjustWorkSheet9);				
			info.AddValue(RecruitmentW4FormBase.Property_AdjustWorkSheet10, AdjustWorkSheet10);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet1, JobWroksheet1);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet2, JobWroksheet2);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet3, JobWroksheet3);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet4, JobWroksheet4);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet5, JobWroksheet5);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet6, JobWroksheet6);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet7, JobWroksheet7);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet8, JobWroksheet8);				
			info.AddValue(RecruitmentW4FormBase.Property_JobWroksheet9, JobWroksheet9);				
		}
		#endregion

		
	}
}
