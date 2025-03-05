using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.Custom
{
    public class NumberVerifyResponse
    {
        public string country_code { get; set; }
        public string carrier { get; set; }
        public string location { get; set; }
        public string line_type { get; set; }
        public string valid { get; set; }
        //public string MyProperty { get; set; }
        //public string MyProperty { get; set; }
        //public string MyProperty { get; set; }
    }

    //public class PrimaryGrantee
    //{
    //    public string Name1Full { get; set; }
    //    public string Name2Full { get; set; }
    //}
    //public class Records
    //{
    //    public PrimaryGrantee PrimaryGrantee { get; set; }
    //}
    //public class VerifiedHomeOwner
    //{
    //    public List<Records> Records { get; set; }

    //}
    public class VerifiedRecord
    {
        public string AddressExtras { get; set; }
        public string AddressKey { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string MelissaAddressKey { get; set; }
        public string MelissaAddressKeyBase { get; set; }
        public string NameFull { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string RecordExtras { get; set; }
        public string RecordID { get; set; }
        public string Reserved { get; set; }
        public string Results { get; set; }
        public string State { get; set; }
    }

    public class VerifiedHomeOwnerRoot
    {
        public List<VerifiedRecord> Records { get; set; }
        public string TotalRecords { get; set; }
        public string TransmissionReference { get; set; }
        public string TransmissionResults { get; set; }
        public string Version { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Parcel
    {
        public string FIPSCode { get; set; }
        public string County { get; set; }
        public string UnformattedAPN { get; set; }
        public string FormattedAPN { get; set; }
        public string AlternateAPN { get; set; }
        public string APNYearChange { get; set; }
        public string PreviousAPN { get; set; }
        public string AccountNumber { get; set; }
        public string YearAdded { get; set; }
        public string MapBook { get; set; }
        public string MapPage { get; set; }
        public string TaxJurisdiction { get; set; }
        public string CBSAName { get; set; }
        public string CBSACode { get; set; }
        public string MSAName { get; set; }
        public string MSACode { get; set; }
        public string MetropolitanDivision { get; set; }
        public string MinorCivilDivisionCode { get; set; }
        public string NeighborhoodCode { get; set; }
        public string CensusTract { get; set; }
        public string CensusBlockGrp { get; set; }
        public string CensusBlock { get; set; }
    }

    public class Legal
    {
        public string LegalDescription { get; set; }
        public string Range { get; set; }
        public string Township { get; set; }
        public string Section { get; set; }
        public string Quarter { get; set; }
        public string QuarterQuarter { get; set; }
        public string Subdivision { get; set; }
        public string Phase { get; set; }
        public string TractNumber { get; set; }
        public string Block1 { get; set; }
        public string Block2 { get; set; }
        public string LotNumber1 { get; set; }
        public string LotNumber2 { get; set; }
        public string LotNumber3 { get; set; }
        public string Unit { get; set; }
    }

    public class PropertyAddress
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CarrierRoute { get; set; }
        public string AddressKey { get; set; }
        public string MAK { get; set; }
        public string BaseMAK { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PrivacyInfo { get; set; }
    }

    public class ParsedPropertyAddress
    {
        public string Range { get; set; }
        public string PreDirectional { get; set; }
        public string StreetName { get; set; }
        public string Suffix { get; set; }
        public string PostDirectional { get; set; }
        public string SuiteName { get; set; }
        public string SuiteRange { get; set; }
    }

    public class PrimaryOwner
    {
        public string Name1Full { get; set; }
        public string Name1First { get; set; }
        public string Name1Middle { get; set; }
        public string Name1Last { get; set; }
        public string Name1Suffix { get; set; }
        public string TrustFlag { get; set; }
        public string CompanyFlag { get; set; }
        public string Name2Full { get; set; }
        public string Name2First { get; set; }
        public string Name2Middle { get; set; }
        public string Name2Last { get; set; }
        public string Name2Suffix { get; set; }
        public string Type { get; set; }
        public string VestingType { get; set; }
    }

    public class SecondaryOwner
    {
        public string Name3Full { get; set; }
        public string Name3First { get; set; }
        public string Name3Middle { get; set; }
        public string Name3Last { get; set; }
        public string Name3Suffix { get; set; }
        public string Name4Full { get; set; }
        public string Name4First { get; set; }
        public string Name4Middle { get; set; }
        public string Name4Last { get; set; }
        public string Name4Suffix { get; set; }
        public string Type { get; set; }
    }

    public class OwnerAddress
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CarrierRoute { get; set; }
        public string MAK { get; set; }
        public string BaseMAK { get; set; }
        public string FormatInfo { get; set; }
        public string PrivacyInfo { get; set; }
        public string OwnerOccupied { get; set; }
    }

    public class LastDeedOwnerInfo
    {
        public string Name1Full { get; set; }
        public string Name1First { get; set; }
        public string Name1Middle { get; set; }
        public string Name1Last { get; set; }
        public string Name1Suffix { get; set; }
        public string Name2Full { get; set; }
        public string Name2First { get; set; }
        public string Name2Middle { get; set; }
        public string Name2Last { get; set; }
        public string Name2Suffix { get; set; }
        public string Name3Full { get; set; }
        public string Name3First { get; set; }
        public string Name3Middle { get; set; }
        public string Name3Last { get; set; }
        public string Name3Suffix { get; set; }
        public string Name4Full { get; set; }
        public string Name4First { get; set; }
        public string Name4Middle { get; set; }
        public string Name4Last { get; set; }
        public string Name4Suffix { get; set; }
    }

    public class CurrentDeed
    {
        public string MortgageAmount { get; set; }
        public string MortgageDate { get; set; }
        public string MortgageLoanTypeCode { get; set; }
        public string MortgageTermCode { get; set; }
        public string MortgageTerm { get; set; }
        public string MortgageDueDate { get; set; }
        public string LenderCode { get; set; }
        public string LenderName { get; set; }
        public string SecondMortgageAmount { get; set; }
        public string SecondMortgageLoanTypeCode { get; set; }
    }

    public class Tax
    {
        public string YearAssessed { get; set; }
        public string AssessedValueTotal { get; set; }
        public string AssessedValueImprovements { get; set; }
        public string AssessedValueLand { get; set; }
        public string AssessedImprovementsPerc { get; set; }
        public string PreviousAssessedValue { get; set; }
        public string MarketValueYear { get; set; }
        public string MarketValueTotal { get; set; }
        public string MarketValueImprovements { get; set; }
        public string MarketValueLand { get; set; }
        public string MarketImprovementsPerc { get; set; }
        public string TaxFiscalYear { get; set; }
        public string TaxRateArea { get; set; }
        public string TaxBilledAmount { get; set; }
        public string TaxDelinquentYear { get; set; }
        public string LastTaxRollUpdate { get; set; }
        public string AssrLastUpdated { get; set; }
        public string TaxExemptionHomeowner { get; set; }
        public string TaxExemptionDisabled { get; set; }
        public string TaxExemptionSenior { get; set; }
        public string TaxExemptionVeteran { get; set; }
        public string TaxExemptionWidow { get; set; }
        public string TaxExemptionAdditional { get; set; }
    }

    public class PropertyUseInfo
    {
        public string YearBuilt { get; set; }
        public string YearBuiltEffective { get; set; }
        public string ZonedCodeLocal { get; set; }
        public string PropertyUseMuni { get; set; }
        public string PropertyUseGroup { get; set; }
        public string PropertyUseStandardized { get; set; }
    }

    public class SaleInfo
    {
        public string AssessorLastSaleDate { get; set; }
        public string AssessorLastSaleAmount { get; set; }
        public string AssessorPriorSaleDate { get; set; }
        public string AssessorPriorSaleAmount { get; set; }
        public string LastOwnershipTransferDate { get; set; }
        public string LastOwnershipTransferDocumentNumber { get; set; }
        public string LastOwnershipTransferTxID { get; set; }
        public string DeedLastSaleDocumentBook { get; set; }
        public string DeedLastSaleDocumentPage { get; set; }
        public string DeedLastDocumentNumber { get; set; }
        public string DeedLastSaleDate { get; set; }
        public string DeedLastSalePrice { get; set; }
        public string DeedLastSaleTxID { get; set; }
    }

    public class PropertySize
    {
        public string AreaBuilding { get; set; }
        public string AreaBuildingDefinitionCode { get; set; }
        public string AreaGross { get; set; }
        public string Area1stFloor { get; set; }
        public string Area2ndFloor { get; set; }
        public string AreaUpperFloors { get; set; }
        public string AreaLotAcres { get; set; }
        public string AreaLotSF { get; set; }
        public string LotDepth { get; set; }
        public string LotWidth { get; set; }
        public string AtticArea { get; set; }
        public string AtticFlag { get; set; }
        public string BasementArea { get; set; }
        public string BasementAreaFinished { get; set; }
        public string BasementAreaUnfinished { get; set; }
        public string ParkingGarage { get; set; }
        public string ParkingGarageArea { get; set; }
        public string ParkingCarport { get; set; }
        public string ParkingCarportArea { get; set; }
    }

    public class OwnerPool
    {
        public string Pool { get; set; }
        public string PoolArea { get; set; }
        public string SaunaFlag { get; set; }
    }

    public class IntStructInfo
    {
        public string Foundation { get; set; }
        public string Construction { get; set; }
        public string InteriorStructure { get; set; }
        public string PlumbingFixturesCount { get; set; }
        public string ConstructionFireResistanceClass { get; set; }
        public string SafetyFireSprinklersFlag { get; set; }
        public string FlooringMaterialPrimary { get; set; }
    }

    public class IntRoomInfo
    {
        public string BathCount { get; set; }
        public string BathPartialCount { get; set; }
        public string BedroomsCount { get; set; }
        public string RoomsCount { get; set; }
        public string StoriesCount { get; set; }
        public string UnitsCount { get; set; }
        public string BonusRoomFlag { get; set; }
        public string BreakfastNookFlag { get; set; }
        public string CellarFlag { get; set; }
        public string CellarWineFlag { get; set; }
        public string ExcerciseFlag { get; set; }
        public string FamilyCode { get; set; }
        public string GameFlag { get; set; }
        public string GreatFlag { get; set; }
        public string HobbyFlag { get; set; }
        public string LaundryFlag { get; set; }
        public string MediaFlag { get; set; }
        public string MudFlag { get; set; }
        public string OfficeArea { get; set; }
        public string OfficeFlag { get; set; }
        public string SafeRoomFlag { get; set; }
        public string SittingFlag { get; set; }
        public string StormShelter { get; set; }
        public string StudyFlag { get; set; }
        public string SunroomFlag { get; set; }
        public string UtilityArea { get; set; }
        public string UtilityCode { get; set; }
    }

    public class IntAmenities
    {
        public string Fireplace { get; set; }
        public string FireplaceCount { get; set; }
        public string AccessabilityElevatorFlag { get; set; }
        public string AccessabilityHandicapFlag { get; set; }
        public string EscalatorFlag { get; set; }
        public string CentralVacuumFlag { get; set; }
        public string IntercomFlag { get; set; }
        public string SoundSystemFlag { get; set; }
        public string WetBarFlag { get; set; }
        public string SecurityAlarmFlag { get; set; }
    }

    public class ExtStructInfo
    {
        public string StructureStyle { get; set; }
        public string Exterior1Code { get; set; }
        public string RoofMaterial { get; set; }
        public string RoofConstruction { get; set; }
        public string StormShutterFlag { get; set; }
        public string OverheadDoorFlag { get; set; }
    }

    public class ExtAmenities
    {
        public string ViewDescription { get; set; }
        public string PorchCode { get; set; }
        public string PorchArea { get; set; }
        public string PatioArea { get; set; }
        public string DeckFlag { get; set; }
        public string DeckArea { get; set; }
        public string FeatureBalconyFlag { get; set; }
        public string BalconyArea { get; set; }
        public string BreezewayFlag { get; set; }
    }

    public class ExtBuildings
    {
        public string BuildingsCount { get; set; }
        public string BathHouseArea { get; set; }
        public string BathHouseFlag { get; set; }
        public string BoatAccessFlag { get; set; }
        public string BoatHouseArea { get; set; }
        public string BoatHouseFlag { get; set; }
        public string CabinArea { get; set; }
        public string CabinFlag { get; set; }
        public string CanopyArea { get; set; }
        public string CanopyFlag { get; set; }
        public string GazeboArea { get; set; }
        public string GazeboFlag { get; set; }
        public string GranaryArea { get; set; }
        public string GranaryFlag { get; set; }
        public string GreenHouseArea { get; set; }
        public string GreenHouseFlag { get; set; }
        public string GuestHouseArea { get; set; }
        public string GuestHouseFlag { get; set; }
        public string KennelArea { get; set; }
        public string KennelFlag { get; set; }
        public string LeanToArea { get; set; }
        public string LeanToFlag { get; set; }
        public string LoadingPlatformArea { get; set; }
        public string LoadingPlatformFlag { get; set; }
        public string MilkHouseArea { get; set; }
        public string MilkHouseFlag { get; set; }
        public string OutdoorKitchenFireplaceFlag { get; set; }
        public string PoolHouseArea { get; set; }
        public string PoolHouseFlag { get; set; }
        public string PoultryHouseArea { get; set; }
        public string PoultryHouseFlag { get; set; }
        public string QuonsetArea { get; set; }
        public string QuonsetFlag { get; set; }
        public string ShedArea { get; set; }
        public string ShedCode { get; set; }
        public string SiloArea { get; set; }
        public string SiloFlag { get; set; }
        public string StableArea { get; set; }
        public string StableFlag { get; set; }
        public string StorageBuildingArea { get; set; }
        public string StorageBuildingFlag { get; set; }
        public string UtilityBuildingArea { get; set; }
        public string UtilityBuildingFlag { get; set; }
        public string PoleStructureArea { get; set; }
        public string PoleStructureFlag { get; set; }
    }

    public class Utilities
    {
        public string HVACCoolingDetail { get; set; }
        public string HVACHeatingDetail { get; set; }
        public string HVACHeatingFuel { get; set; }
        public string SewageUsage { get; set; }
        public string WaterSource { get; set; }
        public string MobileHomeHookupFlag { get; set; }
    }

    public class Parking
    {
        public string RVParkingFlag { get; set; }
        public string ParkingSpaceCount { get; set; }
        public string DrivewayArea { get; set; }
        public string DrivewayMaterial { get; set; }
    }

    public class YardGardenInfo
    {
        public string TopographyCode { get; set; }
        public string FenceCode { get; set; }
        public string FenceArea { get; set; }
        public string CourtyardFlag { get; set; }
        public string CourtyardArea { get; set; }
        public string ArborPergolaFlag { get; set; }
        public string SprinklersFlag { get; set; }
        public string GolfCourseGreenFlag { get; set; }
        public string TennisCourtFlag { get; set; }
        public string SportsCourtFlag { get; set; }
        public string ArenaFlag { get; set; }
        public string WaterFeatureFlag { get; set; }
        public string PondFlag { get; set; }
        public string BoatLiftFlag { get; set; }
    }

    public class OwnerEstimatedValue
    {
        public string EstimatedValue { get; set; }
        public string EstimatedMinValue { get; set; }
        public string EstimatedMaxValue { get; set; }
        public string ConfidenceScore { get; set; }
        public string ValuationDate { get; set; }
    }

    public class Shape
    {
        public string WellKnownText { get; set; }
    }

    public class AssociatedParcel
    {
        public string FIPS { get; set; }
        public string APN { get; set; }
        public string MAK { get; set; }
    }

    public class OwnerRecord
    {
        public string Results { get; set; }
        public Parcel Parcel { get; set; }
        public Legal Legal { get; set; }
        public PropertyAddress PropertyAddress { get; set; }
        public ParsedPropertyAddress ParsedPropertyAddress { get; set; }
        public PrimaryOwner PrimaryOwner { get; set; }
        public SecondaryOwner SecondaryOwner { get; set; }
        public OwnerAddress OwnerAddress { get; set; }
        public LastDeedOwnerInfo LastDeedOwnerInfo { get; set; }
        public CurrentDeed CurrentDeed { get; set; }
        public Tax Tax { get; set; }
        public PropertyUseInfo PropertyUseInfo { get; set; }
        public SaleInfo SaleInfo { get; set; }
        public PropertySize PropertySize { get; set; }
        public OwnerPool Pool { get; set; }
        public IntStructInfo IntStructInfo { get; set; }
        public IntRoomInfo IntRoomInfo { get; set; }
        public IntAmenities IntAmenities { get; set; }
        public ExtStructInfo ExtStructInfo { get; set; }
        public ExtAmenities ExtAmenities { get; set; }
        public ExtBuildings ExtBuildings { get; set; }
        public Utilities Utilities { get; set; }
        public Parking Parking { get; set; }
        public YardGardenInfo YardGardenInfo { get; set; }
        public OwnerEstimatedValue EstimatedValue { get; set; }
        public Shape Shape { get; set; }
        public List<AssociatedParcel> AssociatedParcels { get; set; }
    }

    public class OwnerRoot
    {
        public string Version { get; set; }
        public string TransmissionReference { get; set; }
        public string TransmissionResults { get; set; }
        public int TotalRecords { get; set; }
        public List<OwnerRecord> Records { get; set; }
    }


}
