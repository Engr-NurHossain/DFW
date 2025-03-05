using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ResturantOrderInstructionToppingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ResturantOrderInstructionToppingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			OrderId = 1,
			ItemId = 2,
			SpecialInstruction = 3,
			CreatedDate = 4,
			CreatedBy = 5,
			Toppings = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_OrderId = "OrderId";		            
		public const string Property_ItemId = "ItemId";		            
		public const string Property_SpecialInstruction = "SpecialInstruction";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_Toppings = "Toppings";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _OrderId;	            
		private Int32 _ItemId;	            
		private String _SpecialInstruction;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private String _Toppings;	            
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
		public Guid OrderId
		{	
			get{ return _OrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderId, value, _OrderId);
				if (PropertyChanging(args))
				{
					_OrderId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ItemId
		{	
			get{ return _ItemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemId, value, _ItemId);
				if (PropertyChanging(args))
				{
					_ItemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SpecialInstruction
		{	
			get{ return _SpecialInstruction; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SpecialInstruction, value, _SpecialInstruction);
				if (PropertyChanging(args))
				{
					_SpecialInstruction = value;
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
		public String Toppings
		{	
			get{ return _Toppings; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Toppings, value, _Toppings);
				if (PropertyChanging(args))
				{
					_Toppings = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ResturantOrderInstructionToppingBase Clone()
		{
			ResturantOrderInstructionToppingBase newObj = new  ResturantOrderInstructionToppingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.OrderId = this.OrderId;						
			newObj.ItemId = this.ItemId;						
			newObj.SpecialInstruction = this.SpecialInstruction;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.Toppings = this.Toppings;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ResturantOrderInstructionToppingBase.Property_Id, Id);				
			info.AddValue(ResturantOrderInstructionToppingBase.Property_OrderId, OrderId);				
			info.AddValue(ResturantOrderInstructionToppingBase.Property_ItemId, ItemId);				
			info.AddValue(ResturantOrderInstructionToppingBase.Property_SpecialInstruction, SpecialInstruction);				
			info.AddValue(ResturantOrderInstructionToppingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ResturantOrderInstructionToppingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ResturantOrderInstructionToppingBase.Property_Toppings, Toppings);				
		}
		#endregion

		
	}
}
