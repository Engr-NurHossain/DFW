using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "WebsiteConfigurationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class WebsiteConfigurationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			SiteName = 2,
			DomainName = 3,
			Phone = 4,
			IsEmail = 5,
			ThemeLoc = 6,
			CreatedBy = 7,
			CreatedDate = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SiteName = "SiteName";		            
		public const string Property_DomainName = "DomainName";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_IsEmail = "IsEmail";		            
		public const string Property_ThemeLoc = "ThemeLoc";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _SiteName;	            
		private String _DomainName;	            
		private String _Phone;	            
		private Nullable<Boolean> _IsEmail;	            
		private String _ThemeLoc;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public String Phone
		{	
			get{ return _Phone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone, value, _Phone);
				if (PropertyChanging(args))
				{
					_Phone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEmail
		{	
			get{ return _IsEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEmail, value, _IsEmail);
				if (PropertyChanging(args))
				{
					_IsEmail = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  WebsiteConfigurationBase Clone()
		{
			WebsiteConfigurationBase newObj = new  WebsiteConfigurationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SiteName = this.SiteName;						
			newObj.DomainName = this.DomainName;						
			newObj.Phone = this.Phone;						
			newObj.IsEmail = this.IsEmail;						
			newObj.ThemeLoc = this.ThemeLoc;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(WebsiteConfigurationBase.Property_Id, Id);				
			info.AddValue(WebsiteConfigurationBase.Property_CompanyId, CompanyId);				
			info.AddValue(WebsiteConfigurationBase.Property_SiteName, SiteName);				
			info.AddValue(WebsiteConfigurationBase.Property_DomainName, DomainName);				
			info.AddValue(WebsiteConfigurationBase.Property_Phone, Phone);				
			info.AddValue(WebsiteConfigurationBase.Property_IsEmail, IsEmail);				
			info.AddValue(WebsiteConfigurationBase.Property_ThemeLoc, ThemeLoc);				
			info.AddValue(WebsiteConfigurationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(WebsiteConfigurationBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
