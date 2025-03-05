using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ContactBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ContactId = 1,
			FirstName = 2,
			LastName = 3,
			Suffix = 4,
			Title = 5,
			Work = 6,
			Ext = 7,
			Mobile = 8,
			Email = 9,
			Role = 10,
			Facebook = 11,
			Twitter = 12,
			Instagram = 13,
			LinkedIN = 14,
			Notes = 15,
			ContactOwner = 16,
			CreatedBy = 17,
			CreatedDate = 18,
			ContactType = 19
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ContactId = "ContactId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_Suffix = "Suffix";		            
		public const string Property_Title = "Title";		            
		public const string Property_Work = "Work";		            
		public const string Property_Ext = "Ext";		            
		public const string Property_Mobile = "Mobile";		            
		public const string Property_Email = "Email";		            
		public const string Property_Role = "Role";		            
		public const string Property_Facebook = "Facebook";		            
		public const string Property_Twitter = "Twitter";		            
		public const string Property_Instagram = "Instagram";		            
		public const string Property_LinkedIN = "LinkedIN";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_ContactOwner = "ContactOwner";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_ContactType = "ContactType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ContactId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _Suffix;	            
		private String _Title;	            
		private String _Work;	            
		private String _Ext;	            
		private String _Mobile;	            
		private String _Email;	            
		private String _Role;	            
		private String _Facebook;	            
		private String _Twitter;	            
		private String _Instagram;	            
		private String _LinkedIN;	            
		private String _Notes;	            
		private Guid _ContactOwner;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private String _ContactType;	            
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
		public String Suffix
		{	
			get{ return _Suffix; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Suffix, value, _Suffix);
				if (PropertyChanging(args))
				{
					_Suffix = value;
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
		public String Work
		{	
			get{ return _Work; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Work, value, _Work);
				if (PropertyChanging(args))
				{
					_Work = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Ext
		{	
			get{ return _Ext; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Ext, value, _Ext);
				if (PropertyChanging(args))
				{
					_Ext = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Mobile
		{	
			get{ return _Mobile; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Mobile, value, _Mobile);
				if (PropertyChanging(args))
				{
					_Mobile = value;
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
		public String Role
		{	
			get{ return _Role; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Role, value, _Role);
				if (PropertyChanging(args))
				{
					_Role = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Facebook
		{	
			get{ return _Facebook; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Facebook, value, _Facebook);
				if (PropertyChanging(args))
				{
					_Facebook = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Twitter
		{	
			get{ return _Twitter; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Twitter, value, _Twitter);
				if (PropertyChanging(args))
				{
					_Twitter = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Instagram
		{	
			get{ return _Instagram; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Instagram, value, _Instagram);
				if (PropertyChanging(args))
				{
					_Instagram = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LinkedIN
		{	
			get{ return _LinkedIN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LinkedIN, value, _LinkedIN);
				if (PropertyChanging(args))
				{
					_LinkedIN = value;
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
		public Guid ContactOwner
		{	
			get{ return _ContactOwner; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactOwner, value, _ContactOwner);
				if (PropertyChanging(args))
				{
					_ContactOwner = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  ContactBase Clone()
		{
			ContactBase newObj = new  ContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ContactId = this.ContactId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.Suffix = this.Suffix;						
			newObj.Title = this.Title;						
			newObj.Work = this.Work;						
			newObj.Ext = this.Ext;						
			newObj.Mobile = this.Mobile;						
			newObj.Email = this.Email;						
			newObj.Role = this.Role;						
			newObj.Facebook = this.Facebook;						
			newObj.Twitter = this.Twitter;						
			newObj.Instagram = this.Instagram;						
			newObj.LinkedIN = this.LinkedIN;						
			newObj.Notes = this.Notes;						
			newObj.ContactOwner = this.ContactOwner;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.ContactType = this.ContactType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ContactBase.Property_Id, Id);				
			info.AddValue(ContactBase.Property_ContactId, ContactId);				
			info.AddValue(ContactBase.Property_FirstName, FirstName);				
			info.AddValue(ContactBase.Property_LastName, LastName);				
			info.AddValue(ContactBase.Property_Suffix, Suffix);				
			info.AddValue(ContactBase.Property_Title, Title);				
			info.AddValue(ContactBase.Property_Work, Work);				
			info.AddValue(ContactBase.Property_Ext, Ext);				
			info.AddValue(ContactBase.Property_Mobile, Mobile);				
			info.AddValue(ContactBase.Property_Email, Email);				
			info.AddValue(ContactBase.Property_Role, Role);				
			info.AddValue(ContactBase.Property_Facebook, Facebook);				
			info.AddValue(ContactBase.Property_Twitter, Twitter);				
			info.AddValue(ContactBase.Property_Instagram, Instagram);				
			info.AddValue(ContactBase.Property_LinkedIN, LinkedIN);				
			info.AddValue(ContactBase.Property_Notes, Notes);				
			info.AddValue(ContactBase.Property_ContactOwner, ContactOwner);				
			info.AddValue(ContactBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ContactBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ContactBase.Property_ContactType, ContactType);				
		}
		#endregion

		
	}
}
