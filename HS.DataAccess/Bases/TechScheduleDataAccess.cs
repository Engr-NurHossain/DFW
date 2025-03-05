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
	public partial class TechScheduleDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTECHSCHEDULE = "InsertTechSchedule";
		private const string UPDATETECHSCHEDULE = "UpdateTechSchedule";
		private const string DELETETECHSCHEDULE = "DeleteTechSchedule";
		private const string GETTECHSCHEDULEBYID = "GetTechScheduleById";
		private const string GETALLTECHSCHEDULE = "GetAllTechSchedule";
		private const string GETPAGEDTECHSCHEDULE = "GetPagedTechSchedule";
		private const string GETTECHSCHEDULEMAXIMUMID = "GetTechScheduleMaximumId";
		private const string GETTECHSCHEDULEROWCOUNT = "GetTechScheduleRowCount";	
		private const string GETTECHSCHEDULEBYQUERY = "GetTechScheduleByQuery";
		#endregion
		
		#region Constructors
		public TechScheduleDataAccess(ClientContext context) : base(context) { }
		public TechScheduleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="techScheduleObject"></param>
		private void AddCommonParams(SqlCommand cmd, TechScheduleBase techScheduleObject)
		{	
			AddParameter(cmd, pGuid(TechScheduleBase.Property_EmployeeId, techScheduleObject.EmployeeId));
			AddParameter(cmd, pGuid(TechScheduleBase.Property_CustomerId, techScheduleObject.CustomerId));
			AddParameter(cmd, pGuid(TechScheduleBase.Property_CompanyId, techScheduleObject.CompanyId));
			AddParameter(cmd, pDateTime(TechScheduleBase.Property_InstallDate, techScheduleObject.InstallDate));
			AddParameter(cmd, pNVarChar(TechScheduleBase.Property_ArrivalTime, 50, techScheduleObject.ArrivalTime));
			AddParameter(cmd, pNVarChar(TechScheduleBase.Property_DepartureTime, 50, techScheduleObject.DepartureTime));
			AddParameter(cmd, pNVarChar(TechScheduleBase.Property_EstimatedArrival, 50, techScheduleObject.EstimatedArrival));
			AddParameter(cmd, pBool(TechScheduleBase.Property_CheckScheduleConflict, techScheduleObject.CheckScheduleConflict));
			AddParameter(cmd, pBool(TechScheduleBase.Property_IsSendNotification, techScheduleObject.IsSendNotification));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TechSchedule
        /// </summary>
        /// <param name="techScheduleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TechScheduleBase techScheduleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTECHSCHEDULE);
	
				AddParameter(cmd, pInt32Out(TechScheduleBase.Property_Id));
				AddCommonParams(cmd, techScheduleObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					techScheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					techScheduleObject.Id = (Int32)GetOutParameter(cmd, TechScheduleBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(techScheduleObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TechSchedule
        /// </summary>
        /// <param name="techScheduleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TechScheduleBase techScheduleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETECHSCHEDULE);
				
				AddParameter(cmd, pInt32(TechScheduleBase.Property_Id, techScheduleObject.Id));
				AddCommonParams(cmd, techScheduleObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					techScheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(techScheduleObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TechSchedule
        /// </summary>
        /// <param name="Id">Id of the TechSchedule object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETECHSCHEDULE);	
				
				AddParameter(cmd, pInt32(TechScheduleBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TechSchedule), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TechSchedule object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TechSchedule object to retrieve</param>
        /// <returns>TechSchedule object, null if not found</returns>
		public TechSchedule Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTECHSCHEDULEBYID))
			{
				AddParameter( cmd, pInt32(TechScheduleBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TechSchedule objects 
        /// </summary>
        /// <returns>A list of TechSchedule objects</returns>
		public TechScheduleList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTECHSCHEDULE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TechSchedule objects by PageRequest
        /// </summary>
        /// <returns>A list of TechSchedule objects</returns>
		public TechScheduleList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTECHSCHEDULE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TechScheduleList _TechScheduleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TechScheduleList;
			}
		}
		
		/// <summary>
        /// Retrieves all TechSchedule objects by query String
        /// </summary>
        /// <returns>A list of TechSchedule objects</returns>
		public TechScheduleList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTECHSCHEDULEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TechSchedule Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TechSchedule
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTECHSCHEDULEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TechSchedule Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TechSchedule
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TechScheduleRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTECHSCHEDULEROWCOUNT))
			{
				SqlDataReader reader;
				_TechScheduleRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TechScheduleRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TechSchedule object
        /// </summary>
        /// <param name="techScheduleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TechScheduleBase techScheduleObject, SqlDataReader reader, int start)
		{
			
				techScheduleObject.Id = reader.GetInt32( start + 0 );			
				techScheduleObject.EmployeeId = reader.GetGuid( start + 1 );			
				techScheduleObject.CustomerId = reader.GetGuid( start + 2 );			
				techScheduleObject.CompanyId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) techScheduleObject.InstallDate = reader.GetDateTime( start + 4 );			
				if(!reader.IsDBNull(5)) techScheduleObject.ArrivalTime = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) techScheduleObject.DepartureTime = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) techScheduleObject.EstimatedArrival = reader.GetString( start + 7 );			
				techScheduleObject.CheckScheduleConflict = reader.GetBoolean( start + 8 );			
				techScheduleObject.IsSendNotification = reader.GetBoolean( start + 9 );			
			FillBaseObject(techScheduleObject, reader, (start + 10));

			
			techScheduleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TechSchedule object
        /// </summary>
        /// <param name="techScheduleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TechScheduleBase techScheduleObject, SqlDataReader reader)
		{
			FillObject(techScheduleObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TechSchedule object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TechSchedule object</returns>
		private TechSchedule GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TechSchedule techScheduleObject= new TechSchedule();
					FillObject(techScheduleObject, reader);
					return techScheduleObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TechSchedule objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TechSchedule objects</returns>
		private TechScheduleList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TechSchedule list
			TechScheduleList list = new TechScheduleList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TechSchedule techScheduleObject = new TechSchedule();
					FillObject(techScheduleObject, reader);

					list.Add(techScheduleObject);
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
