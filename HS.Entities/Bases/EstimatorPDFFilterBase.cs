using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EstimatorPDFFilterBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EstimatorPDFFilterBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			GroupedbyNone = 3,
			GroupedbyCategory = 4,
			GroupedbyLabor = 5,
			GroupedbyLaborAndMaterial = 6,
			GroupedbyMaterial = 7,
			GroupedbySupplier = 8,
			IncludeCost = 9,
			IncludeImage = 10,
			IncludeManufacturer = 11,
			IncludeMargin = 12,
			IncludeOverhead = 13,
			IncludePDF = 14,
			IncludeProfit = 15,
			IncludeService = 16,
			WithoutIndividualLaborPricing = 17,
			WithoutIndividualMaterialPricing = 18,
			WithoutPricing = 19,
			CreatedBy = 20,
			CreatedDate = 21,
			IncludeVariation = 22,
			OneTimeService = 23
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_GroupedbyNone = "GroupedbyNone";		            
		public const string Property_GroupedbyCategory = "GroupedbyCategory";		            
		public const string Property_GroupedbyLabor = "GroupedbyLabor";		            
		public const string Property_GroupedbyLaborAndMaterial = "GroupedbyLaborAndMaterial";		            
		public const string Property_GroupedbyMaterial = "GroupedbyMaterial";		            
		public const string Property_GroupedbySupplier = "GroupedbySupplier";		            
		public const string Property_IncludeCost = "IncludeCost";		            
		public const string Property_IncludeImage = "IncludeImage";		            
		public const string Property_IncludeManufacturer = "IncludeManufacturer";		            
		public const string Property_IncludeMargin = "IncludeMargin";		            
		public const string Property_IncludeOverhead = "IncludeOverhead";		            
		public const string Property_IncludePDF = "IncludePDF";		            
		public const string Property_IncludeProfit = "IncludeProfit";		            
		public const string Property_IncludeService = "IncludeService";		            
		public const string Property_WithoutIndividualLaborPricing = "WithoutIndividualLaborPricing";		            
		public const string Property_WithoutIndividualMaterialPricing = "WithoutIndividualMaterialPricing";		            
		public const string Property_WithoutPricing = "WithoutPricing";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IncludeVariation = "IncludeVariation";		            
		public const string Property_OneTimeService = "OneTimeService";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Nullable<Boolean> _GroupedbyNone;	            
		private Nullable<Boolean> _GroupedbyCategory;	            
		private Nullable<Boolean> _GroupedbyLabor;	            
		private Nullable<Boolean> _GroupedbyLaborAndMaterial;	            
		private Nullable<Boolean> _GroupedbyMaterial;	            
		private Nullable<Boolean> _GroupedbySupplier;	            
		private Nullable<Boolean> _IncludeCost;	            
		private Nullable<Boolean> _IncludeImage;	            
		private Nullable<Boolean> _IncludeManufacturer;	            
		private Nullable<Boolean> _IncludeMargin;	            
		private Nullable<Boolean> _IncludeOverhead;	            
		private Nullable<Boolean> _IncludePDF;	            
		private Nullable<Boolean> _IncludeProfit;	            
		private Nullable<Boolean> _IncludeService;	            
		private Nullable<Boolean> _WithoutIndividualLaborPricing;	            
		private Nullable<Boolean> _WithoutIndividualMaterialPricing;	            
		private Nullable<Boolean> _WithoutPricing;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IncludeVariation;	            
		private Nullable<Boolean> _OneTimeService;	            
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
		public Nullable<Boolean> GroupedbyNone
		{	
			get{ return _GroupedbyNone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupedbyNone, value, _GroupedbyNone);
				if (PropertyChanging(args))
				{
					_GroupedbyNone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> GroupedbyCategory
		{	
			get{ return _GroupedbyCategory; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupedbyCategory, value, _GroupedbyCategory);
				if (PropertyChanging(args))
				{
					_GroupedbyCategory = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> GroupedbyLabor
		{	
			get{ return _GroupedbyLabor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupedbyLabor, value, _GroupedbyLabor);
				if (PropertyChanging(args))
				{
					_GroupedbyLabor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> GroupedbyLaborAndMaterial
		{	
			get{ return _GroupedbyLaborAndMaterial; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupedbyLaborAndMaterial, value, _GroupedbyLaborAndMaterial);
				if (PropertyChanging(args))
				{
					_GroupedbyLaborAndMaterial = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> GroupedbyMaterial
		{	
			get{ return _GroupedbyMaterial; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupedbyMaterial, value, _GroupedbyMaterial);
				if (PropertyChanging(args))
				{
					_GroupedbyMaterial = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> GroupedbySupplier
		{	
			get{ return _GroupedbySupplier; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroupedbySupplier, value, _GroupedbySupplier);
				if (PropertyChanging(args))
				{
					_GroupedbySupplier = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeCost
		{	
			get{ return _IncludeCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeCost, value, _IncludeCost);
				if (PropertyChanging(args))
				{
					_IncludeCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeImage
		{	
			get{ return _IncludeImage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeImage, value, _IncludeImage);
				if (PropertyChanging(args))
				{
					_IncludeImage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeManufacturer
		{	
			get{ return _IncludeManufacturer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeManufacturer, value, _IncludeManufacturer);
				if (PropertyChanging(args))
				{
					_IncludeManufacturer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeMargin
		{	
			get{ return _IncludeMargin; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeMargin, value, _IncludeMargin);
				if (PropertyChanging(args))
				{
					_IncludeMargin = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeOverhead
		{	
			get{ return _IncludeOverhead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeOverhead, value, _IncludeOverhead);
				if (PropertyChanging(args))
				{
					_IncludeOverhead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludePDF
		{	
			get{ return _IncludePDF; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludePDF, value, _IncludePDF);
				if (PropertyChanging(args))
				{
					_IncludePDF = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeProfit
		{	
			get{ return _IncludeProfit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeProfit, value, _IncludeProfit);
				if (PropertyChanging(args))
				{
					_IncludeProfit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IncludeService
		{	
			get{ return _IncludeService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeService, value, _IncludeService);
				if (PropertyChanging(args))
				{
					_IncludeService = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> WithoutIndividualLaborPricing
		{	
			get{ return _WithoutIndividualLaborPricing; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WithoutIndividualLaborPricing, value, _WithoutIndividualLaborPricing);
				if (PropertyChanging(args))
				{
					_WithoutIndividualLaborPricing = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> WithoutIndividualMaterialPricing
		{	
			get{ return _WithoutIndividualMaterialPricing; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WithoutIndividualMaterialPricing, value, _WithoutIndividualMaterialPricing);
				if (PropertyChanging(args))
				{
					_WithoutIndividualMaterialPricing = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> WithoutPricing
		{	
			get{ return _WithoutPricing; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WithoutPricing, value, _WithoutPricing);
				if (PropertyChanging(args))
				{
					_WithoutPricing = value;
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
		public Nullable<Boolean> IncludeVariation
		{	
			get{ return _IncludeVariation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeVariation, value, _IncludeVariation);
				if (PropertyChanging(args))
				{
					_IncludeVariation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> OneTimeService
		{	
			get{ return _OneTimeService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OneTimeService, value, _OneTimeService);
				if (PropertyChanging(args))
				{
					_OneTimeService = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EstimatorPDFFilterBase Clone()
		{
			EstimatorPDFFilterBase newObj = new  EstimatorPDFFilterBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.GroupedbyNone = this.GroupedbyNone;						
			newObj.GroupedbyCategory = this.GroupedbyCategory;						
			newObj.GroupedbyLabor = this.GroupedbyLabor;						
			newObj.GroupedbyLaborAndMaterial = this.GroupedbyLaborAndMaterial;						
			newObj.GroupedbyMaterial = this.GroupedbyMaterial;						
			newObj.GroupedbySupplier = this.GroupedbySupplier;						
			newObj.IncludeCost = this.IncludeCost;						
			newObj.IncludeImage = this.IncludeImage;						
			newObj.IncludeManufacturer = this.IncludeManufacturer;						
			newObj.IncludeMargin = this.IncludeMargin;						
			newObj.IncludeOverhead = this.IncludeOverhead;						
			newObj.IncludePDF = this.IncludePDF;						
			newObj.IncludeProfit = this.IncludeProfit;						
			newObj.IncludeService = this.IncludeService;						
			newObj.WithoutIndividualLaborPricing = this.WithoutIndividualLaborPricing;						
			newObj.WithoutIndividualMaterialPricing = this.WithoutIndividualMaterialPricing;						
			newObj.WithoutPricing = this.WithoutPricing;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IncludeVariation = this.IncludeVariation;						
			newObj.OneTimeService = this.OneTimeService;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EstimatorPDFFilterBase.Property_Id, Id);				
			info.AddValue(EstimatorPDFFilterBase.Property_CompanyId, CompanyId);				
			info.AddValue(EstimatorPDFFilterBase.Property_CustomerId, CustomerId);				
			info.AddValue(EstimatorPDFFilterBase.Property_GroupedbyNone, GroupedbyNone);				
			info.AddValue(EstimatorPDFFilterBase.Property_GroupedbyCategory, GroupedbyCategory);				
			info.AddValue(EstimatorPDFFilterBase.Property_GroupedbyLabor, GroupedbyLabor);				
			info.AddValue(EstimatorPDFFilterBase.Property_GroupedbyLaborAndMaterial, GroupedbyLaborAndMaterial);				
			info.AddValue(EstimatorPDFFilterBase.Property_GroupedbyMaterial, GroupedbyMaterial);				
			info.AddValue(EstimatorPDFFilterBase.Property_GroupedbySupplier, GroupedbySupplier);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeCost, IncludeCost);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeImage, IncludeImage);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeManufacturer, IncludeManufacturer);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeMargin, IncludeMargin);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeOverhead, IncludeOverhead);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludePDF, IncludePDF);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeProfit, IncludeProfit);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeService, IncludeService);				
			info.AddValue(EstimatorPDFFilterBase.Property_WithoutIndividualLaborPricing, WithoutIndividualLaborPricing);				
			info.AddValue(EstimatorPDFFilterBase.Property_WithoutIndividualMaterialPricing, WithoutIndividualMaterialPricing);				
			info.AddValue(EstimatorPDFFilterBase.Property_WithoutPricing, WithoutPricing);				
			info.AddValue(EstimatorPDFFilterBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EstimatorPDFFilterBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EstimatorPDFFilterBase.Property_IncludeVariation, IncludeVariation);				
			info.AddValue(EstimatorPDFFilterBase.Property_OneTimeService, OneTimeService);				
		}
		#endregion

		
	}
}
