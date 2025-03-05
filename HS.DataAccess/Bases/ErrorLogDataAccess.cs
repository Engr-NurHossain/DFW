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
	public partial class ErrorLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTERRORLOG = "InsertErrorLog";
		private const string UPDATEERRORLOG = "UpdateErrorLog";
		private const string DELETEERRORLOG = "DeleteErrorLog";
		private const string GETERRORLOGBYID = "GetErrorLogById";
		private const string GETALLERRORLOG = "GetAllErrorLog";
		private const string GETPAGEDERRORLOG = "GetPagedErrorLog";
		private const string GETERRORLOGMAXIMUMID = "GetErrorLogMaximumId";
		private const string GETERRORLOGROWCOUNT = "GetErrorLogRowCount";	
		private const string GETERRORLOGBYQUERY = "GetErrorLogByQuery";
		#endregion
		
		#region Constructors
		public ErrorLogDataAccess(ClientContext context) : base(context) { }
		public ErrorLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="errorLogObject"></param>
		private void AddCommonParams(SqlCommand cmd, ErrorLogBase errorLogObject)
		{	
			AddParameter(cmd, pGuid(ErrorLogBase.Property_ErrorId, errorLogObject.ErrorId));
			AddParameter(cmd, pNVarChar(ErrorLogBase.Property_ErrorFor, 600, errorLogObject.ErrorFor));
			AddParameter(cmd, pNVarChar(ErrorLogBase.Property_Message, errorLogObject.Message));
			AddParameter(cmd, pDateTime(ErrorLogBase.Property_TimeUtc, errorLogObject.TimeUtc));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts ErrorLog
        /// </summary>
        /// <param name="errorLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ErrorLogBase errorLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTERRORLOG);
	
				AddParameter(cmd, pInt32Out(ErrorLogBase.Property_Id));
				AddCommonParams(cmd, errorLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					errorLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					errorLogObject.Id = (Int32)GetOutParameter(cmd, ErrorLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(errorLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates ErrorLog
        /// </summary>
        /// <param name="errorLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ErrorLogBase errorLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEERRORLOG);
				
				AddParameter(cmd, pInt32(ErrorLogBase.Property_Id, errorLogObject.Id));
				AddCommonParams(cmd, errorLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					errorLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(errorLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes ErrorLog
        /// </summary>
        /// <param name="Id">Id of the ErrorLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEERRORLOG);	
				
				AddParameter(cmd, pInt32(ErrorLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(ErrorLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves ErrorLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the ErrorLog object to retrieve</param>
        /// <returns>ErrorLog object, null if not found</returns>
		public ErrorLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETERRORLOGBYID))
			{
				AddParameter( cmd, pInt32(ErrorLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all ErrorLog objects 
        /// </summary>
        /// <returns>A list of ErrorLog objects</returns>
		public ErrorLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLERRORLOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all ErrorLog objects by PageRequest
        /// </summary>
        /// <returns>A list of ErrorLog objects</returns>
		public ErrorLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDERRORLOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ErrorLogList _ErrorLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ErrorLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all ErrorLog objects by query String
        /// </summary>
        /// <returns>A list of ErrorLog objects</returns>
		public ErrorLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETERRORLOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get ErrorLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of ErrorLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETERRORLOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get ErrorLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of ErrorLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ErrorLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETERRORLOGROWCOUNT))
			{
				SqlDataReader reader;
				_ErrorLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ErrorLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills ErrorLog object
        /// </summary>
        /// <param name="errorLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ErrorLogBase errorLogObject, SqlDataReader reader, int start)
		{
			
				errorLogObject.Id = reader.GetInt32( start + 0 );			
				errorLogObject.ErrorId = reader.GetGuid( start + 1 );			
				errorLogObject.ErrorFor = reader.GetString( start + 2 );			
				errorLogObject.Message = reader.GetString( start + 3 );			
				errorLogObject.TimeUtc = reader.GetDateTime( start + 4 );			
			FillBaseObject(errorLogObject, reader, (start + 5));

			
			errorLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills ErrorLog object
        /// </summary>
        /// <param name="errorLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ErrorLogBase errorLogObject, SqlDataReader reader)
		{
			FillObject(errorLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves ErrorLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>ErrorLog object</returns>
		private ErrorLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					ErrorLog errorLogObject= new ErrorLog();
					FillObject(errorLogObject, reader);
					return errorLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of ErrorLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of ErrorLog objects</returns>
		private ErrorLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//ErrorLog list
			ErrorLogList list = new ErrorLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					ErrorLog errorLogObject = new ErrorLog();
					FillObject(errorLogObject, reader);

					list.Add(errorLogObject);
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
