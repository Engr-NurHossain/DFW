using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerSystemNoDraftBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerSystemNoDraftBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerNo = 2,
			IsUsed = 3,
			IsReserved = 4,
			GenerateDate = 5,
			ReserveDate = 6,
			UsedDate = 7,
			CustomerId = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerNo = "CustomerNo";		            
		public const string Property_IsUsed = "IsUsed";		            
		public const string Property_IsReserved = "IsReserved";		            
		public const string Property_GenerateDate = "GenerateDate";		            
		public const string Property_ReserveDate = "ReserveDate";		            
		public const string Property_UsedDate = "UsedDate";		            
		public const string Property_CustomerId = "CustomerId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _CustomerNo;	            
		private Boolean _IsUsed;	            
		private Boolean _IsReserved;	            
		private DateTime _GenerateDate;	            
		private Nullable<DateTime> _ReserveDate;	            
		private Nullable<DateTime> _UsedDate;	            
		private Nullable<Int32> _CustomerId;	            
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
		public String CustomerNo
		{	
			get{ return _CustomerNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerNo, value, _CustomerNo);
				if (PropertyChanging(args))
				{
					_CustomerNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsUsed
		{	
			get{ return _IsUsed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUsed, value, _IsUsed);
				if (PropertyChanging(args))
				{
					_IsUsed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsReserved
		{	
			get{ return _IsReserved; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReserved, value, _IsReserved);
				if (PropertyChanging(args))
				{
					_IsReserved = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime GenerateDate
		{	
			get{ return _GenerateDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GenerateDate, value, _GenerateDate);
				if (PropertyChanging(args))
				{
					_GenerateDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReserveDate
		{	
			get{ return _ReserveDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReserveDate, value, _ReserveDate);
				if (PropertyChanging(args))
				{
					_ReserveDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> UsedDate
		{	
			get{ return _UsedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UsedDate, value, _UsedDate);
				if (PropertyChanging(args))
				{
					_UsedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CustomerId
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerSystemNoDraftBase Clone()
		{
			CustomerSystemNoDraftBase newObj = new  CustomerSystemNoDraftBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerNo = this.CustomerNo;						
			newObj.IsUsed = this.IsUsed;						
			newObj.IsReserved = this.IsReserved;						
			newObj.GenerateDate = this.GenerateDate;						
			newObj.ReserveDate = this.ReserveDate;						
			newObj.UsedDate = this.UsedDate;						
			newObj.CustomerId = this.CustomerId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerSystemNoDraftBase.Property_Id, Id);				
			info.AddValue(CustomerSystemNoDraftBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerSystemNoDraftBase.Property_CustomerNo, CustomerNo);				
			info.AddValue(CustomerSystemNoDraftBase.Property_IsUsed, IsUsed);				
			info.AddValue(CustomerSystemNoDraftBase.Property_IsReserved, IsReserved);				
			info.AddValue(CustomerSystemNoDraftBase.Property_GenerateDate, GenerateDate);				
			info.AddValue(CustomerSystemNoDraftBase.Property_ReserveDate, ReserveDate);				
			info.AddValue(CustomerSystemNoDraftBase.Property_UsedDate, UsedDate);				
			info.AddValue(CustomerSystemNoDraftBase.Property_CustomerId, CustomerId);				
		}
		#endregion

		
	}
}
