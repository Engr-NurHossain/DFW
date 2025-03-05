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
	public partial class EstimatorServiceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATORSERVICE = "InsertEstimatorService";
		private const string UPDATEESTIMATORSERVICE = "UpdateEstimatorService";
		private const string DELETEESTIMATORSERVICE = "DeleteEstimatorService";
		private const string GETESTIMATORSERVICEBYID = "GetEstimatorServiceById";
		private const string GETALLESTIMATORSERVICE = "GetAllEstimatorService";
		private const string GETPAGEDESTIMATORSERVICE = "GetPagedEstimatorService";
		private const string GETESTIMATORSERVICEMAXIMUMID = "GetEstimatorServiceMaximumId";
		private const string GETESTIMATORSERVICEROWCOUNT = "GetEstimatorServiceRowCount";	
		private const string GETESTIMATORSERVICEBYQUERY = "GetEstimatorServiceByQuery";
		#endregion
		
		#region Constructors
		public EstimatorServiceDataAccess(ClientContext context) : base(context) { }
		public EstimatorServiceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimatorServiceObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimatorServiceBase estimatorServiceObject)
		{	
			AddParameter(cmd, pNVarChar(EstimatorServiceBase.Property_EstimatorId, 50, estimatorServiceObject.EstimatorId));
			AddParameter(cmd, pGuid(EstimatorServiceBase.Property_EquipmentId, estimatorServiceObject.EquipmentId));
			AddParameter(cmd, pNVarChar(EstimatorServiceBase.Property_EquipmentName, 250, estimatorServiceObject.EquipmentName));
			AddParameter(cmd, pDouble(EstimatorServiceBase.Property_UnitPrice, estimatorServiceObject.UnitPrice));
			AddParameter(cmd, pInt32(EstimatorServiceBase.Property_Quantity, estimatorServiceObject.Quantity));
			AddParameter(cmd, pDouble(EstimatorServiceBase.Property_Amount, estimatorServiceObject.Amount));
			AddParameter(cmd, pBool(EstimatorServiceBase.Property_IsTaxable, estimatorServiceObject.IsTaxable));
			AddParameter(cmd, pGuid(EstimatorServiceBase.Property_CreatedBy, estimatorServiceObject.CreatedBy));
			AddParameter(cmd, pDateTime(EstimatorServiceBase.Property_CreatedDate, estimatorServiceObject.CreatedDate));
			AddParameter(cmd, pBool(EstimatorServiceBase.Property_IsOneTimeService, estimatorServiceObject.IsOneTimeService));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EstimatorService
        /// </summary>
        /// <param name="estimatorServiceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimatorServiceBase estimatorServiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATORSERVICE);
	
				AddParameter(cmd, pInt32Out(EstimatorServiceBase.Property_Id));
				AddCommonParams(cmd, estimatorServiceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimatorServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimatorServiceObject.Id = (Int32)GetOutParameter(cmd, EstimatorServiceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimatorServiceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EstimatorService
        /// </summary>
        /// <param name="estimatorServiceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimatorServiceBase estimatorServiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATORSERVICE);
				
				AddParameter(cmd, pInt32(EstimatorServiceBase.Property_Id, estimatorServiceObject.Id));
				AddCommonParams(cmd, estimatorServiceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimatorServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimatorServiceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EstimatorService
        /// </summary>
        /// <param name="Id">Id of the EstimatorService object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATORSERVICE);	
				
				AddParameter(cmd, pInt32(EstimatorServiceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EstimatorService), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EstimatorService object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EstimatorService object to retrieve</param>
        /// <returns>EstimatorService object, null if not found</returns>
		public EstimatorService Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORSERVICEBYID))
			{
				AddParameter( cmd, pInt32(EstimatorServiceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EstimatorService objects 
        /// </summary>
        /// <returns>A list of EstimatorService objects</returns>
		public EstimatorServiceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATORSERVICE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EstimatorService objects by PageRequest
        /// </summary>
        /// <returns>A list of EstimatorService objects</returns>
		public EstimatorServiceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATORSERVICE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimatorServiceList _EstimatorServiceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimatorServiceList;
			}
		}
		
		/// <summary>
        /// Retrieves all EstimatorService objects by query String
        /// </summary>
        /// <returns>A list of EstimatorService objects</returns>
		public EstimatorServiceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORSERVICEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EstimatorService Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EstimatorService
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORSERVICEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EstimatorService Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EstimatorService
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimatorServiceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORSERVICEROWCOUNT))
			{
				SqlDataReader reader;
				_EstimatorServiceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimatorServiceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EstimatorService object
        /// </summary>
        /// <param name="estimatorServiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimatorServiceBase estimatorServiceObject, SqlDataReader reader, int start)
		{
			
				estimatorServiceObject.Id = reader.GetInt32( start + 0 );			
				estimatorServiceObject.EstimatorId = reader.GetString( start + 1 );			
				estimatorServiceObject.EquipmentId = reader.GetGuid( start + 2 );			
				estimatorServiceObject.EquipmentName = reader.GetString( start + 3 );			
				estimatorServiceObject.UnitPrice = reader.GetDouble( start + 4 );			
				estimatorServiceObject.Quantity = reader.GetInt32( start + 5 );			
				estimatorServiceObject.Amount = reader.GetDouble( start + 6 );			
				estimatorServiceObject.IsTaxable = reader.GetBoolean( start + 7 );			
				estimatorServiceObject.CreatedBy = reader.GetGuid( start + 8 );			
				estimatorServiceObject.CreatedDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) estimatorServiceObject.IsOneTimeService = reader.GetBoolean( start + 10 );			
			FillBaseObject(estimatorServiceObject, reader, (start + 11));

			
			estimatorServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EstimatorService object
        /// </summary>
        /// <param name="estimatorServiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimatorServiceBase estimatorServiceObject, SqlDataReader reader)
		{
			FillObject(estimatorServiceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EstimatorService object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EstimatorService object</returns>
		private EstimatorService GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EstimatorService estimatorServiceObject= new EstimatorService();
					FillObject(estimatorServiceObject, reader);
					return estimatorServiceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EstimatorService objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EstimatorService objects</returns>
		private EstimatorServiceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EstimatorService list
			EstimatorServiceList list = new EstimatorServiceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EstimatorService estimatorServiceObject = new EstimatorService();
					FillObject(estimatorServiceObject, reader);

					list.Add(estimatorServiceObject);
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
