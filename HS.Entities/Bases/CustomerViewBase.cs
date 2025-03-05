using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerViewBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerViewBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			LastVistited = 3,
			LastVisitedBy = 4,
			LastVisitedByUId = 5,
			IsLead = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_LastVistited = "LastVistited";		            
		public const string Property_LastVisitedBy = "LastVisitedBy";		            
		public const string Property_LastVisitedByUId = "LastVisitedByUId";		            
		public const string Property_IsLead = "IsLead";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private DateTime _LastVistited;	            
		private String _LastVisitedBy;	            
		private Guid _LastVisitedByUId;	            
		private Nullable<Boolean> _IsLead;	            
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
		public DateTime LastVistited
		{	
			get{ return _LastVistited; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastVistited, value, _LastVistited);
				if (PropertyChanging(args))
				{
					_LastVistited = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastVisitedBy
		{	
			get{ return _LastVisitedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastVisitedBy, value, _LastVisitedBy);
				if (PropertyChanging(args))
				{
					_LastVisitedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastVisitedByUId
		{	
			get{ return _LastVisitedByUId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastVisitedByUId, value, _LastVisitedByUId);
				if (PropertyChanging(args))
				{
					_LastVisitedByUId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsLead
		{	
			get{ return _IsLead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsLead, value, _IsLead);
				if (PropertyChanging(args))
				{
					_IsLead = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerViewBase Clone()
		{
			CustomerViewBase newObj = new  CustomerViewBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.LastVistited = this.LastVistited;						
			newObj.LastVisitedBy = this.LastVisitedBy;						
			newObj.LastVisitedByUId = this.LastVisitedByUId;						
			newObj.IsLead = this.IsLead;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerViewBase.Property_Id, Id);				
			info.AddValue(CustomerViewBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerViewBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerViewBase.Property_LastVistited, LastVistited);				
			info.AddValue(CustomerViewBase.Property_LastVisitedBy, LastVisitedBy);				
			info.AddValue(CustomerViewBase.Property_LastVisitedByUId, LastVisitedByUId);				
			info.AddValue(CustomerViewBase.Property_IsLead, IsLead);				
		}
		#endregion

		
	}
}
