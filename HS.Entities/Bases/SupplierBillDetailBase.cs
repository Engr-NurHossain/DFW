using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SupplierBillDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class SupplierBillDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SupplierBillId = 1,
			EquipmentId = 2,
			AccoutTypeId = 3,
			Dscription = 4,
			Quantity = 5,
			Rate = 6,
			Amount = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SupplierBillId = "SupplierBillId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_AccoutTypeId = "AccoutTypeId";		            
		public const string Property_Dscription = "Dscription";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_Rate = "Rate";		            
		public const string Property_Amount = "Amount";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _SupplierBillId;	            
		private Nullable<Int32> _EquipmentId;	            
		private Nullable<Int32> _AccoutTypeId;	            
		private String _Dscription;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _Rate;	            
		private Nullable<Double> _Amount;	            
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
		public Int32 SupplierBillId
		{	
			get{ return _SupplierBillId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierBillId, value, _SupplierBillId);
				if (PropertyChanging(args))
				{
					_SupplierBillId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AccoutTypeId
		{	
			get{ return _AccoutTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccoutTypeId, value, _AccoutTypeId);
				if (PropertyChanging(args))
				{
					_AccoutTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Dscription
		{	
			get{ return _Dscription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Dscription, value, _Dscription);
				if (PropertyChanging(args))
				{
					_Dscription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Quantity
		{	
			get{ return _Quantity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Quantity, value, _Quantity);
				if (PropertyChanging(args))
				{
					_Quantity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Rate
		{	
			get{ return _Rate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Rate, value, _Rate);
				if (PropertyChanging(args))
				{
					_Rate = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  SupplierBillDetailBase Clone()
		{
			SupplierBillDetailBase newObj = new  SupplierBillDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SupplierBillId = this.SupplierBillId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.AccoutTypeId = this.AccoutTypeId;						
			newObj.Dscription = this.Dscription;						
			newObj.Quantity = this.Quantity;						
			newObj.Rate = this.Rate;						
			newObj.Amount = this.Amount;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SupplierBillDetailBase.Property_Id, Id);				
			info.AddValue(SupplierBillDetailBase.Property_SupplierBillId, SupplierBillId);				
			info.AddValue(SupplierBillDetailBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(SupplierBillDetailBase.Property_AccoutTypeId, AccoutTypeId);				
			info.AddValue(SupplierBillDetailBase.Property_Dscription, Dscription);				
			info.AddValue(SupplierBillDetailBase.Property_Quantity, Quantity);				
			info.AddValue(SupplierBillDetailBase.Property_Rate, Rate);				
			info.AddValue(SupplierBillDetailBase.Property_Amount, Amount);				
		}
		#endregion

		
	}
}
