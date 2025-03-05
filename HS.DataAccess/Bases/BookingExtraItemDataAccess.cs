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
	public partial class BookingExtraItemDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBOOKINGEXTRAITEM = "InsertBookingExtraItem";
		private const string UPDATEBOOKINGEXTRAITEM = "UpdateBookingExtraItem";
		private const string DELETEBOOKINGEXTRAITEM = "DeleteBookingExtraItem";
		private const string GETBOOKINGEXTRAITEMBYID = "GetBookingExtraItemById";
		private const string GETALLBOOKINGEXTRAITEM = "GetAllBookingExtraItem";
		private const string GETPAGEDBOOKINGEXTRAITEM = "GetPagedBookingExtraItem";
		private const string GETBOOKINGEXTRAITEMMAXIMUMID = "GetBookingExtraItemMaximumId";
		private const string GETBOOKINGEXTRAITEMROWCOUNT = "GetBookingExtraItemRowCount";	
		private const string GETBOOKINGEXTRAITEMBYQUERY = "GetBookingExtraItemByQuery";
		#endregion
		
		#region Constructors
		public BookingExtraItemDataAccess(ClientContext context) : base(context) { }
		public BookingExtraItemDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="bookingExtraItemObject"></param>
		private void AddCommonParams(SqlCommand cmd, BookingExtraItemBase bookingExtraItemObject)
		{	
			AddParameter(cmd, pInt32(BookingExtraItemBase.Property_BookingId, bookingExtraItemObject.BookingId));
			AddParameter(cmd, pGuid(BookingExtraItemBase.Property_EquipmentId, bookingExtraItemObject.EquipmentId));
			AddParameter(cmd, pNVarChar(BookingExtraItemBase.Property_EquipName, 500, bookingExtraItemObject.EquipName));
			AddParameter(cmd, pNVarChar(BookingExtraItemBase.Property_EquipDetail, bookingExtraItemObject.EquipDetail));
			AddParameter(cmd, pInt32(BookingExtraItemBase.Property_Quantity, bookingExtraItemObject.Quantity));
			AddParameter(cmd, pDouble(BookingExtraItemBase.Property_UnitPrice, bookingExtraItemObject.UnitPrice));
			AddParameter(cmd, pDouble(BookingExtraItemBase.Property_Discount, bookingExtraItemObject.Discount));
			AddParameter(cmd, pDouble(BookingExtraItemBase.Property_TotalPrice, bookingExtraItemObject.TotalPrice));
			AddParameter(cmd, pDateTime(BookingExtraItemBase.Property_CreatedDate, bookingExtraItemObject.CreatedDate));
			AddParameter(cmd, pNVarChar(BookingExtraItemBase.Property_CreatedBy, 50, bookingExtraItemObject.CreatedBy));
			AddParameter(cmd, pBool(BookingExtraItemBase.Property_Taxable, bookingExtraItemObject.Taxable));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BookingExtraItem
        /// </summary>
        /// <param name="bookingExtraItemObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BookingExtraItemBase bookingExtraItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBOOKINGEXTRAITEM);
	
				AddParameter(cmd, pInt32Out(BookingExtraItemBase.Property_Id));
				AddCommonParams(cmd, bookingExtraItemObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					bookingExtraItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					bookingExtraItemObject.Id = (Int32)GetOutParameter(cmd, BookingExtraItemBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(bookingExtraItemObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BookingExtraItem
        /// </summary>
        /// <param name="bookingExtraItemObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BookingExtraItemBase bookingExtraItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBOOKINGEXTRAITEM);
				
				AddParameter(cmd, pInt32(BookingExtraItemBase.Property_Id, bookingExtraItemObject.Id));
				AddCommonParams(cmd, bookingExtraItemObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					bookingExtraItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(bookingExtraItemObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BookingExtraItem
        /// </summary>
        /// <param name="Id">Id of the BookingExtraItem object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBOOKINGEXTRAITEM);	
				
				AddParameter(cmd, pInt32(BookingExtraItemBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BookingExtraItem), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BookingExtraItem object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BookingExtraItem object to retrieve</param>
        /// <returns>BookingExtraItem object, null if not found</returns>
		public BookingExtraItem Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGEXTRAITEMBYID))
			{
				AddParameter( cmd, pInt32(BookingExtraItemBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BookingExtraItem objects 
        /// </summary>
        /// <returns>A list of BookingExtraItem objects</returns>
		public BookingExtraItemList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBOOKINGEXTRAITEM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BookingExtraItem objects by PageRequest
        /// </summary>
        /// <returns>A list of BookingExtraItem objects</returns>
		public BookingExtraItemList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBOOKINGEXTRAITEM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BookingExtraItemList _BookingExtraItemList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BookingExtraItemList;
			}
		}
		
		/// <summary>
        /// Retrieves all BookingExtraItem objects by query String
        /// </summary>
        /// <returns>A list of BookingExtraItem objects</returns>
		public BookingExtraItemList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGEXTRAITEMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BookingExtraItem Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BookingExtraItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGEXTRAITEMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BookingExtraItem Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BookingExtraItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BookingExtraItemRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGEXTRAITEMROWCOUNT))
			{
				SqlDataReader reader;
				_BookingExtraItemRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BookingExtraItemRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BookingExtraItem object
        /// </summary>
        /// <param name="bookingExtraItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BookingExtraItemBase bookingExtraItemObject, SqlDataReader reader, int start)
		{
			
				bookingExtraItemObject.Id = reader.GetInt32( start + 0 );			
				bookingExtraItemObject.BookingId = reader.GetInt32( start + 1 );			
				bookingExtraItemObject.EquipmentId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) bookingExtraItemObject.EquipName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) bookingExtraItemObject.EquipDetail = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) bookingExtraItemObject.Quantity = reader.GetInt32( start + 5 );			
				if(!reader.IsDBNull(6)) bookingExtraItemObject.UnitPrice = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) bookingExtraItemObject.Discount = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) bookingExtraItemObject.TotalPrice = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) bookingExtraItemObject.CreatedDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) bookingExtraItemObject.CreatedBy = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) bookingExtraItemObject.Taxable = reader.GetBoolean( start + 11 );			
			FillBaseObject(bookingExtraItemObject, reader, (start + 12));

			
			bookingExtraItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BookingExtraItem object
        /// </summary>
        /// <param name="bookingExtraItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BookingExtraItemBase bookingExtraItemObject, SqlDataReader reader)
		{
			FillObject(bookingExtraItemObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BookingExtraItem object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BookingExtraItem object</returns>
		private BookingExtraItem GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BookingExtraItem bookingExtraItemObject= new BookingExtraItem();
					FillObject(bookingExtraItemObject, reader);
					return bookingExtraItemObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BookingExtraItem objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BookingExtraItem objects</returns>
		private BookingExtraItemList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BookingExtraItem list
			BookingExtraItemList list = new BookingExtraItemList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BookingExtraItem bookingExtraItemObject = new BookingExtraItem();
					FillObject(bookingExtraItemObject, reader);

					list.Add(bookingExtraItemObject);
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
