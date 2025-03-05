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
	public partial class OpportunityDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTOPPORTUNITY = "InsertOpportunity";
		private const string UPDATEOPPORTUNITY = "UpdateOpportunity";
		private const string DELETEOPPORTUNITY = "DeleteOpportunity";
		private const string GETOPPORTUNITYBYID = "GetOpportunityById";
		private const string GETALLOPPORTUNITY = "GetAllOpportunity";
		private const string GETPAGEDOPPORTUNITY = "GetPagedOpportunity";
		private const string GETOPPORTUNITYMAXIMUMID = "GetOpportunityMaximumId";
		private const string GETOPPORTUNITYROWCOUNT = "GetOpportunityRowCount";	
		private const string GETOPPORTUNITYBYQUERY = "GetOpportunityByQuery";
		#endregion
		
		#region Constructors
		public OpportunityDataAccess(ClientContext context) : base(context) { }
		public OpportunityDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="opportunityObject"></param>
		private void AddCommonParams(SqlCommand cmd, OpportunityBase opportunityObject)
		{	
			AddParameter(cmd, pGuid(OpportunityBase.Property_OpportunityId, opportunityObject.OpportunityId));
			AddParameter(cmd, pGuid(OpportunityBase.Property_CustomerId, opportunityObject.CustomerId));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_OpportunityName, 500, opportunityObject.OpportunityName));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Type, 50, opportunityObject.Type));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_LeadSource, 50, opportunityObject.LeadSource));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Revenue, 50, opportunityObject.Revenue));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_ProjectedGP, 50, opportunityObject.ProjectedGP));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Points, 50, opportunityObject.Points));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_TotalProjectedGP, 50, opportunityObject.TotalProjectedGP));
			AddParameter(cmd, pDateTime(OpportunityBase.Property_CloseDate, opportunityObject.CloseDate));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Status, 50, opportunityObject.Status));
			AddParameter(cmd, pInt32(OpportunityBase.Property_Probability, opportunityObject.Probability));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_DealReason, 500, opportunityObject.DealReason));
			AddParameter(cmd, pBool(OpportunityBase.Property_IsForecast, opportunityObject.IsForecast));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_DeliveryDays, 50, opportunityObject.DeliveryDays));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Competitors, 250, opportunityObject.Competitors));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_CampaignSource, 50, opportunityObject.CampaignSource));
			AddParameter(cmd, pGuid(OpportunityBase.Property_AccountOwner, opportunityObject.AccountOwner));
			AddParameter(cmd, pGuid(OpportunityBase.Property_CreatedBy, opportunityObject.CreatedBy));
			AddParameter(cmd, pDateTime(OpportunityBase.Property_CreatedDate, opportunityObject.CreatedDate));
			AddParameter(cmd, pDateTime(OpportunityBase.Property_LastUpdatedDate, opportunityObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Market, 50, opportunityObject.Market));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Used, 50, opportunityObject.Used));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_YearModel, 50, opportunityObject.YearModel));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Make, 50, opportunityObject.Make));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Model, 50, opportunityObject.Model));
			AddParameter(cmd, pInt32(OpportunityBase.Property_Capacity, opportunityObject.Capacity));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_Note, opportunityObject.Note));
			AddParameter(cmd, pGuid(OpportunityBase.Property_AccessGivenTo, opportunityObject.AccessGivenTo));
			AddParameter(cmd, pNVarChar(OpportunityBase.Property_VehicleCondition, 100, opportunityObject.VehicleCondition));
			AddParameter(cmd, pDateTime(OpportunityBase.Property_CloseDateSetDate, opportunityObject.CloseDateSetDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Opportunity
        /// </summary>
        /// <param name="opportunityObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(OpportunityBase opportunityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTOPPORTUNITY);
	
				AddParameter(cmd, pInt32Out(OpportunityBase.Property_Id));
				AddCommonParams(cmd, opportunityObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					opportunityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					opportunityObject.Id = (Int32)GetOutParameter(cmd, OpportunityBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(opportunityObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Opportunity
        /// </summary>
        /// <param name="opportunityObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(OpportunityBase opportunityObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEOPPORTUNITY);
				
				AddParameter(cmd, pInt32(OpportunityBase.Property_Id, opportunityObject.Id));
				AddCommonParams(cmd, opportunityObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					opportunityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(opportunityObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Opportunity
        /// </summary>
        /// <param name="Id">Id of the Opportunity object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEOPPORTUNITY);	
				
				AddParameter(cmd, pInt32(OpportunityBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Opportunity), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Opportunity object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Opportunity object to retrieve</param>
        /// <returns>Opportunity object, null if not found</returns>
		public Opportunity Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETOPPORTUNITYBYID))
			{
				AddParameter( cmd, pInt32(OpportunityBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Opportunity objects 
        /// </summary>
        /// <returns>A list of Opportunity objects</returns>
		public OpportunityList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLOPPORTUNITY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Opportunity objects by PageRequest
        /// </summary>
        /// <returns>A list of Opportunity objects</returns>
		public OpportunityList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDOPPORTUNITY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				OpportunityList _OpportunityList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _OpportunityList;
			}
		}
		
		/// <summary>
        /// Retrieves all Opportunity objects by query String
        /// </summary>
        /// <returns>A list of Opportunity objects</returns>
		public OpportunityList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETOPPORTUNITYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Opportunity Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Opportunity
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETOPPORTUNITYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Opportunity Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Opportunity
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _OpportunityRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETOPPORTUNITYROWCOUNT))
			{
				SqlDataReader reader;
				_OpportunityRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _OpportunityRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Opportunity object
        /// </summary>
        /// <param name="opportunityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(OpportunityBase opportunityObject, SqlDataReader reader, int start)
		{
			
				opportunityObject.Id = reader.GetInt32( start + 0 );			
				opportunityObject.OpportunityId = reader.GetGuid( start + 1 );			
				opportunityObject.CustomerId = reader.GetGuid( start + 2 );			
				opportunityObject.OpportunityName = reader.GetString( start + 3 );			
				opportunityObject.Type = reader.GetString( start + 4 );			
				opportunityObject.LeadSource = reader.GetString( start + 5 );			
				opportunityObject.Revenue = reader.GetString( start + 6 );			
				opportunityObject.ProjectedGP = reader.GetString( start + 7 );			
				opportunityObject.Points = reader.GetString( start + 8 );			
				opportunityObject.TotalProjectedGP = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) opportunityObject.CloseDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) opportunityObject.Status = reader.GetString( start + 11 );			
				opportunityObject.Probability = reader.GetInt32( start + 12 );			
				opportunityObject.DealReason = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) opportunityObject.IsForecast = reader.GetBoolean( start + 14 );			
				opportunityObject.DeliveryDays = reader.GetString( start + 15 );			
				opportunityObject.Competitors = reader.GetString( start + 16 );			
				opportunityObject.CampaignSource = reader.GetString( start + 17 );			
				opportunityObject.AccountOwner = reader.GetGuid( start + 18 );			
				opportunityObject.CreatedBy = reader.GetGuid( start + 19 );			
				opportunityObject.CreatedDate = reader.GetDateTime( start + 20 );			
				opportunityObject.LastUpdatedDate = reader.GetDateTime( start + 21 );			
				if(!reader.IsDBNull(22)) opportunityObject.Market = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) opportunityObject.Used = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) opportunityObject.YearModel = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) opportunityObject.Make = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) opportunityObject.Model = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) opportunityObject.Capacity = reader.GetInt32( start + 27 );			
				if(!reader.IsDBNull(28)) opportunityObject.Note = reader.GetString( start + 28 );			
				opportunityObject.AccessGivenTo = reader.GetGuid( start + 29 );			
				if(!reader.IsDBNull(30)) opportunityObject.VehicleCondition = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) opportunityObject.CloseDateSetDate = reader.GetDateTime( start + 31 );			
			FillBaseObject(opportunityObject, reader, (start + 32));

			
			opportunityObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Opportunity object
        /// </summary>
        /// <param name="opportunityObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(OpportunityBase opportunityObject, SqlDataReader reader)
		{
			FillObject(opportunityObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Opportunity object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Opportunity object</returns>
		private Opportunity GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Opportunity opportunityObject= new Opportunity();
					FillObject(opportunityObject, reader);
					return opportunityObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Opportunity objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Opportunity objects</returns>
		private OpportunityList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Opportunity list
			OpportunityList list = new OpportunityList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Opportunity opportunityObject = new Opportunity();
					FillObject(opportunityObject, reader);

					list.Add(opportunityObject);
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
