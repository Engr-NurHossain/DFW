using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EquipmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EquipmentId = 1,
			CompanyId = 2,
			Name = 3,
			SKU = 4,
			ManufacturerId = 5,
			SupplierId = 6,
			EquipmentTypeId = 7,
			EquipmentClassId = 8,
			Point = 9,
			SupplierCost = 10,
			Cost = 11,
			Retail = 12,
			EqOrder = 13,
			Service = 14,
			AsOfDate = 15,
			reorderpoint = 16,
			IsActive = 17,
			Comments = 18,
			CreatedDate = 19,
			LastUpdatedDate = 20,
			LastUpdatedBy = 21,
			POOrder = 22,
			IsKit = 23,
			RepCost = 24,
			RackNo = 25,
			Location = 26,
			Type = 27,
			Model = 28,
			Finish = 29,
			Capacity = 30,
			EquipmentPrice = 31,
			EquipmentPriceIsCharged = 32,
			ModelNumber = 33,
			Barcode = 34,
			Tag = 35,
			Note = 36,
			IsWarrenty = 37,
			IsARBEnabled = 38,
			IsUpsold = 39,
			IsTaxable = 40,
			OverheadRate = 41,
			ProfitRate = 42,
			Unit = 43,
			TaggedEmail = 44,
			IsIncludeEstimate = 45,
			whreorderpoint =46
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_SKU = "SKU";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_SupplierId = "SupplierId";		            
		public const string Property_EquipmentTypeId = "EquipmentTypeId";		            
		public const string Property_EquipmentClassId = "EquipmentClassId";		            
		public const string Property_Point = "Point";		            
		public const string Property_SupplierCost = "SupplierCost";		            
		public const string Property_Cost = "Cost";		            
		public const string Property_Retail = "Retail";		            
		public const string Property_EqOrder = "EqOrder";		            
		public const string Property_Service = "Service";		            
		public const string Property_AsOfDate = "AsOfDate";		            
		public const string Property_reorderpoint = "reorderpoint";
		public const string Property_whreorderpoint = "whreorderpoint";
		public const string Property_IsActive = "IsActive";		            
		public const string Property_Comments = "Comments";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_POOrder = "POOrder";		            
		public const string Property_IsKit = "IsKit";		            
		public const string Property_RepCost = "RepCost";		            
		public const string Property_RackNo = "RackNo";		            
		public const string Property_Location = "Location";		            
		public const string Property_Type = "Type";		            
		public const string Property_Model = "Model";		            
		public const string Property_Finish = "Finish";		            
		public const string Property_Capacity = "Capacity";		            
		public const string Property_EquipmentPrice = "EquipmentPrice";		            
		public const string Property_EquipmentPriceIsCharged = "EquipmentPriceIsCharged";		            
		public const string Property_ModelNumber = "ModelNumber";		            
		public const string Property_Barcode = "Barcode";		            
		public const string Property_Tag = "Tag";		            
		public const string Property_Note = "Note";		            
		public const string Property_IsWarrenty = "IsWarrenty";		            
		public const string Property_IsARBEnabled = "IsARBEnabled";		            
		public const string Property_IsUpsold = "IsUpsold";		            
		public const string Property_IsTaxable = "IsTaxable";		            
		public const string Property_OverheadRate = "OverheadRate";		            
		public const string Property_ProfitRate = "ProfitRate";		            
		public const string Property_Unit = "Unit";		            
		public const string Property_TaggedEmail = "TaggedEmail";		            
		public const string Property_IsIncludeEstimate = "IsIncludeEstimate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EquipmentId;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _SKU;	            
		private Int32 _ManufacturerId;	            
		private Int32 _SupplierId;	            
		private Int32 _EquipmentTypeId;	            
		private Int32 _EquipmentClassId;	            
		private Nullable<Double> _Point;	            
		private Nullable<Double> _SupplierCost;	            
		private Nullable<Double> _Cost;	            
		private Nullable<Double> _Retail;	            
		private Nullable<Int32> _EqOrder;	            
		private String _Service;	            
		private Nullable<DateTime> _AsOfDate;	            
		private Nullable<Int32> _reorderpoint;
		private Nullable<Int32> _whreorderpoint;
		private Boolean _IsActive;	            
		private String _Comments;	            
		private DateTime _CreatedDate;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private String _LastUpdatedBy;	            
		private Nullable<Boolean> _POOrder;	            
		private Nullable<Boolean> _IsKit;	            
		private Nullable<Double> _RepCost;	            
		private String _RackNo;	            
		private String _Location;	            
		private String _Type;	            
		private String _Model;	            
		private String _Finish;	            
		private String _Capacity;	            
		private Nullable<Double> _EquipmentPrice;	            
		private Nullable<Boolean> _EquipmentPriceIsCharged;	            
		private String _ModelNumber;	            
		private String _Barcode;	            
		private String _Tag;	            
		private String _Note;	            
		private Nullable<Boolean> _IsWarrenty;	            
		private Nullable<Boolean> _IsARBEnabled;	            
		private Nullable<Boolean> _IsUpsold;	            
		private Nullable<Boolean> _IsTaxable;	            
		private Nullable<Double> _OverheadRate;	            
		private Nullable<Double> _ProfitRate;	            
		private String _Unit;	            
		private String _TaggedEmail;	            
		private Nullable<Boolean> _IsIncludeEstimate;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SKU
		{	
			get{ return _SKU; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SKU, value, _SKU);
				if (PropertyChanging(args))
				{
					_SKU = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 ManufacturerId
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
		public Int32 EquipmentTypeId
		{	
			get{ return _EquipmentTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentTypeId, value, _EquipmentTypeId);
				if (PropertyChanging(args))
				{
					_EquipmentTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 EquipmentClassId
		{	
			get{ return _EquipmentClassId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentClassId, value, _EquipmentClassId);
				if (PropertyChanging(args))
				{
					_EquipmentClassId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Point
		{	
			get{ return _Point; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Point, value, _Point);
				if (PropertyChanging(args))
				{
					_Point = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> SupplierCost
		{	
			get{ return _SupplierCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierCost, value, _SupplierCost);
				if (PropertyChanging(args))
				{
					_SupplierCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Cost
		{	
			get{ return _Cost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Cost, value, _Cost);
				if (PropertyChanging(args))
				{
					_Cost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Retail
		{	
			get{ return _Retail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Retail, value, _Retail);
				if (PropertyChanging(args))
				{
					_Retail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> EqOrder
		{	
			get{ return _EqOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EqOrder, value, _EqOrder);
				if (PropertyChanging(args))
				{
					_EqOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Service
		{	
			get{ return _Service; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Service, value, _Service);
				if (PropertyChanging(args))
				{
					_Service = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AsOfDate
		{	
			get{ return _AsOfDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AsOfDate, value, _AsOfDate);
				if (PropertyChanging(args))
				{
					_AsOfDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> reorderpoint
		{	
			get{ return _reorderpoint; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_reorderpoint, value, _reorderpoint);
				if (PropertyChanging(args))
				{
					_reorderpoint = value;
					PropertyChanged(args);					
				}	
			}
        }
		[DataMember]
		public Nullable<Int32> whreorderpoint
		{
			get { return _whreorderpoint; }
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_whreorderpoint, value, _whreorderpoint);
				if (PropertyChanging(args))
				{
					_whreorderpoint = value;
					PropertyChanged(args);
				}
			}
		}
		[DataMember]
		public Boolean IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Comments
		{	
			get{ return _Comments; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Comments, value, _Comments);
				if (PropertyChanging(args))
				{
					_Comments = value;
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
		public Nullable<DateTime> LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> POOrder
		{	
			get{ return _POOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_POOrder, value, _POOrder);
				if (PropertyChanging(args))
				{
					_POOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsKit
		{	
			get{ return _IsKit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsKit, value, _IsKit);
				if (PropertyChanging(args))
				{
					_IsKit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RepCost
		{	
			get{ return _RepCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepCost, value, _RepCost);
				if (PropertyChanging(args))
				{
					_RepCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RackNo
		{	
			get{ return _RackNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RackNo, value, _RackNo);
				if (PropertyChanging(args))
				{
					_RackNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Location
		{	
			get{ return _Location; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Location, value, _Location);
				if (PropertyChanging(args))
				{
					_Location = value;
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
		public String Model
		{	
			get{ return _Model; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Model, value, _Model);
				if (PropertyChanging(args))
				{
					_Model = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Finish
		{	
			get{ return _Finish; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Finish, value, _Finish);
				if (PropertyChanging(args))
				{
					_Finish = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Capacity
		{	
			get{ return _Capacity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Capacity, value, _Capacity);
				if (PropertyChanging(args))
				{
					_Capacity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> EquipmentPrice
		{	
			get{ return _EquipmentPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentPrice, value, _EquipmentPrice);
				if (PropertyChanging(args))
				{
					_EquipmentPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> EquipmentPriceIsCharged
		{	
			get{ return _EquipmentPriceIsCharged; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentPriceIsCharged, value, _EquipmentPriceIsCharged);
				if (PropertyChanging(args))
				{
					_EquipmentPriceIsCharged = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ModelNumber
		{	
			get{ return _ModelNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ModelNumber, value, _ModelNumber);
				if (PropertyChanging(args))
				{
					_ModelNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Barcode
		{	
			get{ return _Barcode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Barcode, value, _Barcode);
				if (PropertyChanging(args))
				{
					_Barcode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Tag
		{	
			get{ return _Tag; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tag, value, _Tag);
				if (PropertyChanging(args))
				{
					_Tag = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsWarrenty
		{	
			get{ return _IsWarrenty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsWarrenty, value, _IsWarrenty);
				if (PropertyChanging(args))
				{
					_IsWarrenty = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsARBEnabled
		{	
			get{ return _IsARBEnabled; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsARBEnabled, value, _IsARBEnabled);
				if (PropertyChanging(args))
				{
					_IsARBEnabled = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsUpsold
		{	
			get{ return _IsUpsold; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUpsold, value, _IsUpsold);
				if (PropertyChanging(args))
				{
					_IsUpsold = value;
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
		public String TaggedEmail
		{	
			get{ return _TaggedEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaggedEmail, value, _TaggedEmail);
				if (PropertyChanging(args))
				{
					_TaggedEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsIncludeEstimate
		{	
			get{ return _IsIncludeEstimate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsIncludeEstimate, value, _IsIncludeEstimate);
				if (PropertyChanging(args))
				{
					_IsIncludeEstimate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EquipmentBase Clone()
		{
			EquipmentBase newObj = new  EquipmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.SKU = this.SKU;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.SupplierId = this.SupplierId;						
			newObj.EquipmentTypeId = this.EquipmentTypeId;						
			newObj.EquipmentClassId = this.EquipmentClassId;						
			newObj.Point = this.Point;						
			newObj.SupplierCost = this.SupplierCost;						
			newObj.Cost = this.Cost;						
			newObj.Retail = this.Retail;						
			newObj.EqOrder = this.EqOrder;						
			newObj.Service = this.Service;						
			newObj.AsOfDate = this.AsOfDate;						
			newObj.reorderpoint = this.reorderpoint;
			newObj.whreorderpoint = this.whreorderpoint;
			newObj.IsActive = this.IsActive;						
			newObj.Comments = this.Comments;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.POOrder = this.POOrder;						
			newObj.IsKit = this.IsKit;						
			newObj.RepCost = this.RepCost;						
			newObj.RackNo = this.RackNo;						
			newObj.Location = this.Location;						
			newObj.Type = this.Type;						
			newObj.Model = this.Model;						
			newObj.Finish = this.Finish;						
			newObj.Capacity = this.Capacity;						
			newObj.EquipmentPrice = this.EquipmentPrice;						
			newObj.EquipmentPriceIsCharged = this.EquipmentPriceIsCharged;						
			newObj.ModelNumber = this.ModelNumber;						
			newObj.Barcode = this.Barcode;						
			newObj.Tag = this.Tag;						
			newObj.Note = this.Note;						
			newObj.IsWarrenty = this.IsWarrenty;						
			newObj.IsARBEnabled = this.IsARBEnabled;						
			newObj.IsUpsold = this.IsUpsold;						
			newObj.IsTaxable = this.IsTaxable;						
			newObj.OverheadRate = this.OverheadRate;						
			newObj.ProfitRate = this.ProfitRate;						
			newObj.Unit = this.Unit;						
			newObj.TaggedEmail = this.TaggedEmail;						
			newObj.IsIncludeEstimate = this.IsIncludeEstimate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentBase.Property_Id, Id);				
			info.AddValue(EquipmentBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentBase.Property_CompanyId, CompanyId);				
			info.AddValue(EquipmentBase.Property_Name, Name);				
			info.AddValue(EquipmentBase.Property_SKU, SKU);				
			info.AddValue(EquipmentBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(EquipmentBase.Property_SupplierId, SupplierId);				
			info.AddValue(EquipmentBase.Property_EquipmentTypeId, EquipmentTypeId);				
			info.AddValue(EquipmentBase.Property_EquipmentClassId, EquipmentClassId);				
			info.AddValue(EquipmentBase.Property_Point, Point);				
			info.AddValue(EquipmentBase.Property_SupplierCost, SupplierCost);				
			info.AddValue(EquipmentBase.Property_Cost, Cost);				
			info.AddValue(EquipmentBase.Property_Retail, Retail);				
			info.AddValue(EquipmentBase.Property_EqOrder, EqOrder);				
			info.AddValue(EquipmentBase.Property_Service, Service);				
			info.AddValue(EquipmentBase.Property_AsOfDate, AsOfDate);				
			info.AddValue(EquipmentBase.Property_reorderpoint, reorderpoint);
			info.AddValue(EquipmentBase.Property_whreorderpoint, whreorderpoint);
			info.AddValue(EquipmentBase.Property_IsActive, IsActive);				
			info.AddValue(EquipmentBase.Property_Comments, Comments);				
			info.AddValue(EquipmentBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EquipmentBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(EquipmentBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EquipmentBase.Property_POOrder, POOrder);				
			info.AddValue(EquipmentBase.Property_IsKit, IsKit);				
			info.AddValue(EquipmentBase.Property_RepCost, RepCost);				
			info.AddValue(EquipmentBase.Property_RackNo, RackNo);				
			info.AddValue(EquipmentBase.Property_Location, Location);				
			info.AddValue(EquipmentBase.Property_Type, Type);				
			info.AddValue(EquipmentBase.Property_Model, Model);				
			info.AddValue(EquipmentBase.Property_Finish, Finish);				
			info.AddValue(EquipmentBase.Property_Capacity, Capacity);				
			info.AddValue(EquipmentBase.Property_EquipmentPrice, EquipmentPrice);				
			info.AddValue(EquipmentBase.Property_EquipmentPriceIsCharged, EquipmentPriceIsCharged);				
			info.AddValue(EquipmentBase.Property_ModelNumber, ModelNumber);				
			info.AddValue(EquipmentBase.Property_Barcode, Barcode);				
			info.AddValue(EquipmentBase.Property_Tag, Tag);				
			info.AddValue(EquipmentBase.Property_Note, Note);				
			info.AddValue(EquipmentBase.Property_IsWarrenty, IsWarrenty);				
			info.AddValue(EquipmentBase.Property_IsARBEnabled, IsARBEnabled);				
			info.AddValue(EquipmentBase.Property_IsUpsold, IsUpsold);				
			info.AddValue(EquipmentBase.Property_IsTaxable, IsTaxable);				
			info.AddValue(EquipmentBase.Property_OverheadRate, OverheadRate);				
			info.AddValue(EquipmentBase.Property_ProfitRate, ProfitRate);				
			info.AddValue(EquipmentBase.Property_Unit, Unit);				
			info.AddValue(EquipmentBase.Property_TaggedEmail, TaggedEmail);				
			info.AddValue(EquipmentBase.Property_IsIncludeEstimate, IsIncludeEstimate);				
		}
		#endregion

		
	}
}
