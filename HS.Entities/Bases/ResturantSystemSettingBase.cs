using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ResturantSystemSettingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ResturantSystemSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Restaurant = 2,
			Logo = 3,
			TaxRate = 4,
			PrimaryContact = 5,
			CreatedDate = 6,
			CreatedBy = 7,
			AuthApiLoginKey = 8,
			AuthApiTransactionKey = 9,
			MinimumOrderValue = 10,
			AutoConfirmOrder = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Restaurant = "Restaurant";		            
		public const string Property_Logo = "Logo";		            
		public const string Property_TaxRate = "TaxRate";		            
		public const string Property_PrimaryContact = "PrimaryContact";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_AuthApiLoginKey = "AuthApiLoginKey";		            
		public const string Property_AuthApiTransactionKey = "AuthApiTransactionKey";		            
		public const string Property_MinimumOrderValue = "MinimumOrderValue";		            
		public const string Property_AutoConfirmOrder = "AutoConfirmOrder";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Restaurant;	            
		private String _Logo;	            
		private Nullable<Double> _TaxRate;	            
		private String _PrimaryContact;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private String _AuthApiLoginKey;	            
		private String _AuthApiTransactionKey;	            
		private Nullable<Double> _MinimumOrderValue;	            
		private Nullable<Boolean> _AutoConfirmOrder;	            
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
		public String Restaurant
		{	
			get{ return _Restaurant; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Restaurant, value, _Restaurant);
				if (PropertyChanging(args))
				{
					_Restaurant = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Logo
		{	
			get{ return _Logo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Logo, value, _Logo);
				if (PropertyChanging(args))
				{
					_Logo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxRate
		{	
			get{ return _TaxRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxRate, value, _TaxRate);
				if (PropertyChanging(args))
				{
					_TaxRate = value;
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
		public String AuthApiLoginKey
		{	
			get{ return _AuthApiLoginKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthApiLoginKey, value, _AuthApiLoginKey);
				if (PropertyChanging(args))
				{
					_AuthApiLoginKey = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthApiTransactionKey
		{	
			get{ return _AuthApiTransactionKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthApiTransactionKey, value, _AuthApiTransactionKey);
				if (PropertyChanging(args))
				{
					_AuthApiTransactionKey = value;
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

		[DataMember]
		public Nullable<Boolean> AutoConfirmOrder
		{	
			get{ return _AutoConfirmOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AutoConfirmOrder, value, _AutoConfirmOrder);
				if (PropertyChanging(args))
				{
					_AutoConfirmOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ResturantSystemSettingBase Clone()
		{
			ResturantSystemSettingBase newObj = new  ResturantSystemSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Restaurant = this.Restaurant;						
			newObj.Logo = this.Logo;						
			newObj.TaxRate = this.TaxRate;						
			newObj.PrimaryContact = this.PrimaryContact;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.AuthApiLoginKey = this.AuthApiLoginKey;						
			newObj.AuthApiTransactionKey = this.AuthApiTransactionKey;						
			newObj.MinimumOrderValue = this.MinimumOrderValue;						
			newObj.AutoConfirmOrder = this.AutoConfirmOrder;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ResturantSystemSettingBase.Property_Id, Id);				
			info.AddValue(ResturantSystemSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(ResturantSystemSettingBase.Property_Restaurant, Restaurant);				
			info.AddValue(ResturantSystemSettingBase.Property_Logo, Logo);				
			info.AddValue(ResturantSystemSettingBase.Property_TaxRate, TaxRate);				
			info.AddValue(ResturantSystemSettingBase.Property_PrimaryContact, PrimaryContact);				
			info.AddValue(ResturantSystemSettingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ResturantSystemSettingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ResturantSystemSettingBase.Property_AuthApiLoginKey, AuthApiLoginKey);				
			info.AddValue(ResturantSystemSettingBase.Property_AuthApiTransactionKey, AuthApiTransactionKey);				
			info.AddValue(ResturantSystemSettingBase.Property_MinimumOrderValue, MinimumOrderValue);				
			info.AddValue(ResturantSystemSettingBase.Property_AutoConfirmOrder, AutoConfirmOrder);				
		}
		#endregion

		
	}
}
