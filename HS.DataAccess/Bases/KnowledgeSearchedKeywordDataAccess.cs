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
	public partial class KnowledgeSearchedKeywordDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTKNOWLEDGESEARCHEDKEYWORD = "InsertKnowledgeSearchedKeyword";
		private const string UPDATEKNOWLEDGESEARCHEDKEYWORD = "UpdateKnowledgeSearchedKeyword";
		private const string DELETEKNOWLEDGESEARCHEDKEYWORD = "DeleteKnowledgeSearchedKeyword";
		private const string GETKNOWLEDGESEARCHEDKEYWORDBYID = "GetKnowledgeSearchedKeywordById";
		private const string GETALLKNOWLEDGESEARCHEDKEYWORD = "GetAllKnowledgeSearchedKeyword";
		private const string GETPAGEDKNOWLEDGESEARCHEDKEYWORD = "GetPagedKnowledgeSearchedKeyword";
		private const string GETKNOWLEDGESEARCHEDKEYWORDMAXIMUMID = "GetKnowledgeSearchedKeywordMaximumId";
		private const string GETKNOWLEDGESEARCHEDKEYWORDROWCOUNT = "GetKnowledgeSearchedKeywordRowCount";	
		private const string GETKNOWLEDGESEARCHEDKEYWORDBYQUERY = "GetKnowledgeSearchedKeywordByQuery";
		#endregion
		
		#region Constructors
		public KnowledgeSearchedKeywordDataAccess(ClientContext context) : base(context) { }
		public KnowledgeSearchedKeywordDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="knowledgeSearchedKeywordObject"></param>
		private void AddCommonParams(SqlCommand cmd, KnowledgeSearchedKeywordBase knowledgeSearchedKeywordObject)
		{	
			AddParameter(cmd, pNVarChar(KnowledgeSearchedKeywordBase.Property_Keyword, knowledgeSearchedKeywordObject.Keyword));
			AddParameter(cmd, pGuid(KnowledgeSearchedKeywordBase.Property_SearchedBy, knowledgeSearchedKeywordObject.SearchedBy));
			AddParameter(cmd, pDateTime(KnowledgeSearchedKeywordBase.Property_SearchedDate, knowledgeSearchedKeywordObject.SearchedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts KnowledgeSearchedKeyword
        /// </summary>
        /// <param name="knowledgeSearchedKeywordObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(KnowledgeSearchedKeywordBase knowledgeSearchedKeywordObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTKNOWLEDGESEARCHEDKEYWORD);
	
				AddParameter(cmd, pInt32Out(KnowledgeSearchedKeywordBase.Property_Id));
				AddCommonParams(cmd, knowledgeSearchedKeywordObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					knowledgeSearchedKeywordObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					knowledgeSearchedKeywordObject.Id = (Int32)GetOutParameter(cmd, KnowledgeSearchedKeywordBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(knowledgeSearchedKeywordObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates KnowledgeSearchedKeyword
        /// </summary>
        /// <param name="knowledgeSearchedKeywordObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(KnowledgeSearchedKeywordBase knowledgeSearchedKeywordObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEKNOWLEDGESEARCHEDKEYWORD);
				
				AddParameter(cmd, pInt32(KnowledgeSearchedKeywordBase.Property_Id, knowledgeSearchedKeywordObject.Id));
				AddCommonParams(cmd, knowledgeSearchedKeywordObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					knowledgeSearchedKeywordObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(knowledgeSearchedKeywordObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes KnowledgeSearchedKeyword
        /// </summary>
        /// <param name="Id">Id of the KnowledgeSearchedKeyword object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEKNOWLEDGESEARCHEDKEYWORD);	
				
				AddParameter(cmd, pInt32(KnowledgeSearchedKeywordBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(KnowledgeSearchedKeyword), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves KnowledgeSearchedKeyword object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the KnowledgeSearchedKeyword object to retrieve</param>
        /// <returns>KnowledgeSearchedKeyword object, null if not found</returns>
		public KnowledgeSearchedKeyword Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGESEARCHEDKEYWORDBYID))
			{
				AddParameter( cmd, pInt32(KnowledgeSearchedKeywordBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all KnowledgeSearchedKeyword objects 
        /// </summary>
        /// <returns>A list of KnowledgeSearchedKeyword objects</returns>
		public KnowledgeSearchedKeywordList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLKNOWLEDGESEARCHEDKEYWORD))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all KnowledgeSearchedKeyword objects by PageRequest
        /// </summary>
        /// <returns>A list of KnowledgeSearchedKeyword objects</returns>
		public KnowledgeSearchedKeywordList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDKNOWLEDGESEARCHEDKEYWORD))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				KnowledgeSearchedKeywordList _KnowledgeSearchedKeywordList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _KnowledgeSearchedKeywordList;
			}
		}
		
		/// <summary>
        /// Retrieves all KnowledgeSearchedKeyword objects by query String
        /// </summary>
        /// <returns>A list of KnowledgeSearchedKeyword objects</returns>
		public KnowledgeSearchedKeywordList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGESEARCHEDKEYWORDBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get KnowledgeSearchedKeyword Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of KnowledgeSearchedKeyword
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGESEARCHEDKEYWORDMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get KnowledgeSearchedKeyword Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of KnowledgeSearchedKeyword
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _KnowledgeSearchedKeywordRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETKNOWLEDGESEARCHEDKEYWORDROWCOUNT))
			{
				SqlDataReader reader;
				_KnowledgeSearchedKeywordRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _KnowledgeSearchedKeywordRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills KnowledgeSearchedKeyword object
        /// </summary>
        /// <param name="knowledgeSearchedKeywordObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(KnowledgeSearchedKeywordBase knowledgeSearchedKeywordObject, SqlDataReader reader, int start)
		{
			
				knowledgeSearchedKeywordObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) knowledgeSearchedKeywordObject.Keyword = reader.GetString( start + 1 );			
				knowledgeSearchedKeywordObject.SearchedBy = reader.GetGuid( start + 2 );			
				knowledgeSearchedKeywordObject.SearchedDate = reader.GetDateTime( start + 3 );			
			FillBaseObject(knowledgeSearchedKeywordObject, reader, (start + 4));

			
			knowledgeSearchedKeywordObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills KnowledgeSearchedKeyword object
        /// </summary>
        /// <param name="knowledgeSearchedKeywordObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(KnowledgeSearchedKeywordBase knowledgeSearchedKeywordObject, SqlDataReader reader)
		{
			FillObject(knowledgeSearchedKeywordObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves KnowledgeSearchedKeyword object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>KnowledgeSearchedKeyword object</returns>
		private KnowledgeSearchedKeyword GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					KnowledgeSearchedKeyword knowledgeSearchedKeywordObject= new KnowledgeSearchedKeyword();
					FillObject(knowledgeSearchedKeywordObject, reader);
					return knowledgeSearchedKeywordObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of KnowledgeSearchedKeyword objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of KnowledgeSearchedKeyword objects</returns>
		private KnowledgeSearchedKeywordList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//KnowledgeSearchedKeyword list
			KnowledgeSearchedKeywordList list = new KnowledgeSearchedKeywordList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					KnowledgeSearchedKeyword knowledgeSearchedKeywordObject = new KnowledgeSearchedKeyword();
					FillObject(knowledgeSearchedKeywordObject, reader);

					list.Add(knowledgeSearchedKeywordObject);
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