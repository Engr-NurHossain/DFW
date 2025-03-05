using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PaymentInfoSupplierBase", Namespace = "http://www.piistech.com//entities")]
	public class PaymentInfoSupplierBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			SupplierId = 2,
			PaymentInfoId = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SupplierId = "SupplierId";		            
		public const string Property_PaymentInfoId = "PaymentInfoId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _SupplierId;	            
		private Int32 _PaymentInfoId;	            
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
		public Int32 SupplierId
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
		public Int32 PaymentInfoId
		{	
			get{ return _PaymentInfoId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentInfoId, value, _PaymentInfoId);
				if (PropertyChanging(args))
				{
					_PaymentInfoId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PaymentInfoSupplierBase Clone()
		{
			PaymentInfoSupplierBase newObj = new  PaymentInfoSupplierBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SupplierId = this.SupplierId;						
			newObj.PaymentInfoId = this.PaymentInfoId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PaymentInfoSupplierBase.Property_Id, Id);				
			info.AddValue(PaymentInfoSupplierBase.Property_CompanyId, CompanyId);				
			info.AddValue(PaymentInfoSupplierBase.Property_SupplierId, SupplierId);				
			info.AddValue(PaymentInfoSupplierBase.Property_PaymentInfoId, PaymentInfoId);				
		}
		#endregion

		
	}
}
