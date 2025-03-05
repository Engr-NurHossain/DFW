using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SocialMediaContentBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SocialMediaContentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			FollowUpLink = 3,
			ShareLink = 4,
			ImageLink = 5,
			CreatedDate = 6,
			CreatedBy = 7,
			LastUpdatedBy = 8,
			LastUpdatedDate = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_FollowUpLink = "FollowUpLink";		            
		public const string Property_ShareLink = "ShareLink";		            
		public const string Property_ImageLink = "ImageLink";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _FollowUpLink;	            
		private String _ShareLink;	            
		private String _ImageLink;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public String FollowUpLink
		{	
			get{ return _FollowUpLink; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FollowUpLink, value, _FollowUpLink);
				if (PropertyChanging(args))
				{
					_FollowUpLink = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ShareLink
		{	
			get{ return _ShareLink; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShareLink, value, _ShareLink);
				if (PropertyChanging(args))
				{
					_ShareLink = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ImageLink
		{	
			get{ return _ImageLink; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ImageLink, value, _ImageLink);
				if (PropertyChanging(args))
				{
					_ImageLink = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  SocialMediaContentBase Clone()
		{
			SocialMediaContentBase newObj = new  SocialMediaContentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.FollowUpLink = this.FollowUpLink;						
			newObj.ShareLink = this.ShareLink;						
			newObj.ImageLink = this.ImageLink;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SocialMediaContentBase.Property_Id, Id);				
			info.AddValue(SocialMediaContentBase.Property_CompanyId, CompanyId);				
			info.AddValue(SocialMediaContentBase.Property_Name, Name);				
			info.AddValue(SocialMediaContentBase.Property_FollowUpLink, FollowUpLink);				
			info.AddValue(SocialMediaContentBase.Property_ShareLink, ShareLink);				
			info.AddValue(SocialMediaContentBase.Property_ImageLink, ImageLink);				
			info.AddValue(SocialMediaContentBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(SocialMediaContentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(SocialMediaContentBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(SocialMediaContentBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
