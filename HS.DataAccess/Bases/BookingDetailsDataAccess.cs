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
	public partial class BookingDetailsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBOOKINGDETAILS = "InsertBookingDetails";
		private const string UPDATEBOOKINGDETAILS = "UpdateBookingDetails";
		private const string DELETEBOOKINGDETAILS = "DeleteBookingDetails";
		private const string GETBOOKINGDETAILSBYID = "GetBookingDetailsById";
		private const string GETALLBOOKINGDETAILS = "GetAllBookingDetails";
		private const string GETPAGEDBOOKINGDETAILS = "GetPagedBookingDetails";
		private const string GETBOOKINGDETAILSMAXIMUMID = "GetBookingDetailsMaximumId";
		private const string GETBOOKINGDETAILSROWCOUNT = "GetBookingDetailsRowCount";	
		private const string GETBOOKINGDETAILSBYQUERY = "GetBookingDetailsByQuery";
		#endregion
		
		#region Constructors
		public BookingDetailsDataAccess(ClientContext context) : base(context) { }
		public BookingDetailsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="bookingDetailsObject"></param>
		private void AddCommonParams(SqlCommand cmd, BookingDetailsBase bookingDetailsObject)
		{	
			AddParameter(cmd, pGuid(BookingDetailsBase.Property_CompanyId, bookingDetailsObject.CompanyId));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_BookingId, 15, bookingDetailsObject.BookingId));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_RugType, 30, bookingDetailsObject.RugType));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_Length, bookingDetailsObject.Length));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_LengthInch, bookingDetailsObject.LengthInch));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_Width, bookingDetailsObject.Width));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_WidthInch, bookingDetailsObject.WidthInch));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_Radius, bookingDetailsObject.Radius));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_RadiusInch, bookingDetailsObject.RadiusInch));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_TotalSize, bookingDetailsObject.TotalSize));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_Package, 100, bookingDetailsObject.Package));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_Included, 100, bookingDetailsObject.Included));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_Extras, 100, bookingDetailsObject.Extras));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_UnitPrice, bookingDetailsObject.UnitPrice));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_DiscountType, 50, bookingDetailsObject.DiscountType));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_TaxType, 50, bookingDetailsObject.TaxType));
			AddParameter(cmd, pInt32(BookingDetailsBase.Property_Quantity, bookingDetailsObject.Quantity));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_Discount, bookingDetailsObject.Discount));
			AddParameter(cmd, pDouble(BookingDetailsBase.Property_TotalPrice, bookingDetailsObject.TotalPrice));
			AddParameter(cmd, pDateTime(BookingDetailsBase.Property_AddedDate, bookingDetailsObject.AddedDate));
			AddParameter(cmd, pGuid(BookingDetailsBase.Property_AddedBy, bookingDetailsObject.AddedBy));
			AddParameter(cmd, pNVarChar(BookingDetailsBase.Property_RugConditions, 1000, bookingDetailsObject.RugConditions));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BookingDetails
        /// </summary>
        /// <param name="bookingDetailsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BookingDetailsBase bookingDetailsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBOOKINGDETAILS);
	
				AddParameter(cmd, pInt32Out(BookingDetailsBase.Property_Id));
				AddCommonParams(cmd, bookingDetailsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					bookingDetailsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					bookingDetailsObject.Id = (Int32)GetOutParameter(cmd, BookingDetailsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(bookingDetailsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BookingDetails
        /// </summary>
        /// <param name="bookingDetailsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BookingDetailsBase bookingDetailsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBOOKINGDETAILS);
				
				AddParameter(cmd, pInt32(BookingDetailsBase.Property_Id, bookingDetailsObject.Id));
				AddCommonParams(cmd, bookingDetailsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					bookingDetailsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(bookingDetailsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BookingDetails
        /// </summary>
        /// <param name="Id">Id of the BookingDetails object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBOOKINGDETAILS);	
				
				AddParameter(cmd, pInt32(BookingDetailsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BookingDetails), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BookingDetails object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BookingDetails object to retrieve</param>
        /// <returns>BookingDetails object, null if not found</returns>
		public BookingDetails Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGDETAILSBYID))
			{
				AddParameter( cmd, pInt32(BookingDetailsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BookingDetails objects 
        /// </summary>
        /// <returns>A list of BookingDetails objects</returns>
		public BookingDetailsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBOOKINGDETAILS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BookingDetails objects by PageRequest
        /// </summary>
        /// <returns>A list of BookingDetails objects</returns>
		public BookingDetailsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBOOKINGDETAILS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BookingDetailsList _BookingDetailsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BookingDetailsList;
			}
		}
		
		/// <summary>
        /// Retrieves all BookingDetails objects by query String
        /// </summary>
        /// <returns>A list of BookingDetails objects</returns>
		public BookingDetailsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGDETAILSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BookingDetails Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BookingDetails
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGDETAILSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BookingDetails Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BookingDetails
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BookingDetailsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBOOKINGDETAILSROWCOUNT))
			{
				SqlDataReader reader;
				_BookingDetailsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BookingDetailsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BookingDetails object
        /// </summary>
        /// <param name="bookingDetailsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BookingDetailsBase bookingDetailsObject, SqlDataReader reader, int start)
		{
			
				bookingDetailsObject.Id = reader.GetInt32( start + 0 );			
				bookingDetailsObject.CompanyId = reader.GetGuid( start + 1 );			
				bookingDetailsObject.BookingId = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) bookingDetailsObject.RugType = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) bookingDetailsObject.Length = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) bookingDetailsObject.LengthInch = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) bookingDetailsObject.Width = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) bookingDetailsObject.WidthInch = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) bookingDetailsObject.Radius = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) bookingDetailsObject.RadiusInch = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) bookingDetailsObject.TotalSize = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) bookingDetailsObject.Package = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) bookingDetailsObject.Included = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) bookingDetailsObject.Extras = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) bookingDetailsObject.UnitPrice = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) bookingDetailsObject.DiscountType = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) bookingDetailsObject.TaxType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) bookingDetailsObject.Quantity = reader.GetInt32( start + 17 );			
				if(!reader.IsDBNull(18)) bookingDetailsObject.Discount = reader.GetDouble( start + 18 );			
				if(!reader.IsDBNull(19)) bookingDetailsObject.TotalPrice = reader.GetDouble( start + 19 );			
				bookingDetailsObject.AddedDate = reader.GetDateTime( start + 20 );			
				bookingDetailsObject.AddedBy = reader.GetGuid( start + 21 );			
				if(!reader.IsDBNull(22)) bookingDetailsObject.RugConditions = reader.GetString( start + 22 );			
			FillBaseObject(bookingDetailsObject, reader, (start + 23));

			
			bookingDetailsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BookingDetails object
        /// </summary>
        /// <param name="bookingDetailsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BookingDetailsBase bookingDetailsObject, SqlDataReader reader)
		{
			FillObject(bookingDetailsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BookingDetails object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BookingDetails object</returns>
		private BookingDetails GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BookingDetails bookingDetailsObject= new BookingDetails();
					FillObject(bookingDetailsObject, reader);
					return bookingDetailsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BookingDetails objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BookingDetails objects</returns>
		private BookingDetailsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BookingDetails list
			BookingDetailsList list = new BookingDetailsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BookingDetails bookingDetailsObject = new BookingDetails();
					FillObject(bookingDetailsObject, reader);

					list.Add(bookingDetailsObject);
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
