using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BillBase", Namespace = "http://www.piistech.com//entities")]
	public class BillBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			BillNo = 1,
			CompanyId = 2,
			SupplierId = 3,
			Type = 4,
			Amount = 5,
			PaymentMethod = 6,
			PaymentStatus = 7,
			PaymentDate = 8,
			PaymentDueDate = 9,
			BillCycle = 10,
			Notes = 11,
			UpdatedBy = 12,
			UpdatedDate = 13,
			PaymentDue = 14,
			SupplierAddress = 15,
			BillFor = 16,
			EmployeeId = 17,
			InvoiceId = 18,
			PurchaseOrderId = 19,
			PaymentTerm = 20,
			JobName = 21
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_BillNo = "BillNo";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SupplierId = "SupplierId";		            
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
		public const string Property_PaymentDue = "PaymentDue";		            
		public const string Property_SupplierAddress = "SupplierAddress";		            
		public const string Property_BillFor = "BillFor";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_PaymentTerm = "PaymentTerm";		            
		public const string Property_JobName = "JobName";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _BillNo;	            
		private Guid _CompanyId;	            
		private Nullable<Int32> _SupplierId;	            
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
		private Nullable<Double> _PaymentDue;	            
		private String _SupplierAddress;	            
		private String _BillFor;	            
		private Guid _EmployeeId;	            
		private String _InvoiceId;	            
		private String _PurchaseOrderId;	            
		private String _PaymentTerm;	            
		private String _JobName;	            
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
		public Nullable<Int32> SupplierId
		{	
			get{ return _SupplierId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierId, value, _SupplierId);
				if (PropertyChanging(args))
				{
					_SupplierId = value;
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

		[DataMember]
		public Nullable<Double> PaymentDue
		{	
			get{ return _PaymentDue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentDue, value, _PaymentDue);
				if (PropertyChanging(args))
				{
					_PaymentDue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SupplierAddress
		{	
			get{ return _SupplierAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierAddress, value, _SupplierAddress);
				if (PropertyChanging(args))
				{
					_SupplierAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillFor
		{	
			get{ return _BillFor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillFor, value, _BillFor);
				if (PropertyChanging(args))
				{
					_BillFor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid EmployeeId
		{	
			get{ return _EmployeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeId, value, _EmployeeId);
				if (PropertyChanging(args))
				{
					_EmployeeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceId
		{	
			get{ return _InvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceId, value, _InvoiceId);
				if (PropertyChanging(args))
				{
					_InvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PurchaseOrderId
		{	
			get{ return _PurchaseOrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PurchaseOrderId, value, _PurchaseOrderId);
				if (PropertyChanging(args))
				{
					_PurchaseOrderId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentTerm
		{	
			get{ return _PaymentTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentTerm, value, _PaymentTerm);
				if (PropertyChanging(args))
				{
					_PaymentTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobName
		{	
			get{ return _JobName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobName, value, _JobName);
				if (PropertyChanging(args))
				{
					_JobName = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BillBase Clone()
		{
			BillBase newObj = new  BillBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.BillNo = this.BillNo;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SupplierId = this.SupplierId;						
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
			newObj.PaymentDue = this.PaymentDue;						
			newObj.SupplierAddress = this.SupplierAddress;						
			newObj.BillFor = this.BillFor;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.PaymentTerm = this.PaymentTerm;						
			newObj.JobName = this.JobName;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BillBase.Property_Id, Id);				
			info.AddValue(BillBase.Property_BillNo, BillNo);				
			info.AddValue(BillBase.Property_CompanyId, CompanyId);				
			info.AddValue(BillBase.Property_SupplierId, SupplierId);				
			info.AddValue(BillBase.Property_Type, Type);				
			info.AddValue(BillBase.Property_Amount, Amount);				
			info.AddValue(BillBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(BillBase.Property_PaymentStatus, PaymentStatus);				
			info.AddValue(BillBase.Property_PaymentDate, PaymentDate);				
			info.AddValue(BillBase.Property_PaymentDueDate, PaymentDueDate);				
			info.AddValue(BillBase.Property_BillCycle, BillCycle);				
			info.AddValue(BillBase.Property_Notes, Notes);				
			info.AddValue(BillBase.Property_UpdatedBy, UpdatedBy);				
			info.AddValue(BillBase.Property_UpdatedDate, UpdatedDate);				
			info.AddValue(BillBase.Property_PaymentDue, PaymentDue);				
			info.AddValue(BillBase.Property_SupplierAddress, SupplierAddress);				
			info.AddValue(BillBase.Property_BillFor, BillFor);				
			info.AddValue(BillBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(BillBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(BillBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(BillBase.Property_PaymentTerm, PaymentTerm);				
			info.AddValue(BillBase.Property_JobName, JobName);				
		}
		#endregion

		
	}
}
