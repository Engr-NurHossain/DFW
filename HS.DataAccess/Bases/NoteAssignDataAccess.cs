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
	public partial class NoteAssignDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTNOTEASSIGN = "InsertNoteAssign";
		private const string UPDATENOTEASSIGN = "UpdateNoteAssign";
		private const string DELETENOTEASSIGN = "DeleteNoteAssign";
		private const string GETNOTEASSIGNBYID = "GetNoteAssignById";
		private const string GETALLNOTEASSIGN = "GetAllNoteAssign";
		private const string GETPAGEDNOTEASSIGN = "GetPagedNoteAssign";
		private const string GETNOTEASSIGNMAXIMUMID = "GetNoteAssignMaximumId";
		private const string GETNOTEASSIGNROWCOUNT = "GetNoteAssignRowCount";	
		private const string GETNOTEASSIGNBYQUERY = "GetNoteAssignByQuery";
		#endregion
		
		#region Constructors
		public NoteAssignDataAccess(ClientContext context) : base(context) { }
		public NoteAssignDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="noteAssignObject"></param>
		private void AddCommonParams(SqlCommand cmd, NoteAssignBase noteAssignObject)
		{	
			AddParameter(cmd, pInt32(NoteAssignBase.Property_NoteId, noteAssignObject.NoteId));
			AddParameter(cmd, pGuid(NoteAssignBase.Property_EmployeeId, noteAssignObject.EmployeeId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts NoteAssign
        /// </summary>
        /// <param name="noteAssignObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(NoteAssignBase noteAssignObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTNOTEASSIGN);
	
				AddParameter(cmd, pInt32Out(NoteAssignBase.Property_Id));
				AddCommonParams(cmd, noteAssignObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					noteAssignObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					noteAssignObject.Id = (Int32)GetOutParameter(cmd, NoteAssignBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(noteAssignObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates NoteAssign
        /// </summary>
        /// <param name="noteAssignObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(NoteAssignBase noteAssignObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATENOTEASSIGN);
				
				AddParameter(cmd, pInt32(NoteAssignBase.Property_Id, noteAssignObject.Id));
				AddCommonParams(cmd, noteAssignObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					noteAssignObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(noteAssignObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes NoteAssign
        /// </summary>
        /// <param name="Id">Id of the NoteAssign object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETENOTEASSIGN);	
				
				AddParameter(cmd, pInt32(NoteAssignBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(NoteAssign), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves NoteAssign object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the NoteAssign object to retrieve</param>
        /// <returns>NoteAssign object, null if not found</returns>
		public NoteAssign Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETNOTEASSIGNBYID))
			{
				AddParameter( cmd, pInt32(NoteAssignBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all NoteAssign objects 
        /// </summary>
        /// <returns>A list of NoteAssign objects</returns>
		public NoteAssignList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLNOTEASSIGN))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all NoteAssign objects by PageRequest
        /// </summary>
        /// <returns>A list of NoteAssign objects</returns>
		public NoteAssignList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDNOTEASSIGN))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				NoteAssignList _NoteAssignList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _NoteAssignList;
			}
		}
		
		/// <summary>
        /// Retrieves all NoteAssign objects by query String
        /// </summary>
        /// <returns>A list of NoteAssign objects</returns>
		public NoteAssignList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETNOTEASSIGNBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get NoteAssign Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of NoteAssign
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNOTEASSIGNMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get NoteAssign Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of NoteAssign
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _NoteAssignRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETNOTEASSIGNROWCOUNT))
			{
				SqlDataReader reader;
				_NoteAssignRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _NoteAssignRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills NoteAssign object
        /// </summary>
        /// <param name="noteAssignObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(NoteAssignBase noteAssignObject, SqlDataReader reader, int start)
		{
			
				noteAssignObject.Id = reader.GetInt32( start + 0 );			
				noteAssignObject.NoteId = reader.GetInt32( start + 1 );			
				noteAssignObject.EmployeeId = reader.GetGuid( start + 2 );			
			FillBaseObject(noteAssignObject, reader, (start + 3));

			
			noteAssignObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills NoteAssign object
        /// </summary>
        /// <param name="noteAssignObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(NoteAssignBase noteAssignObject, SqlDataReader reader)
		{
			FillObject(noteAssignObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves NoteAssign object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>NoteAssign object</returns>
		private NoteAssign GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					NoteAssign noteAssignObject= new NoteAssign();
					FillObject(noteAssignObject, reader);
					return noteAssignObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of NoteAssign objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of NoteAssign objects</returns>
		private NoteAssignList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//NoteAssign list
			NoteAssignList list = new NoteAssignList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					NoteAssign noteAssignObject = new NoteAssign();
					FillObject(noteAssignObject, reader);

					list.Add(noteAssignObject);
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
