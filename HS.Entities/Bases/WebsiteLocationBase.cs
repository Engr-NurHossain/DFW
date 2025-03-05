using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "WebsiteLocationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class WebsiteLocationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			Address = 3,
			Address2 = 4,
			City = 5,
			State = 6,
			Zipcode = 7,
			CreatedDate = 8,
			CreatedBy = 9,
			PrimaryContact = 10,
			DomainName = 11,
			StorePhone = 12,
			TrackingPhonePhone = 13,
			OrdersEmail = 14,
			ThemeLoc = 15,
			HoursofOperation = 16,
			DaysAvailable = 17,
			DaysAvailableOption = 18,
			HoursofOperationOption = 19,
			OperationStartTime = 20,
			OperationEndTime = 21,
			UrlSlug = 22,
			WebsiteURL = 23,
			MetaTitle = 24,
			MetaDescription = 25,
			CartOption = 26,
			ImageLoc = 27,
			IsTax = 28,
			TaxPercentage = 29,
			CuisineType = 30,
			PreparationTime = 31,
			IsInstruction = 32,
			Notes = 33,
			InstructionNotes = 34,
			PaidOption = 35,
			PaymentOption = 36,
			FacebookFollowURL = 37,
			TwitterFollowURL = 38,
			InstagramFollowURL = 39,
			YoutubeFollowURL = 40,
			ReferCompanyId = 41,
			IsDefault = 42,
			CoverImageLoc = 43,
			ExpireTime = 44,
			DeliveryRadius = 45,
			CoverPhotoDate = 46,
			DirectoryPhotoDate = 47,
			LastUpdatedBy = 48,
			LastUpdatedDate = 49,
			DeliveryFee = 50,
			SearchEngineIndex = 51,
			DiscountValue = 52,
			DiscountCode = 53,
			MinimumDeliveryTime = 54,
			MinimumOrderValue = 55
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_Address = "Address";		            
		public const string Property_Address2 = "Address2";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zipcode = "Zipcode";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_PrimaryContact = "PrimaryContact";		            
		public const string Property_DomainName = "DomainName";		            
		public const string Property_StorePhone = "StorePhone";		            
		public const string Property_TrackingPhonePhone = "TrackingPhonePhone";		            
		public const string Property_OrdersEmail = "OrdersEmail";		            
		public const string Property_ThemeLoc = "ThemeLoc";		            
		public const string Property_HoursofOperation = "HoursofOperation";		            
		public const string Property_DaysAvailable = "DaysAvailable";		            
		public const string Property_DaysAvailableOption = "DaysAvailableOption";		            
		public const string Property_HoursofOperationOption = "HoursofOperationOption";		            
		public const string Property_OperationStartTime = "OperationStartTime";		            
		public const string Property_OperationEndTime = "OperationEndTime";		            
		public const string Property_UrlSlug = "UrlSlug";		            
		public const string Property_WebsiteURL = "WebsiteURL";		            
		public const string Property_MetaTitle = "MetaTitle";		            
		public const string Property_MetaDescription = "MetaDescription";		            
		public const string Property_CartOption = "CartOption";		            
		public const string Property_ImageLoc = "ImageLoc";		            
		public const string Property_IsTax = "IsTax";		            
		public const string Property_TaxPercentage = "TaxPercentage";		            
		public const string Property_CuisineType = "CuisineType";		            
		public const string Property_PreparationTime = "PreparationTime";		            
		public const string Property_IsInstruction = "IsInstruction";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_InstructionNotes = "InstructionNotes";		            
		public const string Property_PaidOption = "PaidOption";		            
		public const string Property_PaymentOption = "PaymentOption";		            
		public const string Property_FacebookFollowURL = "FacebookFollowURL";		            
		public const string Property_TwitterFollowURL = "TwitterFollowURL";		            
		public const string Property_InstagramFollowURL = "InstagramFollowURL";		            
		public const string Property_YoutubeFollowURL = "YoutubeFollowURL";		            
		public const string Property_ReferCompanyId = "ReferCompanyId";		            
		public const string Property_IsDefault = "IsDefault";		            
		public const string Property_CoverImageLoc = "CoverImageLoc";		            
		public const string Property_ExpireTime = "ExpireTime";		            
		public const string Property_DeliveryRadius = "DeliveryRadius";		            
		public const string Property_CoverPhotoDate = "CoverPhotoDate";		            
		public const string Property_DirectoryPhotoDate = "DirectoryPhotoDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_DeliveryFee = "DeliveryFee";		            
		public const string Property_SearchEngineIndex = "SearchEngineIndex";		            
		public const string Property_DiscountValue = "DiscountValue";		            
		public const string Property_DiscountCode = "DiscountCode";		            
		public const string Property_MinimumDeliveryTime = "MinimumDeliveryTime";		            
		public const string Property_MinimumOrderValue = "MinimumOrderValue";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _Address;	            
		private String _Address2;	            
		private String _City;	            
		private String _State;	            
		private String _Zipcode;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private String _PrimaryContact;	            
		private String _DomainName;	            
		private String _StorePhone;	            
		private String _TrackingPhonePhone;	            
		private Nullable<Boolean> _OrdersEmail;	            
		private String _ThemeLoc;	            
		private String _HoursofOperation;	            
		private String _DaysAvailable;	            
		private String _DaysAvailableOption;	            
		private String _HoursofOperationOption;	            
		private String _OperationStartTime;	            
		private String _OperationEndTime;	            
		private String _UrlSlug;	            
		private String _WebsiteURL;	            
		private String _MetaTitle;	            
		private String _MetaDescription;	            
		private String _CartOption;	            
		private String _ImageLoc;	            
		private Nullable<Boolean> _IsTax;	            
		private Nullable<Double> _TaxPercentage;	            
		private String _CuisineType;	            
		private Nullable<Int32> _PreparationTime;	            
		private Nullable<Boolean> _IsInstruction;	            
		private String _Notes;	            
		private String _InstructionNotes;	            
		private String _PaidOption;	            
		private String _PaymentOption;	            
		private String _FacebookFollowURL;	            
		private String _TwitterFollowURL;	            
		private String _InstagramFollowURL;	            
		private String _YoutubeFollowURL;	            
		private Guid _ReferCompanyId;	            
		private Nullable<Boolean> _IsDefault;	            
		private String _CoverImageLoc;	            
		private String _ExpireTime;	            
		private Nullable<Double> _DeliveryRadius;	            
		private Nullable<DateTime> _CoverPhotoDate;	            
		private Nullable<DateTime> _DirectoryPhotoDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Double> _DeliveryFee;	            
		private Nullable<Boolean> _SearchEngineIndex;	            
		private Nullable<Double> _DiscountValue;	            
		private String _DiscountCode;	            
		private Nullable<Int32> _MinimumDeliveryTime;	            
		private Nullable<Double> _MinimumOrderValue;	            
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
		public String Address2
		{	
			get{ return _Address2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address2, value, _Address2);
				if (PropertyChanging(args))
				{
					_Address2 = value;
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
		public String PrimaryContact
		{	
			get{ return _PrimaryContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryContact, value, _PrimaryContact);
				if (PropertyChanging(args))
				{
					_PrimaryContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DomainName
		{	
			get{ return _DomainName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DomainName, value, _DomainName);
				if (PropertyChanging(args))
				{
					_DomainName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StorePhone
		{	
			get{ return _StorePhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StorePhone, value, _StorePhone);
				if (PropertyChanging(args))
				{
					_StorePhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TrackingPhonePhone
		{	
			get{ return _TrackingPhonePhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TrackingPhonePhone, value, _TrackingPhonePhone);
				if (PropertyChanging(args))
				{
					_TrackingPhonePhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> OrdersEmail
		{	
			get{ return _OrdersEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrdersEmail, value, _OrdersEmail);
				if (PropertyChanging(args))
				{
					_OrdersEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ThemeLoc
		{	
			get{ return _ThemeLoc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ThemeLoc, value, _ThemeLoc);
				if (PropertyChanging(args))
				{
					_ThemeLoc = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HoursofOperation
		{	
			get{ return _HoursofOperation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HoursofOperation, value, _HoursofOperation);
				if (PropertyChanging(args))
				{
					_HoursofOperation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DaysAvailable
		{	
			get{ return _DaysAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DaysAvailable, value, _DaysAvailable);
				if (PropertyChanging(args))
				{
					_DaysAvailable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DaysAvailableOption
		{	
			get{ return _DaysAvailableOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DaysAvailableOption, value, _DaysAvailableOption);
				if (PropertyChanging(args))
				{
					_DaysAvailableOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HoursofOperationOption
		{	
			get{ return _HoursofOperationOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HoursofOperationOption, value, _HoursofOperationOption);
				if (PropertyChanging(args))
				{
					_HoursofOperationOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OperationStartTime
		{	
			get{ return _OperationStartTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OperationStartTime, value, _OperationStartTime);
				if (PropertyChanging(args))
				{
					_OperationStartTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OperationEndTime
		{	
			get{ return _OperationEndTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OperationEndTime, value, _OperationEndTime);
				if (PropertyChanging(args))
				{
					_OperationEndTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UrlSlug
		{	
			get{ return _UrlSlug; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UrlSlug, value, _UrlSlug);
				if (PropertyChanging(args))
				{
					_UrlSlug = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WebsiteURL
		{	
			get{ return _WebsiteURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WebsiteURL, value, _WebsiteURL);
				if (PropertyChanging(args))
				{
					_WebsiteURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MetaTitle
		{	
			get{ return _MetaTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MetaTitle, value, _MetaTitle);
				if (PropertyChanging(args))
				{
					_MetaTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MetaDescription
		{	
			get{ return _MetaDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MetaDescription, value, _MetaDescription);
				if (PropertyChanging(args))
				{
					_MetaDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CartOption
		{	
			get{ return _CartOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CartOption, value, _CartOption);
				if (PropertyChanging(args))
				{
					_CartOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ImageLoc
		{	
			get{ return _ImageLoc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ImageLoc, value, _ImageLoc);
				if (PropertyChanging(args))
				{
					_ImageLoc = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTax
		{	
			get{ return _IsTax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTax, value, _IsTax);
				if (PropertyChanging(args))
				{
					_IsTax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxPercentage
		{	
			get{ return _TaxPercentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxPercentage, value, _TaxPercentage);
				if (PropertyChanging(args))
				{
					_TaxPercentage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CuisineType
		{	
			get{ return _CuisineType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CuisineType, value, _CuisineType);
				if (PropertyChanging(args))
				{
					_CuisineType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> PreparationTime
		{	
			get{ return _PreparationTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreparationTime, value, _PreparationTime);
				if (PropertyChanging(args))
				{
					_PreparationTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsInstruction
		{	
			get{ return _IsInstruction; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInstruction, value, _IsInstruction);
				if (PropertyChanging(args))
				{
					_IsInstruction = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstructionNotes
		{	
			get{ return _InstructionNotes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstructionNotes, value, _InstructionNotes);
				if (PropertyChanging(args))
				{
					_InstructionNotes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaidOption
		{	
			get{ return _PaidOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaidOption, value, _PaidOption);
				if (PropertyChanging(args))
				{
					_PaidOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentOption
		{	
			get{ return _PaymentOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentOption, value, _PaymentOption);
				if (PropertyChanging(args))
				{
					_PaymentOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FacebookFollowURL
		{	
			get{ return _FacebookFollowURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FacebookFollowURL, value, _FacebookFollowURL);
				if (PropertyChanging(args))
				{
					_FacebookFollowURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TwitterFollowURL
		{	
			get{ return _TwitterFollowURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TwitterFollowURL, value, _TwitterFollowURL);
				if (PropertyChanging(args))
				{
					_TwitterFollowURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstagramFollowURL
		{	
			get{ return _InstagramFollowURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstagramFollowURL, value, _InstagramFollowURL);
				if (PropertyChanging(args))
				{
					_InstagramFollowURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String YoutubeFollowURL
		{	
			get{ return _YoutubeFollowURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_YoutubeFollowURL, value, _YoutubeFollowURL);
				if (PropertyChanging(args))
				{
					_YoutubeFollowURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ReferCompanyId
		{	
			get{ return _ReferCompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferCompanyId, value, _ReferCompanyId);
				if (PropertyChanging(args))
				{
					_ReferCompanyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDefault
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

		[DataMember]
		public String CoverImageLoc
		{	
			get{ return _CoverImageLoc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoverImageLoc, value, _CoverImageLoc);
				if (PropertyChanging(args))
				{
					_CoverImageLoc = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExpireTime
		{	
			get{ return _ExpireTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpireTime, value, _ExpireTime);
				if (PropertyChanging(args))
				{
					_ExpireTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DeliveryRadius
		{	
			get{ return _DeliveryRadius; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DeliveryRadius, value, _DeliveryRadius);
				if (PropertyChanging(args))
				{
					_DeliveryRadius = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CoverPhotoDate
		{	
			get{ return _CoverPhotoDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoverPhotoDate, value, _CoverPhotoDate);
				if (PropertyChanging(args))
				{
					_CoverPhotoDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DirectoryPhotoDate
		{	
			get{ return _DirectoryPhotoDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DirectoryPhotoDate, value, _DirectoryPhotoDate);
				if (PropertyChanging(args))
				{
					_DirectoryPhotoDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
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
		public Nullable<Double> DeliveryFee
		{	
			get{ return _DeliveryFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DeliveryFee, value, _DeliveryFee);
				if (PropertyChanging(args))
				{
					_DeliveryFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> SearchEngineIndex
		{	
			get{ return _SearchEngineIndex; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchEngineIndex, value, _SearchEngineIndex);
				if (PropertyChanging(args))
				{
					_SearchEngineIndex = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountValue
		{	
			get{ return _DiscountValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountValue, value, _DiscountValue);
				if (PropertyChanging(args))
				{
					_DiscountValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DiscountCode
		{	
			get{ return _DiscountCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountCode, value, _DiscountCode);
				if (PropertyChanging(args))
				{
					_DiscountCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> MinimumDeliveryTime
		{	
			get{ return _MinimumDeliveryTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinimumDeliveryTime, value, _MinimumDeliveryTime);
				if (PropertyChanging(args))
				{
					_MinimumDeliveryTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MinimumOrderValue
		{	
			get{ return _MinimumOrderValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinimumOrderValue, value, _MinimumOrderValue);
				if (PropertyChanging(args))
				{
					_MinimumOrderValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  WebsiteLocationBase Clone()
		{
			WebsiteLocationBase newObj = new  WebsiteLocationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.Address = this.Address;						
			newObj.Address2 = this.Address2;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zipcode = this.Zipcode;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.PrimaryContact = this.PrimaryContact;						
			newObj.DomainName = this.DomainName;						
			newObj.StorePhone = this.StorePhone;						
			newObj.TrackingPhonePhone = this.TrackingPhonePhone;						
			newObj.OrdersEmail = this.OrdersEmail;						
			newObj.ThemeLoc = this.ThemeLoc;						
			newObj.HoursofOperation = this.HoursofOperation;						
			newObj.DaysAvailable = this.DaysAvailable;						
			newObj.DaysAvailableOption = this.DaysAvailableOption;						
			newObj.HoursofOperationOption = this.HoursofOperationOption;						
			newObj.OperationStartTime = this.OperationStartTime;						
			newObj.OperationEndTime = this.OperationEndTime;						
			newObj.UrlSlug = this.UrlSlug;						
			newObj.WebsiteURL = this.WebsiteURL;						
			newObj.MetaTitle = this.MetaTitle;						
			newObj.MetaDescription = this.MetaDescription;						
			newObj.CartOption = this.CartOption;						
			newObj.ImageLoc = this.ImageLoc;						
			newObj.IsTax = this.IsTax;						
			newObj.TaxPercentage = this.TaxPercentage;						
			newObj.CuisineType = this.CuisineType;						
			newObj.PreparationTime = this.PreparationTime;						
			newObj.IsInstruction = this.IsInstruction;						
			newObj.Notes = this.Notes;						
			newObj.InstructionNotes = this.InstructionNotes;						
			newObj.PaidOption = this.PaidOption;						
			newObj.PaymentOption = this.PaymentOption;						
			newObj.FacebookFollowURL = this.FacebookFollowURL;						
			newObj.TwitterFollowURL = this.TwitterFollowURL;						
			newObj.InstagramFollowURL = this.InstagramFollowURL;						
			newObj.YoutubeFollowURL = this.YoutubeFollowURL;						
			newObj.ReferCompanyId = this.ReferCompanyId;						
			newObj.IsDefault = this.IsDefault;						
			newObj.CoverImageLoc = this.CoverImageLoc;						
			newObj.ExpireTime = this.ExpireTime;						
			newObj.DeliveryRadius = this.DeliveryRadius;						
			newObj.CoverPhotoDate = this.CoverPhotoDate;						
			newObj.DirectoryPhotoDate = this.DirectoryPhotoDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.DeliveryFee = this.DeliveryFee;						
			newObj.SearchEngineIndex = this.SearchEngineIndex;						
			newObj.DiscountValue = this.DiscountValue;						
			newObj.DiscountCode = this.DiscountCode;						
			newObj.MinimumDeliveryTime = this.MinimumDeliveryTime;						
			newObj.MinimumOrderValue = this.MinimumOrderValue;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(WebsiteLocationBase.Property_Id, Id);				
			info.AddValue(WebsiteLocationBase.Property_CompanyId, CompanyId);				
			info.AddValue(WebsiteLocationBase.Property_Name, Name);				
			info.AddValue(WebsiteLocationBase.Property_Address, Address);				
			info.AddValue(WebsiteLocationBase.Property_Address2, Address2);				
			info.AddValue(WebsiteLocationBase.Property_City, City);				
			info.AddValue(WebsiteLocationBase.Property_State, State);				
			info.AddValue(WebsiteLocationBase.Property_Zipcode, Zipcode);				
			info.AddValue(WebsiteLocationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(WebsiteLocationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(WebsiteLocationBase.Property_PrimaryContact, PrimaryContact);				
			info.AddValue(WebsiteLocationBase.Property_DomainName, DomainName);				
			info.AddValue(WebsiteLocationBase.Property_StorePhone, StorePhone);				
			info.AddValue(WebsiteLocationBase.Property_TrackingPhonePhone, TrackingPhonePhone);				
			info.AddValue(WebsiteLocationBase.Property_OrdersEmail, OrdersEmail);				
			info.AddValue(WebsiteLocationBase.Property_ThemeLoc, ThemeLoc);				
			info.AddValue(WebsiteLocationBase.Property_HoursofOperation, HoursofOperation);				
			info.AddValue(WebsiteLocationBase.Property_DaysAvailable, DaysAvailable);				
			info.AddValue(WebsiteLocationBase.Property_DaysAvailableOption, DaysAvailableOption);				
			info.AddValue(WebsiteLocationBase.Property_HoursofOperationOption, HoursofOperationOption);				
			info.AddValue(WebsiteLocationBase.Property_OperationStartTime, OperationStartTime);				
			info.AddValue(WebsiteLocationBase.Property_OperationEndTime, OperationEndTime);				
			info.AddValue(WebsiteLocationBase.Property_UrlSlug, UrlSlug);				
			info.AddValue(WebsiteLocationBase.Property_WebsiteURL, WebsiteURL);				
			info.AddValue(WebsiteLocationBase.Property_MetaTitle, MetaTitle);				
			info.AddValue(WebsiteLocationBase.Property_MetaDescription, MetaDescription);				
			info.AddValue(WebsiteLocationBase.Property_CartOption, CartOption);				
			info.AddValue(WebsiteLocationBase.Property_ImageLoc, ImageLoc);				
			info.AddValue(WebsiteLocationBase.Property_IsTax, IsTax);				
			info.AddValue(WebsiteLocationBase.Property_TaxPercentage, TaxPercentage);				
			info.AddValue(WebsiteLocationBase.Property_CuisineType, CuisineType);				
			info.AddValue(WebsiteLocationBase.Property_PreparationTime, PreparationTime);				
			info.AddValue(WebsiteLocationBase.Property_IsInstruction, IsInstruction);				
			info.AddValue(WebsiteLocationBase.Property_Notes, Notes);				
			info.AddValue(WebsiteLocationBase.Property_InstructionNotes, InstructionNotes);				
			info.AddValue(WebsiteLocationBase.Property_PaidOption, PaidOption);				
			info.AddValue(WebsiteLocationBase.Property_PaymentOption, PaymentOption);				
			info.AddValue(WebsiteLocationBase.Property_FacebookFollowURL, FacebookFollowURL);				
			info.AddValue(WebsiteLocationBase.Property_TwitterFollowURL, TwitterFollowURL);				
			info.AddValue(WebsiteLocationBase.Property_InstagramFollowURL, InstagramFollowURL);				
			info.AddValue(WebsiteLocationBase.Property_YoutubeFollowURL, YoutubeFollowURL);				
			info.AddValue(WebsiteLocationBase.Property_ReferCompanyId, ReferCompanyId);				
			info.AddValue(WebsiteLocationBase.Property_IsDefault, IsDefault);				
			info.AddValue(WebsiteLocationBase.Property_CoverImageLoc, CoverImageLoc);				
			info.AddValue(WebsiteLocationBase.Property_ExpireTime, ExpireTime);				
			info.AddValue(WebsiteLocationBase.Property_DeliveryRadius, DeliveryRadius);				
			info.AddValue(WebsiteLocationBase.Property_CoverPhotoDate, CoverPhotoDate);				
			info.AddValue(WebsiteLocationBase.Property_DirectoryPhotoDate, DirectoryPhotoDate);				
			info.AddValue(WebsiteLocationBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(WebsiteLocationBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(WebsiteLocationBase.Property_DeliveryFee, DeliveryFee);				
			info.AddValue(WebsiteLocationBase.Property_SearchEngineIndex, SearchEngineIndex);				
			info.AddValue(WebsiteLocationBase.Property_DiscountValue, DiscountValue);				
			info.AddValue(WebsiteLocationBase.Property_DiscountCode, DiscountCode);				
			info.AddValue(WebsiteLocationBase.Property_MinimumDeliveryTime, MinimumDeliveryTime);				
			info.AddValue(WebsiteLocationBase.Property_MinimumOrderValue, MinimumOrderValue);				
		}
		#endregion

		
	}
}
