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
	public partial class BookingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBOOKING = "InsertBooking";
		private const string UPDATEBOOKING = "UpdateBooking";
		private const string DELETEBOOKING = "DeleteBooking";
		private const string GETBOOKINGBYID = "GetBookingById";
		private const string GETALLBOOKING = "GetAllBooking";
		private const string GETPAGEDBOOKING = "GetPagedBooking";
		private const string GETBOOKINGMAXIMUMID = "GetBookingMaximumId";
		private const string GETBOOKINGROWCOUNT = "GetBookingRowCount";	
		private const string GETBOOKINGBYQUERY = "GetBookingByQuery";
		#endregion
		
		#region Constructors
		public BookingDataAccess(ClientContext context) : base(context) { }
		public BookingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="bookingObject"></param>
		private void AddCommonParams(SqlCommand cmd, BookingBase bookingObject)
		{	
			AddParameter(cmd, pNVarChar(BookingBase.Property_BookingId, 15, bookingObject.BookingId));
			AddParameter(cmd, pGuid(BookingBase.Property_CustomerId, bookingObject.CustomerId));
			AddParameter(cmd, pGuid(BookingBase.Property_CompanyId, bookingObject.CompanyId));
			AddParameter(cmd, pDouble(BookingBase.Property_Amount, bookingObject.Amount));
			AddParameter(cmd, pNVarChar(BookingBase.Property_TaxType, 50, bookingObject.TaxType));
			AddParameter(cmd, pDouble(BookingBase.Property_Tax, bookingObject.Tax));
			AddParameter(cmd, pNVarChar(BookingBase.Property_DiscountType, 50, bookingObject.DiscountType));
			AddParameter(cmd, pNVarChar(BookingBase.Property_DiscountCode, 50, bookingObject.DiscountCode));
			AddParameter(cmd, pDouble(BookingBase.Property_DiscountAmount, bookingObject.DiscountAmount));
			AddParameter(cmd, pDouble(BookingBase.Property_TotalAmount, bookingObject.TotalAmount));
			AddParameter(cmd, pNVarChar(BookingBase.Property_Status, 50, bookingObject.Status));
			AddParameter(cmd, pNVarChar(BookingBase.Property_Description, bookingObject.Description));
			AddParameter(cmd, pNVarChar(BookingBase.Property_Message, bookingObject.Message));
			AddParameter(cmd, pNVarChar(BookingBase.Property_BillingAddress, bookingObject.BillingAddress));
			AddParameter(cmd, pDateTime(BookingBase.Property_PickUpDate, bookingObject.PickUpDate));
			AddParameter(cmd, pNVarChar(BookingBase.Property_Signature, 250, bookingObject.Signature));
			AddParameter(cmd, pNVarChar(BookingBase.Property_CancelReason, bookingObject.CancelReason));
			AddParameter(cmd, pDateTime(BookingBase.Property_CreatedDate, bookingObject.CreatedDate));
			AddParameter(cmd, pGuid(BookingBase.Property_CreatedBy, bookingObject.CreatedBy));
			AddParameter(cmd, pDateTime(BookingBase.Property_LastUpdatedDate, bookingObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(BookingBase.Property_LastUpdatedBy, bookingObject.LastUpdatedBy));
			AddParameter(cmd, pNVarChar(BookingBase.Property_EmailAddress, 100, bookingObject.EmailAddress));
			AddParameter(cmd, pNVarChar(BookingBase.Property_PickUpLocation, bookingObject.PickUpLocation));
			AddParameter(cmd, pNVarChar(BookingBase.Property_DropOffLocation, bookingObject.DropOffLocation));
			AddParameter(cmd, pDateTime(BookingBase.Property_DropOffDate, bookingObject.DropOffDate));
			AddParameter(cmd, pNVarChar(BookingBase.Property_BookingSource, 50, bookingObject.BookingSource));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Booking
        /// </summary>
        /// <param name="bookingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BookingBase bookingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBOOKING);
	
				AddParameter(cmd, pInt32Out(BookingBase.Property_Id));
				AddCommonParams(cmd, bookingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					bookingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					bookingObject.Id = (Int32)GetOutParameter(cmd, BookingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(bookingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Booking
        /// </summary>
        /// <param name="bookingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BookingBase bookingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBOOKING);
				
				AddParameter(cmd, pInt32(BookingBase.Property_Id, bookingObject.Id));
				AddCommonParams(cmd, bookingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					bookingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(bookingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Booking
        /// </summary>
        /// <param name="Id">Id of the Booking object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBOOKING);	
				
				AddParameter(cmd, pInt32(BookingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Booking), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Booking object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Booking object to retrieve</param>
        /// <returns>Booking object, null if not found</returns>
		public Booking Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGBYID))
			{
				AddParameter( cmd, pInt32(BookingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Booking objects 
        /// </summary>
        /// <returns>A list of Booking objects</returns>
		public BookingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBOOKING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Booking objects by PageRequest
        /// </summary>
        /// <returns>A list of Booking objects</returns>
		public BookingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBOOKING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BookingList _BookingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BookingList;
			}
		}
		
		/// <summary>
        /// Retrieves all Booking objects by query String
        /// </summary>
        /// <returns>A list of Booking objects</returns>
		public BookingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Booking Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Booking
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Booking Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Booking
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BookingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGROWCOUNT))
			{
				SqlDataReader reader;
				_BookingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BookingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Booking object
        /// </summary>
        /// <param name="bookingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BookingBase bookingObject, SqlDataReader reader, int start)
		{
			
				bookingObject.Id = reader.GetInt32( start + 0 );			
				bookingObject.BookingId = reader.GetString( start + 1 );			
				bookingObject.CustomerId = reader.GetGuid( start + 2 );			
				bookingObject.CompanyId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) bookingObject.Amount = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) bookingObject.TaxType = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) bookingObject.Tax = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) bookingObject.DiscountType = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) bookingObject.DiscountCode = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) bookingObject.DiscountAmount = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) bookingObject.TotalAmount = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) bookingObject.Status = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) bookingObject.Description = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) bookingObject.Message = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) bookingObject.BillingAddress = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) bookingObject.PickUpDate = reader.GetDateTime( start + 15 );			
				if(!reader.IsDBNull(16)) bookingObject.Signature = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) bookingObject.CancelReason = reader.GetString( start + 17 );			
				bookingObject.CreatedDate = reader.GetDateTime( start + 18 );			
				bookingObject.CreatedBy = reader.GetGuid( start + 19 );			
				bookingObject.LastUpdatedDate = reader.GetDateTime( start + 20 );			
				bookingObject.LastUpdatedBy = reader.GetGuid( start + 21 );			
				if(!reader.IsDBNull(22)) bookingObject.EmailAddress = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) bookingObject.PickUpLocation = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) bookingObject.DropOffLocation = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) bookingObject.DropOffDate = reader.GetDateTime( start + 25 );			
				if(!reader.IsDBNull(26)) bookingObject.BookingSource = reader.GetString( start + 26 );			
			FillBaseObject(bookingObject, reader, (start + 27));

			
			bookingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Booking object
        /// </summary>
        /// <param name="bookingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BookingBase bookingObject, SqlDataReader reader)
		{
			FillObject(bookingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Booking object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Booking object</returns>
		private Booking GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Booking bookingObject= new Booking();
					FillObject(bookingObject, reader);
					return bookingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Booking objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Booking objects</returns>
		private BookingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Booking list
			BookingList list = new BookingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Booking bookingObject = new Booking();
					FillObject(bookingObject, reader);

					list.Add(bookingObject);
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
