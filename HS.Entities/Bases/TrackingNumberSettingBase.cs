using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TrackingNumberSettingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TrackingNumberSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			TrackingId = 2,
			TrackingNumber = 3,
			IsActive = 4,
			IsRecorded = 5,
			IsPrompt = 6,
			Comments = 7,
			CreatedBy = 8,
			LastUpdatedBy = 9,
			CreatedDate = 10,
			LastUpdatedDate = 11,
			ForwardingNumber = 12,
			SubAccountId = 13,
			SubAccountToken = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TrackingId = "TrackingId";		            
		public const string Property_TrackingNumber = "TrackingNumber";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_IsRecorded = "IsRecorded";		            
		public const string Property_IsPrompt = "IsPrompt";		            
		public const string Property_Comments = "Comments";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_ForwardingNumber = "ForwardingNumber";		            
		public const string Property_SubAccountId = "SubAccountId";		            
		public const string Property_SubAccountToken = "SubAccountToken";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _TrackingId;	            
		private String _TrackingNumber;	            
		private Nullable<Boolean> _IsActive;	            
		private Nullable<Boolean> _IsRecorded;	            
		private Nullable<Boolean> _IsPrompt;	            
		private String _Comments;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _CreatedDate;	            
		private DateTime _LastUpdatedDate;	            
		private String _ForwardingNumber;	            
		private String _SubAccountId;	            
		private String _SubAccountToken;	            
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
		public Guid TrackingId
		{	
			get{ return _TrackingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TrackingId, value, _TrackingId);
				if (PropertyChanging(args))
				{
					_TrackingId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TrackingNumber
		{	
			get{ return _TrackingNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TrackingNumber, value, _TrackingNumber);
				if (PropertyChanging(args))
				{
					_TrackingNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
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
		public Nullable<Boolean> IsRecorded
		{	
			get{ return _IsRecorded; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRecorded, value, _IsRecorded);
				if (PropertyChanging(args))
				{
					_IsRecorded = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPrompt
		{	
			get{ return _IsPrompt; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPrompt, value, _IsPrompt);
				if (PropertyChanging(args))
				{
					_IsPrompt = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Comments
		{	
			get{ return _Comments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Comments, value, _Comments);
				if (PropertyChanging(args))
				{
					_Comments = value;
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
		public String ForwardingNumber
		{	
			get{ return _ForwardingNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ForwardingNumber, value, _ForwardingNumber);
				if (PropertyChanging(args))
				{
					_ForwardingNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SubAccountId
		{	
			get{ return _SubAccountId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SubAccountId, value, _SubAccountId);
				if (PropertyChanging(args))
				{
					_SubAccountId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SubAccountToken
		{	
			get{ return _SubAccountToken; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SubAccountToken, value, _SubAccountToken);
				if (PropertyChanging(args))
				{
					_SubAccountToken = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TrackingNumberSettingBase Clone()
		{
			TrackingNumberSettingBase newObj = new  TrackingNumberSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TrackingId = this.TrackingId;						
			newObj.TrackingNumber = this.TrackingNumber;						
			newObj.IsActive = this.IsActive;						
			newObj.IsRecorded = this.IsRecorded;						
			newObj.IsPrompt = this.IsPrompt;						
			newObj.Comments = this.Comments;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.ForwardingNumber = this.ForwardingNumber;						
			newObj.SubAccountId = this.SubAccountId;						
			newObj.SubAccountToken = this.SubAccountToken;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TrackingNumberSettingBase.Property_Id, Id);				
			info.AddValue(TrackingNumberSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(TrackingNumberSettingBase.Property_TrackingId, TrackingId);				
			info.AddValue(TrackingNumberSettingBase.Property_TrackingNumber, TrackingNumber);				
			info.AddValue(TrackingNumberSettingBase.Property_IsActive, IsActive);				
			info.AddValue(TrackingNumberSettingBase.Property_IsRecorded, IsRecorded);				
			info.AddValue(TrackingNumberSettingBase.Property_IsPrompt, IsPrompt);				
			info.AddValue(TrackingNumberSettingBase.Property_Comments, Comments);				
			info.AddValue(TrackingNumberSettingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TrackingNumberSettingBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(TrackingNumberSettingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TrackingNumberSettingBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(TrackingNumberSettingBase.Property_ForwardingNumber, ForwardingNumber);				
			info.AddValue(TrackingNumberSettingBase.Property_SubAccountId, SubAccountId);				
			info.AddValue(TrackingNumberSettingBase.Property_SubAccountToken, SubAccountToken);				
		}
		#endregion

		
	}
}
