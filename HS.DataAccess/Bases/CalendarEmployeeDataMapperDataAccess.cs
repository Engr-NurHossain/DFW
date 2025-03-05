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
	public partial class CalendarEmployeeDataMapperDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCALENDAREMPLOYEEDATAMAPPER = "InsertCalendarEmployeeDataMapper";
		private const string UPDATECALENDAREMPLOYEEDATAMAPPER = "UpdateCalendarEmployeeDataMapper";
		private const string DELETECALENDAREMPLOYEEDATAMAPPER = "DeleteCalendarEmployeeDataMapper";
		private const string GETCALENDAREMPLOYEEDATAMAPPERBYID = "GetCalendarEmployeeDataMapperById";
		private const string GETALLCALENDAREMPLOYEEDATAMAPPER = "GetAllCalendarEmployeeDataMapper";
		private const string GETPAGEDCALENDAREMPLOYEEDATAMAPPER = "GetPagedCalendarEmployeeDataMapper";
		private const string GETCALENDAREMPLOYEEDATAMAPPERMAXIMUMID = "GetCalendarEmployeeDataMapperMaximumId";
		private const string GETCALENDAREMPLOYEEDATAMAPPERROWCOUNT = "GetCalendarEmployeeDataMapperRowCount";	
		private const string GETCALENDAREMPLOYEEDATAMAPPERBYQUERY = "GetCalendarEmployeeDataMapperByQuery";
		#endregion
		
		#region Constructors
		public CalendarEmployeeDataMapperDataAccess(ClientContext context) : base(context) { }
		public CalendarEmployeeDataMapperDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="calendarEmployeeDataMapperObject"></param>
		private void AddCommonParams(SqlCommand cmd, CalendarEmployeeDataMapperBase calendarEmployeeDataMapperObject)
		{	
			AddParameter(cmd, pGuid(CalendarEmployeeDataMapperBase.Property_UserId, calendarEmployeeDataMapperObject.UserId));
			AddParameter(cmd, pBool(CalendarEmployeeDataMapperBase.Property_IsActive, calendarEmployeeDataMapperObject.IsActive));
			AddParameter(cmd, pNVarChar(CalendarEmployeeDataMapperBase.Property_MapType, 50, calendarEmployeeDataMapperObject.MapType));
			AddParameter(cmd, pDateTime(CalendarEmployeeDataMapperBase.Property_CreatedDate, calendarEmployeeDataMapperObject.CreatedDate));
			AddParameter(cmd, pGuid(CalendarEmployeeDataMapperBase.Property_EmplyeeSelectedId, calendarEmployeeDataMapperObject.EmplyeeSelectedId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CalendarEmployeeDataMapper
        /// </summary>
        /// <param name="calendarEmployeeDataMapperObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CalendarEmployeeDataMapperBase calendarEmployeeDataMapperObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCALENDAREMPLOYEEDATAMAPPER);
	
				AddParameter(cmd, pInt32Out(CalendarEmployeeDataMapperBase.Property_Id));
				AddCommonParams(cmd, calendarEmployeeDataMapperObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					calendarEmployeeDataMapperObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					calendarEmployeeDataMapperObject.Id = (Int32)GetOutParameter(cmd, CalendarEmployeeDataMapperBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(calendarEmployeeDataMapperObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CalendarEmployeeDataMapper
        /// </summary>
        /// <param name="calendarEmployeeDataMapperObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CalendarEmployeeDataMapperBase calendarEmployeeDataMapperObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECALENDAREMPLOYEEDATAMAPPER);
				
				AddParameter(cmd, pInt32(CalendarEmployeeDataMapperBase.Property_Id, calendarEmployeeDataMapperObject.Id));
				AddCommonParams(cmd, calendarEmployeeDataMapperObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					calendarEmployeeDataMapperObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(calendarEmployeeDataMapperObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CalendarEmployeeDataMapper
        /// </summary>
        /// <param name="Id">Id of the CalendarEmployeeDataMapper object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECALENDAREMPLOYEEDATAMAPPER);	
				
				AddParameter(cmd, pInt32(CalendarEmployeeDataMapperBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CalendarEmployeeDataMapper), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CalendarEmployeeDataMapper object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CalendarEmployeeDataMapper object to retrieve</param>
        /// <returns>CalendarEmployeeDataMapper object, null if not found</returns>
		public CalendarEmployeeDataMapper Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCALENDAREMPLOYEEDATAMAPPERBYID))
			{
				AddParameter( cmd, pInt32(CalendarEmployeeDataMapperBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CalendarEmployeeDataMapper objects 
        /// </summary>
        /// <returns>A list of CalendarEmployeeDataMapper objects</returns>
		public CalendarEmployeeDataMapperList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCALENDAREMPLOYEEDATAMAPPER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CalendarEmployeeDataMapper objects by PageRequest
        /// </summary>
        /// <returns>A list of CalendarEmployeeDataMapper objects</returns>
		public CalendarEmployeeDataMapperList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCALENDAREMPLOYEEDATAMAPPER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CalendarEmployeeDataMapperList _CalendarEmployeeDataMapperList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CalendarEmployeeDataMapperList;
			}
		}
		
		/// <summary>
        /// Retrieves all CalendarEmployeeDataMapper objects by query String
        /// </summary>
        /// <returns>A list of CalendarEmployeeDataMapper objects</returns>
		public CalendarEmployeeDataMapperList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCALENDAREMPLOYEEDATAMAPPERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CalendarEmployeeDataMapper Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CalendarEmployeeDataMapper
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCALENDAREMPLOYEEDATAMAPPERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CalendarEmployeeDataMapper Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CalendarEmployeeDataMapper
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CalendarEmployeeDataMapperRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCALENDAREMPLOYEEDATAMAPPERROWCOUNT))
			{
				SqlDataReader reader;
				_CalendarEmployeeDataMapperRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CalendarEmployeeDataMapperRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CalendarEmployeeDataMapper object
        /// </summary>
        /// <param name="calendarEmployeeDataMapperObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CalendarEmployeeDataMapperBase calendarEmployeeDataMapperObject, SqlDataReader reader, int start)
		{
			
				calendarEmployeeDataMapperObject.Id = reader.GetInt32( start + 0 );			
				calendarEmployeeDataMapperObject.UserId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) calendarEmployeeDataMapperObject.IsActive = reader.GetBoolean( start + 2 );			
				if(!reader.IsDBNull(3)) calendarEmployeeDataMapperObject.MapType = reader.GetString( start + 3 );			
				calendarEmployeeDataMapperObject.CreatedDate = reader.GetDateTime( start + 4 );			
				calendarEmployeeDataMapperObject.EmplyeeSelectedId = reader.GetGuid( start + 5 );			
			FillBaseObject(calendarEmployeeDataMapperObject, reader, (start + 6));

			
			calendarEmployeeDataMapperObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CalendarEmployeeDataMapper object
        /// </summary>
        /// <param name="calendarEmployeeDataMapperObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CalendarEmployeeDataMapperBase calendarEmployeeDataMapperObject, SqlDataReader reader)
		{
			FillObject(calendarEmployeeDataMapperObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CalendarEmployeeDataMapper object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CalendarEmployeeDataMapper object</returns>
		private CalendarEmployeeDataMapper GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CalendarEmployeeDataMapper calendarEmployeeDataMapperObject= new CalendarEmployeeDataMapper();
					FillObject(calendarEmployeeDataMapperObject, reader);
					return calendarEmployeeDataMapperObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CalendarEmployeeDataMapper objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CalendarEmployeeDataMapper objects</returns>
		private CalendarEmployeeDataMapperList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CalendarEmployeeDataMapper list
			CalendarEmployeeDataMapperList list = new CalendarEmployeeDataMapperList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CalendarEmployeeDataMapper calendarEmployeeDataMapperObject = new CalendarEmployeeDataMapper();
					FillObject(calendarEmployeeDataMapperObject, reader);

					list.Add(calendarEmployeeDataMapperObject);
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
