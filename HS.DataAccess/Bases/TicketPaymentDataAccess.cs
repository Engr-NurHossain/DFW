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
	public partial class TicketPaymentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETPAYMENT = "InsertTicketPayment";
		private const string UPDATETICKETPAYMENT = "UpdateTicketPayment";
		private const string DELETETICKETPAYMENT = "DeleteTicketPayment";
		private const string GETTICKETPAYMENTBYID = "GetTicketPaymentById";
		private const string GETALLTICKETPAYMENT = "GetAllTicketPayment";
		private const string GETPAGEDTICKETPAYMENT = "GetPagedTicketPayment";
		private const string GETTICKETPAYMENTMAXIMUMID = "GetTicketPaymentMaximumId";
		private const string GETTICKETPAYMENTROWCOUNT = "GetTicketPaymentRowCount";	
		private const string GETTICKETPAYMENTBYQUERY = "GetTicketPaymentByQuery";
		#endregion
		
		#region Constructors
		public TicketPaymentDataAccess(ClientContext context) : base(context) { }
		public TicketPaymentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketPaymentObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketPaymentBase ticketPaymentObject)
		{	
			AddParameter(cmd, pGuid(TicketPaymentBase.Property_CustomerId, ticketPaymentObject.CustomerId));
			AddParameter(cmd, pGuid(TicketPaymentBase.Property_TicketId, ticketPaymentObject.TicketId));
			AddParameter(cmd, pBool(TicketPaymentBase.Property_IsPaid, ticketPaymentObject.IsPaid));
			AddParameter(cmd, pNVarChar(TicketPaymentBase.Property_PaymentMethod, 100, ticketPaymentObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(TicketPaymentBase.Property_ConfirmationNo, 250, ticketPaymentObject.ConfirmationNo));
			AddParameter(cmd, pDateTime(TicketPaymentBase.Property_CreatedDate, ticketPaymentObject.CreatedDate));
			AddParameter(cmd, pGuid(TicketPaymentBase.Property_CreatedBy, ticketPaymentObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketPayment
        /// </summary>
        /// <param name="ticketPaymentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketPaymentBase ticketPaymentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETPAYMENT);
	
				AddParameter(cmd, pInt32Out(TicketPaymentBase.Property_Id));
				AddCommonParams(cmd, ticketPaymentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketPaymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketPaymentObject.Id = (Int32)GetOutParameter(cmd, TicketPaymentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketPaymentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketPayment
        /// </summary>
        /// <param name="ticketPaymentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketPaymentBase ticketPaymentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETPAYMENT);
				
				AddParameter(cmd, pInt32(TicketPaymentBase.Property_Id, ticketPaymentObject.Id));
				AddCommonParams(cmd, ticketPaymentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketPaymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketPaymentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketPayment
        /// </summary>
        /// <param name="Id">Id of the TicketPayment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETPAYMENT);	
				
				AddParameter(cmd, pInt32(TicketPaymentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketPayment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketPayment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketPayment object to retrieve</param>
        /// <returns>TicketPayment object, null if not found</returns>
		public TicketPayment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETPAYMENTBYID))
			{
				AddParameter( cmd, pInt32(TicketPaymentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketPayment objects 
        /// </summary>
        /// <returns>A list of TicketPayment objects</returns>
		public TicketPaymentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETPAYMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketPayment objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketPayment objects</returns>
		public TicketPaymentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETPAYMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketPaymentList _TicketPaymentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketPaymentList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketPayment objects by query String
        /// </summary>
        /// <returns>A list of TicketPayment objects</returns>
		public TicketPaymentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETPAYMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketPayment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketPayment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETPAYMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketPayment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketPayment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketPaymentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETPAYMENTROWCOUNT))
			{
				SqlDataReader reader;
				_TicketPaymentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketPaymentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketPayment object
        /// </summary>
        /// <param name="ticketPaymentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketPaymentBase ticketPaymentObject, SqlDataReader reader, int start)
		{
			
				ticketPaymentObject.Id = reader.GetInt32( start + 0 );			
				ticketPaymentObject.CustomerId = reader.GetGuid( start + 1 );			
				ticketPaymentObject.TicketId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) ticketPaymentObject.IsPaid = reader.GetBoolean( start + 3 );			
				if(!reader.IsDBNull(4)) ticketPaymentObject.PaymentMethod = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketPaymentObject.ConfirmationNo = reader.GetString( start + 5 );			
				ticketPaymentObject.CreatedDate = reader.GetDateTime( start + 6 );			
				ticketPaymentObject.CreatedBy = reader.GetGuid( start + 7 );			
			FillBaseObject(ticketPaymentObject, reader, (start + 8));

			
			ticketPaymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketPayment object
        /// </summary>
        /// <param name="ticketPaymentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketPaymentBase ticketPaymentObject, SqlDataReader reader)
		{
			FillObject(ticketPaymentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketPayment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketPayment object</returns>
		private TicketPayment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketPayment ticketPaymentObject= new TicketPayment();
					FillObject(ticketPaymentObject, reader);
					return ticketPaymentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketPayment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketPayment objects</returns>
		private TicketPaymentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketPayment list
			TicketPaymentList list = new TicketPaymentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketPayment ticketPaymentObject = new TicketPayment();
					FillObject(ticketPaymentObject, reader);

					list.Add(ticketPaymentObject);
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
