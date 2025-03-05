using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class CustomerInspectionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERINSPECTION = "InsertCustomerInspection";
		private const string UPDATECUSTOMERINSPECTION = "UpdateCustomerInspection";
		private const string DELETECUSTOMERINSPECTION = "DeleteCustomerInspection";
		private const string GETCUSTOMERINSPECTIONBYID = "GetCustomerInspectionById";
		private const string GETALLCUSTOMERINSPECTION = "GetAllCustomerInspection";
		private const string GETPAGEDCUSTOMERINSPECTION = "GetPagedCustomerInspection";
		private const string GETCUSTOMERINSPECTIONMAXIMUMID = "GetCustomerInspectionMaximumId";
		private const string GETCUSTOMERINSPECTIONROWCOUNT = "GetCustomerInspectionRowCount";	
		private const string GETCUSTOMERINSPECTIONBYQUERY = "GetCustomerInspectionByQuery";
		#endregion
		
		#region Constructors
		public CustomerInspectionDataAccess(ClientContext context) : base(context) { }
		public CustomerInspectionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerInspectionObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerInspectionBase customerInspectionObject)
		{	
			AddParameter(cmd, pGuid(CustomerInspectionBase.Property_CompanyId, customerInspectionObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerInspectionBase.Property_CustomerId, customerInspectionObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_CurrentOutsideConditions, 100, customerInspectionObject.CurrentOutsideConditions));
			AddParameter(cmd, pDouble(CustomerInspectionBase.Property_OutsideRelativeHumidity, customerInspectionObject.OutsideRelativeHumidity));
			AddParameter(cmd, pDouble(CustomerInspectionBase.Property_OutsideTemperature, customerInspectionObject.OutsideTemperature));
			AddParameter(cmd, pDouble(CustomerInspectionBase.Property_FirstFloorRelativeHumidity, customerInspectionObject.FirstFloorRelativeHumidity));
			AddParameter(cmd, pDouble(CustomerInspectionBase.Property_FirstFloorTemperature, customerInspectionObject.FirstFloorTemperature));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_RelativeOther1, customerInspectionObject.RelativeOther1));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_RelativeOther2, customerInspectionObject.RelativeOther2));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_Heat, 50, customerInspectionObject.Heat));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_Air, 50, customerInspectionObject.Air));
			AddParameter(cmd, pDouble(CustomerInspectionBase.Property_BasementRelativeHumidity, customerInspectionObject.BasementRelativeHumidity));
			AddParameter(cmd, pDouble(CustomerInspectionBase.Property_BasementTemperature, customerInspectionObject.BasementTemperature));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_BasementDehumidifier, 100, customerInspectionObject.BasementDehumidifier));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_GroundWater, 25, customerInspectionObject.GroundWater));
			AddParameter(cmd, pInt32(CustomerInspectionBase.Property_GroundWaterRating, customerInspectionObject.GroundWaterRating));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_IronBacteria, 25, customerInspectionObject.IronBacteria));
			AddParameter(cmd, pInt32(CustomerInspectionBase.Property_IronBacteriaRating, customerInspectionObject.IronBacteriaRating));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_Condensation, 25, customerInspectionObject.Condensation));
			AddParameter(cmd, pInt32(CustomerInspectionBase.Property_CondensationRating, customerInspectionObject.CondensationRating));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_WallCracks, 25, customerInspectionObject.WallCracks));
			AddParameter(cmd, pInt32(CustomerInspectionBase.Property_WallCracksRating, customerInspectionObject.WallCracksRating));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_FloorCracks, 25, customerInspectionObject.FloorCracks));
			AddParameter(cmd, pInt32(CustomerInspectionBase.Property_FloorCracksRating, customerInspectionObject.FloorCracksRating));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_ExistingSumpPump, 25, customerInspectionObject.ExistingSumpPump));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_ExistingDrainageSystem, 25, customerInspectionObject.ExistingDrainageSystem));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_ExistingRadonSystem, 25, customerInspectionObject.ExistingRadonSystem));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_DryerVentToCode, 25, customerInspectionObject.DryerVentToCode));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_FoundationType, 50, customerInspectionObject.FoundationType));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_Bulkhead, 25, customerInspectionObject.Bulkhead));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_VisualBasementOther, customerInspectionObject.VisualBasementOther));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_NoticedSmellsOrOdors, 25, customerInspectionObject.NoticedSmellsOrOdors));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_NoticedSmellsOrOdorsComment, 100, customerInspectionObject.NoticedSmellsOrOdorsComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_NoticedMoldOrMildew, 25, customerInspectionObject.NoticedMoldOrMildew));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_NoticedMoldOrMildewComment, 100, customerInspectionObject.NoticedMoldOrMildewComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_BasementGoDown, 50, customerInspectionObject.BasementGoDown));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_HomeSufferForRespiratory, 25, customerInspectionObject.HomeSufferForRespiratory));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_HomeSufferForrespiratoryComment, 100, customerInspectionObject.HomeSufferForrespiratoryComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_ChildrenPlayInBasement, 25, customerInspectionObject.ChildrenPlayInBasement));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_ChildrenPlayInBasementComment, 100, customerInspectionObject.ChildrenPlayInBasementComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_PetsGoInBasement, 25, customerInspectionObject.PetsGoInBasement));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_PetsGoInBasementComment, 100, customerInspectionObject.PetsGoInBasementComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_NoticedBugsOrRodents, 25, customerInspectionObject.NoticedBugsOrRodents));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_NoticedBugsOrRodentsComment, 100, customerInspectionObject.NoticedBugsOrRodentsComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_GetWater, 25, customerInspectionObject.GetWater));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_GetWaterComment, 100, customerInspectionObject.GetWaterComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_RemoveWater, 50, customerInspectionObject.RemoveWater));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_SeeCondensationPipesDripping, 25, customerInspectionObject.SeeCondensationPipesDripping));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_SeeCondensationPipesDrippingComment, 100, customerInspectionObject.SeeCondensationPipesDrippingComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_RepairsProblems, 25, customerInspectionObject.RepairsProblems));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_RepairsProblemsComment, 100, customerInspectionObject.RepairsProblemsComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_LivingPlan, 25, customerInspectionObject.LivingPlan));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_SellPlaning, 25, customerInspectionObject.SellPlaning));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_PlansForBasementOnce, 50, customerInspectionObject.PlansForBasementOnce));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_HomeTestForPastRadon, 25, customerInspectionObject.HomeTestForPastRadon));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_HomeTestForPastRadonComment, 100, customerInspectionObject.HomeTestForPastRadonComment));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_LosePower, 50, customerInspectionObject.LosePower));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_LosePowerHowOften, 50, customerInspectionObject.LosePowerHowOften));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_CustomerBasementOther, customerInspectionObject.CustomerBasementOther));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_Drawing, customerInspectionObject.Drawing));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_Notes, customerInspectionObject.Notes));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_PMSignature, customerInspectionObject.PMSignature));
			AddParameter(cmd, pDateTime(CustomerInspectionBase.Property_PMSignatureDate, customerInspectionObject.PMSignatureDate));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_HomeOwnerSignature, customerInspectionObject.HomeOwnerSignature));
			AddParameter(cmd, pDateTime(CustomerInspectionBase.Property_HomeOwnerSignatureDate, customerInspectionObject.HomeOwnerSignatureDate));
			AddParameter(cmd, pGuid(CustomerInspectionBase.Property_CreatedBy, customerInspectionObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomerInspectionBase.Property_CreatedDate, customerInspectionObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerInspectionBase.Property_LastUpdatedBy, customerInspectionObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(CustomerInspectionBase.Property_LastUpdatedDate, customerInspectionObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(CustomerInspectionBase.Property_InspectionPhoto, customerInspectionObject.InspectionPhoto));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerInspection
        /// </summary>
        /// <param name="customerInspectionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerInspectionBase customerInspectionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERINSPECTION);
	
				AddParameter(cmd, pInt32Out(CustomerInspectionBase.Property_Id));
				AddCommonParams(cmd, customerInspectionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerInspectionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerInspectionObject.Id = (Int32)GetOutParameter(cmd, CustomerInspectionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerInspectionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerInspection
        /// </summary>
        /// <param name="customerInspectionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerInspectionBase customerInspectionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERINSPECTION);
				
				AddParameter(cmd, pInt32(CustomerInspectionBase.Property_Id, customerInspectionObject.Id));
				AddCommonParams(cmd, customerInspectionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerInspectionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerInspectionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerInspection
        /// </summary>
        /// <param name="Id">Id of the CustomerInspection object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERINSPECTION);	
				
				AddParameter(cmd, pInt32(CustomerInspectionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerInspection), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerInspection object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerInspection object to retrieve</param>
        /// <returns>CustomerInspection object, null if not found</returns>
		public CustomerInspection Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERINSPECTIONBYID))
			{
				AddParameter( cmd, pInt32(CustomerInspectionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerInspection objects 
        /// </summary>
        /// <returns>A list of CustomerInspection objects</returns>
		public CustomerInspectionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERINSPECTION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerInspection objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerInspection objects</returns>
		public CustomerInspectionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERINSPECTION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerInspectionList _CustomerInspectionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerInspectionList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerInspection objects by query String
        /// </summary>
        /// <returns>A list of CustomerInspection objects</returns>
		public CustomerInspectionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERINSPECTIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerInspection Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerInspection
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERINSPECTIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerInspection Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerInspection
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerInspectionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERINSPECTIONROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerInspectionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerInspectionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerInspection object
        /// </summary>
        /// <param name="customerInspectionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerInspectionBase customerInspectionObject, SqlDataReader reader, int start)
		{
			
				customerInspectionObject.Id = reader.GetInt32( start + 0 );			
				customerInspectionObject.CompanyId = reader.GetGuid( start + 1 );			
				customerInspectionObject.CustomerId = reader.GetGuid( start + 2 );			
				customerInspectionObject.CurrentOutsideConditions = reader.GetString( start + 3 );			
				customerInspectionObject.OutsideRelativeHumidity = reader.GetDouble( start + 4 );			
				customerInspectionObject.OutsideTemperature = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) customerInspectionObject.FirstFloorRelativeHumidity = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) customerInspectionObject.FirstFloorTemperature = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) customerInspectionObject.RelativeOther1 = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerInspectionObject.RelativeOther2 = reader.GetString( start + 9 );			
				customerInspectionObject.Heat = reader.GetString( start + 10 );			
				customerInspectionObject.Air = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerInspectionObject.BasementRelativeHumidity = reader.GetDouble( start + 12 );			
				if(!reader.IsDBNull(13)) customerInspectionObject.BasementTemperature = reader.GetDouble( start + 13 );			
				customerInspectionObject.BasementDehumidifier = reader.GetString( start + 14 );			
				customerInspectionObject.GroundWater = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerInspectionObject.GroundWaterRating = reader.GetInt32( start + 16 );			
				customerInspectionObject.IronBacteria = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerInspectionObject.IronBacteriaRating = reader.GetInt32( start + 18 );			
				customerInspectionObject.Condensation = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) customerInspectionObject.CondensationRating = reader.GetInt32( start + 20 );			
				customerInspectionObject.WallCracks = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) customerInspectionObject.WallCracksRating = reader.GetInt32( start + 22 );			
				customerInspectionObject.FloorCracks = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerInspectionObject.FloorCracksRating = reader.GetInt32( start + 24 );			
				customerInspectionObject.ExistingSumpPump = reader.GetString( start + 25 );			
				customerInspectionObject.ExistingDrainageSystem = reader.GetString( start + 26 );			
				customerInspectionObject.ExistingRadonSystem = reader.GetString( start + 27 );			
				customerInspectionObject.DryerVentToCode = reader.GetString( start + 28 );			
				customerInspectionObject.FoundationType = reader.GetString( start + 29 );			
				customerInspectionObject.Bulkhead = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) customerInspectionObject.VisualBasementOther = reader.GetString( start + 31 );			
				customerInspectionObject.NoticedSmellsOrOdors = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) customerInspectionObject.NoticedSmellsOrOdorsComment = reader.GetString( start + 33 );			
				customerInspectionObject.NoticedMoldOrMildew = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) customerInspectionObject.NoticedMoldOrMildewComment = reader.GetString( start + 35 );			
				customerInspectionObject.BasementGoDown = reader.GetString( start + 36 );			
				customerInspectionObject.HomeSufferForRespiratory = reader.GetString( start + 37 );			
				if(!reader.IsDBNull(38)) customerInspectionObject.HomeSufferForrespiratoryComment = reader.GetString( start + 38 );			
				customerInspectionObject.ChildrenPlayInBasement = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) customerInspectionObject.ChildrenPlayInBasementComment = reader.GetString( start + 40 );			
				customerInspectionObject.PetsGoInBasement = reader.GetString( start + 41 );			
				if(!reader.IsDBNull(42)) customerInspectionObject.PetsGoInBasementComment = reader.GetString( start + 42 );			
				customerInspectionObject.NoticedBugsOrRodents = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) customerInspectionObject.NoticedBugsOrRodentsComment = reader.GetString( start + 44 );			
				customerInspectionObject.GetWater = reader.GetString( start + 45 );			
				if(!reader.IsDBNull(46)) customerInspectionObject.GetWaterComment = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) customerInspectionObject.RemoveWater = reader.GetString( start + 47 );			
				customerInspectionObject.SeeCondensationPipesDripping = reader.GetString( start + 48 );			
				if(!reader.IsDBNull(49)) customerInspectionObject.SeeCondensationPipesDrippingComment = reader.GetString( start + 49 );			
				customerInspectionObject.RepairsProblems = reader.GetString( start + 50 );			
				if(!reader.IsDBNull(51)) customerInspectionObject.RepairsProblemsComment = reader.GetString( start + 51 );			
				customerInspectionObject.LivingPlan = reader.GetString( start + 52 );			
				customerInspectionObject.SellPlaning = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) customerInspectionObject.PlansForBasementOnce = reader.GetString( start + 54 );			
				customerInspectionObject.HomeTestForPastRadon = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) customerInspectionObject.HomeTestForPastRadonComment = reader.GetString( start + 56 );			
				customerInspectionObject.LosePower = reader.GetString( start + 57 );			
				customerInspectionObject.LosePowerHowOften = reader.GetString( start + 58 );			
				if(!reader.IsDBNull(59)) customerInspectionObject.CustomerBasementOther = reader.GetString( start + 59 );			
				if(!reader.IsDBNull(60)) customerInspectionObject.Drawing = reader.GetString( start + 60 );			
				if(!reader.IsDBNull(61)) customerInspectionObject.Notes = reader.GetString( start + 61 );			
				if(!reader.IsDBNull(62)) customerInspectionObject.PMSignature = reader.GetString( start + 62 );			
				if(!reader.IsDBNull(63)) customerInspectionObject.PMSignatureDate = reader.GetDateTime( start + 63 );			
				if(!reader.IsDBNull(64)) customerInspectionObject.HomeOwnerSignature = reader.GetString( start + 64 );			
				if(!reader.IsDBNull(65)) customerInspectionObject.HomeOwnerSignatureDate = reader.GetDateTime( start + 65 );			
				customerInspectionObject.CreatedBy = reader.GetGuid( start + 66 );			
				customerInspectionObject.CreatedDate = reader.GetDateTime( start + 67 );			
				customerInspectionObject.LastUpdatedBy = reader.GetGuid( start + 68 );			
				customerInspectionObject.LastUpdatedDate = reader.GetDateTime( start + 69 );			
				if(!reader.IsDBNull(70)) customerInspectionObject.InspectionPhoto = reader.GetString( start + 70 );			
			FillBaseObject(customerInspectionObject, reader, (start + 71));

			
			customerInspectionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerInspection object
        /// </summary>
        /// <param name="customerInspectionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerInspectionBase customerInspectionObject, SqlDataReader reader)
		{
			FillObject(customerInspectionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerInspection object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerInspection object</returns>
		private CustomerInspection GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerInspection customerInspectionObject= new CustomerInspection();
					FillObject(customerInspectionObject, reader);
					return customerInspectionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerInspection objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerInspection objects</returns>
		private CustomerInspectionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerInspection list
			CustomerInspectionList list = new CustomerInspectionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerInspection customerInspectionObject = new CustomerInspection();
					FillObject(customerInspectionObject, reader);

					list.Add(customerInspectionObject);
				}
				
				// Close the reader in order to receive output parameters
				// Output parameters are not available until reader is closed.
				reader.Close();
			}

			return list;
		}
		
		#endregion
	}	
}
