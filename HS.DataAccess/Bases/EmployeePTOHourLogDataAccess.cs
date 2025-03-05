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
	public partial class EmployeePTOHourLogDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEEPTOHOURLOG = "InsertEmployeePTOHourLog";
		private const string UPDATEEMPLOYEEPTOHOURLOG = "UpdateEmployeePTOHourLog";
		private const string DELETEEMPLOYEEPTOHOURLOG = "DeleteEmployeePTOHourLog";
		private const string GETEMPLOYEEPTOHOURLOGBYID = "GetEmployeePTOHourLogById";
		private const string GETALLEMPLOYEEPTOHOURLOG = "GetAllEmployeePTOHourLog";
		private const string GETPAGEDEMPLOYEEPTOHOURLOG = "GetPagedEmployeePTOHourLog";
		private const string GETEMPLOYEEPTOHOURLOGMAXIMUMID = "GetEmployeePTOHourLogMaximumId";
		private const string GETEMPLOYEEPTOHOURLOGROWCOUNT = "GetEmployeePTOHourLogRowCount";	
		private const string GETEMPLOYEEPTOHOURLOGBYQUERY = "GetEmployeePTOHourLogByQuery";
		#endregion
		
		#region Constructors
		public EmployeePTOHourLogDataAccess(ClientContext context) : base(context) { }
		public EmployeePTOHourLogDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeePTOHourLogObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeePTOHourLogBase employeePTOHourLogObject)
		{	
			AddParameter(cmd, pGuid(EmployeePTOHourLogBase.Property_UserId, employeePTOHourLogObject.UserId));
			AddParameter(cmd, pDouble(EmployeePTOHourLogBase.Property_PTOHour, employeePTOHourLogObject.PTOHour));
			AddParameter(cmd, pDateTime(EmployeePTOHourLogBase.Property_FromDate, employeePTOHourLogObject.FromDate));
			AddParameter(cmd, pDateTime(EmployeePTOHourLogBase.Property_EndDate, employeePTOHourLogObject.EndDate));
			AddParameter(cmd, pDateTime(EmployeePTOHourLogBase.Property_CreatedDate, employeePTOHourLogObject.CreatedDate));
			AddParameter(cmd, pDateTime(EmployeePTOHourLogBase.Property_LastUpdatedDate, employeePTOHourLogObject.LastUpdatedDate));
			AddParameter(cmd, pDouble(EmployeePTOHourLogBase.Property_WorkingHours, employeePTOHourLogObject.WorkingHours));
			AddParameter(cmd, pDouble(EmployeePTOHourLogBase.Property_PtoRate, employeePTOHourLogObject.PtoRate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeePTOHourLog
        /// </summary>
        /// <param name="employeePTOHourLogObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeePTOHourLogBase employeePTOHourLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEEPTOHOURLOG);
	
				AddParameter(cmd, pInt32Out(EmployeePTOHourLogBase.Property_Id));
				AddCommonParams(cmd, employeePTOHourLogObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeePTOHourLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeePTOHourLogObject.Id = (Int32)GetOutParameter(cmd, EmployeePTOHourLogBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeePTOHourLogObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeePTOHourLog
        /// </summary>
        /// <param name="employeePTOHourLogObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeePTOHourLogBase employeePTOHourLogObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEEPTOHOURLOG);
				
				AddParameter(cmd, pInt32(EmployeePTOHourLogBase.Property_Id, employeePTOHourLogObject.Id));
				AddCommonParams(cmd, employeePTOHourLogObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeePTOHourLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeePTOHourLogObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeePTOHourLog
        /// </summary>
        /// <param name="Id">Id of the EmployeePTOHourLog object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEEPTOHOURLOG);	
				
				AddParameter(cmd, pInt32(EmployeePTOHourLogBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeePTOHourLog), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeePTOHourLog object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeePTOHourLog object to retrieve</param>
        /// <returns>EmployeePTOHourLog object, null if not found</returns>
		public EmployeePTOHourLog Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOHOURLOGBYID))
			{
				AddParameter( cmd, pInt32(EmployeePTOHourLogBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeePTOHourLog objects 
        /// </summary>
        /// <returns>A list of EmployeePTOHourLog objects</returns>
		public EmployeePTOHourLogList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEEPTOHOURLOG))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeePTOHourLog objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeePTOHourLog objects</returns>
		public EmployeePTOHourLogList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEEPTOHOURLOG))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeePTOHourLogList _EmployeePTOHourLogList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeePTOHourLogList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeePTOHourLog objects by query String
        /// </summary>
        /// <returns>A list of EmployeePTOHourLog objects</returns>
		public EmployeePTOHourLogList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOHOURLOGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeePTOHourLog Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeePTOHourLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOHOURLOGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeePTOHourLog Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeePTOHourLog
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeePTOHourLogRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEEPTOHOURLOGROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeePTOHourLogRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeePTOHourLogRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeePTOHourLog object
        /// </summary>
        /// <param name="employeePTOHourLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeePTOHourLogBase employeePTOHourLogObject, SqlDataReader reader, int start)
		{
			
				employeePTOHourLogObject.Id = reader.GetInt32( start + 0 );			
				employeePTOHourLogObject.UserId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) employeePTOHourLogObject.PTOHour = reader.GetDouble( start + 2 );			
				if(!reader.IsDBNull(3)) employeePTOHourLogObject.FromDate = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) employeePTOHourLogObject.EndDate = reader.GetDateTime( start + 4 );			
				employeePTOHourLogObject.CreatedDate = reader.GetDateTime( start + 5 );			
				employeePTOHourLogObject.LastUpdatedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) employeePTOHourLogObject.WorkingHours = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) employeePTOHourLogObject.PtoRate = reader.GetDouble( start + 8 );			
			FillBaseObject(employeePTOHourLogObject, reader, (start + 9));

			
			employeePTOHourLogObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeePTOHourLog object
        /// </summary>
        /// <param name="employeePTOHourLogObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeePTOHourLogBase employeePTOHourLogObject, SqlDataReader reader)
		{
			FillObject(employeePTOHourLogObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeePTOHourLog object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeePTOHourLog object</returns>
		private EmployeePTOHourLog GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeePTOHourLog employeePTOHourLogObject= new EmployeePTOHourLog();
					FillObject(employeePTOHourLogObject, reader);
					return employeePTOHourLogObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeePTOHourLog objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeePTOHourLog objects</returns>
		private EmployeePTOHourLogList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeePTOHourLog list
			EmployeePTOHourLogList list = new EmployeePTOHourLogList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeePTOHourLog employeePTOHourLogObject = new EmployeePTOHourLog();
					FillObject(employeePTOHourLogObject, reader);

					list.Add(employeePTOHourLogObject);
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