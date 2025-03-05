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
	public partial class DocumentLibraryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTDOCUMENTLIBRARY = "InsertDocumentLibrary";
		private const string UPDATEDOCUMENTLIBRARY = "UpdateDocumentLibrary";
		private const string DELETEDOCUMENTLIBRARY = "DeleteDocumentLibrary";
		private const string GETDOCUMENTLIBRARYBYID = "GetDocumentLibraryById";
		private const string GETALLDOCUMENTLIBRARY = "GetAllDocumentLibrary";
		private const string GETPAGEDDOCUMENTLIBRARY = "GetPagedDocumentLibrary";
		private const string GETDOCUMENTLIBRARYMAXIMUMID = "GetDocumentLibraryMaximumId";
		private const string GETDOCUMENTLIBRARYROWCOUNT = "GetDocumentLibraryRowCount";	
		private const string GETDOCUMENTLIBRARYBYQUERY = "GetDocumentLibraryByQuery";
		#endregion
		
		#region Constructors
		public DocumentLibraryDataAccess(ClientContext context) : base(context) { }
		public DocumentLibraryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="documentLibraryObject"></param>
		private void AddCommonParams(SqlCommand cmd, DocumentLibraryBase documentLibraryObject)
		{	
			AddParameter(cmd, pNVarChar(DocumentLibraryBase.Property_Title, 500, documentLibraryObject.Title));
			AddParameter(cmd, pNVarChar(DocumentLibraryBase.Property_Answer, documentLibraryObject.Answer));
			AddParameter(cmd, pNVarChar(DocumentLibraryBase.Property_Tags, 100, documentLibraryObject.Tags));
			AddParameter(cmd, pGuid(DocumentLibraryBase.Property_CreatedBy, documentLibraryObject.CreatedBy));
			AddParameter(cmd, pDateTime(DocumentLibraryBase.Property_CreatedDate, documentLibraryObject.CreatedDate));
			AddParameter(cmd, pGuid(DocumentLibraryBase.Property_LastUpdatedBy, documentLibraryObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(DocumentLibraryBase.Property_LastUpdatedDate, documentLibraryObject.LastUpdatedDate));
			AddParameter(cmd, pBool(DocumentLibraryBase.Property_IsDeleted, documentLibraryObject.IsDeleted));
			AddParameter(cmd, pBool(DocumentLibraryBase.Property_IsDocumentLibrary, documentLibraryObject.IsDocumentLibrary));
			AddParameter(cmd, pBool(DocumentLibraryBase.Property_IsFlag, documentLibraryObject.IsFlag));
			AddParameter(cmd, pGuid(DocumentLibraryBase.Property_FlagBy, documentLibraryObject.FlagBy));
			AddParameter(cmd, pDateTime(DocumentLibraryBase.Property_FlagDate, documentLibraryObject.FlagDate));
			AddParameter(cmd, pBool(DocumentLibraryBase.Property_IsHidden, documentLibraryObject.IsHidden));
			AddParameter(cmd, pNVarChar(DocumentLibraryBase.Property_CarrierTag, 100, documentLibraryObject.CarrierTag));
			AddParameter(cmd, pNVarChar(DocumentLibraryBase.Property_PolicyTag, 100, documentLibraryObject.PolicyTag));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts DocumentLibrary
        /// </summary>
        /// <param name="documentLibraryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(DocumentLibraryBase documentLibraryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTDOCUMENTLIBRARY);
	
				AddParameter(cmd, pInt32Out(DocumentLibraryBase.Property_Id));
				AddCommonParams(cmd, documentLibraryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					documentLibraryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					documentLibraryObject.Id = (Int32)GetOutParameter(cmd, DocumentLibraryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(documentLibraryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates DocumentLibrary
        /// </summary>
        /// <param name="documentLibraryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(DocumentLibraryBase documentLibraryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEDOCUMENTLIBRARY);
				
				AddParameter(cmd, pInt32(DocumentLibraryBase.Property_Id, documentLibraryObject.Id));
				AddCommonParams(cmd, documentLibraryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					documentLibraryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(documentLibraryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes DocumentLibrary
        /// </summary>
        /// <param name="Id">Id of the DocumentLibrary object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEDOCUMENTLIBRARY);	
				
				AddParameter(cmd, pInt32(DocumentLibraryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(DocumentLibrary), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves DocumentLibrary object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the DocumentLibrary object to retrieve</param>
        /// <returns>DocumentLibrary object, null if not found</returns>
		public DocumentLibrary Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYBYID))
			{
				AddParameter( cmd, pInt32(DocumentLibraryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all DocumentLibrary objects 
        /// </summary>
        /// <returns>A list of DocumentLibrary objects</returns>
		public DocumentLibraryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLDOCUMENTLIBRARY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all DocumentLibrary objects by PageRequest
        /// </summary>
        /// <returns>A list of DocumentLibrary objects</returns>
		public DocumentLibraryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDDOCUMENTLIBRARY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				DocumentLibraryList _DocumentLibraryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _DocumentLibraryList;
			}
		}
		
		/// <summary>
        /// Retrieves all DocumentLibrary objects by query String
        /// </summary>
        /// <returns>A list of DocumentLibrary objects</returns>
		public DocumentLibraryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get DocumentLibrary Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of DocumentLibrary
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get DocumentLibrary Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of DocumentLibrary
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _DocumentLibraryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETDOCUMENTLIBRARYROWCOUNT))
			{
				SqlDataReader reader;
				_DocumentLibraryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _DocumentLibraryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills DocumentLibrary object
        /// </summary>
        /// <param name="documentLibraryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(DocumentLibraryBase documentLibraryObject, SqlDataReader reader, int start)
		{
			
				documentLibraryObject.Id = reader.GetInt32( start + 0 );			
				documentLibraryObject.Title = reader.GetString( start + 1 );			
				documentLibraryObject.Answer = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) documentLibraryObject.Tags = reader.GetString( start + 3 );			
				documentLibraryObject.CreatedBy = reader.GetGuid( start + 4 );			
				documentLibraryObject.CreatedDate = reader.GetDateTime( start + 5 );			
				documentLibraryObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				documentLibraryObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				documentLibraryObject.IsDeleted = reader.GetBoolean( start + 8 );			
				documentLibraryObject.IsDocumentLibrary = reader.GetBoolean( start + 9 );			
				documentLibraryObject.IsFlag = reader.GetBoolean( start + 10 );			
				documentLibraryObject.FlagBy = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) documentLibraryObject.FlagDate = reader.GetDateTime( start + 12 );			
				documentLibraryObject.IsHidden = reader.GetBoolean( start + 13 );			
				if(!reader.IsDBNull(14)) documentLibraryObject.CarrierTag = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) documentLibraryObject.PolicyTag = reader.GetString( start + 15 );			
			FillBaseObject(documentLibraryObject, reader, (start + 16));

			
			documentLibraryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills DocumentLibrary object
        /// </summary>
        /// <param name="documentLibraryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(DocumentLibraryBase documentLibraryObject, SqlDataReader reader)
		{
			FillObject(documentLibraryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves DocumentLibrary object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>DocumentLibrary object</returns>
		private DocumentLibrary GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					DocumentLibrary documentLibraryObject= new DocumentLibrary();
					FillObject(documentLibraryObject, reader);
					return documentLibraryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of DocumentLibrary objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of DocumentLibrary objects</returns>
		private DocumentLibraryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//DocumentLibrary list
			DocumentLibraryList list = new DocumentLibraryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					DocumentLibrary documentLibraryObject = new DocumentLibrary();
					FillObject(documentLibraryObject, reader);

					list.Add(documentLibraryObject);
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