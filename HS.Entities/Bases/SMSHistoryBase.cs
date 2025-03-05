using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SMSHistoryBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SMSHistoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TemplateKey = 1,
			ToMobileNo = 2,
			FromMobileNo = 3,
			FromName = 4,
			SMSBodyContent = 5,
			SMSSentDate = 6,
			IsSystemAutoSent = 7,
			IsRead = 8,
			ReadCount = 9,
			LastUpdatedDate = 10,
			CompanyId = 11,
			CreatedBy = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TemplateKey = "TemplateKey";		            
		public const string Property_ToMobileNo = "ToMobileNo";		            
		public const string Property_FromMobileNo = "FromMobileNo";		            
		public const string Property_FromName = "FromName";		            
		public const string Property_SMSBodyContent = "SMSBodyContent";		            
		public const string Property_SMSSentDate = "SMSSentDate";		            
		public const string Property_IsSystemAutoSent = "IsSystemAutoSent";		            
		public const string Property_IsRead = "IsRead";		            
		public const string Property_ReadCount = "ReadCount";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _TemplateKey;	            
		private String _ToMobileNo;	            
		private String _FromMobileNo;	            
		private String _FromName;	            
		private String _SMSBodyContent;	            
		private Nullable<DateTime> _SMSSentDate;	            
		private Nullable<Boolean> _IsSystemAutoSent;	            
		private Nullable<Boolean> _IsRead;	            
		private Nullable<Int32> _ReadCount;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private Guid _CompanyId;	            
		private Guid _CreatedBy;	            
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
		public String SMSBodyContent
		{	
			get{ return _SMSBodyContent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SMSBodyContent, value, _SMSBodyContent);
				if (PropertyChanging(args))
				{
					_SMSBodyContent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SMSSentDate
		{	
			get{ return _SMSSentDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SMSSentDate, value, _SMSSentDate);
				if (PropertyChanging(args))
				{
					_SMSSentDate = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  SMSHistoryBase Clone()
		{
			SMSHistoryBase newObj = new  SMSHistoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TemplateKey = this.TemplateKey;						
			newObj.ToMobileNo = this.ToMobileNo;						
			newObj.FromMobileNo = this.FromMobileNo;						
			newObj.FromName = this.FromName;						
			newObj.SMSBodyContent = this.SMSBodyContent;						
			newObj.SMSSentDate = this.SMSSentDate;						
			newObj.IsSystemAutoSent = this.IsSystemAutoSent;						
			newObj.IsRead = this.IsRead;						
			newObj.ReadCount = this.ReadCount;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SMSHistoryBase.Property_Id, Id);				
			info.AddValue(SMSHistoryBase.Property_TemplateKey, TemplateKey);				
			info.AddValue(SMSHistoryBase.Property_ToMobileNo, ToMobileNo);				
			info.AddValue(SMSHistoryBase.Property_FromMobileNo, FromMobileNo);				
			info.AddValue(SMSHistoryBase.Property_FromName, FromName);				
			info.AddValue(SMSHistoryBase.Property_SMSBodyContent, SMSBodyContent);				
			info.AddValue(SMSHistoryBase.Property_SMSSentDate, SMSSentDate);				
			info.AddValue(SMSHistoryBase.Property_IsSystemAutoSent, IsSystemAutoSent);				
			info.AddValue(SMSHistoryBase.Property_IsRead, IsRead);				
			info.AddValue(SMSHistoryBase.Property_ReadCount, ReadCount);				
			info.AddValue(SMSHistoryBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(SMSHistoryBase.Property_CompanyId, CompanyId);				
			info.AddValue(SMSHistoryBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
