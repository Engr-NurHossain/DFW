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
	public partial class DocumentLibraryWeblinkDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTDOCUMENTLIBRARYWEBLINK = "InsertDocumentLibraryWeblink";
		private const string UPDATEDOCUMENTLIBRARYWEBLINK = "UpdateDocumentLibraryWeblink";
		private const string DELETEDOCUMENTLIBRARYWEBLINK = "DeleteDocumentLibraryWeblink";
		private const string GETDOCUMENTLIBRARYWEBLINKBYID = "GetDocumentLibraryWeblinkById";
		private const string GETALLDOCUMENTLIBRARYWEBLINK = "GetAllDocumentLibraryWeblink";
		private const string GETPAGEDDOCUMENTLIBRARYWEBLINK = "GetPagedDocumentLibraryWeblink";
		private const string GETDOCUMENTLIBRARYWEBLINKMAXIMUMID = "GetDocumentLibraryWeblinkMaximumId";
		private const string GETDOCUMENTLIBRARYWEBLINKROWCOUNT = "GetDocumentLibraryWeblinkRowCount";	
		private const string GETDOCUMENTLIBRARYWEBLINKBYQUERY = "GetDocumentLibraryWeblinkByQuery";
		#endregion
		
		#region Constructors
		public DocumentLibraryWeblinkDataAccess(ClientContext context) : base(context) { }
		public DocumentLibraryWeblinkDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="documentLibraryWeblinkObject"></param>
		private void AddCommonParams(SqlCommand cmd, DocumentLibraryWeblinkBase documentLibraryWeblinkObject)
		{	
			AddParameter(cmd, pInt32(DocumentLibraryWeblinkBase.Property_KnowledgebaseId, documentLibraryWeblinkObject.KnowledgebaseId));
			AddParameter(cmd, pNVarChar(DocumentLibraryWeblinkBase.Property_Title, 100, documentLibraryWeblinkObject.Title));
			AddParameter(cmd, pNVarChar(DocumentLibraryWeblinkBase.Property_Link, 100, documentLibraryWeblinkObject.Link));
			AddParameter(cmd, pBool(DocumentLibraryWeblinkBase.Property_IsRelated, documentLibraryWeblinkObject.IsRelated));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts DocumentLibraryWeblink
        /// </summary>
        /// <param name="documentLibraryWeblinkObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(DocumentLibraryWeblinkBase documentLibraryWeblinkObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTDOCUMENTLIBRARYWEBLINK);
	
				AddParameter(cmd, pInt32Out(DocumentLibraryWeblinkBase.Property_Id));
				AddCommonParams(cmd, documentLibraryWeblinkObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					documentLibraryWeblinkObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					documentLibraryWeblinkObject.Id = (Int32)GetOutParameter(cmd, DocumentLibraryWeblinkBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(documentLibraryWeblinkObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates DocumentLibraryWeblink
        /// </summary>
        /// <param name="documentLibraryWeblinkObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(DocumentLibraryWeblinkBase documentLibraryWeblinkObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEDOCUMENTLIBRARYWEBLINK);
				
				AddParameter(cmd, pInt32(DocumentLibraryWeblinkBase.Property_Id, documentLibraryWeblinkObject.Id));
				AddCommonParams(cmd, documentLibraryWeblinkObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					documentLibraryWeblinkObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(documentLibraryWeblinkObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes DocumentLibraryWeblink
        /// </summary>
        /// <param name="Id">Id of the DocumentLibraryWeblink object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEDOCUMENTLIBRARYWEBLINK);	
				
				AddParameter(cmd, pInt32(DocumentLibraryWeblinkBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(DocumentLibraryWeblink), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves DocumentLibraryWeblink object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the DocumentLibraryWeblink object to retrieve</param>
        /// <returns>DocumentLibraryWeblink object, null if not found</returns>
		public DocumentLibraryWeblink Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYWEBLINKBYID))
			{
				AddParameter( cmd, pInt32(DocumentLibraryWeblinkBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all DocumentLibraryWeblink objects 
        /// </summary>
        /// <returns>A list of DocumentLibraryWeblink objects</returns>
		public DocumentLibraryWeblinkList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLDOCUMENTLIBRARYWEBLINK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all DocumentLibraryWeblink objects by PageRequest
        /// </summary>
        /// <returns>A list of DocumentLibraryWeblink objects</returns>
		public DocumentLibraryWeblinkList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDDOCUMENTLIBRARYWEBLINK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				DocumentLibraryWeblinkList _DocumentLibraryWeblinkList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _DocumentLibraryWeblinkList;
			}
		}
		
		/// <summary>
        /// Retrieves all DocumentLibraryWeblink objects by query String
        /// </summary>
        /// <returns>A list of DocumentLibraryWeblink objects</returns>
		public DocumentLibraryWeblinkList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYWEBLINKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get DocumentLibraryWeblink Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of DocumentLibraryWeblink
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYWEBLINKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get DocumentLibraryWeblink Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of DocumentLibraryWeblink
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _DocumentLibraryWeblinkRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYWEBLINKROWCOUNT))
			{
				SqlDataReader reader;
				_DocumentLibraryWeblinkRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _DocumentLibraryWeblinkRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills DocumentLibraryWeblink object
        /// </summary>
        /// <param name="documentLibraryWeblinkObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(DocumentLibraryWeblinkBase documentLibraryWeblinkObject, SqlDataReader reader, int start)
		{
			
				documentLibraryWeblinkObject.Id = reader.GetInt32( start + 0 );			
				documentLibraryWeblinkObject.KnowledgebaseId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) documentLibraryWeblinkObject.Title = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) documentLibraryWeblinkObject.Link = reader.GetString( start + 3 );			
				documentLibraryWeblinkObject.IsRelated = reader.GetBoolean( start + 4 );			
			FillBaseObject(documentLibraryWeblinkObject, reader, (start + 5));

			
			documentLibraryWeblinkObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills DocumentLibraryWeblink object
        /// </summary>
        /// <param name="documentLibraryWeblinkObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(DocumentLibraryWeblinkBase documentLibraryWeblinkObject, SqlDataReader reader)
		{
			FillObject(documentLibraryWeblinkObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves DocumentLibraryWeblink object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>DocumentLibraryWeblink object</returns>
		private DocumentLibraryWeblink GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					DocumentLibraryWeblink documentLibraryWeblinkObject= new DocumentLibraryWeblink();
					FillObject(documentLibraryWeblinkObject, reader);
					return documentLibraryWeblinkObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of DocumentLibraryWeblink objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of DocumentLibraryWeblink objects</returns>
		private DocumentLibraryWeblinkList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//DocumentLibraryWeblink list
			DocumentLibraryWeblinkList list = new DocumentLibraryWeblinkList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					DocumentLibraryWeblink documentLibraryWeblinkObject = new DocumentLibraryWeblink();
					FillObject(documentLibraryWeblinkObject, reader);

					list.Add(documentLibraryWeblinkObject);
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