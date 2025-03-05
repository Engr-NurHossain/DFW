using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerBillBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerBillBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			BillNo = 1,
			CompanyId = 2,
			CustomerId = 3,
			Type = 4,
			Amount = 5,
			PaymentMethod = 6,
			PaymentStatus = 7,
			PaymentDate = 8,
			PaymentDueDate = 9,
			BillCycle = 10,
			Notes = 11,
			UpdatedBy = 12,
			UpdatedDate = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_BillNo = "BillNo";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_PaymentStatus = "PaymentStatus";		            
		public const string Property_PaymentDate = "PaymentDate";		            
		public const string Property_PaymentDueDate = "PaymentDueDate";		            
		public const string Property_BillCycle = "BillCycle";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_UpdatedBy = "UpdatedBy";		            
		public const string Property_UpdatedDate = "UpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _BillNo;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _Type;	            
		private Double _Amount;	            
		private String _PaymentMethod;	            
		private String _PaymentStatus;	            
		private DateTime _PaymentDate;	            
		private DateTime _PaymentDueDate;	            
		private String _BillCycle;	            
		private String _Notes;	            
		private String _UpdatedBy;	            
		private Nullable<DateTime> _UpdatedDate;	            
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
		public String BillNo
		{	
			get{ return _BillNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillNo, value, _BillNo);
				if (PropertyChanging(args))
				{
					_BillNo = value;
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
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Amount
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
		public String PaymentMethod
		{	
			get{ return _PaymentMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentMethod, value, _PaymentMethod);
				if (PropertyChanging(args))
				{
					_PaymentMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentStatus
		{	
			get{ return _PaymentStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentStatus, value, _PaymentStatus);
				if (PropertyChanging(args))
				{
					_PaymentStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime PaymentDate
		{	
			get{ return _PaymentDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentDate, value, _PaymentDate);
				if (PropertyChanging(args))
				{
					_PaymentDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime PaymentDueDate
		{	
			get{ return _PaymentDueDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentDueDate, value, _PaymentDueDate);
				if (PropertyChanging(args))
				{
					_PaymentDueDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillCycle
		{	
			get{ return _BillCycle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillCycle, value, _BillCycle);
				if (PropertyChanging(args))
				{
					_BillCycle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UpdatedBy
		{	
			get{ return _UpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedBy, value, _UpdatedBy);
				if (PropertyChanging(args))
				{
					_UpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> UpdatedDate
		{	
			get{ return _UpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedDate, value, _UpdatedDate);
				if (PropertyChanging(args))
				{
					_UpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerBillBase Clone()
		{
			CustomerBillBase newObj = new  CustomerBillBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.BillNo = this.BillNo;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Type = this.Type;						
			newObj.Amount = this.Amount;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.PaymentStatus = this.PaymentStatus;						
			newObj.PaymentDate = this.PaymentDate;						
			newObj.PaymentDueDate = this.PaymentDueDate;						
			newObj.BillCycle = this.BillCycle;						
			newObj.Notes = this.Notes;						
			newObj.UpdatedBy = this.UpdatedBy;						
			newObj.UpdatedDate = this.UpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerBillBase.Property_Id, Id);				
			info.AddValue(CustomerBillBase.Property_BillNo, BillNo);				
			info.AddValue(CustomerBillBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerBillBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerBillBase.Property_Type, Type);				
			info.AddValue(CustomerBillBase.Property_Amount, Amount);				
			info.AddValue(CustomerBillBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(CustomerBillBase.Property_PaymentStatus, PaymentStatus);				
			info.AddValue(CustomerBillBase.Property_PaymentDate, PaymentDate);				
			info.AddValue(CustomerBillBase.Property_PaymentDueDate, PaymentDueDate);				
			info.AddValue(CustomerBillBase.Property_BillCycle, BillCycle);				
			info.AddValue(CustomerBillBase.Property_Notes, Notes);				
			info.AddValue(CustomerBillBase.Property_UpdatedBy, UpdatedBy);				
			info.AddValue(CustomerBillBase.Property_UpdatedDate, UpdatedDate);				
		}
		#endregion

		
	}
}
