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
	public partial class TicketNotificationEmailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETNOTIFICATIONEMAIL = "InsertTicketNotificationEmail";
		private const string UPDATETICKETNOTIFICATIONEMAIL = "UpdateTicketNotificationEmail";
		private const string DELETETICKETNOTIFICATIONEMAIL = "DeleteTicketNotificationEmail";
		private const string GETTICKETNOTIFICATIONEMAILBYID = "GetTicketNotificationEmailById";
		private const string GETALLTICKETNOTIFICATIONEMAIL = "GetAllTicketNotificationEmail";
		private const string GETPAGEDTICKETNOTIFICATIONEMAIL = "GetPagedTicketNotificationEmail";
		private const string GETTICKETNOTIFICATIONEMAILMAXIMUMID = "GetTicketNotificationEmailMaximumId";
		private const string GETTICKETNOTIFICATIONEMAILROWCOUNT = "GetTicketNotificationEmailRowCount";	
		private const string GETTICKETNOTIFICATIONEMAILBYQUERY = "GetTicketNotificationEmailByQuery";
		#endregion
		
		#region Constructors
		public TicketNotificationEmailDataAccess(ClientContext context) : base(context) { }
		public TicketNotificationEmailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketNotificationEmailObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketNotificationEmailBase ticketNotificationEmailObject)
		{	
			AddParameter(cmd, pNVarChar(TicketNotificationEmailBase.Property_TicketStatus, 100, ticketNotificationEmailObject.TicketStatus));
			AddParameter(cmd, pNVarChar(TicketNotificationEmailBase.Property_Email, ticketNotificationEmailObject.Email));
			AddParameter(cmd, pGuid(TicketNotificationEmailBase.Property_CreatedBy, ticketNotificationEmailObject.CreatedBy));
			AddParameter(cmd, pDateTime(TicketNotificationEmailBase.Property_CreatedDate, ticketNotificationEmailObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketNotificationEmail
        /// </summary>
        /// <param name="ticketNotificationEmailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketNotificationEmailBase ticketNotificationEmailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETNOTIFICATIONEMAIL);
	
				AddParameter(cmd, pInt32Out(TicketNotificationEmailBase.Property_Id));
				AddCommonParams(cmd, ticketNotificationEmailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketNotificationEmailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketNotificationEmailObject.Id = (Int32)GetOutParameter(cmd, TicketNotificationEmailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketNotificationEmailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketNotificationEmail
        /// </summary>
        /// <param name="ticketNotificationEmailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketNotificationEmailBase ticketNotificationEmailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETNOTIFICATIONEMAIL);
				
				AddParameter(cmd, pInt32(TicketNotificationEmailBase.Property_Id, ticketNotificationEmailObject.Id));
				AddCommonParams(cmd, ticketNotificationEmailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketNotificationEmailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketNotificationEmailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketNotificationEmail
        /// </summary>
        /// <param name="Id">Id of the TicketNotificationEmail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETNOTIFICATIONEMAIL);	
				
				AddParameter(cmd, pInt32(TicketNotificationEmailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketNotificationEmail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketNotificationEmail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketNotificationEmail object to retrieve</param>
        /// <returns>TicketNotificationEmail object, null if not found</returns>
		public TicketNotificationEmail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETNOTIFICATIONEMAILBYID))
			{
				AddParameter( cmd, pInt32(TicketNotificationEmailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketNotificationEmail objects 
        /// </summary>
        /// <returns>A list of TicketNotificationEmail objects</returns>
		public TicketNotificationEmailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETNOTIFICATIONEMAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketNotificationEmail objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketNotificationEmail objects</returns>
		public TicketNotificationEmailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETNOTIFICATIONEMAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketNotificationEmailList _TicketNotificationEmailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketNotificationEmailList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketNotificationEmail objects by query String
        /// </summary>
        /// <returns>A list of TicketNotificationEmail objects</returns>
		public TicketNotificationEmailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETNOTIFICATIONEMAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketNotificationEmail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketNotificationEmail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETNOTIFICATIONEMAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketNotificationEmail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketNotificationEmail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketNotificationEmailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETNOTIFICATIONEMAILROWCOUNT))
			{
				SqlDataReader reader;
				_TicketNotificationEmailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketNotificationEmailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketNotificationEmail object
        /// </summary>
        /// <param name="ticketNotificationEmailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketNotificationEmailBase ticketNotificationEmailObject, SqlDataReader reader, int start)
		{
			
				ticketNotificationEmailObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) ticketNotificationEmailObject.TicketStatus = reader.GetString( start + 1 );			
				if(!reader.IsDBNull(2)) ticketNotificationEmailObject.Email = reader.GetString( start + 2 );			
				ticketNotificationEmailObject.CreatedBy = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) ticketNotificationEmailObject.CreatedDate = reader.GetDateTime( start + 4 );			
			FillBaseObject(ticketNotificationEmailObject, reader, (start + 5));

			
			ticketNotificationEmailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketNotificationEmail object
        /// </summary>
        /// <param name="ticketNotificationEmailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketNotificationEmailBase ticketNotificationEmailObject, SqlDataReader reader)
		{
			FillObject(ticketNotificationEmailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketNotificationEmail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketNotificationEmail object</returns>
		private TicketNotificationEmail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketNotificationEmail ticketNotificationEmailObject= new TicketNotificationEmail();
					FillObject(ticketNotificationEmailObject, reader);
					return ticketNotificationEmailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketNotificationEmail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketNotificationEmail objects</returns>
		private TicketNotificationEmailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketNotificationEmail list
			TicketNotificationEmailList list = new TicketNotificationEmailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketNotificationEmail ticketNotificationEmailObject = new TicketNotificationEmail();
					FillObject(ticketNotificationEmailObject, reader);

					list.Add(ticketNotificationEmailObject);
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
