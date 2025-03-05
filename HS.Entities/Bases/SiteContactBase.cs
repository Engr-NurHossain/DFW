using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SiteContactBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SiteContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ContactId = 1,
			FirstName = 2,
			LastName = 3,
			CellPhone = 4,
			WorkPhone = 5,
			Email = 6,
			Title = 7,
			ContactType = 8,
			AccessLevel = 9,
			CreatedDate = 10,
			CreatedBy = 11,
			LastUpdatedBy = 12,
			LastUpdatedDate = 13,
			FileLocation = 14,
			CompanyId = 15,
			IsEmail = 16,
			IsText = 17
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ContactId = "ContactId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_CellPhone = "CellPhone";		            
		public const string Property_WorkPhone = "WorkPhone";		            
		public const string Property_Email = "Email";		            
		public const string Property_Title = "Title";		            
		public const string Property_ContactType = "ContactType";		            
		public const string Property_AccessLevel = "AccessLevel";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_FileLocation = "FileLocation";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsEmail = "IsEmail";		            
		public const string Property_IsText = "IsText";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ContactId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _CellPhone;	            
		private String _WorkPhone;	            
		private String _Email;	            
		private String _Title;	            
		private String _ContactType;	            
		private String _AccessLevel;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _FileLocation;	            
		private Guid _CompanyId;	            
		private Nullable<Boolean> _IsEmail;	            
		private Nullable<Boolean> _IsText;	            
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
		public Guid ContactId
		{	
			get{ return _ContactId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactId, value, _ContactId);
				if (PropertyChanging(args))
				{
					_ContactId = value;
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
		public String CellPhone
		{	
			get{ return _CellPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CellPhone, value, _CellPhone);
				if (PropertyChanging(args))
				{
					_CellPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WorkPhone
		{	
			get{ return _WorkPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WorkPhone, value, _WorkPhone);
				if (PropertyChanging(args))
				{
					_WorkPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Email
		{	
			get{ return _Email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Email, value, _Email);
				if (PropertyChanging(args))
				{
					_Email = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Title
		{	
			get{ return _Title; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Title, value, _Title);
				if (PropertyChanging(args))
				{
					_Title = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactType
		{	
			get{ return _ContactType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactType, value, _ContactType);
				if (PropertyChanging(args))
				{
					_ContactType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AccessLevel
		{	
			get{ return _AccessLevel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccessLevel, value, _AccessLevel);
				if (PropertyChanging(args))
				{
					_AccessLevel = value;
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
		public String FileLocation
		{	
			get{ return _FileLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileLocation, value, _FileLocation);
				if (PropertyChanging(args))
				{
					_FileLocation = value;
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
		public Nullable<Boolean> IsText
		{	
			get{ return _IsText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsText, value, _IsText);
				if (PropertyChanging(args))
				{
					_IsText = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SiteContactBase Clone()
		{
			SiteContactBase newObj = new  SiteContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ContactId = this.ContactId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.CellPhone = this.CellPhone;						
			newObj.WorkPhone = this.WorkPhone;						
			newObj.Email = this.Email;						
			newObj.Title = this.Title;						
			newObj.ContactType = this.ContactType;						
			newObj.AccessLevel = this.AccessLevel;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.FileLocation = this.FileLocation;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsEmail = this.IsEmail;						
			newObj.IsText = this.IsText;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SiteContactBase.Property_Id, Id);				
			info.AddValue(SiteContactBase.Property_ContactId, ContactId);				
			info.AddValue(SiteContactBase.Property_FirstName, FirstName);				
			info.AddValue(SiteContactBase.Property_LastName, LastName);				
			info.AddValue(SiteContactBase.Property_CellPhone, CellPhone);				
			info.AddValue(SiteContactBase.Property_WorkPhone, WorkPhone);				
			info.AddValue(SiteContactBase.Property_Email, Email);				
			info.AddValue(SiteContactBase.Property_Title, Title);				
			info.AddValue(SiteContactBase.Property_ContactType, ContactType);				
			info.AddValue(SiteContactBase.Property_AccessLevel, AccessLevel);				
			info.AddValue(SiteContactBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(SiteContactBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(SiteContactBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(SiteContactBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(SiteContactBase.Property_FileLocation, FileLocation);				
			info.AddValue(SiteContactBase.Property_CompanyId, CompanyId);				
			info.AddValue(SiteContactBase.Property_IsEmail, IsEmail);				
			info.AddValue(SiteContactBase.Property_IsText, IsText);				
		}
		#endregion

		
	}
}
