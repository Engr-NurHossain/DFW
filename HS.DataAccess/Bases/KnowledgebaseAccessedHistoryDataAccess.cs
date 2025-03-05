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
	public partial class KnowledgebaseAccessedHistoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGEBASEACCESSEDHISTORY = "InsertKnowledgebaseAccessedHistory";
		private const string UPDATEKNOWLEDGEBASEACCESSEDHISTORY = "UpdateKnowledgebaseAccessedHistory";
		private const string DELETEKNOWLEDGEBASEACCESSEDHISTORY = "DeleteKnowledgebaseAccessedHistory";
		private const string GETKNOWLEDGEBASEACCESSEDHISTORYBYID = "GetKnowledgebaseAccessedHistoryById";
		private const string GETALLKNOWLEDGEBASEACCESSEDHISTORY = "GetAllKnowledgebaseAccessedHistory";
		private const string GETPAGEDKNOWLEDGEBASEACCESSEDHISTORY = "GetPagedKnowledgebaseAccessedHistory";
		private const string GETKNOWLEDGEBASEACCESSEDHISTORYMAXIMUMID = "GetKnowledgebaseAccessedHistoryMaximumId";
		private const string GETKNOWLEDGEBASEACCESSEDHISTORYROWCOUNT = "GetKnowledgebaseAccessedHistoryRowCount";	
		private const string GETKNOWLEDGEBASEACCESSEDHISTORYBYQUERY = "GetKnowledgebaseAccessedHistoryByQuery";
		#endregion
		
		#region Constructors
		public KnowledgebaseAccessedHistoryDataAccess(ClientContext context) : base(context) { }
		public KnowledgebaseAccessedHistoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgebaseAccessedHistoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgebaseAccessedHistoryBase knowledgebaseAccessedHistoryObject)
		{	
			AddParameter(cmd, pInt32(KnowledgebaseAccessedHistoryBase.Property_KnowledgebaseId, knowledgebaseAccessedHistoryObject.KnowledgebaseId));
			AddParameter(cmd, pBool(KnowledgebaseAccessedHistoryBase.Property_IsDocumentLibrary, knowledgebaseAccessedHistoryObject.IsDocumentLibrary));
			AddParameter(cmd, pGuid(KnowledgebaseAccessedHistoryBase.Property_VisitedBy, knowledgebaseAccessedHistoryObject.VisitedBy));
			AddParameter(cmd, pDateTime(KnowledgebaseAccessedHistoryBase.Property_VisitedDate, knowledgebaseAccessedHistoryObject.VisitedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgebaseAccessedHistory
        /// </summary>
        /// <param name="knowledgebaseAccessedHistoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgebaseAccessedHistoryBase knowledgebaseAccessedHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGEBASEACCESSEDHISTORY);
	
				AddParameter(cmd, pInt32Out(KnowledgebaseAccessedHistoryBase.Property_Id));
				AddCommonParams(cmd, knowledgebaseAccessedHistoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgebaseAccessedHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgebaseAccessedHistoryObject.Id = (Int32)GetOutParameter(cmd, KnowledgebaseAccessedHistoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgebaseAccessedHistoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgebaseAccessedHistory
        /// </summary>
        /// <param name="knowledgebaseAccessedHistoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgebaseAccessedHistoryBase knowledgebaseAccessedHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGEBASEACCESSEDHISTORY);
				
				AddParameter(cmd, pInt32(KnowledgebaseAccessedHistoryBase.Property_Id, knowledgebaseAccessedHistoryObject.Id));
				AddCommonParams(cmd, knowledgebaseAccessedHistoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgebaseAccessedHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgebaseAccessedHistoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgebaseAccessedHistory
        /// </summary>
        /// <param name="Id">Id of the KnowledgebaseAccessedHistory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGEBASEACCESSEDHISTORY);	
				
				AddParameter(cmd, pInt32(KnowledgebaseAccessedHistoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgebaseAccessedHistory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgebaseAccessedHistory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgebaseAccessedHistory object to retrieve</param>
        /// <returns>KnowledgebaseAccessedHistory object, null if not found</returns>
		public KnowledgebaseAccessedHistory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCESSEDHISTORYBYID))
			{
				AddParameter( cmd, pInt32(KnowledgebaseAccessedHistoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgebaseAccessedHistory objects 
        /// </summary>
        /// <returns>A list of KnowledgebaseAccessedHistory objects</returns>
		public KnowledgebaseAccessedHistoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGEBASEACCESSEDHISTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgebaseAccessedHistory objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgebaseAccessedHistory objects</returns>
		public KnowledgebaseAccessedHistoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGEBASEACCESSEDHISTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgebaseAccessedHistoryList _KnowledgebaseAccessedHistoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgebaseAccessedHistoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgebaseAccessedHistory objects by query String
        /// </summary>
        /// <returns>A list of KnowledgebaseAccessedHistory objects</returns>
		public KnowledgebaseAccessedHistoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCESSEDHISTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgebaseAccessedHistory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgebaseAccessedHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCESSEDHISTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgebaseAccessedHistory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgebaseAccessedHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgebaseAccessedHistoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGEBASEACCESSEDHISTORYROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgebaseAccessedHistoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgebaseAccessedHistoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgebaseAccessedHistory object
        /// </summary>
        /// <param name="knowledgebaseAccessedHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgebaseAccessedHistoryBase knowledgebaseAccessedHistoryObject, SqlDataReader reader, int start)
		{
			
				knowledgebaseAccessedHistoryObject.Id = reader.GetInt32( start + 0 );			
				knowledgebaseAccessedHistoryObject.KnowledgebaseId = reader.GetInt32( start + 1 );			
				knowledgebaseAccessedHistoryObject.IsDocumentLibrary = reader.GetBoolean( start + 2 );			
				knowledgebaseAccessedHistoryObject.VisitedBy = reader.GetGuid( start + 3 );			
				knowledgebaseAccessedHistoryObject.VisitedDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(knowledgebaseAccessedHistoryObject, reader, (start + 5));

			
			knowledgebaseAccessedHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgebaseAccessedHistory object
        /// </summary>
        /// <param name="knowledgebaseAccessedHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgebaseAccessedHistoryBase knowledgebaseAccessedHistoryObject, SqlDataReader reader)
		{
			FillObject(knowledgebaseAccessedHistoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgebaseAccessedHistory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgebaseAccessedHistory object</returns>
		private KnowledgebaseAccessedHistory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgebaseAccessedHistory knowledgebaseAccessedHistoryObject= new KnowledgebaseAccessedHistory();
					FillObject(knowledgebaseAccessedHistoryObject, reader);
					return knowledgebaseAccessedHistoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgebaseAccessedHistory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgebaseAccessedHistory objects</returns>
		private KnowledgebaseAccessedHistoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgebaseAccessedHistory list
			KnowledgebaseAccessedHistoryList list = new KnowledgebaseAccessedHistoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgebaseAccessedHistory knowledgebaseAccessedHistoryObject = new KnowledgebaseAccessedHistory();
					FillObject(knowledgebaseAccessedHistoryObject, reader);

					list.Add(knowledgebaseAccessedHistoryObject);
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