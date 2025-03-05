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
	public partial class RescheduleTicketDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESCHEDULETICKET = "InsertRescheduleTicket";
		private const string UPDATERESCHEDULETICKET = "UpdateRescheduleTicket";
		private const string DELETERESCHEDULETICKET = "DeleteRescheduleTicket";
		private const string GETRESCHEDULETICKETBYID = "GetRescheduleTicketById";
		private const string GETALLRESCHEDULETICKET = "GetAllRescheduleTicket";
		private const string GETPAGEDRESCHEDULETICKET = "GetPagedRescheduleTicket";
		private const string GETRESCHEDULETICKETMAXIMUMID = "GetRescheduleTicketMaximumId";
		private const string GETRESCHEDULETICKETROWCOUNT = "GetRescheduleTicketRowCount";	
		private const string GETRESCHEDULETICKETBYQUERY = "GetRescheduleTicketByQuery";
		#endregion
		
		#region Constructors
		public RescheduleTicketDataAccess(ClientContext context) : base(context) { }
		public RescheduleTicketDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="rescheduleTicketObject"></param>
		private void AddCommonParams(SqlCommand cmd, RescheduleTicketBase rescheduleTicketObject)
		{	
			AddParameter(cmd, pGuid(RescheduleTicketBase.Property_RescheduleId, rescheduleTicketObject.RescheduleId));
			AddParameter(cmd, pGuid(RescheduleTicketBase.Property_TicketId, rescheduleTicketObject.TicketId));
			AddParameter(cmd, pNVarChar(RescheduleTicketBase.Property_Reason, rescheduleTicketObject.Reason));
			AddParameter(cmd, pBool(RescheduleTicketBase.Property_IsPay, rescheduleTicketObject.IsPay));
			AddParameter(cmd, pGuid(RescheduleTicketBase.Property_CreatedBy, rescheduleTicketObject.CreatedBy));
			AddParameter(cmd, pDateTime(RescheduleTicketBase.Property_CreatedDate, rescheduleTicketObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RescheduleTicket
        /// </summary>
        /// <param name="rescheduleTicketObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RescheduleTicketBase rescheduleTicketObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESCHEDULETICKET);
	
				AddParameter(cmd, pInt32Out(RescheduleTicketBase.Property_Id));
				AddCommonParams(cmd, rescheduleTicketObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					rescheduleTicketObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					rescheduleTicketObject.Id = (Int32)GetOutParameter(cmd, RescheduleTicketBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(rescheduleTicketObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RescheduleTicket
        /// </summary>
        /// <param name="rescheduleTicketObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RescheduleTicketBase rescheduleTicketObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESCHEDULETICKET);
				
				AddParameter(cmd, pInt32(RescheduleTicketBase.Property_Id, rescheduleTicketObject.Id));
				AddCommonParams(cmd, rescheduleTicketObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					rescheduleTicketObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(rescheduleTicketObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RescheduleTicket
        /// </summary>
        /// <param name="Id">Id of the RescheduleTicket object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESCHEDULETICKET);	
				
				AddParameter(cmd, pInt32(RescheduleTicketBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RescheduleTicket), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RescheduleTicket object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RescheduleTicket object to retrieve</param>
        /// <returns>RescheduleTicket object, null if not found</returns>
		public RescheduleTicket Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULETICKETBYID))
			{
				AddParameter( cmd, pInt32(RescheduleTicketBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RescheduleTicket objects 
        /// </summary>
        /// <returns>A list of RescheduleTicket objects</returns>
		public RescheduleTicketList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESCHEDULETICKET))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RescheduleTicket objects by PageRequest
        /// </summary>
        /// <returns>A list of RescheduleTicket objects</returns>
		public RescheduleTicketList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESCHEDULETICKET))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RescheduleTicketList _RescheduleTicketList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RescheduleTicketList;
			}
		}
		
		/// <summary>
        /// Retrieves all RescheduleTicket objects by query String
        /// </summary>
        /// <returns>A list of RescheduleTicket objects</returns>
		public RescheduleTicketList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULETICKETBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RescheduleTicket Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RescheduleTicket
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULETICKETMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RescheduleTicket Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RescheduleTicket
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RescheduleTicketRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESCHEDULETICKETROWCOUNT))
			{
				SqlDataReader reader;
				_RescheduleTicketRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RescheduleTicketRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RescheduleTicket object
        /// </summary>
        /// <param name="rescheduleTicketObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RescheduleTicketBase rescheduleTicketObject, SqlDataReader reader, int start)
		{
			
				rescheduleTicketObject.Id = reader.GetInt32( start + 0 );			
				rescheduleTicketObject.RescheduleId = reader.GetGuid( start + 1 );			
				rescheduleTicketObject.TicketId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) rescheduleTicketObject.Reason = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) rescheduleTicketObject.IsPay = reader.GetBoolean( start + 4 );			
				rescheduleTicketObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) rescheduleTicketObject.CreatedDate = reader.GetDateTime( start + 6 );			
			FillBaseObject(rescheduleTicketObject, reader, (start + 7));

			
			rescheduleTicketObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RescheduleTicket object
        /// </summary>
        /// <param name="rescheduleTicketObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RescheduleTicketBase rescheduleTicketObject, SqlDataReader reader)
		{
			FillObject(rescheduleTicketObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RescheduleTicket object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RescheduleTicket object</returns>
		private RescheduleTicket GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RescheduleTicket rescheduleTicketObject= new RescheduleTicket();
					FillObject(rescheduleTicketObject, reader);
					return rescheduleTicketObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RescheduleTicket objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RescheduleTicket objects</returns>
		private RescheduleTicketList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RescheduleTicket list
			RescheduleTicketList list = new RescheduleTicketList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RescheduleTicket rescheduleTicketObject = new RescheduleTicket();
					FillObject(rescheduleTicketObject, reader);

					list.Add(rescheduleTicketObject);
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
