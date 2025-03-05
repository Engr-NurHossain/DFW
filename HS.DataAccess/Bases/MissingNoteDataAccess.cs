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
	public partial class MissingNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMISSINGNOTE = "InsertMissingNote";
		private const string UPDATEMISSINGNOTE = "UpdateMissingNote";
		private const string DELETEMISSINGNOTE = "DeleteMissingNote";
		private const string GETMISSINGNOTEBYID = "GetMissingNoteById";
		private const string GETALLMISSINGNOTE = "GetAllMissingNote";
		private const string GETPAGEDMISSINGNOTE = "GetPagedMissingNote";
		private const string GETMISSINGNOTEMAXIMUMID = "GetMissingNoteMaximumId";
		private const string GETMISSINGNOTEROWCOUNT = "GetMissingNoteRowCount";	
		private const string GETMISSINGNOTEBYQUERY = "GetMissingNoteByQuery";
		#endregion
		
		#region Constructors
		public MissingNoteDataAccess(ClientContext context) : base(context) { }
		public MissingNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="missingNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, MissingNoteBase missingNoteObject)
		{	
			AddParameter(cmd, pInt32(MissingNoteBase.Property_CustomerID, missingNoteObject.CustomerID));
			AddParameter(cmd, pVarChar(MissingNoteBase.Property_NoteHtml, missingNoteObject.NoteHtml));
			AddParameter(cmd, pVarChar(MissingNoteBase.Property_NoteText, missingNoteObject.NoteText));
			AddParameter(cmd, pNVarChar(MissingNoteBase.Property_NoteType, missingNoteObject.NoteType));
			AddParameter(cmd, pDateTime(MissingNoteBase.Property_CreatedDate, missingNoteObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MissingNote
        /// </summary>
        /// <param name="missingNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MissingNoteBase missingNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMISSINGNOTE);
	
				AddParameter(cmd, pInt32Out(MissingNoteBase.Property_Id));
				AddCommonParams(cmd, missingNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					missingNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					missingNoteObject.Id = (Int32)GetOutParameter(cmd, MissingNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(missingNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MissingNote
        /// </summary>
        /// <param name="missingNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MissingNoteBase missingNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMISSINGNOTE);
				
				AddParameter(cmd, pInt32(MissingNoteBase.Property_Id, missingNoteObject.Id));
				AddCommonParams(cmd, missingNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					missingNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(missingNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MissingNote
        /// </summary>
        /// <param name="Id">Id of the MissingNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMISSINGNOTE);	
				
				AddParameter(cmd, pInt32(MissingNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MissingNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MissingNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MissingNote object to retrieve</param>
        /// <returns>MissingNote object, null if not found</returns>
		public MissingNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMISSINGNOTEBYID))
			{
				AddParameter( cmd, pInt32(MissingNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MissingNote objects 
        /// </summary>
        /// <returns>A list of MissingNote objects</returns>
		public MissingNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMISSINGNOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MissingNote objects by PageRequest
        /// </summary>
        /// <returns>A list of MissingNote objects</returns>
		public MissingNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMISSINGNOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MissingNoteList _MissingNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MissingNoteList;
			}
		}
		
		/// <summary>
        /// Retrieves all MissingNote objects by query String
        /// </summary>
        /// <returns>A list of MissingNote objects</returns>
		public MissingNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMISSINGNOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MissingNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MissingNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMISSINGNOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MissingNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MissingNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MissingNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMISSINGNOTEROWCOUNT))
			{
				SqlDataReader reader;
				_MissingNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MissingNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MissingNote object
        /// </summary>
        /// <param name="missingNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MissingNoteBase missingNoteObject, SqlDataReader reader, int start)
		{
			
				missingNoteObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) missingNoteObject.CustomerID = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) missingNoteObject.NoteHtml = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) missingNoteObject.NoteText = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) missingNoteObject.NoteType = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) missingNoteObject.CreatedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(missingNoteObject, reader, (start + 6));

			
			missingNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MissingNote object
        /// </summary>
        /// <param name="missingNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MissingNoteBase missingNoteObject, SqlDataReader reader)
		{
			FillObject(missingNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MissingNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MissingNote object</returns>
		private MissingNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MissingNote missingNoteObject= new MissingNote();
					FillObject(missingNoteObject, reader);
					return missingNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MissingNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MissingNote objects</returns>
		private MissingNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MissingNote list
			MissingNoteList list = new MissingNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MissingNote missingNoteObject = new MissingNote();
					FillObject(missingNoteObject, reader);

					list.Add(missingNoteObject);
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
