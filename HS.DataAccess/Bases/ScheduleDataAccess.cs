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
	public partial class ScheduleDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTSCHEDULE = "InsertSchedule";
		private const string UPDATESCHEDULE = "UpdateSchedule";
		private const string DELETESCHEDULE = "DeleteSchedule";
		private const string GETSCHEDULEBYID = "GetScheduleById";
		private const string GETALLSCHEDULE = "GetAllSchedule";
		private const string GETPAGEDSCHEDULE = "GetPagedSchedule";
		private const string GETSCHEDULEMAXIMUMID = "GetScheduleMaximumId";
		private const string GETSCHEDULEROWCOUNT = "GetScheduleRowCount";	
		private const string GETSCHEDULEBYQUERY = "GetScheduleByQuery";
		#endregion
		
		#region Constructors
		public ScheduleDataAccess(ClientContext context) : base(context) { }
		public ScheduleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="scheduleObject"></param>
		private void AddCommonParams(SqlCommand cmd, ScheduleBase scheduleObject)
		{	
			AddParameter(cmd, pGuid(ScheduleBase.Property_CompanyId, scheduleObject.CompanyId));
			AddParameter(cmd, pNVarChar(ScheduleBase.Property_Type, 50, scheduleObject.Type));
			AddParameter(cmd, pDateTime(ScheduleBase.Property_StartDate, scheduleObject.StartDate));
			AddParameter(cmd, pDateTime(ScheduleBase.Property_EndDate, scheduleObject.EndDate));
			AddParameter(cmd, pNVarChar(ScheduleBase.Property_Title, 500, scheduleObject.Title));
			AddParameter(cmd, pBool(ScheduleBase.Property_IsCompleted, scheduleObject.IsCompleted));
			AddParameter(cmd, pInt32(ScheduleBase.Property_LeadId, scheduleObject.LeadId));
			AddParameter(cmd, pBool(ScheduleBase.Property_IsFullDay, scheduleObject.IsFullDay));
			AddParameter(cmd, pNVarChar(ScheduleBase.Property_Identifier, 150, scheduleObject.Identifier));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Schedule
        /// </summary>
        /// <param name="scheduleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(ScheduleBase scheduleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTSCHEDULE);
	
				AddParameter(cmd, pInt32Out(ScheduleBase.Property_Id));
				AddCommonParams(cmd, scheduleObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					scheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					scheduleObject.Id = (Int32)GetOutParameter(cmd, ScheduleBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(scheduleObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Schedule
        /// </summary>
        /// <param name="scheduleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(ScheduleBase scheduleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATESCHEDULE);
				
				AddParameter(cmd, pInt32(ScheduleBase.Property_Id, scheduleObject.Id));
				AddCommonParams(cmd, scheduleObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					scheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(scheduleObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Schedule
        /// </summary>
        /// <param name="Id">Id of the Schedule object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETESCHEDULE);	
				
				AddParameter(cmd, pInt32(ScheduleBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Schedule), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Schedule object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Schedule object to retrieve</param>
        /// <returns>Schedule object, null if not found</returns>
		public Schedule Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETSCHEDULEBYID))
			{
				AddParameter( cmd, pInt32(ScheduleBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Schedule objects 
        /// </summary>
        /// <returns>A list of Schedule objects</returns>
		public ScheduleList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLSCHEDULE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Schedule objects by PageRequest
        /// </summary>
        /// <returns>A list of Schedule objects</returns>
		public ScheduleList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDSCHEDULE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				ScheduleList _ScheduleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _ScheduleList;
			}
		}
		
		/// <summary>
        /// Retrieves all Schedule objects by query String
        /// </summary>
        /// <returns>A list of Schedule objects</returns>
		public ScheduleList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETSCHEDULEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Schedule Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Schedule
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSCHEDULEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Schedule Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Schedule
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _ScheduleRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETSCHEDULEROWCOUNT))
			{
				SqlDataReader reader;
				_ScheduleRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _ScheduleRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Schedule object
        /// </summary>
        /// <param name="scheduleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(ScheduleBase scheduleObject, SqlDataReader reader, int start)
		{
			
				scheduleObject.Id = reader.GetInt32( start + 0 );			
				scheduleObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) scheduleObject.Type = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) scheduleObject.StartDate = reader.GetDateTime( start + 3 );			
				if(!reader.IsDBNull(4)) scheduleObject.EndDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) scheduleObject.Title = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) scheduleObject.IsCompleted = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) scheduleObject.LeadId = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) scheduleObject.IsFullDay = reader.GetBoolean( start + 8 );			
				if(!reader.IsDBNull(9)) scheduleObject.Identifier = reader.GetString( start + 9 );			
			FillBaseObject(scheduleObject, reader, (start + 10));

			
			scheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Schedule object
        /// </summary>
        /// <param name="scheduleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(ScheduleBase scheduleObject, SqlDataReader reader)
		{
			FillObject(scheduleObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Schedule object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Schedule object</returns>
		private Schedule GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Schedule scheduleObject= new Schedule();
					FillObject(scheduleObject, reader);
					return scheduleObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Schedule objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Schedule objects</returns>
		private ScheduleList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Schedule list
			ScheduleList list = new ScheduleList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Schedule scheduleObject = new Schedule();
					FillObject(scheduleObject, reader);

					list.Add(scheduleObject);
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
