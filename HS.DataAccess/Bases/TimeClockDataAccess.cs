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
	public partial class TimeClockDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTIMECLOCK = "InsertTimeClock";
		private const string UPDATETIMECLOCK = "UpdateTimeClock";
		private const string DELETETIMECLOCK = "DeleteTimeClock";
		private const string GETTIMECLOCKBYID = "GetTimeClockById";
		private const string GETALLTIMECLOCK = "GetAllTimeClock";
		private const string GETPAGEDTIMECLOCK = "GetPagedTimeClock";
		private const string GETTIMECLOCKMAXIMUMID = "GetTimeClockMaximumId";
		private const string GETTIMECLOCKROWCOUNT = "GetTimeClockRowCount";	
		private const string GETTIMECLOCKBYQUERY = "GetTimeClockByQuery";
		#endregion
		
		#region Constructors
		public TimeClockDataAccess(ClientContext context) : base(context) { }
		public TimeClockDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="timeClockObject"></param>
		private void AddCommonParams(SqlCommand cmd, TimeClockBase timeClockObject)
		{	
			AddParameter(cmd, pGuid(TimeClockBase.Property_UserId, timeClockObject.UserId));
			AddParameter(cmd, pDateTime(TimeClockBase.Property_Time, timeClockObject.Time));
			AddParameter(cmd, pNVarChar(TimeClockBase.Property_Type, 50, timeClockObject.Type));
			AddParameter(cmd, pNVarChar(TimeClockBase.Property_Lat, 150, timeClockObject.Lat));
			AddParameter(cmd, pNVarChar(TimeClockBase.Property_Lng, 150, timeClockObject.Lng));
			AddParameter(cmd, pNVarChar(TimeClockBase.Property_Note, timeClockObject.Note));
			AddParameter(cmd, pGuid(TimeClockBase.Property_CreatedBy, timeClockObject.CreatedBy));
			AddParameter(cmd, pInt32(TimeClockBase.Property_ClockedInMinutes, timeClockObject.ClockedInMinutes));
			AddParameter(cmd, pGuid(TimeClockBase.Property_LastUpdateBy, timeClockObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(TimeClockBase.Property_LastUpdatedDate, timeClockObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TimeClock
        /// </summary>
        /// <param name="timeClockObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TimeClockBase timeClockObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTIMECLOCK);
	
				AddParameter(cmd, pInt32Out(TimeClockBase.Property_Id));
				AddCommonParams(cmd, timeClockObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					timeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					timeClockObject.Id = (Int32)GetOutParameter(cmd, TimeClockBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(timeClockObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TimeClock
        /// </summary>
        /// <param name="timeClockObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TimeClockBase timeClockObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETIMECLOCK);
				
				AddParameter(cmd, pInt32(TimeClockBase.Property_Id, timeClockObject.Id));
				AddCommonParams(cmd, timeClockObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					timeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(timeClockObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TimeClock
        /// </summary>
        /// <param name="Id">Id of the TimeClock object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETIMECLOCK);	
				
				AddParameter(cmd, pInt32(TimeClockBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TimeClock), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TimeClock object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TimeClock object to retrieve</param>
        /// <returns>TimeClock object, null if not found</returns>
		public TimeClock Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTIMECLOCKBYID))
			{
				AddParameter( cmd, pInt32(TimeClockBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TimeClock objects 
        /// </summary>
        /// <returns>A list of TimeClock objects</returns>
		public TimeClockList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTIMECLOCK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TimeClock objects by PageRequest
        /// </summary>
        /// <returns>A list of TimeClock objects</returns>
		public TimeClockList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTIMECLOCK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TimeClockList _TimeClockList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TimeClockList;
			}
		}
		
		/// <summary>
        /// Retrieves all TimeClock objects by query String
        /// </summary>
        /// <returns>A list of TimeClock objects</returns>
		public TimeClockList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTIMECLOCKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TimeClock Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TimeClock
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTIMECLOCKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TimeClock Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TimeClock
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TimeClockRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTIMECLOCKROWCOUNT))
			{
				SqlDataReader reader;
				_TimeClockRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TimeClockRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TimeClock object
        /// </summary>
        /// <param name="timeClockObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TimeClockBase timeClockObject, SqlDataReader reader, int start)
		{
			
				timeClockObject.Id = reader.GetInt32( start + 0 );			
				timeClockObject.UserId = reader.GetGuid( start + 1 );			
				timeClockObject.Time = reader.GetDateTime( start + 2 );			
				if(!reader.IsDBNull(3)) timeClockObject.Type = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) timeClockObject.Lat = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) timeClockObject.Lng = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) timeClockObject.Note = reader.GetString( start + 6 );			
				timeClockObject.CreatedBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) timeClockObject.ClockedInMinutes = reader.GetInt32( start + 8 );			
				timeClockObject.LastUpdateBy = reader.GetGuid( start + 9 );			
				timeClockObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(timeClockObject, reader, (start + 11));

			
			timeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TimeClock object
        /// </summary>
        /// <param name="timeClockObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TimeClockBase timeClockObject, SqlDataReader reader)
		{
			FillObject(timeClockObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TimeClock object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TimeClock object</returns>
		private TimeClock GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TimeClock timeClockObject= new TimeClock();
					FillObject(timeClockObject, reader);
					return timeClockObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TimeClock objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TimeClock objects</returns>
		private TimeClockList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TimeClock list
			TimeClockList list = new TimeClockList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TimeClock timeClockObject = new TimeClock();
					FillObject(timeClockObject, reader);

					list.Add(timeClockObject);
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
