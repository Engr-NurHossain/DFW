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
	public partial class TicketTimeClockDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETTIMECLOCK = "InsertTicketTimeClock";
		private const string UPDATETICKETTIMECLOCK = "UpdateTicketTimeClock";
		private const string DELETETICKETTIMECLOCK = "DeleteTicketTimeClock";
		private const string GETTICKETTIMECLOCKBYID = "GetTicketTimeClockById";
		private const string GETALLTICKETTIMECLOCK = "GetAllTicketTimeClock";
		private const string GETPAGEDTICKETTIMECLOCK = "GetPagedTicketTimeClock";
		private const string GETTICKETTIMECLOCKMAXIMUMID = "GetTicketTimeClockMaximumId";
		private const string GETTICKETTIMECLOCKROWCOUNT = "GetTicketTimeClockRowCount";	
		private const string GETTICKETTIMECLOCKBYQUERY = "GetTicketTimeClockByQuery";
		#endregion
		
		#region Constructors
		public TicketTimeClockDataAccess(ClientContext context) : base(context) { }
		public TicketTimeClockDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketTimeClockObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketTimeClockBase ticketTimeClockObject)
		{	
			AddParameter(cmd, pGuid(TicketTimeClockBase.Property_TicketId, ticketTimeClockObject.TicketId));
			AddParameter(cmd, pGuid(TicketTimeClockBase.Property_UserId, ticketTimeClockObject.UserId));
			AddParameter(cmd, pDateTime(TicketTimeClockBase.Property_Time, ticketTimeClockObject.Time));
			AddParameter(cmd, pNVarChar(TicketTimeClockBase.Property_Type, 50, ticketTimeClockObject.Type));
			AddParameter(cmd, pNVarChar(TicketTimeClockBase.Property_Lat, 150, ticketTimeClockObject.Lat));
			AddParameter(cmd, pNVarChar(TicketTimeClockBase.Property_Lng, 150, ticketTimeClockObject.Lng));
			AddParameter(cmd, pNVarChar(TicketTimeClockBase.Property_Note, ticketTimeClockObject.Note));
			AddParameter(cmd, pGuid(TicketTimeClockBase.Property_CreatedBy, ticketTimeClockObject.CreatedBy));
			AddParameter(cmd, pInt32(TicketTimeClockBase.Property_ClockedInMinutes, ticketTimeClockObject.ClockedInMinutes));
			AddParameter(cmd, pGuid(TicketTimeClockBase.Property_LastUpdateBy, ticketTimeClockObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(TicketTimeClockBase.Property_LastUpdatedDate, ticketTimeClockObject.LastUpdatedDate));
		}
        #endregion

        #region Insert Method
        /// <summary>
        /// Inserts TicketTimeClock
        /// </summary>
        /// <param name="ticketTimeClockObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
        public long Insert(TicketTimeClockBase ticketTimeClockObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETTIMECLOCK);
	
				AddParameter(cmd, pInt32Out(TicketTimeClockBase.Property_Id));
				AddCommonParams(cmd, ticketTimeClockObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketTimeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketTimeClockObject.Id = (Int32)GetOutParameter(cmd, TicketTimeClockBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketTimeClockObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketTimeClock
        /// </summary>
        /// <param name="ticketTimeClockObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketTimeClockBase ticketTimeClockObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETTIMECLOCK);
				
				AddParameter(cmd, pInt32(TicketTimeClockBase.Property_Id, ticketTimeClockObject.Id));
				AddCommonParams(cmd, ticketTimeClockObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketTimeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketTimeClockObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketTimeClock
        /// </summary>
        /// <param name="Id">Id of the TicketTimeClock object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETTIMECLOCK);	
				
				AddParameter(cmd, pInt32(TicketTimeClockBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketTimeClock), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketTimeClock object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketTimeClock object to retrieve</param>
        /// <returns>TicketTimeClock object, null if not found</returns>
		public TicketTimeClock Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETTIMECLOCKBYID))
			{
				AddParameter( cmd, pInt32(TicketTimeClockBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketTimeClock objects 
        /// </summary>
        /// <returns>A list of TicketTimeClock objects</returns>
		public TicketTimeClockList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETTIMECLOCK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketTimeClock objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketTimeClock objects</returns>
		public TicketTimeClockList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETTIMECLOCK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketTimeClockList _TicketTimeClockList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketTimeClockList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketTimeClock objects by query String
        /// </summary>
        /// <returns>A list of TicketTimeClock objects</returns>
		public TicketTimeClockList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETTIMECLOCKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketTimeClock Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketTimeClock
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETTIMECLOCKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketTimeClock Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketTimeClock
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketTimeClockRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETTIMECLOCKROWCOUNT))
			{
				SqlDataReader reader;
				_TicketTimeClockRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketTimeClockRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketTimeClock object
        /// </summary>
        /// <param name="ticketTimeClockObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketTimeClockBase ticketTimeClockObject, SqlDataReader reader, int start)
		{
			
				ticketTimeClockObject.Id = reader.GetInt32( start + 0 );			
				ticketTimeClockObject.TicketId = reader.GetGuid( start + 1 );			
				ticketTimeClockObject.UserId = reader.GetGuid( start + 2 );			
				ticketTimeClockObject.Time = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) ticketTimeClockObject.Type = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketTimeClockObject.Lat = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) ticketTimeClockObject.Lng = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) ticketTimeClockObject.Note = reader.GetString( start + 7 );			
				ticketTimeClockObject.CreatedBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) ticketTimeClockObject.ClockedInMinutes = reader.GetInt32( start + 9 );			
				ticketTimeClockObject.LastUpdateBy = reader.GetGuid( start + 10 );			
				ticketTimeClockObject.LastUpdatedDate = reader.GetDateTime( start + 11 );			
			FillBaseObject(ticketTimeClockObject, reader, (start + 12));

			
			ticketTimeClockObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketTimeClock object
        /// </summary>
        /// <param name="ticketTimeClockObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketTimeClockBase ticketTimeClockObject, SqlDataReader reader)
		{
			FillObject(ticketTimeClockObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketTimeClock object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketTimeClock object</returns>
		private TicketTimeClock GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketTimeClock ticketTimeClockObject= new TicketTimeClock();
					FillObject(ticketTimeClockObject, reader);
					return ticketTimeClockObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketTimeClock objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketTimeClock objects</returns>
		private TicketTimeClockList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketTimeClock list
			TicketTimeClockList list = new TicketTimeClockList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketTimeClock ticketTimeClockObject = new TicketTimeClock();
					FillObject(ticketTimeClockObject, reader);

					list.Add(ticketTimeClockObject);
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
