﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BillDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class BillDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerBillId = 1,
			EquipmentId = 2,
			AccoutTypeId = 3,
			Dscription = 4,
			Quantity = 5,
			Rate = 6,
			Amount = 7,
			ItemName = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerBillId = "CustomerBillId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_AccoutTypeId = "AccoutTypeId";		            
		public const string Property_Dscription = "Dscription";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_Rate = "Rate";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_ItemName = "ItemName";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _CustomerBillId;	            
		private Guid _EquipmentId;	            
		private Nullable<Int32> _AccoutTypeId;	            
		private String _Dscription;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _Rate;	            
		private Nullable<Double> _Amount;	            
		private String _ItemName;	            
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
		public Int32 CustomerBillId
		{	
			get{ return _CustomerBillId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerBillId, value, _CustomerBillId);
				if (PropertyChanging(args))
				{
					_CustomerBillId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid EquipmentId
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

		[DataMember]
		public String ItemName
		{	
			get{ return _ItemName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemName, value, _ItemName);
				if (PropertyChanging(args))
				{
					_ItemName = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BillDetailBase Clone()
		{
			BillDetailBase newObj = new  BillDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerBillId = this.CustomerBillId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.AccoutTypeId = this.AccoutTypeId;						
			newObj.Dscription = this.Dscription;						
			newObj.Quantity = this.Quantity;						
			newObj.Rate = this.Rate;						
			newObj.Amount = this.Amount;						
			newObj.ItemName = this.ItemName;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BillDetailBase.Property_Id, Id);				
			info.AddValue(BillDetailBase.Property_CustomerBillId, CustomerBillId);				
			info.AddValue(BillDetailBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(BillDetailBase.Property_AccoutTypeId, AccoutTypeId);				
			info.AddValue(BillDetailBase.Property_Dscription, Dscription);				
			info.AddValue(BillDetailBase.Property_Quantity, Quantity);				
			info.AddValue(BillDetailBase.Property_Rate, Rate);				
			info.AddValue(BillDetailBase.Property_Amount, Amount);				
			info.AddValue(BillDetailBase.Property_ItemName, ItemName);				
		}
		#endregion

		
	}
}
