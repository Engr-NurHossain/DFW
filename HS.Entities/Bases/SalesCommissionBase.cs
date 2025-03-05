using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SalesCommissionBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SalesCommissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SalesCommissionId = 1,
			TicketId = 2,
			CustomerId = 3,
			UserId = 4,
			CompletionDate = 5,
			RMRSold = 6,
			RMRCommission = 7,
			NoOfEquipment = 8,
			EquipmentCommission = 9,
			TotalCommission = 10,
			IsPaid = 11,
			CreatedBy = 12,
			CreatedDate = 13,
			Batch = 14,
			Adjustment = 15,
			RMRCommissionCalculation = 16,
			EquipmentCommissionCalculation = 17,
			PaidDate = 18,
			IsPermanent = 19,
			OriginalPoint = 20,
			AdjustablePoint = 21,
			TotalPoint = 22,
			IsSealed = 23
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SalesCommissionId = "SalesCommissionId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_CompletionDate = "CompletionDate";		            
		public const string Property_RMRSold = "RMRSold";		            
		public const string Property_RMRCommission = "RMRCommission";		            
		public const string Property_NoOfEquipment = "NoOfEquipment";		            
		public const string Property_EquipmentCommission = "EquipmentCommission";		            
		public const string Property_TotalCommission = "TotalCommission";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_Batch = "Batch";		            
		public const string Property_Adjustment = "Adjustment";		            
		public const string Property_RMRCommissionCalculation = "RMRCommissionCalculation";		            
		public const string Property_EquipmentCommissionCalculation = "EquipmentCommissionCalculation";		            
		public const string Property_PaidDate = "PaidDate";		            
		public const string Property_IsPermanent = "IsPermanent";		            
		public const string Property_OriginalPoint = "OriginalPoint";		            
		public const string Property_AdjustablePoint = "AdjustablePoint";		            
		public const string Property_TotalPoint = "TotalPoint";		            
		public const string Property_IsSealed = "IsSealed";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SalesCommissionId;	            
		private Guid _TicketId;	            
		private Guid _CustomerId;	            
		private Guid _UserId;	            
		private Nullable<DateTime> _CompletionDate;	            
		private Nullable<Double> _RMRSold;	            
		private Nullable<Double> _RMRCommission;	            
		private Nullable<Int32> _NoOfEquipment;	            
		private Nullable<Double> _EquipmentCommission;	            
		private Nullable<Double> _TotalCommission;	            
		private Nullable<Boolean> _IsPaid;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private String _Batch;	            
		private Nullable<Double> _Adjustment;	            
		private String _RMRCommissionCalculation;	            
		private String _EquipmentCommissionCalculation;	            
		private Nullable<DateTime> _PaidDate;	            
		private Nullable<Boolean> _IsPermanent;	            
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
		public Guid SalesCommissionId
		{	
			get{ return _SalesCommissionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesCommissionId, value, _SalesCommissionId);
				if (PropertyChanging(args))
				{
					_SalesCommissionId = value;
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
		public Nullable<Double> RMRSold
		{	
			get{ return _RMRSold; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RMRSold, value, _RMRSold);
				if (PropertyChanging(args))
				{
					_RMRSold = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RMRCommission
		{	
			get{ return _RMRCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RMRCommission, value, _RMRCommission);
				if (PropertyChanging(args))
				{
					_RMRCommission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> NoOfEquipment
		{	
			get{ return _NoOfEquipment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoOfEquipment, value, _NoOfEquipment);
				if (PropertyChanging(args))
				{
					_NoOfEquipment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> EquipmentCommission
		{	
			get{ return _EquipmentCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentCommission, value, _EquipmentCommission);
				if (PropertyChanging(args))
				{
					_EquipmentCommission = value;
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
		public String RMRCommissionCalculation
		{	
			get{ return _RMRCommissionCalculation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RMRCommissionCalculation, value, _RMRCommissionCalculation);
				if (PropertyChanging(args))
				{
					_RMRCommissionCalculation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipmentCommissionCalculation
		{	
			get{ return _EquipmentCommissionCalculation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentCommissionCalculation, value, _EquipmentCommissionCalculation);
				if (PropertyChanging(args))
				{
					_EquipmentCommissionCalculation = value;
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
		public Nullable<Boolean> IsPermanent
		{	
			get{ return _IsPermanent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPermanent, value, _IsPermanent);
				if (PropertyChanging(args))
				{
					_IsPermanent = value;
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
		public  SalesCommissionBase Clone()
		{
			SalesCommissionBase newObj = new  SalesCommissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SalesCommissionId = this.SalesCommissionId;						
			newObj.TicketId = this.TicketId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.UserId = this.UserId;						
			newObj.CompletionDate = this.CompletionDate;						
			newObj.RMRSold = this.RMRSold;						
			newObj.RMRCommission = this.RMRCommission;						
			newObj.NoOfEquipment = this.NoOfEquipment;						
			newObj.EquipmentCommission = this.EquipmentCommission;						
			newObj.TotalCommission = this.TotalCommission;						
			newObj.IsPaid = this.IsPaid;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.Batch = this.Batch;						
			newObj.Adjustment = this.Adjustment;						
			newObj.RMRCommissionCalculation = this.RMRCommissionCalculation;						
			newObj.EquipmentCommissionCalculation = this.EquipmentCommissionCalculation;						
			newObj.PaidDate = this.PaidDate;						
			newObj.IsPermanent = this.IsPermanent;						
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
			info.AddValue(SalesCommissionBase.Property_Id, Id);				
			info.AddValue(SalesCommissionBase.Property_SalesCommissionId, SalesCommissionId);				
			info.AddValue(SalesCommissionBase.Property_TicketId, TicketId);				
			info.AddValue(SalesCommissionBase.Property_CustomerId, CustomerId);				
			info.AddValue(SalesCommissionBase.Property_UserId, UserId);				
			info.AddValue(SalesCommissionBase.Property_CompletionDate, CompletionDate);				
			info.AddValue(SalesCommissionBase.Property_RMRSold, RMRSold);				
			info.AddValue(SalesCommissionBase.Property_RMRCommission, RMRCommission);				
			info.AddValue(SalesCommissionBase.Property_NoOfEquipment, NoOfEquipment);				
			info.AddValue(SalesCommissionBase.Property_EquipmentCommission, EquipmentCommission);				
			info.AddValue(SalesCommissionBase.Property_TotalCommission, TotalCommission);				
			info.AddValue(SalesCommissionBase.Property_IsPaid, IsPaid);				
			info.AddValue(SalesCommissionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(SalesCommissionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(SalesCommissionBase.Property_Batch, Batch);				
			info.AddValue(SalesCommissionBase.Property_Adjustment, Adjustment);				
			info.AddValue(SalesCommissionBase.Property_RMRCommissionCalculation, RMRCommissionCalculation);				
			info.AddValue(SalesCommissionBase.Property_EquipmentCommissionCalculation, EquipmentCommissionCalculation);				
			info.AddValue(SalesCommissionBase.Property_PaidDate, PaidDate);				
			info.AddValue(SalesCommissionBase.Property_IsPermanent, IsPermanent);				
			info.AddValue(SalesCommissionBase.Property_OriginalPoint, OriginalPoint);				
			info.AddValue(SalesCommissionBase.Property_AdjustablePoint, AdjustablePoint);				
			info.AddValue(SalesCommissionBase.Property_TotalPoint, TotalPoint);				
			info.AddValue(SalesCommissionBase.Property_IsSealed, IsSealed);				
		}
		#endregion

		
	}
}
