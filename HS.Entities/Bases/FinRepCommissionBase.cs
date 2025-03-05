using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "FinRepCommissionBase", Namespace = "http://www.hims-tech.com//entities")]
	public class FinRepCommissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FinRepCommissionId = 1,
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
			IsManual = 14,
			OriginalPoint = 15,
			AdjustablePoint = 16,
			TotalPoint = 17
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FinRepCommissionId = "FinRepCommissionId";		            
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
		public const string Property_OriginalPoint = "OriginalPoint";		            
		public const string Property_AdjustablePoint = "AdjustablePoint";		            
		public const string Property_TotalPoint = "TotalPoint";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _FinRepCommissionId;	            
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
		private Nullable<Double> _OriginalPoint;	            
		private Nullable<Double> _AdjustablePoint;	            
		private Nullable<Double> _TotalPoint;	            
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
		public Guid FinRepCommissionId
		{	
			get{ return _FinRepCommissionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinRepCommissionId, value, _FinRepCommissionId);
				if (PropertyChanging(args))
				{
					_FinRepCommissionId = value;
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

		[DataMember]
		public Nullable<Double> OriginalPoint
		{	
			get{ return _OriginalPoint; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OriginalPoint, value, _OriginalPoint);
				if (PropertyChanging(args))
				{
					_OriginalPoint = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AdjustablePoint
		{	
			get{ return _AdjustablePoint; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustablePoint, value, _AdjustablePoint);
				if (PropertyChanging(args))
				{
					_AdjustablePoint = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalPoint
		{	
			get{ return _TotalPoint; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalPoint, value, _TotalPoint);
				if (PropertyChanging(args))
				{
					_TotalPoint = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  FinRepCommissionBase Clone()
		{
			FinRepCommissionBase newObj = new  FinRepCommissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FinRepCommissionId = this.FinRepCommissionId;						
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
			newObj.OriginalPoint = this.OriginalPoint;						
			newObj.AdjustablePoint = this.AdjustablePoint;						
			newObj.TotalPoint = this.TotalPoint;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(FinRepCommissionBase.Property_Id, Id);				
			info.AddValue(FinRepCommissionBase.Property_FinRepCommissionId, FinRepCommissionId);				
			info.AddValue(FinRepCommissionBase.Property_TicketId, TicketId);				
			info.AddValue(FinRepCommissionBase.Property_CustomerId, CustomerId);				
			info.AddValue(FinRepCommissionBase.Property_UserId, UserId);				
			info.AddValue(FinRepCommissionBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(FinRepCommissionBase.Property_Adjustment, Adjustment);				
			info.AddValue(FinRepCommissionBase.Property_Commission, Commission);				
			info.AddValue(FinRepCommissionBase.Property_IsPaid, IsPaid);				
			info.AddValue(FinRepCommissionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(FinRepCommissionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(FinRepCommissionBase.Property_Batch, Batch);				
			info.AddValue(FinRepCommissionBase.Property_CommissionCalculation, CommissionCalculation);				
			info.AddValue(FinRepCommissionBase.Property_PaidDate, PaidDate);				
			info.AddValue(FinRepCommissionBase.Property_IsManual, IsManual);				
			info.AddValue(FinRepCommissionBase.Property_OriginalPoint, OriginalPoint);				
			info.AddValue(FinRepCommissionBase.Property_AdjustablePoint, AdjustablePoint);				
			info.AddValue(FinRepCommissionBase.Property_TotalPoint, TotalPoint);				
		}
		#endregion

		
	}
}
