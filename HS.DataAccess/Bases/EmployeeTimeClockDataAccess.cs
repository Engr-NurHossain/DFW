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
	public partial class EmployeeTimeClockDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTEMPLOYEETIMECLOCK = "InsertEmployeeTimeClock";
		private const string UPDATEEMPLOYEETIMECLOCK = "UpdateEmployeeTimeClock";
		private const string DELETEEMPLOYEETIMECLOCK = "DeleteEmployeeTimeClock";
		private const string GETEMPLOYEETIMECLOCKBYID = "GetEmployeeTimeClockById";
		private const string GETALLEMPLOYEETIMECLOCK = "GetAllEmployeeTimeClock";
		private const string GETPAGEDEMPLOYEETIMECLOCK = "GetPagedEmployeeTimeClock";
		private const string GETEMPLOYEETIMECLOCKMAXIMUMID = "GetEmployeeTimeClockMaximumId";
		private const string GETEMPLOYEETIMECLOCKROWCOUNT = "GetEmployeeTimeClockRowCount";	
		private const string GETEMPLOYEETIMECLOCKBYQUERY = "GetEmployeeTimeClockByQuery";
		#endregion
		
		#region Constructors
		public EmployeeTimeClockDataAccess(ClientContext context) : base(context) { }
        public EmployeeTimeClockDataAccess(string ConStr) : base(ConStr) { }
        public EmployeeTimeClockDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="employeeTimeClockObject"></param>
		private void AddCommonParams(SqlCommand cmd, EmployeeTimeClockBase employeeTimeClockObject)
		{	
			AddParameter(cmd, pGuid(EmployeeTimeClockBase.Property_UserId, employeeTimeClockObject.UserId));
			AddParameter(cmd, pDateTime(EmployeeTimeClockBase.Property_ClockInTime, employeeTimeClockObject.ClockInTime));
			AddParameter(cmd, pDateTime(EmployeeTimeClockBase.Property_ClockOutTime, employeeTimeClockObject.ClockOutTime));
			AddParameter(cmd, pNVarChar(EmployeeTimeClockBase.Property_ClockInLat, 150, employeeTimeClockObject.ClockInLat));
			AddParameter(cmd, pNVarChar(EmployeeTimeClockBase.Property_ClockInLng, 150, employeeTimeClockObject.ClockInLng));
			AddParameter(cmd, pNVarChar(EmployeeTimeClockBase.Property_ClockOutLat, 150, employeeTimeClockObject.ClockOutLat));
			AddParameter(cmd, pNVarChar(EmployeeTimeClockBase.Property_ClockOutLng, 150, employeeTimeClockObject.ClockOutLng));
			AddParameter(cmd, pNVarChar(EmployeeTimeClockBase.Property_ClockInNote, employeeTimeClockObject.ClockInNote));
			AddParameter(cmd, pNVarChar(EmployeeTimeClockBase.Property_ClockOutNote, employeeTimeClockObject.ClockOutNote));
			AddParameter(cmd, pGuid(EmployeeTimeClockBase.Property_ClockInCreatedBy, employeeTimeClockObject.ClockInCreatedBy));
			AddParameter(cmd, pGuid(EmployeeTimeClockBase.Property_ClockOutCreatedBy, employeeTimeClockObject.ClockOutCreatedBy));
			AddParameter(cmd, pInt32(EmployeeTimeClockBase.Property_ClockedInSeconds, employeeTimeClockObject.ClockedInSeconds));
			AddParameter(cmd, pGuid(EmployeeTimeClockBase.Property_LastUpdateBy, employeeTimeClockObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(EmployeeTimeClockBase.Property_LastUpdatedDate, employeeTimeClockObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts EmployeeTimeClock
        /// </summary>
        /// <param name="employeeTimeClockObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(EmployeeTimeClockBase employeeTimeClockObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTEMPLOYEETIMECLOCK);
	
				AddParameter(cmd, pInt32Out(EmployeeTimeClockBase.Property_Id));
				AddCommonParams(cmd, employeeTimeClockObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					employeeTimeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					employeeTimeClockObject.Id = (Int32)GetOutParameter(cmd, EmployeeTimeClockBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(employeeTimeClockObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates EmployeeTimeClock
        /// </summary>
        /// <param name="employeeTimeClockObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(EmployeeTimeClockBase employeeTimeClockObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEEMPLOYEETIMECLOCK);
				
				AddParameter(cmd, pInt32(EmployeeTimeClockBase.Property_Id, employeeTimeClockObject.Id));
				AddCommonParams(cmd, employeeTimeClockObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					employeeTimeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(employeeTimeClockObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes EmployeeTimeClock
        /// </summary>
        /// <param name="Id">Id of the EmployeeTimeClock object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEEMPLOYEETIMECLOCK);	
				
				AddParameter(cmd, pInt32(EmployeeTimeClockBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(EmployeeTimeClock), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves EmployeeTimeClock object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the EmployeeTimeClock object to retrieve</param>
        /// <returns>EmployeeTimeClock object, null if not found</returns>
		public EmployeeTimeClock Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKBYID))
			{
				AddParameter( cmd, pInt32(EmployeeTimeClockBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all EmployeeTimeClock objects 
        /// </summary>
        /// <returns>A list of EmployeeTimeClock objects</returns>
		public EmployeeTimeClockList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLEMPLOYEETIMECLOCK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all EmployeeTimeClock objects by PageRequest
        /// </summary>
        /// <returns>A list of EmployeeTimeClock objects</returns>
		public EmployeeTimeClockList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDEMPLOYEETIMECLOCK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				EmployeeTimeClockList _EmployeeTimeClockList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _EmployeeTimeClockList;
			}
		}
		
		/// <summary>
        /// Retrieves all EmployeeTimeClock objects by query String
        /// </summary>
        /// <returns>A list of EmployeeTimeClock objects</returns>
		public EmployeeTimeClockList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get EmployeeTimeClock Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of EmployeeTimeClock
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get EmployeeTimeClock Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of EmployeeTimeClock
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _EmployeeTimeClockRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETEMPLOYEETIMECLOCKROWCOUNT))
			{
				SqlDataReader reader;
				_EmployeeTimeClockRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _EmployeeTimeClockRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills EmployeeTimeClock object
        /// </summary>
        /// <param name="employeeTimeClockObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(EmployeeTimeClockBase employeeTimeClockObject, SqlDataReader reader, int start)
		{
			
				employeeTimeClockObject.Id = reader.GetInt32( start + 0 );			
				employeeTimeClockObject.UserId = reader.GetGuid( start + 1 );			
				employeeTimeClockObject.ClockInTime = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) employeeTimeClockObject.ClockOutTime = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) employeeTimeClockObject.ClockInLat = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) employeeTimeClockObject.ClockInLng = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) employeeTimeClockObject.ClockOutLat = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) employeeTimeClockObject.ClockOutLng = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) employeeTimeClockObject.ClockInNote = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) employeeTimeClockObject.ClockOutNote = reader.GetString( start + 9 );			
				employeeTimeClockObject.ClockInCreatedBy = reader.GetGuid( start + 10 );			
				employeeTimeClockObject.ClockOutCreatedBy = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) employeeTimeClockObject.ClockedInSeconds = reader.GetInt32( start + 12 );			
				employeeTimeClockObject.LastUpdateBy = reader.GetGuid( start + 13 );			
				employeeTimeClockObject.LastUpdatedDate = reader.GetDateTime( start + 14 );			
			FillBaseObject(employeeTimeClockObject, reader, (start + 15));

			
			employeeTimeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills EmployeeTimeClock object
        /// </summary>
        /// <param name="employeeTimeClockObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(EmployeeTimeClockBase employeeTimeClockObject, SqlDataReader reader)
		{
			FillObject(employeeTimeClockObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves EmployeeTimeClock object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>EmployeeTimeClock object</returns>
		private EmployeeTimeClock GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					EmployeeTimeClock employeeTimeClockObject= new EmployeeTimeClock();
					FillObject(employeeTimeClockObject, reader);
					return employeeTimeClockObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of EmployeeTimeClock objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of EmployeeTimeClock objects</returns>
		private EmployeeTimeClockList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//EmployeeTimeClock list
			EmployeeTimeClockList list = new EmployeeTimeClockList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					EmployeeTimeClock employeeTimeClockObject = new EmployeeTimeClock();
					FillObject(employeeTimeClockObject, reader);

					list.Add(employeeTimeClockObject);
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
