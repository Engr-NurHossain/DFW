using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class EstimatorNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTESTIMATORNOTE = "InsertEstimatorNote";
		private const string UPDATEESTIMATORNOTE = "UpdateEstimatorNote";
		private const string DELETEESTIMATORNOTE = "DeleteEstimatorNote";
		private const string GETESTIMATORNOTEBYID = "GetEstimatorNoteById";
		private const string GETALLESTIMATORNOTE = "GetAllEstimatorNote";
		private const string GETPAGEDESTIMATORNOTE = "GetPagedEstimatorNote";
		private const string GETESTIMATORNOTEMAXIMUMID = "GetEstimatorNoteMaximumId";
		private const string GETESTIMATORNOTEROWCOUNT = "GetEstimatorNoteRowCount";	
		private const string GETESTIMATORNOTEBYQUERY = "GetEstimatorNoteByQuery";
		#endregion
		
		#region Constructors
		public EstimatorNoteDataAccess(ClientContext context) : base(context) { }
		public EstimatorNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="estimatorNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, EstimatorNoteBase estimatorNoteObject)
		{	
			AddParameter(cmd, pGuid(EstimatorNoteBase.Property_CompanyId, estimatorNoteObject.CompanyId));
			AddParameter(cmd, pInt32(EstimatorNoteBase.Property_EstimatorId, estimatorNoteObject.EstimatorId));
			AddParameter(cmd, pNVarChar(EstimatorNoteBase.Property_Note, estimatorNoteObject.Note));
			AddParameter(cmd, pDateTime(EstimatorNoteBase.Property_AddedDate, estimatorNoteObject.AddedDate));
			AddParameter(cmd, pGuid(EstimatorNoteBase.Property_AddedBy, estimatorNoteObject.AddedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EstimatorNote
        /// </summary>
        /// <param name="estimatorNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EstimatorNoteBase estimatorNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTESTIMATORNOTE);
	
				AddParameter(cmd, pInt32Out(EstimatorNoteBase.Property_Id));
				AddCommonParams(cmd, estimatorNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					estimatorNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					estimatorNoteObject.Id = (Int32)GetOutParameter(cmd, EstimatorNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(estimatorNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EstimatorNote
        /// </summary>
        /// <param name="estimatorNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EstimatorNoteBase estimatorNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEESTIMATORNOTE);
				
				AddParameter(cmd, pInt32(EstimatorNoteBase.Property_Id, estimatorNoteObject.Id));
				AddCommonParams(cmd, estimatorNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					estimatorNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(estimatorNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EstimatorNote
        /// </summary>
        /// <param name="Id">Id of the EstimatorNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEESTIMATORNOTE);	
				
				AddParameter(cmd, pInt32(EstimatorNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EstimatorNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EstimatorNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EstimatorNote object to retrieve</param>
        /// <returns>EstimatorNote object, null if not found</returns>
		public EstimatorNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORNOTEBYID))
			{
				AddParameter( cmd, pInt32(EstimatorNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EstimatorNote objects 
        /// </summary>
        /// <returns>A list of EstimatorNote objects</returns>
		public EstimatorNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLESTIMATORNOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EstimatorNote objects by PageRequest
        /// </summary>
        /// <returns>A list of EstimatorNote objects</returns>
		public EstimatorNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDESTIMATORNOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EstimatorNoteList _EstimatorNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EstimatorNoteList;
			}
		}

        /// <summary>
        /// Retrieves all EstimatorNote objects by query String
        /// </summary>
        /// <returns>A list of EstimatorNote objects</returns>
        public EstimatorNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORNOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EstimatorNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EstimatorNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORNOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EstimatorNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EstimatorNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EstimatorNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETESTIMATORNOTEROWCOUNT))
			{
				SqlDataReader reader;
				_EstimatorNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EstimatorNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EstimatorNote object
        /// </summary>
        /// <param name="estimatorNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EstimatorNoteBase estimatorNoteObject, SqlDataReader reader, int start)
		{
			
				estimatorNoteObject.Id = reader.GetInt32( start + 0 );			
				estimatorNoteObject.CompanyId = reader.GetGuid( start + 1 );			
				estimatorNoteObject.EstimatorId = reader.GetInt32( start + 2 );			
				estimatorNoteObject.Note = reader.GetString( start + 3 );			
				estimatorNoteObject.AddedDate = reader.GetDateTime( start + 4 );			
				estimatorNoteObject.AddedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(estimatorNoteObject, reader, (start + 6));

			
			estimatorNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EstimatorNote object
        /// </summary>
        /// <param name="estimatorNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EstimatorNoteBase estimatorNoteObject, SqlDataReader reader)
		{
			FillObject(estimatorNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EstimatorNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EstimatorNote object</returns>
		private EstimatorNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EstimatorNote estimatorNoteObject= new EstimatorNote();
					FillObject(estimatorNoteObject, reader);
					return estimatorNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EstimatorNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EstimatorNote objects</returns>
		private EstimatorNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EstimatorNote list
			EstimatorNoteList list = new EstimatorNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EstimatorNote estimatorNoteObject = new EstimatorNote();
					FillObject(estimatorNoteObject, reader);

					list.Add(estimatorNoteObject);
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
