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
	public partial class EstimatorPDFFilterDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATORPDFFILTER = "InsertEstimatorPDFFilter";
		private const string UPDATEESTIMATORPDFFILTER = "UpdateEstimatorPDFFilter";
		private const string DELETEESTIMATORPDFFILTER = "DeleteEstimatorPDFFilter";
		private const string GETESTIMATORPDFFILTERBYID = "GetEstimatorPDFFilterById";
		private const string GETALLESTIMATORPDFFILTER = "GetAllEstimatorPDFFilter";
		private const string GETPAGEDESTIMATORPDFFILTER = "GetPagedEstimatorPDFFilter";
		private const string GETESTIMATORPDFFILTERMAXIMUMID = "GetEstimatorPDFFilterMaximumId";
		private const string GETESTIMATORPDFFILTERROWCOUNT = "GetEstimatorPDFFilterRowCount";	
		private const string GETESTIMATORPDFFILTERBYQUERY = "GetEstimatorPDFFilterByQuery";
		#endregion
		
		#region Constructors
		public EstimatorPDFFilterDataAccess(ClientContext context) : base(context) { }
		public EstimatorPDFFilterDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimatorPDFFilterObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimatorPDFFilterBase estimatorPDFFilterObject)
		{	
			AddParameter(cmd, pGuid(EstimatorPDFFilterBase.Property_CompanyId, estimatorPDFFilterObject.CompanyId));
			AddParameter(cmd, pGuid(EstimatorPDFFilterBase.Property_CustomerId, estimatorPDFFilterObject.CustomerId));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_GroupedbyNone, estimatorPDFFilterObject.GroupedbyNone));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_GroupedbyCategory, estimatorPDFFilterObject.GroupedbyCategory));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_GroupedbyLabor, estimatorPDFFilterObject.GroupedbyLabor));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_GroupedbyLaborAndMaterial, estimatorPDFFilterObject.GroupedbyLaborAndMaterial));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_GroupedbyMaterial, estimatorPDFFilterObject.GroupedbyMaterial));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_GroupedbySupplier, estimatorPDFFilterObject.GroupedbySupplier));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeCost, estimatorPDFFilterObject.IncludeCost));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeImage, estimatorPDFFilterObject.IncludeImage));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeManufacturer, estimatorPDFFilterObject.IncludeManufacturer));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeMargin, estimatorPDFFilterObject.IncludeMargin));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeOverhead, estimatorPDFFilterObject.IncludeOverhead));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludePDF, estimatorPDFFilterObject.IncludePDF));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeProfit, estimatorPDFFilterObject.IncludeProfit));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeService, estimatorPDFFilterObject.IncludeService));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_WithoutIndividualLaborPricing, estimatorPDFFilterObject.WithoutIndividualLaborPricing));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_WithoutIndividualMaterialPricing, estimatorPDFFilterObject.WithoutIndividualMaterialPricing));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_WithoutPricing, estimatorPDFFilterObject.WithoutPricing));
			AddParameter(cmd, pGuid(EstimatorPDFFilterBase.Property_CreatedBy, estimatorPDFFilterObject.CreatedBy));
			AddParameter(cmd, pDateTime(EstimatorPDFFilterBase.Property_CreatedDate, estimatorPDFFilterObject.CreatedDate));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_IncludeVariation, estimatorPDFFilterObject.IncludeVariation));
			AddParameter(cmd, pBool(EstimatorPDFFilterBase.Property_OneTimeService, estimatorPDFFilterObject.OneTimeService));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EstimatorPDFFilter
        /// </summary>
        /// <param name="estimatorPDFFilterObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimatorPDFFilterBase estimatorPDFFilterObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATORPDFFILTER);
	
				AddParameter(cmd, pInt32Out(EstimatorPDFFilterBase.Property_Id));
				AddCommonParams(cmd, estimatorPDFFilterObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimatorPDFFilterObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimatorPDFFilterObject.Id = (Int32)GetOutParameter(cmd, EstimatorPDFFilterBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimatorPDFFilterObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EstimatorPDFFilter
        /// </summary>
        /// <param name="estimatorPDFFilterObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimatorPDFFilterBase estimatorPDFFilterObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATORPDFFILTER);
				
				AddParameter(cmd, pInt32(EstimatorPDFFilterBase.Property_Id, estimatorPDFFilterObject.Id));
				AddCommonParams(cmd, estimatorPDFFilterObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimatorPDFFilterObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimatorPDFFilterObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EstimatorPDFFilter
        /// </summary>
        /// <param name="Id">Id of the EstimatorPDFFilter object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATORPDFFILTER);	
				
				AddParameter(cmd, pInt32(EstimatorPDFFilterBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EstimatorPDFFilter), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EstimatorPDFFilter object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EstimatorPDFFilter object to retrieve</param>
        /// <returns>EstimatorPDFFilter object, null if not found</returns>
		public EstimatorPDFFilter Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORPDFFILTERBYID))
			{
				AddParameter( cmd, pInt32(EstimatorPDFFilterBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EstimatorPDFFilter objects 
        /// </summary>
        /// <returns>A list of EstimatorPDFFilter objects</returns>
		public EstimatorPDFFilterList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATORPDFFILTER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EstimatorPDFFilter objects by PageRequest
        /// </summary>
        /// <returns>A list of EstimatorPDFFilter objects</returns>
		public EstimatorPDFFilterList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATORPDFFILTER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimatorPDFFilterList _EstimatorPDFFilterList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimatorPDFFilterList;
			}
		}
		
		/// <summary>
        /// Retrieves all EstimatorPDFFilter objects by query String
        /// </summary>
        /// <returns>A list of EstimatorPDFFilter objects</returns>
		public EstimatorPDFFilterList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORPDFFILTERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EstimatorPDFFilter Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EstimatorPDFFilter
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORPDFFILTERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EstimatorPDFFilter Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EstimatorPDFFilter
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimatorPDFFilterRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORPDFFILTERROWCOUNT))
			{
				SqlDataReader reader;
				_EstimatorPDFFilterRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimatorPDFFilterRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EstimatorPDFFilter object
        /// </summary>
        /// <param name="estimatorPDFFilterObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimatorPDFFilterBase estimatorPDFFilterObject, SqlDataReader reader, int start)
		{
			
				estimatorPDFFilterObject.Id = reader.GetInt32( start + 0 );			
				estimatorPDFFilterObject.CompanyId = reader.GetGuid( start + 1 );			
				estimatorPDFFilterObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) estimatorPDFFilterObject.GroupedbyNone = reader.GetBoolean( start + 3 );			
				if(!reader.IsDBNull(4)) estimatorPDFFilterObject.GroupedbyCategory = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) estimatorPDFFilterObject.GroupedbyLabor = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) estimatorPDFFilterObject.GroupedbyLaborAndMaterial = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) estimatorPDFFilterObject.GroupedbyMaterial = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) estimatorPDFFilterObject.GroupedbySupplier = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) estimatorPDFFilterObject.IncludeCost = reader.GetBoolean( start + 9 );			
				if(!reader.IsDBNull(10)) estimatorPDFFilterObject.IncludeImage = reader.GetBoolean( start + 10 );			
				if(!reader.IsDBNull(11)) estimatorPDFFilterObject.IncludeManufacturer = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) estimatorPDFFilterObject.IncludeMargin = reader.GetBoolean( start + 12 );			
				if(!reader.IsDBNull(13)) estimatorPDFFilterObject.IncludeOverhead = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) estimatorPDFFilterObject.IncludePDF = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) estimatorPDFFilterObject.IncludeProfit = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) estimatorPDFFilterObject.IncludeService = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) estimatorPDFFilterObject.WithoutIndividualLaborPricing = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) estimatorPDFFilterObject.WithoutIndividualMaterialPricing = reader.GetBoolean( start + 18 );			
				if(!reader.IsDBNull(19)) estimatorPDFFilterObject.WithoutPricing = reader.GetBoolean( start + 19 );			
				estimatorPDFFilterObject.CreatedBy = reader.GetGuid( start + 20 );			
				estimatorPDFFilterObject.CreatedDate = reader.GetDateTime( start + 21 );			
				if(!reader.IsDBNull(22)) estimatorPDFFilterObject.IncludeVariation = reader.GetBoolean( start + 22 );			
				if(!reader.IsDBNull(23)) estimatorPDFFilterObject.OneTimeService = reader.GetBoolean( start + 23 );			
			FillBaseObject(estimatorPDFFilterObject, reader, (start + 24));

			
			estimatorPDFFilterObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EstimatorPDFFilter object
        /// </summary>
        /// <param name="estimatorPDFFilterObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimatorPDFFilterBase estimatorPDFFilterObject, SqlDataReader reader)
		{
			FillObject(estimatorPDFFilterObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EstimatorPDFFilter object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EstimatorPDFFilter object</returns>
		private EstimatorPDFFilter GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EstimatorPDFFilter estimatorPDFFilterObject= new EstimatorPDFFilter();
					FillObject(estimatorPDFFilterObject, reader);
					return estimatorPDFFilterObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EstimatorPDFFilter objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EstimatorPDFFilter objects</returns>
		private EstimatorPDFFilterList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EstimatorPDFFilter list
			EstimatorPDFFilterList list = new EstimatorPDFFilterList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EstimatorPDFFilter estimatorPDFFilterObject = new EstimatorPDFFilter();
					FillObject(estimatorPDFFilterObject, reader);

					list.Add(estimatorPDFFilterObject);
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
