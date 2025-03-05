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
	public partial class TicketBookingExtraItemDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTICKETBOOKINGEXTRAITEM = "InsertTicketBookingExtraItem";
		private const string UPDATETICKETBOOKINGEXTRAITEM = "UpdateTicketBookingExtraItem";
		private const string DELETETICKETBOOKINGEXTRAITEM = "DeleteTicketBookingExtraItem";
		private const string GETTICKETBOOKINGEXTRAITEMBYID = "GetTicketBookingExtraItemById";
		private const string GETALLTICKETBOOKINGEXTRAITEM = "GetAllTicketBookingExtraItem";
		private const string GETPAGEDTICKETBOOKINGEXTRAITEM = "GetPagedTicketBookingExtraItem";
		private const string GETTICKETBOOKINGEXTRAITEMMAXIMUMID = "GetTicketBookingExtraItemMaximumId";
		private const string GETTICKETBOOKINGEXTRAITEMROWCOUNT = "GetTicketBookingExtraItemRowCount";	
		private const string GETTICKETBOOKINGEXTRAITEMBYQUERY = "GetTicketBookingExtraItemByQuery";
		#endregion
		
		#region Constructors
		public TicketBookingExtraItemDataAccess(ClientContext context) : base(context) { }
		public TicketBookingExtraItemDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="ticketBookingExtraItemObject"></param>
		private void AddCommonParams(SqlCommand cmd, TicketBookingExtraItemBase ticketBookingExtraItemObject)
		{	
			AddParameter(cmd, pInt32(TicketBookingExtraItemBase.Property_BookingId, ticketBookingExtraItemObject.BookingId));
			AddParameter(cmd, pGuid(TicketBookingExtraItemBase.Property_EquipmentId, ticketBookingExtraItemObject.EquipmentId));
			AddParameter(cmd, pNVarChar(TicketBookingExtraItemBase.Property_EquipName, 500, ticketBookingExtraItemObject.EquipName));
			AddParameter(cmd, pNVarChar(TicketBookingExtraItemBase.Property_EquipDetail, ticketBookingExtraItemObject.EquipDetail));
			AddParameter(cmd, pInt32(TicketBookingExtraItemBase.Property_Quantity, ticketBookingExtraItemObject.Quantity));
			AddParameter(cmd, pDouble(TicketBookingExtraItemBase.Property_UnitPrice, ticketBookingExtraItemObject.UnitPrice));
			AddParameter(cmd, pDouble(TicketBookingExtraItemBase.Property_Discount, ticketBookingExtraItemObject.Discount));
			AddParameter(cmd, pDouble(TicketBookingExtraItemBase.Property_TotalPrice, ticketBookingExtraItemObject.TotalPrice));
			AddParameter(cmd, pDateTime(TicketBookingExtraItemBase.Property_CreatedDate, ticketBookingExtraItemObject.CreatedDate));
			AddParameter(cmd, pNVarChar(TicketBookingExtraItemBase.Property_CreatedBy, 50, ticketBookingExtraItemObject.CreatedBy));
			AddParameter(cmd, pBool(TicketBookingExtraItemBase.Property_Taxable, ticketBookingExtraItemObject.Taxable));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TicketBookingExtraItem
        /// </summary>
        /// <param name="ticketBookingExtraItemObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TicketBookingExtraItemBase ticketBookingExtraItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTICKETBOOKINGEXTRAITEM);
	
				AddParameter(cmd, pInt32Out(TicketBookingExtraItemBase.Property_Id));
				AddCommonParams(cmd, ticketBookingExtraItemObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					ticketBookingExtraItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					ticketBookingExtraItemObject.Id = (Int32)GetOutParameter(cmd, TicketBookingExtraItemBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(ticketBookingExtraItemObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TicketBookingExtraItem
        /// </summary>
        /// <param name="ticketBookingExtraItemObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TicketBookingExtraItemBase ticketBookingExtraItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETICKETBOOKINGEXTRAITEM);
				
				AddParameter(cmd, pInt32(TicketBookingExtraItemBase.Property_Id, ticketBookingExtraItemObject.Id));
				AddCommonParams(cmd, ticketBookingExtraItemObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					ticketBookingExtraItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(ticketBookingExtraItemObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TicketBookingExtraItem
        /// </summary>
        /// <param name="Id">Id of the TicketBookingExtraItem object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETICKETBOOKINGEXTRAITEM);	
				
				AddParameter(cmd, pInt32(TicketBookingExtraItemBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TicketBookingExtraItem), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TicketBookingExtraItem object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TicketBookingExtraItem object to retrieve</param>
        /// <returns>TicketBookingExtraItem object, null if not found</returns>
		public TicketBookingExtraItem Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGEXTRAITEMBYID))
			{
				AddParameter( cmd, pInt32(TicketBookingExtraItemBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TicketBookingExtraItem objects 
        /// </summary>
        /// <returns>A list of TicketBookingExtraItem objects</returns>
		public TicketBookingExtraItemList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTICKETBOOKINGEXTRAITEM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TicketBookingExtraItem objects by PageRequest
        /// </summary>
        /// <returns>A list of TicketBookingExtraItem objects</returns>
		public TicketBookingExtraItemList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTICKETBOOKINGEXTRAITEM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TicketBookingExtraItemList _TicketBookingExtraItemList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TicketBookingExtraItemList;
			}
		}
		
		/// <summary>
        /// Retrieves all TicketBookingExtraItem objects by query String
        /// </summary>
        /// <returns>A list of TicketBookingExtraItem objects</returns>
		public TicketBookingExtraItemList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGEXTRAITEMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TicketBookingExtraItem Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TicketBookingExtraItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGEXTRAITEMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TicketBookingExtraItem Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TicketBookingExtraItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TicketBookingExtraItemRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTICKETBOOKINGEXTRAITEMROWCOUNT))
			{
				SqlDataReader reader;
				_TicketBookingExtraItemRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TicketBookingExtraItemRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TicketBookingExtraItem object
        /// </summary>
        /// <param name="ticketBookingExtraItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TicketBookingExtraItemBase ticketBookingExtraItemObject, SqlDataReader reader, int start)
		{
			
				ticketBookingExtraItemObject.Id = reader.GetInt32( start + 0 );			
				ticketBookingExtraItemObject.BookingId = reader.GetInt32( start + 1 );			
				ticketBookingExtraItemObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) ticketBookingExtraItemObject.EquipName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) ticketBookingExtraItemObject.EquipDetail = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) ticketBookingExtraItemObject.Quantity = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) ticketBookingExtraItemObject.UnitPrice = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) ticketBookingExtraItemObject.Discount = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) ticketBookingExtraItemObject.TotalPrice = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) ticketBookingExtraItemObject.CreatedDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) ticketBookingExtraItemObject.CreatedBy = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) ticketBookingExtraItemObject.Taxable = reader.GetBoolean( start + 11 );			
			FillBaseObject(ticketBookingExtraItemObject, reader, (start + 12));

			
			ticketBookingExtraItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TicketBookingExtraItem object
        /// </summary>
        /// <param name="ticketBookingExtraItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TicketBookingExtraItemBase ticketBookingExtraItemObject, SqlDataReader reader)
		{
			FillObject(ticketBookingExtraItemObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TicketBookingExtraItem object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TicketBookingExtraItem object</returns>
		private TicketBookingExtraItem GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TicketBookingExtraItem ticketBookingExtraItemObject= new TicketBookingExtraItem();
					FillObject(ticketBookingExtraItemObject, reader);
					return ticketBookingExtraItemObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TicketBookingExtraItem objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TicketBookingExtraItem objects</returns>
		private TicketBookingExtraItemList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TicketBookingExtraItem list
			TicketBookingExtraItemList list = new TicketBookingExtraItemList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TicketBookingExtraItem ticketBookingExtraItemObject = new TicketBookingExtraItem();
					FillObject(ticketBookingExtraItemObject, reader);

					list.Add(ticketBookingExtraItemObject);
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
