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
	public partial class TaskNoteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTASKNOTE = "InsertTaskNote";
		private const string UPDATETASKNOTE = "UpdateTaskNote";
		private const string DELETETASKNOTE = "DeleteTaskNote";
		private const string GETTASKNOTEBYID = "GetTaskNoteById";
		private const string GETALLTASKNOTE = "GetAllTaskNote";
		private const string GETPAGEDTASKNOTE = "GetPagedTaskNote";
		private const string GETTASKNOTEMAXIMUMID = "GetTaskNoteMaximumId";
		private const string GETTASKNOTEROWCOUNT = "GetTaskNoteRowCount";	
		private const string GETTASKNOTEBYQUERY = "GetTaskNoteByQuery";
		#endregion
		
		#region Constructors
		public TaskNoteDataAccess(ClientContext context) : base(context) { }
		public TaskNoteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="taskNoteObject"></param>
		private void AddCommonParams(SqlCommand cmd, TaskNoteBase taskNoteObject)
		{	
			AddParameter(cmd, pGuid(TaskNoteBase.Property_CompanyId, taskNoteObject.CompanyId));
			AddParameter(cmd, pInt32(TaskNoteBase.Property_TaskId, taskNoteObject.TaskId));
			AddParameter(cmd, pNVarChar(TaskNoteBase.Property_Note, taskNoteObject.Note));
			AddParameter(cmd, pDateTime(TaskNoteBase.Property_AddedDate, taskNoteObject.AddedDate));
			AddParameter(cmd, pGuid(TaskNoteBase.Property_AddedBy, taskNoteObject.AddedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TaskNote
        /// </summary>
        /// <param name="taskNoteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TaskNoteBase taskNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTASKNOTE);
	
				AddParameter(cmd, pInt32Out(TaskNoteBase.Property_Id));
				AddCommonParams(cmd, taskNoteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					taskNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					taskNoteObject.Id = (Int32)GetOutParameter(cmd, TaskNoteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(taskNoteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TaskNote
        /// </summary>
        /// <param name="taskNoteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TaskNoteBase taskNoteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETASKNOTE);
				
				AddParameter(cmd, pInt32(TaskNoteBase.Property_Id, taskNoteObject.Id));
				AddCommonParams(cmd, taskNoteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					taskNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(taskNoteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TaskNote
        /// </summary>
        /// <param name="Id">Id of the TaskNote object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETASKNOTE);	
				
				AddParameter(cmd, pInt32(TaskNoteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TaskNote), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TaskNote object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TaskNote object to retrieve</param>
        /// <returns>TaskNote object, null if not found</returns>
		public TaskNote Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTASKNOTEBYID))
			{
				AddParameter( cmd, pInt32(TaskNoteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TaskNote objects 
        /// </summary>
        /// <returns>A list of TaskNote objects</returns>
		public TaskNoteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTASKNOTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TaskNote objects by PageRequest
        /// </summary>
        /// <returns>A list of TaskNote objects</returns>
		public TaskNoteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTASKNOTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TaskNoteList _TaskNoteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TaskNoteList;
			}
		}
		
		/// <summary>
        /// Retrieves all TaskNote objects by query String
        /// </summary>
        /// <returns>A list of TaskNote objects</returns>
		public TaskNoteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTASKNOTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TaskNote Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TaskNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTASKNOTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TaskNote Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TaskNote
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TaskNoteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTASKNOTEROWCOUNT))
			{
				SqlDataReader reader;
				_TaskNoteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TaskNoteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TaskNote object
        /// </summary>
        /// <param name="taskNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TaskNoteBase taskNoteObject, SqlDataReader reader, int start)
		{
			
				taskNoteObject.Id = reader.GetInt32( start + 0 );			
				taskNoteObject.CompanyId = reader.GetGuid( start + 1 );			
				taskNoteObject.TaskId = reader.GetInt32( start + 2 );			
				taskNoteObject.Note = reader.GetString( start + 3 );			
				taskNoteObject.AddedDate = reader.GetDateTime( start + 4 );			
				taskNoteObject.AddedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(taskNoteObject, reader, (start + 6));

			
			taskNoteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TaskNote object
        /// </summary>
        /// <param name="taskNoteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TaskNoteBase taskNoteObject, SqlDataReader reader)
		{
			FillObject(taskNoteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TaskNote object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TaskNote object</returns>
		private TaskNote GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TaskNote taskNoteObject= new TaskNote();
					FillObject(taskNoteObject, reader);
					return taskNoteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TaskNote objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TaskNote objects</returns>
		private TaskNoteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TaskNote list
			TaskNoteList list = new TaskNoteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TaskNote taskNoteObject = new TaskNote();
					FillObject(taskNoteObject, reader);

					list.Add(taskNoteObject);
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
