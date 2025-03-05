using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MarchantInvoiceBase", Namespace = "http://www.piistech.com//entities")]
	public class MarchantInvoiceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			InvoiceId = 1,
			InventoryId = 2,
			MarchantId = 3,
			CompanyId = 4,
			Amount = 5,
			Tax = 6,
			DiscountCode = 7,
			DiscountAmount = 8,
			TotalAmount = 9,
			Status = 10,
			CreatedDate = 11,
			CreatedBy = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_InventoryId = "InventoryId";		            
		public const string Property_MarchantId = "MarchantId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Tax = "Tax";		            
		public const string Property_DiscountCode = "DiscountCode";		            
		public const string Property_DiscountAmount = "DiscountAmount";		            
		public const string Property_TotalAmount = "TotalAmount";		            
		public const string Property_Status = "Status";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _InvoiceId;	            
		private Guid _InventoryId;	            
		private Guid _MarchantId;	            
		private Guid _CompanyId;	            
		private Double _Amount;	            
		private Nullable<Double> _Tax;	            
		private String _DiscountCode;	            
		private Nullable<Double> _DiscountAmount;	            
		private Nullable<Double> _TotalAmount;	            
		private String _Status;	            
		private DateTime _CreatedDate;	            
		private String _CreatedBy;	            
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
		public Guid InventoryId
		{	
			get{ return _InventoryId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InventoryId, value, _InventoryId);
				if (PropertyChanging(args))
				{
					_InventoryId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid MarchantId
		{	
			get{ return _MarchantId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MarchantId, value, _MarchantId);
				if (PropertyChanging(args))
				{
					_MarchantId = value;
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
		public Nullable<Double> Tax
		{	
			get{ return _Tax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tax, value, _Tax);
				if (PropertyChanging(args))
				{
					_Tax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DiscountCode
		{	
			get{ return _DiscountCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountCode, value, _DiscountCode);
				if (PropertyChanging(args))
				{
					_DiscountCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountAmount
		{	
			get{ return _DiscountAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountAmount, value, _DiscountAmount);
				if (PropertyChanging(args))
				{
					_DiscountAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalAmount
		{	
			get{ return _TotalAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalAmount, value, _TotalAmount);
				if (PropertyChanging(args))
				{
					_TotalAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
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
		public String CreatedBy
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
		public  MarchantInvoiceBase Clone()
		{
			MarchantInvoiceBase newObj = new  MarchantInvoiceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.InventoryId = this.InventoryId;						
			newObj.MarchantId = this.MarchantId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Amount = this.Amount;						
			newObj.Tax = this.Tax;						
			newObj.DiscountCode = this.DiscountCode;						
			newObj.DiscountAmount = this.DiscountAmount;						
			newObj.TotalAmount = this.TotalAmount;						
			newObj.Status = this.Status;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MarchantInvoiceBase.Property_Id, Id);				
			info.AddValue(MarchantInvoiceBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(MarchantInvoiceBase.Property_InventoryId, InventoryId);				
			info.AddValue(MarchantInvoiceBase.Property_MarchantId, MarchantId);				
			info.AddValue(MarchantInvoiceBase.Property_CompanyId, CompanyId);				
			info.AddValue(MarchantInvoiceBase.Property_Amount, Amount);				
			info.AddValue(MarchantInvoiceBase.Property_Tax, Tax);				
			info.AddValue(MarchantInvoiceBase.Property_DiscountCode, DiscountCode);				
			info.AddValue(MarchantInvoiceBase.Property_DiscountAmount, DiscountAmount);				
			info.AddValue(MarchantInvoiceBase.Property_TotalAmount, TotalAmount);				
			info.AddValue(MarchantInvoiceBase.Property_Status, Status);				
			info.AddValue(MarchantInvoiceBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(MarchantInvoiceBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
