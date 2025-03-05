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
	public partial class TicketUserDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETUSER = "InsertTicketUser";
		private const string UPDATETICKETUSER = "UpdateTicketUser";
		private const string DELETETICKETUSER = "DeleteTicketUser";
		private const string GETTICKETUSERBYID = "GetTicketUserById";
		private const string GETALLTICKETUSER = "GetAllTicketUser";
		private const string GETPAGEDTICKETUSER = "GetPagedTicketUser";
		private const string GETTICKETUSERMAXIMUMID = "GetTicketUserMaximumId";
		private const string GETTICKETUSERROWCOUNT = "GetTicketUserRowCount";	
		private const string GETTICKETUSERBYQUERY = "GetTicketUserByQuery";
		#endregion
		
		#region Constructors
		public TicketUserDataAccess(ClientContext context) : base(context) { }
		public TicketUserDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketUserObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketUserBase ticketUserObject)
		{	
			AddParameter(cmd, pGuid(TicketUserBase.Property_TiketId, ticketUserObject.TiketId));
			AddParameter(cmd, pGuid(TicketUserBase.Property_UserId, ticketUserObject.UserId));
			AddParameter(cmd, pBool(TicketUserBase.Property_IsPrimary, ticketUserObject.IsPrimary));
			AddParameter(cmd, pDateTime(TicketUserBase.Property_AddedDate, ticketUserObject.AddedDate));
			AddParameter(cmd, pGuid(TicketUserBase.Property_AddedBy, ticketUserObject.AddedBy));
			AddParameter(cmd, pBool(TicketUserBase.Property_NotificationOnly, ticketUserObject.NotificationOnly));
			AddParameter(cmd, pBool(TicketUserBase.Property_IsReschedulePay, ticketUserObject.IsReschedulePay));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketUser
        /// </summary>
        /// <param name="ticketUserObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketUserBase ticketUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETUSER);
	
				AddParameter(cmd, pInt32Out(TicketUserBase.Property_Id));
				AddCommonParams(cmd, ticketUserObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketUserObject.Id = (Int32)GetOutParameter(cmd, TicketUserBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketUserObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketUser
        /// </summary>
        /// <param name="ticketUserObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketUserBase ticketUserObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETUSER);
				
				AddParameter(cmd, pInt32(TicketUserBase.Property_Id, ticketUserObject.Id));
				AddCommonParams(cmd, ticketUserObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketUserObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketUser
        /// </summary>
        /// <param name="Id">Id of the TicketUser object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETUSER);	
				
				AddParameter(cmd, pInt32(TicketUserBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketUser), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketUser object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketUser object to retrieve</param>
        /// <returns>TicketUser object, null if not found</returns>
		public TicketUser Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETUSERBYID))
			{
				AddParameter( cmd, pInt32(TicketUserBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketUser objects 
        /// </summary>
        /// <returns>A list of TicketUser objects</returns>
		public TicketUserList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETUSER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketUser objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketUser objects</returns>
		public TicketUserList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETUSER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketUserList _TicketUserList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketUserList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketUser objects by query String
        /// </summary>
        /// <returns>A list of TicketUser objects</returns>
		public TicketUserList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETUSERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketUser Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETUSERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketUser Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketUser
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketUserRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETUSERROWCOUNT))
			{
				SqlDataReader reader;
				_TicketUserRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketUserRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketUser object
        /// </summary>
        /// <param name="ticketUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketUserBase ticketUserObject, SqlDataReader reader, int start)
		{
			
				ticketUserObject.Id = reader.GetInt32( start + 0 );			
				ticketUserObject.TiketId = reader.GetGuid( start + 1 );			
				ticketUserObject.UserId = reader.GetGuid( start + 2 );			
				ticketUserObject.IsPrimary = reader.GetBoolean( start + 3 );			
				ticketUserObject.AddedDate = reader.GetDateTime( start + 4 );			
				ticketUserObject.AddedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) ticketUserObject.NotificationOnly = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) ticketUserObject.IsReschedulePay = reader.GetBoolean( start + 7 );			
			FillBaseObject(ticketUserObject, reader, (start + 8));

			
			ticketUserObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketUser object
        /// </summary>
        /// <param name="ticketUserObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketUserBase ticketUserObject, SqlDataReader reader)
		{
			FillObject(ticketUserObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketUser object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketUser object</returns>
		private TicketUser GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketUser ticketUserObject= new TicketUser();
					FillObject(ticketUserObject, reader);
					return ticketUserObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketUser objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketUser objects</returns>
		private TicketUserList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketUser list
			TicketUserList list = new TicketUserList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketUser ticketUserObject = new TicketUser();
					FillObject(ticketUserObject, reader);

					list.Add(ticketUserObject);
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
