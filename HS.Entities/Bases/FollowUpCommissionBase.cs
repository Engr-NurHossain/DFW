using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "FollowUpCommissionBase", Namespace = "http://www.piistech.com//entities")]
	public class FollowUpCommissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FollowUpCommissionId = 1,
			TicketId = 2,
			CustomerId = 3,
			UserId = 4,
			CompletionDate = 5,
			Adjustment = 6,
			Commission = 7,
			IsPaid = 8,
			CreatedBy = 9,
			CreatedDate = 10,
			Batch = 11,
			CommissionCalculation = 12,
			PaidDate = 13,
			IsManual = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FollowUpCommissionId = "FollowUpCommissionId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_CompletionDate = "CompletionDate";		            
		public const string Property_Adjustment = "Adjustment";		            
		public const string Property_Commission = "Commission";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_Batch = "Batch";		            
		public const string Property_CommissionCalculation = "CommissionCalculation";		            
		public const string Property_PaidDate = "PaidDate";		            
		public const string Property_IsManual = "IsManual";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _FollowUpCommissionId;	            
		private Guid _TicketId;	            
		private Guid _CustomerId;	            
		private Guid _UserId;	            
		private Nullable<DateTime> _CompletionDate;	            
		private Nullable<Double> _Adjustment;	            
		private Nullable<Double> _Commission;	            
		private Nullable<Boolean> _IsPaid;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private String _Batch;	            
		private String _CommissionCalculation;	            
		private Nullable<DateTime> _PaidDate;	            
		private Nullable<Boolean> _IsManual;	            
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
		public Guid FollowUpCommissionId
		{	
			get{ return _FollowUpCommissionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FollowUpCommissionId, value, _FollowUpCommissionId);
				if (PropertyChanging(args))
				{
					_FollowUpCommissionId = value;
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CompletionDate
		{	
			get{ return _CompletionDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompletionDate, value, _CompletionDate);
				if (PropertyChanging(args))
				{
					_CompletionDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Adjustment
		{	
			get{ return _Adjustment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Adjustment, value, _Adjustment);
				if (PropertyChanging(args))
				{
					_Adjustment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Commission
		{	
			get{ return _Commission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Commission, value, _Commission);
				if (PropertyChanging(args))
				{
					_Commission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPaid
		{	
			get{ return _IsPaid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPaid, value, _IsPaid);
				if (PropertyChanging(args))
				{
					_IsPaid = value;
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
		public String Batch
		{	
			get{ return _Batch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Batch, value, _Batch);
				if (PropertyChanging(args))
				{
					_Batch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CommissionCalculation
		{	
			get{ return _CommissionCalculation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommissionCalculation, value, _CommissionCalculation);
				if (PropertyChanging(args))
				{
					_CommissionCalculation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PaidDate
		{	
			get{ return _PaidDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaidDate, value, _PaidDate);
				if (PropertyChanging(args))
				{
					_PaidDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsManual
		{	
			get{ return _IsManual; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsManual, value, _IsManual);
				if (PropertyChanging(args))
				{
					_IsManual = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  FollowUpCommissionBase Clone()
		{
			FollowUpCommissionBase newObj = new  FollowUpCommissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FollowUpCommissionId = this.FollowUpCommissionId;						
			newObj.TicketId = this.TicketId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.UserId = this.UserId;						
			newObj.CompletionDate = this.CompletionDate;						
			newObj.Adjustment = this.Adjustment;						
			newObj.Commission = this.Commission;						
			newObj.IsPaid = this.IsPaid;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.Batch = this.Batch;						
			newObj.CommissionCalculation = this.CommissionCalculation;						
			newObj.PaidDate = this.PaidDate;						
			newObj.IsManual = this.IsManual;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(FollowUpCommissionBase.Property_Id, Id);				
			info.AddValue(FollowUpCommissionBase.Property_FollowUpCommissionId, FollowUpCommissionId);				
			info.AddValue(FollowUpCommissionBase.Property_TicketId, TicketId);				
			info.AddValue(FollowUpCommissionBase.Property_CustomerId, CustomerId);				
			info.AddValue(FollowUpCommissionBase.Property_UserId, UserId);				
			info.AddValue(FollowUpCommissionBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(FollowUpCommissionBase.Property_Adjustment, Adjustment);				
			info.AddValue(FollowUpCommissionBase.Property_Commission, Commission);				
			info.AddValue(FollowUpCommissionBase.Property_IsPaid, IsPaid);				
			info.AddValue(FollowUpCommissionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(FollowUpCommissionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(FollowUpCommissionBase.Property_Batch, Batch);				
			info.AddValue(FollowUpCommissionBase.Property_CommissionCalculation, CommissionCalculation);				
			info.AddValue(FollowUpCommissionBase.Property_PaidDate, PaidDate);				
			info.AddValue(FollowUpCommissionBase.Property_IsManual, IsManual);				
		}
		#endregion

		
	}
}
