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
	public partial class TicketReplyDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETREPLY = "InsertTicketReply";
		private const string UPDATETICKETREPLY = "UpdateTicketReply";
		private const string DELETETICKETREPLY = "DeleteTicketReply";
		private const string GETTICKETREPLYBYID = "GetTicketReplyById";
		private const string GETALLTICKETREPLY = "GetAllTicketReply";
		private const string GETPAGEDTICKETREPLY = "GetPagedTicketReply";
		private const string GETTICKETREPLYMAXIMUMID = "GetTicketReplyMaximumId";
		private const string GETTICKETREPLYROWCOUNT = "GetTicketReplyRowCount";	
		private const string GETTICKETREPLYBYQUERY = "GetTicketReplyByQuery";
		#endregion
		
		#region Constructors
		public TicketReplyDataAccess(ClientContext context) : base(context) { }
		public TicketReplyDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketReplyObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketReplyBase ticketReplyObject)
		{	
			AddParameter(cmd, pGuid(TicketReplyBase.Property_TicketId, ticketReplyObject.TicketId));
			AddParameter(cmd, pGuid(TicketReplyBase.Property_UserId, ticketReplyObject.UserId));
			AddParameter(cmd, pDateTime(TicketReplyBase.Property_RepliedDate, ticketReplyObject.RepliedDate));
			AddParameter(cmd, pNVarChar(TicketReplyBase.Property_Message, ticketReplyObject.Message));
			AddParameter(cmd, pBool(TicketReplyBase.Property_IsPrivate, ticketReplyObject.IsPrivate));
			AddParameter(cmd, pNVarChar(TicketReplyBase.Property_ReplyType, 100, ticketReplyObject.ReplyType));
			AddParameter(cmd, pNVarChar(TicketReplyBase.Property_LatLng, 100, ticketReplyObject.LatLng));
			AddParameter(cmd, pBool(TicketReplyBase.Property_IsOverview, ticketReplyObject.IsOverview));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketReply
        /// </summary>
        /// <param name="ticketReplyObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketReplyBase ticketReplyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETREPLY);
	
				AddParameter(cmd, pInt32Out(TicketReplyBase.Property_Id));
				AddCommonParams(cmd, ticketReplyObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketReplyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketReplyObject.Id = (Int32)GetOutParameter(cmd, TicketReplyBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketReplyObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketReply
        /// </summary>
        /// <param name="ticketReplyObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketReplyBase ticketReplyObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETREPLY);
				
				AddParameter(cmd, pInt32(TicketReplyBase.Property_Id, ticketReplyObject.Id));
				AddCommonParams(cmd, ticketReplyObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketReplyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketReplyObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketReply
        /// </summary>
        /// <param name="Id">Id of the TicketReply object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETREPLY);	
				
				AddParameter(cmd, pInt32(TicketReplyBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketReply), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketReply object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketReply object to retrieve</param>
        /// <returns>TicketReply object, null if not found</returns>
		public TicketReply Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETREPLYBYID))
			{
				AddParameter( cmd, pInt32(TicketReplyBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketReply objects 
        /// </summary>
        /// <returns>A list of TicketReply objects</returns>
		public TicketReplyList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETREPLY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketReply objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketReply objects</returns>
		public TicketReplyList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETREPLY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketReplyList _TicketReplyList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketReplyList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketReply objects by query String
        /// </summary>
        /// <returns>A list of TicketReply objects</returns>
		public TicketReplyList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETREPLYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketReply Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketReply
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETREPLYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketReply Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketReply
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketReplyRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETREPLYROWCOUNT))
			{
				SqlDataReader reader;
				_TicketReplyRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketReplyRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketReply object
        /// </summary>
        /// <param name="ticketReplyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketReplyBase ticketReplyObject, SqlDataReader reader, int start)
		{
			
				ticketReplyObject.Id = reader.GetInt32( start + 0 );			
				ticketReplyObject.TicketId = reader.GetGuid( start + 1 );			
				ticketReplyObject.UserId = reader.GetGuid( start + 2 );			
				ticketReplyObject.RepliedDate = reader.GetDateTime( start + 3 );			
				ticketReplyObject.Message = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketReplyObject.IsPrivate = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) ticketReplyObject.ReplyType = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) ticketReplyObject.LatLng = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) ticketReplyObject.IsOverview = reader.GetBoolean( start + 8 );			
			FillBaseObject(ticketReplyObject, reader, (start + 9));

			
			ticketReplyObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketReply object
        /// </summary>
        /// <param name="ticketReplyObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketReplyBase ticketReplyObject, SqlDataReader reader)
		{
			FillObject(ticketReplyObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketReply object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketReply object</returns>
		private TicketReply GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketReply ticketReplyObject= new TicketReply();
					FillObject(ticketReplyObject, reader);
					return ticketReplyObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketReply objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketReply objects</returns>
		private TicketReplyList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketReply list
			TicketReplyList list = new TicketReplyList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketReply ticketReplyObject = new TicketReply();
					FillObject(ticketReplyObject, reader);

					list.Add(ticketReplyObject);
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
