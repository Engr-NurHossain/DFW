using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "WebsiteLocationOperationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class WebsiteLocationOperationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SiteLocationId = 1,
			HoursofOperation = 2,
			OperationStartTime = 3,
			OperationEndTime = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			CompanyId = 7,
			StoreOperationStartTime = 8,
			StoreOperationEndTime = 9,
			IsAdditional = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SiteLocationId = "SiteLocationId";		            
		public const string Property_HoursofOperation = "HoursofOperation";		            
		public const string Property_OperationStartTime = "OperationStartTime";		            
		public const string Property_OperationEndTime = "OperationEndTime";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_StoreOperationStartTime = "StoreOperationStartTime";		            
		public const string Property_StoreOperationEndTime = "StoreOperationEndTime";		            
		public const string Property_IsAdditional = "IsAdditional";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _SiteLocationId;	            
		private String _HoursofOperation;	            
		private String _OperationStartTime;	            
		private String _OperationEndTime;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _CompanyId;	            
		private String _StoreOperationStartTime;	            
		private String _StoreOperationEndTime;	            
		private Nullable<Boolean> _IsAdditional;	            
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
		public Int32 SiteLocationId
		{	
			get{ return _SiteLocationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SiteLocationId, value, _SiteLocationId);
				if (PropertyChanging(args))
				{
					_SiteLocationId = value;
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
		public String StoreOperationStartTime
		{	
			get{ return _StoreOperationStartTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StoreOperationStartTime, value, _StoreOperationStartTime);
				if (PropertyChanging(args))
				{
					_StoreOperationStartTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StoreOperationEndTime
		{	
			get{ return _StoreOperationEndTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StoreOperationEndTime, value, _StoreOperationEndTime);
				if (PropertyChanging(args))
				{
					_StoreOperationEndTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAdditional
		{	
			get{ return _IsAdditional; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAdditional, value, _IsAdditional);
				if (PropertyChanging(args))
				{
					_IsAdditional = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  WebsiteLocationOperationBase Clone()
		{
			WebsiteLocationOperationBase newObj = new  WebsiteLocationOperationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SiteLocationId = this.SiteLocationId;						
			newObj.HoursofOperation = this.HoursofOperation;						
			newObj.OperationStartTime = this.OperationStartTime;						
			newObj.OperationEndTime = this.OperationEndTime;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.StoreOperationStartTime = this.StoreOperationStartTime;						
			newObj.StoreOperationEndTime = this.StoreOperationEndTime;						
			newObj.IsAdditional = this.IsAdditional;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(WebsiteLocationOperationBase.Property_Id, Id);				
			info.AddValue(WebsiteLocationOperationBase.Property_SiteLocationId, SiteLocationId);				
			info.AddValue(WebsiteLocationOperationBase.Property_HoursofOperation, HoursofOperation);				
			info.AddValue(WebsiteLocationOperationBase.Property_OperationStartTime, OperationStartTime);				
			info.AddValue(WebsiteLocationOperationBase.Property_OperationEndTime, OperationEndTime);				
			info.AddValue(WebsiteLocationOperationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(WebsiteLocationOperationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(WebsiteLocationOperationBase.Property_CompanyId, CompanyId);				
			info.AddValue(WebsiteLocationOperationBase.Property_StoreOperationStartTime, StoreOperationStartTime);				
			info.AddValue(WebsiteLocationOperationBase.Property_StoreOperationEndTime, StoreOperationEndTime);				
			info.AddValue(WebsiteLocationOperationBase.Property_IsAdditional, IsAdditional);				
		}
		#endregion

		
	}
}
