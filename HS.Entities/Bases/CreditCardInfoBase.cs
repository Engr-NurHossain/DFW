using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CreditCardInfoBase", Namespace = "http://www.piistech.com//entities")]
	public class CreditCardInfoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			CardType = 3,
			CardNumber = 4,
			CardName = 5,
			ExpireMonth = 6,
			ExpireYear = 7,
			StreetAddress = 8,
			City = 9,
			State = 10,
			ZipCode = 11,
			Country = 12,
			IsDefault = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CardType = "CardType";		            
		public const string Property_CardNumber = "CardNumber";		            
		public const string Property_CardName = "CardName";		            
		public const string Property_ExpireMonth = "ExpireMonth";		            
		public const string Property_ExpireYear = "ExpireYear";		            
		public const string Property_StreetAddress = "StreetAddress";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Country = "Country";		            
		public const string Property_IsDefault = "IsDefault";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _CardType;	            
		private String _CardNumber;	            
		private String _CardName;	            
		private Int32 _ExpireMonth;	            
		private Int32 _ExpireYear;	            
		private String _StreetAddress;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _Country;	            
		private Boolean _IsDefault;	            
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
		public String CardName
		{	
			get{ return _CardName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardName, value, _CardName);
				if (PropertyChanging(args))
				{
					_CardName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ExpireMonth
		{	
			get{ return _ExpireMonth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpireMonth, value, _ExpireMonth);
				if (PropertyChanging(args))
				{
					_ExpireMonth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ExpireYear
		{	
			get{ return _ExpireYear; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpireYear, value, _ExpireYear);
				if (PropertyChanging(args))
				{
					_ExpireYear = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StreetAddress
		{	
			get{ return _StreetAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StreetAddress, value, _StreetAddress);
				if (PropertyChanging(args))
				{
					_StreetAddress = value;
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
		public String Country
		{	
			get{ return _Country; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Country, value, _Country);
				if (PropertyChanging(args))
				{
					_Country = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsDefault
		{	
			get{ return _IsDefault; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefault, value, _IsDefault);
				if (PropertyChanging(args))
				{
					_IsDefault = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CreditCardInfoBase Clone()
		{
			CreditCardInfoBase newObj = new  CreditCardInfoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CardType = this.CardType;						
			newObj.CardNumber = this.CardNumber;						
			newObj.CardName = this.CardName;						
			newObj.ExpireMonth = this.ExpireMonth;						
			newObj.ExpireYear = this.ExpireYear;						
			newObj.StreetAddress = this.StreetAddress;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Country = this.Country;						
			newObj.IsDefault = this.IsDefault;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CreditCardInfoBase.Property_Id, Id);				
			info.AddValue(CreditCardInfoBase.Property_CompanyId, CompanyId);				
			info.AddValue(CreditCardInfoBase.Property_CustomerId, CustomerId);				
			info.AddValue(CreditCardInfoBase.Property_CardType, CardType);				
			info.AddValue(CreditCardInfoBase.Property_CardNumber, CardNumber);				
			info.AddValue(CreditCardInfoBase.Property_CardName, CardName);				
			info.AddValue(CreditCardInfoBase.Property_ExpireMonth, ExpireMonth);				
			info.AddValue(CreditCardInfoBase.Property_ExpireYear, ExpireYear);				
			info.AddValue(CreditCardInfoBase.Property_StreetAddress, StreetAddress);				
			info.AddValue(CreditCardInfoBase.Property_City, City);				
			info.AddValue(CreditCardInfoBase.Property_State, State);				
			info.AddValue(CreditCardInfoBase.Property_ZipCode, ZipCode);				
			info.AddValue(CreditCardInfoBase.Property_Country, Country);				
			info.AddValue(CreditCardInfoBase.Property_IsDefault, IsDefault);				
		}
		#endregion

		
	}
}
