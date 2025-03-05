using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "HomeOwnerHistoryBase", Namespace = "http://www.hims-tech.com//entities")]
	public class HomeOwnerHistoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			HomeOwnerName = 2,
			OwnerAddress = 3,
			RequestedDate = 4,
			RequestedBy = 5,
			CompanyId = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_HomeOwnerName = "HomeOwnerName";		            
		public const string Property_OwnerAddress = "OwnerAddress";		            
		public const string Property_RequestedDate = "RequestedDate";		            
		public const string Property_RequestedBy = "RequestedBy";		            
		public const string Property_CompanyId = "CompanyId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _HomeOwnerName;	            
		private String _OwnerAddress;	            
		private Nullable<DateTime> _RequestedDate;	            
		private Guid _RequestedBy;	            
		private Guid _CompanyId;	            
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
		public String HomeOwnerName
		{	
			get{ return _HomeOwnerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeOwnerName, value, _HomeOwnerName);
				if (PropertyChanging(args))
				{
					_HomeOwnerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OwnerAddress
		{	
			get{ return _OwnerAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OwnerAddress, value, _OwnerAddress);
				if (PropertyChanging(args))
				{
					_OwnerAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> RequestedDate
		{	
			get{ return _RequestedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RequestedDate, value, _RequestedDate);
				if (PropertyChanging(args))
				{
					_RequestedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid RequestedBy
		{	
			get{ return _RequestedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RequestedBy, value, _RequestedBy);
				if (PropertyChanging(args))
				{
					_RequestedBy = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  HomeOwnerHistoryBase Clone()
		{
			HomeOwnerHistoryBase newObj = new  HomeOwnerHistoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.HomeOwnerName = this.HomeOwnerName;						
			newObj.OwnerAddress = this.OwnerAddress;						
			newObj.RequestedDate = this.RequestedDate;						
			newObj.RequestedBy = this.RequestedBy;						
			newObj.CompanyId = this.CompanyId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(HomeOwnerHistoryBase.Property_Id, Id);				
			info.AddValue(HomeOwnerHistoryBase.Property_CustomerId, CustomerId);				
			info.AddValue(HomeOwnerHistoryBase.Property_HomeOwnerName, HomeOwnerName);				
			info.AddValue(HomeOwnerHistoryBase.Property_OwnerAddress, OwnerAddress);				
			info.AddValue(HomeOwnerHistoryBase.Property_RequestedDate, RequestedDate);				
			info.AddValue(HomeOwnerHistoryBase.Property_RequestedBy, RequestedBy);				
			info.AddValue(HomeOwnerHistoryBase.Property_CompanyId, CompanyId);				
		}
		#endregion

		
	}
}
