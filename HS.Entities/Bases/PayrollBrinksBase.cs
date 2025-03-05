using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollBrinksBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollBrinksBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollBrinksId = 1,
			CustomerId = 2,
			SalesPersonId = 3,
			TicketId = 4,
			MMR = 5,
			Multiple = 6,
			GrossPay = 7,
			Deductions = 8,
			Adjustments = 9,
			NetPay = 10,
			CreatedBy = 11,
			CreatedDate = 12,
			LastUpdateBy = 13,
			LastUpdateDate = 14,
			HoldBack = 15,
			PassThrus = 16,
			FundingStatus = 17,
			IsPaid = 18,
			BatchNo = 19,
			IsManagerPayroll = 20
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollBrinksId = "PayrollBrinksId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_SalesPersonId = "SalesPersonId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_MMR = "MMR";		            
		public const string Property_Multiple = "Multiple";		            
		public const string Property_GrossPay = "GrossPay";		            
		public const string Property_Deductions = "Deductions";		            
		public const string Property_Adjustments = "Adjustments";		            
		public const string Property_NetPay = "NetPay";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_HoldBack = "HoldBack";		            
		public const string Property_PassThrus = "PassThrus";		            
		public const string Property_FundingStatus = "FundingStatus";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_BatchNo = "BatchNo";		            
		public const string Property_IsManagerPayroll = "IsManagerPayroll";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollBrinksId;	            
		private Guid _CustomerId;	            
		private Guid _SalesPersonId;	            
		private Guid _TicketId;	            
		private Nullable<Double> _MMR;	            
		private Nullable<Double> _Multiple;	            
		private Nullable<Double> _GrossPay;	            
		private Nullable<Double> _Deductions;	            
		private Nullable<Double> _Adjustments;	            
		private Nullable<Double> _NetPay;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _LastUpdateBy;	            
		private Nullable<DateTime> _LastUpdateDate;	            
		private Nullable<Double> _HoldBack;	            
		private Nullable<Double> _PassThrus;	            
		private String _FundingStatus;	            
		private Nullable<Boolean> _IsPaid;	            
		private Nullable<Int32> _BatchNo;	            
		private Nullable<Boolean> _IsManagerPayroll;	            
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
		public Guid PayrollBrinksId
		{	
			get{ return _PayrollBrinksId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollBrinksId, value, _PayrollBrinksId);
				if (PropertyChanging(args))
				{
					_PayrollBrinksId = value;
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
		public Guid SalesPersonId
		{	
			get{ return _SalesPersonId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesPersonId, value, _SalesPersonId);
				if (PropertyChanging(args))
				{
					_SalesPersonId = value;
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
		public Nullable<Double> MMR
		{	
			get{ return _MMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MMR, value, _MMR);
				if (PropertyChanging(args))
				{
					_MMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Multiple
		{	
			get{ return _Multiple; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Multiple, value, _Multiple);
				if (PropertyChanging(args))
				{
					_Multiple = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> GrossPay
		{	
			get{ return _GrossPay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GrossPay, value, _GrossPay);
				if (PropertyChanging(args))
				{
					_GrossPay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Deductions
		{	
			get{ return _Deductions; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Deductions, value, _Deductions);
				if (PropertyChanging(args))
				{
					_Deductions = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Adjustments
		{	
			get{ return _Adjustments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Adjustments, value, _Adjustments);
				if (PropertyChanging(args))
				{
					_Adjustments = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> NetPay
		{	
			get{ return _NetPay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NetPay, value, _NetPay);
				if (PropertyChanging(args))
				{
					_NetPay = value;
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
		public Guid LastUpdateBy
		{	
			get{ return _LastUpdateBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateBy, value, _LastUpdateBy);
				if (PropertyChanging(args))
				{
					_LastUpdateBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastUpdateDate
		{	
			get{ return _LastUpdateDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateDate, value, _LastUpdateDate);
				if (PropertyChanging(args))
				{
					_LastUpdateDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> HoldBack
		{	
			get{ return _HoldBack; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HoldBack, value, _HoldBack);
				if (PropertyChanging(args))
				{
					_HoldBack = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PassThrus
		{	
			get{ return _PassThrus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PassThrus, value, _PassThrus);
				if (PropertyChanging(args))
				{
					_PassThrus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FundingStatus
		{	
			get{ return _FundingStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FundingStatus, value, _FundingStatus);
				if (PropertyChanging(args))
				{
					_FundingStatus = value;
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
		public Nullable<Int32> BatchNo
		{	
			get{ return _BatchNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BatchNo, value, _BatchNo);
				if (PropertyChanging(args))
				{
					_BatchNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsManagerPayroll
		{	
			get{ return _IsManagerPayroll; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsManagerPayroll, value, _IsManagerPayroll);
				if (PropertyChanging(args))
				{
					_IsManagerPayroll = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PayrollBrinksBase Clone()
		{
			PayrollBrinksBase newObj = new  PayrollBrinksBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollBrinksId = this.PayrollBrinksId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.SalesPersonId = this.SalesPersonId;						
			newObj.TicketId = this.TicketId;						
			newObj.MMR = this.MMR;						
			newObj.Multiple = this.Multiple;						
			newObj.GrossPay = this.GrossPay;						
			newObj.Deductions = this.Deductions;						
			newObj.Adjustments = this.Adjustments;						
			newObj.NetPay = this.NetPay;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdateDate = this.LastUpdateDate;						
			newObj.HoldBack = this.HoldBack;						
			newObj.PassThrus = this.PassThrus;						
			newObj.FundingStatus = this.FundingStatus;						
			newObj.IsPaid = this.IsPaid;						
			newObj.BatchNo = this.BatchNo;						
			newObj.IsManagerPayroll = this.IsManagerPayroll;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollBrinksBase.Property_Id, Id);				
			info.AddValue(PayrollBrinksBase.Property_PayrollBrinksId, PayrollBrinksId);				
			info.AddValue(PayrollBrinksBase.Property_CustomerId, CustomerId);				
			info.AddValue(PayrollBrinksBase.Property_SalesPersonId, SalesPersonId);				
			info.AddValue(PayrollBrinksBase.Property_TicketId, TicketId);				
			info.AddValue(PayrollBrinksBase.Property_MMR, MMR);				
			info.AddValue(PayrollBrinksBase.Property_Multiple, Multiple);				
			info.AddValue(PayrollBrinksBase.Property_GrossPay, GrossPay);				
			info.AddValue(PayrollBrinksBase.Property_Deductions, Deductions);				
			info.AddValue(PayrollBrinksBase.Property_Adjustments, Adjustments);				
			info.AddValue(PayrollBrinksBase.Property_NetPay, NetPay);				
			info.AddValue(PayrollBrinksBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollBrinksBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollBrinksBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollBrinksBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollBrinksBase.Property_HoldBack, HoldBack);				
			info.AddValue(PayrollBrinksBase.Property_PassThrus, PassThrus);				
			info.AddValue(PayrollBrinksBase.Property_FundingStatus, FundingStatus);				
			info.AddValue(PayrollBrinksBase.Property_IsPaid, IsPaid);				
			info.AddValue(PayrollBrinksBase.Property_BatchNo, BatchNo);				
			info.AddValue(PayrollBrinksBase.Property_IsManagerPayroll, IsManagerPayroll);				
		}
		#endregion

		
	}
}
