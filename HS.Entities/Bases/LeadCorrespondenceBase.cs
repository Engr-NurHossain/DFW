using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "LeadCorrespondenceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class LeadCorrespondenceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			TemplateKey = 3,
			Type = 4,
			ToEmail = 5,
			CcEmail = 6,
			BccEmail = 7,
			FromEmail = 8,
			ToMobileNo = 9,
			FromMobileNo = 10,
			FromName = 11,
			Subject = 12,
			BodyContent = 13,
			SentDate = 14,
			IsSystemAutoSent = 15,
			IsRead = 16,
			ReadCount = 17,
			LastUpdatedDate = 18,
			SentBy = 19,
			FileId = 20
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TemplateKey = "TemplateKey";		            
		public const string Property_Type = "Type";		            
		public const string Property_ToEmail = "ToEmail";		            
		public const string Property_CcEmail = "CcEmail";		            
		public const string Property_BccEmail = "BccEmail";		            
		public const string Property_FromEmail = "FromEmail";		            
		public const string Property_ToMobileNo = "ToMobileNo";		            
		public const string Property_FromMobileNo = "FromMobileNo";		            
		public const string Property_FromName = "FromName";		            
		public const string Property_Subject = "Subject";		            
		public const string Property_BodyContent = "BodyContent";		            
		public const string Property_SentDate = "SentDate";		            
		public const string Property_IsSystemAutoSent = "IsSystemAutoSent";		            
		public const string Property_IsRead = "IsRead";		            
		public const string Property_ReadCount = "ReadCount";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_SentBy = "SentBy";		            
		public const string Property_FileId = "FileId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _TemplateKey;	            
		private String _Type;	            
		private String _ToEmail;	            
		private String _CcEmail;	            
		private String _BccEmail;	            
		private String _FromEmail;	            
		private String _ToMobileNo;	            
		private String _FromMobileNo;	            
		private String _FromName;	            
		private String _Subject;	            
		private String _BodyContent;	            
		private Nullable<DateTime> _SentDate;	            
		private Nullable<Boolean> _IsSystemAutoSent;	            
		private Nullable<Boolean> _IsRead;	            
		private Nullable<Int32> _ReadCount;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private Guid _SentBy;	            
		private Nullable<Int32> _FileId;	            
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
		public String TemplateKey
		{	
			get{ return _TemplateKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TemplateKey, value, _TemplateKey);
				if (PropertyChanging(args))
				{
					_TemplateKey = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ToEmail
		{	
			get{ return _ToEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToEmail, value, _ToEmail);
				if (PropertyChanging(args))
				{
					_ToEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CcEmail
		{	
			get{ return _CcEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CcEmail, value, _CcEmail);
				if (PropertyChanging(args))
				{
					_CcEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BccEmail
		{	
			get{ return _BccEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BccEmail, value, _BccEmail);
				if (PropertyChanging(args))
				{
					_BccEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FromEmail
		{	
			get{ return _FromEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FromEmail, value, _FromEmail);
				if (PropertyChanging(args))
				{
					_FromEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ToMobileNo
		{	
			get{ return _ToMobileNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToMobileNo, value, _ToMobileNo);
				if (PropertyChanging(args))
				{
					_ToMobileNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FromMobileNo
		{	
			get{ return _FromMobileNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FromMobileNo, value, _FromMobileNo);
				if (PropertyChanging(args))
				{
					_FromMobileNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FromName
		{	
			get{ return _FromName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FromName, value, _FromName);
				if (PropertyChanging(args))
				{
					_FromName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Subject
		{	
			get{ return _Subject; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Subject, value, _Subject);
				if (PropertyChanging(args))
				{
					_Subject = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BodyContent
		{	
			get{ return _BodyContent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BodyContent, value, _BodyContent);
				if (PropertyChanging(args))
				{
					_BodyContent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SentDate
		{	
			get{ return _SentDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SentDate, value, _SentDate);
				if (PropertyChanging(args))
				{
					_SentDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSystemAutoSent
		{	
			get{ return _IsSystemAutoSent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSystemAutoSent, value, _IsSystemAutoSent);
				if (PropertyChanging(args))
				{
					_IsSystemAutoSent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsRead
		{	
			get{ return _IsRead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRead, value, _IsRead);
				if (PropertyChanging(args))
				{
					_IsRead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ReadCount
		{	
			get{ return _ReadCount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReadCount, value, _ReadCount);
				if (PropertyChanging(args))
				{
					_ReadCount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastUpdatedDate
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
		public Guid SentBy
		{	
			get{ return _SentBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SentBy, value, _SentBy);
				if (PropertyChanging(args))
				{
					_SentBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> FileId
		{	
			get{ return _FileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileId, value, _FileId);
				if (PropertyChanging(args))
				{
					_FileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  LeadCorrespondenceBase Clone()
		{
			LeadCorrespondenceBase newObj = new  LeadCorrespondenceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TemplateKey = this.TemplateKey;						
			newObj.Type = this.Type;						
			newObj.ToEmail = this.ToEmail;						
			newObj.CcEmail = this.CcEmail;						
			newObj.BccEmail = this.BccEmail;						
			newObj.FromEmail = this.FromEmail;						
			newObj.ToMobileNo = this.ToMobileNo;						
			newObj.FromMobileNo = this.FromMobileNo;						
			newObj.FromName = this.FromName;						
			newObj.Subject = this.Subject;						
			newObj.BodyContent = this.BodyContent;						
			newObj.SentDate = this.SentDate;						
			newObj.IsSystemAutoSent = this.IsSystemAutoSent;						
			newObj.IsRead = this.IsRead;						
			newObj.ReadCount = this.ReadCount;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.SentBy = this.SentBy;						
			newObj.FileId = this.FileId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(LeadCorrespondenceBase.Property_Id, Id);				
			info.AddValue(LeadCorrespondenceBase.Property_CompanyId, CompanyId);				
			info.AddValue(LeadCorrespondenceBase.Property_CustomerId, CustomerId);				
			info.AddValue(LeadCorrespondenceBase.Property_TemplateKey, TemplateKey);				
			info.AddValue(LeadCorrespondenceBase.Property_Type, Type);				
			info.AddValue(LeadCorrespondenceBase.Property_ToEmail, ToEmail);				
			info.AddValue(LeadCorrespondenceBase.Property_CcEmail, CcEmail);				
			info.AddValue(LeadCorrespondenceBase.Property_BccEmail, BccEmail);				
			info.AddValue(LeadCorrespondenceBase.Property_FromEmail, FromEmail);				
			info.AddValue(LeadCorrespondenceBase.Property_ToMobileNo, ToMobileNo);				
			info.AddValue(LeadCorrespondenceBase.Property_FromMobileNo, FromMobileNo);				
			info.AddValue(LeadCorrespondenceBase.Property_FromName, FromName);				
			info.AddValue(LeadCorrespondenceBase.Property_Subject, Subject);				
			info.AddValue(LeadCorrespondenceBase.Property_BodyContent, BodyContent);				
			info.AddValue(LeadCorrespondenceBase.Property_SentDate, SentDate);				
			info.AddValue(LeadCorrespondenceBase.Property_IsSystemAutoSent, IsSystemAutoSent);				
			info.AddValue(LeadCorrespondenceBase.Property_IsRead, IsRead);				
			info.AddValue(LeadCorrespondenceBase.Property_ReadCount, ReadCount);				
			info.AddValue(LeadCorrespondenceBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(LeadCorrespondenceBase.Property_SentBy, SentBy);				
			info.AddValue(LeadCorrespondenceBase.Property_FileId, FileId);				
		}
		#endregion

		
	}
}
