using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAddendumBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerAddendumBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			TicketId = 2,
			Signature = 3,
			CreatedDate = 4,
			CreatedBy = 5,
			IsSigned = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_IsSigned = "IsSigned";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _TicketId;	            
		private String _Signature;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Nullable<Boolean> _IsSigned;	            
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
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Signature
		{	
			get{ return _Signature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Signature, value, _Signature);
				if (PropertyChanging(args))
				{
					_Signature = value;
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
		public Nullable<Boolean> IsSigned
		{	
			get{ return _IsSigned; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSigned, value, _IsSigned);
				if (PropertyChanging(args))
				{
					_IsSigned = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAddendumBase Clone()
		{
			CustomerAddendumBase newObj = new  CustomerAddendumBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TicketId = this.TicketId;						
			newObj.Signature = this.Signature;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.IsSigned = this.IsSigned;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAddendumBase.Property_Id, Id);				
			info.AddValue(CustomerAddendumBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerAddendumBase.Property_TicketId, TicketId);				
			info.AddValue(CustomerAddendumBase.Property_Signature, Signature);				
			info.AddValue(CustomerAddendumBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerAddendumBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerAddendumBase.Property_IsSigned, IsSigned);				
		}
		#endregion

		
	}
}
