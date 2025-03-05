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
	public partial class TicketCustomerNotificationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETCUSTOMERNOTIFICATION = "InsertTicketCustomerNotification";
		private const string UPDATETICKETCUSTOMERNOTIFICATION = "UpdateTicketCustomerNotification";
		private const string DELETETICKETCUSTOMERNOTIFICATION = "DeleteTicketCustomerNotification";
		private const string GETTICKETCUSTOMERNOTIFICATIONBYID = "GetTicketCustomerNotificationById";
		private const string GETALLTICKETCUSTOMERNOTIFICATION = "GetAllTicketCustomerNotification";
		private const string GETPAGEDTICKETCUSTOMERNOTIFICATION = "GetPagedTicketCustomerNotification";
		private const string GETTICKETCUSTOMERNOTIFICATIONMAXIMUMID = "GetTicketCustomerNotificationMaximumId";
		private const string GETTICKETCUSTOMERNOTIFICATIONROWCOUNT = "GetTicketCustomerNotificationRowCount";	
		private const string GETTICKETCUSTOMERNOTIFICATIONBYQUERY = "GetTicketCustomerNotificationByQuery";
		#endregion
		
		#region Constructors
		public TicketCustomerNotificationDataAccess(ClientContext context) : base(context) { }
		public TicketCustomerNotificationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketCustomerNotificationObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketCustomerNotificationBase ticketCustomerNotificationObject)
		{	
			AddParameter(cmd, pNVarChar(TicketCustomerNotificationBase.Property_TicketStatus, 50, ticketCustomerNotificationObject.TicketStatus));
			AddParameter(cmd, pNVarChar(TicketCustomerNotificationBase.Property_Text, 500, ticketCustomerNotificationObject.Text));
			AddParameter(cmd, pNVarChar(TicketCustomerNotificationBase.Property_Email, ticketCustomerNotificationObject.Email));
			AddParameter(cmd, pGuid(TicketCustomerNotificationBase.Property_CreatedBy, ticketCustomerNotificationObject.CreatedBy));
			AddParameter(cmd, pDateTime(TicketCustomerNotificationBase.Property_CreatedDate, ticketCustomerNotificationObject.CreatedDate));
			AddParameter(cmd, pGuid(TicketCustomerNotificationBase.Property_LastUpdatedBy, ticketCustomerNotificationObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(TicketCustomerNotificationBase.Property_LastUpdatedDate, ticketCustomerNotificationObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(TicketCustomerNotificationBase.Property_TicketType, 50, ticketCustomerNotificationObject.TicketType));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketCustomerNotification
        /// </summary>
        /// <param name="ticketCustomerNotificationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketCustomerNotificationBase ticketCustomerNotificationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETCUSTOMERNOTIFICATION);
	
				AddParameter(cmd, pInt32Out(TicketCustomerNotificationBase.Property_Id));
				AddCommonParams(cmd, ticketCustomerNotificationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketCustomerNotificationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketCustomerNotificationObject.Id = (Int32)GetOutParameter(cmd, TicketCustomerNotificationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketCustomerNotificationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketCustomerNotification
        /// </summary>
        /// <param name="ticketCustomerNotificationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketCustomerNotificationBase ticketCustomerNotificationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETCUSTOMERNOTIFICATION);
				
				AddParameter(cmd, pInt32(TicketCustomerNotificationBase.Property_Id, ticketCustomerNotificationObject.Id));
				AddCommonParams(cmd, ticketCustomerNotificationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketCustomerNotificationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketCustomerNotificationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketCustomerNotification
        /// </summary>
        /// <param name="Id">Id of the TicketCustomerNotification object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETCUSTOMERNOTIFICATION);	
				
				AddParameter(cmd, pInt32(TicketCustomerNotificationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketCustomerNotification), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketCustomerNotification object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketCustomerNotification object to retrieve</param>
        /// <returns>TicketCustomerNotification object, null if not found</returns>
		public TicketCustomerNotification Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETCUSTOMERNOTIFICATIONBYID))
			{
				AddParameter( cmd, pInt32(TicketCustomerNotificationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketCustomerNotification objects 
        /// </summary>
        /// <returns>A list of TicketCustomerNotification objects</returns>
		public TicketCustomerNotificationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETCUSTOMERNOTIFICATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketCustomerNotification objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketCustomerNotification objects</returns>
		public TicketCustomerNotificationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETCUSTOMERNOTIFICATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketCustomerNotificationList _TicketCustomerNotificationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketCustomerNotificationList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketCustomerNotification objects by query String
        /// </summary>
        /// <returns>A list of TicketCustomerNotification objects</returns>
		public TicketCustomerNotificationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETCUSTOMERNOTIFICATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketCustomerNotification Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketCustomerNotification
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETCUSTOMERNOTIFICATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketCustomerNotification Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketCustomerNotification
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketCustomerNotificationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETCUSTOMERNOTIFICATIONROWCOUNT))
			{
				SqlDataReader reader;
				_TicketCustomerNotificationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketCustomerNotificationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketCustomerNotification object
        /// </summary>
        /// <param name="ticketCustomerNotificationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketCustomerNotificationBase ticketCustomerNotificationObject, SqlDataReader reader, int start)
		{
			
				ticketCustomerNotificationObject.Id = reader.GetInt32( start + 0 );			
				ticketCustomerNotificationObject.TicketStatus = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) ticketCustomerNotificationObject.Text = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) ticketCustomerNotificationObject.Email = reader.GetString( start + 3 );			
				ticketCustomerNotificationObject.CreatedBy = reader.GetGuid( start + 4 );			
				ticketCustomerNotificationObject.CreatedDate = reader.GetDateTime( start + 5 );			
				ticketCustomerNotificationObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) ticketCustomerNotificationObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) ticketCustomerNotificationObject.TicketType = reader.GetString( start + 8 );			
			FillBaseObject(ticketCustomerNotificationObject, reader, (start + 9));

			
			ticketCustomerNotificationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketCustomerNotification object
        /// </summary>
        /// <param name="ticketCustomerNotificationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketCustomerNotificationBase ticketCustomerNotificationObject, SqlDataReader reader)
		{
			FillObject(ticketCustomerNotificationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketCustomerNotification object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketCustomerNotification object</returns>
		private TicketCustomerNotification GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketCustomerNotification ticketCustomerNotificationObject= new TicketCustomerNotification();
					FillObject(ticketCustomerNotificationObject, reader);
					return ticketCustomerNotificationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketCustomerNotification objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketCustomerNotification objects</returns>
		private TicketCustomerNotificationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketCustomerNotification list
			TicketCustomerNotificationList list = new TicketCustomerNotificationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketCustomerNotification ticketCustomerNotificationObject = new TicketCustomerNotification();
					FillObject(ticketCustomerNotificationObject, reader);

					list.Add(ticketCustomerNotificationObject);
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
