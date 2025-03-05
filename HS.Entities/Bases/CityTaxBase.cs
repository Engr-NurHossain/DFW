using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CityTaxBase", Namespace = "http://www.piistech.com//entities")]
	public class CityTaxBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			City = 2,
			Country = 3,
			State = 4,
			ZipCode = 5,
			Rate = 6,
			IsActive = 7,
			TaxText = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_City = "City";		            
		public const string Property_Country = "Country";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Rate = "Rate";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_TaxText = "TaxText";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _City;	            
		private String _Country;	            
		private String _State;	            
		private String _ZipCode;	            
		private Double _Rate;	            
		private Boolean _IsActive;	            
		private String _TaxText;	            
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
		public Double Rate
		{	
			get{ return _Rate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Rate, value, _Rate);
				if (PropertyChanging(args))
				{
					_Rate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TaxText
		{	
			get{ return _TaxText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxText, value, _TaxText);
				if (PropertyChanging(args))
				{
					_TaxText = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CityTaxBase Clone()
		{
			CityTaxBase newObj = new  CityTaxBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.City = this.City;						
			newObj.Country = this.Country;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Rate = this.Rate;						
			newObj.IsActive = this.IsActive;						
			newObj.TaxText = this.TaxText;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CityTaxBase.Property_Id, Id);				
			info.AddValue(CityTaxBase.Property_CompanyId, CompanyId);				
			info.AddValue(CityTaxBase.Property_City, City);				
			info.AddValue(CityTaxBase.Property_Country, Country);				
			info.AddValue(CityTaxBase.Property_State, State);				
			info.AddValue(CityTaxBase.Property_ZipCode, ZipCode);				
			info.AddValue(CityTaxBase.Property_Rate, Rate);				
			info.AddValue(CityTaxBase.Property_IsActive, IsActive);				
			info.AddValue(CityTaxBase.Property_TaxText, TaxText);				
		}
		#endregion

		
	}
}
