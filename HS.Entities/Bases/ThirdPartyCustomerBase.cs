using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ThirdPartyCustomerBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ThirdPartyCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CustomerNumber = 2,
			SiteName = 3,
			TransectionID = 4,
			DealerNumber = 5,
			AccountOnlineDate = 6,
			CodeWord = 7,
			SiteAddress = 8,
			ReceiverPhone = 9,
			PanelPhone = 10,
			PanelLocation = 11,
			InstallDate = 12,
			PanelCode = 13,
			City = 14,
			State = 15,
			ZipCode = 16,
			CountryName = 17,
			CrossStreet = 18,
			eContact = 19,
			IsSold = 20,
			CreatedBy = 21,
			Platform = 22
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CustomerNumber = "CustomerNumber";		            
		public const string Property_SiteName = "SiteName";		            
		public const string Property_TransectionID = "TransectionID";		            
		public const string Property_DealerNumber = "DealerNumber";		            
		public const string Property_AccountOnlineDate = "AccountOnlineDate";		            
		public const string Property_CodeWord = "CodeWord";		            
		public const string Property_SiteAddress = "SiteAddress";		            
		public const string Property_ReceiverPhone = "ReceiverPhone";		            
		public const string Property_PanelPhone = "PanelPhone";		            
		public const string Property_PanelLocation = "PanelLocation";		            
		public const string Property_InstallDate = "InstallDate";		            
		public const string Property_PanelCode = "PanelCode";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_CountryName = "CountryName";		            
		public const string Property_CrossStreet = "CrossStreet";		            
		public const string Property_eContact = "eContact";		            
		public const string Property_IsSold = "IsSold";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_Platform = "Platform";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Nullable<Int32> _CustomerNumber;	            
		private String _SiteName;	            
		private String _TransectionID;	            
		private String _DealerNumber;	            
		private DateTime _AccountOnlineDate;	            
		private String _CodeWord;	            
		private String _SiteAddress;	            
		private String _ReceiverPhone;	            
		private String _PanelPhone;	            
		private String _PanelLocation;	            
		private DateTime _InstallDate;	            
		private String _PanelCode;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _CountryName;	            
		private String _CrossStreet;	            
		private String _eContact;	            
		private Nullable<Boolean> _IsSold;	            
		private Guid _CreatedBy;	            
		private String _Platform;	            
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
		public Nullable<Int32> CustomerNumber
		{	
			get{ return _CustomerNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerNumber, value, _CustomerNumber);
				if (PropertyChanging(args))
				{
					_CustomerNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SiteName
		{	
			get{ return _SiteName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SiteName, value, _SiteName);
				if (PropertyChanging(args))
				{
					_SiteName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransectionID
		{	
			get{ return _TransectionID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransectionID, value, _TransectionID);
				if (PropertyChanging(args))
				{
					_TransectionID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DealerNumber
		{	
			get{ return _DealerNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DealerNumber, value, _DealerNumber);
				if (PropertyChanging(args))
				{
					_DealerNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AccountOnlineDate
		{	
			get{ return _AccountOnlineDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountOnlineDate, value, _AccountOnlineDate);
				if (PropertyChanging(args))
				{
					_AccountOnlineDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CodeWord
		{	
			get{ return _CodeWord; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CodeWord, value, _CodeWord);
				if (PropertyChanging(args))
				{
					_CodeWord = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SiteAddress
		{	
			get{ return _SiteAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SiteAddress, value, _SiteAddress);
				if (PropertyChanging(args))
				{
					_SiteAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReceiverPhone
		{	
			get{ return _ReceiverPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceiverPhone, value, _ReceiverPhone);
				if (PropertyChanging(args))
				{
					_ReceiverPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PanelPhone
		{	
			get{ return _PanelPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelPhone, value, _PanelPhone);
				if (PropertyChanging(args))
				{
					_PanelPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PanelLocation
		{	
			get{ return _PanelLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelLocation, value, _PanelLocation);
				if (PropertyChanging(args))
				{
					_PanelLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime InstallDate
		{	
			get{ return _InstallDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallDate, value, _InstallDate);
				if (PropertyChanging(args))
				{
					_InstallDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PanelCode
		{	
			get{ return _PanelCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelCode, value, _PanelCode);
				if (PropertyChanging(args))
				{
					_PanelCode = value;
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
		public String CountryName
		{	
			get{ return _CountryName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CountryName, value, _CountryName);
				if (PropertyChanging(args))
				{
					_CountryName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CrossStreet
		{	
			get{ return _CrossStreet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CrossStreet, value, _CrossStreet);
				if (PropertyChanging(args))
				{
					_CrossStreet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String eContact
		{	
			get{ return _eContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_eContact, value, _eContact);
				if (PropertyChanging(args))
				{
					_eContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSold
		{	
			get{ return _IsSold; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSold, value, _IsSold);
				if (PropertyChanging(args))
				{
					_IsSold = value;
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
		public String Platform
		{	
			get{ return _Platform; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Platform, value, _Platform);
				if (PropertyChanging(args))
				{
					_Platform = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ThirdPartyCustomerBase Clone()
		{
			ThirdPartyCustomerBase newObj = new  ThirdPartyCustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CustomerNumber = this.CustomerNumber;						
			newObj.SiteName = this.SiteName;						
			newObj.TransectionID = this.TransectionID;						
			newObj.DealerNumber = this.DealerNumber;						
			newObj.AccountOnlineDate = this.AccountOnlineDate;						
			newObj.CodeWord = this.CodeWord;						
			newObj.SiteAddress = this.SiteAddress;						
			newObj.ReceiverPhone = this.ReceiverPhone;						
			newObj.PanelPhone = this.PanelPhone;						
			newObj.PanelLocation = this.PanelLocation;						
			newObj.InstallDate = this.InstallDate;						
			newObj.PanelCode = this.PanelCode;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.CountryName = this.CountryName;						
			newObj.CrossStreet = this.CrossStreet;						
			newObj.eContact = this.eContact;						
			newObj.IsSold = this.IsSold;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.Platform = this.Platform;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ThirdPartyCustomerBase.Property_Id, Id);				
			info.AddValue(ThirdPartyCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(ThirdPartyCustomerBase.Property_CustomerNumber, CustomerNumber);				
			info.AddValue(ThirdPartyCustomerBase.Property_SiteName, SiteName);				
			info.AddValue(ThirdPartyCustomerBase.Property_TransectionID, TransectionID);				
			info.AddValue(ThirdPartyCustomerBase.Property_DealerNumber, DealerNumber);				
			info.AddValue(ThirdPartyCustomerBase.Property_AccountOnlineDate, AccountOnlineDate);				
			info.AddValue(ThirdPartyCustomerBase.Property_CodeWord, CodeWord);				
			info.AddValue(ThirdPartyCustomerBase.Property_SiteAddress, SiteAddress);				
			info.AddValue(ThirdPartyCustomerBase.Property_ReceiverPhone, ReceiverPhone);				
			info.AddValue(ThirdPartyCustomerBase.Property_PanelPhone, PanelPhone);				
			info.AddValue(ThirdPartyCustomerBase.Property_PanelLocation, PanelLocation);				
			info.AddValue(ThirdPartyCustomerBase.Property_InstallDate, InstallDate);				
			info.AddValue(ThirdPartyCustomerBase.Property_PanelCode, PanelCode);				
			info.AddValue(ThirdPartyCustomerBase.Property_City, City);				
			info.AddValue(ThirdPartyCustomerBase.Property_State, State);				
			info.AddValue(ThirdPartyCustomerBase.Property_ZipCode, ZipCode);				
			info.AddValue(ThirdPartyCustomerBase.Property_CountryName, CountryName);				
			info.AddValue(ThirdPartyCustomerBase.Property_CrossStreet, CrossStreet);				
			info.AddValue(ThirdPartyCustomerBase.Property_eContact, eContact);				
			info.AddValue(ThirdPartyCustomerBase.Property_IsSold, IsSold);				
			info.AddValue(ThirdPartyCustomerBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ThirdPartyCustomerBase.Property_Platform, Platform);				
		}
		#endregion

		
	}
}
