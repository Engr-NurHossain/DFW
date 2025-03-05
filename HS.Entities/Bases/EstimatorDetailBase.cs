using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EstimatorDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class EstimatorDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EstimatorId = 1,
			PartDescription = 2,
			PartNumber = 3,
			CategoryId = 4,
			Unit = 5,
			Qunatity = 6,
			Overhead = 7,
			UnitCost = 8,
			TotalCost = 9,
			Profit = 10,
			TotalPrice = 11,
			EquipmentId = 12,
			SupplierId = 13,
			CreatedBy = 14,
			CreatedDate = 15,
			IsTaxable = 16,
			OverheadRate = 17,
			ProfitRate = 18,
			ManufacturerId = 19,
			EquipmentManufacturerId = 20,
			Variation = 21
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EstimatorId = "EstimatorId";		            
		public const string Property_PartDescription = "PartDescription";		            
		public const string Property_PartNumber = "PartNumber";		            
		public const string Property_CategoryId = "CategoryId";		            
		public const string Property_Unit = "Unit";		            
		public const string Property_Qunatity = "Qunatity";		            
		public const string Property_Overhead = "Overhead";		            
		public const string Property_UnitCost = "UnitCost";		            
		public const string Property_TotalCost = "TotalCost";		            
		public const string Property_Profit = "Profit";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_SupplierId = "SupplierId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsTaxable = "IsTaxable";		            
		public const string Property_OverheadRate = "OverheadRate";		            
		public const string Property_ProfitRate = "ProfitRate";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_EquipmentManufacturerId = "EquipmentManufacturerId";		            
		public const string Property_Variation = "Variation";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _EstimatorId;	            
		private String _PartDescription;	            
		private String _PartNumber;	            
		private Int32 _CategoryId;	            
		private String _Unit;	            
		private Nullable<Double> _Qunatity;	            
		private Nullable<Double> _Overhead;	            
		private Nullable<Double> _UnitCost;	            
		private Nullable<Double> _TotalCost;	            
		private Nullable<Double> _Profit;	            
		private Nullable<Double> _TotalPrice;	            
		private Guid _EquipmentId;	            
		private Guid _SupplierId;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsTaxable;	            
		private Nullable<Double> _OverheadRate;	            
		private Nullable<Double> _ProfitRate;	            
		private Guid _ManufacturerId;	            
		private Nullable<Int32> _EquipmentManufacturerId;	            
		private String _Variation;	            
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
		public String PartDescription
		{	
			get{ return _PartDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PartDescription, value, _PartDescription);
				if (PropertyChanging(args))
				{
					_PartDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PartNumber
		{	
			get{ return _PartNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PartNumber, value, _PartNumber);
				if (PropertyChanging(args))
				{
					_PartNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 CategoryId
		{	
			get{ return _CategoryId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CategoryId, value, _CategoryId);
				if (PropertyChanging(args))
				{
					_CategoryId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Unit
		{	
			get{ return _Unit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Unit, value, _Unit);
				if (PropertyChanging(args))
				{
					_Unit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Qunatity
		{	
			get{ return _Qunatity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Qunatity, value, _Qunatity);
				if (PropertyChanging(args))
				{
					_Qunatity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Overhead
		{	
			get{ return _Overhead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Overhead, value, _Overhead);
				if (PropertyChanging(args))
				{
					_Overhead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> UnitCost
		{	
			get{ return _UnitCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnitCost, value, _UnitCost);
				if (PropertyChanging(args))
				{
					_UnitCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalCost
		{	
			get{ return _TotalCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalCost, value, _TotalCost);
				if (PropertyChanging(args))
				{
					_TotalCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Profit
		{	
			get{ return _Profit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Profit, value, _Profit);
				if (PropertyChanging(args))
				{
					_Profit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalPrice
		{	
			get{ return _TotalPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalPrice, value, _TotalPrice);
				if (PropertyChanging(args))
				{
					_TotalPrice = value;
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
		public Guid SupplierId
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
		public Nullable<Boolean> IsTaxable
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
		public Nullable<Double> OverheadRate
		{	
			get{ return _OverheadRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OverheadRate, value, _OverheadRate);
				if (PropertyChanging(args))
				{
					_OverheadRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ProfitRate
		{	
			get{ return _ProfitRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProfitRate, value, _ProfitRate);
				if (PropertyChanging(args))
				{
					_ProfitRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ManufacturerId
		{	
			get{ return _ManufacturerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManufacturerId, value, _ManufacturerId);
				if (PropertyChanging(args))
				{
					_ManufacturerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> EquipmentManufacturerId
		{	
			get{ return _EquipmentManufacturerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentManufacturerId, value, _EquipmentManufacturerId);
				if (PropertyChanging(args))
				{
					_EquipmentManufacturerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Variation
		{	
			get{ return _Variation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Variation, value, _Variation);
				if (PropertyChanging(args))
				{
					_Variation = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EstimatorDetailBase Clone()
		{
			EstimatorDetailBase newObj = new  EstimatorDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EstimatorId = this.EstimatorId;						
			newObj.PartDescription = this.PartDescription;						
			newObj.PartNumber = this.PartNumber;						
			newObj.CategoryId = this.CategoryId;						
			newObj.Unit = this.Unit;						
			newObj.Qunatity = this.Qunatity;						
			newObj.Overhead = this.Overhead;						
			newObj.UnitCost = this.UnitCost;						
			newObj.TotalCost = this.TotalCost;						
			newObj.Profit = this.Profit;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.SupplierId = this.SupplierId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsTaxable = this.IsTaxable;						
			newObj.OverheadRate = this.OverheadRate;						
			newObj.ProfitRate = this.ProfitRate;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.EquipmentManufacturerId = this.EquipmentManufacturerId;						
			newObj.Variation = this.Variation;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EstimatorDetailBase.Property_Id, Id);				
			info.AddValue(EstimatorDetailBase.Property_EstimatorId, EstimatorId);				
			info.AddValue(EstimatorDetailBase.Property_PartDescription, PartDescription);				
			info.AddValue(EstimatorDetailBase.Property_PartNumber, PartNumber);				
			info.AddValue(EstimatorDetailBase.Property_CategoryId, CategoryId);				
			info.AddValue(EstimatorDetailBase.Property_Unit, Unit);				
			info.AddValue(EstimatorDetailBase.Property_Qunatity, Qunatity);				
			info.AddValue(EstimatorDetailBase.Property_Overhead, Overhead);				
			info.AddValue(EstimatorDetailBase.Property_UnitCost, UnitCost);				
			info.AddValue(EstimatorDetailBase.Property_TotalCost, TotalCost);				
			info.AddValue(EstimatorDetailBase.Property_Profit, Profit);				
			info.AddValue(EstimatorDetailBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(EstimatorDetailBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EstimatorDetailBase.Property_SupplierId, SupplierId);				
			info.AddValue(EstimatorDetailBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EstimatorDetailBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EstimatorDetailBase.Property_IsTaxable, IsTaxable);				
			info.AddValue(EstimatorDetailBase.Property_OverheadRate, OverheadRate);				
			info.AddValue(EstimatorDetailBase.Property_ProfitRate, ProfitRate);				
			info.AddValue(EstimatorDetailBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(EstimatorDetailBase.Property_EquipmentManufacturerId, EquipmentManufacturerId);				
			info.AddValue(EstimatorDetailBase.Property_Variation, Variation);				
		}
		#endregion

		
	}
}
