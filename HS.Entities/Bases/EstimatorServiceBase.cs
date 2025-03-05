using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EstimatorServiceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EstimatorServiceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EstimatorId = 1,
			EquipmentId = 2,
			EquipmentName = 3,
			UnitPrice = 4,
			Quantity = 5,
			Amount = 6,
			IsTaxable = 7,
			CreatedBy = 8,
			CreatedDate = 9,
			IsOneTimeService = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EstimatorId = "EstimatorId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_EquipmentName = "EquipmentName";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_IsTaxable = "IsTaxable";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsOneTimeService = "IsOneTimeService";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _EstimatorId;	            
		private Guid _EquipmentId;	            
		private String _EquipmentName;	            
		private Double _UnitPrice;	            
		private Int32 _Quantity;	            
		private Double _Amount;	            
		private Boolean _IsTaxable;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsOneTimeService;	            
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
		public String EstimatorId
		{	
			get{ return _EstimatorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatorId, value, _EstimatorId);
				if (PropertyChanging(args))
				{
					_EstimatorId = value;
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
		public String EquipmentName
		{	
			get{ return _EquipmentName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentName, value, _EquipmentName);
				if (PropertyChanging(args))
				{
					_EquipmentName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double UnitPrice
		{	
			get{ return _UnitPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnitPrice, value, _UnitPrice);
				if (PropertyChanging(args))
				{
					_UnitPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Quantity
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
		public Boolean IsTaxable
		{	
			get{ return _IsTaxable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTaxable, value, _IsTaxable);
				if (PropertyChanging(args))
				{
					_IsTaxable = value;
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
		public Nullable<Boolean> IsOneTimeService
		{	
			get{ return _IsOneTimeService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsOneTimeService, value, _IsOneTimeService);
				if (PropertyChanging(args))
				{
					_IsOneTimeService = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EstimatorServiceBase Clone()
		{
			EstimatorServiceBase newObj = new  EstimatorServiceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EstimatorId = this.EstimatorId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.EquipmentName = this.EquipmentName;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.Quantity = this.Quantity;						
			newObj.Amount = this.Amount;						
			newObj.IsTaxable = this.IsTaxable;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsOneTimeService = this.IsOneTimeService;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EstimatorServiceBase.Property_Id, Id);				
			info.AddValue(EstimatorServiceBase.Property_EstimatorId, EstimatorId);				
			info.AddValue(EstimatorServiceBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EstimatorServiceBase.Property_EquipmentName, EquipmentName);				
			info.AddValue(EstimatorServiceBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(EstimatorServiceBase.Property_Quantity, Quantity);				
			info.AddValue(EstimatorServiceBase.Property_Amount, Amount);				
			info.AddValue(EstimatorServiceBase.Property_IsTaxable, IsTaxable);				
			info.AddValue(EstimatorServiceBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EstimatorServiceBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EstimatorServiceBase.Property_IsOneTimeService, IsOneTimeService);				
		}
		#endregion

		
	}
}
