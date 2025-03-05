using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerExistingItemBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerExistingItemBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			ItemName = 2,
			Quantity = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_ItemName = "ItemName";		            
		public const string Property_Quantity = "Quantity";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _ItemName;	            
		private Nullable<Int32> _Quantity;	            
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerExistingItemBase Clone()
		{
			CustomerExistingItemBase newObj = new  CustomerExistingItemBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.ItemName = this.ItemName;						
			newObj.Quantity = this.Quantity;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerExistingItemBase.Property_Id, Id);				
			info.AddValue(CustomerExistingItemBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerExistingItemBase.Property_ItemName, ItemName);				
			info.AddValue(CustomerExistingItemBase.Property_Quantity, Quantity);				
		}
		#endregion

		
	}
}
