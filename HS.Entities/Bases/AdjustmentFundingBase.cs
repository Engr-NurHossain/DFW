using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AdjustmentFundingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class AdjustmentFundingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AdjustmentId = 1,
			UserId = 2,
			Reason = 3,
			Amount = 4,
			Date = 5,
			IsPaid = 6,
			Batch = 7,
			PaidDate = 8,
			CreatedDate = 9,
			CreatedBy = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AdjustmentId = "AdjustmentId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_Reason = "Reason";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Date = "Date";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_Batch = "Batch";		            
		public const string Property_PaidDate = "PaidDate";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _AdjustmentId;	            
		private Guid _UserId;	            
		private String _Reason;	            
		private Nullable<Double> _Amount;	            
		private Nullable<DateTime> _Date;	            
		private Nullable<Boolean> _IsPaid;	            
		private Nullable<Int32> _Batch;	            
		private Nullable<DateTime> _PaidDate;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public Guid AdjustmentId
		{	
			get{ return _AdjustmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdjustmentId, value, _AdjustmentId);
				if (PropertyChanging(args))
				{
					_AdjustmentId = value;
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
		public String Reason
		{	
			get{ return _Reason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reason, value, _Reason);
				if (PropertyChanging(args))
				{
					_Reason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Date
		{	
			get{ return _Date; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Date, value, _Date);
				if (PropertyChanging(args))
				{
					_Date = value;
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
		public Nullable<Int32> Batch
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
		public Nullable<DateTime> CreatedDate
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

		#endregion
		
		#region Cloning Base Objects
		public  AdjustmentFundingBase Clone()
		{
			AdjustmentFundingBase newObj = new  AdjustmentFundingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AdjustmentId = this.AdjustmentId;						
			newObj.UserId = this.UserId;						
			newObj.Reason = this.Reason;						
			newObj.Amount = this.Amount;						
			newObj.Date = this.Date;						
			newObj.IsPaid = this.IsPaid;						
			newObj.Batch = this.Batch;						
			newObj.PaidDate = this.PaidDate;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AdjustmentFundingBase.Property_Id, Id);				
			info.AddValue(AdjustmentFundingBase.Property_AdjustmentId, AdjustmentId);				
			info.AddValue(AdjustmentFundingBase.Property_UserId, UserId);				
			info.AddValue(AdjustmentFundingBase.Property_Reason, Reason);				
			info.AddValue(AdjustmentFundingBase.Property_Amount, Amount);				
			info.AddValue(AdjustmentFundingBase.Property_Date, Date);				
			info.AddValue(AdjustmentFundingBase.Property_IsPaid, IsPaid);				
			info.AddValue(AdjustmentFundingBase.Property_Batch, Batch);				
			info.AddValue(AdjustmentFundingBase.Property_PaidDate, PaidDate);				
			info.AddValue(AdjustmentFundingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(AdjustmentFundingBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
