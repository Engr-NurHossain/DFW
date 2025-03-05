using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PaymentInfoBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PaymentInfoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			AccountName = 2,
			BankAccountType = 3,
			RoutingNo = 4,
			AcountNo = 5,
			CardType = 6,
			CardNumber = 7,
			CardExpireDate = 8,
			CardSecurityCode = 9,
			CheckNo = 10,
			IsCash = 11,
			EcheckType = 12,
			FileName = 13,
			BankName = 14,
			IsForBrinks = 15,
			IsPartialPayment = 16,
			IsInitialPayment = 17,
			City = 18,
			State = 19,
			ZipCode = 20,
			Street = 21,
			CusotmerId = 22,
			Token = 23,
			RMRToken = 24
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_AccountName = "AccountName";		            
		public const string Property_BankAccountType = "BankAccountType";		            
		public const string Property_RoutingNo = "RoutingNo";		            
		public const string Property_AcountNo = "AcountNo";		            
		public const string Property_CardType = "CardType";		            
		public const string Property_CardNumber = "CardNumber";		            
		public const string Property_CardExpireDate = "CardExpireDate";		            
		public const string Property_CardSecurityCode = "CardSecurityCode";		            
		public const string Property_CheckNo = "CheckNo";		            
		public const string Property_IsCash = "IsCash";		            
		public const string Property_EcheckType = "EcheckType";		            
		public const string Property_FileName = "FileName";		            
		public const string Property_BankName = "BankName";		            
		public const string Property_IsForBrinks = "IsForBrinks";		            
		public const string Property_IsPartialPayment = "IsPartialPayment";		            
		public const string Property_IsInitialPayment = "IsInitialPayment";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Street = "Street";		            
		public const string Property_CusotmerId = "CusotmerId";		            
		public const string Property_Token = "Token";		            
		public const string Property_RMRToken = "RMRToken";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _AccountName;	            
		private String _BankAccountType;	            
		private String _RoutingNo;	            
		private String _AcountNo;	            
		private String _CardType;	            
		private String _CardNumber;	            
		private String _CardExpireDate;	            
		private String _CardSecurityCode;	            
		private String _CheckNo;	            
		private Nullable<Boolean> _IsCash;	            
		private String _EcheckType;	            
		private String _FileName;	            
		private String _BankName;	            
		private Nullable<Boolean> _IsForBrinks;	            
		private Nullable<Boolean> _IsPartialPayment;	            
		private Nullable<Boolean> _IsInitialPayment;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _Street;	            
		private Guid _CusotmerId;	            
		private String _Token;	            
		private String _RMRToken;	            
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
		public String AccountName
		{	
			get{ return _AccountName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountName, value, _AccountName);
				if (PropertyChanging(args))
				{
					_AccountName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BankAccountType
		{	
			get{ return _BankAccountType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BankAccountType, value, _BankAccountType);
				if (PropertyChanging(args))
				{
					_BankAccountType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RoutingNo
		{	
			get{ return _RoutingNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RoutingNo, value, _RoutingNo);
				if (PropertyChanging(args))
				{
					_RoutingNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AcountNo
		{	
			get{ return _AcountNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AcountNo, value, _AcountNo);
				if (PropertyChanging(args))
				{
					_AcountNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardType
		{	
			get{ return _CardType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardType, value, _CardType);
				if (PropertyChanging(args))
				{
					_CardType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardNumber
		{	
			get{ return _CardNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardNumber, value, _CardNumber);
				if (PropertyChanging(args))
				{
					_CardNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardExpireDate
		{	
			get{ return _CardExpireDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardExpireDate, value, _CardExpireDate);
				if (PropertyChanging(args))
				{
					_CardExpireDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardSecurityCode
		{	
			get{ return _CardSecurityCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardSecurityCode, value, _CardSecurityCode);
				if (PropertyChanging(args))
				{
					_CardSecurityCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CheckNo
		{	
			get{ return _CheckNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CheckNo, value, _CheckNo);
				if (PropertyChanging(args))
				{
					_CheckNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCash
		{	
			get{ return _IsCash; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCash, value, _IsCash);
				if (PropertyChanging(args))
				{
					_IsCash = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EcheckType
		{	
			get{ return _EcheckType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EcheckType, value, _EcheckType);
				if (PropertyChanging(args))
				{
					_EcheckType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileName
		{	
			get{ return _FileName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileName, value, _FileName);
				if (PropertyChanging(args))
				{
					_FileName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BankName
		{	
			get{ return _BankName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BankName, value, _BankName);
				if (PropertyChanging(args))
				{
					_BankName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsForBrinks
		{	
			get{ return _IsForBrinks; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsForBrinks, value, _IsForBrinks);
				if (PropertyChanging(args))
				{
					_IsForBrinks = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPartialPayment
		{	
			get{ return _IsPartialPayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPartialPayment, value, _IsPartialPayment);
				if (PropertyChanging(args))
				{
					_IsPartialPayment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsInitialPayment
		{	
			get{ return _IsInitialPayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInitialPayment, value, _IsInitialPayment);
				if (PropertyChanging(args))
				{
					_IsInitialPayment = value;
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
		public Guid CusotmerId
		{	
			get{ return _CusotmerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CusotmerId, value, _CusotmerId);
				if (PropertyChanging(args))
				{
					_CusotmerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Token
		{	
			get{ return _Token; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Token, value, _Token);
				if (PropertyChanging(args))
				{
					_Token = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RMRToken
		{	
			get{ return _RMRToken; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RMRToken, value, _RMRToken);
				if (PropertyChanging(args))
				{
					_RMRToken = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PaymentInfoBase Clone()
		{
			PaymentInfoBase newObj = new  PaymentInfoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.AccountName = this.AccountName;						
			newObj.BankAccountType = this.BankAccountType;						
			newObj.RoutingNo = this.RoutingNo;						
			newObj.AcountNo = this.AcountNo;						
			newObj.CardType = this.CardType;						
			newObj.CardNumber = this.CardNumber;						
			newObj.CardExpireDate = this.CardExpireDate;						
			newObj.CardSecurityCode = this.CardSecurityCode;						
			newObj.CheckNo = this.CheckNo;						
			newObj.IsCash = this.IsCash;						
			newObj.EcheckType = this.EcheckType;						
			newObj.FileName = this.FileName;						
			newObj.BankName = this.BankName;						
			newObj.IsForBrinks = this.IsForBrinks;						
			newObj.IsPartialPayment = this.IsPartialPayment;						
			newObj.IsInitialPayment = this.IsInitialPayment;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Street = this.Street;						
			newObj.CusotmerId = this.CusotmerId;						
			newObj.Token = this.Token;						
			newObj.RMRToken = this.RMRToken;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PaymentInfoBase.Property_Id, Id);				
			info.AddValue(PaymentInfoBase.Property_CompanyId, CompanyId);				
			info.AddValue(PaymentInfoBase.Property_AccountName, AccountName);				
			info.AddValue(PaymentInfoBase.Property_BankAccountType, BankAccountType);				
			info.AddValue(PaymentInfoBase.Property_RoutingNo, RoutingNo);				
			info.AddValue(PaymentInfoBase.Property_AcountNo, AcountNo);				
			info.AddValue(PaymentInfoBase.Property_CardType, CardType);				
			info.AddValue(PaymentInfoBase.Property_CardNumber, CardNumber);				
			info.AddValue(PaymentInfoBase.Property_CardExpireDate, CardExpireDate);				
			info.AddValue(PaymentInfoBase.Property_CardSecurityCode, CardSecurityCode);				
			info.AddValue(PaymentInfoBase.Property_CheckNo, CheckNo);				
			info.AddValue(PaymentInfoBase.Property_IsCash, IsCash);				
			info.AddValue(PaymentInfoBase.Property_EcheckType, EcheckType);				
			info.AddValue(PaymentInfoBase.Property_FileName, FileName);				
			info.AddValue(PaymentInfoBase.Property_BankName, BankName);				
			info.AddValue(PaymentInfoBase.Property_IsForBrinks, IsForBrinks);				
			info.AddValue(PaymentInfoBase.Property_IsPartialPayment, IsPartialPayment);				
			info.AddValue(PaymentInfoBase.Property_IsInitialPayment, IsInitialPayment);				
			info.AddValue(PaymentInfoBase.Property_City, City);				
			info.AddValue(PaymentInfoBase.Property_State, State);				
			info.AddValue(PaymentInfoBase.Property_ZipCode, ZipCode);				
			info.AddValue(PaymentInfoBase.Property_Street, Street);				
			info.AddValue(PaymentInfoBase.Property_CusotmerId, CusotmerId);				
			info.AddValue(PaymentInfoBase.Property_Token, Token);				
			info.AddValue(PaymentInfoBase.Property_RMRToken, RMRToken);				
		}
		#endregion

		
	}
}
