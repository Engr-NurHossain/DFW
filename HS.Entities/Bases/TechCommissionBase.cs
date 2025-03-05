using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TechCommissionBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TechCommissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TechCommissionId = 1,
			TicketId = 2,
			CustomerId = 3,
			UserId = 4,
			CompletionDate = 5,
			BaseRMR = 6,
			BaseRMRCommission = 7,
			AddedRMR = 8,
			AddedRMRCommission = 9,
			TotalCommission = 10,
			IsPaid = 11,
			CreatedBy = 12,
			CreatedDate = 13,
			Batch = 14,
			Adjustment = 15,
			BaseRMRCommissionCalculation = 16,
			AddedRMRCommissionCalculation = 17,
			PaidDate = 18,
			OriginalPoint = 19,
			AdjustablePoint = 20,
			TotalPoint = 21,
			IsSealed = 22
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TechCommissionId = "TechCommissionId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_CompletionDate = "CompletionDate";		            
		public const string Property_BaseRMR = "BaseRMR";		            
		public const string Property_BaseRMRCommission = "BaseRMRCommission";		            
		public const string Property_AddedRMR = "AddedRMR";		            
		public const string Property_AddedRMRCommission = "AddedRMRCommission";		            
		public const string Property_TotalCommission = "TotalCommission";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_Batch = "Batch";		            
		public const string Property_Adjustment = "Adjustment";		            
		public const string Property_BaseRMRCommissionCalculation = "BaseRMRCommissionCalculation";		            
		public const string Property_AddedRMRCommissionCalculation = "AddedRMRCommissionCalculation";		            
		public const string Property_PaidDate = "PaidDate";		            
		public const string Property_OriginalPoint = "OriginalPoint";		            
		public const string Property_AdjustablePoint = "AdjustablePoint";		            
		public const string Property_TotalPoint = "TotalPoint";		            
		public const string Property_IsSealed = "IsSealed";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TechCommissionId;	            
		private Guid _TicketId;	            
		private Guid _CustomerId;	            
		private Guid _UserId;	            
		private Nullable<DateTime> _CompletionDate;	            
		private Nullable<Double> _BaseRMR;	            
		private Nullable<Double> _BaseRMRCommission;	            
		private Nullable<Double> _AddedRMR;	            
		private Nullable<Double> _AddedRMRCommission;	            
		private Nullable<Double> _TotalCommission;	            
		private Nullable<Boolean> _IsPaid;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private String _Batch;	            
		private Nullable<Double> _Adjustment;	            
		private String _BaseRMRCommissionCalculation;	            
		private String _AddedRMRCommissionCalculation;	            
		private Nullable<DateTime> _PaidDate;	            
		private Nullable<Double> _OriginalPoint;	            
		private Nullable<Double> _AdjustablePoint;	            
		private Nullable<Double> _TotalPoint;	            
		private Nullable<Boolean> _IsSealed;	            
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
		public Guid TechCommissionId
		{	
			get{ return _TechCommissionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechCommissionId, value, _TechCommissionId);
				if (PropertyChanging(args))
				{
					_TechCommissionId = value;
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
		public Nullable<Double> BaseRMR
		{	
			get{ return _BaseRMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BaseRMR, value, _BaseRMR);
				if (PropertyChanging(args))
				{
					_BaseRMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BaseRMRCommission
		{	
			get{ return _BaseRMRCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BaseRMRCommission, value, _BaseRMRCommission);
				if (PropertyChanging(args))
				{
					_BaseRMRCommission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AddedRMR
		{	
			get{ return _AddedRMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedRMR, value, _AddedRMR);
				if (PropertyChanging(args))
				{
					_AddedRMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AddedRMRCommission
		{	
			get{ return _AddedRMRCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedRMRCommission, value, _AddedRMRCommission);
				if (PropertyChanging(args))
				{
					_AddedRMRCommission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalCommission
		{	
			get{ return _TotalCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalCommission, value, _TotalCommission);
				if (PropertyChanging(args))
				{
					_TotalCommission = value;
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
		public String BaseRMRCommissionCalculation
		{	
			get{ return _BaseRMRCommissionCalculation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BaseRMRCommissionCalculation, value, _BaseRMRCommissionCalculation);
				if (PropertyChanging(args))
				{
					_BaseRMRCommissionCalculation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddedRMRCommissionCalculation
		{	
			get{ return _AddedRMRCommissionCalculation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedRMRCommissionCalculation, value, _AddedRMRCommissionCalculation);
				if (PropertyChanging(args))
				{
					_AddedRMRCommissionCalculation = value;
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

		[DataMember]
		public Nullable<Boolean> IsSealed
		{	
			get{ return _IsSealed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSealed, value, _IsSealed);
				if (PropertyChanging(args))
				{
					_IsSealed = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TechCommissionBase Clone()
		{
			TechCommissionBase newObj = new  TechCommissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TechCommissionId = this.TechCommissionId;						
			newObj.TicketId = this.TicketId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.UserId = this.UserId;						
			newObj.CompletionDate = this.CompletionDate;						
			newObj.BaseRMR = this.BaseRMR;						
			newObj.BaseRMRCommission = this.BaseRMRCommission;						
			newObj.AddedRMR = this.AddedRMR;						
			newObj.AddedRMRCommission = this.AddedRMRCommission;						
			newObj.TotalCommission = this.TotalCommission;						
			newObj.IsPaid = this.IsPaid;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.Batch = this.Batch;						
			newObj.Adjustment = this.Adjustment;						
			newObj.BaseRMRCommissionCalculation = this.BaseRMRCommissionCalculation;						
			newObj.AddedRMRCommissionCalculation = this.AddedRMRCommissionCalculation;						
			newObj.PaidDate = this.PaidDate;						
			newObj.OriginalPoint = this.OriginalPoint;						
			newObj.AdjustablePoint = this.AdjustablePoint;						
			newObj.TotalPoint = this.TotalPoint;						
			newObj.IsSealed = this.IsSealed;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TechCommissionBase.Property_Id, Id);				
			info.AddValue(TechCommissionBase.Property_TechCommissionId, TechCommissionId);				
			info.AddValue(TechCommissionBase.Property_TicketId, TicketId);				
			info.AddValue(TechCommissionBase.Property_CustomerId, CustomerId);				
			info.AddValue(TechCommissionBase.Property_UserId, UserId);				
			info.AddValue(TechCommissionBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(TechCommissionBase.Property_BaseRMR, BaseRMR);				
			info.AddValue(TechCommissionBase.Property_BaseRMRCommission, BaseRMRCommission);				
			info.AddValue(TechCommissionBase.Property_AddedRMR, AddedRMR);				
			info.AddValue(TechCommissionBase.Property_AddedRMRCommission, AddedRMRCommission);				
			info.AddValue(TechCommissionBase.Property_TotalCommission, TotalCommission);				
			info.AddValue(TechCommissionBase.Property_IsPaid, IsPaid);				
			info.AddValue(TechCommissionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TechCommissionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TechCommissionBase.Property_Batch, Batch);				
			info.AddValue(TechCommissionBase.Property_Adjustment, Adjustment);				
			info.AddValue(TechCommissionBase.Property_BaseRMRCommissionCalculation, BaseRMRCommissionCalculation);				
			info.AddValue(TechCommissionBase.Property_AddedRMRCommissionCalculation, AddedRMRCommissionCalculation);				
			info.AddValue(TechCommissionBase.Property_PaidDate, PaidDate);				
			info.AddValue(TechCommissionBase.Property_OriginalPoint, OriginalPoint);				
			info.AddValue(TechCommissionBase.Property_AdjustablePoint, AdjustablePoint);				
			info.AddValue(TechCommissionBase.Property_TotalPoint, TotalPoint);				
			info.AddValue(TechCommissionBase.Property_IsSealed, IsSealed);				
		}
		#endregion

		
	}
}
