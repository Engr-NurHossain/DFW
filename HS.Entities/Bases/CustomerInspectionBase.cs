using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerInspectionBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerInspectionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			CurrentOutsideConditions = 3,
			OutsideRelativeHumidity = 4,
			OutsideTemperature = 5,
			FirstFloorRelativeHumidity = 6,
			FirstFloorTemperature = 7,
			RelativeOther1 = 8,
			RelativeOther2 = 9,
			Heat = 10,
			Air = 11,
			BasementRelativeHumidity = 12,
			BasementTemperature = 13,
			BasementDehumidifier = 14,
			GroundWater = 15,
			GroundWaterRating = 16,
			IronBacteria = 17,
			IronBacteriaRating = 18,
			Condensation = 19,
			CondensationRating = 20,
			WallCracks = 21,
			WallCracksRating = 22,
			FloorCracks = 23,
			FloorCracksRating = 24,
			ExistingSumpPump = 25,
			ExistingDrainageSystem = 26,
			ExistingRadonSystem = 27,
			DryerVentToCode = 28,
			FoundationType = 29,
			Bulkhead = 30,
			VisualBasementOther = 31,
			NoticedSmellsOrOdors = 32,
			NoticedSmellsOrOdorsComment = 33,
			NoticedMoldOrMildew = 34,
			NoticedMoldOrMildewComment = 35,
			BasementGoDown = 36,
			HomeSufferForRespiratory = 37,
			HomeSufferForrespiratoryComment = 38,
			ChildrenPlayInBasement = 39,
			ChildrenPlayInBasementComment = 40,
			PetsGoInBasement = 41,
			PetsGoInBasementComment = 42,
			NoticedBugsOrRodents = 43,
			NoticedBugsOrRodentsComment = 44,
			GetWater = 45,
			GetWaterComment = 46,
			RemoveWater = 47,
			SeeCondensationPipesDripping = 48,
			SeeCondensationPipesDrippingComment = 49,
			RepairsProblems = 50,
			RepairsProblemsComment = 51,
			LivingPlan = 52,
			SellPlaning = 53,
			PlansForBasementOnce = 54,
			HomeTestForPastRadon = 55,
			HomeTestForPastRadonComment = 56,
			LosePower = 57,
			LosePowerHowOften = 58,
			CustomerBasementOther = 59,
			Drawing = 60,
			Notes = 61,
			PMSignature = 62,
			PMSignatureDate = 63,
			HomeOwnerSignature = 64,
			HomeOwnerSignatureDate = 65,
			CreatedBy = 66,
			CreatedDate = 67,
			LastUpdatedBy = 68,
			LastUpdatedDate = 69,
			InspectionPhoto = 70
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CurrentOutsideConditions = "CurrentOutsideConditions";		            
		public const string Property_OutsideRelativeHumidity = "OutsideRelativeHumidity";		            
		public const string Property_OutsideTemperature = "OutsideTemperature";		            
		public const string Property_FirstFloorRelativeHumidity = "FirstFloorRelativeHumidity";		            
		public const string Property_FirstFloorTemperature = "FirstFloorTemperature";		            
		public const string Property_RelativeOther1 = "RelativeOther1";		            
		public const string Property_RelativeOther2 = "RelativeOther2";		            
		public const string Property_Heat = "Heat";		            
		public const string Property_Air = "Air";		            
		public const string Property_BasementRelativeHumidity = "BasementRelativeHumidity";		            
		public const string Property_BasementTemperature = "BasementTemperature";		            
		public const string Property_BasementDehumidifier = "BasementDehumidifier";		            
		public const string Property_GroundWater = "GroundWater";		            
		public const string Property_GroundWaterRating = "GroundWaterRating";		            
		public const string Property_IronBacteria = "IronBacteria";		            
		public const string Property_IronBacteriaRating = "IronBacteriaRating";		            
		public const string Property_Condensation = "Condensation";		            
		public const string Property_CondensationRating = "CondensationRating";		            
		public const string Property_WallCracks = "WallCracks";		            
		public const string Property_WallCracksRating = "WallCracksRating";		            
		public const string Property_FloorCracks = "FloorCracks";		            
		public const string Property_FloorCracksRating = "FloorCracksRating";		            
		public const string Property_ExistingSumpPump = "ExistingSumpPump";		            
		public const string Property_ExistingDrainageSystem = "ExistingDrainageSystem";		            
		public const string Property_ExistingRadonSystem = "ExistingRadonSystem";		            
		public const string Property_DryerVentToCode = "DryerVentToCode";		            
		public const string Property_FoundationType = "FoundationType";		            
		public const string Property_Bulkhead = "Bulkhead";		            
		public const string Property_VisualBasementOther = "VisualBasementOther";		            
		public const string Property_NoticedSmellsOrOdors = "NoticedSmellsOrOdors";		            
		public const string Property_NoticedSmellsOrOdorsComment = "NoticedSmellsOrOdorsComment";		            
		public const string Property_NoticedMoldOrMildew = "NoticedMoldOrMildew";		            
		public const string Property_NoticedMoldOrMildewComment = "NoticedMoldOrMildewComment";		            
		public const string Property_BasementGoDown = "BasementGoDown";		            
		public const string Property_HomeSufferForRespiratory = "HomeSufferForRespiratory";		            
		public const string Property_HomeSufferForrespiratoryComment = "HomeSufferForrespiratoryComment";		            
		public const string Property_ChildrenPlayInBasement = "ChildrenPlayInBasement";		            
		public const string Property_ChildrenPlayInBasementComment = "ChildrenPlayInBasementComment";		            
		public const string Property_PetsGoInBasement = "PetsGoInBasement";		            
		public const string Property_PetsGoInBasementComment = "PetsGoInBasementComment";		            
		public const string Property_NoticedBugsOrRodents = "NoticedBugsOrRodents";		            
		public const string Property_NoticedBugsOrRodentsComment = "NoticedBugsOrRodentsComment";		            
		public const string Property_GetWater = "GetWater";		            
		public const string Property_GetWaterComment = "GetWaterComment";		            
		public const string Property_RemoveWater = "RemoveWater";		            
		public const string Property_SeeCondensationPipesDripping = "SeeCondensationPipesDripping";		            
		public const string Property_SeeCondensationPipesDrippingComment = "SeeCondensationPipesDrippingComment";		            
		public const string Property_RepairsProblems = "RepairsProblems";		            
		public const string Property_RepairsProblemsComment = "RepairsProblemsComment";		            
		public const string Property_LivingPlan = "LivingPlan";		            
		public const string Property_SellPlaning = "SellPlaning";		            
		public const string Property_PlansForBasementOnce = "PlansForBasementOnce";		            
		public const string Property_HomeTestForPastRadon = "HomeTestForPastRadon";		            
		public const string Property_HomeTestForPastRadonComment = "HomeTestForPastRadonComment";		            
		public const string Property_LosePower = "LosePower";		            
		public const string Property_LosePowerHowOften = "LosePowerHowOften";		            
		public const string Property_CustomerBasementOther = "CustomerBasementOther";		            
		public const string Property_Drawing = "Drawing";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_PMSignature = "PMSignature";		            
		public const string Property_PMSignatureDate = "PMSignatureDate";		            
		public const string Property_HomeOwnerSignature = "HomeOwnerSignature";		            
		public const string Property_HomeOwnerSignatureDate = "HomeOwnerSignatureDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_InspectionPhoto = "InspectionPhoto";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _CurrentOutsideConditions;	            
		private Double _OutsideRelativeHumidity;	            
		private Double _OutsideTemperature;	            
		private Nullable<Double> _FirstFloorRelativeHumidity;	            
		private Nullable<Double> _FirstFloorTemperature;	            
		private String _RelativeOther1;	            
		private String _RelativeOther2;	            
		private String _Heat;	            
		private String _Air;	            
		private Nullable<Double> _BasementRelativeHumidity;	            
		private Nullable<Double> _BasementTemperature;	            
		private String _BasementDehumidifier;	            
		private String _GroundWater;	            
		private Nullable<Int32> _GroundWaterRating;	            
		private String _IronBacteria;	            
		private Nullable<Int32> _IronBacteriaRating;	            
		private String _Condensation;	            
		private Nullable<Int32> _CondensationRating;	            
		private String _WallCracks;	            
		private Nullable<Int32> _WallCracksRating;	            
		private String _FloorCracks;	            
		private Nullable<Int32> _FloorCracksRating;	            
		private String _ExistingSumpPump;	            
		private String _ExistingDrainageSystem;	            
		private String _ExistingRadonSystem;	            
		private String _DryerVentToCode;	            
		private String _FoundationType;	            
		private String _Bulkhead;	            
		private String _VisualBasementOther;	            
		private String _NoticedSmellsOrOdors;	            
		private String _NoticedSmellsOrOdorsComment;	            
		private String _NoticedMoldOrMildew;	            
		private String _NoticedMoldOrMildewComment;	            
		private String _BasementGoDown;	            
		private String _HomeSufferForRespiratory;	            
		private String _HomeSufferForrespiratoryComment;	            
		private String _ChildrenPlayInBasement;	            
		private String _ChildrenPlayInBasementComment;	            
		private String _PetsGoInBasement;	            
		private String _PetsGoInBasementComment;	            
		private String _NoticedBugsOrRodents;	            
		private String _NoticedBugsOrRodentsComment;	            
		private String _GetWater;	            
		private String _GetWaterComment;	            
		private String _RemoveWater;	            
		private String _SeeCondensationPipesDripping;	            
		private String _SeeCondensationPipesDrippingComment;	            
		private String _RepairsProblems;	            
		private String _RepairsProblemsComment;	            
		private String _LivingPlan;	            
		private String _SellPlaning;	            
		private String _PlansForBasementOnce;	            
		private String _HomeTestForPastRadon;	            
		private String _HomeTestForPastRadonComment;	            
		private String _LosePower;	            
		private String _LosePowerHowOften;	            
		private String _CustomerBasementOther;	            
		private String _Drawing;	            
		private String _Notes;	            
		private String _PMSignature;	            
		private Nullable<DateTime> _PMSignatureDate;	            
		private String _HomeOwnerSignature;	            
		private Nullable<DateTime> _HomeOwnerSignatureDate;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _InspectionPhoto;	            
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
		public String CurrentOutsideConditions
		{	
			get{ return _CurrentOutsideConditions; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CurrentOutsideConditions, value, _CurrentOutsideConditions);
				if (PropertyChanging(args))
				{
					_CurrentOutsideConditions = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double OutsideRelativeHumidity
		{	
			get{ return _OutsideRelativeHumidity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OutsideRelativeHumidity, value, _OutsideRelativeHumidity);
				if (PropertyChanging(args))
				{
					_OutsideRelativeHumidity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double OutsideTemperature
		{	
			get{ return _OutsideTemperature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OutsideTemperature, value, _OutsideTemperature);
				if (PropertyChanging(args))
				{
					_OutsideTemperature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FirstFloorRelativeHumidity
		{	
			get{ return _FirstFloorRelativeHumidity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstFloorRelativeHumidity, value, _FirstFloorRelativeHumidity);
				if (PropertyChanging(args))
				{
					_FirstFloorRelativeHumidity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FirstFloorTemperature
		{	
			get{ return _FirstFloorTemperature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstFloorTemperature, value, _FirstFloorTemperature);
				if (PropertyChanging(args))
				{
					_FirstFloorTemperature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RelativeOther1
		{	
			get{ return _RelativeOther1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RelativeOther1, value, _RelativeOther1);
				if (PropertyChanging(args))
				{
					_RelativeOther1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RelativeOther2
		{	
			get{ return _RelativeOther2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RelativeOther2, value, _RelativeOther2);
				if (PropertyChanging(args))
				{
					_RelativeOther2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Heat
		{	
			get{ return _Heat; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Heat, value, _Heat);
				if (PropertyChanging(args))
				{
					_Heat = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Air
		{	
			get{ return _Air; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Air, value, _Air);
				if (PropertyChanging(args))
				{
					_Air = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BasementRelativeHumidity
		{	
			get{ return _BasementRelativeHumidity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BasementRelativeHumidity, value, _BasementRelativeHumidity);
				if (PropertyChanging(args))
				{
					_BasementRelativeHumidity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BasementTemperature
		{	
			get{ return _BasementTemperature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BasementTemperature, value, _BasementTemperature);
				if (PropertyChanging(args))
				{
					_BasementTemperature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BasementDehumidifier
		{	
			get{ return _BasementDehumidifier; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BasementDehumidifier, value, _BasementDehumidifier);
				if (PropertyChanging(args))
				{
					_BasementDehumidifier = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GroundWater
		{	
			get{ return _GroundWater; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroundWater, value, _GroundWater);
				if (PropertyChanging(args))
				{
					_GroundWater = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> GroundWaterRating
		{	
			get{ return _GroundWaterRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GroundWaterRating, value, _GroundWaterRating);
				if (PropertyChanging(args))
				{
					_GroundWaterRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IronBacteria
		{	
			get{ return _IronBacteria; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IronBacteria, value, _IronBacteria);
				if (PropertyChanging(args))
				{
					_IronBacteria = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> IronBacteriaRating
		{	
			get{ return _IronBacteriaRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IronBacteriaRating, value, _IronBacteriaRating);
				if (PropertyChanging(args))
				{
					_IronBacteriaRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Condensation
		{	
			get{ return _Condensation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Condensation, value, _Condensation);
				if (PropertyChanging(args))
				{
					_Condensation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CondensationRating
		{	
			get{ return _CondensationRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CondensationRating, value, _CondensationRating);
				if (PropertyChanging(args))
				{
					_CondensationRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WallCracks
		{	
			get{ return _WallCracks; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WallCracks, value, _WallCracks);
				if (PropertyChanging(args))
				{
					_WallCracks = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> WallCracksRating
		{	
			get{ return _WallCracksRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WallCracksRating, value, _WallCracksRating);
				if (PropertyChanging(args))
				{
					_WallCracksRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FloorCracks
		{	
			get{ return _FloorCracks; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FloorCracks, value, _FloorCracks);
				if (PropertyChanging(args))
				{
					_FloorCracks = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> FloorCracksRating
		{	
			get{ return _FloorCracksRating; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FloorCracksRating, value, _FloorCracksRating);
				if (PropertyChanging(args))
				{
					_FloorCracksRating = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExistingSumpPump
		{	
			get{ return _ExistingSumpPump; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExistingSumpPump, value, _ExistingSumpPump);
				if (PropertyChanging(args))
				{
					_ExistingSumpPump = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExistingDrainageSystem
		{	
			get{ return _ExistingDrainageSystem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExistingDrainageSystem, value, _ExistingDrainageSystem);
				if (PropertyChanging(args))
				{
					_ExistingDrainageSystem = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExistingRadonSystem
		{	
			get{ return _ExistingRadonSystem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExistingRadonSystem, value, _ExistingRadonSystem);
				if (PropertyChanging(args))
				{
					_ExistingRadonSystem = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DryerVentToCode
		{	
			get{ return _DryerVentToCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DryerVentToCode, value, _DryerVentToCode);
				if (PropertyChanging(args))
				{
					_DryerVentToCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FoundationType
		{	
			get{ return _FoundationType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FoundationType, value, _FoundationType);
				if (PropertyChanging(args))
				{
					_FoundationType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Bulkhead
		{	
			get{ return _Bulkhead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Bulkhead, value, _Bulkhead);
				if (PropertyChanging(args))
				{
					_Bulkhead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String VisualBasementOther
		{	
			get{ return _VisualBasementOther; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VisualBasementOther, value, _VisualBasementOther);
				if (PropertyChanging(args))
				{
					_VisualBasementOther = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoticedSmellsOrOdors
		{	
			get{ return _NoticedSmellsOrOdors; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoticedSmellsOrOdors, value, _NoticedSmellsOrOdors);
				if (PropertyChanging(args))
				{
					_NoticedSmellsOrOdors = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoticedSmellsOrOdorsComment
		{	
			get{ return _NoticedSmellsOrOdorsComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoticedSmellsOrOdorsComment, value, _NoticedSmellsOrOdorsComment);
				if (PropertyChanging(args))
				{
					_NoticedSmellsOrOdorsComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoticedMoldOrMildew
		{	
			get{ return _NoticedMoldOrMildew; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoticedMoldOrMildew, value, _NoticedMoldOrMildew);
				if (PropertyChanging(args))
				{
					_NoticedMoldOrMildew = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoticedMoldOrMildewComment
		{	
			get{ return _NoticedMoldOrMildewComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoticedMoldOrMildewComment, value, _NoticedMoldOrMildewComment);
				if (PropertyChanging(args))
				{
					_NoticedMoldOrMildewComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BasementGoDown
		{	
			get{ return _BasementGoDown; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BasementGoDown, value, _BasementGoDown);
				if (PropertyChanging(args))
				{
					_BasementGoDown = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeSufferForRespiratory
		{	
			get{ return _HomeSufferForRespiratory; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeSufferForRespiratory, value, _HomeSufferForRespiratory);
				if (PropertyChanging(args))
				{
					_HomeSufferForRespiratory = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeSufferForrespiratoryComment
		{	
			get{ return _HomeSufferForrespiratoryComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeSufferForrespiratoryComment, value, _HomeSufferForrespiratoryComment);
				if (PropertyChanging(args))
				{
					_HomeSufferForrespiratoryComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ChildrenPlayInBasement
		{	
			get{ return _ChildrenPlayInBasement; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ChildrenPlayInBasement, value, _ChildrenPlayInBasement);
				if (PropertyChanging(args))
				{
					_ChildrenPlayInBasement = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ChildrenPlayInBasementComment
		{	
			get{ return _ChildrenPlayInBasementComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ChildrenPlayInBasementComment, value, _ChildrenPlayInBasementComment);
				if (PropertyChanging(args))
				{
					_ChildrenPlayInBasementComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PetsGoInBasement
		{	
			get{ return _PetsGoInBasement; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PetsGoInBasement, value, _PetsGoInBasement);
				if (PropertyChanging(args))
				{
					_PetsGoInBasement = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PetsGoInBasementComment
		{	
			get{ return _PetsGoInBasementComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PetsGoInBasementComment, value, _PetsGoInBasementComment);
				if (PropertyChanging(args))
				{
					_PetsGoInBasementComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoticedBugsOrRodents
		{	
			get{ return _NoticedBugsOrRodents; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoticedBugsOrRodents, value, _NoticedBugsOrRodents);
				if (PropertyChanging(args))
				{
					_NoticedBugsOrRodents = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoticedBugsOrRodentsComment
		{	
			get{ return _NoticedBugsOrRodentsComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoticedBugsOrRodentsComment, value, _NoticedBugsOrRodentsComment);
				if (PropertyChanging(args))
				{
					_NoticedBugsOrRodentsComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GetWater
		{	
			get{ return _GetWater; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GetWater, value, _GetWater);
				if (PropertyChanging(args))
				{
					_GetWater = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GetWaterComment
		{	
			get{ return _GetWaterComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GetWaterComment, value, _GetWaterComment);
				if (PropertyChanging(args))
				{
					_GetWaterComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RemoveWater
		{	
			get{ return _RemoveWater; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RemoveWater, value, _RemoveWater);
				if (PropertyChanging(args))
				{
					_RemoveWater = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SeeCondensationPipesDripping
		{	
			get{ return _SeeCondensationPipesDripping; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SeeCondensationPipesDripping, value, _SeeCondensationPipesDripping);
				if (PropertyChanging(args))
				{
					_SeeCondensationPipesDripping = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SeeCondensationPipesDrippingComment
		{	
			get{ return _SeeCondensationPipesDrippingComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SeeCondensationPipesDrippingComment, value, _SeeCondensationPipesDrippingComment);
				if (PropertyChanging(args))
				{
					_SeeCondensationPipesDrippingComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RepairsProblems
		{	
			get{ return _RepairsProblems; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepairsProblems, value, _RepairsProblems);
				if (PropertyChanging(args))
				{
					_RepairsProblems = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RepairsProblemsComment
		{	
			get{ return _RepairsProblemsComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepairsProblemsComment, value, _RepairsProblemsComment);
				if (PropertyChanging(args))
				{
					_RepairsProblemsComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LivingPlan
		{	
			get{ return _LivingPlan; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LivingPlan, value, _LivingPlan);
				if (PropertyChanging(args))
				{
					_LivingPlan = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SellPlaning
		{	
			get{ return _SellPlaning; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SellPlaning, value, _SellPlaning);
				if (PropertyChanging(args))
				{
					_SellPlaning = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PlansForBasementOnce
		{	
			get{ return _PlansForBasementOnce; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PlansForBasementOnce, value, _PlansForBasementOnce);
				if (PropertyChanging(args))
				{
					_PlansForBasementOnce = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeTestForPastRadon
		{	
			get{ return _HomeTestForPastRadon; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeTestForPastRadon, value, _HomeTestForPastRadon);
				if (PropertyChanging(args))
				{
					_HomeTestForPastRadon = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeTestForPastRadonComment
		{	
			get{ return _HomeTestForPastRadonComment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeTestForPastRadonComment, value, _HomeTestForPastRadonComment);
				if (PropertyChanging(args))
				{
					_HomeTestForPastRadonComment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LosePower
		{	
			get{ return _LosePower; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LosePower, value, _LosePower);
				if (PropertyChanging(args))
				{
					_LosePower = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LosePowerHowOften
		{	
			get{ return _LosePowerHowOften; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LosePowerHowOften, value, _LosePowerHowOften);
				if (PropertyChanging(args))
				{
					_LosePowerHowOften = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerBasementOther
		{	
			get{ return _CustomerBasementOther; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerBasementOther, value, _CustomerBasementOther);
				if (PropertyChanging(args))
				{
					_CustomerBasementOther = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Drawing
		{	
			get{ return _Drawing; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Drawing, value, _Drawing);
				if (PropertyChanging(args))
				{
					_Drawing = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PMSignature
		{	
			get{ return _PMSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PMSignature, value, _PMSignature);
				if (PropertyChanging(args))
				{
					_PMSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PMSignatureDate
		{	
			get{ return _PMSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PMSignatureDate, value, _PMSignatureDate);
				if (PropertyChanging(args))
				{
					_PMSignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeOwnerSignature
		{	
			get{ return _HomeOwnerSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeOwnerSignature, value, _HomeOwnerSignature);
				if (PropertyChanging(args))
				{
					_HomeOwnerSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> HomeOwnerSignatureDate
		{	
			get{ return _HomeOwnerSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeOwnerSignatureDate, value, _HomeOwnerSignatureDate);
				if (PropertyChanging(args))
				{
					_HomeOwnerSignatureDate = value;
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
		public Guid LastUpdatedBy
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
		public DateTime LastUpdatedDate
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
		public String InspectionPhoto
		{	
			get{ return _InspectionPhoto; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InspectionPhoto, value, _InspectionPhoto);
				if (PropertyChanging(args))
				{
					_InspectionPhoto = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerInspectionBase Clone()
		{
			CustomerInspectionBase newObj = new  CustomerInspectionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CurrentOutsideConditions = this.CurrentOutsideConditions;						
			newObj.OutsideRelativeHumidity = this.OutsideRelativeHumidity;						
			newObj.OutsideTemperature = this.OutsideTemperature;						
			newObj.FirstFloorRelativeHumidity = this.FirstFloorRelativeHumidity;						
			newObj.FirstFloorTemperature = this.FirstFloorTemperature;						
			newObj.RelativeOther1 = this.RelativeOther1;						
			newObj.RelativeOther2 = this.RelativeOther2;						
			newObj.Heat = this.Heat;						
			newObj.Air = this.Air;						
			newObj.BasementRelativeHumidity = this.BasementRelativeHumidity;						
			newObj.BasementTemperature = this.BasementTemperature;						
			newObj.BasementDehumidifier = this.BasementDehumidifier;						
			newObj.GroundWater = this.GroundWater;						
			newObj.GroundWaterRating = this.GroundWaterRating;						
			newObj.IronBacteria = this.IronBacteria;						
			newObj.IronBacteriaRating = this.IronBacteriaRating;						
			newObj.Condensation = this.Condensation;						
			newObj.CondensationRating = this.CondensationRating;						
			newObj.WallCracks = this.WallCracks;						
			newObj.WallCracksRating = this.WallCracksRating;						
			newObj.FloorCracks = this.FloorCracks;						
			newObj.FloorCracksRating = this.FloorCracksRating;						
			newObj.ExistingSumpPump = this.ExistingSumpPump;						
			newObj.ExistingDrainageSystem = this.ExistingDrainageSystem;						
			newObj.ExistingRadonSystem = this.ExistingRadonSystem;						
			newObj.DryerVentToCode = this.DryerVentToCode;						
			newObj.FoundationType = this.FoundationType;						
			newObj.Bulkhead = this.Bulkhead;						
			newObj.VisualBasementOther = this.VisualBasementOther;						
			newObj.NoticedSmellsOrOdors = this.NoticedSmellsOrOdors;						
			newObj.NoticedSmellsOrOdorsComment = this.NoticedSmellsOrOdorsComment;						
			newObj.NoticedMoldOrMildew = this.NoticedMoldOrMildew;						
			newObj.NoticedMoldOrMildewComment = this.NoticedMoldOrMildewComment;						
			newObj.BasementGoDown = this.BasementGoDown;						
			newObj.HomeSufferForRespiratory = this.HomeSufferForRespiratory;						
			newObj.HomeSufferForrespiratoryComment = this.HomeSufferForrespiratoryComment;						
			newObj.ChildrenPlayInBasement = this.ChildrenPlayInBasement;						
			newObj.ChildrenPlayInBasementComment = this.ChildrenPlayInBasementComment;						
			newObj.PetsGoInBasement = this.PetsGoInBasement;						
			newObj.PetsGoInBasementComment = this.PetsGoInBasementComment;						
			newObj.NoticedBugsOrRodents = this.NoticedBugsOrRodents;						
			newObj.NoticedBugsOrRodentsComment = this.NoticedBugsOrRodentsComment;						
			newObj.GetWater = this.GetWater;						
			newObj.GetWaterComment = this.GetWaterComment;						
			newObj.RemoveWater = this.RemoveWater;						
			newObj.SeeCondensationPipesDripping = this.SeeCondensationPipesDripping;						
			newObj.SeeCondensationPipesDrippingComment = this.SeeCondensationPipesDrippingComment;						
			newObj.RepairsProblems = this.RepairsProblems;						
			newObj.RepairsProblemsComment = this.RepairsProblemsComment;						
			newObj.LivingPlan = this.LivingPlan;						
			newObj.SellPlaning = this.SellPlaning;						
			newObj.PlansForBasementOnce = this.PlansForBasementOnce;						
			newObj.HomeTestForPastRadon = this.HomeTestForPastRadon;						
			newObj.HomeTestForPastRadonComment = this.HomeTestForPastRadonComment;						
			newObj.LosePower = this.LosePower;						
			newObj.LosePowerHowOften = this.LosePowerHowOften;						
			newObj.CustomerBasementOther = this.CustomerBasementOther;						
			newObj.Drawing = this.Drawing;						
			newObj.Notes = this.Notes;						
			newObj.PMSignature = this.PMSignature;						
			newObj.PMSignatureDate = this.PMSignatureDate;						
			newObj.HomeOwnerSignature = this.HomeOwnerSignature;						
			newObj.HomeOwnerSignatureDate = this.HomeOwnerSignatureDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.InspectionPhoto = this.InspectionPhoto;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerInspectionBase.Property_Id, Id);				
			info.AddValue(CustomerInspectionBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerInspectionBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerInspectionBase.Property_CurrentOutsideConditions, CurrentOutsideConditions);				
			info.AddValue(CustomerInspectionBase.Property_OutsideRelativeHumidity, OutsideRelativeHumidity);				
			info.AddValue(CustomerInspectionBase.Property_OutsideTemperature, OutsideTemperature);				
			info.AddValue(CustomerInspectionBase.Property_FirstFloorRelativeHumidity, FirstFloorRelativeHumidity);				
			info.AddValue(CustomerInspectionBase.Property_FirstFloorTemperature, FirstFloorTemperature);				
			info.AddValue(CustomerInspectionBase.Property_RelativeOther1, RelativeOther1);				
			info.AddValue(CustomerInspectionBase.Property_RelativeOther2, RelativeOther2);				
			info.AddValue(CustomerInspectionBase.Property_Heat, Heat);				
			info.AddValue(CustomerInspectionBase.Property_Air, Air);				
			info.AddValue(CustomerInspectionBase.Property_BasementRelativeHumidity, BasementRelativeHumidity);				
			info.AddValue(CustomerInspectionBase.Property_BasementTemperature, BasementTemperature);				
			info.AddValue(CustomerInspectionBase.Property_BasementDehumidifier, BasementDehumidifier);				
			info.AddValue(CustomerInspectionBase.Property_GroundWater, GroundWater);				
			info.AddValue(CustomerInspectionBase.Property_GroundWaterRating, GroundWaterRating);				
			info.AddValue(CustomerInspectionBase.Property_IronBacteria, IronBacteria);				
			info.AddValue(CustomerInspectionBase.Property_IronBacteriaRating, IronBacteriaRating);				
			info.AddValue(CustomerInspectionBase.Property_Condensation, Condensation);				
			info.AddValue(CustomerInspectionBase.Property_CondensationRating, CondensationRating);				
			info.AddValue(CustomerInspectionBase.Property_WallCracks, WallCracks);				
			info.AddValue(CustomerInspectionBase.Property_WallCracksRating, WallCracksRating);				
			info.AddValue(CustomerInspectionBase.Property_FloorCracks, FloorCracks);				
			info.AddValue(CustomerInspectionBase.Property_FloorCracksRating, FloorCracksRating);				
			info.AddValue(CustomerInspectionBase.Property_ExistingSumpPump, ExistingSumpPump);				
			info.AddValue(CustomerInspectionBase.Property_ExistingDrainageSystem, ExistingDrainageSystem);				
			info.AddValue(CustomerInspectionBase.Property_ExistingRadonSystem, ExistingRadonSystem);				
			info.AddValue(CustomerInspectionBase.Property_DryerVentToCode, DryerVentToCode);				
			info.AddValue(CustomerInspectionBase.Property_FoundationType, FoundationType);				
			info.AddValue(CustomerInspectionBase.Property_Bulkhead, Bulkhead);				
			info.AddValue(CustomerInspectionBase.Property_VisualBasementOther, VisualBasementOther);				
			info.AddValue(CustomerInspectionBase.Property_NoticedSmellsOrOdors, NoticedSmellsOrOdors);				
			info.AddValue(CustomerInspectionBase.Property_NoticedSmellsOrOdorsComment, NoticedSmellsOrOdorsComment);				
			info.AddValue(CustomerInspectionBase.Property_NoticedMoldOrMildew, NoticedMoldOrMildew);				
			info.AddValue(CustomerInspectionBase.Property_NoticedMoldOrMildewComment, NoticedMoldOrMildewComment);				
			info.AddValue(CustomerInspectionBase.Property_BasementGoDown, BasementGoDown);				
			info.AddValue(CustomerInspectionBase.Property_HomeSufferForRespiratory, HomeSufferForRespiratory);				
			info.AddValue(CustomerInspectionBase.Property_HomeSufferForrespiratoryComment, HomeSufferForrespiratoryComment);				
			info.AddValue(CustomerInspectionBase.Property_ChildrenPlayInBasement, ChildrenPlayInBasement);				
			info.AddValue(CustomerInspectionBase.Property_ChildrenPlayInBasementComment, ChildrenPlayInBasementComment);				
			info.AddValue(CustomerInspectionBase.Property_PetsGoInBasement, PetsGoInBasement);				
			info.AddValue(CustomerInspectionBase.Property_PetsGoInBasementComment, PetsGoInBasementComment);				
			info.AddValue(CustomerInspectionBase.Property_NoticedBugsOrRodents, NoticedBugsOrRodents);				
			info.AddValue(CustomerInspectionBase.Property_NoticedBugsOrRodentsComment, NoticedBugsOrRodentsComment);				
			info.AddValue(CustomerInspectionBase.Property_GetWater, GetWater);				
			info.AddValue(CustomerInspectionBase.Property_GetWaterComment, GetWaterComment);				
			info.AddValue(CustomerInspectionBase.Property_RemoveWater, RemoveWater);				
			info.AddValue(CustomerInspectionBase.Property_SeeCondensationPipesDripping, SeeCondensationPipesDripping);				
			info.AddValue(CustomerInspectionBase.Property_SeeCondensationPipesDrippingComment, SeeCondensationPipesDrippingComment);				
			info.AddValue(CustomerInspectionBase.Property_RepairsProblems, RepairsProblems);				
			info.AddValue(CustomerInspectionBase.Property_RepairsProblemsComment, RepairsProblemsComment);				
			info.AddValue(CustomerInspectionBase.Property_LivingPlan, LivingPlan);				
			info.AddValue(CustomerInspectionBase.Property_SellPlaning, SellPlaning);				
			info.AddValue(CustomerInspectionBase.Property_PlansForBasementOnce, PlansForBasementOnce);				
			info.AddValue(CustomerInspectionBase.Property_HomeTestForPastRadon, HomeTestForPastRadon);				
			info.AddValue(CustomerInspectionBase.Property_HomeTestForPastRadonComment, HomeTestForPastRadonComment);				
			info.AddValue(CustomerInspectionBase.Property_LosePower, LosePower);				
			info.AddValue(CustomerInspectionBase.Property_LosePowerHowOften, LosePowerHowOften);				
			info.AddValue(CustomerInspectionBase.Property_CustomerBasementOther, CustomerBasementOther);				
			info.AddValue(CustomerInspectionBase.Property_Drawing, Drawing);				
			info.AddValue(CustomerInspectionBase.Property_Notes, Notes);				
			info.AddValue(CustomerInspectionBase.Property_PMSignature, PMSignature);				
			info.AddValue(CustomerInspectionBase.Property_PMSignatureDate, PMSignatureDate);				
			info.AddValue(CustomerInspectionBase.Property_HomeOwnerSignature, HomeOwnerSignature);				
			info.AddValue(CustomerInspectionBase.Property_HomeOwnerSignatureDate, HomeOwnerSignatureDate);				
			info.AddValue(CustomerInspectionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerInspectionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerInspectionBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(CustomerInspectionBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(CustomerInspectionBase.Property_InspectionPhoto, InspectionPhoto);				
		}
		#endregion

		
	}
}
